using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.UniWater
{
   public class HdUser
    {
        public string _id { set; get; }
        public string Account { set; get; }
        public string Sn { set; get; }
        public string Name { set; get; }
        public string Sex { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
        public string Group { set; get; }
        public Group_Data group_data { set; get; }
        public string Role { set; get; }
        public string Roles { set; get; }
       
    }
}
