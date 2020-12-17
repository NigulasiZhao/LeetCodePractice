using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class UniOrganization
    {
        public string _id { get; set; }
        public int created { get; set; }
        public int changed { get; set; }
        public string cid { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string level { get; set; }
        public string pid { get; set; }
        public string linkman { get; set; }
        public string phone { get; set; }
        public string remark { get; set; }
        public string order { get; set; }
        public string status { get; set; }
        public List<UniOrganization> children { get; set; }
       
    }
}
