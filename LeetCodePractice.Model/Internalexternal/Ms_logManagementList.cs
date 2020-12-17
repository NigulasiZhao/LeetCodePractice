using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Internalexternal
{
  public  class Ms_logManagementList
    {
        /// <summary>
        /// 日志id
        /// </summary>

        public string LId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int operationType { get; set; }

        /// <summary>
        /// 操作类型名
        /// </summary>
        public string operationTypeName { get; set; }


        /// <summary>
        /// 操作人id
        /// </summary>
        public string operatorId { get; set; }
        /// <summary>
        ///操作时间
        /// </summary>

        public DateTime operatorTime { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>

        public string operatorName { get; set; }
        /// <summary>
        /// 新值
        /// </summary>

        public string newValue { get; set; }
        /// <summary>
        /// 旧值
        /// </summary>

        public string oldValue { get; set; }
        /// <summary>
        /// 操作字段
        /// </summary>

        public string operationField { get; set; }
    }
}
