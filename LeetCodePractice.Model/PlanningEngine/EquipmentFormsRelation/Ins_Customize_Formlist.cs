using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.EquipmentFormsRelation
{
   [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Customize_Formlist")]
    public class Ins_Customize_Formlist
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Formlist_id { get; set; }

        /// <summary>
        /// 表单表名
        /// </summary>
        [DataMember]
        public string TableID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string TableName { get; set; }

        /// <summary>
        /// 表单表编号
        /// </summary>
        [DataMember]
        public string TableCode { get; set; }
    }
}
