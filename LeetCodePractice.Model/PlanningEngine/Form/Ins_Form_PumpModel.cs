using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form
{
    public class Ins_Form_PumpModel
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
        ///电控柜
        /// </summary>

        public int Electric { get; set; }
        /// <summary>
        ///测量仪表
        /// </summary>

        public int Measuremeter { get; set; }

        /// <summary>
        ///电动机
        /// </summary>

        public int Motor { get; set; }

        /// <summary>
        ///电动机与控制柜之间的电源
        /// </summary>

        public int Motorpowersupply { get; set; }


        /// <summary>
        ///油漆修补
        /// </summary>

        public int Paintrepair { get; set; }



        /// <summary>
        ///排污设施
        /// </summary>

        public int Sewageequ { get; set; }



        /// <summary>
        ///机组基础
        /// </summary>

        public int Unitfoundation { get; set; }

        /// <summary>
        ///各类管道
        /// </summary>

        public int Variouspipe { get; set; }

        /// <summary>
        ///各类阀门
        /// </summary>

        public int Variousvalve { get; set; }

        /// <summary>
        ///水泵
        /// </summary>

        public int Waterpump { get; set; }
    }
}
