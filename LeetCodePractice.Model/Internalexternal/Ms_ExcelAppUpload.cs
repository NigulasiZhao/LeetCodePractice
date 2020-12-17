using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Internalexternal
{
  public class Ms_ExcelAppUpload
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EName { get; set; }
        /// <summary>
        /// 表格具体列属性
        /// </summary>
         public List<Ms_Excel> EquipmentMappingGroup { get; set; }
    }
}