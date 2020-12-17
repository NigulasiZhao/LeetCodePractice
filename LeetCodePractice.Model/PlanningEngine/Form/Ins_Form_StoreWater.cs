using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;


namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Form_StoreWater")]
    public class Ins_Form_StoreWater
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
        public string Plan_task_id { get; set; }
        /// <summary>
        /// 是否填单
        /// </summary>
        [DataMember]
        public int IsFillForm { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        /// <summary>
        /// 内、外设备
        /// </summary>  
        [DataMember]
        public decimal ExternalEqu
        {
            set; get;
        }
        /// <summary>
        /// 附属设备
        /// </summary>  
        [DataMember]
        public decimal SubsidiaryEqu
        {
            set; get;
        }
        /// <summary>
        /// 水位控制装置及其他测量仪
        /// </summary>  
        [DataMember]
        public decimal WaterLevel
        {
            set; get;
        }
        /// <summary>
        /// 油漆修补
        /// </summary>  
        [DataMember]
        public decimal PaintRepair
        {
            set; get;
        }


    }
}
