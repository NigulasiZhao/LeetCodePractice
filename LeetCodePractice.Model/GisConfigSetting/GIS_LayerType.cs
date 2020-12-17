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
    /// 图层类别配置
    /// </summary>
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_LayerType")]
    public class GIS_LayerType
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        [DataMember]
        public string TypeCName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// 类别编码
        /// </summary>
        [DataMember]
        public int TypeCode { get; set; }

        /// <summary>
        /// 是否系统
        /// </summary>
        [DataMember]
        public int ISSystem { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public int OrderNO { get; set; }

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
