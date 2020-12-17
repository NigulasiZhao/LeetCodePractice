using GISWaterSupplyAndSewageServer.Model.Equipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.Equipments
{
   public class EquipmentPorpertyMappingList
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
        /// 设备属性
        /// </summary>
        public List<EquipmentPorpertyList> EquipmentPorpertyGroup { get; set; }
    }
}
