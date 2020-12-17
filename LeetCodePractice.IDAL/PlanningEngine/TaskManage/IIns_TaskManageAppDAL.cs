using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage
{
    public interface IIns_TaskManageAppDAL : IDependency
    {
        /// <summary>
        /// 获得任务信息列表
        /// </summary>
        /// <param name="iAdminID">巡检员id</param>
        /// <param name="isFinish">任务状态 0：未完成 1：已完成  空代表全部</param>
        /// <param name="task_Type_id">任务类型</param>
        /// <param name="startTime">yyyy-MM-dd</param>
        /// <param name="endTime">yyyy-MM-dd</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity Get(string iAdminID, string? taskName, string? taskid, string? isFinish, string? task_Type_id, DateTime? startTime ,DateTime? endTime, string sort, string ordering, int num, int page);
        MessageEntity GetWorkCode(string tableId);
    }
}
