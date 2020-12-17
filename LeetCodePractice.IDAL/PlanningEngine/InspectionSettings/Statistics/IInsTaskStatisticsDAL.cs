using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics
{
    public interface IInsTaskStatisticsDAL : IDependency
    {
        MessageEntity GetTaskStateEchart(DateTime? startTime, DateTime? endTime, string rangids, string? task_Type_id = null);
        MessageEntity GetTaskTypeEchart(DateTime? startTime, DateTime? endTime, string rangids, string? task_Type_id = null);
        MessageEntity GetTask(DateTime? startTime, DateTime? endTime, string rangids, string? task_Type_id);
        MessageEntity GetTaskExecuteRate(DateTime? startTime, DateTime? endTime ,string rangids);
        MessageEntity GetTaskStatistics(DateTime? startTime, DateTime? endTime, string rangids, string sqlCondition);
        MessageEntity GetInspectorWorkloadStatistics(DateTime? startTime, DateTime? endTime, string rangids, string sqlCondition);

        


    }
}
