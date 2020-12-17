/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/27 17:15:40
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
    ///阀门一般性维修记录表DAL层
    /// </summary>
    public interface IAccounts_ValvemaintainDAL : IDependency
    {
        /// <summary>
        /// 添加阀门一般性维修记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Accounts_Valvemaintain model);
        /// <summary>
        ///修改阀门一般性维修记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Accounts_Valvemaintain model);
        /// <summary>
        /// 删除阀门一般性维修记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Accounts_Valvemaintain model);
        /// <summary>
        /// 根据ID获取阀门一般性维修记录表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Accounts_Valvemaintain GetInfo(string ID);
        /// <summary>
        /// 获得阀门一般性维修记录表列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql, string mi_Shape);
        /// <summary>
        /// 获得阀门一般性维修记录统计表
        /// </summary>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <param name="sqlCondition">查询条件</param>
        /// <param name="groupByFields">统计维度字段</param>
        /// <returns></returns>
        MessageEntity GetCountList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql, string groupByFields, string mi_Shape);
    }
}