using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
 public class Ins_PersonPositionDto
    {
      
        public string PositionId { get; set; }
   
        public string Personid { get; set; }

    
        public string Personname { get; set; }

    
        public string DetpID { get; set; }

        public string DetptName { get; set; }


      
        public string PositionX { get; set; }

        public string PositionY { get; set; }

   
        public string Taskid { get; set; }

        public decimal Distance { get; set; }
        public string WorkTime { get; set; }

        public string GroupID { get; set; }
        public string IsOnline { get; set; }
        public DateTime UpTime { get; set; }

    }
}
