using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_PersonPosition")]
    public  class Ins_PersonPosition
    {
        /// <summary>
        ///主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string PositionId { get; set; }
        /// <summary>
        ///上传人id
        /// </summary>
        [DataMember]
        public string Personid { get; set; }

        /// <summary>
        ///上传人
        /// </summary>
        [DataMember]
        public string Personname { get; set; }

        /// <summary>
        ///上传人id
        /// </summary>
        [DataMember]
        public string DetpID { get; set; }

        /// <summary>
        ///上传人
        /// </summary>
        [DataMember]
        public string DetptName { get; set; }


        /// <summary>
        ///横坐标
        /// </summary>
        [DataMember]
        public string PositionX { get; set; }

        /// <summary>
        ///纵坐标
        /// </summary>
        [DataMember]
        public string PositionY { get; set; }

        /// <summary>
        ///任务id
        /// </summary>
        [DataMember]
        public string Taskid { get; set; }
        /// <summary>
        ///轨迹距离
        /// </summary>
        [DataMember]
        public decimal Distance { get; set; }

        /// <summary>
        ///巡检时间
        /// </summary>
        [DataMember]
        public decimal Minutes { get; set; }
        
        /// <summary>
        ///分组id  1：签到 2：签退
        /// </summary>
        [DataMember]
        public string GroupID { get; set; }
      
    }
}
