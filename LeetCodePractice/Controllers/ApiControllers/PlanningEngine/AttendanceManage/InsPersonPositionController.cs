using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.AttendanceManage
{
    /// <summary>
    /// 任务轨迹相关(实时上报位置信息)
    /// </summary>
    public class InsPersonPositionController : BaseController
    {
        private readonly IIns_PersonPositionDAL _iIns_P_PositionDAL;
        private readonly IIns_AttendanceDAL _iIns_AttendanceDAL;


        public InsPersonPositionController(IIns_PersonPositionDAL iIns_P_PositionDAL, IIns_AttendanceDAL iIns_AttendanceDAL)
        {
            _iIns_AttendanceDAL = iIns_AttendanceDAL;
            _iIns_P_PositionDAL = iIns_P_PositionDAL;
        }
        /// <summary>
        /// 添加任务轨迹
        /// </summary>
        /// <param name="taskid">任务</param>
        /// <param name="positionX">横坐标</param>
        /// <param name="positionY">纵坐标</param>
        /// <param name="groupID">分组id  1：签到 2：签退</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string taskid, string positionX, string positionY, string groupID)
        {

            Ins_PersonPosition value = new Ins_PersonPosition();
            value.Taskid = taskid;
            value.PositionX = positionX;
            value.PositionY = positionY;
            value.GroupID = groupID;
            if (value == null || string.IsNullOrEmpty(value.Taskid) || string.IsNullOrEmpty(positionX) || string.IsNullOrEmpty(positionY))
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
            //获取最后一条签到数据
            if (value.GroupID == "1")
            {
                MessageEntity messresult = _iIns_AttendanceDAL.GetLastAttendanceByTaskid(value.Taskid);
                List<Ins_Attendance> ptslist = (List<Ins_Attendance>)messresult.Data.Result;
                if (ptslist.Count > 0 && ptslist[0].GroupID == "2")
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "该任务已签退不能上传位置信息！");
                }
            }
            //获取最新轨迹信息
            DataTable dt= _iIns_P_PositionDAL.GetPositionByTaskid(value.Taskid);
            decimal distance = 0;
            DateTime lastuptime = DateTime.Now ;
            DateTime now = DateTime.Now;
            string positionid = "",groupId="1";
            int Distancem = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                distance = decimal.Parse(dt.Rows[0]["distance"].ToString());

                value.Distance = distance;
                lastuptime = DateTime.Parse(dt.Rows[0]["uptime"].ToString());
                positionid = dt.Rows[0]["positionid"].ToString();
                Distancem = (int)Math.Sqrt(Math.Pow(double.Parse(dt.Rows[0]["positionx"].ToString()) - double.Parse(value.PositionX), 2.0) + Math.Pow(double.Parse(dt.Rows[0]["positiony"].ToString()) - double.Parse(value.PositionY), 2.0));
                groupId= dt.Rows[0]["groupId"].ToString();
            }
            //防止轨迹偏移，在以最大值一秒10米计算最大距离
            TimeSpan ts = now.Subtract(lastuptime);
            int sec = (int)ts.TotalSeconds;
            int maxdistance = sec * 10;
            //LogHelper.Info("签到日志Distancem" + Distancem + "---maxdistance:----" + maxdistance);

            //单位米
            if ( Distancem <= maxdistance)
            {
                _iIns_P_PositionDAL.Add(value);
            }
            else//只更新uptime 工作时间
            {
                _iIns_P_PositionDAL.Update(positionid);
            }
            if (dt!=null && dt.Rows.Count > 0)
            {
                //当前任务id执行签到，前一次有签退记录  中间距离和时间不累加
                if (!(dt.Rows[0]["groupid"].ToString() == "2" && value.GroupID=="1"))
                {
                    //计算距离positionx,p.positiony 单位公里
                    //decimal Distance = (decimal)(Math.Round(Distancem / 100d,1) / 10d);
                    if (Distancem > 7 && Distancem < maxdistance)//半径大于7米累加工作距离
                    {
                        _iIns_P_PositionDAL.Update(value.Taskid, dt.Rows[0]["xcminutes"].ToString(), Distancem);
                    }
                   
                }
            }
            return MessageEntityTool.GetMessage(1,true); 
        }
        /// <summary>
        /// 按照人员分组返回人员的最后一次上报位置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetPositionByPersonid()
        {
            var OffLineTime = Appsettings.app(new string[] { "OffLineTime" });
            return _iIns_P_PositionDAL.GetPositionByPersonid(OffLineTime);
        }
    }
}