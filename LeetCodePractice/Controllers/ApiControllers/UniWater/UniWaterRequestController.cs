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
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.UniWater
{
    
    public class UniWaterRequestController : Controller
    {
        ///// <summary>
        ///// 应用授权(2.1.1)
        ///// </summary>
        ///// <returns></returns>
        //public string Access(string url, string jsonParas)
        //{
        //    MessageEntity en = new MessageEntity();
        //    string eventPictures = string.Empty;
        //    UniWaterRequest uniWaterRequest = new UniWaterRequest();
        //    return uniWaterRequest.Post(url, jsonParas);
        //}
        ///// <summary>
        ///// 凭据刷新(2.1.2)
        ///// </summary>
        ///// <returns></returns>
        //public string Credentialrefresh(string url, string jsonParas)
        //{
        //    MessageEntity en = new MessageEntity();
        //    string eventPictures = string.Empty;
        //    UniWaterRequest uniWaterRequest = new UniWaterRequest();
        //    return uniWaterRequest.Post(url, jsonParas);
        //}
        ///// <summary>
        ///// 授权登录页(2.2.1)
        ///// </summary>
        ///// <returns></returns>
        //public string AccessLogin(string url)
        //{
        //    MessageEntity en = new MessageEntity();
        //    string eventPictures = string.Empty;
        //    UniWaterRequest uniWaterRequest = new UniWaterRequest();
        //    return uniWaterRequest.HttpGet(url);
        //}
        ///// <summary>
        ///// 获取用户Access_token(2.2.1)
        ///// </summary>
        ///// <returns></returns>
        //public string GetAccess_token(string url, string jsonParas)
        //{
        //    MessageEntity en = new MessageEntity();
        //    string eventPictures = string.Empty;
        //    UniWaterRequest uniWaterRequest = new UniWaterRequest();
        //    return uniWaterRequest.Post(url, jsonParas);
        //}
        ///// <summary>
        ///// 获取组织架构(3.1)
        ///// </summary>
        ///// <returns></returns>
        //public string GetOrganization(string url, string jsonParas)
        //{
        //    MessageEntity en = new MessageEntity();
        //    string eventPictures = string.Empty;
        //    UniWaterRequest uniWaterRequest = new UniWaterRequest();
        //    return uniWaterRequest.Post(url, jsonParas);
        //}
        ///// <summary>
        ///// 获取改组织架构下用户信息(3.2)
        ///// </summary>
        ///// <returns></returns>
        //public string GetUserInfo(string url, string jsonParas)
        //{
        //    MessageEntity en = new MessageEntity();
        //    string eventPictures = string.Empty;
        //    UniWaterRequest uniWaterRequest = new UniWaterRequest();
        //    return uniWaterRequest.Post(url, jsonParas);
        //}
    }
}
