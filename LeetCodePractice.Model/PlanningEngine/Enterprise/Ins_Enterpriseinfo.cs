/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/11/25 11:26:28
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Enterprise
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Enterpriseinfo")]
    /// <summary>
    ///企业信息
    /// </summary>
    public class Ins_Enterpriseinfo
    {

        /// <summary>
        ///添加人部门ID
        /// </summary>
        [DataMember]
        public string Creatordepartmentid { get; set; }

        /// <summary>
        ///添加人部门名称
        /// </summary>
        [DataMember]
        public string Creatordepartmentname { get; set; }

        /// <summary>
        ///添加人ID
        /// </summary>
        [DataMember]
        public string Creatorid { get; set; }

        /// <summary>
        ///添加人名称
        /// </summary>
        [DataMember]
        public string Creatorname { get; set; }

        /// <summary>
        ///添加时间
        /// </summary>
        [DataMember]
        public DateTime Creatortime { get; set; }
        /// <summary>
        ///单位地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        ///法定代表人
        /// </summary>
        [DataMember]
        public string Behalfperson { get; set; }

        /// <summary>
        ///企业编码
        /// </summary>
        [DataMember]
        public string Enterprisecode { get; set; }

        /// <summary>
        ///企业资质
        /// </summary>
        [DataMember]
        public string Enterpriselevel { get; set; }

        /// <summary>
        ///企业名称
        /// </summary>
        [DataMember]
        public string Enterprisename { get; set; }

        /// <summary>
        ///单位性质
        /// </summary>
        [DataMember]
        public string Enterprisetype { get; set; }

        /// <summary>
        ///传真
        /// </summary>
        [DataMember]
        public string Fax { get; set; }

        /// <summary>
        ///主键ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }

        /// <summary>
        ///地理位置信息
        /// </summary>
        [DataMember]
        public string Mishape { get; set; }

        /// <summary>
        ///电话
        /// </summary>
        [DataMember]
        public string Tel { get; set; }
        /// <summary>
        ///其他
        /// </summary>
        [DataMember]
        public string Other { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
    }
}