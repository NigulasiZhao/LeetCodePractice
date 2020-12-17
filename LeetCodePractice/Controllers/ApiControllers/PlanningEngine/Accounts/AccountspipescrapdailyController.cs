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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine
{
    /// <summary>
    ///管线拆除、报废登记表接口
    /// </summary>
    public class AccountsPipeScrapDailyController : BaseController
    {

        private readonly IAccounts_PipescrapdailyDAL _iAccounts_PipescrapdailyDAL;

        public AccountsPipeScrapDailyController(IAccounts_PipescrapdailyDAL iAccounts_PipescrapdailyDAL)
        {

            _iAccounts_PipescrapdailyDAL = iAccounts_PipescrapdailyDAL;
        }
        /// <summary>
        /// 添加管线拆除、报废登记表
        /// </summary>
        /// <param name="model">Auditleader:审核部领导;Auditleaderqzdate:审核部领导签字时间;Auditreviewer:审核部审核人;Auditreviewerqzdate:审核部审核人签字时间;Companymanager:公司经理;Companymanagerqzdate:公司经理签字时间;Constructioncompany:施工单位;Fillingcompany:填表单位;Fillingdate:填表日期;Financeagent:财务部经办人;Financeagentqzdate:财务部经办人签字时间;Financeleader:财务部领导;Financeleaderqzdate:财务部领导签字时间;Globid:管线编号;Id:主键;Installplace:安装地点;Mi_Shape:WTK格式;Originalvalue:原值(元);Pipechief:原管总长;Pipecode:管线编号;Primary_Completeddate:原竣工日期;Primary_Projectcode:原工程档号;Primary_Projectreference:原工程档号;Projectcode:工程编号;Projectname:工程名称;Rangename:区域名称;Rangename_Fgs:分公司区域名称;Reason:原因;Scrapcaliber:报废口径;Scrapdismantle:废管或拆除;Scrapnum:报废数量;Scrapyj:报废意见;Scrapyz:报废原值;Scrapzj:报废折旧;Shape:N;Sycompanyjbr:使用单位经办人;Sycompanyld:使用单位领导;Sycompanyqzdate:使用单位领导签字时间;Sycompanyyj:使用单位意见;Systemtime:系统时间;Unitprice:单元(元/m);Zyinstallgg:主要安装规格;</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Accounts_Pipescrapdaily model)
        {
            if (model == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            var result = _iAccounts_PipescrapdailyDAL.Add(model);
            return result;
        }
        /// <summary>
        ///修改管线拆除、报废登记表
        /// </summary>
        /// <param name="ID">主键ID(Id)</param>
        /// <param name="value">Auditleader:审核部领导;Auditleaderqzdate:审核部领导签字时间;Auditreviewer:审核部审核人;Auditreviewerqzdate:审核部审核人签字时间;Companymanager:公司经理;Companymanagerqzdate:公司经理签字时间;Constructioncompany:施工单位;Fillingcompany:填表单位;Fillingdate:填表日期;Financeagent:财务部经办人;Financeagentqzdate:财务部经办人签字时间;Financeleader:财务部领导;Financeleaderqzdate:财务部领导签字时间;Globid:管线编号;Id:主键;Installplace:安装地点;Mi_Shape:WTK格式;Originalvalue:原值(元);Pipechief:原管总长;Pipecode:管线编号;Primary_Completeddate:原竣工日期;Primary_Projectcode:原工程档号;Primary_Projectreference:原工程档号;Projectcode:工程编号;Projectname:工程名称;Rangename:区域名称;Rangename_Fgs:分公司区域名称;Reason:原因;Scrapcaliber:报废口径;Scrapdismantle:废管或拆除;Scrapnum:报废数量;Scrapyj:报废意见;Scrapyz:报废原值;Scrapzj:报废折旧;Shape:N;Sycompanyjbr:使用单位经办人;Sycompanyld:使用单位领导;Sycompanyqzdate:使用单位领导签字时间;Sycompanyyj:使用单位意见;Systemtime:系统时间;Unitprice:单元(元/m);Zyinstallgg:主要安装规格;</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Accounts_Pipescrapdaily value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Id = ID;
            return _iAccounts_PipescrapdailyDAL.Update(value);
        }
        /// <summary>
        /// 删除管线拆除、报废登记表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string id)
        {
            var modeInfo = _iAccounts_PipescrapdailyDAL.GetInfo(id);
            return _iAccounts_PipescrapdailyDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得管线拆除、报废登记表列表
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
            var result = _iAccounts_PipescrapdailyDAL.GetList(sort, ordering, num, page, sqlCondition, searchSql, mi_Shape);
            return result;
        }
        /// <summary>
        /// 获得管线拆除、报废登记统计表
        /// </summary>
        /// <param name="ParInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <param name="groupByFields">统计纬度字段</param>
        /// <param name="searchSql">拼接的查询语句</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetCountList(string ParInfo, string sort = "RANGENAME", string ordering = "desc", int num = 20, int page = 1, string searchSql = "", string groupByFields = "RANGENAME", string mi_Shape = "")
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iAccounts_PipescrapdailyDAL.GetCountList(sort, ordering, num, page, sqlCondition, searchSql, groupByFields, mi_Shape);
            return result;
        }
    }
}