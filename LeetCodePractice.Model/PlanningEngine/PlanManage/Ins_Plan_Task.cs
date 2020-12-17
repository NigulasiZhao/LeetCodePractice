using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan_Task")]
    public class Ins_Plan_Task
    {
        /// <summary>
        /// 计划任务id
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Plan_Task_Id { get; set; }

        /// <summary>
        /// 设施信息id
        /// </summary>
        [DataMember]
        public string Equipment_Info_Id { get; set; }

        /// <summary>
        /// 计划id
        /// </summary>
        [DataMember]
        public string Plan_Id { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime Start_Time { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string TaskName { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime End_Time { get; set; }

        ///// <summary>
        ///// bpm流程id
        ///// </summary>
        //[DataMember]
        //public string Bpm_Id { get; set; }

        ///// <summary>
        ///// BPM流程名称
        ///// </summary>
        //[DataMember]
        //public string Bpm_Name { get; set; }

        ///// <summary>
        ///// BPM编号
        ///// </summary>
        //[DataMember]
        //public string Bpm_Code { get; set; }

        /// <summary>
        /// 自定义表单ID
        /// </summary>
        [DataMember]
        public string TableId { get; set; }

        /// <summary>
        /// 是否执行成功 0：未成功  1：已成功
        /// </summary>
        [DataMember]
        public int IsSuccess { get; set; }

        /// <summary>
        ///  添加人
        /// </summary>
        [DataMember]
        public string Creator_Nm { get; set; }

        /// <summary>
        /// 添加人ID
        /// </summary>
        [DataMember]
        public string Creator_Id { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [DataMember]
        public string DepartmentId { get; set; }

        /// <summary>
        ///  部门名称
        /// </summary>
        [DataMember]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 处理人id(下一环节处理人 为空时不限)
        /// </summary>
        [DataMember]
        public string ExecPersonId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        [DataMember]
        public string ExecPersonName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [DataMember]
        public string TaskDescription { get; set; }
        /// <summary>
        /// 0:未完成 1:已完成
        /// </summary>
        [DataMember]
        public int IsFinish { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        public string TaskId { get; set; }
    }
}
