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
    ///企业信息
    /// </summary>
    public class InsEnterpriseInfoController : BaseController
    {

        private readonly IIns_EnterpriseinfoDAL _iIns_EnterpriseinfoDAL;

        public InsEnterpriseInfoController(IIns_EnterpriseinfoDAL iIns_EnterpriseinfoDAL)
        {

            _iIns_EnterpriseinfoDAL = iIns_EnterpriseinfoDAL;
        }
        /// <summary>
        /// 添加企业信息
        /// </summary>
        /// <param name="model">Address:单位地址;Behalfperson:法定代表人;Enterprisecode:企业编码;Enterpriselevel:企业资质;Enterprisename:企业名称;Enterprisetype:单位性质;Fax:传真;Id:主键ID;Mishape:地理位置信息(WKT);Tel:电话;</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Enterpriseinfo model)
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
            var result = _iIns_EnterpriseinfoDAL.Add(model);
            return result;
        }
        /// <summary>
        ///修改企业信息
        /// </summary>
        /// <param name="ID">主键ID(Id)</param>
        /// <param name="value">Address:单位地址;Behalfperson:法定代表人;Enterprisecode:企业编码;Enterpriselevel:企业资质;Enterprisename:企业名称;Enterprisetype:单位性质;Fax:传真;Id:主键ID;Mishape:地理位置信息(WKT);Tel:电话;</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Enterpriseinfo value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Id = ID;
            return _iIns_EnterpriseinfoDAL.Update(value);
        }
        /// <summary>
        /// 删除企业信息
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _iIns_EnterpriseinfoDAL.GetInfo(ID);
            return _iIns_EnterpriseinfoDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获得企业信息列表
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
            var result = _iIns_EnterpriseinfoDAL.GetList(list, sort, ordering, num, page, sqlCondition, SearchConditions);
            return result;
        }
        /// <summary>
        /// 获取企业性质下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEnterpriseType()
        {
            List<string> list = new List<string>();
            list.Add("私有企业");
            list.Add("国有企业");
            list.Add("股份制");
            list.Add("事业单位");
            return MessageEntityTool.GetMessage(list.Count, list);
        }
        /// <summary>
        /// 获取企业资质下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEnterpriseLevle()
        {
            List<string> list = new List<string>();
            list.Add("一级");
            list.Add("二级");
            list.Add("三级");
            list.Add("四级");
            list.Add("五级");
            return MessageEntityTool.GetMessage(list.Count, list);
        }

        /// <summary>
        /// 查询统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetStatisticalData(DateTime? startDate = null, DateTime? endDate = null)
        {
            return _iIns_EnterpriseinfoDAL.GetStatisticalData(startDate, endDate);
        }
        /// <summary>
        /// 根据ID获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetInfo(string id)
        {
            Ins_Enterpriseinfo model = _iIns_EnterpriseinfoDAL.GetInfo(id);
            return MessageEntityTool.GetMessage(0, model);
        }
    }
}