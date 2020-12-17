using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.ConfigurationManagement
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_MoveTerminal")]
    public class Ins_MoveTerminal
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string MoveId { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [DataMember]
        public string Operating_system { get; set; }

        /// <summary>
        /// 购买日期
        /// </summary>
        [DataMember]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// 购买设备名称
        /// </summary>
        [DataMember]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        [DataMember]
        public string EquipmentType { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [DataMember]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 人员id
        /// </summary>
        [DataMember]
        public string PersonID { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        [DataMember]
        public string Personname { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
        public DateTime UpdateDate { get; set; }
    }
}
