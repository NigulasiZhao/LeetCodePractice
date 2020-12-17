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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ms_WorkOrder")]
    public class Ms_WorkOrder
    {
        /// <summary>
        /// 工单id
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string WorkOrderID { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        [DataMember]
        public string taskid { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string taskName { get; set; }

        /// <summary>
        /// 内业处理人员
        /// </summary>
        [DataMember]
        public string HandlePerson { get; set; }
        /// <summary>
        /// 是否分派 0：未分派 1：已分派 2:已完成
        /// </summary>
        [DataMember]
        public string isPostcomplete { get; set; }


        /// <summary>
        /// 设备信息json
        /// </summary>
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string Equipmentjson { get; set; }

        /// <summary>
        ///上传时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]
        public DateTime uploadeTime { get; set; }
    }
}
