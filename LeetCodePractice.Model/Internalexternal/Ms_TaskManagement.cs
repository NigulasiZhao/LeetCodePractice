using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Internalexternal
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ms_TaskManagement")]
    public class Ms_TaskManagement
    {
        /// <summary>
        /// 任务id
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string TaskId { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        [DataMember]
        public string Fid { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string TaskName { get; set; }
        /// <summary>
        ///任务派发时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]
        public DateTime TaskDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string TaskAlias { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [DataMember]
        public string TaskDescribe { get; set; }
        /// <summary>
        /// 处理人id
        /// </summary>
        [DataMember]
        public string ExecPersonId { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember]
        public string ExecPersonName { get; set; }
        /// <summary>
        /// 分派人id
        /// </summary>
        [DataMember]
        public string DispatchPersonId { get; set; }
        /// <summary>
        /// 分派人
        /// </summary>
        [DataMember]
        public string DispatchPersonName { get; set; }
    }
}