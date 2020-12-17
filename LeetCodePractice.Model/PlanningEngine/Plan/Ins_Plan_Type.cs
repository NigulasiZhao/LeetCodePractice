using GISWaterSupplyAndSewageServer.Model.AttributePack;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Plan
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan_Type")]
    public class Ins_Plan_Type
    {
        /// <summary>
        /// 巡检类型主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Plan_Type_Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Plan_Type_Name { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        [DataMember]
        public string ParentTypeId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        public string Operater { get; set; }

        /// <summary>
        /// 设备类型列表
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<Ins_Range> Ins_RangeList { get; set; }

    }
}
