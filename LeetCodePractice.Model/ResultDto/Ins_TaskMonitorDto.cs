using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
    public class Ins_TaskMonitorDto
    {
        /// <summary>
        /// 任务id主键
        /// </summary>
        [DataMember]
        public string TaskId { get; set; }
        /// <summary>
        /// 任务编号
        /// </summary>
        [DataMember]
        public string TaskCode { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string TaskName { get; set; }
        public string IsFinishName { get; set; }
        public string Geometry { get; set; }
        public string Type { get; set; }
        public string ProraterDeptName { get; set; }
        /// <summary>
        /// 巡检员
        /// </summary>
        [DataMember]
        public string ProraterName { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        [DataMember]
        public DateTime? VisitStarTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        [DataMember]
        public DateTime? VisitOverTime { get; set; }
        public string Task_Type_Id { get; set; }
        /// <summary>
        /// 巡检员
        /// </summary>
        [DataMember]
        public string Task_Type_Name { get; set; }

        public string Plan_Cycle_Name { get; set; }
        /// <summary>
        /// 行驶里程数
        /// </summary>
        public string TravelMileage { get; set; }
        public int? Allcount { get; set; }
        /// <summary>
        /// 已巡查设备数
        /// </summary>
        [DataMember]
        public int? Completecount { get; set; }

        /// <summary>
        /// 未到位数
        /// </summary>
        [DataMember]
        public int? NoCompletecount { get; set; }

        /// <summary>
        ///完成率
        /// </summary>
        [DataMember]
        public decimal Wcl { get; set; }
    }
}
