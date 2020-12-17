using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.UniWater;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.TaskManage
{
    /// <summary>
    /// 任务列表(APP)
    /// </summary>
    public class InsTaskManageAppController : BaseController
    {
        private readonly IIns_TaskManageAppDAL _iIns_TaskManageAppDAL;

        public InsTaskManageAppController(IIns_TaskManageAppDAL iIns_TaskManageAppDAL)
        {
            _iIns_TaskManageAppDAL = iIns_TaskManageAppDAL;
        }
        /// <summary>
        /// 获得任务信息列表 APP调用：(Headers传递app和token)
        /// </summary>
        /// <param name="taskName">任务名稱</param>
        /// <param name="taskid">任务id</param>
        /// <param name="isFinish">任务状态 0：未完成(执行中) 1：已完成 2：未开始 3:已超时 空代表全部</param>
        /// <param name="task_Type_id">任务类型 id  传参格式 '1','2','3'</param>
        /// <param name="startTime">yyyy-MM-dd</param>
        /// <param name="endTime">yyyy-MM-dd</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string? taskName = null, string? taskid = null, string? isFinish = null, string? task_Type_id = null, DateTime? startTime = null, DateTime? endTime = null, string sort = "IsFinishName", string ordering = "desc", int num = 20, int page = 1)
        {
            string iAdminID = "";
            if (UniWaterUserInfo != null)
            {
                iAdminID = UniWaterUserInfo._id;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "uniwater获取用户信息失败！");
            }
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            var result = _iIns_TaskManageAppDAL.Get(iAdminID, taskName, taskid, isFinish, task_Type_id, startTime, endTime, sort, ordering, num, page);
            return result;
        }
        /// <summary>
        /// 获取表单中工单编号
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetWorkCode(string tableId)
        {
            var result = _iIns_TaskManageAppDAL.GetWorkCode(tableId);
            return result;
        }
    }
}