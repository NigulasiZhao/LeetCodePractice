using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.AttributePack
{

    /// <summary>
    /// 数据库表的字段的映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        ///　<summary>
        ///　对应数据库表的字段名称
        ///　</summary>
        public string Name { get; set; }

        ///　<summary>
        ///　对应数据库表的字段名称
        ///　</summary>
        public string Description { get; set; }

        ///　<summary>
        ///　最小长度
        ///　</summary>
        public int MinLength { get; set; }

        ///　<summary>
        ///　最大长度
        ///　</summary>
        public int MaxLength { get; set; }

        ///　<summary>
        ///　验证类型
        ///　</summary>
        public FilterType FilterType { get; set; }

        ///　<summary>
        ///　判断数据类型
        ///　</summary>
        public DataType DataType { get; set; }

        ///　<summary>
        ///　主键类型
        ///　</summary>
        public PrimaryKeyType PrimaryKeyType { get; set; }

    }

}
