using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.InspectionSettings.Statistics
{
    /// <summary>
    /// 事件统计--包含Echart图数据
    /// </summary>
    public class InsEventStatisticsController : Controller
    {
        private readonly IIns_EventStatisticsDAL _iIns_EventStatisticsDAL;
        public InsEventStatisticsController(IIns_EventStatisticsDAL iIns_EventStatisticsDAL)
        {
            _iIns_EventStatisticsDAL = iIns_EventStatisticsDAL;

        }  /// <summary>
           ///  获取事件来源Echart数据
           /// </summary>
           /// <param name="startTime">开始时间 yyy-mm-dd</param>
           /// <param name="endTime">结束时间 yyy-mm-dd</param>
           /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventFromEchart(DateTime? startTime, DateTime? endTime)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);
           
            var result = _iIns_EventStatisticsDAL.GetEventFromEchart(startTime, endTime);
            return result;
        }
        /// <summary>
        ///  获取事件类型Echart数据
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventTypeEchart(DateTime? startTime, DateTime? endTime)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);

            var result = _iIns_EventStatisticsDAL.GetEventTypeEchart(startTime, endTime);
            return result;
        }
        /// <summary>
        ///  获取事件状态chart数据
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventOperEchart(DateTime? startTime, DateTime? endTime)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);

            var result = _iIns_EventStatisticsDAL.GetEventOperEchart(startTime, endTime);
            return result;
        }

        /// <summary>
        ///  获取事件类型分析(表格数据)
        /// </summary>
        /// <param name="startTime">开始时间 yyy-mm-dd</param>
        /// <param name="endTime">结束时间 yyy-mm-dd</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventTypeDT(DateTime? startTime, DateTime? endTime)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);

            var result = _iIns_EventStatisticsDAL.GetEventTypeDT(startTime, endTime);
            return result;
        }
        /// <summary>
        /// 事件类型趋势分析--Table
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="startMonth">开始月</param>
        /// <param name="endMonth">结束月</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventTypeTrendTable(int year, int startMonth, int endMonth)
        {
            var monthArry = GetMonthArry(startMonth, endMonth);
            if (monthArry == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            var result = _iIns_EventStatisticsDAL.GetEventTypeTrendTable(monthArry, year.ToString(), startMonth.ToString(), endMonth.ToString());
            return result;

        }

        /// <summary>
        /// 事件人员上报分析-table/pieChart/lineChart(startTime/endTime都不填的话 则统计所有时间段内事件)
        /// </summary>
        /// <param name="startTime">yyyy-MM-dd</param>
        /// <param name="endTime">yyyy-MM-dd</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetUserReportTable(DateTime? startTime = null, DateTime? endTime = null)
        {
            if ((startTime == null && endTime == null) || (startTime != null && endTime != null && startTime.Value <= endTime.Value))
            {
                if (endTime != null)
                    endTime = endTime.Value.AddDays(1).AddSeconds(-1);

                return _iIns_EventStatisticsDAL.GetUserReportTable(startTime, endTime);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
        }
        /// <summary>
        /// 事件数量统计按照网格区域名称
        /// </summary>
        /// <param name="startTime">yyyy-MM-dd</param>
        /// <param name="endTime">yyyy-MM-dd</param>
        /// <param name="rangName">网格区域名称</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventByRangName(DateTime? startTime = null, DateTime? endTime = null, string? rangName=null)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);

            var result = _iIns_EventStatisticsDAL.GetEventByRangName(startTime, endTime,rangName);
            return result;
        }
        /// <summary>
        /// 事件接单率完成率统计按照网格区域名称和时间月份
        /// </summary>
        /// <param name="startTime">yyyy-MM-dd</param>
        /// <param name="endTime">yyyy-MM-dd</param>
        /// <param name="rangName">网格区域名称</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventEchartByRangeDate(DateTime? startTime = null, DateTime? endTime = null, string? rangName = null)
        {
            if (endTime != null)
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1).AddSeconds(-1);

            var result = _iIns_EventStatisticsDAL.GetEventEchartByRangeDate(startTime, endTime, rangName);
            return result;
        }
        private string[] GetMonthArry(int startMonth, int endMonth)
        {
            if (endMonth < startMonth)
                return null;
            if (endMonth == startMonth)
                return new string[] { startMonth + "月" };
            List<string> monthList = new List<string>();
            for (int i = startMonth; i <= endMonth; i++)
            {
                monthList.Add(i + "月");
            }
            return monthList.ToArray();
        }
    }
}