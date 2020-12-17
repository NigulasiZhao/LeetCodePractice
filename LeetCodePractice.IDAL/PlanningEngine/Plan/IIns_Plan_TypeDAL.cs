using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Plan;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.Plan
{
    public interface IIns_Plan_TypeDAL : IDependency
    {
        /// <summary>
        /// 添加巡检类型
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Ins_Plan_Type model);
        /// <summary>
        /// 删除巡检类型
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Ins_Plan_Type model);
        /// <summary>
        /// 根据ID获取巡检类型
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Ins_Plan_Type GetInfo(string ID);
        /// <summary>
        /// 获得巡检类型信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        /// <summary>
        ///修改巡检类型信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Ins_Plan_Type model);
        MessageEntity IsExistPlanType(Ins_Plan_Type model, int isAdd);
        /// <summary>
        /// 获取巡检类型及明细信息
        /// </summary>
        /// <returns></returns>
        MessageEntity GetTreeList();

    }
}
