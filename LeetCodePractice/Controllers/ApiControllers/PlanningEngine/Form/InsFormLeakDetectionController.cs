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
    /// 检漏填单(APP)
    /// </summary>
    public class InsFormLeakDetectionController : BaseController
    {
        private readonly IIns_FormDAL _iIns_FormDAL;
        private readonly IIns_TaskManageDAL _iIns_TaskManageDAL;
        //private readonly IIns_EventDAL _eventManage;
        //private readonly IIns_Event_TypeDAL _ins_Event_TypeDAL;


        public InsFormLeakDetectionController(IIns_FormDAL iIns_FormDAL, IIns_TaskManageDAL iIns_TaskManageDAL
            /*, IIns_EventDAL eventManage, IIns_Event_TypeDAL ins_Event_TypeDAL*/)
        {
            _iIns_FormDAL = iIns_FormDAL;
            _iIns_TaskManageDAL = iIns_TaskManageDAL;



        }
        /// <summary>
        /// 检漏 APP调用：(Headers传递app和token) 
        /// <param name="ID">plan_task_id</param>
        /// <param name="x">上报位置横坐标</param>
        /// <param name="y">上报位置纵坐标</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string ID, string x, string y, [FromBody] Ins_Form_LeakDetectionModel value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_task_id = ID;

            string iadminid = "", iadminame = "";
            if (UniWaterUserInfo != null)
            {
                iadminid = UniWaterUserInfo._id;
                iadminame = UniWaterUserInfo.Name;
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
                plan_task_id = value.Plan_task_id,
                x = x,
                y = y,
                Uptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                Peopleid = iadminid,
                PointType = 0,
                IsFeedback = 0,
                IsHidden = 0
            };
            MessageEntity result = _iIns_FormDAL.PostLeakDetection(value, taskdetailmode, ImagePath, x, y, iadminid, iadminame);
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
            return result;
        }
    }
}