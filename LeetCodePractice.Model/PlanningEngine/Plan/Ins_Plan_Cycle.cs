using GISWaterSupplyAndSewageServer.Model.AttributePack;
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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan_Cycle")]
    public class Ins_Plan_Cycle
    {
        /// <summary>
        /// 计划周期主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Plan_Cycle_Id { get; set; }

        /// <summary>
        /// 计划周期名称
        /// </summary>
        [DataMember]
        public string Plan_Cycle_Name { get; set; }

        /// <summary>
        /// 周期时长 如 1
        /// </summary>
        [DataMember]
        public string CycleTime { get; set; }

        /// <summary>
        /// 单位：天，周，月
        /// </summary>
        [DataMember]
        public string CycleUnit { get; set; }

        /// <summary>
        /// 频率
        /// </summary>
        [DataMember]
        public int CycleHz { get; set; }

        /// <summary>
        /// 是否删除 0未删除 1删除
        /// </summary>
        [DataMember]
        public int DeleteState { get; set; }
    }
}
