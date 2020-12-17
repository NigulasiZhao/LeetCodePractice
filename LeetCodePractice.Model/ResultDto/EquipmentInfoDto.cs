using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
   public class EquipmentInfoDto
    {
        public string TaskId { get; set; }
        public string TableId { get; set; }
        public string IsFinish { get; set; }
        public string GlobId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Lat { get; set; }

        public string Geometry { get; set; }

        /// <summary>
        /// 1点位 2管线
        /// </summary>
        public int EquType { get; set; }
    }
}
