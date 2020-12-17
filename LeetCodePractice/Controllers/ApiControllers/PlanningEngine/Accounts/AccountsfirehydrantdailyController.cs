/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/28 11:01:58
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine
{
    /// <summary>
    ///消火栓日常巡查记录表接口
    /// </summary>
    public class AccountsFireHydrantDailyController : BaseController
    {

        private readonly IAccounts_FirehydrantdailyDAL _iAccounts_FirehydrantdailyDAL;
        private readonly RedisHelper _redisHelper;
        public AccountsFireHydrantDailyController(IAccounts_FirehydrantdailyDAL iAccounts_FirehydrantdailyDAL, RedisHelper redisHelper)
        {

            _iAccounts_FirehydrantdailyDAL = iAccounts_FirehydrantdailyDAL;
            _redisHelper = redisHelper;
        }
        [HttpGet]
        public MessageEntity TestRedis()
        {
            MessageEntity Me = new MessageEntity();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            RedisValue[] Result = _redisHelper.TestHash();
            sw.Stop();

            Me.Data = new DataInfo();
            Me.Data.Result = JsonConvert.SerializeObject(Result);
            Me.ExceptionMsg = ((float)sw.ElapsedMilliseconds / (float)1000).ToString();
            return Me;
            //_redisHelper.HashSet("ZLTest", "id", "");
            //_redisHelper.StringSet("ZL", "123123123");
            ////List<Accounts_Firehydrantdaily> list = new List<Accounts_Firehydrantdaily>();
            ////_redisHelper.SetBatchData(Guid.NewGuid().ToString(), list);
            //return _redisHelper.StringGet("ZL");
        }
        /// <summary>
        /// 添加消火栓日常巡查记录表
        /// </summary>
        /// <param name="firehydrantaddress">所在道路</param>
        /// <param name="firehydrantcode">设备编号</param>
        /// <param name="firehydrantvender">厂家</param>
        /// <param name="iscomplete">是否完好 0否1是</param>
        /// <param name="isdirty">是否污损 0否1是</param>
        /// <param name="isfillbrand">是否补订牌 0否1是</param>
        /// <param name="isfixedbrand">是否未钉牌 0否1是</param>
        /// <param name="isheight">是否过高 0否1是</param>
        /// <param name="islackconnect">是否缺连接扣 0否1是</param>
        /// <param name="isleakwater">是否漏水 0否1是</param>
        /// <param name="ismisscover">是否缺盖 0否1是</param>
        /// <param name="isold">是否陈旧 0否1是</param>
        /// <param name="isshort">是否过矮 0否1是</param>
        /// <param name="istilt">是否倾斜 0否1是</param>
        /// <param name="isunlocation">是否位置不合理 0否1是</param>
        /// <param name="otherqueation">其他问题</param>
        /// <param name="outwatertime">出水时间</param>
        /// <param name="pressure">压力</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="globid">globid</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string firehydrantaddress, string firehydrantcode, string firehydrantvender, int iscomplete, int isdirty, int isfillbrand, int isfixedbrand,
            int isheight, int islackconnect, int isleakwater, int ismisscover, int isold, int isshort, int istilt,
            int isunlocation, string otherqueation, DateTime outwatertime, string pressure, string rangename, string globid, string rangename_Fgs, string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Firehydrantdaily model = new Accounts_Firehydrantdaily();
            #region 上传文件
            IFormFileCollection files = formData.Files;
            FileFactory file = new FileFactory();
            string Path = file.UploadFiles(files);
            if (Path != "")
            {
                model.Photos = Path.Split('-')[0].ToString();
            }
            #endregion
            model.Firehydrantaddress = firehydrantaddress;
            model.Firehydrantcode = firehydrantcode;
            model.Firehydrantvender = firehydrantvender;
            model.Iscomplete = iscomplete;
            model.Isdirty = isdirty;
            model.Isfillbrand = isfillbrand;
            model.Isfixedbrand = isfixedbrand;
            model.Isheight = isheight;
            model.Islackconnect = islackconnect;
            model.Isleakwater = isleakwater;
            model.Ismisscover = ismisscover;
            model.Isold = isold;
            model.Isshort = isshort;
            model.Istilt = istilt;
            model.Isunlocation = isunlocation;
            model.Otherqueation = otherqueation;
            model.Outwatertime = outwatertime;
            model.Pressure = pressure;
            model.Rangename = rangename;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Remark = formData["remark"].ToString();
            model.Systemtime = DateTime.Now;
            var result = _iAccounts_FirehydrantdailyDAL.Add(model);
            return result;
        }
        /// <summary>
        /// 修改消火栓日常巡查记录表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="firehydrantaddress">所在道路</param>
        /// <param name="firehydrantcode">设备编号</param>
        /// <param name="firehydrantvender">厂家</param>
        /// <param name="iscomplete">是否完好 0否1是</param>
        /// <param name="isdirty">是否污损 0否1是</param>
        /// <param name="isfillbrand">是否补订牌 0否1是</param>
        /// <param name="isfixedbrand">是否未钉牌 0否1是</param>
        /// <param name="isheight">是否过高 0否1是</param>
        /// <param name="islackconnect">是否缺连接扣 0否1是</param>
        /// <param name="isleakwater">是否漏水 0否1是</param>
        /// <param name="ismisscover">是否缺盖 0否1是</param>
        /// <param name="isold">是否陈旧 0否1是</param>
        /// <param name="isshort">是否过矮 0否1是</param>
        /// <param name="istilt">是否倾斜 0否1是</param>
        /// <param name="isunlocation">是否位置不合理 0否1是</param>
        /// <param name="otherqueation">其他问题</param>
        /// <param name="outwatertime">出水时间</param>
        /// <param name="pressure">压力</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="globid">globid</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string id, string firehydrantaddress, string firehydrantcode, string firehydrantvender, int iscomplete, int isdirty, int isfillbrand, int isfixedbrand,
            int isheight, int islackconnect, int isleakwater, int ismisscover, int isold, int isshort, int istilt,
            int isunlocation, string otherqueation, DateTime outwatertime, string pressure, string rangename, string globid, string rangename_Fgs, string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Firehydrantdaily model = _iAccounts_FirehydrantdailyDAL.GetInfo(id);
            if (model == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            #region 上传文件

            IFormFileCollection files = formData.Files;
            FileFactory file = new FileFactory();
            string Path = file.UploadFiles(files);
            if (Path != "")
            {
                model.Photos = Path.Split('-')[0].ToString();
            }
            if (!string.IsNullOrEmpty(formData["imageUrl"].ToString()))
            {
                model.Photos += formData["imageUrl"].ToString() + "|";
            }
            #endregion
            model.Firehydrantaddress = firehydrantaddress;
            model.Firehydrantcode = firehydrantcode;
            model.Firehydrantvender = firehydrantvender;
            model.Iscomplete = iscomplete;
            model.Isdirty = isdirty;
            model.Isfillbrand = isfillbrand;
            model.Isfixedbrand = isfixedbrand;
            model.Isheight = isheight;
            model.Islackconnect = islackconnect;
            model.Isleakwater = isleakwater;
            model.Ismisscover = ismisscover;
            model.Isold = isold;
            model.Isshort = isshort;
            model.Istilt = istilt;
            model.Isunlocation = isunlocation;
            model.Otherqueation = otherqueation;
            model.Outwatertime = outwatertime;
            model.Pressure = pressure;
            model.Rangename = rangename;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Remark = formData["remark"].ToString();
            return _iAccounts_FirehydrantdailyDAL.Update(model);
        }
        /// <summary>
        /// 删除消火栓日常巡查记录表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string id)
        {
            var modeInfo = _iAccounts_FirehydrantdailyDAL.GetInfo(id);
            return _iAccounts_FirehydrantdailyDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得消火栓日常巡查记录表列表
        /// </summary>
        /// <param name="ParInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <param name="searchSql">拼接的查询语句</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetList(string ParInfo, string sort = "Systemtime", string ordering = "desc", int num = 20, int page = 1, string searchSql = "", string mi_Shape = "")
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iAccounts_FirehydrantdailyDAL.GetList(sort, ordering, num, page, sqlCondition, searchSql, mi_Shape);
            return result;
        }
        /// <summary>
        /// 获得消火栓日常巡查记录统计表
        /// </summary>
        /// <param name="ParInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <param name="searchSql">拼接的查询语句</param>
        /// <param name="groupByFields">统计纬度字段</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetCountList(string ParInfo, string sort = "RANGENAME", string ordering = "desc", int num = 20, int page = 1, string searchSql = "", string groupByFields = "RANGENAME", string mi_Shape = "")
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iAccounts_FirehydrantdailyDAL.GetCountList(sort, ordering, num, page, sqlCondition, searchSql, groupByFields, mi_Shape);
            return result;
        }
    }
}