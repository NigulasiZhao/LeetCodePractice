using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using GISWaterSupplyAndSewageServer.App_Authorize;
using Microsoft.Extensions.Primitives;

namespace GISWaterSupplyAndSewageServer.AttributePack
{
    //public class WebApiFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(HttpActionContext filterContext)
    //    {
    //        RequestCheck requestCheck = new RequestCheck();

    //        filterContext.Request.Headers.TryGetValues("Token", out IEnumerable<string> token);

    //        var loginState = requestCheck.RequestLoginStateCheck(token == null ? "" : token.First(), filterContext.Request.RequestUri.LocalPath);

    //        if (ErrorType.Success == loginState)
    //        {
    //        }
    //        else
    //        {
    //            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
    //            response.Content = new StringContent(JsonConvert.SerializeObject(MessageEntityTool.GetMessage(loginState)), Encoding.UTF8, "application/json");
    //            return;

    //        }

    //    }
    //}
    public class WebApiFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RequestCheck requestCheck = new RequestCheck();

            context.HttpContext.Request.Headers.TryGetValue("Token", out StringValues token);
            requestCheck.RequestLoginStateCheck(token.First());
            var loginState = requestCheck.RequestLoginStateCheck(StringValues.IsNullOrEmpty(token) ? "" : token.First());

            if (ErrorType.Success == loginState)
            {
            }
            else
            {
                //var response = context.HttpContext.get = context.HttpContext ?? new HttpResponseMessage();
                //response.Content = new StringContent(JsonConvert.SerializeObject(MessageEntityTool.GetMessage(loginState)), Encoding.UTF8, "application/json");
                //return;

            }
        }

    }
}