using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class UniUserInfo
    {
        public string _id { get; set; }
        public int created { get; set; }
        public int changed { get; set; }
        public string sn { get; set; }
        public string cid { get; set; }
        public string name { get; set; }
        public string account { get; set; }
        public string sex { get; set; }
        public string birth { get; set; }
        public string idcard { get; set; }
        public string job { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string imei { get; set; }
        public string smobile { get; set; }
        public string telephone { get; set; }
        public string group { get; set; }
        public string role { get; set; }
        public string[] roless { get; set; }
        public int order { get; set; }
        public int status { get; set; }
        public string duty { get; set; }
    }
}
