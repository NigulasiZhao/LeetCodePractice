using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.AttributePack;
using GISWaterSupplyAndSewageServer.Model.ResultDto;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Task")]
    public class Ins_Task
    {
        /// <summary>
        /// 任务id主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
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
        public DateTime? VisitStarTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        [DataMember]
        public DateTime? VisitOverTime { get; set; }

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
        /// 操作人id
        /// </summary>
        [DataMember]
        public string OperatorId { get; set; }
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


        /// <summary>
        /// 任务分类主键
        /// </summary>
        [DataMember]
        public string Task_TypeId{ get; set; }
        /// <summary>
        /// 设备类型列表
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<GetTaskDetailInfoDto> TaskDetailInfoList { get; set; }

       
    }
}
