using System;
using System.Collections.Generic;
using System.Linq;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.Equipments;
using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Internalexternal
{
    /// <summary>
    /// 采集工单内业系统对接(内外业)
    /// </summary>
    public class Ms_WorkOrderController : Controller
    {
        private readonly IMs_WorkOrderDAL _ms_WorkOrderDAL;
        private readonly IMs_logManagementDAL _ms_logManagement;

        public Ms_WorkOrderController(IMs_WorkOrderDAL ms_WorkOrderDAL, IMs_logManagementDAL ms_logManagement)
        {
            _ms_WorkOrderDAL = ms_WorkOrderDAL;
            _ms_logManagement = ms_logManagement;
        }
        /// <summary>
        /// 获取工单上报信息（包含具体设备信息）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get()
        {
            var result = _ms_WorkOrderDAL.GetList();
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="workorderid">工单id</param>
        /// <param name="handlePersonID">编辑人员id</param>
        /// <param name="handlePerson">编辑人员</param>
        /// <param name="isPostcomplete">是否分派 0,1,2 0：未分派 1：已分派 2:已完成</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string workorderid, string handlePersonID, string handlePerson, string isPostcomplete)
        {
            string isPostcompleteName = "";
            if (isPostcomplete == "1")
            {
                isPostcompleteName = "已分派";
            }
            else if (isPostcomplete == "2")
            {
                isPostcompleteName = "已完成";
            }
            else
            {
                isPostcompleteName = "未分派";
            }
            //1.获取旧值
            string oldValue = "";
            MessageEntity messresult1 = _ms_WorkOrderDAL.GetWorkOrderByWorkorderid(workorderid);
            List<Ms_WorkOrder> list = (List<Ms_WorkOrder>)messresult1.Data.Result;
            if (list.Count > 0)
            {
                oldValue = list[0].isPostcomplete.ToString();
            }
            Ms_logManagement logModel = new Ms_logManagement();
            logModel.operationType = 6;
            logModel.operatorId = handlePersonID;
            logModel.operatorName = handlePerson;
            logModel.newValue = isPostcompleteName;
            logModel.oldValue = oldValue;
            logModel.operationField = "内业系统是否分派完成";
            var result = _ms_WorkOrderDAL.Update(workorderid, handlePerson, isPostcomplete);
            if (result.ErrorType == 3)
            {
                _ms_logManagement.Add(logModel);
            }
            return result;
        }
    }
}
