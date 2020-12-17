using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
  public  class InsTaskExecuteRate
    {
        public string Proraterdeptname { set; get; }
        public string Proratername { set; get; }
        /// <summary>
        /// 总设备数
        /// </summary>
        public int? Allcount { get; set; }
        /// <summary>
        /// 已巡查设备数
        /// </summary>
        public int? Completecount { get; set; }
        /// <summary>
        ///已上报设备数
        /// </summary>
        public int? Reportcount { get; set; }

        /// <summary>
        ///完成率
        /// </summary>
        public string Wcl { get; set; }
        public string TaskCode { get; set; }

        public string TaskName { get; set; }
        public DateTime? VisitStarTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime? VisitOverTime { get; set; }
    }
}
