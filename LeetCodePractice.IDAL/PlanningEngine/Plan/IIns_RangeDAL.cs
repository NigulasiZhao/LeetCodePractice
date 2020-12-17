using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Text;
namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan
{
    public interface IIns_RangeDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        List<Ins_Range> GetTree(string pID = "00000000-0000-0000-0000-000000000000");
        MessageEntity Add(Ins_Range model);
        MessageEntity Update(Ins_Range model);
        MessageEntity Delete(Ins_Range model);
        MessageEntity IsExistRange(Ins_Range model, int isAdd);
        MessageEntity IsExistRangeChildren(string range_id);

    }
}