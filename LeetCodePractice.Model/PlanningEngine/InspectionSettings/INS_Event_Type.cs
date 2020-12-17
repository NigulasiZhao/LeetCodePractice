using GisPlateform.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace GisPlateformForCore.Model.PlanningEngine.InspectionSettings
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Event_Type")]
   public class Ins_Event_Type
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Event_Type_id { set; get; }
        /// <summary>
        /// 事件名称
        /// </summary>
        [DataMember]
        public string EventTypeName { set; get; }
        [DataMember]
        public int? ExecTime { set; get; }
        [DataMember]
        public string ParentTypeId { set; get; }
    }
}
