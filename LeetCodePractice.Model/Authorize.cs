using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model
{
    public class Authorize
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime ExpireTime { get; set; }
        public string IP { get; set; }
    }
}
