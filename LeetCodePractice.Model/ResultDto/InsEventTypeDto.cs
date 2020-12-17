using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
   public class InsEventTypeDto
    {
        public string Event_Type_id { set; get; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventTypeName { set; get; }
        public string ParentTypeId { set; get; }

        public List<InsEventTypeDto> EventContentList { get; set; }

    }
}
