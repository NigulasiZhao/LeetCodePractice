using GISWaterSupplyAndSewageServer.Model.Equipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.Equipments
{
  public  class EquipmentPorpertyList
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public int EquipmentId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EName { get; set; }
        /// <summary>
        /// 设备属性的下拉列表的值  设备属性值ID
        /// </summary>
        public string EquipmentPorpertyId { get; set; }
        /// <summary>
        /// 设备属性值
        /// </summary>
        public string EPName { get; set; }
        /// <summary>
        /// 设备属性的下拉列表的值集合
        /// </summary>
        public List<EquipmentPorpertyValue> EquipmentPorpertyValueGroup { get; set; }
    }
}
