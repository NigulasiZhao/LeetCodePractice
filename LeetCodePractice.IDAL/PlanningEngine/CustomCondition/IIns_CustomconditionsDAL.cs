/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/2 17:04:16
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Plan;
using System.Collections.Generic;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.CustomCondition
{
    /// <summary>
    ///用户自定义查询条件DAL层
    /// </summary>
    public interface IIns_CustomconditionsDAL : IDependency
    {
        /// <summary>
        /// 添加用户自定义查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Ins_Customconditions model);
        /// <summary>
        ///修改用户自定义查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Ins_Customconditions model);
        /// <summary>
        /// 删除用户自定义查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Ins_Customconditions model);
        /// <summary>
        /// 根据ID获取用户自定义查询条件
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Ins_Customconditions GetInfo(string ID);
        /// <summary>
        /// 获得用户自定义查询条件列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        /// <summary>
        /// 获得用户自定义查询条件
        /// </summary>
        /// <param name="parInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetFirstCondition(string UserId, string Functionid);
    }
}