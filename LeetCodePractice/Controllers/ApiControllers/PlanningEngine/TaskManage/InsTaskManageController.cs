using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.TaskManage
{
    /// <summary>
    /// 任务列表
    /// </summary>
    public class InsTaskManageController : Controller
    {
        private readonly IIns_TaskManageDAL _iIns_TaskManageDAL;

        public InsTaskManageController(IIns_TaskManageDAL iIns_TaskManageDAL)
        {
            _iIns_TaskManageDAL = iIns_TaskManageDAL;
        }

        /// <summary>
        /// 获得任务信息列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]查询参数：TaskName：任务名称；task_type_id：任务类型id； proraterdeptid：部门id； proraterid：人员id  range_id： 任务区域id ； assignstate：任务状态； IsFinish：完成状态</param>
        /// <param name="rangids">区域ids </param>
        /// <param name="task_Type_ids">任务类型 id  </param>
        /// <param name="taskState">任务状态  1：已派发 2：未派发  3：已完成 4:已作废  5 处理中（已分派未完成）</param>
        /// <param name="startTime">开始时间 yyyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyyy-mm-dd</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Get(string ParInfo, [FromBody] List<string>? rangids = null, string task_Type_ids = null, string taskState =null, DateTime? startTime = null, DateTime? endTime = null, string sort = "OPERATEDATE", string ordering = "desc", int num = 20, int page = 1)
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
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iIns_TaskManageDAL.Get(list, Ids, task_Type_ids, taskState,startTime, endTime, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 根据任务id获取设备详情
        /// </summary>
        /// <param name="taskid">任务id</param>
        /// <param name="IsFillForm">是否填单 0：否 1：是</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <param name="X">当前横坐标</param>
        /// <param name="Y">当前纵坐标</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEquipmentInfo(string taskid,string? IsFillForm=null, string sort = "equipment_info_name", string ordering = "desc", int num = 20, int page = 1, string X = null, string Y = null)
        {
   
            var result = _iIns_TaskManageDAL.GetEquipmentInfo(taskid, IsFillForm,sort, ordering, num, page,X,Y);
            return result;
        }

        /// <summary>
        /// 分派任务
        /// </summary>
        /// <param name="taskIds">任务ids</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity AssignTask([FromBody] List<string> taskIds )
        {
            if (taskIds != null && taskIds.Count<=0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            string Ids = "";
            if (taskIds != null)
            {
                Ids = "'" + string.Join("','", taskIds.ToArray()) + "'";
                if (taskIds.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            return _iIns_TaskManageDAL.AssignTask(Ids);
        }
        /// <summary>
        /// 转派任务
        /// </summary>
        /// <param name="taskIds">任务id </param>
        /// <param name="proraterDeptName">部门名称</param>
        /// <param name="proraterDeptId">部门Id</param>
        /// <param name="proraterName">分派人员</param>
        /// <param name="proraterId">分派人员Id</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity ReAssignTask(string taskIds, string proraterDeptName, string proraterDeptId, string proraterName, string proraterId)
        {
            if (string.IsNullOrEmpty(taskIds)|| string.IsNullOrEmpty(proraterDeptName) || string.IsNullOrEmpty(proraterDeptId) || string.IsNullOrEmpty(proraterName) || string.IsNullOrEmpty(proraterId))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }

            return _iIns_TaskManageDAL.ReAssignTask(taskIds, proraterDeptName, proraterDeptId, proraterName, proraterId);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string taskId)
        {
            if (string.IsNullOrEmpty(taskId))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            //首先判断任务是否分派，已分派不允许删除
          DataTable dt=  _iIns_TaskManageDAL.IsAssign(taskId);
            if(dt!=null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["AssignState"].ToString() == "1")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "任务已被分派，不允许删除！");
                }
            }
            return _iIns_TaskManageDAL.Delete(taskId);
        }
        /// <summary>
        /// 作废任务
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Cancel([FromBody] List<string> taskIds)
        {
            if (taskIds != null && taskIds.Count <= 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            string Ids = "";
            if (taskIds != null)
            {
                Ids = "'" + string.Join("','", taskIds.ToArray()) + "'";
                if (taskIds.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }

            return _iIns_TaskManageDAL.Cancel(Ids);
        }
        ///// <summary>
        ///// 获取任务所包含设备信息
        ///// </summary>
        ///// <param name="ParInfo"></param>
        ///// <param name="sort"></param>
        ///// <param name="ordering"></param>
        ///// <param name="num"></param>
        ///// <param name="page"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public MessageEntity GetTaskEquipmentInfoList(string ParInfo, string taskId, string sort = "Create_Time", string ordering = "desc", int num = 20, int page = 1)
        //{
        //    List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
        //    SqlCondition sqlWhere = new SqlCondition();
        //    string sqlCondition = sqlWhere.getParInfo(list);
        //    var result = _iIns_TaskManageDAL.GetTaskEquipmentInfoList(list, taskId, sort, ordering, num, page, sqlCondition);
        //    return result;
        //}
        ///// <summary>
        ///// 获取任务信息及所属任务明细
        ///// </summary>
        ///// <param name="taskId"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public MessageEntity GetTaskPlanInfo(string taskId)
        //{
        //    return _iIns_TaskManageDAL.GetTaskPlanInfo(taskId);
        //}

    }
}