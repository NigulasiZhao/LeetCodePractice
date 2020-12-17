using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics
{
    public interface IIns_EventStatisticsDAL : IDependency
    {
        MessageEntity GetEventFromEchart( DateTime? startTime, DateTime? endTime);
        MessageEntity GetEventTypeEchart(DateTime? startTime, DateTime? endTime);
        MessageEntity GetEventOperEchart(DateTime? startTime, DateTime? endTime);
        MessageEntity GetEventTypeDT(DateTime? startTime, DateTime? endTime);
        MessageEntity GetEventTypeTrendTable(string[] months, string yearStr, string startMStr, string endMStr);
        MessageEntity GetUserReportTable(DateTime? startTime, DateTime? endTime);
        MessageEntity GetEventByRangName(DateTime? startTime, DateTime? endTime, string? rangName );
        MessageEntity GetEventEchartByRangeDate(DateTime? startTime, DateTime? endTime, string? rangName);

        
    }
}
