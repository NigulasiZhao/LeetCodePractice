using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto
{
    public class UniResult<T>
    {
        /// <summary>
        /// 返回码 , 为0表示正常返回，否则为错误码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 异常消息文本
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// object - 返回数据结构
        /// </summary>
        public List<T> Response { get; set; }
    }
}
