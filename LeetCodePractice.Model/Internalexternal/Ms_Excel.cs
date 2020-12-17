using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Internalexternal
{
  public  class Ms_Excel
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 列值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 输入类型 s ：下拉框  t：输入框
        /// </summary>
        public string Inputtype { get; set; }
        /// <summary>
        /// 是否必填 可为空1:空 0 不可空
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// 是否可编辑 0：不可编辑 1：可编辑
        /// </summary>
        public bool IsEdit { get; set; }
    }
}
