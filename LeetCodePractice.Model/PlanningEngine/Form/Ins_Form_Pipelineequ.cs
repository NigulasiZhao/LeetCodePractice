/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/10/29 9:12:22
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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Form_Pipelineequ")]
    /// <summary>
    ///共用管道及附属设备
    /// </summary>
    public class Ins_Form_Pipelineequ
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
        ///自动排气阀
        /// </summary>
        [DataMember]
        public int Automaticpqvalve { get; set; }

        /// <summary>
        ///减压阀(高层建筑)
        /// </summary>
        [DataMember]
        public int Jpressurevalve { get; set; }

        /// <summary>
        ///减压阀前过滤器
        /// </summary>
        [DataMember]
        public int Jpressurevalvegl { get; set; }

        /// <summary>
        ///油漆修补
        /// </summary>
        [DataMember]
        public int PaintRepair { get; set; }

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


    }
}