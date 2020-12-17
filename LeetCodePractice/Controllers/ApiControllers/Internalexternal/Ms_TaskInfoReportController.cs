using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Newtonsoft.Json;
using GISWaterSupplyAndSewageServer.CommonTools;
using System.Data;
using System.Text;
using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.IO;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Internalexternal
{
    /// <summary>
    /// 采集工单上报(内外业)
    /// </summary>
    public class Ms_TaskInfoReportController : Controller
    {
        private readonly IMs_TaskInfoReportDAL _ms_TaskInfoReportDAL;
        private readonly IMs_TaskManagementDAL _ms_TaskManagementDAL;
        private readonly IMs_logManagementDAL _ms_logManagement;
        private readonly IMs_WorkOrderDAL _ms_WorkOrderDAL;


        public Ms_TaskInfoReportController(IMs_TaskInfoReportDAL ms_TaskInfoReportDAL, IMs_TaskManagementDAL ms_TaskManagementDAL, IMs_logManagementDAL ms_logManagement, IMs_WorkOrderDAL ms_WorkOrderDAL)
        {
            _ms_TaskInfoReportDAL = ms_TaskInfoReportDAL;
            _ms_TaskManagementDAL = ms_TaskManagementDAL;
            _ms_logManagement = ms_logManagement;
            _ms_WorkOrderDAL = ms_WorkOrderDAL;
        }
        /// <summary>
        /// 采集工单上报
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="operatorId">操作人id</param>
        /// <param name="operatorName">操作人姓名</param>
        /// <param name="value">工单数据</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string taskId, string taskName, string operatorId, string operatorName, [FromBody]List<Ms_ExcelPointLine> value)
        {
            try
            {
                string valuejson = JsonConvert.SerializeObject(value);
                //1.首先根据任务id查询是否存在记录
                MessageEntity messresult1 = _ms_WorkOrderDAL.GetWorkOrderByTaskid(taskId);
                List<Ms_WorkOrder> list = (List<Ms_WorkOrder>)messresult1.Data.Result;
                if (list.Count > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "该工单已上报，不允许重复上报！");
                }
                //2.根据任务id获取excel上传路径
                MessageEntity messresult = _ms_TaskManagementDAL.GetFileInfoByTaskid(taskId);
                List<Ms_FileStore> ptslist = (List<Ms_FileStore>)messresult.Data.Result;
                if (ptslist.Count <= 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "该任务ID无上传记录！");
                }
                if (value == null)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "工单数据不能为空！");
                }
                if (value.Count < 2)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "工单数据不符合，应包含点和线数据！");
                }
                # region 1.1首先验证属性信息中必输项是否输入
                //1.首先验证属性信息中必输项是否输入
                DataTable dtErr = new DataTable();
                DataColumn dc1 = new DataColumn("EName", Type.GetType("System.String"));
                DataColumn dc2 = new DataColumn("Remark", Type.GetType("System.String"));
                dtErr.Columns.Add(dc1);
                dtErr.Columns.Add(dc2);
                value.ForEach(row =>
                {
                    //1.获取点或线表list
                    List<Ms_ExcelAppUpload> _list = row.ExcelAppGroup;
                    _list.ForEach(row1 =>
                    {
                        //2.获取设备的所有列信息list
                        List<Ms_Excel> equGrouplist = row1.EquipmentMappingGroup;
                        //3.获取设备中非空列
                        List<Ms_Excel> nullablelist = equGrouplist.Where(x => x.Nullable == false).ToList();
                        //4.获取设备中非空列内容为空属性
                        List<Ms_Excel> _nullvaluelist = nullablelist.Where(x => x.Value == "").ToList();
                        List<string> Nameslist = _nullvaluelist.Select(Row => Row.Name).ToList();
                        if (Nameslist.Count > 0)
                        {
                            string Names = "'" + string.Join("','", Nameslist.ToArray()) + "'";
                            DataRow dr = dtErr.NewRow();
                            dr["EName"] = row.Name + ":" + row1.EName + "-" + equGrouplist[0].Value;
                            dr["Remark"] = Names + ",以上必输项不能为空";
                            dtErr.Rows.Add(dr);
                        }
                    });
                });
                if (dtErr.Rows.Count > 0)
                {
                    //返回错误信息json
                    //var json = new JavaScriptSerializer().Serialize(ExcelUtility.DataTableToList(dtErr));
                    var json = JsonConvert.SerializeObject(ExcelUtility.DataTableToList(dtErr));
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", json);
                }
                #endregion 1.1
                # region  1.2首先根据任务id找到文件上传记录用于判断工单数据修改信息
                //1.首先根据任务id找到文件上传记录用于判断工单数据修改信息
                //根据路径获取excel，转成dataTable
                DataTable dtPoint = ExcelUtility.ExcelToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,ptslist[0].uploadpath), true, 0);
                DataTable dtLine = ExcelUtility.ExcelToDataTable(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ptslist[0].uploadpath), true, 1);
                List<Ms_ExcelPointLine> listPoints = value.Where(x => x.Name == "点表").ToList();
                List<Ms_ExcelPointLine> listLines = value.Where(x => x.Name == "线表").ToList();
                List<Ms_logManagement> loglist = new List<Ms_logManagement>();
                if (dtPoint == null || dtLine == null)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "路径下没找到对应excel！");
                }
                int i = 0;
                //循环点表 比对修改项
                foreach (DataRow item in dtPoint.Rows)
                {
                    List<Ms_Excel> equGrouplist = listPoints[0].ExcelAppGroup[i].EquipmentMappingGroup;
                    int len = item.ItemArray.Length;
                    for (int j = 0; j < len; j++)
                    {
                        //如果之前下载文件内容与采集工单上传内容不一致 添加日志
                        //设备属性允许编辑
                        if (equGrouplist[j].IsEdit)
                        {
                            if (item[j].ToString() != equGrouplist[j].Value)
                            {
                                Ms_logManagement logModel = new Ms_logManagement();
                                logModel.operationType = 3;
                                logModel.operatorId = operatorId;
                                logModel.operatorName = operatorName;
                                logModel.newValue = equGrouplist[j].Value;
                                logModel.oldValue = item[j].ToString();
                                logModel.operationField = equGrouplist[j].Name;
                                loglist.Add(logModel);
                            }
                        }
                    }
                    i++;
                }
                int ii = 0;
                foreach (DataRow item in dtLine.Rows)
                {
                    List<Ms_Excel> equGrouplist = listLines[0].ExcelAppGroup[ii].EquipmentMappingGroup;
                    int len = item.ItemArray.Length;
                    for (int j = 0; j < len; j++)
                    {
                        //如果之前下载文件内容与采集工单上传内容不一致 添加日志
                        //设备属性允许编辑
                        if (equGrouplist[j].IsEdit)
                        {
                            if (item[j].ToString() != equGrouplist[j].Value)
                            {
                                Ms_logManagement logModel = new Ms_logManagement();
                                logModel.operationType = 4;
                                logModel.operatorId = operatorId;
                                logModel.operatorName = operatorName;
                                logModel.newValue = equGrouplist[j].Value;
                                logModel.oldValue = item[j].ToString();
                                logModel.operationField = equGrouplist[j].Name;
                                loglist.Add(logModel);
                            }
                        }
                    }
                    ii++;
                }
                #endregion 1.2
                //循环遍历增加日志记录 然后添加采集工单上报数据
                if (loglist.Count > 0)
                {
                    MessageEntity res = _ms_logManagement.AddList(loglist);
                }

                //日志插入成功之后添加工单上报信息
                Ms_WorkOrder model = new Ms_WorkOrder();
                model.taskid = taskId;
                model.taskName = taskName;
                model.isPostcomplete = "0";
                model.Equipmentjson = valuejson;
                var result = _ms_WorkOrderDAL.Add(model);
                return result;
            }
            catch (Exception ex)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "", ex.Message);
            }
        }

    }
}
