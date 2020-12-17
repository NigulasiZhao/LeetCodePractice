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
    [System.ComponentModel.DataAnnotations.Schema.Table("Accounts_Valvedaily")]
    /// <summary>
    ///阀门日常巡查记录表
    /// </summary>
    public class Accounts_Valvedaily
    {


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
        ///其他问题
        /// </summary>
        [DataMember]
        public string Otherquestion { get; set; }

        /// <summary>
        ///照片
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
        ///所在道路
        /// </summary>
        [DataMember]
        public string Valveaddress { get; set; }

        /// <summary>
        ///传动箱情况
        /// </summary>
        [DataMember]
        public string Valveboxdetail { get; set; }

        /// <summary>
        ///口径
        /// </summary>
        [DataMember]
        public string Valvecaliber { get; set; }

        /// <summary>
        ///阀门编号
        /// </summary>
        [DataMember]
        public string Valvecode { get; set; }

        /// <summary>
        ///信息登记与现场情况
        /// </summary>
        [DataMember]
        public string Valvedetail { get; set; }

        /// <summary>
        ///是否完好 0否 1是
        /// </summary>
        [DataMember]
        public int Valveiscomplete { get; set; }

        /// <summary>
        ///是否漏水 0否1是
        /// </summary>
        [DataMember]
        public int Valveisleakage { get; set; }

        /// <summary>
        ///阀门井盖高低不平 0否 1是
        /// </summary>
        [DataMember]
        public int Valveisstandard { get; set; }

        /// <summary>
        ///全开/全关 0全开1全关
        /// </summary>
        [DataMember]
        public int Valveswitchstate { get; set; }

        /// <summary>
        ///阀门类型
        /// </summary>
        [DataMember]
        public string Valvetype { get; set; }
    }
}