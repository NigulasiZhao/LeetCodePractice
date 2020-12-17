using System;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.Model.BaseEntity
{
    [Serializable]
    public class BaseModel
    {
        /// <summary>
        /// 插入的数据的ID
        /// </summary>
        [Column(FilterType=FilterType.IsNotEdit)]
        public string NewId { get; set; }
    }
}
