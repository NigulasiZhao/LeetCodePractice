﻿using GISWaterSupplyAndSewageServer.Model.AttributePack;
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
    [System.ComponentModel.DataAnnotations.Schema.Table("EquipmentPorpertyMapping")]
    public class EquipmentPorpertyMapping
    {
        /// <summary>
        /// 点\线对应表主键
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
        /// 可为空1:空 0 不可空
        /// </summary>
        [DataMember]
        public int nullable { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        [DataMember]
        public int EquipmentId { get; set; }


    }
}
