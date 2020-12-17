/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/3 16:02:14
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Plan;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.CustomCondition
{
    /// <summary>
    ///用户查询条件DAL层
    /// </summary>
    public interface IT_CustomconditionsDAL : IDependency
    {
        /// <summary>
        /// 添加用户查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(T_Customconditions model);
        /// <summary>
        ///修改用户查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(T_Customconditions model);
        /// <summary>
        /// 删除用户查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(T_Customconditions model);
        /// <summary>
        /// 根据ID获取用户查询条件
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        T_Customconditions GetInfo(string ID);
        /// <summary>
        /// 获得用户查询条件列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);

    }
}