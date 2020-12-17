using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan_EquipmentForm")]
    public class Ins_Plan_EquipmentForm
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
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
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
        /// <summary>
        /// 是否自动填单
        /// </summary>
        [DataMember]
        public int IsFillForm { get; set; }
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
        [Column(DataType = DataType.IsClob)]
        public string seniorSearch { get; set; }
    }
}
