using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.Gis_Spatialquery
{
    public class SpatialSearchPaginationForm
    {
        public Dictionary<string, int> EquipmentCount { set; get; }
        public List<double[]> Polygon { set; get; }
        public List<string> ClassNames { set; get; }
        public int[] Grids { set; get; }
        public string[] Districts { set; get; }
    }
}
