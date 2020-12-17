/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/3 16:02:14
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.CustomCondition
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("T_Customconditions")]
    /// <summary>
    ///用户查询条件
    /// </summary>
    public class T_Customconditions
    {


        /// <summary>
        ///添加时间
        /// </summary>
        [DataMember]
        public DateTime Createtime { get; set; }

        /// <summary>
        ///主键ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        ///条件详情
        /// </summary>
        [DataMember]
        public string Searchconditon { get; set; }

        /// <summary>
        ///名称
        /// </summary>
        [DataMember]
        public string Searchname { get; set; }

        /// <summary>
        ///类型
        /// </summary>
        [DataMember]
        public int Searchtype { get; set; }

        /// <summary>
        ///用户ID
        /// </summary>
        [DataMember]
        public string Userid { get; set; }
    }
}