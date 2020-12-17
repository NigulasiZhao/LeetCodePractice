using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Newtonsoft.Json;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;
using System.Data;
using System.Text;
using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Microsoft.AspNetCore.Http;
using GISWaterSupplyAndSewageServer.CommonTools;
using System.Text.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Internalexternal
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class Ms_FileStoreController : Controller
    {
        private readonly IMs_FileStoreDAL _ms_FileStoreDAL;
        private readonly IMs_logManagementDAL _ms_logManagement;
        public Ms_FileStoreController(IMs_FileStoreDAL ms_FileStoreDAL, IMs_logManagementDAL ms_logManagement)
        {
            _ms_FileStoreDAL = ms_FileStoreDAL;
            _ms_logManagement = ms_logManagement;
        }
        /// <summary>
        /// 获取文件上传记录信息
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition|startTime|endTime",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "uploadetime", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ms_FileStoreDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }


        /// <summary>
        /// 上传文件接口(包含上传文件）
        /// </summary>
        /// <param name="formData">上传文件</param>
        /// <param name="uploaderId">上传人id</param>
        /// <param name="uploaderName">上传人名</param>
        /// <param name="describe">文件描述</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public MessageEntity Post([FromForm]IFormCollection formData,string uploaderId, string uploaderName, string describe="")
        {
            if (string.IsNullOrEmpty(uploaderId) || string.IsNullOrEmpty(uploaderName) )
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            //文件实体赋值
            Ms_FileStore value = new Ms_FileStore();
            value.uploaderId = uploaderId;
            value.uploaderName = uploaderName;
            value.FileDescribe = describe;
            //var files = Request.Form.Files;
            IFormFileCollection files = formData.Files;
            Random random = new Random();
            try
            {
                if (files.Count > 0)
                {


                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        if (file.Length > 0)
                        {
                            if (file.Length > Convert.ToInt32(Appsettings.app(new string[] { "FileSize" })))
                            {
                                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "文件大小超出限制");
                            }
                            DateTime dateNow = DateTime.Now;
                            //全路径 
                            string FullFullName = file.FileName;

                            string relativeDir = "upload/MsFileStore/" + dateNow.Year + "/" + dateNow.Month + "/" + dateNow.Day;
                            string imageName = string.Format("{0:yyyyMMddHHmmssfff}", dateNow);
                            string AllPath = string.Format("{0}", relativeDir);

                            string relativePath = string.Format("{0}/{1}", relativeDir, imageName + FullFullName);
                            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath)))
                            {
                                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath));
                            }
                            using (var stream = new FileStream(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath), imageName + FullFullName), FileMode.Create))
                            {
                                file.OpenReadStream().CopyTo(stream);
                            }
                            //file.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
                            #region 根据哈希值验证文件是否上传重复
                            string hdKey = string.Empty;
                            try
                            {
                                hdKey = Sha1Helper.GetHash(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
                            }
                            catch (Exception e)
                            {
                                return MessageEntityTool.GetMessage(ErrorType.FieldError, e.Message);
                            }
                            //判断是否上传过重复文件
                            MessageEntity messresult = _ms_FileStoreDAL.IsExistFileHash(hdKey);
                            List<Ms_FileStore> ptslist = (List<Ms_FileStore>)messresult.Data.Result;
                            if (ptslist.Count > 0)
                            {
                                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "已上传相同文件内容，不允许重复上传！");
                            }
                            #endregion
                            DataTable dtErr = new DataTable();
                            DataColumn dc1 = new DataColumn("type", Type.GetType("System.String"));
                            DataColumn dc2 = new DataColumn("Linenumber", Type.GetType("System.String"));
                            DataColumn dc3 = new DataColumn("Remark", Type.GetType("System.String"));
                            dtErr.Columns.Add(dc1);
                            dtErr.Columns.Add(dc2);
                            dtErr.Columns.Add(dc3);

                            DataTable dtPoint = ExcelUtility.ExcelToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath), true, 0);
                            DataTable dtLine = ExcelUtility.ExcelToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath), true, 1);
                            if(dtPoint==null || dtLine==null)
                            {
                                   //首先根据路径删除上传文件
                                ExcelUtility.DeleteFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
                                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "excel获取文件数据失败！");

                            }
                            #region  循环点位和线表格，验证表格
                            #region 1.首先验证点位表格是否缺少设备编号、纵坐标、横坐标、类型、设备类型列
                            if (!dtPoint.Columns.Contains("设备编号"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "点表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "点表缺少设备编号";
                                dtErr.Rows.Add(dr);
                            }
                            if (!dtPoint.Columns.Contains("纵坐标"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "点表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "点表缺少纵坐标";
                                dtErr.Rows.Add(dr);
                            }
                            if (!dtPoint.Columns.Contains("横坐标"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "点表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "点表缺少横坐标";
                                dtErr.Rows.Add(dr);
                            }
                            if (!dtPoint.Columns.Contains("类型"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "点表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "点表缺少类型";
                                dtErr.Rows.Add(dr);
                            }
                            if (!dtPoint.Columns.Contains("设备类型"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "点表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "点表缺少设备类型";
                                dtErr.Rows.Add(dr);
                            }
                            #endregion
                            #region 1.2验证点表具体列内容是否为空
                            int ii = 1;
                            foreach (DataRow item in dtPoint.Rows)
                            {
                                ii++;
                                if (dtPoint.Columns.Contains("设备编号") && item["设备编号"].ToString() == "")
                                {
                                    DataRow dr = dtErr.NewRow();
                                    dr["type"] = "点表";
                                    dr["Linenumber"] = "第" + ii + "行";
                                    dr["Remark"] = "点表第" + ii + "行" + "缺少设备编号";
                                    dtErr.Rows.Add(dr);
                                }
                                if (dtPoint.Columns.Contains("纵坐标"))
                                {
                                    if (item["纵坐标"].ToString() == "")
                                    {
                                        DataRow dr = dtErr.NewRow();
                                        dr["type"] = "点表";
                                        dr["Linenumber"] = "第" + ii + "行";
                                        dr["Remark"] = "点表第" + ii + "行" + "缺少纵坐标";
                                        dtErr.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        bool isDouble = Regex.IsMatch(item["纵坐标"].ToString(), @"^[+-]?\d*[.]?\d*$"); //这个方法会返回一个布尔值，如果string字符串可以转换为double，则返回True，反之为False。
                                                                                                                     //如果不是有效数值
                                        if (!isDouble)
                                        {
                                            DataRow dr = dtErr.NewRow();
                                            dr["type"] = "点表";
                                            dr["Linenumber"] = "第" + ii + "行";
                                            dr["Remark"] = "点表第" + ii + "行" + "纵坐标不是有效的数值";
                                            dtErr.Rows.Add(dr);
                                        }
                                    }
                                }
                                if (dtPoint.Columns.Contains("横坐标"))
                                {
                                    if (item["横坐标"].ToString() == "")
                                    {
                                        DataRow dr = dtErr.NewRow();
                                        dr["type"] = "点表";
                                        dr["Linenumber"] = "第" + ii + "行";
                                        dr["Remark"] = "点表第" + ii + "行" + "缺少横坐标";
                                        dtErr.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        bool isDouble = Regex.IsMatch(item["横坐标"].ToString(), @"^[+-]?\d*[.]?\d*$"); //这个方法会返回一个布尔值，如果string字符串可以转换为double，则返回True，反之为False。
                                                                                                                     //如果不是有效数值
                                        if (!isDouble)
                                        {
                                            DataRow dr = dtErr.NewRow();
                                            dr["type"] = "点表";
                                            dr["Linenumber"] = "第" + ii + "行";
                                            dr["Remark"] = "点表第" + ii + "行" + "横坐标不是有效的数值";
                                            dtErr.Rows.Add(dr);
                                        }
                                    }
                                }
                                if (dtPoint.Columns.Contains("类型") && item["类型"].ToString() == "")
                                {
                                    DataRow dr = dtErr.NewRow();
                                    dr["type"] = "点表";
                                    dr["Linenumber"] = "第" + ii + "行";
                                    dr["Remark"] = "点表第" + ii + "行" + "缺少类型";
                                    dtErr.Rows.Add(dr);
                                }

                            }
                            #endregion
                            #region 2.1首先验证线标是否缺少管线编号、类型、起点物探点、终点物探点
                            //判断线表是否存在列信息
                            if (!dtLine.Columns.Contains("管线编号"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "线表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "线表缺少管线编号";
                                dtErr.Rows.Add(dr);
                            }
                            if (!dtLine.Columns.Contains("类型"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "线表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "线表缺少类型";
                                dtErr.Rows.Add(dr);
                            }
                            if (!dtLine.Columns.Contains("起点物探点号"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "线表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "线表缺少起点物探点号";
                                dtErr.Rows.Add(dr);
                            }
                            if (!dtLine.Columns.Contains("终点物探点号"))
                            {
                                DataRow dr = dtErr.NewRow();
                                dr["type"] = "线表";
                                dr["Linenumber"] = "";
                                dr["Remark"] = "线表缺少终点物探点号";
                                dtErr.Rows.Add(dr);
                            }
                            #endregion
                            #region 2.2验证线表具体列内容是否为空
                            int jj = 1;
                            foreach (DataRow item in dtLine.Rows)
                            {
                                jj++;
                                if (dtLine.Columns.Contains("起点物探点号") && item["起点物探点号"].ToString() == "")
                                {
                                    DataRow dr = dtErr.NewRow();
                                    dr["type"] = "线表";
                                    dr["Linenumber"] = "第" + jj + "行";
                                    dr["Remark"] = "线表第" + jj + "行" + "缺少起点物探点";
                                    dtErr.Rows.Add(dr);
                                }
                                if (dtLine.Columns.Contains("终点物探点号") && item["终点物探点号"].ToString() == "")
                                {
                                    DataRow dr = dtErr.NewRow();
                                    dr["type"] = "线表";
                                    dr["Linenumber"] = "第" + jj + "行";
                                    dr["Remark"] = "线表第" + jj + "行" + "缺少终点物探点号";
                                    dtErr.Rows.Add(dr);
                                }
                                //判断起点和终点物探编号都在点位表中存在
                                bool isExistS = true, isExistE = true;
                                if (dtLine.Columns.Contains("起点物探点号") && item["起点物探点号"].ToString() != "")
                                {
                                    isExistS = ExcelUtility.IsColumnIncludeData(dtPoint, "设备编号", item["起点物探点号"].ToString());
                                }
                                if (dtLine.Columns.Contains("终点物探点号") && item["终点物探点号"].ToString() != "")
                                {
                                    isExistE = ExcelUtility.IsColumnIncludeData(dtPoint, "设备编号", item["终点物探点号"].ToString());
                                }

                                if (!isExistS)
                                {
                                    DataRow dr = dtErr.NewRow();
                                    dr["type"] = "线表";
                                    dr["Linenumber"] = "第" + jj + "行";
                                    dr["Remark"] = "线表第" + jj + "行" + "起点物探点号不存在";
                                    dtErr.Rows.Add(dr);
                                }
                                if (!isExistE)
                                {
                                    DataRow dr = dtErr.NewRow();
                                    dr["type"] = "线表";
                                    dr["Linenumber"] = "第" + jj + "行";
                                    dr["Remark"] = "线表第" + jj + "行" + "终点物探点号不存在";
                                    dtErr.Rows.Add(dr);
                                }

                            }
                            #endregion 2.2结束
                            #endregion
                            if (dtErr.Rows.Count > 0)
                            {
                                //首先根据路径删除上传文件
                                ExcelUtility.DeleteFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
                                //返回错误信息json
                                //var json = new JavaScriptSerializer().Serialize(ExcelUtility.DataTableToList(dtErr));
                                var json = JsonConvert.SerializeObject(ExcelUtility.DataTableToList(dtErr));
                                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", json);
                            }
                            else
                            {
                                //添加文件上传信息
                                value.uploadpath = relativePath;//文件上传路径
                                value.FileKey = hdKey;
                                _ms_FileStoreDAL.Add(value);
                                //文件上传成功后添加日志
                                Ms_logManagement logModel = new Ms_logManagement();
                                logModel.operationType = 5;
                                logModel.operatorId = uploaderId;
                                logModel.operatorName = uploaderName;
                                logModel.newValue = FullFullName;
                                logModel.operationField = "文件导入";
                                _ms_logManagement.Add(logModel);
                                return MessageEntityTool.GetMessage(1, null, true, relativePath);
                            }


                        }
                        else
                        {
                            return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "文件内容为空");
                        }
                    }
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "文件为空");



                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "没有选择文件");
                }
            }
            catch (Exception ex)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "", ex.Message);
            }
            }
        class ErrorMsg
        {
            public string type { get; set; }
            public string Linenumber { get; set; }
            public string Remark { get; set; }
        }

    }
}
