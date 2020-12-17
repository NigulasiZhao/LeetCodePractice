using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
    public class EquipmentFormDto
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Task_Name { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string GlobID { get; set; }

        /// <summary>
        /// 设施编号
        /// </summary>
        public string Equipment_Info_Code { get; set; }
        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkCode { get; set; }
    }
}
