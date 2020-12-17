using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan
{
   public class Ins_Plan_EquipmentFormList
    {
        /// <summary>
        /// 计划模板主键
        /// </summary>
        [DataMember]
        public string Planttemplate_id { get; set; }
        /// <summary>
        /// 设备实体英文名称
        /// </summary>
        [DataMember]
        public string LayerName { get; set; }
        /// <summary>
        /// 设施与自定义表单挂接表主键
        /// </summary>
        [DataMember]
        public string Equipment_Form_Id { get; set; }
        /// <summary>
        /// 设备实体中文名称
        /// </summary>
        [DataMember]
        public string LayerCName { get; set; }
        /// <summary>
        /// 自定义表单id
        /// </summary>
        [DataMember]
        public string TableId { get; set; }
  
        [DataMember]
        public string TableName { get; set; }
        [DataMember]
        public string TableCode { get; set; }
        /// <summary>
        /// 中文高级搜索where
        /// </summary>
        [DataMember]
        public string CNameWhere { get; set; }
        /// <summary>
        /// 英文高级搜索where
        /// </summary>
        [DataMember]
        public string NameWhere { get; set; }
        /// <summary>
        /// 高级搜索
        /// </summary>
        [DataMember]
        public string seniorSearch { get; set; }
        /// <summary>
        /// 是否自动填充
        /// </summary>
        [DataMember]
        public string IsFillForm { get; set; }
    }
}
