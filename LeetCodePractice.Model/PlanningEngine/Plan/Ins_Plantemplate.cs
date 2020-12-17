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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plantemplate")]
    public class Ins_Plantemplate
    {
        /// <summary>
        /// 计划模板主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Planttemplate_id { get; set; }

        /// <summary>
        /// 模板大类id
        /// </summary>
        [DataMember]
        public string Plan_templatetypeid { get; set; }
        /// <summary>
        /// 计划模板名称
        /// </summary>
        [DataMember]
        public string PlantTemplate_name { get; set; }


        /// <summary>
        /// 创建人id
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsString)]

        public string Create_person_id { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsString)]
        public string Create_person_name { get; set; }

        /// <summary>
        ///设备类型集合
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<Ins_Plan_EquipmentForm> EquipmentFormGroup { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime Create_time { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]

        public DateTime Update_time { get; set; }
        /// <summary>
        /// 更新人id
        /// </summary>
        [DataMember]
        public string Update_person_id { get; set; }

        /// <summary>
        /// 更新人姓名
        /// </summary>
        [DataMember]
        public string Update_person_name { get; set; }
    }
}
