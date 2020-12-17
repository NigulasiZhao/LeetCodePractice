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
    [System.ComponentModel.DataAnnotations.Schema.Table("Accounts_Pipescrapdaily")]
    /// <summary>
    ///管线拆除、报废登记表
    /// </summary>
    public class Accounts_Pipescrapdaily
    {


        /// <summary>
        ///审核部领导
        /// </summary>
        [DataMember]
        public string Auditleader { get; set; }

        /// <summary>
        ///审核部领导签字时间
        /// </summary>
        [DataMember]
        public DateTime Auditleaderqzdate { get; set; }

        /// <summary>
        ///审核部审核人
        /// </summary>
        [DataMember]
        public string Auditreviewer { get; set; }

        /// <summary>
        ///审核部审核人签字时间
        /// </summary>
        [DataMember]
        public DateTime Auditreviewerqzdate { get; set; }

        /// <summary>
        ///公司经理
        /// </summary>
        [DataMember]
        public string Companymanager { get; set; }

        /// <summary>
        ///公司经理签字时间
        /// </summary>
        [DataMember]
        public DateTime Companymanagerqzdate { get; set; }

        /// <summary>
        ///施工单位
        /// </summary>
        [DataMember]
        public string Constructioncompany { get; set; }

        /// <summary>
        ///填表单位
        /// </summary>
        [DataMember]
        public string Fillingcompany { get; set; }

        /// <summary>
        ///填表日期
        /// </summary>
        [DataMember]
        public DateTime Fillingdate { get; set; }

        /// <summary>
        ///财务部经办人
        /// </summary>
        [DataMember]
        public string Financeagent { get; set; }

        /// <summary>
        ///财务部经办人签字时间
        /// </summary>
        [DataMember]
        public DateTime Financeagentqzdate { get; set; }

        /// <summary>
        ///财务部领导
        /// </summary>
        [DataMember]
        public string Financeleader { get; set; }

        /// <summary>
        ///财务部领导签字时间
        /// </summary>
        [DataMember]
        public DateTime Financeleaderqzdate { get; set; }

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
        ///安装地点
        /// </summary>
        [DataMember]
        public string Installplace { get; set; }

        /// <summary>
        ///WTK格式
        /// </summary>
        [DataMember]
        public string Mi_Shape { get; set; }

        /// <summary>
        ///原值(元)
        /// </summary>
        [DataMember]
        public string Originalvalue { get; set; }

        /// <summary>
        ///原管总长
        /// </summary>
        [DataMember]
        public string Pipechief { get; set; }

        /// <summary>
        ///管线编号
        /// </summary>
        [DataMember]
        public string Pipecode { get; set; }

        /// <summary>
        ///原竣工日期
        /// </summary>
        [DataMember]
        public DateTime Primary_Completeddate { get; set; }

        /// <summary>
        ///原工程档号
        /// </summary>
        [DataMember]
        public string Primary_Projectcode { get; set; }

        /// <summary>
        ///原工程档号
        /// </summary>
        [DataMember]
        public string Primary_Projectreference { get; set; }

        /// <summary>
        ///工程编号
        /// </summary>
        [DataMember]
        public string Projectcode { get; set; }

        /// <summary>
        ///工程名称
        /// </summary>
        [DataMember]
        public string Projectname { get; set; }

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
        ///原因
        /// </summary>
        [DataMember]
        public string Reason { get; set; }

        /// <summary>
        ///报废口径
        /// </summary>
        [DataMember]
        public string Scrapcaliber { get; set; }

        /// <summary>
        ///废管或拆除
        /// </summary>
        [DataMember]
        public string Scrapdismantle { get; set; }

        /// <summary>
        ///报废数量
        /// </summary>
        [DataMember]
        public string Scrapnum { get; set; }

        /// <summary>
        ///报废意见
        /// </summary>
        [DataMember]
        public string Scrapyj { get; set; }

        /// <summary>
        ///报废原值
        /// </summary>
        [DataMember]
        public string Scrapyz { get; set; }

        /// <summary>
        ///报废折旧
        /// </summary>
        [DataMember]
        public string Scrapzj { get; set; }

   

        /// <summary>
        ///使用单位经办人
        /// </summary>
        [DataMember]
        public string Sycompanyjbr { get; set; }

        /// <summary>
        ///使用单位领导
        /// </summary>
        [DataMember]
        public string Sycompanyld { get; set; }

        /// <summary>
        ///使用单位领导签字时间
        /// </summary>
        [DataMember]
        public DateTime Sycompanyqzdate { get; set; }

        /// <summary>
        ///使用单位意见
        /// </summary>
        [DataMember]
        public string Sycompanyyj { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>
        [DataMember]
        public DateTime Systemtime { get; set; }

        /// <summary>
        ///单元(元/m)
        /// </summary>
        [DataMember]
        public string Unitprice { get; set; }

        /// <summary>
        ///主要安装规格
        /// </summary>
        [DataMember]
        public string Zyinstallgg { get; set; }
    }
}