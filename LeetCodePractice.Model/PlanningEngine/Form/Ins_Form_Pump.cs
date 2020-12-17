/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/10/29 9:22:20
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Form_Pump")]
    /// <summary>
    ///泵房
    /// </summary>
    public class Ins_Form_Pump
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }

        [DataMember]
        public string TaskId { get; set; }
        [DataMember]
        public string TaskName { get; set; }
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
        public string ImagePath { get; set; }

        /// <summary>
        ///电控柜
        /// </summary>
        [DataMember]
        public int Electric { get; set; }
        /// <summary>
        ///测量仪表
        /// </summary>
        [DataMember]
        public int Measuremeter { get; set; }

        /// <summary>
        ///电动机
        /// </summary>
        [DataMember]
        public int Motor { get; set; }

        /// <summary>
        ///电动机与控制柜之间的电源
        /// </summary>
        [DataMember]
        public int Motorpowersupply { get; set; }


        /// <summary>
        ///油漆修补
        /// </summary>
        [DataMember]
        public int PaintRepair { get; set; }



        /// <summary>
        ///排污设施
        /// </summary>
        [DataMember]
        public int Sewageequ { get; set; }



        /// <summary>
        ///机组基础
        /// </summary>
        [DataMember]
        public int Unitfoundation { get; set; }

        /// <summary>
        ///各类管道
        /// </summary>
        [DataMember]
        public int Variouspipe { get; set; }

        /// <summary>
        ///各类阀门
        /// </summary>
        [DataMember]
        public int Variousvalve { get; set; }

        /// <summary>
        ///水泵
        /// </summary>
        [DataMember]
        public int Waterpump { get; set; }


    }
}