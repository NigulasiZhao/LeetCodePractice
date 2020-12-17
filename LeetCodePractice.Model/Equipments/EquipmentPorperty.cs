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
    [System.ComponentModel.DataAnnotations.Schema.Table("EquipmentPorperty")]
    public class EquipmentPorperty
    {
        /// <summary>
        /// 设备属性  设备属性值ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string EquipmentPorpertyId { get; set; }

        /// <summary>
        /// 设备属性值
        /// </summary>
        [DataMember]
        public string EPName { get; set; }

        /// <summary>
        /// s:下拉框 t:输入框 d:日期 yyyy-dd-mm dt:时间
        /// </summary>
        [DataMember]
        public string InputType { get; set; }
        /// <summary>
        /// 是否可编辑 0：不可编辑 1：可编辑
        /// </summary>
        [DataMember]
        public int IsEdit { get; set; }
        
        /// <summary>
        /// 设备属性的下拉列表的值集合
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<EquipmentPorpertyValue> EquipmentPorpertyValueGroup { get; set; }
    }
    }
