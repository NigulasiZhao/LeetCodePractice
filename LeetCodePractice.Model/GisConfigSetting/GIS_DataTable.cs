using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace GISWaterSupplyAndSewageServer.Model.GisConfigSetting
{
    //GIS_DataTable

    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_DataTable")]
    public class GIS_DataTable
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
        /// 别名
        /// </summary>  
        [DataMember]
        public string TableCName
        {
            set; get;
        }
        /// <summary>
        /// 数据表名
        /// </summary>  
        [DataMember]
        public string TableName
        {
            set; get;
        }
        /// <summary>
        /// 创建人编号
        /// </summary>  
        [DataMember]
        public string CreatedID
        {
            set; get;
        }
        /// <summary>
        /// 创建人
        /// </summary>  
        [DataMember]
        public string CreatedBy
        {
            set; get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>  
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime CreatedTime
        {
            set; get;
        }
        
        /// <summary>
        /// 服务地址
        /// </summary>  
        [DataMember]
        public string LinkUrl
        {
            set; get;
        }
        /// <summary>
        /// 数据字段集合
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<GIS_DataTableColumn> GisDataTableColumn { set; get; }
    }
}
