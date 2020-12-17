using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Internalexternal
{
 public class Ms_ExcelPointLine
    {
        /// <summary>
        /// 点线名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 点线集合
        /// </summary>
        public List<Ms_ExcelAppUpload> ExcelAppGroup { get; set; }
    }
}