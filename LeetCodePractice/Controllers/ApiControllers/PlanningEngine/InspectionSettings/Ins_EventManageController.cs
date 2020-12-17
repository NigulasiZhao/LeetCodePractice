using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GisPlateform.CommonTools;
using GisPlateform.Model;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.CommonTools;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.UniWater;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace GISServerForCore2._0.Controllers.ApiControllers.PlanningEngine.InspectionSettings
{
    /// <summary>
    /// 事件管理
    /// </summary>
    public class Ins_EventManageController : BaseController
    {
        private readonly IIns_EventDAL _eventManage;
        private readonly IIns_Event_TypeDAL _ins_Event_TypeDAL;


        public Ins_EventManageController(IIns_EventDAL eventManage, IIns_Event_TypeDAL ins_Event_TypeDAL)
        {
            _eventManage = eventManage;
            _ins_Event_TypeDAL = ins_Event_TypeDAL;
        }
        /// <summary>
        /// 事件工单查询和(个人处理-待办处理--(Headers传递access_token))查询
        /// </summary>
        ///  <param name="systemType">1:PC   2:APP </param>
        /// <param name="startTime">开始时间YYYY-MM-DD</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="EventFromId">事件来源3:巡检上报 默认空查全部</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="DeptId">上报部门id</param>
        /// <param name="EventContenct">查询条件(事件来源:类型、上报人、编号)</param>
        /// <param name="IsNodealt">是否待办 空代表全部，1代表待办页面</param>
        /// <param name="X">当前X坐标</param>
        /// <param name="Y">当前Y坐标</param>
        /// <param name="EventID">事件ID</param>
        /// <param name="OperId">事件处理状态步骤ID 0:无效 2:待接受  3:待处置 4到场 5:处置中  5:延期确认  6:待审核  8:审核完成  11:待处理    null:待分派</param>
        /// <param name="IsValid">是否有效   1有效   0无效  2 退回  3 回复 4:退单 5延期 6 延期确认</param>
        /// <param name="sort">排序字段默认EventID</param>
        /// <param name="ordering">desc/asc</param>
        /// <param name="num">默认20</param>
        /// <param name="page">默认1</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string systemType = "1", DateTime? startTime = null, DateTime? endTime = null, string? rangName = null, int? EventFromId = null, string? eventType = null, string? EventID = null, string OperId = "", int? IsValid = null, string? DeptId = null, string EventContenct = "", string IsNodealt = null, string X = null, string Y = null, string sort = "uptime", string ordering = "desc", int num = 20, int page = 1)
        {
            string ExecPersonId = null;
            if (IsNodealt != null)
            {
                if (UniWaterUserInfo != null)
                {
                    ExecPersonId = UniWaterUserInfo._id;
                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
                }
            }
            if (endTime != null)
                endTime = endTime.Value.AddDays(1).AddSeconds(-1);

            var messageEntity = _eventManage.GetEventWorkorderListForMaintain(startTime, endTime, rangName, EventFromId, eventType, EventID, OperId, IsValid, DeptId, EventContenct, ExecPersonId, X, Y, sort, ordering, num, page);

            return messageEntity;
        }
        /// <summary>
        /// 事件工单处理步骤信息
        /// </summary>
        /// <param name="eventID">事件id</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventWorkorderStepForMaintain(string eventID)
        {
            return _eventManage.GetEventWorkorderStepForMaintain(eventID);
        }

        /// <summary>
        /// 个人处理-已办处理(传递)(Headers传递access_token)
        /// </summary>
        ///  <param name="systemType">1:PC   2:APP </param>
        /// <param name="eventType">事件类型</param>
        /// <param name="OperId">事件处理状态步骤ID 0:无效 2:待接受  3:待处置 4到场 5:处置中  5:延期确认  6:待审核  8:审核完成  11:待处理  null:待分派</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="sort">排序字段默认EventID</param>
        /// <param name="ordering">desc/asc</param>
        /// <param name="num">默认20</param>
        /// <param name="page">默认1</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventListOwn(string systemType = "1", string? eventType = null, string? OperId = null, DateTime? startTime = null, DateTime? endTime = null, string sort = "eventUpdateTime", string ordering = "desc", int num = 20, int page = 1)
        {
            string ExecPersonId = null;
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                ExecPersonId = UniWaterUserInfo._id;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            if (endTime != null)
                endTime = endTime.Value.AddDays(1).AddSeconds(-1);
            var messageEntity = _eventManage.GetEventListOwn(ExecPersonId, eventType, OperId, startTime, endTime, sort, ordering, num, page);

            return messageEntity;
        }
        #region 旧事件上报
        //public MessageEntity Post(string systemType,  [FromBody]Ins_Event value, string? plan_Task_Id= null)
        //{
        //    if (value == null)
        //    {
        //        return MessageEntityTool.GetMessage(ErrorType.FieldError);
        //    }
        //    #region uniwater获取用户信息
        //    if (systemType == "1")
        //    {
        //        string access_token = HttpContext.Request.Headers["access_token"];
        //        string UniwaterUrl = Appsettings.app(new string[] { "UniwaterUrl" });
        //        string data = JsonConvert.SerializeObject(new
        //        {
        //            access_token = access_token,
        //            type = "user"
        //        });
        //        string url = UniwaterUrl + "/hdl/oauth/v1.0/access_check.json";
        //        UniWaterRequest request = new UniWaterRequest();
        //        var responseData = request.PostResponse(url, data).Result;
        //        var resault = JsonConvert.DeserializeObject(responseData);
        //        UserAccessResult accessResult = JsonConvert.DeserializeObject<UserAccessResult>(responseData);
        //        if (accessResult != null && accessResult.User != null)
        //        {
        //            value.UppersonId = accessResult.User._id;
        //            value.UpName = accessResult.User.Name;
        //            value.UpDeptId = accessResult.User.Group;
        //            value.UpDeptName = accessResult.User.group_data.Name;
        //        }
        //        else
        //        {
        //            return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "access_token失效，获取用户信息失败！");
        //        }
        //    }
        //    else
        //    {
        //        string app = HttpContext.Request.Headers["app"];
        //        string Authorization = HttpContext.Request.Headers["token"];
        //        //string app = "5d9c5eeff22b41096ced9569";
        //        //string Authorization = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2MiOiJhZG1pbiIsImNpZCI6IjVkOWM1ZWVmZjIyYjQxMDk2Y2VkOTU2OSIsImV4cCI6MTYyMzg5NjU1NiwidWlkIjoiNWQ5YzYxYjRmMjJiNDEwOTZjZWQ5NTgxIn0.p6V_uJoFVA9NF7t2DDKeWfQ00SCfWkzFfe6WqX3gvJE";
        //        string UniwaterUrl = Appsettings.app(new string[] { "UniwaterUrl" });
        //        string url = UniwaterUrl + "/app/v1.0/userinfo.json";
        //        UniWaterRequest request = new UniWaterRequest();
        //        var responseData = request.PostResponse(url, "{}", Authorization, app).Result;
        //        var resault = JsonConvert.DeserializeObject(responseData);
        //        HdUser user = JsonConvert.DeserializeObject<HdUser>(((dynamic)resault).Response.ToString());
        //        if (user != null)
        //        {
        //            value.UppersonId = user._id;
        //            value.UpName = user.Name;
        //            value.UpDeptId = user.Group;
        //            value.UpDeptName =user.group_data.Name;;
        //        }
        //        else
        //        {
        //            return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "uniwater获取用户信息失败！");
        //        }
        //    }
        //    #endregion
        //    DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    Ins_Event m_Event = new Ins_Event();
        //    #region
        //    ImageFactory imageFactory = new ImageFactory();
        //    //将照片存储到/upload/EventsImg  返回url
        //    m_Event.EventPictures = imageFactory.getPictureUrl(value.Base64Image);
        //    if (m_Event.EventPictures != null && !m_Event.EventPictures.Contains("/"))
        //    {
        //        return MessageEntityTool.GetMessage(ErrorType.SystemError, m_Event.EventPictures);
        //    }

        //    //m_Event.EventAudio= imageFactory.getPictureUrl(value.Base64Audio);
        //    //if (m_Event.EventAudio != null && !m_Event.EventAudio.Contains("/"))
        //    //{
        //    //    return MessageEntityTool.GetMessage(ErrorType.SystemError, m_Event.EventAudio);
        //    //}
        //    //m_Event.EventVideo = imageFactory.getPictureUrl(value.Base64Video);
        //    //if (m_Event.EventVideo != null && !m_Event.EventVideo.Contains("/"))
        //    //{
        //    //    return MessageEntityTool.GetMessage(ErrorType.SystemError, m_Event.EventVideo);
        //    //}
        //    #endregion
        //    int ExecTime = 36;
        //    DataTable dt = _ins_Event_TypeDAL.GetEventExecTime(value.EventTypeId);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        ExecTime = int.Parse(dt.Rows[0]["ExecTime"].ToString());
        //    }
        //    if (value.EventFromId == 1)
        //    {
        //        m_Event.EventCode = "DH";
        //    }
        //    else if (value.EventFromId == 2)
        //    {
        //        m_Event.EventCode = "RX";
        //    }
        //    else if (value.EventFromId == 3)
        //    {
        //        m_Event.EventCode = "XJ";
        //    }
        //    else
        //    {
        //        m_Event.EventCode = "LS";
        //    }
        //    m_Event.EventAddress = value.EventAddress;
        //    m_Event.UpTime = dateNow;
        //    m_Event.UppersonId = value.UppersonId;
        //    m_Event.UpName = value.UpName;
        //    m_Event.UpDeptId = value.UpDeptId;
        //    m_Event.UpDeptName = value.UpDeptName;
        //    m_Event.EventTypeId = value.EventTypeId;
        //    m_Event.EventTypeId2 = value.EventTypeId2;
        //    m_Event.EventFromId = value.EventFromId;
        //    m_Event.UrgencyId = value.UrgencyId;
        //    m_Event.HandlerLevelId = 1;
        //    m_Event.EventDesc = value.EventDesc;
        //    m_Event.EventX = value.EventX;
        //    m_Event.EventY = value.EventY;
        //    m_Event.EventUpdateTime = dateNow;
        //    m_Event.IsValid = 1;
        //    m_Event.DeleteStatus = 0;
        //    m_Event.ExecTime = ExecTime;
        //    m_Event.LinkMan = value.LinkMan;
        //    m_Event.LinkCall = value.LinkCall;

        //    return _eventManage.PostEvent(plan_Task_Id,m_Event, new Ins_WorkOrder_Oper_History()
        //    {
        //        OperId = 11,
        //        OperTime = dateNow,
        //        ExecPersonId =value.ExecPersonId,
        //        ExecPersonName=value.ExecPersonName,
        //        DispatchPersonID = value.UppersonId,
        //        DispatchPersonName=value.UpName,
        //        DispatchDetpID = value.UpDeptId,
        //        DispatchDetptName = value.UpDeptName,
        //        ExecDetpID = value.ExecDetpID,
        //        ExecDetptName=value.ExecDetptName

        //    });
        //}
        #endregion
        /// <summary>
        ///  新增事件(上报事件11)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventFromId">事件来源id</param>
        /// <param name="urgencyId">紧急程度id</param>
        /// <param name="eventTypeId">事件类型id</param>
        /// <param name="eventTypeId2">事件内容id</param>
        /// <param name="linkMan">联系人</param>
        /// <param name="linkCall">联系电话</param>
        /// <param name="eventX">横坐标</param>
        /// <param name="eventY">纵坐标</param>
        /// <param name="execDetpID">执行人部门ID</param>
        /// <param name="execDetpName">执行人部门名称</param>
        /// <param name="execPersonId">执行人ID</param>
        /// <param name="execPersonName">执行人名称</param>
        /// <param name="rangName">网络分区名称</param>
        /// <param name="formData"></param>
        /// <param name="plan_Task_Id">任务明细id</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public MessageEntity Post(string systemType, int eventFromId, int urgencyId, string eventTypeId, string eventTypeId2, string linkMan, string linkCall, string eventX, string eventY, string execDetpID, string execDetpName, string execPersonId, string execPersonName, string rangName, [FromForm] IFormCollection formData, string? plan_Task_Id = null)
        {
            if (formData == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            //获取事件内容
            string eventDesc = "", eventAddress = "";
            eventDesc = formData["eventDesc"].ToString();
            eventAddress = formData["eventAddress"].ToString();
            DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Ins_Event m_Event = new Ins_Event();
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                m_Event.UppersonId = UniWaterUserInfo._id;
                m_Event.UpName = UniWaterUserInfo.Name;
                m_Event.UpDeptId = UniWaterUserInfo.Group;
                m_Event.UpDeptName = UniWaterUserInfo.group_data.Name;
                m_Event.LinkCall = UniWaterUserInfo.Mobile;
                m_Event.LinkMan = UniWaterUserInfo.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion

            #region 上传文件
            IFormFileCollection files = formData.Files;
            FileFactory file = new FileFactory();
            string Path =  file.UploadFiles(files);
            if (Path != "")
            {
                m_Event.EventPictures = Path.Split('-')[0].ToString();
                m_Event.EventVideo = Path.Split('-')[1].ToString();

            }
            #endregion

            int ExecTime = 36;
            DataTable dt = _ins_Event_TypeDAL.GetEventExecTime(eventTypeId2);
            if (dt != null && dt.Rows.Count > 0)
            {
                ExecTime = int.Parse(dt.Rows[0]["ExecTime"].ToString());
            }
            if (eventFromId == 1)
            {
                m_Event.EventCode = "DH";
            }
            else if (eventFromId == 2)
            {
                m_Event.EventCode = "RX";
            }
            else if (eventFromId == 3)
            {
                m_Event.EventCode = "XJ";
            }
            else
            {
                m_Event.EventCode = "LS";
            }
            m_Event.EventAddress = eventAddress;
            m_Event.UpTime = dateNow;

            m_Event.EventTypeId = eventTypeId;
            m_Event.EventTypeId2 = eventTypeId2;
            m_Event.EventFromId = eventFromId;
            m_Event.UrgencyId = urgencyId;
            m_Event.HandlerLevelId = 1;
            m_Event.EventDesc = eventDesc;
            m_Event.EventX = eventX;
            m_Event.EventY = eventY;
            m_Event.EventUpdateTime = dateNow;
            m_Event.IsValid = 1;
            m_Event.DeleteStatus = 0;
            m_Event.ExecTime = ExecTime;
            m_Event.RangName = rangName;
            m_Event.EventID = Guid.NewGuid().ToString();
            if (plan_Task_Id != null)
                m_Event.Plan_task_id = plan_Task_Id;


            return _eventManage.PostEvent(plan_Task_Id, m_Event, new Ins_WorkOrder_Oper_History()
            {
                OperId = 11,
                OperTime = dateNow,
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = m_Event.UppersonId,
                DispatchPersonName = m_Event.UpName,
                DispatchDetpID = m_Event.UpDeptId,
                DispatchDetptName = m_Event.UpDeptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName

            });

        }


        /// <summary>
        ///  事件工单流转操作（分派工单2)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="execDetpID">执行人部门ID</param>
        /// <param name="execDetpName">执行人部门名称</param>
        /// <param name="execPersonId">执行人ID</param>
        /// <param name="execPersonName">执行人名称</param>
        /// <param name="execTime">处理时间（单位：小时)</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListAssign(string systemType, string eventID, string execDetpID, string execDetpName, string execPersonId, string execPersonName, string execTime)
        {
            string dispatchPersonID = "", dispatchPersonName = "";

            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion

            string StepNum = "2";
            //是否允许分派 是否分派过
            if (_eventManage.IsExecute(eventID, StepNum, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已被分派，不能多次执行", "提示");
                }
            }

            return _eventManage.WorkListAssign(eventID, execTime, new Ins_WorkOrder_Oper_History()
            {
                OperId = 2,
                EventID = eventID,
                OperTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = dispatchPersonID,
                DispatchPersonName = dispatchPersonName,
                DispatchDetpID = dispatchDetpID,
                DispatchDetptName = dispatchDetptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName,
                IsValid = 1
            });
        }
        /// <summary>
        /// 事件工单流转操作（接单3)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="operRemarks">操作意见</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListReceipt(string systemType, string eventID, string operRemarks = "")
        {
            string execDetpID = "", execDetpName = "", execPersonId = "", execPersonName = "", dispatchPersonID = "", dispatchPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name; ;
                execPersonId = UniWaterUserInfo._id;
                execPersonName = UniWaterUserInfo.Name;
                execDetpID = UniWaterUserInfo.Group;
                execDetpName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion

            string StepNum = "3";
            //是否允许接单
            if (_eventManage.IsExecute(eventID, StepNum, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行接单，不能多执行", "提示");
                }
            }

            return _eventManage.WorkListReceipt(eventID, new Ins_WorkOrder_Oper_History()
            {
                OperId = int.Parse(StepNum),
                EventID = eventID,
                OperTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = dispatchPersonID,
                DispatchPersonName = dispatchPersonName,
                DispatchDetpID = dispatchDetpID,
                DispatchDetptName = dispatchDetptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName,
                IsValid = 1,
                OperRemarks = operRemarks
            });
        }
        /// <summary>
        /// 事件工单流转操作（到场4)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="base64Image">上传照片</param> 
        /// <param name="operRemarks">操作意见</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListPresent(string systemType, string eventID, [FromBody] string[] base64Image, string operRemarks = "")
        {
            string execDetpID = "", execDetpName = "", execPersonId = "", execPersonName = "", dispatchPersonID = "", dispatchPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name; ;
                execPersonId = UniWaterUserInfo._id;
                execPersonName = UniWaterUserInfo.Name;
                execDetpID = UniWaterUserInfo.Group;
                execDetpName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            string StepNum = "4";
            string eventPictures = string.Empty;
            ImageFactory imageFactory = new ImageFactory();
            //将照片存储到/upload/EventsImg  返回url
            eventPictures = imageFactory.getPictureUrl(base64Image);
            if (eventPictures != null && !eventPictures.Contains("/"))
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, eventPictures);
            }
            //是否到场过
            if (_eventManage.IsExecute(eventID, StepNum, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行到场，不能多次执行", "提示");
                }
            }
            return _eventManage.WorkListPresent(eventID, new Ins_WorkOrder_Oper_History()
            {
                OperId = int.Parse(StepNum),
                EventID = eventID,
                OperTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = dispatchPersonID,
                DispatchPersonName = dispatchPersonName,
                DispatchDetpID = dispatchDetpID,
                DispatchDetptName = dispatchDetptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName,
                IsValid = 1,
                OperRemarks = operRemarks,
                Pictures = eventPictures
            });
        }
        /// <summary>
        /// 事件工单流转操作（处置5)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="base64Image">上传照片</param>     
        /// <param name="operRemarks">操作意见</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListChuZhi(string systemType, string eventID, [FromBody] string[] base64Image, string operRemarks = "")
        {
            string execDetpID = "", execDetpName = "", execPersonId = "", execPersonName = "", dispatchPersonID = "", dispatchPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name; ;
                execPersonId = UniWaterUserInfo._id;
                execPersonName = UniWaterUserInfo.Name;
                execDetpID = UniWaterUserInfo.Group;
                execDetpName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            string StepNum = "5";
            string eventPictures = string.Empty;
            ImageFactory imageFactory = new ImageFactory();
            //将照片存储到/upload/EventsImg  返回url
            eventPictures = imageFactory.getPictureUrl(base64Image);
            if (eventPictures != null && !eventPictures.Contains("/"))
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, eventPictures);
            }
            //是否到场过
            if (_eventManage.IsExecute(eventID, StepNum, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行处置，不能多次执行", "提示");
                }
            }
            return _eventManage.WorkListChuZhi(eventID, new Ins_WorkOrder_Oper_History()
            {
                OperId = int.Parse(StepNum),
                EventID = eventID,
                OperTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = dispatchPersonID,
                DispatchPersonName = dispatchPersonName,
                DispatchDetpID = dispatchDetpID,
                DispatchDetptName = dispatchDetptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName,
                IsValid = 1,
                OperRemarks = operRemarks,
                Pictures = eventPictures
            });
        }

        /// <summary>
        /// 事件工单流转操作（完工6)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="base64Image">上传照片</param>    
        /// <param name="operRemarks">操作意见</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListFinished(string systemType, string eventID, [FromBody] string[] base64Image, string operRemarks = "")
        {
            string execDetpID = "", execDetpName = "", execPersonId = "", execPersonName = "", dispatchPersonID = "", dispatchPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name; ;
                execPersonId = UniWaterUserInfo._id;
                execPersonName = UniWaterUserInfo.Name;
                execDetpID = UniWaterUserInfo.Group;
                execDetpName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            string StepNum = "6";
            string eventPictures = string.Empty;
            ImageFactory imageFactory = new ImageFactory();
            //将照片存储到/upload/EventsImg  返回url
            eventPictures = imageFactory.getPictureUrl(base64Image);
            if (eventPictures != null && !eventPictures.Contains("/"))
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, eventPictures);
            }
            //是否到场过
            if (_eventManage.IsExecute(eventID, StepNum, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行处置，不能多次执行", "提示");
                }
            }
            return _eventManage.WorkListFinished(eventID, new Ins_WorkOrder_Oper_History()
            {
                OperId = int.Parse(StepNum),
                EventID = eventID,
                OperTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = dispatchPersonID,
                DispatchPersonName = dispatchPersonName,
                DispatchDetpID = dispatchDetpID,
                DispatchDetptName = dispatchDetptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName,
                IsValid = 1,
                OperRemarks = operRemarks,
                Pictures = eventPictures
            });
        }
        /// <summary>
        /// 事件工单流转操作（审核7 8)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>    
        /// <param name="operRemarks">操作意见</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListAudit(string systemType, string eventID, string operRemarks = "")
        {
            string execDetpID = "", execDetpName = "", execPersonId = "", execPersonName = "", dispatchPersonID = "", dispatchPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name; ;
                execPersonId = UniWaterUserInfo._id;
                execPersonName = UniWaterUserInfo.Name;
                execDetpID = UniWaterUserInfo.Group;
                execDetpName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            string StepNum = "7";
            //是否到场过
            if (_eventManage.IsExecute(eventID, "8", out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行处置，不能多次执行", "提示");
                }
            }
            return _eventManage.WorkListAudit(eventID, new Ins_WorkOrder_Oper_History()
            {
                OperId = int.Parse(StepNum),
                EventID = eventID,
                OperTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = dispatchPersonID,
                DispatchPersonName = dispatchPersonName,
                DispatchDetpID = dispatchDetpID,
                DispatchDetptName = dispatchDetptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName,
                IsValid = 1,
                OperRemarks = operRemarks
            });
        }
        /// <summary>
        /// 事件工单流转操作（延期)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="operRemarks">延期完成说明</param>
        /// <param name="complishTime">延期完成时间</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WordListDelay(string systemType, string eventID, string operRemarks, string complishTime)
        {
            string dispatchPersonID = "", dispatchPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name; 
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            //延期
            string isValid = "5";
            //是否延期过
            if (_eventManage.IsValid(eventID, isValid, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行延期，不能多次执行", "提示");
                }
            }
            //判断同一天是否执行多次延期
            DataTable dt = _eventManage.IsExistPostponeOrderByDay(eventID, complishTime);
            if (dt.Rows.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "同一个工单在同一天不可延期多次！");
            }
            return _eventManage.WordListDelay(eventID, operRemarks, complishTime, dispatchPersonID, dispatchPersonName, dispatchDetpID, dispatchDetptName);
        }
        /// <summary>
        /// 事件工单流转操作（延期确认)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="complishTime">延期完成时间</param>
        ///  <param name="OperRemarks">操作意见</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListDelayExec(string systemType, string eventID, string complishTime, string OperRemarks = "")
        {
            string dispatchPersonID = "", dispatchPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            //延期确认
            string isValid = "6";
            //是否延期确认过
            if (_eventManage.IsValid(eventID, isValid, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行延期确认，不能多次执行", "提示");
                }
            }
            return _eventManage.WorkListDelayExec(eventID, complishTime, dispatchPersonID, dispatchPersonName, dispatchDetpID, dispatchDetptName, OperRemarks);
        }

        /// <summary>
        /// 事件工单流转操作（延期确认退回)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="complishTime">延期完成时间</param>
        ///  <param name="OperRemarks">操作意见</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkListDelayReturn(string systemType, string eventID, string complishTime, string OperRemarks = "")
        {
            string dispatchPersonID = "", dispatchPersonName = "", dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchPersonID = UniWaterUserInfo._id;
                dispatchPersonName = UniWaterUserInfo.Name;
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            //延期确认
            string isValid = "7";
            //是否延期确认过
            if (_eventManage.IsValid(eventID, isValid, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行延期退回，不能多次执行", "提示");
                }
            }
            return _eventManage.WorkListDelayReturn(eventID, complishTime, dispatchPersonID, dispatchPersonName, dispatchDetpID, dispatchDetptName, OperRemarks);
        }
        /// <summary>
        ///  事件工单流转操作(退单)APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="backDesc">退单备注</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WordListBackExec(string systemType, string eventID, string backDesc)
        {
            string iAdminID = "", iAdminName = "", detpID = "", detpName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                iAdminID = UniWaterUserInfo._id;
                iAdminName = UniWaterUserInfo.Name;
                detpID = UniWaterUserInfo.Group;
                detpName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            //退单
            string isValid = "4";
            //是否退单过
            if (_eventManage.IsValid(eventID, isValid, out string errorMsg))
            {
                if (errorMsg != "")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, errorMsg, "提示");

                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该任务已执行退单，不能多次执行", "提示");
                }
            }
            return _eventManage.WordListBackExec(eventID, iAdminID, iAdminName, detpID, detpName, backDesc);
        }

        /// <summary>
        /// 事件工单作废APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="OperId">步骤ID 0:无效 1: 待处理 2:待接受  3:待处置 4 5:处置中  5:延期确认  6:待审核  7:审核完成  11:待处理  12:回复完成  null:待分派</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WorkorderInvalid(string systemType, string eventID, string OperId = "")
        {
            string execDetpID = "", execDetpName = "", execPersonId = "", execPersonName = "";
            string dispatchDetpID = "", dispatchDetptName = "";
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                dispatchDetpID = UniWaterUserInfo.Group;
                dispatchDetptName = UniWaterUserInfo.group_data.Name; ;
                execPersonId = UniWaterUserInfo._id;
                execPersonName = UniWaterUserInfo.Name;
                execDetpID = UniWaterUserInfo.Group;
                execDetpName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
            #endregion
            return _eventManage.WorkorderInvalid(eventID, new Ins_WorkOrder_Oper_History()
            {
                OperId = int.Parse(OperId),
                EventID = eventID,
                OperTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ExecPersonId = execPersonId,
                ExecPersonName = execPersonName,
                DispatchPersonID = execPersonId,
                DispatchPersonName = execPersonName,
                DispatchDetpID = dispatchDetpID,
                DispatchDetptName = dispatchDetptName,
                ExecDetpID = execDetpID,
                ExecDetptName = execDetpName
            });
        }


    }
}