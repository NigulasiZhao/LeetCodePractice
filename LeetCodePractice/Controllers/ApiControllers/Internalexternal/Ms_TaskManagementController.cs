using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Newtonsoft.Json;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;
using System.Data;
using System.Text;
using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Internalexternal
{
    /// <summary>
    /// 任务管理
    /// </summary>
    public class Ms_TaskManagementController : Controller
    {
        private readonly IMs_TaskManagementDAL _ms_TaskManagementDAL;

        public Ms_TaskManagementController(IMs_TaskManagementDAL ms_TaskManagementDAL)
        {
            _ms_TaskManagementDAL = ms_TaskManagementDAL;
        }
        /// <summary>
        /// 任务查询
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition|startTime|endTime",LinkType:and|or}]查询参数名称 上传人：uploaderid 派发人:dispatchpersonid 外业人：execpersonid 派发状态:ispost 任务名称:taskname  上传时间:uploadetime</param>
        /// <param name="sort"></param>
        /// <param name="describe">文件描述</param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string describe = "", string sort = "taskdate", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ms_TaskManagementDAL.GetList(list, describe, sort, ordering, num, page, sqlCondition);
            return result;
        }


        /// <summary>
        /// 任务派发
        /// </summary>
        /// <param name="fid">文件id</param>
        /// <param name="dispatchPersonId">分派人id</param>
        /// <param name="dispatchPersonName">分派人姓名</param>
        /// <param name="execPersonId">处理人id</param>
        /// <param name="execPersonName">处理人姓名</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="describe">任务描述</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string fid, string dispatchPersonId, string dispatchPersonName, string execPersonId, string execPersonName, string taskName, string describe = "")
        {
            Ms_TaskManagement value = new Ms_TaskManagement();
            value.Fid = fid;
            value.DispatchPersonId = dispatchPersonId;
            value.DispatchPersonName = dispatchPersonName;
            value.ExecPersonId = execPersonId;
            value.ExecPersonName = execPersonName;
            value.TaskName = taskName;
            value.TaskDescribe = describe;
            return _ms_TaskManagementDAL.Add(value);
        }

        /// <summary>
        /// 任务修改
        /// </summary>
        /// <param name="taskID">任务id</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string taskID, [FromBody] Ms_TaskManagement value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.TaskId = taskID;
            return _ms_TaskManagementDAL.Update(value);
        }

    }
}
