using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;


namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
    [Serializable]
    [DataContract]

    public class Ins_Form_LeakDetection
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string TaskId { get; set; }
        [DataMember]
        public string TaskName { get; set; }
        [DataMember]
        public string Plan_task_id { get; set; }
        [DataMember]
        public string LayerName { get; set; }
        /// <summary>
        /// 是否填单
        /// </summary>
        [DataMember]
        public int IsFillForm { get; set; }
        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public string WorkCode { get; set; }
        [DataMember]
        public string Caliber { get; set; }
        [DataMember]
        public string Remark { get; set; }

    }
}
