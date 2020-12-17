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
    /// 阀门启闭台账
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Gis_ACCOUNT_VALVE")]
    public class Gis_Account_Valve
    {
        [DataMember]
        [Column( FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        [DataMember]
        public string LayerNO { get; set; }
        [DataMember]
        public string AccountNO { get; set; }
        [DataMember]
        public int ValveNO { get; set; }
        [DataMember]
        public int Caliber { get; set; }
        [DataMember]
        public string BelongArea { get; set; }
        [DataMember]
        public string Material { get; set; }
        [DataMember]
        public string BelongRoad { get; set; }
        [DataMember]
        public string OperUser { get; set; }
        [DataMember]
        public DateTime OperTime { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public float Degree { get; set; }
        [DataMember]
        public int ValveStatus { get; set; }
        [DataMember]
        public string Leadman { get; set; }
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
