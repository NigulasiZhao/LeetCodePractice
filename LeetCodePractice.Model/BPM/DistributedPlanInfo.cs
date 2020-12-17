using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.BPM
{
    public class DistributedPlanInfo
    {
        /// <summary>
        /// 计划Id
        /// </summary>
        public string Plan_Id { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>
        public string Plan_Name { get; set; }
        /// <summary>
        /// 计划周期
        /// </summary>
        public string PlanCycleId { get; set; }
        /// <summary>
        /// 是否常规 0:常规 1:临时
        /// </summary>
        public int IsNormalPlan { get; set; }
        /// <summary>
        /// 是否反馈 0:需反馈 1::仅到位
        /// </summary>
        public int IsFeedBack { get; set; }
        /// <summary>
        /// 巡检计划类型主键
        /// </summary>
        public string Plan_Type_Id { get; set; }
        /// <summary>
        /// 巡检区域名称 网格区域时默认最后一个名称
        /// </summary>
        public string Range_Name { get; set; }
        /// <summary>
        /// 区域坐标信息
        /// </summary>
        public string Geometry { get; set; }
        /// <summary>
        /// 巡检方式1  车巡   2  徒步
        /// </summary>
        public int MoveType { get; set; }
        /// <summary>
        /// 计划模板大类id
        /// </summary>
        public string Plan_TemplateType_Id { get; set; }
        /// <summary>
        /// 计划模板id
        /// </summary>
        public string PlanTtemplate_Id { get; set; }
        /// <summary>
        /// 设备设施 英文名称
        /// </summary>
        public string LayerName { get; set; }
        ///// <summary>
        ///// bpm流程id
        ///// </summary>
        //public string Bpm_Id { get; set; }
        ///// <summary>
        ///// BPM流程名称
        ///// </summary>
        //public string Bpm_Name { get; set; }
        /// <summary>
        /// 设施名称
        /// </summary>
        public string LayerCName { get; set; }
        /// <summary>
        /// 设备类型id
        /// </summary>
        public string Plan_Equipment_Id { get; set; }
        /// <summary>
        /// 自定义表单ID
        /// </summary>
        public string TableId { get; set; }
        /// <summary>
        /// 设施信息表Id
        /// </summary>
        public string Equipment_Info_Id { get; set; }
        /// <summary>
        /// 设施Id
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
        public string GXGeometry { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public int EquType { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public string creator_nm { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? Start_Time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? End_Time { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Task_Name { get; set; }
        /// <summary>
        /// 添加人ID
        /// </summary>
        public string creator_id { get; set; }
        /// <summary>
        /// 添加人公司ID
        /// </summary>
        public string creator_gid { get; set; }
        /// <summary>
        /// 添加人公司名称
        /// </summary>
        public string creator_gnm { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 分派人员ID
        /// </summary>
        public string DistributedId { get; set; }
        /// <summary>
        /// 分派人员名称
        /// </summary>
        public string DistributedName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDescription { get; set; }
        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string Plan_Task_Id { get; set; }
    }
}
