using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Plan;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan
{
    public interface IIns_P_PositionDAL
    {
       
        /// <summary>
        /// 获取任务轨迹信息
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="getTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        MessageEntity GetInspectionRoute(string taskid, DateTime getTime, DateTime endTime);
        MessageEntity GetRouteByTaskId(string taskid);
    }
}
