﻿/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/11/25 11:26:28
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Enterprise
{
    /// <summary>
    ///企业业务信息DAL层
    /// </summary>
    public interface IIns_EnterprisebusinessDAL : IDependency
    {
        /// <summary>
        /// 添加企业业务信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Ins_Enterprisebusiness model);
        /// <summary>
        ///修改企业业务信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Ins_Enterprisebusiness model);
        /// <summary>
        /// 删除企业业务信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Ins_Enterprisebusiness model);
        /// <summary>
        /// 根据ID获取企业业务信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Ins_Enterprisebusiness GetInfo(string ID);
        /// <summary>
        /// 获得企业业务信息列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition, string SearchConditions);

    }
}