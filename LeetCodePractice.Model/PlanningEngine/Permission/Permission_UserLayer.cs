using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Runtime.Serialization;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Permission_UserLayer")]
    public class Permission_UserLayer
    {
        /// <summary>
        /// 用户图层权限主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 图层Name
        /// </summary>
        [DataMember]
        public string LayerName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        [DataMember]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 是否只读(0 否 1 是)
        /// </summary>
        [DataMember]
        public int IsReadonly { get; set; }
    }
}
