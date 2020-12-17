using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace GISWaterSupplyAndSewageServer.Model.EssentialFactor
{
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_ESSENTIALFACTOR")]
     public class GIS_EssentialFactor
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        [DataMember]
        public string PID { get; set; }
        [DataMember]
        public string EFName { get; set; }
        [DataMember]
        public string EFCName { get; set; }
        [DataMember]
        public int EFType { get; set; }
        [DataMember]
        public int EFViewType { get; set; }
        [DataMember]
        public string LinkURL { get; set; }
        [DataMember]
        public string ICon { get; set; }
        [DataMember]
        public int NodeType { get; set; }
        [DataMember]
        public string NodeData { get; set; }
        [DataMember]
        public int OrderNO { get; set; }
        [DataMember]
        public int Enabled { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public string CreateId { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string ModifyId { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]
        public DateTime ModifyTime { get; set; }
        [DataMember]
        public string LayerName { get; set; }
        [DataMember]
        public string TitleAttribute { get; set; }
        [DataMember]
        public string AttributeId { get; set; }
        
    }
}
