using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
    [Serializable]
    public  class Ins_Form_PipelineequModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

      
        public string TaskId { get; set; }
      
        public string TaskName { get; set; }
        public string WorkCode { get; set; }

        public string GlobID { get; set; }
      
        public string LayerName { get; set; }
      
        public string X { get; set; }
      
        public string Y { get; set; }
      
        public string Plan_task_id { get; set; }
        /// <summary>
        /// 是否填单
        /// </summary>
      
        public int IsFillForm { get; set; }
      
        public string[] ImagePath { get; set; }
        /// <summary>
        ///自动排气阀
        /// </summary>
      
        public int Automaticpqvalve { get; set; }

        /// <summary>
        ///减压阀(高层建筑)
        /// </summary>
      
        public int Jpressurevalve { get; set; }

        /// <summary>
        ///减压阀前过滤器
        /// </summary>
      
        public int Jpressurevalvegl { get; set; }

        /// <summary>
        ///油漆修补
        /// </summary>
      
        public int Paintrepair { get; set; }

        /// <summary>
        ///各类管道
        /// </summary>
      
        public int Variouspipe { get; set; }

        /// <summary>
        ///各类阀门
        /// </summary>
      
        public int Variousvalve { get; set; }
    }
}
