using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.BPM;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.PlanManage
{
    public interface IIns_PlanDAL : IDependency
    {
        /// <summary>
        /// 添加计划管理
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Add(Ins_Plan model);
        /// <summary>
        /// 删除计划管理
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Delete(Ins_Plan model);
        /// <summary>
        /// 获取计划任务数量
        /// </summary>
        /// <returns></returns>
        int GetTaskCount(string Plan_Id);
        /// <summary>
        /// 根据ID获取计划管理
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Ins_Plan GetInfo(string ID);
        /// <summary>
        /// 获得计划管理信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        MessageEntity GetList(string planttemplate_id ,string rangids, List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        /// <summary>
        ///修改计划管理信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        MessageEntity Update(Ins_Plan model);
        /// <summary>
        /// 计划管理派发
        /// </summary>
        /// <returns></returns>
        Task<MessageEntity> DistributedPlan(DistributedPlanBPM model);
        /// <summary>
        /// 计划管理重发
        /// </summary>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        MessageEntity RepeatDistributedPlan(string PlanId);
        MessageEntity IsExistPlan(string planTypeId);
    }
}
