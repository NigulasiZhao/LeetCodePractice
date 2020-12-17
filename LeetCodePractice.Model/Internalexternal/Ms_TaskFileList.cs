using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Internalexternal
{
   public class Ms_TaskFileList
    {
        /// <summary>
        /// 设备属性  设备属性值ID
        /// </summary>
        [DataMember]
        public string FId { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        [DataMember]
        public string taskId { get; set; }
        /// <summary>
        /// 上传人id
        /// </summary>
        [DataMember]
        public string uploaderId { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        [DataMember]
        public string uploaderName { get; set; }
        /// <summary>
        ///上传时间
        /// </summary>
        [DataMember]
        public DateTime uploadeTime { get; set; }
        /// <summary>
        /// 上传路径
        /// </summary>
        [DataMember]
        public string uploadpath { get; set; }
        /// <summary>
        /// 文件描述
        /// </summary>
        [DataMember]
        public string FileDescribe { get; set; }
        /// <summary>
        /// 是否分派
        /// </summary>
        [DataMember]
        public string isPost { get; set; }

        //任务model
        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string TaskName { get; set; }
        /// <summary>
        ///任务派发时间
        /// </summary>
        [DataMember]
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
        /// <summary>
        /// 对应任务的点线集合
        /// </summary>
        [DataMember]
        public List<Ms_ExcelPointLine> ExcelPointLineList { get; set; }

    }
}
