/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/10/13 16:43:30
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Accounts_Pipeinspectiondaily")]
    /// <summary>
    ///高风险管段巡查记录表
    /// </summary>
    public class Accounts_Pipeinspectiondaily
    {


        /// <summary>
        ///发现问题详细地址
        /// </summary>
        [DataMember]
        public string Detailaddress { get; set; }

        /// <summary>
        ///管线编号
        /// </summary>
        [DataMember]
        public string Globid { get; set; }

        /// <summary>
        ///主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }

        /// <summary>
        ///巡查时间
        /// </summary>
        [DataMember]
        public DateTime Inspectiondate { get; set; }

        /// <summary>
        ///巡查人
        /// </summary>
        [DataMember]
        public string Inspector { get; set; }

        /// <summary>
        ///是否暴漏0:否 1：是
        /// </summary>
        [DataMember]
        public int Isexplose { get; set; }

        /// <summary>
        ///是否当场整改
        /// </summary>
        [DataMember]
        public int Isrectification { get; set; }

        /// <summary>
        ///是否骑压 0:否 1：是
        /// </summary>
        [DataMember]
        public int Isridingpressure { get; set; }

        /// <summary>
        ///WTK格式
        /// </summary>
        [DataMember]
        public string Mi_Shape { get; set; }

        /// <summary>
        ///管线编号
        /// </summary>
        [DataMember]
        public string Pipecode { get; set; }

        /// <summary>
        ///区域名称
        /// </summary>
        [DataMember]
        public string Rangename { get; set; }

        /// <summary>
        ///分公司区域名称
        /// </summary>
        [DataMember]
        public string Rangename_Fgs { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        ///路线名称
        /// </summary>
        [DataMember]
        public string Routename { get; set; }

       

        /// <summary>
        ///系统时间
        /// </summary>
        [DataMember]
        public DateTime Systemtime { get; set; }

        /// <summary>
        ///工单编号
        /// </summary>
        [DataMember]
        public string Workcode { get; set; }
    }
}