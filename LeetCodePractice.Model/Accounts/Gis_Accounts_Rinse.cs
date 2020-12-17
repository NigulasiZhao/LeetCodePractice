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
    /// 管网冲洗台账
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Gis_ACCOUNTS_RINSE")]
    public class Gis_Accounts_Rinse
    {
        [DataMember]
        [Column( FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectNO { get; set; }
        [DataMember]
        public string BelongArea { get; set; }
        [DataMember]
        public string AccountNO { get; set; }
        [DataMember]
        public string ConstructionUnit { get; set; }
        [DataMember]
        public string RinseReson { get; set; }
        [DataMember]
        public DateTime BeginTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public string RinseAddress { get; set; }
        [DataMember]
        public float RinseTime { get; set; }
        [DataMember]
        public string Caliber { get; set; }
        [DataMember]
        public string PipeLength { get; set; }
        [DataMember]
        public string WaterVolume { get; set; }
        [DataMember]
        public string RinseDepartMent { get; set; }
        [DataMember]
        public string RinsePerson { get; set; }
        [DataMember]
        public string RinseNote { get; set; }
        [DataMember]
        public string OtherPerson { get; set; }
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
