using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.GisConfigSetting
{
    public class GIS_LayerSearchField
    {
        public string Name { set; get; }
        public string ClassName { set; get; }
        public Dictionary<string, string> Fields { set; get; }
        public string[] Parameters { set; get; }
        public Dictionary<string, string> Tags { set; get; }
    }
}
