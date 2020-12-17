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
using GISWaterSupplyAndSewageServer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine
{
    /// <summary>
    ///高风险管段巡查记录表接口
    /// </summary>
    public class AccountsPipeInspectionDailyController : BaseController
    {

        private readonly IAccounts_PipeinspectiondailyDAL _iAccounts_PipeinspectiondailyDAL;

        public AccountsPipeInspectionDailyController(IAccounts_PipeinspectiondailyDAL iAccounts_PipeinspectiondailyDAL)
        {

            _iAccounts_PipeinspectiondailyDAL = iAccounts_PipeinspectiondailyDAL;
        }
        ///// <summary>
        ///// 添加高风险管段巡查记录表
        ///// </summary>
        ///// <param name="model">Isridingpressure:是否骑压 0:否 1：是;Rangename:区域名称;Remark:备注;Routename:路线名称;Systemtime:系统时间;Workcode:工单编号;</param>
        ///// <returns></returns>
        /// <summary>
        /// 添加高风险管段巡查记录表
        /// </summary>
        /// <param name="detailaddress">发现问题详细地址</param>
        /// <param name="globid">管线编号</param>
        /// <param name="inspectiondate">巡查时间</param>
        /// <param name="inspector">巡查人</param>
        /// <param name="isexplose">是否暴漏0:否 1：是</param>
        /// <param name="isrectification">是否当场整改0:否 1：是</param>
        /// <param name="isridingpressure">是否骑压 0:否 1：是;</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="routename">路线名称</param>
        /// <param name="workcode">工单编号</param>
        /// <param name="Pipecode">管线编号</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string detailaddress, string globid, DateTime inspectiondate, string inspector, int isexplose, int isrectification,
            int isridingpressure, string rangename, string routename, string workcode, string Pipecode, string rangename_Fgs, string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Pipeinspectiondaily model = new Accounts_Pipeinspectiondaily();
            model.Detailaddress = detailaddress;
            model.Globid = globid;
            model.Inspectiondate = inspectiondate;
            model.Inspector = inspector;
            model.Isexplose = isexplose;
            model.Isrectification = isrectification;
            model.Isridingpressure = isridingpressure;
            model.Rangename = rangename;
            model.Routename = routename;
            model.Workcode = workcode;
            model.Pipecode = Pipecode;
            model.Systemtime = DateTime.Now;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Remark = formData["remark"].ToString();
            var result = _iAccounts_PipeinspectiondailyDAL.Add(model);
            return result;
        }
        /// <summary>
        /// 修改高风险管段巡查记录表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="detailaddress">发现问题详细地址</param>
        /// <param name="globid">管线编号</param>
        /// <param name="inspectiondate">巡查时间</param>
        /// <param name="inspector">巡查人</param>
        /// <param name="isexplose">是否暴漏0:否 1：是</param>
        /// <param name="isrectification">是否当场整改0:否 1：是</param>
        /// <param name="isridingpressure">是否骑压 0:否 1：是;</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="routename">路线名称</param>
        /// <param name="workcode">工单编号</param>
        /// <param name="Pipecode">管线编号</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
       /// <param name="mi_Shape">shape信息 WKT格式</param>

        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string id, string detailaddress, string globid, DateTime inspectiondate, string inspector, int isexplose, int isrectification,
            int isridingpressure, string rangename, string routename, string workcode, string Pipecode, string rangename_Fgs,  string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Pipeinspectiondaily model = _iAccounts_PipeinspectiondailyDAL.GetInfo(id);
            if (model == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            model.Detailaddress = detailaddress;
            model.Globid = globid;
            model.Inspectiondate = inspectiondate;
            model.Inspector = inspector;
            model.Isexplose = isexplose;
            model.Isrectification = isrectification;
            model.Isridingpressure = isridingpressure;
            model.Rangename = rangename;
            model.Routename = routename;
            model.Workcode = workcode;
            model.Pipecode = Pipecode;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Remark = formData["remark"].ToString();
            return _iAccounts_PipeinspectiondailyDAL.Update(model);
        }
        /// <summary>
        /// 删除高风险管段巡查记录表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string id)
        {
            var modeInfo = _iAccounts_PipeinspectiondailyDAL.GetInfo(id);
            return _iAccounts_PipeinspectiondailyDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得高风险管段巡查记录表列表
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
            var result = _iAccounts_PipeinspectiondailyDAL.GetList(sort, ordering, num, page, sqlCondition, searchSql, mi_Shape);
            return result;
        }
        /// <summary>
        /// 获得高风险管段巡查记录统计表
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
            var result = _iAccounts_PipeinspectiondailyDAL.GetCountList(sort, ordering, num, page, sqlCondition, searchSql, groupByFields, mi_Shape);
            return result;
        }
    }
}