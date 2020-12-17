using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
    public class GetTaskDetailInfoDto
    {
        /// <summary>
        /// 计划任务id
        /// </summary>
        public string Plan_Task_Id { get; set; }

        /// <summary>
        /// 设施信息id
        /// </summary>
        public string Equipment_Info_Id { get; set; }

        /// <summary>
        /// 计划id
        /// </summary>
        public string Plan_Id { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Start_Time { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Task_Name { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime End_Time { get; set; }
        /// <summary>
        /// 自定义表单ID
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 是否执行成功 0：未成功  1：已成功
        /// </summary>
        public int IsSuccess { get; set; }

        /// <summary>
        ///  添加人
        /// </summary>
        public string Creator_Nm { get; set; }

        /// <summary>
        /// 添加人ID
        /// </summary>
        public string Creator_Id { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        ///  部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 处理人id(下一环节处理人 为空时不限)
        /// </summary>
        public string ExecPersonId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string ExecPersonName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDescription { get; set; }
        /// <summary>
        /// 0:未完成 1:已完成
        /// </summary>
        public int IsFinish { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 计划设备主键
        /// </summary>
        public string Plan_Equipment_Id { get; set; }
        /// <summary>
        /// 设施id
        /// </summary>
        public string GlobID { get; set; }
        /// <summary>
        /// 设施编号
        /// </summary>
        public string Equipment_Info_Code { get; set; }

        /// <summary>
        /// 设施名称
        /// </summary>
        public string Equipment_Info_Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 口径
        /// </summary>
        public int Caliber { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create_Time { get; set; }
    }
}
