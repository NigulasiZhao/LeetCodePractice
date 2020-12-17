using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
  public  class Ins_PlanDto
    {
        public string Plan_Id { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>
        public string Plan_Name { get; set; }
        public string Plan_Type_Id { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>
        public string Plan_Type_Name { get; set; }
        public string Assign_State { get; set; }
        public string Assign_StateName { get; set; }
        
        public string Plan_Templatetype_Id { get; set; }
        /// <summary>
        /// 模板分类
        /// </summary>
        public string Templatetype_Name { get; set; }

        public string Range_Name { get; set; }
        public string Range_Id { get; set; }
        /// <summary>
        /// 是否常规 巡检类型
        /// </summary>
        public string IsnormalPlan { get; set; }
        public string IsnormalPlanName { get; set; }
        public string Planttemplate_Id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Planttemplate_Name { get; set; }
        public string Plan_Cycle_Id { get; set; }
        /// <summary>
        /// 计划周期
        /// </summary>
        public string Plan_Cycle_Name { get; set; }
        /// <summary>
        /// 是否反馈
        /// </summary>
        public string IsFeedBack { get; set; }
        public string IsFeedBackName { get; set; }

        /// <summary>
        /// 巡检方式
        /// </summary>
        public string MoveType { get; set; }
        public string MoveTypeName { get; set; }
       public string Create_Person_Id { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Create_Person_Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create_Time { get; set; }
        /// <summary>
        /// 工单数
        /// </summary>
        public string SumCount { get; set; }
        public string Geometry { get; set; }
        public int Assignstate { get; set; }
        /// <summary>
        /// 设备类型列表
        /// </summary>

        public List<Ins_Plan_EquipmentDto> Ins_Plan_EquipmentList { get; set; }
    }
}
