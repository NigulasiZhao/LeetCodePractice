using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
   public class InsEventStaticDto
    {
        public string EventTypeName { set; get; }
        public string EventTypeName2 { set; get; }
        /// <summary>
        /// 按照事件类型 事件内容分组总数
        /// </summary>
        public int SL { set; get; }
        /// <summary>
        /// 按照事件类型查询总数
        /// </summary>
        public int XJ { set; get; }
        /// <summary>
        /// SL/XJ
        /// </summary>
        public string Bili { set; get; }
       /// <summary>
       /// XJ/总事件数
       /// </summary>
        public string BiliAll { set; get; }
    }
}
