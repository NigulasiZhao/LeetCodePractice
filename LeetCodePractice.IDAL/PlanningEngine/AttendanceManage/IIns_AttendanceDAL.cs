using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage
{
      public interface IIns_AttendanceDAL : IDependency
    {
        MessageEntity Add(Ins_Attendance model);
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);

        MessageEntity GetLastAttendance(string Personid);
        MessageEntity GetLastAttendanceByTaskid(string taskid);
    }
}
