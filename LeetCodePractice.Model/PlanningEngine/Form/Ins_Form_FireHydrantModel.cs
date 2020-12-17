using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
   [Serializable]
    public class Ins_Form_FireHydrantModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string TaskId { get; set; }
        [DataMember]
        public string TaskName { get; set; }
        [DataMember]
        public int IsExistProblem { get; set; }
        [DataMember]
        public string DetailAddress { get; set; }
        /// <summary>
        /// 是否覆盖
        /// </summary>
        [DataMember]
        public int IsMissCover { get; set; }
        /// <summary>
        /// 是否污损陈旧
        /// </summary>
        [DataMember]
        public int IsOldStain { get; set; }
        /// <summary>
        /// 是否过高过矮
        /// </summary>
        [DataMember]
        public int IsHeight { get; set; }
        /// <summary>
        /// 是否漏水
        /// </summary>
        [DataMember]
        public int IsleakWater { get; set; }
        /// <summary>
        /// 是否倾斜
        /// </summary>
        [DataMember]
        public int IsTilt { get; set; }
        /// <summary>
        /// 是否未订牌
        /// </summary>
        [DataMember]
        public int IsFixedBrand { get; set; }
        /// <summary>
        /// 是否当场整改
        /// </summary>
        [DataMember]
        public int IsRectification { get; set; }


        [DataMember]
        public string WorkCode { get; set; }
        [DataMember]
        public string GlobID { get; set; }
        [DataMember]
        public string LayerName { get; set; }
        [DataMember]
        public string X { get; set; }
        [DataMember]
        public string Y { get; set; }
        [DataMember]
        public string Plan_task_id { get; set; }
        /// <summary>
        /// 是否填单
        /// </summary>
        [DataMember]
        public int IsFillForm { get; set; }
        [DataMember]
        public string[] ImagePath { get; set; }
    }
}