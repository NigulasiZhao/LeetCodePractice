using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage
{
    public interface IIns_PersonPositionDAL : IDependency
    {
        MessageEntity Add(Ins_PersonPosition model);
        MessageEntity GetPositionByPersonid(string OffLineTime);
        MessageEntity GetWorkingTimeDistance(string personid,DateTime startTime, DateTime endTime);
        DataTable GetPositionByTaskid(string taskid);
        MessageEntity Update(string taskid,string minutes,int distance);
        MessageEntity Update(string positionid);
    }
}
