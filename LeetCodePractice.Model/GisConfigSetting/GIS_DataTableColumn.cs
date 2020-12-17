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
    /// 要素配置信息
    /// </summary>

    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_DataTableColumn")]
    public class GIS_DataTableColumn
    {

        /// <summary>
        /// 编号
        /// </summary>  
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID
        {
            set; get;
        }

        /// <summary>
        /// 主表编号
        /// </summary>  
        [DataMember]
        public string TableID
        {
            set; get;
        }
        /// <summary>
        /// 对齐方式
        /// </summary>  
        [DataMember]
        public string align
        {
            set; get;
        }
        /// <summary>
        /// 字段名称
        /// </summary>  
        [DataMember]
        public string field
        {
            set; get;
        }
        /// <summary>
        /// 别名
        /// </summary>  
        [DataMember]
        public string text
        {
            set; get;
        }
        /// <summary>
        /// 数据类型
        /// </summary>  
        [DataMember]
        public string DataType
        {
            set; get;
        }
        /// <summary>
        /// 控件类型
        /// </summary>  
        [DataMember]
        public string ControlType
        {
            set; get;
        }
        /// <summary>
        /// 是否启用
        /// </summary>  
        [DataMember]
        public int Enabled
        {
            set; get;
        }
        /// <summary>
        /// 是否POPTemp字段
        /// </summary>  
        [DataMember]
        public int ISTempInfo
        {
            set; get;
        }
        /// <summary>
        /// 是否系统字段
        /// </summary>  
        [DataMember]
        public int ISSystem
        {
            set; get;
        }
        /// <summary>
        /// 是否分析字段
        /// </summary>  
        [DataMember]
        public int ISAnalysis
        {
            set; get;
        }
        /// <summary>
        /// 序号
        /// </summary>  
        [DataMember]
        public int OrderNO
        {
            set; get;
        }
        /// <summary>
        /// 字段最大长度
        /// </summary>  
        [DataMember]
        public int Maxlength
        {
            set; get;
        }
        
    }

}
