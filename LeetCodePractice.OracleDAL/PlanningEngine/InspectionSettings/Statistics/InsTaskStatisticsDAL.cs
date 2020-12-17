using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics;
using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using System.Data;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.InspectionSettings.Statistics
{
    public class InsTaskStatisticsDAL : IInsTaskStatisticsDAL
    {
        public MessageEntity GetInspectorWorkloadStatistics(DateTime? startTime, DateTime? endTime, string rangids, string sqlCondition)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = sqlCondition;
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
            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlwhere += $" and p.range_id in ({rangids})";
            }
            sqlwhere += " and t.taskstate=1";
            #endregion
            string query = string.Format(@"  select t.TASKNAME,t.taskcode,t.visitstartime,t.visitovertime, t.proraterdeptname, t.proratername ,nvl(reportcount,0)reportcount, 
                                 allcount,nvl(completecount,0)completecount ,t.travelmileage,tc.days,round(case days when 0 then  travelmileage else travelmileage/days end ,1) daytravelmileage
                         from INS_TASK t
                         left join INS_plan p on p.plan_id=t.plan_id
                         left join (select pt.taskid,count(0) completecount from INS_plan_task pt where pt.isfinish=1 group by pt.taskid) pt on pt.taskid=t.taskid
                         left join (select pt.taskid,count(0) allcount from INS_plan_task pt  group by pt.taskid) ptc on ptc.taskid=t.taskid
                         left join (select pt.taskid,count(0) reportcount from INS_plan_task pt where pt.isreport=1 group by pt.taskid) pts on pts.taskid=t.taskid 
                         left join (  select tc.taskid,max(tc.uptime) maxuptime ,min(tc.uptime) minuptime,round(max(tc.uptime)-min(tc.uptime),0)days from ins_task_completedetail  tc group by  tc.taskid) tc on tc.taskid=t.taskid   {0}", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_TaskDto> eventType = conn.Query<Ins_TaskDto>(query).ToList();
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", eventType.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetTask(DateTime? startTime, DateTime? endTime, string rangids, string task_Type_id)
        {
            string errorMsg = "";
            string sqlCondition = " where 1=1 ";
            if (task_Type_id != null)
            {
                sqlCondition += $" and ty.task_type_id in({task_Type_id}) ";
            }
            if (startTime != null && endTime != null)
            { sqlCondition += $@" AND (      (t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss'))
                                              OR(t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                                              OR(t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                                              OR(t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                                                        	)";
               
            }
            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlCondition += $" and p.range_id in ({rangids})";
            }
            string sql = @"SELECT t.TASKNAME,t.taskid,p.range_id, p.range_name,p.geometry
                                        FROM
                                          INS_TASK  t 
                                          left join ins_task_type ty on ty.task_type_id=t.Task_TypeId
                                          left join INS_plan p on p.plan_id=t.plan_id
                                            " + sqlCondition;
            List<Ins_TaskStaticDto> ResultList = null;
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    ResultList = conn.Query<Ins_TaskStaticDto>(sql).ToList();
                    //删除已经删除的配置项
                    List<string> FieldIDS = ResultList.Select(Row => Row.TaskId).ToList();
                    string Ids = "'" + string.Join("','", FieldIDS.ToArray()) + "'";
                    if (FieldIDS.Count <= 0)
                    {
                        Ids = "'" + "0" + "'";

                    }
                        List<EquipmentInfoDto> EquipmentInfoList = conn.Query<EquipmentInfoDto>($@"select pt.taskid, pt.tableid,   pt.isfinish,e.globid,e.lon,e.lat ,e.EquType,e.Geometry  from  ins_plan_task pt
                                                                        left join INS_plan_equipment_info e on e.equipment_info_id = pt.equipment_info_id where pt.taskid in({Ids})").ToList();
                    //取得当前对应的数据列表信息
                    ResultList.ForEach(row1 =>
                    {
                        row1.EquipmentDetailInfoList = EquipmentInfoList.Where(x => x.TaskId == row1.TaskId).ToList();
                    });

                    return MessageEntityTool.GetMessage(ResultList.Count, ResultList, true, "", ResultList.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetTaskExecuteRate(DateTime? startTime, DateTime? endTime, string rangids)
        {
            string errorMsg = "";
            #region 条件
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
            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlwhere += $" and p.range_id in ({rangids})";
            }
            #endregion
            string query = string.Format(@"select p.plan_name,t.TASKNAME,t.taskcode,t.visitstartime,t.visitovertime, t.proraterdeptname,t.proratername ,allcount,nvl(completecount,0)completecount ,nvl(reportcount,0)reportcount, case allcount when 0 then 0 ||'% ' else round((nvl(completecount,0)/allcount)*100,2)  ||'% ' end as wcl 
                         from INS_TASK t
                         left join ins_plan p on p.plan_id=t.plan_id 
                         left join ins_task_type ty on ty.task_type_id=t.task_typeid
                         left join (select pt.taskid,count(0) completecount from INS_plan_task pt where pt.isfinish=1 group by pt.taskid) pt on pt.taskid=t.taskid
                         left join (select pt.taskid,count(0) allcount from INS_plan_task pt  group by pt.taskid) ptc on ptc.taskid=t.taskid
                         left join (select pt.taskid,count(0) reportcount from INS_plan_task pt where pt.isreport=1 group by pt.taskid) pts on pts.taskid=t.taskid where 1=1 and t.taskstate=1 {0}", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<InsTaskExecuteRate> eventType = conn.Query<InsTaskExecuteRate>(query).ToList();
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", eventType.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetTaskStateEchart(DateTime? startTime, DateTime? endTime, string rangids, string? task_Type_id = null)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (task_Type_id != null)
            {
                sqlwhere += $" and t.task_typeid in('{task_Type_id}') ";
            }
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
            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlwhere += $" and p.range_id in ({rangids})";
            }
            #endregion
            string query = string.Format(@"         
select taskstate  Name,count(0) sumcount from(
select  case  when t.assignstate=0 then '待分派' when t.isfinish=1 then '已完成' else '处理中' end as  taskstate
 from ins_task t    left join ins_plan p on p.plan_id=t.plan_id  where t.taskstate=1 {0})
 group by taskstate", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EchartDto> eventType = conn.Query<EchartDto>(query).ToList();
                    var rows = 0;
                    foreach (EchartDto item in eventType)
                    {
                        rows += item.SumCount;
                    }
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", rows);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetTaskStatistics(DateTime? startTime, DateTime? endTime, string rangids, string sqlCondition)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = sqlCondition;
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
            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlwhere += $" and p.range_id in ({rangids})";
            }
            sqlwhere += " and t.taskstate=1";
            #endregion
            string query = string.Format(@" select t.proraterdeptname, t.proratername ,t.taskcode,t.taskname,t.visitstartime,t.visitovertime,pc.plan_cycle_name,case t.isfinish when 0 then '执行中' when 1 then  '已完成' else '其他'end as IsFinishName
                                 ,allcount,nvl(completecount,0)completecount ,allcount-nvl(completecount,0) nocompletecount ,nvl(reportcount,0)reportcount,round((nvl(completecount,0)/allcount)*100,2) wcl
                         from INS_TASK t
                         left join ins_plan p on p.plan_id=t.plan_id 
                         left join ins_plan_cycle pc on pc.plan_cycle_id=p.plancycleid
                         left join ins_task_type ty on ty.task_type_id=t.task_typeid
                         left join (select pt.taskid,count(0) completecount from INS_plan_task pt where pt.isfinish=1 group by pt.taskid) pt on pt.taskid=t.taskid
                         left join (select pt.taskid,count(0) allcount from INS_plan_task pt  group by pt.taskid) ptc on ptc.taskid=t.taskid
                         left join (select pt.taskid,count(0) reportcount from INS_plan_task pt where pt.isreport=1 group by pt.taskid) pts on pts.taskid=t.taskid    {0}", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_TaskDto> eventType = conn.Query<Ins_TaskDto>(query).ToList();
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", eventType.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetTaskTypeEchart(DateTime? startTime, DateTime? endTime, string rangids, string? task_Type_id = null)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (task_Type_id != null)
            {
                sqlwhere += $" and t.task_typeid in('{task_Type_id}') ";
            }
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
            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlwhere += $" and p.range_id in ({rangids})";
            }
            #endregion
            string query = string.Format(@" select tt.task_type_name Name , count(0) sumcount 
 from ins_task t left join ins_task_type tt on tt.task_type_id=t.task_typeid  left join ins_plan p on p.plan_id=t.plan_id  where t.taskstate=1  {0}
 group by task_type_name,task_type_id", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EchartDto> eventType = conn.Query<EchartDto>(query).ToList();

                    var rows = 0;
                    foreach (EchartDto item in eventType)
                    {
                        rows += item.SumCount;
                    }
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", rows);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }
    }
}
