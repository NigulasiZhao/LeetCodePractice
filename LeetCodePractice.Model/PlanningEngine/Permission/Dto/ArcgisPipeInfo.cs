using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class ArcgisPipeInfo
    {
        public List<ArcgisPipeAttributes> layers { get; set; }
    }


    public class ArcgisPipeAttributes
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentLayerId { get; set; }
        public bool defaultVisibility { get; set; }
        public int[] subLayerIds { get; set; }
        public int minScale { get; set; }
        public int maxScale { get; set; }
        public string type { get; set; }
        public string geometryType { get; set; }
    }
}
