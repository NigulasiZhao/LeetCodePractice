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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ms_FileStore")]
    public class Ms_FileStore
    {
        /// <summary>
        /// 设备属性  设备属性值ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string FId { get; set; }

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
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]
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
        public int isPost { get; set; }
        /// <summary>
        /// 文件哈希值
        /// </summary>
        [DataMember]
        public string FileKey { get; set; }

    }
}
