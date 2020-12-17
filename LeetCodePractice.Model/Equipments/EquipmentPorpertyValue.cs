using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Equipments
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("EquipmentPorpertyValue")]
    public class EquipmentPorpertyValue
    {
        /// <summary>
        /// 设备属性的下拉列表的值  设备属性值ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        /// <summary>
        /// 设备属性的下拉列表的值  设备属性值ID
        /// </summary>
        [DataMember]
        public string EquipmentPorpertyId { get; set; }

        /// <summary>
        /// 下拉框值
        /// </summary>
        [DataMember]
        public string selectionValue { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate)]
        public int viewOrder { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        [DataMember]
        public int EquipmentId { get; set; }
    }
}
