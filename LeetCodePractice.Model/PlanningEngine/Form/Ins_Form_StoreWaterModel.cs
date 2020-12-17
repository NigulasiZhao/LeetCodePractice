using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
    [Serializable]
    public class Ins_Form_StoreWaterModel
    {
        /// <summary>
        /// 主键
        /// </summary>

        public string ID { get; set; }


        public string TaskId { get; set; }

        public string TaskName { get; set; }
        public string WorkCode { get; set; }

        public string GlobID { get; set; }

        public string LayerName { get; set; }

        public string X { get; set; }

        public string Y { get; set; }

        public string Plan_task_id { get; set; }
        /// <summary>
        /// 是否填单
        /// </summary>

        public int IsFillForm { get; set; }

        public string[] ImagePath { get; set; }
        /// <summary>
        /// 内、外设备
        /// </summary>  

        public decimal ExternalEqu
        {
            set; get;
        }
        /// <summary>
        /// 附属设备
        /// </summary>  

        public decimal SubsidiaryEqu
        {
            set; get;
        }
        /// <summary>
        /// 水位控制装置及其他测量仪
        /// </summary>  

        public decimal WaterLevel
        {
            set; get;
        }
        /// <summary>
        /// 油漆修补
        /// </summary>  

        public decimal PaintRepair
        {
            set; get;
        }

    }
}
