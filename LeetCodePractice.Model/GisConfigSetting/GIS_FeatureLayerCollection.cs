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
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_FeatureLayerCollection")]
    public class GIS_FeatureLayerCollection
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }

        /// <summary>
        /// 图层基础配置表编号
        /// </summary>
        [DataMember]
        public string ConfigID { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [DataMember]
        public string groupCName { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [DataMember]
        public string groupName { get; set; }
        /// <summary>
        /// 图层URL
        /// </summary>
        [DataMember]
        public string layerURL { get; set; }
        /// <summary>
        /// 编辑图层URL
        /// </summary>
        [DataMember]
        public string editURL { get; set; }
        /// <summary>
        /// 分析服务地址
        /// </summary>
        [DataMember]
        public string GPService { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public int viewIndex { get; set; }
        /// <summary>
        /// 是否显示节点
        /// </summary>
        [DataMember]
        public int isActive { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        public int isEnable { get; set; }
        /// <summary>
        /// 图层集合
        /// </summary>
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string featureLayers { get; set; }
    }
}
