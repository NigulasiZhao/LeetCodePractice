/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/11/25 11:26:28
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Controllers;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Enterprise
{
    /// <summary>
    ///企业投标信息
    /// </summary>
    public class InsEnterpriseTenderController : BaseController
    {

        private readonly IIns_EnterprisetenderDAL _iIns_EnterprisetenderDAL;

        public InsEnterpriseTenderController(IIns_EnterprisetenderDAL iIns_EnterprisetenderDAL)
        {

            _iIns_EnterprisetenderDAL = iIns_EnterprisetenderDAL;
        }
        /// <summary>
        /// 添加企业投标信息
        /// </summary>
        /// <param name="model">Enterpriseid:企业ID;Id:主键ID;Istender:是否中标;Monitoringunit:监管单位;Projectname:工程名称;Tendercode:投标编号;Tenderdate:投标日期;Tendertiem:投标期限;Tenderway:招标方式;Totalamount:合同价款(万元);</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Enterprisetender model)
        {
            if (model == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            if (UniWaterUserInfo != null)
            {
                model.Creatorid = UniWaterUserInfo._id;
                model.Creatorname = UniWaterUserInfo.Name;
                model.Creatordepartmentid = UniWaterUserInfo.Group;
                model.Creatordepartmentname = UniWaterUserInfo.group_data.Name;
                model.Creatortime = DateTime.Now;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "access_token失效，获取用户信息失败！");
            }
            var result = _iIns_EnterprisetenderDAL.Add(model);
            return result;
        }
        /// <summary>
        ///修改企业投标信息
        /// </summary>
        /// <param name="ID">主键ID(Id)</param>
        /// <param name="value">Enterpriseid:企业ID;Id:主键ID;Istender:是否中标;Monitoringunit:监管单位;Projectname:工程名称;Tendercode:投标编号;Tenderdate:投标日期;Tendertiem:投标期限;Tenderway:招标方式;Totalamount:合同价款(万元);</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Enterprisetender value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Id = ID;
            return _iIns_EnterprisetenderDAL.Update(value);
        }
        /// <summary>
        /// 删除企业投标信息
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _iIns_EnterprisetenderDAL.GetInfo(ID);
            return _iIns_EnterprisetenderDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得企业投标信息列表
        /// </summary>
        /// <param name="ParInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <param name="SearchConditions">模糊查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetList(string ParInfo, string sort = "Id", string ordering = "desc", int num = 20, int page = 1, string SearchConditions = "")
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iIns_EnterprisetenderDAL.GetList(list, sort, ordering, num, page, sqlCondition, SearchConditions);
            return result;
        }
        /// <summary>
        /// 获取招标方式下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetTenderMethod()
        {
            List<string> list = new List<string>();
            list.Add("公开");
            list.Add("非公开");
            return MessageEntityTool.GetMessage(list.Count, list);
        }
    }
}