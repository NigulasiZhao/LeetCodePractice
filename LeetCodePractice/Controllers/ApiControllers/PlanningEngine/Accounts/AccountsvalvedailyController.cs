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
    ///阀门日常巡查记录表接口
    /// </summary>
    public class AccountsValveDailyController : BaseController
    {

        private readonly IAccounts_ValvedailyDAL _iAccounts_ValvedailyDAL;

        public AccountsValveDailyController(IAccounts_ValvedailyDAL iAccounts_ValvedailyDAL)
        {

            _iAccounts_ValvedailyDAL = iAccounts_ValvedailyDAL;
        }
        /// <summary>
        /// 添加阀门日常巡查记录表
        /// </summary>
        /// <param name="otherquestion">其他问题</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="valveaddress">所在道路</param>
        /// <param name="valveboxdetail">传动箱情况</param>
        /// <param name="valvecaliber">口径</param>
        /// <param name="valvecode">阀门编号</param>
        /// <param name="valvedetail">信息登记与现场情况</param>
        /// <param name="valveiscomplete">是否完好 0否 1是</param>
        /// <param name="valveisleakage">是否漏水 0否1是</param>
        /// <param name="valveisstandard">阀门井盖高低不平 0否 1是</param>
        /// <param name="valveswitchstate">全开/全关 0全开1全关</param>
        /// <param name="valvetype">阀门类型</param>
        /// <param name="globid">globid</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public MessageEntity Post(string otherquestion, string rangename, string valveaddress, string valveboxdetail, string valvecaliber, string valvecode,
            string valvedetail, int valveiscomplete, int valveisleakage, int valveisstandard, int valveswitchstate, string valvetype, string globid, string rangename_Fgs,  string mi_Shape,
            [FromForm] IFormCollection formData)
        {
            Accounts_Valvedaily model = new Accounts_Valvedaily();
            #region 上传文件
            IFormFileCollection files = formData.Files;
            FileFactory file = new FileFactory();
            string Path = file.UploadFiles(files);
            if (Path != "")
            {
                model.Photos = Path.Split('-')[0].ToString();
            }
            #endregion
            model.Otherquestion = otherquestion;
            model.Rangename = rangename;
            model.Remark = formData["remark"].ToString();
            model.Valveaddress = valveaddress;
            model.Valveboxdetail = valveboxdetail;
            model.Valvecaliber = valvecaliber;
            model.Valvecode = valvecode;
            model.Valvedetail = valvedetail;
            model.Valveiscomplete = valveiscomplete;
            model.Valveisleakage = valveisleakage;
            model.Valveisstandard = valveisstandard;
            model.Valveswitchstate = valveswitchstate;
            model.Valvetype = valvetype;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            model.Systemtime = DateTime.Now;
            var result = _iAccounts_ValvedailyDAL.Add(model);
            return result;
        }
        /// <summary>
        /// 修改阀门日常巡查记录表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="otherquestion">其他问题</param>
        /// <param name="rangename">行政区域名称</param>
        /// <param name="rangename_Fgs">分公司区域名称</param>
        /// <param name="mi_Shape">shape信息 WKT格式</param>
        /// <param name="valveaddress">所在道路</param>
        /// <param name="valveboxdetail">传动箱情况</param>
        /// <param name="valvecaliber">口径</param>
        /// <param name="valvecode">阀门编号</param>
        /// <param name="valvedetail">信息登记与现场情况</param>
        /// <param name="valveiscomplete">是否完好 0否 1是</param>
        /// <param name="valveisleakage">是否漏水 0否1是</param>
        /// <param name="valveisstandard">阀门井盖高低不平 0否 1是</param>
        /// <param name="valveswitchstate">全开/全关 0全开1全关</param>
        /// <param name="valvetype">阀门类型</param>
        /// <param name="globid">globid</param>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string id, string otherquestion, string rangename, string valveaddress, string valveboxdetail, string valvecaliber, string valvecode,
            string valvedetail, int valveiscomplete, int valveisleakage, int valveisstandard, int valveswitchstate, string valvetype, string globid, string rangename_Fgs,string mi_Shape,
            [FromForm] IFormCollection formData)
        {
            Accounts_Valvedaily model = _iAccounts_ValvedailyDAL.GetInfo(id);
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
            model.Otherquestion = otherquestion;
            model.Rangename = rangename;
            model.Remark = formData["remark"].ToString();
            model.Valveaddress = valveaddress;
            model.Valveboxdetail = valveboxdetail;
            model.Valvecaliber = valvecaliber;
            model.Valvecode = valvecode;
            model.Valvedetail = valvedetail;
            model.Valveiscomplete = valveiscomplete;
            model.Valveisleakage = valveisleakage;
            model.Valveisstandard = valveisstandard;
            model.Valveswitchstate = valveswitchstate;
            model.Valvetype = valvetype;
            model.Globid = globid;
            model.Rangename_Fgs = rangename_Fgs;
            model.Mi_Shape = mi_Shape;
            return _iAccounts_ValvedailyDAL.Update(model);
        }
        /// <summary>
        /// 删除阀门日常巡查记录表
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _iAccounts_ValvedailyDAL.GetInfo(ID);
            return _iAccounts_ValvedailyDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得阀门日常巡查记录表列表
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
            var result = _iAccounts_ValvedailyDAL.GetList(sort, ordering, num, page, sqlCondition, searchSql, mi_Shape);
            return result;
        }
        /// <summary>
        /// 获得阀门日常巡查记录统计表
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
            var result = _iAccounts_ValvedailyDAL.GetCountList(sort, ordering, num, page, sqlCondition, searchSql, groupByFields, mi_Shape);
            return result;
        }
    }
}