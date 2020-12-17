using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.InspectionSettings
{

    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Task_CompleteDetail")]
    public class Ins_Task_CompleteDetail
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { set; get; }
        [DataMember]
        public string TaskId { set; get; }
        [DataMember]
        public string Devicename { set; get; }
        [DataMember]
        public string Devicesmid { set; get; }
        [DataMember]
        public DateTime Uptime { set; get; }
        [DataMember]
        public string x { set; get; }
        [DataMember]
        public string y { set; get; }
        [DataMember]
        public string Peopleid { set; get; }
        [DataMember]
        public string ImagePath { set; get; }
        [DataMember]
        public string Address { set; get; }
        [DataMember]
        public string Describe { set; get; }
        [DataMember]
        public int PointType { set; get; }
       
        [DataMember]
        public int IsHidden { set; get; }
        [DataMember]
        public int IsFeedback { set; get; }
       
        [DataMember]
        public string plan_task_id { set; get; }
    }
}
