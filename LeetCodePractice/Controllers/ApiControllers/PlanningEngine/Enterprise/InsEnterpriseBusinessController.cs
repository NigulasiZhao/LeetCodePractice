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
    ///企业业务信息
    /// </summary>
    public class InsEnterpriseBusinessController : BaseController
    {

        private readonly IIns_EnterprisebusinessDAL _iIns_EnterprisebusinessDAL;

        public InsEnterpriseBusinessController(IIns_EnterprisebusinessDAL iIns_EnterprisebusinessDAL)
        {

            _iIns_EnterprisebusinessDAL = iIns_EnterprisebusinessDAL;
        }
        /// <summary>
        /// 添加企业业务信息
        /// </summary>
        /// <param name="model">Address:地址;Checkcode:考评编号;Checkcontent:考评内容;Checkdate:考评日期;Checkscore:考评分数;Checkunit:考核单位;Enterpriseid:企业ID;Id:主键ID;Monitoringunit:监管单位;Projectname:工程名称;</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Enterprisebusiness model)
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
            var result = _iIns_EnterprisebusinessDAL.Add(model);
            return result;
        }
        /// <summary>
        ///修改企业业务信息
        /// </summary>
        /// <param name="ID">主键ID(Id)</param>
        /// <param name="value">Address:地址;Checkcode:考评编号;Checkcontent:考评内容;Checkdate:考评日期;Checkscore:考评分数;Checkunit:考核单位;Enterpriseid:企业ID;Id:主键ID;Monitoringunit:监管单位;Projectname:工程名称;</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Enterprisebusiness value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Id = ID;
            return _iIns_EnterprisebusinessDAL.Update(value);
        }
        /// <summary>
        /// 删除企业业务信息
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _iIns_EnterprisebusinessDAL.GetInfo(ID);
            return _iIns_EnterprisebusinessDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得企业业务信息列表
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
            var result = _iIns_EnterprisebusinessDAL.GetList(list, sort, ordering, num, page, sqlCondition, SearchConditions);
            return result;
        }

    }
}