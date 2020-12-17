using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Form
{
   public interface IGetFormInfoDAL : IDependency
    {
        MessageEntity Get(string plan_task_id,string tableId);
    }
}
