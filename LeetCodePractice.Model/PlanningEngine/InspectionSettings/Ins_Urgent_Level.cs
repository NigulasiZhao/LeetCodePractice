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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Urgent_Level")]
    public class Ins_Urgent_Level
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Urgent_Level_Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string UrgencyName { set; get; }
      
    }
}
