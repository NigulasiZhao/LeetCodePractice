using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.Model.GisConfigSetting
{
    /// <summary>
    /// 地图搜索模型字段
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_SearchField")]
    public class GIS_SearchField
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string SearchID { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string FieldCName { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string FieldName { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string DataType { get; set; }
    }
}
