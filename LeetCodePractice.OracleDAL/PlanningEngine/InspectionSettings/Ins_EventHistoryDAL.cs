using GisPlateform.Model;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Database;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GisPlateformForCore.OracleDAL.PlanningEngine.InspectionSettings
{
    public class Ins_EventHistoryDAL : IIns_EventHistoryDAL
    {
        /// <summary>
        /// 获得计划周期信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity Get(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select t.eventtypename, e.*  from ins_event e LEFT JOIN ins_event_type T ON e.eventtypeid=t.event_type_id 
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_EventList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
    }
}
