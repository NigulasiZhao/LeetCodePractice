using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.Plan;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Plan
{
    public class Ins_P_PositionDAL : IIns_P_PositionDAL
    {
       
 
        /// <summary>
        /// 获取任务轨迹信息
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="getTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public MessageEntity GetInspectionRoute(string taskid, DateTime getTime, DateTime endTime)
        {
            string queryStr = $@"SELECT  to_char(a.uptime,'hh24:mi') as nowTime,
                           a.*
                    FROM INS_Task_CompleteDetail  a
                    WHERE  a.taskid='{taskid}' and a.UpTime>= to_date('{getTime}', 'yyyy-mm-dd hh24:mi:ss') and a.UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    List<dynamic> result = conn.Query<dynamic>(queryStr).ToList();

                    return MessageEntityTool.GetMessage(result.Count(), result);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity GetRouteByTaskId(string taskid)
        {
            string sqlWhere = " where 1=1 ";
            if (taskid != null)
            {
                sqlWhere += $" and c.taskid in('{taskid}') ";
            }
            string queryStr = $@"select  to_char(c.uptime,'hh24:mi') as nowTime, c.*,
case pt.tableid when 'Ins_Form_LeakDetection' then c.x  else pe.lon end as lon,case pt.tableid when 'Ins_Form_LeakDetection' then c.y  else pe.lat end as lat 
  from Ins_Task_CompleteDetail c left join  ins_plan_task pt  on c.plan_task_id=pt.plan_task_id left join INS_plan_equipment_info pe on pe.globid=c.devicesmid  {sqlWhere}  order by c.uptime asc ";

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {

                    List<Ins_Task_CompleteDetailDto> result = conn.Query<Ins_Task_CompleteDetailDto>(queryStr).ToList();

                    return MessageEntityTool.GetMessage(result.Count(), result);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }


    }
}
