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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan_Equipment")]
    public class Ins_Plan_Equipment
    {
        /// <summary>
        /// 计划设备主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Plan_Equipment_Id { get; set; }

        /// <summary>
        /// 巡检计划主键
        /// </summary>
        [DataMember]
        public string Plan_Id { get; set; }

        /// <summary>
        /// 设备设施 英文名称
        /// </summary>
        [DataMember]
        public string LayerName { get; set; }

        /// <summary>
        /// 表单对应表名称
        /// </summary>
        [DataMember]
        public string TableId { get; set; }
        /// <summary>
        /// 高级搜索
        /// </summary>
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string seniorSearch { get; set; }

        /// <summary>
        /// 是否自动填单
        /// </summary>
        [DataMember]
        public int IsFillForm { get; set; }

        /// <summary>
        /// 设备信息列表
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<Ins_Plan_Equipment_Info> Ins_Plan_Equipment_InfoList { get; set; }
    }
}
