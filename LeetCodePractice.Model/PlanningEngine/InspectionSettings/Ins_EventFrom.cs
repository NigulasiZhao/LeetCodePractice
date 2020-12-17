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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_EventFrom")]
    public class Ins_EventFrom
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string EventFromId { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string EventFromName { set; get; }

    }
}
