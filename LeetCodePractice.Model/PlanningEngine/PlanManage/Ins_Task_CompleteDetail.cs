/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/7/2 14:27:37
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Task_Completedetail")]
    /// <summary>
    ///
    /// </summary>
    public class Ins_Task_Completedetail
    {


        /// <summary>
        ///详细地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        ///详细描述
        /// </summary>
        [DataMember]
        public string Describe { get; set; }

        /// <summary>
        ///设备名称
        /// </summary>
        [DataMember]
        public string Devicename { get; set; }

        /// <summary>
        ///设备编号
        /// </summary>
        [DataMember]
        public string Devicesmid { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public string Equtype { get; set; }

        /// <summary>
        ///主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Id { get; set; }

        /// <summary>
        ///图片地址|分割
        /// </summary>
        [DataMember]
        public string Imagepath { get; set; }

        /// <summary>
        ///是否已反馈0:未反馈1:已反馈
        /// </summary>
        [DataMember]
        public int Isfeedback { get; set; }

        /// <summary>
        ///0:无隐患 1:有隐患
        /// </summary>
        [DataMember]
        public int Ishidden { get; set; }

        /// <summary>
        ///用户id
        /// </summary>
        [DataMember]
        public string Peopleid { get; set; }

        /// <summary>
        ///用户名
        /// </summary>
        [DataMember]
        public string Peoplename { get; set; }

        /// <summary>
        ///任务明细id(关联任务明细表)
        /// </summary>
        [DataMember]
        public string Plan_Task_Id { get; set; }

        /// <summary>
        ///0:设备实体1:区域点关键2:路线关键点
        /// </summary>
        [DataMember]
        public int Pointtype { get; set; }

        /// <summary>
        ///任务id
        /// </summary>
        [DataMember]
        public string Taskid { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        [DataMember]
        public DateTime Uptime { get; set; }

        /// <summary>
        ///X坐标
        /// </summary>
        [DataMember]
        public string X { get; set; }

        /// <summary>
        ///Y坐标
        /// </summary>
        [DataMember]
        public string Y { get; set; }
    }
}