using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
    [Serializable]
    public  class Ins_Form_LeakDetectionModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Plan_task_id { get; set; }
        public string LayerName { get; set; }
        /// <summary>
        /// 是否填单
        /// </summary>
        public int IsFillForm { get; set; }
        public string[] ImagePath { get; set; }

        public string WorkCode { get; set; }
        public string Caliber { get; set; }
      
        public string Remark { get; set; }
        public string GlobID { get; set; }
        public string DetailAddress { get; set; }

    }
}
