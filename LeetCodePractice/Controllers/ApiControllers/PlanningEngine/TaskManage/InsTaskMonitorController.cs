using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.TaskManage
{
/// <summary>
/// 巡检监控
/// </summary>
    public class InsTaskMonitorController : Controller
    {
        private readonly IIns_TaskMonitorDAL _iIns_TaskMonitorDAL;

        public InsTaskMonitorController(IIns_TaskMonitorDAL iIns_TaskMonitorDAL)
        {
            _iIns_TaskMonitorDAL = iIns_TaskMonitorDAL;
        }
        /// <summary>
        /// 获得任务信息列表
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <param name="rangids">区域ids </param>
        /// <param name="proraterDeptId">部门id</param>
        /// <param name="proraterId">用户id </param>
        /// <param name="task_Type_id">任务类型id </param>
        /// <param name="sort">默认操作时间 OPERATEDATE</param>
        /// <param name="ordering">默认 desc</param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Get(DateTime? startTime, DateTime? endTime, string? proraterDeptId = null, string? proraterId = null, [FromBody] List<string>? rangids = null, string? task_Type_id = null, string sort = "OPERATEDATE", string ordering = "desc", int num = 20, int page = 1)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            string Ids = "";
            if (rangids != null)
            {
                Ids = "'" + string.Join("','", rangids.ToArray()) + "'";
                if (rangids.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            var result = _iIns_TaskMonitorDAL.Get(startTime, endTime, proraterDeptId, proraterId,Ids, task_Type_id, sort, ordering, num, page);
            return result;
        }
        
    }
}