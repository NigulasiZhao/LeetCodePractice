using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.Plan;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Plan
{
    /// <summary>
    /// 任务轨迹相关
    /// </summary>
    public class InsPositionController : Controller
    {
        private readonly IIns_P_PositionDAL _iIns_P_PositionDAL;

        public InsPositionController(IIns_P_PositionDAL iIns_P_PositionDAL)
        {
            _iIns_P_PositionDAL = iIns_P_PositionDAL;
        }
       
    

        /// <summary>
        /// 根据任务id获取轨迹,有数据返回就行 路线现在不叠加(播放按钮)
        /// </summary>
        /// <param name="taskid">根据任务id获取轨迹</param>
        ///  <param name="getTime">轨迹时间 参数格式yyyy-MM-dd</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetInspectorRoute(string taskid, DateTime getTime)
        {
            DateTime endTime = getTime.AddDays(1).AddSeconds(-1);
            return _iIns_P_PositionDAL.GetInspectionRoute(taskid, getTime, endTime);
        }
        /// <summary>
        /// 根据任务id获取巡检轨迹
        /// </summary>
        /// <param name="taskid">任务id</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetRouteByTaskId(string taskid)
        {

            var result = _iIns_P_PositionDAL.GetRouteByTaskId(taskid);
            return result;
        }
    }
}