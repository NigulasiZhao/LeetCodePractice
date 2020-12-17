
using System;

namespace GISWaterSupplyAndSewageServer.Model.AttributePack
{
    ///　<summary>
    ///　过滤类型
    ///　</summary>
    public enum FilterType
    {
        /// <summary>
        /// 字段或属性的值是否为不进行更新操作 
        /// </summary>
        IsNotUpdate = 0x0004,

        /// <summary>
        /// 字段或属性的值是否不进行插入操作
        /// </summary>
        IsNotInsert = 0x0008,

        /// <summary>
        /// 字段或属性的值是否不进行任何更改操作
        /// </summary>
        IsNotEdit = 0x0012,

        IsPrimaryKey = 0x0013
    }

    /// <summary>
    /// 判断数据类型
    /// </summary>
    public enum DataType
    {
        ///　<summary>
        ///　字段或属性的值是否为数值类型
        ///　</summary>
        IsNumber = 0x0012,

        ///　<summary>
        ///　字段或属性的值是否为时间类型
        ///　</summary>
        IsDateTime = 0x0014,

        ///　<summary>
        ///　字段或属性的值是否为正确的浮点类型
        ///　</summary>
        IsDecimal = 0x0016,

        ///　<summary>
        ///　字段或属性的值是否为GUID类型
        ///　</summary>
        IsGuid = 0x0018,

        /// <summary>
        /// 字符串
        /// </summary>
        IsString = 0x0020,
        /// <summary>
        /// clob
        /// </summary>
        IsClob = 0x0020
    }

    /// <summary>
    /// 主键类型
    /// </summary>
    public enum PrimaryKeyType
    {
        ///　<summary>
        ///　主键是否为自动增长类型
        ///　</summary>
        Identity = 0x0020,

        ///　<summary>
        ///　主键是否为GUID类型
        ///　</summary>
        Guid = 0x0022,

        ///　<summary>
        ///　主键是否为本地类型
        ///　</summary>
        Native = 0x0024,

        ///　<summary>
        ///　主键是否为获取Sequence类型
        ///　</summary>
        Sequence = 0x0026,

        ///　<summary>
        ///　主键是否为获取外键唯一对应类型
        ///　</summary>
        Foreign = 0x0028
    }

}
