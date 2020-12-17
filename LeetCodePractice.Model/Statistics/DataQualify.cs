using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.Statistics
{
    public class DataQualify
    {
        //public string[] Titles { set; get; }

        public Dictionary<string, Dictionary<string, IList<string>>> Records { set; get; }
    }
}
