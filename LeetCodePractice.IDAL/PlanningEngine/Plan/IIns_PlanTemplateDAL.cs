using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan
{
    public interface IIns_PlanTemplateDAL : IDependency
    {
        MessageEntity Get(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);

        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Ins_Plantemplate model);
        MessageEntity Update(Ins_Plantemplate model);
        MessageEntity Delete(Ins_Plantemplate model);
        Ins_Plantemplate GetInfo(string ID);
        MessageEntity IsExistPlanTemplate(Ins_Plantemplate plantemplate, int isAdd);
    }
}
