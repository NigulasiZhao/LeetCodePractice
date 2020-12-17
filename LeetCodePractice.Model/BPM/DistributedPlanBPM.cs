using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.BPM
{
    public class DistributedPlanBPM
    {
        /// <summary>
        /// 计划ID
        /// </summary>
        public string Plan_Id { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>
        public string Plan_Name { get; set; }

        /// <summary>
        /// 任务分类主键
        /// </summary>
        public string Task_Type_Id { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 巡检人员Id
        /// </summary>
        public string ExecPersonId { get; set; }
        /// <summary>
        /// 巡检人员名称
        /// </summary>
        public string ExecPersonName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDescription { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>		
        public string CreatorName { set; get; }
        /// <summary>
        /// 创建人id
        /// </summary>	
        public string CreatorId { set; get; }
        /// <summary>
        /// 备注
        /// </summary>	
        public string Remark { set; get; }
    }
}
