using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
   public class Ins_TaskDto
    {
        /// <summary>
        /// 任务id主键
        /// </summary>
        [DataMember]
        public string TaskId { get; set; }
        public string Range_Id { get; set; }
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
        /// <summary>
        /// 任务类型id
        /// </summary>
        [DataMember]
        public string Task_Type_Id { get; set; }
        /// <summary>
        /// 任务类型名称
        /// </summary>
        [DataMember]
        public string Task_Type_Name { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public string ProraterDeptName { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [DataMember]
        public string ProraterDeptId { get; set; }

        /// <summary>
        /// 巡检人员ID
        /// </summary>
        [DataMember]
        public string ProraterId { get; set; }
        /// <summary>
        /// 巡检人员
        /// </summary>
        [DataMember]
        public string ProraterName { get; set; }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        [DataMember]
        public DateTime VisitStarTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        [DataMember]
        public DateTime VisitOverTime { get; set; }
        /// <summary>
        /// 巡检周期
        /// </summary>
        public string Plan_Cycle_Name { get; set; }
        /// <summary>
        /// 任务执行频率
        /// </summary>
        [DataMember]
        public string Frequency { get; set; }

        /// <summary>
        ///  任务描述
        /// </summary>
        [DataMember]
        public string Descript { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        public string Operator { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [DataMember]
        public DateTime? OperateDate { get; set; }

        /// <summary>
        ///  任务状态 0:作废1:启用
        /// </summary>
        [DataMember]
        public int? TaskState { get; set; }
        
        public string TaskstateName { get; set; }

        /// <summary>
        /// 计划id
        /// </summary>
        [DataMember]
        public string Plan_Id { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        [DataMember]
        public string Plan_Name { get; set; }
        /// <summary>
        /// 巡检方式
        /// </summary>
        [DataMember]
        public string MoveType { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
        /// <summary>
        /// 0:未完成 1:已完成
        /// </summary>
        [DataMember]
        public int? IsFinish { get; set; }
        /// <summary>
        /// 0或者空为未分派1:已分派
        /// </summary>
        [DataMember]
        public int? AssignState { get; set; }
        [DataMember]
        public string IsFeedBack { get; set; }
        /// <summary>
        /// 0:未完成 1:已完成
        /// </summary>
        [DataMember]
        public string IsFinishName { get; set; }
        /// <summary>
        /// 总设备数
        /// </summary>
        [DataMember]
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
        ///已上报设备数
        /// </summary>
        [DataMember]
        public int? Reportcount { get; set; }
        /// <summary>
        ///完成率
        /// </summary>
        [DataMember]
        public decimal Wcl { get; set; }

        /// <summary>
        ///完成率整型
        /// </summary>
        [DataMember]
        public int? WclInt { get; set; }
        /// <summary>
        ///分派状态
        /// </summary>
        [DataMember]
        public string AssignstateName { get; set; }
        /// <summary>
        ///gis巡检范围坐标信息
        /// </summary>
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string Geometry { get; set; }

        /// <summary>
        ///行驶里程数
        /// </summary>
        [DataMember]
        public string TravelMileage { get; set; }
        /// <summary>
        ///日平均行驶里程数
        /// </summary>
        [DataMember]
        public string DaytravelMileage { get; set; }

        /// <summary>
        ///巡检天数
        /// </summary>
        [DataMember]
        public int? Days { get; set; }

        /// <summary>
        /// 设备类型列表
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<GetTaskDetailInfoDto> TaskDetailInfoList { get; set; }

        /// <summary>
        /// 任务设备信息列表
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<EquipmentDetailInfoDto> EquipmentDetailInfoList { get; set; }
    }
}
