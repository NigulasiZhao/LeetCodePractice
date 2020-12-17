using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Accounts
{
    /// <summary>
    /// 养护台账
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Gis_ACCOUNTS_MAINTENANCE")]

    public class Gis_Accounts_Maintenance
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        [Column( FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        /// <summary>
        /// 图层编号
        /// </summary>
        [DataMember]
        public string LayerNO { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        [DataMember]
        public string DeviceNO { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public string BelongArea { get; set; }
        [DataMember]
        public string Material { get; set; }
        [DataMember]
        public float Caliber { get; set; }
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string BelongRoad { get; set; }
        [DataMember]
        public DateTime CompletionDate { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public string AccountNO { get; set; }
        [DataMember]
        public string MaintenanceEvent { get; set; }
        [DataMember]
        public string MaintenanceReason { get; set; }
        [DataMember]
        public string MaintenanceType { get; set; }
        [DataMember]
        public string MaintenancePerson { get; set; }
        [DataMember]
        public string MaintenanceUnit { get; set; }
        [DataMember]
        public string MaintenancePart { get; set; }
        [DataMember]
        public string DetailedAddress { get; set; }
        [DataMember]
        public string Leadman { get; set; }
        [DataMember]
        public string Verifiers { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public string Attachment { get; set; }
        [DataMember]
        public string CreatedID { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime CreatedTime { get; set; }

    }
}
