using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class SavePipePermissionForUserDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 图层信息
        /// </summary>
        public List<LayerInfo> LayerInfoList { get; set; }
    }
}
