using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.AttendanceManage
{
    /// <summary>
    /// 任务轨迹
    /// </summary>
    public class InsTaskCoordinatsController : BaseController
    {
        private readonly IIns_TaskCoordinatsDAL _InsTaskCoordinatsDAL;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ins_TaskCoordinatsDAL"></param>
        public InsTaskCoordinatsController(IIns_TaskCoordinatsDAL ins_TaskCoordinatsDAL)
        {
            _InsTaskCoordinatsDAL = ins_TaskCoordinatsDAL;
        }
        /// <summary>
        /// 获取坐标信息
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <param name="taskId">任务Id</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string taskId, DateTime? startTime, DateTime? endTime)
        {
            var result = _InsTaskCoordinatsDAL.GetCoordinatsByTaskid(taskId, startTime,endTime,out ErrorType errorType, out string errorString);

            if (errorType != ErrorType.Success)
            {
                return MessageEntityTool.GetMessage(errorType, errorString, "获取坐标信息失败");
            }
            else
            {
                return result;
            }
        }
    }
}
