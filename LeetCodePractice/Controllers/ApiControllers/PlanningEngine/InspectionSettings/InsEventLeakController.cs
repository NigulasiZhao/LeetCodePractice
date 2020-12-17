using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GisPlateform.CommonTools;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.CommonTools;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.UniWater;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace GISServerForCore2._0.Controllers.ApiControllers.PlanningEngine.InspectionSettings
{
    /// <summary>
    /// 漏点上报
    /// </summary>
    public class InsEventLeakController : BaseController
    {
        private readonly IIns_Event_LeakDAL _iIns_DAL;
        private readonly IIns_Event_TypeDAL _ins_Event_TypeDAL;
        private readonly IIns_EventDAL _eventManage;


        public InsEventLeakController(IIns_Event_LeakDAL iIns_DAL, IIns_Event_TypeDAL ins_Event_TypeDAL, IIns_EventDAL eventManage)
        {
            _ins_Event_TypeDAL = ins_Event_TypeDAL;
            _iIns_DAL = iIns_DAL;
            _eventManage = eventManage;

        }
        /// <summary>
        /// 漏点上报APP调用：(Headers传递app和token) 
        /// </summary>
        /// <param name="leakPointType">漏点类型 1：明漏  2：暗漏</param>
        /// <param name="leakPipeMaterial">管道材质</param>
        /// <param name="leakGound">漏点地面</param>
        /// <param name="leakPipeDiameter">管道管径</param>
        /// <param name="eventX">横坐标</param>
        /// <param name="eventY">纵坐标</param>
        /// <param name="eventFromId">事件来源id</param>
        /// <param name="eventTypeId">事件类型id</param>
        /// <param name="eventTypeId2">事件内容id</param>
        /// <param name="execDetpID">执行人部门ID</param>
        /// <param name="execDetpName">执行人部门名称</param>
        /// <param name="execPersonId">执行人ID</param>
        /// <param name="execPersonName">执行人名称</param>
        /// <param name="rangName">网络分区名称</param>
        /// <param name="formData"></param>
        /// <param name="plan_Task_Id">任务明细id</param>
        /// <param name="urgencyId">紧急程度id默认1  1：一般 2：紧急 3：加急</param>


        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(int leakPointType,string leakPipeMaterial,string leakGound,string leakPipeDiameter, string eventX, string eventY, int eventFromId,  string eventTypeId, string eventTypeId2, string execDetpID, string execDetpName, string execPersonId, string execPersonName, string rangName, [FromForm] IFormCollection formData, string? plan_Task_Id = null, int urgencyId=1)
        {
            //获取事件内容
            string eventDesc = "", eventAddress = "";
            eventDesc = formData["eventDesc"].ToString();
            eventAddress = formData["eventAddress"].ToString();
            Ins_Event_Leak model = new Ins_Event_Leak();
            model.leakPointType = leakPointType;
            model.LeakPipeMaterial = leakPipeMaterial;
            model.LeakGound = leakGound;
            model.LeakPipeDiameter = leakPipeDiameter;
            model.EventTypeId = eventTypeId;
            model.EventTypeId2 = eventTypeId2;
            model.EventFromId = eventFromId;
            model.RangName = rangName;
            model.EventX = eventX;
            model.EventY = eventY;
            model.Plan_task_id = plan_Task_Id;
            model.EventAddress = eventAddress;
            model.EventDesc = eventDesc;

            DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Ins_Event m_Event = new Ins_Event();
      
            if (model == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            #region uniwater获取
            if (UniWaterUserInfo != null)
            {
                model.UppersonId = UniWaterUserInfo._id;
                model.UpName = UniWaterUserInfo.Name;
                model.UpDeptId = UniWaterUserInfo.Group;
                model.UpDeptName = UniWaterUserInfo.group_data.Name;
                m_Event.UppersonId = UniWaterUserInfo._id;
                m_Event.UpName = UniWaterUserInfo.Name;
                m_Event.UpDeptId = UniWaterUserInfo.Group;
                m_Event.UpDeptName = UniWaterUserInfo.group_data.Name;
                m_Event.LinkCall = UniWaterUserInfo.Mobile;
                m_Event.LinkMan = UniWaterUserInfo.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "uniwater获取用户信息失败！");
            }
            #endregion
            IFormFileCollection files = formData.Files;
            FileFactory file = new FileFactory();
            string Path = file.UploadFiles(files);
            if (Path != "")
            {
                m_Event.EventPictures = Path.Split('-')[0].ToString();
                m_Event.EventVideo = Path.Split('-')[1].ToString();
                model.EventPictures = Path.Split('-')[0].ToString();
                model.EventVideo = Path.Split('-')[1].ToString();
            }
            //ImageFactory imageFactory = new ImageFactory();
            ////将照片存储到/upload/EventsImg  返回url
            //string ImagePath = imageFactory.getPictureUrl(model.Base64Image);
            string EventDesc = "";
            if (model.leakPointType == 1)
                EventDesc += "漏点类型-明漏";
            else
                EventDesc += "漏点类型-暗漏";
            EventDesc += ",管道材质-"+model.LeakPipeMaterial+ ",漏点地面-"+model.LeakGound + ",管道管径-" + model.LeakPipeDiameter+"; 设备信息--"+ eventDesc;
            #region 事件上报
            int ExecTime = 36;
            DataTable dt = _ins_Event_TypeDAL.GetEventExecTime(model.EventTypeId2);
            if (dt != null && dt.Rows.Count > 0)
            {
                ExecTime = int.Parse(dt.Rows[0]["ExecTime"].ToString());
            }
            if (model.EventFromId == 1)
            {
                m_Event.EventCode = "DH";
            }
            else if (model.EventFromId == 2)
            {
                m_Event.EventCode = "RX";
            }
            else if (model.EventFromId == 3)
            {
                m_Event.EventCode = "XJ";
            }
            else
            {
                m_Event.EventCode = "LS";
            }
            m_Event.EventAddress = eventAddress;
            m_Event.UpTime = dateNow;
            m_Event.EventTypeId = model.EventTypeId;
            m_Event.EventTypeId2 = model.EventTypeId2;
            m_Event.EventFromId =eventFromId;
            m_Event.UrgencyId = urgencyId;
            m_Event.HandlerLevelId = 1;
            m_Event.EventDesc = EventDesc;
            m_Event.EventX = model.EventX;
            m_Event.EventY = model.EventY;
            m_Event.EventUpdateTime = dateNow;
            m_Event.IsValid = 1;
            m_Event.DeleteStatus = 0;
            m_Event.ExecTime = ExecTime;
            m_Event.RangName = model.RangName;
            m_Event.EventID = Guid.NewGuid().ToString();
            if (plan_Task_Id != null)
                m_Event.Plan_task_id =plan_Task_Id;
            _eventManage.PostEvent(plan_Task_Id, m_Event, new Ins_WorkOrder_Oper_History()
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
            #endregion
            model.EventID = m_Event.EventID;
            return _iIns_DAL.Post(model);
        }
    }
}