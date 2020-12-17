using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Form;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.InspectionSettings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Form
{
    /// <summary>
    /// 阀门填单(APP)
    /// </summary>
    public class InsFormValveController : BaseController
    {
        private readonly IIns_FormDAL _iIns_Form_ValveDAL;
        private readonly IIns_TaskManageDAL _iIns_TaskManageDAL;
        //private readonly IIns_EventDAL _eventManage;
        //private readonly IIns_Event_TypeDAL _ins_Event_TypeDAL;

        public InsFormValveController(IIns_FormDAL iIns_Form_ValveDAL, IIns_TaskManageDAL iIns_TaskManageDAL
            /*, IIns_EventDAL eventManage, IIns_Event_TypeDAL ins_Event_TypeDAL*/)
        {
            _iIns_Form_ValveDAL = iIns_Form_ValveDAL;
            _iIns_TaskManageDAL = iIns_TaskManageDAL;
            //_eventManage = eventManage;
            //_ins_Event_TypeDAL = ins_Event_TypeDAL;

        }
        /// <summary>
        /// 阀门填单 APP调用：(Headers传递app和token) 
        /// <param name="ID">Plan_task_id</param>
        /// <param name="x">上报位置横坐标</param>
        /// <param name="y">上报位置纵坐标</param>
        /// <param name="value"></param>
        /// <param name="isReport">是否上报事件  0：不上报  1：上报</param>
        /// <param name="eventTypeId">事件类型id</param>
        /// <param name="eventTypeId2">事件内容id</param>
        /// <param name="eventDesc">事件备注</param>
        /// <param name="rangName">网格区域名称</param>  
        /// <param name="execDetpID">分派人部门ID</param>
        /// <param name="execDetpName">分派人部门名称</param>
        /// <param name="execPersonId">分派人人ID</param>
        /// <param name="execPersonName">分派人人名称</param>

        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string ID, string x, string y, [FromBody] Ins_Form_ValveModel value, int? isReport = null, string? eventTypeId = null, string? eventTypeId2 = null, string? eventDesc = null, string? rangName = null, string? execDetpID = null, string? execDetpName = null, string? execPersonId = null, string? execPersonName = null)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_task_id = ID;
            DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //Ins_Event m_Event = new Ins_Event();

            string iadminid = "", iadminame = "";

            if (UniWaterUserInfo != null)
            {
                iadminid = UniWaterUserInfo._id;
                iadminame = UniWaterUserInfo.Name;
                //if (isReport == 1)
                //{
                //    m_Event.UppersonId = UniWaterUserInfo._id;
                //    m_Event.UpName = UniWaterUserInfo.Name;
                //    m_Event.UpDeptId = UniWaterUserInfo.Group;
                //    m_Event.UpDeptName = UniWaterUserInfo.group_data.Name;
                //    m_Event.LinkCall = UniWaterUserInfo.Mobile;
                //    m_Event.LinkMan = UniWaterUserInfo.Name;
                //}
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "uniwater获取用户信息失败！");
            }
            ImageFactory imageFactory = new ImageFactory();
            //将照片存储到/upload/EventsImg  返回url
            string ImagePath = imageFactory.getPictureUrl(value.ImagePath);
            //声明任务完成明细
            Ins_Task_CompleteDetail taskdetailmode = new Ins_Task_CompleteDetail
            {
                TaskId = value.TaskId,
                Devicename = value.LayerName,
                Devicesmid = value.GlobID,
                plan_task_id = value.Plan_task_id,
                x = x,
                y = y,
                Uptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                Peopleid = iadminid,
                PointType = 0,
                IsFeedback = 0,
                IsHidden = 0
            };
            MessageEntity result = _iIns_Form_ValveDAL.PostValve(value, taskdetailmode, ImagePath, x, y, iadminid, iadminame);
            //调用判断任务对应的所有设备是否到位
            DataTable dt = _iIns_TaskManageDAL.GetTaskCount(value.TaskId);
            if (dt != null && dt.Rows.Count > 0)
            {
                int allEquCount = int.Parse(dt.Rows[0]["allEquCount"].ToString());
                int CompletedCount = int.Parse(dt.Rows[0]["CompletedCount"].ToString());
                string proraterId = dt.Rows[0]["proraterId"].ToString();
                //所有设备等于已到位设备量，代表任务已经完成
                if (allEquCount == CompletedCount)
                {
                    //调用完成接口
                    _iIns_TaskManageDAL.TaskCompleted(value.TaskId, proraterId, value.TaskName, 1);
                }

            }
            //if (isReport == 1)
            //{
            //    #region 上报事件
            //    int ExecTime = 36;
            //    DataTable dt1 = _ins_Event_TypeDAL.GetEventExecTime(eventTypeId2);
            //    if (dt1 != null && dt.Rows.Count > 0)
            //    {
            //        ExecTime = int.Parse(dt1.Rows[0]["ExecTime"].ToString());
            //    }

            //    m_Event.EventCode = "XJ";
            //    m_Event.UpTime = dateNow;
            //    m_Event.EventTypeId = eventTypeId;
            //    m_Event.EventTypeId2 = eventTypeId2;
            //    m_Event.EventFromId = 3;
            //    m_Event.UrgencyId = 2;
            //    m_Event.HandlerLevelId = 1;
            //    m_Event.EventDesc = eventDesc;
            //    m_Event.EventX = x;
            //    m_Event.EventY = y;
            //    m_Event.EventUpdateTime = dateNow;
            //    m_Event.IsValid = 1;
            //    m_Event.DeleteStatus = 0;
            //    m_Event.ExecTime = ExecTime;
            //    m_Event.RangName = rangName;
            //    m_Event.Plan_task_id = value.Plan_task_id;
            //    m_Event.EventPictures = ImagePath;
            //    m_Event.EventAddress = value.DetailAddress;
            //    m_Event.EventID = Guid.NewGuid().ToString();

            //    return _eventManage.PostEvent(value.Plan_task_id, m_Event, new Ins_WorkOrder_Oper_History()
            //    {
            //        OperId = 11,
            //        OperTime = dateNow,
            //        ExecPersonId = execPersonId,
            //        ExecPersonName = execPersonName,
            //        DispatchPersonID = m_Event.UppersonId,
            //        DispatchPersonName = m_Event.UpName,
            //        DispatchDetpID = m_Event.UpDeptId,
            //        DispatchDetptName = m_Event.UpDeptName,
            //        ExecDetpID = execDetpID,
            //        ExecDetptName = execDetpName

            //    });
            //    #endregion
            //}

            return result;
        }
    }
}