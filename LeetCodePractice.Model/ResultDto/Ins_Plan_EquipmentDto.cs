using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
   public class Ins_Plan_EquipmentDto
    {
        /// <summary>
        /// 计划设备主键
        /// </summary>
        public string Plan_Equipment_Id { get; set; }

        /// <summary>
        /// 巡检计划主键
        /// </summary>
        public string Plan_Id { get; set; }

        /// <summary>
        /// 设备设施 英文名称
        /// </summary>
     
        public string LayerName { get; set; }

        /// <summary>
        /// 表单对应表名称
        /// </summary>
        public string TableId { get; set; }
        public string TableName { get; set; }
        public string seniorSearch { get; set; }
        /// <summary>
        /// 设备信息列表
        /// </summary>
        public List<Ins_Plan_Equipment_Info> Ins_Plan_Equipment_InfoList { get; set; }
    }
}
