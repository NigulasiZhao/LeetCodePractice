using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Gis_Spatialquery
{
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("sde.MANAGE_FIRSTZONE")]
    public class FirstzoneShapeModel
    {
       
                               
        [DataMember]
        public int ObjectID { get; set; }
        [DataMember]
        public string OID { get; set; }
        [DataMember]
        public string Ocode { get; set; }
        [DataMember]
        public string Otitle { get; set; }
        [DataMember]
        public string A_responman { get; set; }
        [DataMember]
        public string A_responphone { get; set; }
        [DataMember]
        public string A_admin { get; set; }
        [DataMember]
        public string A_adminphone { get; set; }
        [DataMember]
        public string A_supervision { get; set; }
        [DataMember]
        public string A_supervisionphone { get; set; }
        [DataMember]
        public string A_company { get; set; }
        [DataMember]
        public int A_level { get; set; }
        [DataMember]
        public string A_areadesc { get; set; }
        [DataMember]
        public DateTime Createtime { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public int ModifyId { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
        [DataMember]
        public DateTime ModifyTime { get; set; }
        [DataMember]
        public string LayerName { get; set; }
    }
}
