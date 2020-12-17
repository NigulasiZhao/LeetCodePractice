using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.UniWater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace GISWaterSupplyAndSewageServer.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// UniWater用户信息
        /// </summary>
        public HdUser UniWaterUserInfo = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //获取Web端用户信息所需参数
            context.HttpContext.Request.Headers.TryGetValue("access_token", out StringValues AccessToken);
            //获取APP端用户信息所需参数
            context.HttpContext.Request.Headers.TryGetValue("app", out StringValues App);
            context.HttpContext.Request.Headers.TryGetValue("token", out StringValues Authorization);
            //动态UniWater地址
            context.HttpContext.Request.Headers.TryGetValue("uniwater_url", out StringValues UniwaterUrl);
            if (!string.IsNullOrEmpty(AccessToken))
            {
                UniWaterHelper uniWaterHelper = new UniWaterHelper();
                UniWaterUserInfo = uniWaterHelper.GetUniWaterInfoForWeb(AccessToken, UniwaterUrl);
            }
            if (!string.IsNullOrEmpty(App) && !string.IsNullOrEmpty(Authorization))
            {
                UniWaterHelper uniWaterHelper = new UniWaterHelper();
                UniWaterUserInfo = uniWaterHelper.GetUniWaterInfoForApp(App, Authorization, UniwaterUrl);
            }
        }
    }
}
