using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Runtime.Serialization;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Permission_OrganizationArea")]
    public class Permission_OrganizationArea
    {
        /// <summary>
        /// 组织机构区域权限主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }
        /// <summary>
        /// 组织机构ID
        /// </summary>
        [DataMember]
        public string OrganizationId { get; set; }
        /// <summary>
        /// 组织机构类型
        /// </summary>
        [DataMember]
        public string OrganizationType { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        [DataMember]
        public string OCode { get; set; }
    }
}
