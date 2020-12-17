using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using GISWaterSupplyAndSewageServer.CommonTools;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GISWaterSupplyAndSewageServer.CommonTools;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Common
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UpLoadFileController : Controller
    {
        ///// <summary>
        ///// 上传文件(以base64格式上传,上传格式:base64|.后缀名)
        ///// </summary>
        ///// <returns></returns>
        //public MessageEntity UpLoad1([FromBody]string[] base64)
        //{
        //    string eventurl = string.Empty;
        //    FileFactory fileFactory = new FileFactory();
        //    //将文件存储到/upload/EventsImg  返回url
        //    eventurl = fileFactory.getFileUrl(base64);
        //    if (eventurl != null && eventurl.Contains("/"))
        //    {
        //        return MessageEntityTool.GetMessage(ErrorType.Success, eventurl);
        //    }
        //    else
        //    {
        //        return MessageEntityTool.GetMessage(ErrorType.SystemError, eventurl);
        //    }

        //}

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity UpLoad(List<IFormFile> files)
        {
            //var files = Request.Form.Files;
            Random random = new Random();

            if (Request.Form.Files.Count > 0)
            {
                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    var file = Request.Form.Files[i];
                    if (file.Length > 0)
                    {
                        if (file.Length > Convert.ToInt32(Appsettings.app(new string[] { "FileSize" })))
                        {
                            return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "文件大小超出限制");
                        }
                        DateTime dateNow = DateTime.Now;
                        //全路径 
                        string FullFullName = file.FileName;

                        string relativeDir = "uploadFile/EventsImg/" + dateNow.Year + "/" + dateNow.Month + "/" + dateNow.Day;
                        string imageName = string.Format("{0:yyyyMMddHHmmssfff}", dateNow);
                        string AllPath = string.Format("{0}", relativeDir);

                        string relativePath = string.Format("{0}/{1}", relativeDir, imageName + FullFullName);
                        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath)))
                        {
                            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath));
                        }
                        using (var stream = new FileStream(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath), imageName + FullFullName), FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        return MessageEntityTool.GetMessage(1, null, true, relativePath);

                    }
                    else
                    {
                        return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "文件内容为空");
                    }
                }
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "文件为空");



            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "没有选择文件");
            }
        }
    }
}
