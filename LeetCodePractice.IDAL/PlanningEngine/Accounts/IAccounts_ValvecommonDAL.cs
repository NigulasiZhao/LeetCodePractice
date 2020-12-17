/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/27 17:15:28
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Accounts
{
    /// <summary>
    ///阀门普查工作情况表DAL层
    /// </summary>
    public interface IAccounts_ValvecommonDAL : IDependency
    {
        /// <summary>
        /// 添加阀门普查工作情况表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Accounts_Valvecommon model);
        /// <summary>
        ///修改阀门普查工作情况表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Accounts_Valvecommon model);
        /// <summary>
        /// 删除阀门普查工作情况表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Accounts_Valvecommon model);
        /// <summary>
        /// 根据ID获取阀门普查工作情况表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Accounts_Valvecommon GetInfo(string ID);
        /// <summary>
        /// 获得阀门普查工作情况表列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql,string mi_Shape);
        /// <summary>
        /// 获得阀门普查工作情况统计表
        /// </summary>
        /// <param name="parInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetCountList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql, string groupByFields, string mi_Shape);
    }
}