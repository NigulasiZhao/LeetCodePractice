using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
  public  class EquipmentDetailInfoDto
    {
        /// <summary>
        /// 任务明细id
        /// </summary>
        public string Plan_Task_Id { get; set; }
        /// <summary>
        /// 设施信息
        /// </summary>
        public string Equipmentinfo { get; set; }
        /// <summary>
        /// 自定义表单ID
        /// </summary>
        public string TableId { get; set; }

        public string TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 设施id
        /// </summary>
        public string GlobID { get; set; }
        /// <summary>
        /// 设施编号
        /// </summary>
        public string Equipment_Info_Code { get; set; }

        /// <summary>
        /// 设施名称
        /// </summary>
        public string Equipment_Info_Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 口径
        /// </summary>
        public string Caliber { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// 管线坐标信息
        /// </summary>
        public string Geometry { get; set; }
        /// <summary>
        /// 任务区域信息
        /// </summary>
        public string PGeometry { get; set; }


        /// <summary>
        /// 1点位 2管线
        /// </summary>
        public int EquType { get; set; }

        /// <summary>
        /// 是否填单
        /// </summary>

        public int IsFillForm { get; set; }

        /// <summary>
        /// 表单主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 完成状态
        /// </summary>
        public string IsFinishName { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public string Operatedate { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string LayerName { get; set; }
        /// <summary>
        /// 是否上报
        /// </summary>
        public int IsReport { get; set; }

        public decimal Distance { get; set; }
    }
}
