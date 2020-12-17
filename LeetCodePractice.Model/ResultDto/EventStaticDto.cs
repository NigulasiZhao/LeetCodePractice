using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
   public class EventStaticDto
    {
        public string RangName { set; get; }
        public int SumCount { set; get; }
        public int SuccessCount { set; get; }

        public string YM { set; get; }
        public string Jdl { set; get; }

        public string Wcl { set; get; }

    }
}
