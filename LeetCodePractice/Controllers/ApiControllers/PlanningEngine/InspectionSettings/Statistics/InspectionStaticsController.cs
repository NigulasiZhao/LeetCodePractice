using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics;
using GISWaterSupplyAndSewageServer.Model.UniWater;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.InspectionSettings.Statistics
{
    /// <summary>
    /// 巡查总览
    /// </summary>
    public class InspectionStaticsController : BaseController
    {
        private readonly IInspectionStaticsDAL _iInspectionStaticsDAL;
        public InspectionStaticsController(IInspectionStaticsDAL iInspectionStaticsDAL)
        {
            _iInspectionStaticsDAL = iInspectionStaticsDAL;

        }  /// <summary>
           ///  获取巡查总览数据(当前登陆人员) APP调用：(Headers传递app和token) 
           /// </summary>
           /// <param name="startTime">开始时间 yyy-mm-dd</param>
           /// <param name="endTime">结束时间 yyy-mm-dd</param>
           /// <param name="task_Type_id">任务类型 id  传参格式 '1','2','3'</param>
           /// <param name="isFinish">任务状态 0：未完成(执行中) 1：已完成 2：未开始 3:已超时 空代表全部</param>
           /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(DateTime? startTime, DateTime? endTime, string? task_Type_id = null, string? isFinish = null)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            string iAdminID = "";
            if (UniWaterUserInfo != null)
            {
                iAdminID = UniWaterUserInfo._id;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "uniwater获取用户信息失败！");
            }
            var result = _iInspectionStaticsDAL.GetByOwnID(iAdminID, task_Type_id, isFinish, startTime, endTime);
            return result;
        }

        /// <summary>
        ///  获取巡检总览分析数据
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <param name="rangName">网格区域名称 </param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetInspectionOverview(DateTime? startTime, DateTime? endTime, string? rangName = null)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            var result = _iInspectionStaticsDAL.GetInspectionOverview(startTime, endTime, rangName);
            return result;
        }

    }
}