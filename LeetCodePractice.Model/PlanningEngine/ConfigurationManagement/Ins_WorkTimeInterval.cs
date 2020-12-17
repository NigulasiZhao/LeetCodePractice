using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.ConfigurationManagement
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_WorkTimeInterval")]
    public class Ins_WorkTimeInterval
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string WorkTimeID { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string Task_Name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime Start_Time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime End_Time { get; set; }

        /// <summary>
        /// 是否轨迹监控
        /// </summary>
        [DataMember]
        public string Is_monitor { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

       
    }
}
