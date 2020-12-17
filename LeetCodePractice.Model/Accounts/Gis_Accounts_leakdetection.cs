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
    /// 捡漏台账
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Gis_ACCOUNTS_LEAKDETECTION")]

    public class Gis_Accounts_leakdetection
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        [DataMember]
        public string LayerNO { get; set; }
        [DataMember]
        public string DeviceNO { get; set; }
        [DataMember]
        public string BelongArea { get; set; }
        [DataMember]
        public string Material { get; set; }
        [DataMember]
        public int Caliber { get; set; }
        [DataMember]
        public string MissingAddress { get; set; }
        [DataMember]
        public string AccountNO { get; set; }
        [DataMember]
        public DateTime LeakTime { get; set; }
        [DataMember]
        public string LeakType { get; set; }
        [DataMember]
        public string LeakageLocation { get; set; }
        [DataMember]
        public int LeakCaliber { get; set; }
        [DataMember]
        public float LeakageAperture { get; set; }
        [DataMember]
        public float WaterVolume { get; set; }
        [DataMember]
        public string LeakPersonnel { get; set; }
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
