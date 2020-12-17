using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Attendance")]
    public class Ins_Attendance
    {
        /// <summary>
        ///主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string AttendanceID { get; set; }
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
        ///任务id
        /// </summary>
        [DataMember]
        public string Taskid { get; set; }
        [DataMember]
        public string TaskName { get; set; }
        
        /// <summary>
        ///分组id  1：签到 2：签退
        /// </summary>
        [DataMember]
        public string GroupID { get; set; }
        /// <summary>
        ///打卡类型 签到 签退
        /// </summary>
        [DataMember]
        public string AttendanceType { get; set; }
        /// <summary>
        ///是否自动 0：手动签退 1：自动签退
        /// </summary>
        [DataMember]
        public int IsAutomatic { get; set; }
        /// <summary>
        ///分组id  1：签到 2：签退
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]
        public DateTime UpTime { get; set; }

    }
}
