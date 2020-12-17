using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class SavePipePermissionForDepartmentDto
    {
        public string Access_Token { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 图层信息列表
        /// </summary>
        public List<LayerInfo> LayerInfoList { get; set; }
    }

    public class LayerInfo
    {
        /// <summary>
        /// 图层Name
        /// </summary>
        public string LayerName { get; set; }
        /// <summary>
        /// 是否只读(0 否 1 是)
        /// </summary>
        public int IsReadonly { get; set; }
    }
}
