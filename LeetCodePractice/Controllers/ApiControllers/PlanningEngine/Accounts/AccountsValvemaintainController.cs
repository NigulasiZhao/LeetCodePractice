using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine;
using GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Accounts
{
    /// <summary>
    /// 阀门一般性维修记录相关接口
    /// </summary>
    public class AccountsValveMaintainController : Controller
    {
        private readonly IAccounts_ValvemaintainDAL _iAccounts_ValvemaintainDAL;

        public AccountsValveMaintainController(IAccounts_ValvemaintainDAL iAccounts_ValvemaintainDAL)
        {
            _iAccounts_ValvemaintainDAL = iAccounts_ValvemaintainDAL;
        }
        ///// <summary>
        ///// 保存阀门一般性维修记录
        ///// </summary>
        ///// <param name="Model">完成情况:Iscomplete;维修项目:Maintaindetail;维修日期:Maintaintime;区域名称:Rangename;备注:Remark;系统时间:Systemtime;地址:Valveaddress;
        ///// 口径:Valvecaliber;阀门编号:Valvecode;型号:Valvetype;</param>
        ///// <returns></returns>
        /// <summary>
        /// 保存阀门一般性维修记录
        /// </summary>
        /// <param name="iscomplete">完成情况0未完成 1已完成</param>
        /// <param name="maintaindetail">维修项目</param>
        /// <param name="maintaintime">维修日期</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="valveaddress">地址</param>
        /// <param name="valvecaliber">口径</param>
        /// <param name="valvecode">阀门编号</param>
        /// <param name="valvetype">型号</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="globid">globid</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(int iscomplete, string maintaindetail, DateTime maintaintime, string rangename, string valveaddress, string valvecaliber,
            string valvecode, string valvetype, string globid, string rangename_Fgs, string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Valvemaintain model = new Accounts_Valvemaintain();
            model.Iscomplete = iscomplete;
            model.Maintaindetail = maintaindetail;
            model.Maintaintime = maintaintime;
            model.Rangename = rangename;
            model.Valveaddress = valveaddress;
            model.Valvecaliber = valvecaliber;
            model.Valvecode = valvecode;
            model.Valvetype = valvetype;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Systemtime = DateTime.Now;
            model.Remark = formData["remark"].ToString();
            var result = _iAccounts_ValvemaintainDAL.Add(model);
            return result;
        }
        /// <summary>
        /// 查询阀门一般性维修记录
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
            var result = _iAccounts_ValvemaintainDAL.GetList(sort, ordering, num, page, sqlCondition, searchSql, mi_Shape);
            return result;
        }
        /// <summary>
        /// 修改阀门一般性维修记录
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="iscomplete">完成情况0未完成 1已完成</param>
        /// <param name="maintaindetail">维修项目</param>
        /// <param name="maintaintime">维修日期</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="valveaddress">地址</param>
        /// <param name="valvecaliber">口径</param>
        /// <param name="valvecode">阀门编号</param>
        /// <param name="valvetype">型号</param>
        /// <param name="globid">globid</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string id, int iscomplete, string maintaindetail, DateTime maintaintime, string rangename, string valveaddress, string valvecaliber,
            string valvecode, string valvetype, string globid, string rangename_Fgs,  string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Valvemaintain model = _iAccounts_ValvemaintainDAL.GetInfo(id);
            if (model == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            model.Iscomplete = iscomplete;
            model.Maintaindetail = maintaindetail;
            model.Maintaintime = maintaintime;
            model.Rangename = rangename;
            model.Valveaddress = valveaddress;
            model.Valvecaliber = valvecaliber;
            model.Valvecode = valvecode;
            model.Valvetype = valvetype;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Remark = formData["remark"].ToString();
            return _iAccounts_ValvemaintainDAL.Update(model);
        }
        /// <summary>
        /// 删除阀门一般性维修记录
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string id)
        {
            var modeInfo = _iAccounts_ValvemaintainDAL.GetInfo(id);
            return _iAccounts_ValvemaintainDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得阀门一般性维修记录统计表
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
            var result = _iAccounts_ValvemaintainDAL.GetCountList(sort, ordering, num, page, sqlCondition, searchSql, groupByFields,mi_Shape);
            return result;
        }
    }
}
