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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ms_logManagement")]
    public class Ms_logManagement
    {
        /// <summary>
        /// 日志id
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string LId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [DataMember]
        public int operationType { get; set; }



        /// <summary>
        /// 操作人id
        /// </summary>
        [DataMember]
        public string operatorId { get; set; }
        /// <summary>
        ///操作时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]
        public DateTime operatorTime { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        [DataMember]
        public string operatorName { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        [DataMember]
        public string newValue { get; set; }
        /// <summary>
        /// 旧值
        /// </summary>
        [DataMember]
        public string oldValue { get; set; }
        /// <summary>
        /// 操作字段
        /// </summary>
        [DataMember]
        public string operationField { get; set; }
    }
}
