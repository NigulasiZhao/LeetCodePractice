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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan_Templatetype")]
    public class Ins_Plan_Templatetype
    {
        /// <summary>
        /// 计划模板主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Plan_templatetype_id { get; set; }

        /// <summary>
        /// 模板编号
        /// </summary>
        [DataMember]
        public string Templatetype_code { get; set; }
        /// <summary>
        /// 模板大类名称
        /// </summary>
        [DataMember]
        public string Templatetype_name { get; set; }


        /// <summary>
        /// 父节点
        /// </summary>
        [DataMember]
        public string Templatetype_parentid { get; set; }
      

    }
}
