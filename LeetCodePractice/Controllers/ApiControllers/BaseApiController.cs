using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.CommonTools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GISWaterSupplyAndSewageServer.Controllers
{
    public class BaseApiController : Controller
    {
        //  /// <summary>
        ///// 公共方法接口
        ///// </summary>
        //  public readonly ICommonDAL CommonDAL;
        //  /// <summary>
        //  /// BaseController构造函数
        //  /// </summary>
        //  /// <param name="commonDAL"></param>
        //  public BaseApiController(ICommonDAL commonDAL)
        //  {
        //      CommonDAL = commonDAL;
        //  }
        [HttpGet]
        public ActionResult Index()
        {
            return Content("服务器运行中");
        }
    }
}