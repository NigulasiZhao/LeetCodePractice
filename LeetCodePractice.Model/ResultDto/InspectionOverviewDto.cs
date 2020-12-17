using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
   public class InspectionOverviewDto
    {
        /// <summary>
        /// 工单总数
        /// </summary>
        public string Gdzs { get; set; }
        /// <summary>
        /// 工单处理数
        /// </summary>
        public string Gdcls { get; set; }
        /// <summary>
        ///巡查公里数
        /// </summary>
        public string Xcgls { get; set; }

        /// <summary>
        ///巡查上报数
        /// </summary>
        public string Xcsbs { get; set; }
        /// <summary>
        ///计划任务数
        /// </summary>
        public string Jhrws { get; set; }
        /// <summary>
        ///接单率
        /// </summary>
        public string Jdl { get; set; }
        /// <summary>
        ///完成率
        /// </summary>
        public string Wcl { get; set; }
        /// <summary>
        ///上下浮接单率
        /// </summary>
        public string SxfJdl { get; set; }
        /// <summary>
        ///上下浮完成率
        /// </summary>
        public string SxfWcl { get; set; }

    }
}
