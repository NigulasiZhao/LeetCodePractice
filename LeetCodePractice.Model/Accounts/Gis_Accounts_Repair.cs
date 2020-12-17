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
    /// 维修台账
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Gis_ACCOUNTS_REPAIR")]

    public class Gis_Accounts_Repair
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        [DataMember]
        public string LayerNO { get; set; }
        [DataMember]
        public string DeviceNO { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public string BelongArea { get; set; }
        [DataMember]
        public string Material { get; set; }
        [DataMember]
        public int Caliber { get; set; }
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
        public string Model { get; set; }
        [DataMember]
        public string AccountNO { get; set; }
        [DataMember]
        public string RepairPart { get; set; }
        [DataMember]
        public string RepairAddress { get; set; }
        [DataMember]
        public string RepairResons { get; set; }
        [DataMember]
        public string DamageType { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public int ISReducePressure { get; set; }
        [DataMember]
        public int ISWaterShutdown { get; set; }
        [DataMember]
        public int OvertimeCount { get; set; }
        [DataMember]
        public float WaterVolume { get; set; }
        [DataMember]
        public string InformationNote { get; set; }
        [DataMember]
        public string RepairUnit { get; set; }
        [DataMember]
        public DateTime DiscoveryTime { get; set; }
        [DataMember]
        public string RepairType { get; set; }
        [DataMember]
        public string RepairPerson { get; set; }
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
