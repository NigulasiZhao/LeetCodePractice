using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage;
using Microsoft.AspNetCore.Mvc;


namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.AttendanceManage
{
  /// <summary>
  /// 获取我的当天工作时间和公里数
  /// </summary>
    public class InsWorkingTimeDistanceController : BaseController
    {
        private readonly IIns_PersonPositionDAL _iIns_P_PositionDAL;

        public InsWorkingTimeDistanceController(IIns_PersonPositionDAL iIns_P_PositionDAL)
        {
            _iIns_P_PositionDAL = iIns_P_PositionDAL;
        }

        /// <summary>
        /// App获取我的当天工作时间和公里数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity WorkingTimeDistance()
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
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            int hours= int.Parse( DateTime.Now.ToShortTimeString().Split(':')[0].ToString());
            if(hours>=0 && hours <= 3)
            {
                dateNow = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            DateTime startTime =DateTime.Parse( dateNow+ " 03:00:00");
            DateTime endTime = DateTime.Parse(DateTime.Parse(dateNow).AddDays(1).ToString("yyyy-MM-dd") + " 03:00:00");
            return _iIns_P_PositionDAL.GetWorkingTimeDistance(Personid,startTime,endTime);
        }
    }
}