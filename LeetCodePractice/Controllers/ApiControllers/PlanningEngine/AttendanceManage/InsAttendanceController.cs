using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.AttendanceManage
{
/// <summary>
/// 签到管理
/// </summary>
    public class InsAttendanceController : BaseController
    {
        private readonly IIns_AttendanceDAL _iIns_AttendanceDAL;

        public InsAttendanceController(IIns_AttendanceDAL iIns_AttendanceDAL)
        {
            _iIns_AttendanceDAL = iIns_AttendanceDAL;
        }
        /// <summary>
        /// 添加签到信息
        /// </summary>
        /// <param name="taskid">任务id</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="groupID">分组id  1：签到 2：签退</param>
        /// <param name="attendanceType">打卡类型: 签到 签退；</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string taskid, string taskName,string groupID ,string attendanceType)
        {
            Ins_Attendance value = new Ins_Attendance();
            value.Taskid = taskid;
            value.TaskName = taskName;
            value.GroupID = groupID;
            value.AttendanceType = attendanceType;
            value.IsAutomatic = 0;
            if (value.GroupID == "2")
                value.Remark = "手工签退";
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                value.Personid = UniWaterUserInfo._id;
                value.Personname = UniWaterUserInfo.Name;
                value.DetpID = UniWaterUserInfo.Group;
                value.DetptName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            //IsAutomatic：0：手动签退 1：自动签退
            
            return _iIns_AttendanceDAL.Add(value);
        }
        /// <summary>
        /// 获取签到记录
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition|startTime|endTime",LinkType:and|or}]  查询参数 签到时间:upTime；人员id：Personid；部门id：DetpID</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "upTime", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iIns_AttendanceDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 返回最后一条签到记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetLastAttendance()
        {
            string Personid = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                Personid = UniWaterUserInfo._id;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            return _iIns_AttendanceDAL.GetLastAttendance(Personid);
        }
    }
}