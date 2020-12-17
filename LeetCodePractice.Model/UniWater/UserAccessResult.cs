using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.UniWater
{
  public  class UserAccessResult
    {
        public string state { set; get; }
        public string status { set; get; }
        public string order { set; get; }
        public HdUser User { set; get; }
    }
}
