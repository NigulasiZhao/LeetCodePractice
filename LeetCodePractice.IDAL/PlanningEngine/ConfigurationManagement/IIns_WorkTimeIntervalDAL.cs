using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.ConfigurationManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.ConfigurationManagement
{
    public interface IIns_WorkTimeIntervalDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Ins_WorkTimeInterval model);
        MessageEntity Update(Ins_WorkTimeInterval model);
        MessageEntity Delete(Ins_WorkTimeInterval model);
    }
}
