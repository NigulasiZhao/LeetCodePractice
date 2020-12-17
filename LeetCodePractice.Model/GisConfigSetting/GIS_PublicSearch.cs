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
    /// 地图搜索配置模型
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_PublicSearch")]
    public class GIS_PublicSearch
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>
        [DataMember]
        public string PID { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        [DataMember]
        public string TypeCName { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string LinkURL { get; set; }
        /// <summary>
        /// 是否菜单
        /// </summary>
        [DataMember]
        public int ISNode { get; set; }
        /// <summary>
        /// 是否图层
        /// </summary>
        [DataMember]
        public int ISLayer { get; set; }

        /// <summary>
        /// 图层数据搜索字段集合
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<GIS_SearchField> SearchField { get; set; }

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

        /// <summary>
        /// 图标
        /// </summary>
        [DataMember]
        public string ICon { get; set; }
    }
}
