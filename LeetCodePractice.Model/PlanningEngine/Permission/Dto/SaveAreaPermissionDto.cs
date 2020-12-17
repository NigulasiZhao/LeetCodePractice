using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class SaveAreaPermissionDto
    {
        public string Access_Token { get; set; }
        /// <summary>
        /// 组织机构ID
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// 组织机构类型
        /// </summary>
        public string OrganizationType { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public string[] OCode { get; set; }
    }
}
