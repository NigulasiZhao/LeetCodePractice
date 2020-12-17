using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class UserPipeAndLayerPermissionInfoOutput
    {
        public string UserId { get; set; }
        public List<Permission_OrganizationArea> AreaList { get; set; }
        public List<Permission_UserLayer> LayerList { get; set; }


    }
}
