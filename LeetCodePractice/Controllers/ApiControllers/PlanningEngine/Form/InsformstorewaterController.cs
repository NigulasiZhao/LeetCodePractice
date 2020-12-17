/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/10/29 9:36:28
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
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

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Form
{
    /// <summary>
    ///蓄水设施填单
    /// </summary>
    public class InsFormStorewaterController : BaseController
    {

        private readonly IIns_FormDAL _iIns_FormDAL;
        private readonly IIns_TaskManageDAL _iIns_TaskManageDAL;
        //private readonly IIns_EventDAL _eventManage;
        //private readonly IIns_Event_TypeDAL _ins_Event_TypeDAL;
        public InsFormStorewaterController(IIns_FormDAL iIns_FormDAL, IIns_TaskManageDAL iIns_TaskManageDAL
            /*, IIns_EventDAL eventManage, IIns_Event_TypeDAL ins_Event_TypeDAL*/)
        {
            _iIns_FormDAL = iIns_FormDAL;
            _iIns_TaskManageDAL = iIns_TaskManageDAL;

        }
        /// <summary>
        /// 添加蓄水设施
        /// </summary>
        /// <param name="ID">plan_task_id</param>
        /// <param name="x">上报位置横坐标</param>
        /// <param name="y">上报位置纵坐标</param>
        ///  <param name="imageUrl">已上傳的照片路徑</param>
        /// <param name="value"></param> /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string ID, string x, string y, [FromBody] Ins_Form_StoreWaterModel value, string? imageUrl = null)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            double Xmin = double.Parse(Appsettings.app(new string[] { "CoordinateRange", "XMin" }));
            double XMax = double.Parse(Appsettings.app(new string[] { "CoordinateRange", "XMax" }));
            double YMin = double.Parse(Appsettings.app(new string[] { "CoordinateRange", "YMin" }));
            double YMax = double.Parse(Appsettings.app(new string[] { "CoordinateRange", "YMax" }));

            if (!(double.Parse(x) >= Xmin && double.Parse(x) <= XMax) || !(double.Parse(y) >= YMin && double.Parse(y) <= YMax))
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "坐标信息有误！");
            }
            value.Plan_task_id = ID;

            string iadminid = "", iadminame = "", upDeptId = "", upDeptName = "";

            if (UniWaterUserInfo != null)
            {
                iadminid = UniWaterUserInfo._id;
                iadminame = UniWaterUserInfo.Name;
                upDeptId = UniWaterUserInfo.Group;
                upDeptName = UniWaterUserInfo.group_data.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "uniwater获取用户信息失败！");
            }
            #region 上传照片
            string ImagePath = "";
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl != null)
            {
                ImagePath = imageUrl + "|";
            }
            else
            {
                ImagePath = "";
            }
            ImageFactory imageFactory = new ImageFactory();
            //将照片存储到/upload/EventsImg  返回url
            ImagePath += imageFactory.getPictureUrl(value.ImagePath);
            #endregion
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
            MessageEntity result = _iIns_FormDAL.PostStoreWater(value, taskdetailmode, ImagePath, x, y, iadminid, iadminame, upDeptId, upDeptName);
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