using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage
{
    public interface IIns_TaskMonitorDAL : IDependency
    {
        MessageEntity Get(DateTime? startTime, DateTime? endTime, string proraterDeptId, string proraterId, string rangids, string? task_Type_id, string sort, string ordering, int num, int page);
    }
}
