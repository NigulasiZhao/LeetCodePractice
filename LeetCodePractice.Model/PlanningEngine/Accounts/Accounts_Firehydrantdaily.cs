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
    [System.ComponentModel.DataAnnotations.Schema.Table("Accounts_Firehydrantdaily")]
    /// <summary>
    ///消火栓日常巡查记录表
    /// </summary>
    public class Accounts_Firehydrantdaily
    {


        /// <summary>
        ///所在道路
        /// </summary>
        [DataMember]
        public string Firehydrantaddress { get; set; }

        /// <summary>
        ///设备编号
        /// </summary>
        [DataMember]
        public string Firehydrantcode { get; set; }

        /// <summary>
        ///厂家
        /// </summary>
        [DataMember]
        public string Firehydrantvender { get; set; }

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
        ///是否完好 0否1是
        /// </summary>
        [DataMember]
        public int Iscomplete { get; set; }

        /// <summary>
        ///是否污损 0否1是
        /// </summary>
        [DataMember]
        public int Isdirty { get; set; }

        /// <summary>
        ///是否补订牌 0否1是
        /// </summary>
        [DataMember]
        public int Isfillbrand { get; set; }

        /// <summary>
        ///是否未钉牌 0否1是
        /// </summary>
        [DataMember]
        public int Isfixedbrand { get; set; }

        /// <summary>
        ///是否过高 0否1是
        /// </summary>
        [DataMember]
        public int Isheight { get; set; }

        /// <summary>
        ///是否缺连接扣 0否1是
        /// </summary>
        [DataMember]
        public int Islackconnect { get; set; }

        /// <summary>
        ///是否漏水 0否1是
        /// </summary>
        [DataMember]
        public int Isleakwater { get; set; }

        /// <summary>
        ///是否缺盖 0否1是
        /// </summary>
        [DataMember]
        public int Ismisscover { get; set; }

        /// <summary>
        ///是否陈旧 0否1是
        /// </summary>
        [DataMember]
        public int Isold { get; set; }

        /// <summary>
        ///是否过矮 0否1是
        /// </summary>
        [DataMember]
        public int Isshort { get; set; }

        /// <summary>
        ///是否倾斜 0否1是
        /// </summary>
        [DataMember]
        public int Istilt { get; set; }

        /// <summary>
        ///是否位置不合理 0否1是
        /// </summary>
        [DataMember]
        public int Isunlocation { get; set; }

        /// <summary>
        ///WTK格式
        /// </summary>
        [DataMember]
        public string Mi_Shape { get; set; }

        /// <summary>
        ///其他问题
        /// </summary>
        [DataMember]
        public string Otherqueation { get; set; }

        /// <summary>
        ///出水时间
        /// </summary>
        [DataMember]
        public DateTime Outwatertime { get; set; }

        /// <summary>
        ///照片
        /// </summary>
        [DataMember]
        public string Photos { get; set; }

        /// <summary>
        ///压力
        /// </summary>
        [DataMember]
        public string Pressure { get; set; }

        /// <summary>
        ///行政区域名称
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
    }
}