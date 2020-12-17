using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.Statistics
{
    public interface IGIS_DataQualifyDAL : IDependency
    {
        DataQualify GetData(DateTime? startTime, DateTime? endTime, out ErrorType errorType, out string errorString);
        DataQualify GetFacilityCount(DateTime? startTime, DateTime? endTime, out ErrorType errorType, out string errorString);
    }
}
