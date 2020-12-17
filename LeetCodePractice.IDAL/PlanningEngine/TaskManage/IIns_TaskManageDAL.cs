using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage
{
    public interface IIns_TaskManageDAL : IDependency
    {
        /// <summary>
        /// 任务明细完成方法
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        MessageEntity TaskDetailComplete(Ins_Task_Completedetail Model);
        /// <summary>
        /// 获得任务信息列表
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity Get(List<ParameterInfo> parInfo, string rangids, string task_Type_ids,string taskState, DateTime? startTime, DateTime? endTime, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity GetEquipmentInfo(string taskid,string IsFillForm, string sort, string ordering, int num, int page, string X, string Y);
        MessageEntity AssignTask(string taskIds);
        MessageEntity ReAssignTask(string taskIds, string proraterDeptName, string proraterDeptId, string proraterName, string proraterId);

        MessageEntity Delete(string taskId);
        MessageEntity Cancel(string taskId);
        DataTable IsAssign(string taskId);
        DataTable IsAssignTask(string plan_id);

        /// <summary>
        /// 获取任务信息及所属任务明细
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        MessageEntity GetTaskPlanInfo(string taskId);
        /// <summary>
        /// 获取任务明细关联的设备信息
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        List<GetTaskDetailInfoDto> GetTaskDetailInfo(string TaskId);

        DataTable GetTaskCount(string taskId);
        MessageEntity TaskCompleted(string taskId,string proraterId,string taskName, int? isFinsh);

    }
}
