using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
   [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Form_Valve")]
    public class Ins_Form_Valve
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }

        [DataMember]
        public string TaskId { get; set; }
        [DataMember]
        public string TaskName { get; set; }
        [DataMember]
        public int IsExistProblem { get; set; }
        [DataMember]
        public string DetailAddress { get; set; }
        /// <summary>
        /// 是否覆盖
        /// </summary>
        [DataMember]
        public int IsCover { get; set; }
        /// <summary>
        /// 是否丢失
        /// </summary>
        [DataMember]
        public int IsLose { get; set; }
        /// <summary>
        /// 是否井盖破损
        /// </summary>
        [DataMember]
        public int IsDamage { get; set; }
        /// <summary>
        /// 是否当场整改
        /// </summary>
        [DataMember]
        public int IsRectification { get; set; }
        [DataMember]
        public string WorkCode { get; set; }
        [DataMember]
        public string GlobID { get; set; }
        [DataMember]
        public string LayerName { get; set; }
        [DataMember]

        public string X { get; set; }
        [DataMember]
        public string Y { get; set; }
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string Geometry { get; set; }
        [DataMember]
        public string Plan_task_id { get; set; }
        /// <summary>
        /// 是否填单
        /// </summary>
        [DataMember]
        public int IsFillForm { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
    }
}
