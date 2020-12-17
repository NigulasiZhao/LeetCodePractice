using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Plan;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.Plan
{
    public interface IIns_Plan_CycleDAL : IDependency
    {
        /// <summary>
        /// 添加计划周期
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Ins_Plan_Cycle model);
        /// <summary>
        /// 删除计划周期
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Ins_Plan_Cycle model);
        /// <summary>
        /// 根据ID获取计划周期
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Ins_Plan_Cycle GetInfo(string ID);
        /// <summary>
        /// 获得计划周期信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        /// <summary>
        ///修改计划周期信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Ins_Plan_Cycle model);
    }
}
