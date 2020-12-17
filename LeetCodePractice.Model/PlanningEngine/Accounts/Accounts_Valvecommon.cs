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
    [System.ComponentModel.DataAnnotations.Schema.Table("Accounts_Valvecommon")]
    /// <summary>
    ///阀门普查工作情况表
    /// </summary>
    public class Accounts_Valvecommon
    {


        /// <summary>
        ///检查人ID
        /// </summary>
        [DataMember]
        public string Checkpersonid { get; set; }

        /// <summary>
        ///检查人名称
        /// </summary>
        [DataMember]
        public string Checkpersonname { get; set; }

        /// <summary>
        ///检查时间
        /// </summary>
        [DataMember]
        public DateTime Checktime { get; set; }

        /// <summary>
        ///编号
        /// </summary>
        [DataMember]
        public string Globid { get; set; }

        /// <summary>
        ///主键ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }

        /// <summary>
        ///WTK格式
        /// </summary>
        [DataMember]
        public string Mi_Shape { get; set; }
        /// <summary>
        ///巡检照片
        /// </summary>
        [DataMember]
        public string Photos { get; set; }

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
        ///系统时间
        /// </summary>
        [DataMember]
        public DateTime Systemtime { get; set; }

        /// <summary>
        ///阀门地址
        /// </summary>
        [DataMember]
        public string Valveaddress { get; set; }

        /// <summary>
        ///阀门口径
        /// </summary>
        [DataMember]
        public string Valvecaliber { get; set; }

        /// <summary>
        ///阀门编号
        /// </summary>
        [DataMember]
        public string Valvecode { get; set; }

        /// <summary>
        ///阀门及阀门井情况
        /// </summary>
        [DataMember]
        public string Valvedetail { get; set; }

        /// <summary>
        ///级别
        /// </summary>
        [DataMember]
        public string Valvelevel { get; set; }

        /// <summary>
        ///阀门运行状态 0正常，1不正常
        /// </summary>
        [DataMember]
        public int Valverunstate { get; set; }

        /// <summary>
        ///阀门开关状态 0开 1关 2未知
        /// </summary>
        [DataMember]
        public int Valveswitchstate { get; set; }
    }
}