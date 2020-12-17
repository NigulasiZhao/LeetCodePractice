using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Runtime.Serialization;

namespace GISWaterSupplyAndSewageServer.Model
{

    public class Pipe
    {
        public string OBJECTID { set; get; }
        public string equipment_number { set; get; }
        public string material_science { set; get; }
        public string caliber { set; get; }
        public decimal length { set; get; }
        public decimal startingpoint_elevation { set; get; }
        public decimal endpoint_elevation { set; get; }
        public string Installation_address { set; get; }
        public string pipeground { set; get; }
        public string management_unit { set; get; }
        public DateTime completion_date { set; get; }
        public DateTime laying_age { set; get; }
        public decimal startingpoint_depth { set; get; }
        public decimal endpoint_depth { set; get; }
        public string embedding_mode { set; get; }
        public string equipment_type { set; get; }
        public string Interface_form { set; get; }
        public string remarks { set; get; }
        public string businessarea { set; get; }
        public string ownedwater { set; get; }
        public int Enabled { set; get; }
        public int PID { set; get; }
        //ZL注释
        //public SqlGeometry Shape { set; get; }
        public string distance { set; get; }
    }
}
