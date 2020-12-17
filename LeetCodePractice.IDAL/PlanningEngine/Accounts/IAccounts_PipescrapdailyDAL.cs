/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/10/10 15:15:28
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Plan;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine
{
    /// <summary>
    ///管线拆除、报废登记表DAL层
    /// </summary>
    public interface IAccounts_PipescrapdailyDAL : IDependency
    {
        /// <summary>
        /// 添加管线拆除、报废登记表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Accounts_Pipescrapdaily model);
        /// <summary>
        ///修改管线拆除、报废登记表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Accounts_Pipescrapdaily model);
        /// <summary>
        /// 删除管线拆除、报废登记表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Accounts_Pipescrapdaily model);
        /// <summary>
        /// 根据ID获取管线拆除、报废登记表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Accounts_Pipescrapdaily GetInfo(string ID);
        /// <summary>
        /// 获得管线拆除、报废登记表列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql, string mi_Shape);
        /// <summary>
        /// 获得管线拆除、报废登记统计表
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