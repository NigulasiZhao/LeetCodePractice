using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.UniWater;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.InspectionSettings
{
    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    public class InsGetUserInfoController : BaseController
    {
        /// <summary>
        /// 获取当前用户信息 APP调用：(Headers传递app和token) PC端调用：(Headers传递access_token)
        /// </summary>
        /// <param name="systemType">1:PC   2:APP</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string systemType)
        {
            #region uniwater获取用户信息
            if (UniWaterUserInfo != null)
            {
                return MessageEntityTool.GetMessage(1, UniWaterUserInfo);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "access_token失效，获取用户信息失败！");
            }
            #endregion
        }
    }
}