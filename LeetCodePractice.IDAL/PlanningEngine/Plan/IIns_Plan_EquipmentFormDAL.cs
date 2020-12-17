using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan
{
    public interface IIns_Plan_EquipmentFormDAL: IDependency
    {
        MessageEntity Add(Ins_Plan_EquipmentForm model);
        MessageEntity Delete(Ins_Plan_EquipmentForm model);
    }
}
