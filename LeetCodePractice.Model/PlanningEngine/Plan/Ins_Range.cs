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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Range")]
    public class Ins_Range
    {
        /// <summary>
        /// 巡检范围主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Range_id { get; set; }

        /// <summary>
        /// 范围名称
        /// </summary>
        [DataMember]
        public string Range_name { get; set; }

        /// <summary>
        /// 1:polygon 多边形 2:string
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// 范围gis信息
        /// </summary>
        [DataMember]
        public string Shape { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        [DataMember]
        public string Department_name { get; set; }
        /// <summary>
        /// 责任部门ID
        /// </summary>
        [DataMember]
        public string DeptId { get; set; }
        /// <summary>
        /// 区域负责人
        /// </summary>
        [DataMember]
        public string Person_name { get; set; }

    
        /// <summary>
        /// 区域负责人ID
        /// </summary>
        [DataMember]
        public string PersonId { get; set; }
        /// <summary>
        /// 巡检范围父节点
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public string Range_parentid { get; set; }

        
    }
}