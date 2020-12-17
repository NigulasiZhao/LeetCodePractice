using GISWaterSupplyAndSewageServer.IDAL;

using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics
{
    public interface IInspectionStaticsDAL : IDependency
    {
        MessageEntity GetByOwnID(string iAdminID, string? task_Type_id, string? isFinish, DateTime? startTime, DateTime? endTime);
        MessageEntity GetInspectionOverview(DateTime? startTime, DateTime? endTime, string rangName);


    }
}
