using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.InspectionSettings.Statistics
{
/// <summary>
/// 任务统计--包含Echart图数据
/// </summary>
    public class InsTaskStatisticsController : Controller
    {
        private readonly IInsTaskStatisticsDAL _iInsTaskStatisticsDAL;
        public InsTaskStatisticsController(IInsTaskStatisticsDAL iInsTaskStatisticsDAL)
        {
            _iInsTaskStatisticsDAL = iInsTaskStatisticsDAL;

        }  /// <summary>
           ///  获取任务状态Echart数据
           /// </summary>
           /// <param name="startTime">开始时间 yyy-mm-dd</param>
           /// <param name="endTime">结束时间 yyy-mm-dd</param>
           /// <param name="rangids">区域ids </param>
           /// <param name="task_Type_id">任务类型id </param>
           /// <returns></returns>
        [HttpPost]
        public MessageEntity GetTaskStateEchart(DateTime? startTime, DateTime? endTime, [FromBody] List<string>? rangids = null, string? task_Type_id = null)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            string Ids = "";
            if (rangids != null)
            {
                Ids = "'" + string.Join("','", rangids.ToArray()) + "'";
                if (rangids.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            var result = _iInsTaskStatisticsDAL.GetTaskStateEchart(startTime, endTime, Ids, task_Type_id);
            return result;
        }
        /// <summary>
        ///  获取任务分类Echart数据
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <param name="rangids">区域ids </param>
        /// <param name="task_Type_id">任务类型id </param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity GetTaskTypeEchart(DateTime? startTime, DateTime? endTime, [FromBody] List<string>? rangids = null, string? task_Type_id = null)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            string Ids = "";
            if (rangids != null)
            {
                Ids = "'" + string.Join("','", rangids.ToArray()) + "'";
                if (rangids.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            var result = _iInsTaskStatisticsDAL.GetTaskTypeEchart(startTime, endTime, Ids, task_Type_id);
            return result;
        }

        /// <summary>
        ///  获取任务执行率分析
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <param name="rangids">区域ids </param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity GetTaskExecuteRate(DateTime? startTime, DateTime? endTime, [FromBody] List<string>? rangids = null)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            string Ids = "";
            if (rangids != null)
            {
                Ids = "'" + string.Join("','", rangids.ToArray()) + "'";
                if (rangids.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            var result = _iInsTaskStatisticsDAL.GetTaskExecuteRate(startTime, endTime, Ids);
            return result;
        }
        /// <summary>
        ///  获取巡检任务统计分析
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]查询参数：ProraterDeptId：部门id；ProraterId：巡检员id；Plan_cycle_id：巡检周期id</param>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <param name="rangids">区域ids </param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity GetTaskStatistics(string ParInfo, DateTime? startTime, DateTime? endTime, [FromBody] List<string>? rangids = null)
        {

            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            string Ids = "";
            if (rangids != null)
            {
                Ids = "'" + string.Join("','", rangids.ToArray()) + "'";
                if (rangids.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iInsTaskStatisticsDAL.GetTaskStatistics(startTime, endTime, Ids, sqlCondition);
            return result;
        }
        /// <summary>
        ///  获取巡检人员工作量分析
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]查询参数：ProraterDeptId：部门id；ProraterId：巡检员id；</param>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <param name="rangids">区域ids </param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity GetInspectorWorkloadStatistics(string ParInfo, DateTime? startTime, DateTime? endTime, [FromBody] List<string>? rangids = null)
        {

            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
            string Ids = "";
            if (rangids != null)
            {
                Ids = "'" + string.Join("','", rangids.ToArray()) + "'";
                if (rangids.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iInsTaskStatisticsDAL.GetInspectorWorkloadStatistics(startTime, endTime, Ids, sqlCondition);
            return result;
        }
    }
}