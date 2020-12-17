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
using GISWaterSupplyAndSewageServer.IDAL.Equipments;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Internalexternal
{
    /// <summary>
    /// APP获取任务列表
    /// </summary>
    public class Ms_TaskInfoAPPController : Controller
    {
        private readonly IMs_TaskManagementDAL _ms_TaskManagementDAL;
        private readonly IMs_FileStoreDAL _ms_FileStoreDAL;
        private readonly IEquipmentPorpertyMappingDAL _equipmentPorpertyMappingDAL;

        public Ms_TaskInfoAPPController(IMs_TaskManagementDAL ms_TaskManagementDAL, IMs_FileStoreDAL ms_FileStoreDAL, IEquipmentPorpertyMappingDAL equipmentPorpertyMappingDAL)
        {
            _ms_TaskManagementDAL = ms_TaskManagementDAL;
            _ms_FileStoreDAL = ms_FileStoreDAL;
            _equipmentPorpertyMappingDAL = equipmentPorpertyMappingDAL;

        }
        /// <summary>
        /// 获取已派发任务详情json信息
        /// </summary>
        /// <param name="fID">文件id</param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string fID)
        {
            //首先根据文件id获取excel上传路径
            MessageEntity messresult = _ms_FileStoreDAL.GetFileInfoByid(fID);
            List<Ms_FileStore> ptslist = (List<Ms_FileStore>)messresult.Data.Result;
            if (ptslist.Count <= 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "该文件ID无上传记录！");
            }
            try
            {
                //根据路径获取excel，转成dataTable
                DataTable dtPoint = ExcelUtility.ExcelToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ptslist[0].uploadpath), true, 0);
                DataTable dtLine = ExcelUtility.ExcelToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ptslist[0].uploadpath), true, 1);
               if(dtPoint==null || dtLine == null)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "该文件无效！");
                }
                List<EquipmentPorpertyMappingModel> listall = _equipmentPorpertyMappingDAL.GetEquipmentPorpertyMapping(null, null);
                #region 关于点表excel操作
                #region 1.首先excel与数据库中属性匹配，没有的添加上去
                foreach (DataRow item in dtPoint.Rows)
                {
                    string ename = item["类型"].ToString();
                    //string epname = item["设备类型"].ToString();
                    List<EquipmentPorpertyMappingModel> listmap = listall.Where(x => x.EName == ename).ToList();
                    listmap.ForEach(row =>
                    {
                        //判断集合中设备类型是否都存在，不存在创建列
                        string epname = row.EPName.ToString();
                        bool isExistColumn = dtPoint.Columns.Contains(epname);
                        if (!isExistColumn)
                        {
                            DataColumn dc1 = new DataColumn(row.EPName, Type.GetType("System.String"));
                            dtPoint.Columns.Add(dc1);
                        }
                    });
                }
                #endregion
                #region 2.将dt转成list集合
                List<Ms_ExcelAppUpload> listmodel = new List<Ms_ExcelAppUpload>();
                string[] Columns = ExcelUtility.GetColumnsByDataTable(dtPoint);
                foreach (DataRow item in dtPoint.Rows)
                {
                    Ms_ExcelAppUpload model = new Ms_ExcelAppUpload();
                    model.EName = item["类型"].ToString();
                    //首先获取设备下配置所有属性
                    List<EquipmentPorpertyMappingModel> listmap = listall.Where(x => x.EName == model.EName).ToList();
                    int len = item.ItemArray.Length;
                    List<Ms_Excel> grouplist = new List<Ms_Excel>();
                    for (int i = 0; i < len; i++)
                    {
                        //在获取某一属性的信息
                        List<EquipmentPorpertyMappingModel> listmapPro = listmap.Where(x => x.EPName == Columns[i].ToString()).ToList();
                        Ms_Excel excelmodel = new Ms_Excel();
                        excelmodel.Name = Columns[i].ToString();
                        excelmodel.Value = item[i].ToString();
                        if (listmapPro.Count > 0)
                        {
                            excelmodel.Inputtype = listmapPro[0].InputType;
                            excelmodel.Nullable = listmapPro[0].nullable;
                            excelmodel.IsEdit = listmapPro[0].IsEdit;
                        }
                        else
                        {
                            excelmodel.Inputtype = "t";//默认输入框
                            excelmodel.Nullable = true;//默认可空
                            excelmodel.IsEdit = true;//默认不可编辑
                        }
                        grouplist.Add(excelmodel);

                    }
                    model.EquipmentMappingGroup = grouplist;
                    listmodel.Add(model);
                }
                #endregion --2结束
                #endregion 结束关于点表excel操作 

                #region 关于线表表excel操作
                List<Ms_ExcelAppUpload> listLinemodel = new List<Ms_ExcelAppUpload>();

                DataColumn dcl1 = new DataColumn("起点X", Type.GetType("System.String"));
                DataColumn dcl2 = new DataColumn("起点Y", Type.GetType("System.String"));
                DataColumn dcl3 = new DataColumn("终点X", Type.GetType("System.String"));
                DataColumn dcl4 = new DataColumn("终点Y", Type.GetType("System.String"));
                dtLine.Columns.Add(dcl1);
                dtLine.Columns.Add(dcl2);
                dtLine.Columns.Add(dcl3);
                dtLine.Columns.Add(dcl4);
                string[] ColumnsLine = ExcelUtility.GetColumnsByDataTable(dtLine);

                foreach (DataRow item in dtLine.Rows)
                {
                    string StartCode = item["起点物探点号"].ToString();
                    string EndCode = item["终点物探点号"].ToString();
                    //根据线表起点和终点物探号获取对应的横纵坐标
                    DataRow[] drsStart = dtPoint.Select("设备编号 = '" + StartCode + "' ");
                    DataRow[] drsEnd = dtPoint.Select("设备编号 = '" + EndCode + "' ");
                    //坐标赋值
                    if (drsStart.Length > 0)
                    {
                        item["起点X"] = drsStart[0]["横坐标"].ToString();
                        item["起点Y"] = drsStart[0]["纵坐标"].ToString();
                    }
                    if (drsEnd.Length > 0)
                    {
                        item["终点X"] = drsEnd[0]["横坐标"].ToString();
                        item["终点Y"] = drsEnd[0]["纵坐标"].ToString();
                    }

                    Ms_ExcelAppUpload model = new Ms_ExcelAppUpload();
                    model.EName = "管线";
                    //首先获取设备下配置所有属性
                    List<EquipmentPorpertyMappingModel> listmap = listall.Where(x => x.EName == "管线").ToList();
                    int len = item.ItemArray.Length;
                    List<Ms_Excel> grouplist = new List<Ms_Excel>();
                    for (int i = 0; i < len; i++)
                    {
                        //在获取某一属性的信息
                        List<EquipmentPorpertyMappingModel> listmapPro = listmap.Where(x => x.EPName == ColumnsLine[i].ToString()).ToList();
                        Ms_Excel excelmodel = new Ms_Excel();
                        excelmodel.Name = ColumnsLine[i].ToString();
                        excelmodel.Value = item[i].ToString();
                        if (listmapPro.Count > 0)
                        {
                            excelmodel.Inputtype = listmapPro[0].InputType;
                            excelmodel.Nullable = listmapPro[0].nullable;
                            excelmodel.IsEdit = listmapPro[0].IsEdit;

                        }
                        else
                        {
                            excelmodel.Inputtype = "t";//默认输入框
                            excelmodel.Nullable = true;//默认可为空
                            excelmodel.IsEdit = true;//默认不可编辑

                        }
                        grouplist.Add(excelmodel);

                    }
                    model.EquipmentMappingGroup = grouplist;
                    listLinemodel.Add(model);

                }
                #endregion
                List<Ms_ExcelPointLine> listallmodel = new List<Ms_ExcelPointLine>();
                Ms_ExcelPointLine modelall = new Ms_ExcelPointLine();
                modelall.Name = "点表";
                modelall.ExcelAppGroup = listmodel;
                listallmodel.Add(modelall);
                Ms_ExcelPointLine modelall1 = new Ms_ExcelPointLine();
                modelall1.Name = "线表";
                modelall1.ExcelAppGroup = listLinemodel;
                listallmodel.Add(modelall1);
                var json = JsonConvert.SerializeObject(listallmodel);
                return MessageEntityTool.GetMessage(1, json, true, "完成");

            }
            catch (Exception ex)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "", ex.Message);
            }

        }

        /// <summary>
        /// 任务列表App
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition|startTime|endTime",LinkType:and|or}]查询参数名称 上传人：uploaderid 派发人:dispatchpersonid 外业人：execpersonid 派发状态:ispost 任务名称:taskname  上传时间:uploadetime</param>
        /// <param name="sort"></param>
        /// <param name="describe">文件描述</param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetList(string ParInfo, string describe = "", string sort = "taskdate", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var messresult = _ms_TaskManagementDAL.GetList(list, describe, sort, ordering, num, page, sqlCondition);
            List<Ms_TaskFileList> ptslist = (List<Ms_TaskFileList>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                ptslist.ForEach(row =>
                {
                    var result = Get(row.FId);
                    if (result.ErrorType == 3)
                    {
                        //json转list
                        List<Ms_ExcelPointLine> list = JsonConvert.DeserializeObject<List<Ms_ExcelPointLine>>(result.Data.Result.ToString());
                        row.ExcelPointLineList = list;
                    }

                });
                return MessageEntityTool.GetMessage(1, ptslist);

            }
            return messresult;
        }
    }
}
