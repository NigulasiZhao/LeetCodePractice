using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.EquipmentFormsRelation
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Equipment_Forms")]
    public class Ins_Equipment_Forms
    {
        /// <summary>
        /// 挂接表主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Equipment_Form_Id { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        [DataMember]
        public string LayerName { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        [DataMember]
        public string LayerCName { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public int ViewOrder { get; set; }

        /// <summary>
        /// BPM编号
        /// </summary>
        [DataMember]
        public string TableID { get; set; }
        /// <summary>
        /// 设备类型中文名称
        /// </summary>
        [DataMember]
        public string GroupCName { get; set; }
        /// <summary>
        /// 设备类型英文名称
        /// </summary>
        [DataMember]
        public string GroupName { get; set; }

    }
}
