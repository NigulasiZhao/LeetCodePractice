using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan
{
   public class Ins_PlantemplateList
    {
        public string Plan_templatetype_id { get; set; }
        public string Templatetype_code { get; set; }
        public string Templatetype_name { get; set; }
        /// <summary>
        /// 计划模板主键
        /// </summary>
        public string Planttemplate_id { get; set; }

        /// <summary>
        /// 计划模板名称
        /// </summary>
        [DataMember]
        public string PlantTemplate_name { get; set; }


        /// <summary>
        /// 创建人id
        /// </summary>
        [DataMember]
        public string Create_person_id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string LayerNames { get; set; }


        /// <summary>
        /// 表单名称
        /// </summary>
        [DataMember]
        public string TableNames { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DataMember]
        public string Create_person_name { get; set; }

    
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime Create_time { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
    

        public DateTime? Update_time { get; set; }
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
        /// <summary>
        ///设备类型集合
        /// </summary>
        [DataMember]
        public List<Ins_Plan_EquipmentFormList> EquipmentFormGroup { get; set; }


    }
}
