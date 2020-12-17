using GisPlateform.IDAL;
using GisPlateform.Model;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings
{
    public interface IIns_EventHistoryDAL : IDependency
    {
        MessageEntity Get(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page,string  sqlCondition);
    
    }
}
