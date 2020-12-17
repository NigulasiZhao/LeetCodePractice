using GisPlateform.IDAL;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings
{
    public interface IIns_Event_LeakDAL : IDependency 
    {
        MessageEntity Post(Ins_Event_Leak m_EventLeak);
    }
}
