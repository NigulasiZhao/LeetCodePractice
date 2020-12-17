using GisPlateform.IDAL;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings
{
    public interface IIns_EventDAL: IDependency
    {
        /// <summary>
        /// 事件工单查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="EventFromId">事件来源 3:巡检上报 默认空查全部</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="DeptId">上报部门id</param>
        /// <param name="EventContenct">查询条件(事件来源:类型、上报人、编号)</param>
        /// <param name="OperId">操作记录行id</param>
        /// <param name="IsValid">是否有效</param>
        /// <param name="sort">排序字段默认EventID</param>
        /// <param name="ordering">desc/asc</param>
        /// <param name="num">默认20</param>
        /// <param name="page">默认1</param>
        MessageEntity GetEventWorkorderListForMaintain(DateTime? startTime, DateTime? endTime,string? rangName, int? EventFromId, string? eventType, string? EventID, string OperId, int? IsValid, string? DeptId, string EventContenct, string ExecPersonId, string X, string Y, string sort, string ordering, int num, int page);
        MessageEntity GetEventWorkorderStepForMaintain(string eventID);
        MessageEntity GetEventListOwn(string OwnID, string? eventType, string? operId, DateTime? startTime, DateTime? endTime, string sort, string ordering, int num, int page);

        MessageEntity PostEvent(string plan_Task_Id,Ins_Event m_Event, Ins_WorkOrder_Oper_History workOrder_Oper_History);
        MessageEntity WorkListAssign(string eventID, string execTime, Ins_WorkOrder_Oper_History workOrder_Oper_History);
        MessageEntity WorkListReceipt(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History);
        MessageEntity WorkListPresent(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History);
        MessageEntity WorkListChuZhi(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History);
        MessageEntity WorkListFinished(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History);
        MessageEntity WorkListAudit(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History);




        MessageEntity WordListDelay(string eventID, string operRemarks, string complishTime, string dispatchPersonID, string dispatchPersonName, string dispatchDetpID, string dispatchDetptName);
        MessageEntity WorkListDelayExec(string eventID, string complishTime, string dispatchPersonID, string dispatchPersonName, string dispatchDetpID, string dispatchDetptName, string OperRemarks = "");
        MessageEntity WorkListDelayReturn(string eventID, string complishTime, string dispatchPersonID, string dispatchPersonName, string dispatchDetpID, string dispatchDetptName, string OperRemarks = "");
        MessageEntity WordListBackExec(string eventID,  string iAdminID, string iAdminName, string detpID, string detpName, string backDesc);
        MessageEntity WorkorderInvalid(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History);
        bool IsExecute(string eventID, string StepNum, out string errorMsg);
        bool IsValid(string eventID, string isValid, out string errorMsg);
        DataTable IsExistPostponeOrderByDay(string eventId, string complishTime);
 


    }
}
