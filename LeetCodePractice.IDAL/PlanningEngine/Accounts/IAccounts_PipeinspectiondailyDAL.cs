/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/28 9:49:33
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
    ///高风险管段巡查记录表DAL层
    /// </summary>
    public interface IAccounts_PipeinspectiondailyDAL : IDependency
    {
        /// <summary>
        /// 添加高风险管段巡查记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Accounts_Pipeinspectiondaily model);
        /// <summary>
        ///修改高风险管段巡查记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Accounts_Pipeinspectiondaily model);
        /// <summary>
        /// 删除高风险管段巡查记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Accounts_Pipeinspectiondaily model);
        /// <summary>
        /// 根据ID获取高风险管段巡查记录表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Accounts_Pipeinspectiondaily GetInfo(string ID);
        /// <summary>
        /// 获得高风险管段巡查记录表列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql, string mi_Shape);
        /// <summary>
        /// 获得高风险管段巡查记录统计表
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