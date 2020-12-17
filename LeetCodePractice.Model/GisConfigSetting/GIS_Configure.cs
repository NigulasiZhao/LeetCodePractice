using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.Model.GisConfigSetting
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_Configure")]
    public class GIS_Configure
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public string SystemCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string TableCName { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        [DataMember]
        public string TableName { get; set; }
        /// <summary>
        /// 配置项
        /// </summary>
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string ConfigContent { get; set; }
        /// <summary>
        /// 是否FeatureLayer
        /// </summary>
        [DataMember]
        public int IsFeatureLayer { get; set; }

        /// <summary>
        /// 矢量图层分组集合
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<GIS_FeatureLayerCollection> FeatureLayerGroup { get; set; }

        /// <summary>
        /// 添加人员编号
        /// </summary>
        [DataMember]
        public string CreatedID { get; set; }

        /// <summary>
        /// 添加人员
        /// </summary>
        [DataMember]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime CreatedTime { get; set; }
    }
}
