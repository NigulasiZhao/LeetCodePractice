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
    public class EquipmentPorpertyMappingModel
    {
        /// <summary>
        /// 点\线对应表主键
        /// </summary>
        [DataMember]
        public string ID { get; set; }
        /// <summary>
        /// 设备属性的下拉列表的值  设备属性值ID
        /// </summary>
        [DataMember]
        public string EquipmentPorpertyId { get; set; }

        /// <summary>
        /// 可为空1:空 0 不可空
        /// </summary>
        [DataMember]
        public bool nullable { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        [DataMember]
        public int EquipmentId { get; set; }

        /// <summary>
        /// 设备属性下拉框中属性id-有值代表私有属性
        /// </summary>
        [DataMember]
        public int EquipmentIdepv { get; set; }
        /// <summary>
        /// 下拉框值
        /// </summary>
        [DataMember]
        public string selectionValue { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [DataMember]
        public int viewOrder { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string EName { get; set; }
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
        public bool IsEdit { get; set; }
    }
}
