using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan
{
  [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Range_Point")]
    public class Ins_Range_Point
    {
        /// <summary>
        /// 巡检范围关键点主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Range_point_id { get; set; }

        /// <summary>
        /// 巡检范围主键
        /// </summary>
        [DataMember]
        public string Range_id { get; set; }
        /// <summary>
        /// 关键点名称
        /// </summary>
        [DataMember]
        public string Range_point_name { get; set; }

        
        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public string Lon { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public string Lat { get; set; }
       
    }
}