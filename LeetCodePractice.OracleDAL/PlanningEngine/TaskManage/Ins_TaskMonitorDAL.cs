using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.TaskManage
{
    public class Ins_TaskMonitorDAL : IIns_TaskMonitorDAL
    {
        public MessageEntity Get(DateTime? startTime, DateTime? endTime, string proraterDeptId, string proraterId, string rangids, string task_Type_id, string sort, string ordering, int num, int page)
        {
            string sqlwhere = " ";
            if (startTime != null && endTime != null)
            {
                //DateTime startTime1 = DateTime.Parse(startTime.ToString()).AddDays(1).AddSeconds(-1);
                //sqlwhere += $@"     AND (      (t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR (t.visitovertime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime < to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime >=  to_date('{startTime1}', 'yyyy-mm-dd hh24:mi:ss'))
                //                   )";
                sqlwhere += $@" AND  t.visitstartime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')  AND t.visitovertime>= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') ";
            }
            if (proraterDeptId != null && proraterDeptId!="")
            {
                sqlwhere += $" and t.proraterDeptId in('{proraterDeptId}') ";
            }
            if (proraterId != null && proraterId != "")
            {
                sqlwhere += $" and t.proraterId in('{proraterId}') ";
            }
            if (task_Type_id != null)
            {
                sqlwhere += $" and ty.task_type_id in('{task_Type_id}') ";
            }
            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlwhere += $" and p.range_id in ({rangids})";
            }
            string queryStr = $@"select t.taskid, t.ProraterName,t.proraterDeptName,t.TaskName,t.TaskCode  ,t.VisitStarTime,t.VisitOverTime,p.geometry,case when r.type is null then 1 else r.type end as type,ty.task_Type_id,ty.task_type_name ,pc.plan_cycle_name,t.travelmileage
                                    ,case when  sysdate<t.visitstartime then '未开始'when t.isfinish=0 and  sysdate>t.visitovertime then '已超时' when  t.isfinish=0 then '执行中' when  t.isfinish=1 then  '已完成' else '其他'end as IsFinishName  
,t.operatedate , nvl(allcount,0) allcount,nvl(completecount,0)completecount, nvl(allcount,0) -nvl(completecount,0) nocompletecount, case nvl(allcount,0) when 0 then 0  else round((nvl(completecount,0)/allcount)*100,2)   end as wcl 
                                 from ins_task t left join ins_plan p on p.plan_id=t.plan_id
                                  left join ins_plan_cycle pc on pc.plan_cycle_id=p.plancycleid
                                  left join ins_range r on r.range_id=p.range_id  
                                  left join ins_task_type ty on ty.task_type_id=T.Task_TypeId
                                  left join (select pt.taskid,count(0) completecount from INS_plan_task pt where pt.isfinish=1 group by pt.taskid) pt on pt.taskid=t.taskid
                                  left join (select pt.taskid,count(0) allcount from INS_plan_task pt  group by pt.taskid) ptc on ptc.taskid=t.taskid 
                              where 1=1 and t.taskstate=1 and t.assignState=1 {sqlwhere}  ";

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    List<Ins_TaskMonitorDto> ResultList = DapperExtentions.EntityForSqlToPager<Ins_TaskMonitorDto>(queryStr, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);


                    return result;
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }


    }
}
