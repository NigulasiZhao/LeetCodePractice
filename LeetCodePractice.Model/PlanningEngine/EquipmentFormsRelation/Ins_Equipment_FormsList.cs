using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.EquipmentFormsRelation
{
 public   class Ins_Equipment_FormsList
    {
        [DataMember]
        public string Equipment_Form_Id { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        [DataMember]
        public string LayerName { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        [DataMember]
        public string LayerCName { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public int ViewOrder { get; set; }

        /// <summary>
        /// BPM编号
        /// </summary>
        [DataMember]
        public string TableID { get; set; }
        /// <summary>
        /// 设备类型英文名称
        /// </summary>
        [DataMember]
        public string TableName { get; set; }
        /// <summary>
        /// 设备类型英文名称
        /// </summary>
        [DataMember]
        public string TableCode { get; set; }
      
    }
}
