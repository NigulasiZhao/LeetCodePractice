using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class ArcgisAreaInfo
    {
        public List<FeaturesInfo> features { get; set; }
    }
    public class FeaturesInfo
    {
        public ArcgisAreaInfoAttributes attributes { get; set; }
    }
    public class ArcgisAreaInfoAttributes
    {
        public string OBJECTID { get; set; }
        public string OID { get; set; }
        public string OCODE { get; set; }
        public string OTITLE { get; set; }
        public string A_RESPONMAN { get; set; }
        public string A_RESPONPHONE { get; set; }
        public string A_ADMIN { get; set; }
        public string A_ADMINPHONE { get; set; }
        public string A_SUPERVISION { get; set; }
        public string A_SUPERVISIONPHONE { get; set; }
        public string A_COMPANY { get; set; }
        public string A_LEVEL { get; set; }
        public string A_AREADESC { get; set; }
        public string CREATETIME { get; set; }
        public string CREATEUSER { get; set; }
        public string EDITTIME { get; set; }
        public string EDITUSER { get; set; }
        public string BORDER_WIDTH { get; set; }
        public string AREA_TRANSPARENCY { get; set; }
        public string BORDER_COLOUR { get; set; }
        public string AREA_COLOUR { get; set; }
        public string A_FIRSTCODE { get; set; }
        public string A_FIRSTOID { get; set; }
        public string RFID { get; set; }
    }
}
