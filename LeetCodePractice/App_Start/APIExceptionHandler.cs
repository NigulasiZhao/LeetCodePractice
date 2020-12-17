using LeetCodePractice.CommonTools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Web;

namespace LeetCodePractice.App_Start
{
    public class APIExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
        }
    }
}