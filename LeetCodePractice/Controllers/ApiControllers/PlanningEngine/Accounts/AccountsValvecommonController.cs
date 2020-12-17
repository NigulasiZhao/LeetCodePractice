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
    /// 阀门普查工作情况相关接口
    /// </summary>
    public class AccountsValveCommonController : Controller
    {
        private readonly IAccounts_ValvecommonDAL _iAccounts_ValvecommonDAL;

        public AccountsValveCommonController(IAccounts_ValvecommonDAL iAccounts_ValvecommonDAL)
        {
            _iAccounts_ValvecommonDAL = iAccounts_ValvecommonDAL;
        }
        /// <summary>
        /// 保存阀门普查工作情况记录
        /// </summary>
        /// <param name="checkpersonid">检查人ID</param>
        /// <param name="checkpersonname">检查人名称</param>
        /// <param name="checktime">检查时间</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="valveaddress">阀门地址</param>
        /// <param name="valvecaliber">阀门口径</param>
        /// <param name="valvecode">阀门编号</param>
        /// <param name="valvedetail">阀门及阀门井情况</param>
        /// <param name="valvelevel">级别</param>
        /// <param name="valverunstate">阀门运行状态 0正常，1不正常</param>
        /// <param name="valveswitchstate">阀门开关状态 0开 1关 2未知</param>
        /// <param name="globid">globid</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(string checkpersonid, string checkpersonname, DateTime checktime, string rangename, string valveaddress, string valvecaliber,
            string valvecode, string valvedetail, string valvelevel, int valverunstate, int valveswitchstate, string globid, string rangename_Fgs, string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Valvecommon model = new Accounts_Valvecommon();
            model.Checkpersonid = checkpersonid;
            model.Checkpersonname = checkpersonname;
            model.Checktime = checktime;
            model.Rangename = rangename;
            model.Valveaddress = valveaddress;
            model.Valvecaliber = valvecaliber;
            model.Valvecode = valvecode;
            model.Valvedetail = valvedetail;
            model.Valvelevel = valvelevel;
            model.Valverunstate = valverunstate;
            model.Valveswitchstate = valveswitchstate;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Systemtime = DateTime.Now;
            model.Remark = formData["remark"].ToString();
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
            var result = _iAccounts_ValvecommonDAL.Add(model);
            return result;
        }
        /// <summary>
        /// 查询阀门普查工作情况记录
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
        public MessageEntity GetList(string ParInfo, string sort = "Systemtime", string ordering = "desc", int num = 20, int page = 1, string searchSql = "", string mi_Shape="")
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iAccounts_ValvecommonDAL.GetList(sort, ordering, num, page, sqlCondition, searchSql,mi_Shape);
            return result;
        }
        /// <summary>
        /// 修改阀门普查工作情况记录
        /// </summary>
        /// <param name="checkpersonid">检查人ID</param>
        /// <param name="checkpersonname">检查人名称</param>
        /// <param name="checktime">检查时间</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="valveaddress">阀门地址</param>
        /// <param name="valvecaliber">阀门口径</param>
        /// <param name="valvecode">阀门编号</param>
        /// <param name="valvedetail">阀门及阀门井情况</param>
        /// <param name="valvelevel">级别</param>
        /// <param name="valverunstate">阀门运行状态 0正常，1不正常</param>
        /// <param name="valveswitchstate">阀门开关状态 0开 1关 2未知</param>
        /// <param name="globid">globid</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string id, string checkpersonid, string checkpersonname, DateTime checktime, string rangename, string valveaddress, string valvecaliber,
            string valvecode, string valvedetail, string valvelevel, int valverunstate, int valveswitchstate, string globid, string rangename_Fgs, string mi_Shape, [FromForm] IFormCollection formData)
        {
            Accounts_Valvecommon model = _iAccounts_ValvecommonDAL.GetInfo(id);
            if (model == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            model.Checkpersonid = checkpersonid;
            model.Checkpersonname = checkpersonname;
            model.Checktime = checktime;
            model.Rangename = rangename;
            model.Valveaddress = valveaddress;
            model.Valvecaliber = valvecaliber;
            model.Valvecode = valvecode;
            model.Valvedetail = valvedetail;
            model.Valvelevel = valvelevel;
            model.Valverunstate = valverunstate;
            model.Valveswitchstate = valveswitchstate;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Remark = formData["remark"].ToString();
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
            return _iAccounts_ValvecommonDAL.Update(model);
        }
        /// <summary>
        /// 删除阀门普查工作情况记录
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string id)
        {
            var modeInfo = _iAccounts_ValvecommonDAL.GetInfo(id);
            return _iAccounts_ValvecommonDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得阀门普查工作情况统计表
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
            var result = _iAccounts_ValvecommonDAL.GetCountList(sort, ordering, num, page, sqlCondition, searchSql, groupByFields,mi_Shape);
            return result;
        }
    }
}
