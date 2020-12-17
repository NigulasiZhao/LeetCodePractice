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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Enterprisebusiness")]
    /// <summary>
    ///企业业务信息
    /// </summary>
    public class Ins_Enterprisebusiness
    {

        /// <summary>
        ///添加人部门ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsGuid)]
        public string Creatordepartmentid { get; set; }

        /// <summary>
        ///添加人部门名称
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsString)]
        public string Creatordepartmentname { get; set; }

        /// <summary>
        ///添加人ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsGuid)]
        public string Creatorid { get; set; }

        /// <summary>
        ///添加人名称
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsString)]
        public string Creatorname { get; set; }

        /// <summary>
        ///添加时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime Creatortime { get; set; }
        /// <summary>
        ///地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        ///考评编号
        /// </summary>
        [DataMember]
        public string Checkcode { get; set; }

        /// <summary>
        ///考评内容
        /// </summary>
        [DataMember]
        public string Checkcontent { get; set; }

        /// <summary>
        ///考评日期
        /// </summary>
        [DataMember]
        public DateTime Checkdate { get; set; }

        /// <summary>
        ///考评分数
        /// </summary>
        [DataMember]
        public decimal Checkscore { get; set; }

        /// <summary>
        ///考核单位
        /// </summary>
        [DataMember]
        public string Checkunit { get; set; }

        /// <summary>
        ///企业ID
        /// </summary>
        [DataMember]
        public string Enterpriseid { get; set; }

        /// <summary>
        ///主键ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }

        /// <summary>
        ///监管单位
        /// </summary>
        [DataMember]
        public string Monitoringunit { get; set; }

        /// <summary>
        ///工程名称
        /// </summary>
        [DataMember]
        public string Projectname { get; set; }
    }
}