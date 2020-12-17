using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings
{
   public interface IEventStartDAL : IDependency
    {
        MessageEntity GetUrgencyComboBoxList();
        MessageEntity GetEventFromComboBoxList();
    }
}
