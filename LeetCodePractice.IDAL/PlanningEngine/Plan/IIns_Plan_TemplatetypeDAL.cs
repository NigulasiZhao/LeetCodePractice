using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Text;
namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan
{
 public interface IIns_Plan_TemplatetypeDAL : IDependency
    {
        List<Ins_Plan_Templatetype> GetTree(string pID = "00000000-0000-0000-0000-000000000000");
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Ins_Plan_Templatetype model);
        MessageEntity Update(Ins_Plan_Templatetype model);
        MessageEntity Delete(Ins_Plan_Templatetype model);
        MessageEntity IsExistPlanTemplatetype(Ins_Plan_Templatetype model, int isAdd);
        MessageEntity IsExistPlanChildren(string plan_templatetype_id);
    }
}