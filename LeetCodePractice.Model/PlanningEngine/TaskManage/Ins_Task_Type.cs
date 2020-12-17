using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.TaskManage
{
  [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Task_Type")]
    public class Ins_Task_Type
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Task_type_id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public string Task_type_code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Task_type_name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        [DataMember]
        public string ParentTypeId { get; set; }


    }
}
