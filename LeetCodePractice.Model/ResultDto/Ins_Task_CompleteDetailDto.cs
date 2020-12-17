using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
 public   class Ins_Task_CompleteDetailDto
    {
     
        public string ID { set; get; }
     
        public string TaskId { set; get; }
     
        public string Devicename { set; get; }
     
        public string Devicesmid { set; get; }
     
        public DateTime Uptime { set; get; }
      public string NowTime { set; get; }
        public string x { set; get; }
     
        public string y { set; get; }
        public string lon { set; get; }

        public string lat { set; get; }
        public string Peopleid { set; get; }
     
        public string ImagePath { set; get; }
     
        public string Address { set; get; }
     
        public string Describe { set; get; }
     
        public int PointType { set; get; }

     
        public int IsHidden { set; get; }
     
        public int IsFeedback { set; get; }

     
        public string plan_task_id { set; get; }
    }
}
