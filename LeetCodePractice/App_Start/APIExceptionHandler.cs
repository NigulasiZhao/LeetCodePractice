using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Web;

namespace GISWaterSupplyAndSewageServer.App_Start
{
    public class APIExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            LogHelper.Error(context.Exception.Message, context.Exception);
            var json = MessageEntityTool.GetMessage(ErrorType.SystemError, context.Exception.ToString());
            context.Result = new ObjectResult(json);
        }
    }
}