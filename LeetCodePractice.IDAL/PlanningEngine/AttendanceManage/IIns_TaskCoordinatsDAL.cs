using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage
{
    public interface IIns_TaskCoordinatsDAL : IDependency
    {
       MessageEntity GetCoordinatsByTaskid(string taskId, DateTime? startTime, DateTime? endTime, out ErrorType errorType ,out string errorString);
    }
}
