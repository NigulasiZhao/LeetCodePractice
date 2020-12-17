/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/7/3 15:30:38
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.Model.Plan
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_P_Position")]
    /// <summary>
    ///
    /// </summary>
    public class Ins_P_Position
    {
        /// <summary>
        ///上传人id
        /// </summary>
        [DataMember]
        public string Personid { get; set; }

        /// <summary>
        ///上传人
        /// </summary>
        [DataMember]
        public string Personname { get; set; }

        /// <summary>
        ///主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Positionid { get; set; }

        /// <summary>
        ///横坐标
        /// </summary>
        [DataMember]
        public string Positionx { get; set; }

        /// <summary>
        ///纵坐标
        /// </summary>
        [DataMember]
        public string Positiony { get; set; }

        /// <summary>
        ///任务id
        /// </summary>
        [DataMember]
        public string Taskid { get; set; }

        /// <summary>
        ///上传时间
        /// </summary>
        [DataMember]
        public DateTime Uptime { get; set; }
    }
}