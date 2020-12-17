using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan
{
   public interface IIns_Range_PointDAL : IDependency
    {
        MessageEntity GetList(string range_id);
        MessageEntity Add(Ins_Range_Point model);
        MessageEntity Update(Ins_Range_Point model);
        MessageEntity Delete(Ins_Range_Point model);
        MessageEntity IsExistRangePoint(Ins_Range_Point rangePoint, int isAdd);
    }
}