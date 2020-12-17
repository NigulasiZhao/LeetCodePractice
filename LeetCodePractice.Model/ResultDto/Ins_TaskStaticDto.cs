using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
  public  class Ins_TaskStaticDto
    {
        public string TaskId { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        public string Range_Id { get; set; }
        public string Range_Name { get; set; }
        public string Geometry { get; set; }
        public List<EquipmentInfoDto> EquipmentDetailInfoList { get; set; }
    }
}
