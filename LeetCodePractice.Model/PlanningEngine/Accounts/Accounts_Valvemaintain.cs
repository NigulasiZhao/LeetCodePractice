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
    [System.ComponentModel.DataAnnotations.Schema.Table("Accounts_Valvemaintain")]
    /// <summary>
    ///阀门一般性维修记录表
    /// </summary>
    public class Accounts_Valvemaintain
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
        ///完成情况
        /// </summary>
        [DataMember]
        public int Iscomplete { get; set; }

        /// <summary>
        ///维修项目
        /// </summary>
        [DataMember]
        public string Maintaindetail { get; set; }

        /// <summary>
        ///维修日期
        /// </summary>
        [DataMember]
        public DateTime Maintaintime { get; set; }

        /// <summary>
        ///WTK格式
        /// </summary>
        [DataMember]
        public string Mi_Shape { get; set; }

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
        ///地址
        /// </summary>
        [DataMember]
        public string Valveaddress { get; set; }

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
        ///型号
        /// </summary>
        [DataMember]
        public string Valvetype { get; set; }
    }
}