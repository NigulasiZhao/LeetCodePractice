/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/2 17:04:16
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.CustomCondition
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Customconditions")]
    /// <summary>
    ///用户自定义查询条件
    /// </summary>
    public class Ins_Customconditions
    {


        /// <summary>
        ///自定义条件内容
        /// </summary>
        [DataMember]
        public string Conditiondetail { get; set; }

        /// <summary>
        ///添加时间
        /// </summary>
        [DataMember]
        public DateTime Createtime { get; set; }

        /// <summary>
        ///功能ID
        /// </summary>
        [DataMember]
        public string Functionid { get; set; }

        /// <summary>
        ///用户自定义查询条件表主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }

        /// <summary>
        ///用户ID
        /// </summary>
        [DataMember]
        public string Userid { get; set; }

        /// <summary>
        ///条件名称
        /// </summary>
        [DataMember]
        public string ConditionName { get; set; }
    }
}