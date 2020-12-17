using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.InspectionSettings.Statistics
{
    public class InspectionStaticsDAL : IInspectionStaticsDAL
    {
        public MessageEntity GetByOwnID(string iAdminID, string? task_Type_id, string? isFinish, DateTime? startTime, DateTime? endTime)
        {
            string sqlCondition = "  ";
            if (iAdminID != null && iAdminID !="")
            {
                sqlCondition += $" and proraterid='{iAdminID}' ";
            }
            if (task_Type_id != null)
            {
                sqlCondition += $" and t.task_typeid in({task_Type_id}) ";
            }
            if (isFinish != null)
            {
                if (isFinish == "2")//未开始
                {
                    sqlCondition += $" and  sysdate < t.visitstartime ";
                }
                else if (isFinish == "3")//已超时
                {
                    sqlCondition += $" and  t.isfinish=0 and  sysdate>t.visitovertime ";
                }
                else
                {
                    sqlCondition += $" and isFinish={isFinish} ";
                }
            }
            if (startTime != null && endTime != null)
            {
                //DateTime startTime1 = DateTime.Parse(startTime.ToString()).AddDays(1).AddSeconds(-1);
                //sqlCondition += $@"     AND (      (t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR (t.visitovertime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime < to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime >=  to_date('{startTime1}', 'yyyy-mm-dd hh24:mi:ss'))
                //                   )";
                sqlCondition += $@" AND  t.visitstartime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')  AND t.visitovertime>= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') ";

            }
            sqlCondition += "  order by floor((nvl(completecount,0)/allcount)*100) desc";

            string sql = @" select t.TaskId, t.isFinish,ty.Task_Type_Id,ty.task_type_name,t.taskcode,t.taskname,t.proratername,t.visitstartime,t.visitovertime,t.travelmileage,p.geometry,allcount,nvl(completecount,0)completecount ,nvl(reportcount,0)reportcount,
   case nvl(allcount,0) when 0 then 0  else floor((nvl(completecount,0)/allcount)*100)   end as  WclInt,
   case when  sysdate<t.visitstartime then '未开始'when t.isfinish=0 and  sysdate>t.visitovertime then '已超时' when  t.isfinish=0 then '执行中' when  t.isfinish=1 then  '已完成' else '其他'end as IsFinishName from ins_task t 
                         left join ins_plan p on p.plan_id=t.plan_id
                         left join ins_task_type ty on ty.task_type_id=t.task_typeid
                         left join (select pt.taskid,count(0) completecount from INS_plan_task pt where pt.isfinish=1 group by pt.taskid) pt on pt.taskid=t.taskid
                         left join (select pt.taskid,count(0) allcount from INS_plan_task pt  group by pt.taskid) ptc on ptc.taskid=t.taskid
                         left join (select pt.taskid,count(0) reportcount from INS_plan_task pt where pt.isreport=1 group by pt.taskid) pts on pts.taskid=t.taskid
where 1=1 and t.taskstate=1 and t.assignState=1" + sqlCondition;
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_TaskDto> list = conn.Query<Ins_TaskDto>(sql).ToList();
 //                   //取得当前对应的数据列表信息
 //                   foreach (Ins_TaskDto row in list)s
 //                   {
 //                       using (var con = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
 //                       {
 //                           row.EquipmentDetailInfoList = conn.Query<EquipmentDetailInfoDto>(@"select c.taskid,e.equipment_info_code,e.equipment_info_name,e.caliber,e.address,c.x  Lon,c.y  lat,e.globid,pt.Plan_Task_Id,pt.isreport,pe.layername 
 //from ins_task_completedetail c
 //                left join ins_plan_task pt on c.taskid=pt.taskid  
 //                left join INS_plan_equipment_info e on e.equipment_info_id = pt.equipment_info_id 
 //                left join INS_plan_equipment pe on pe.plan_equipment_id=e.plan_equipment_id  where c.TaskId = '" + row.TaskId.ToString() + "'" + sqlEquipment).ToList();
 //                       }
 //                   }

                    return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());
                }
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetInspectionOverview(DateTime? startTime, DateTime? endTime, string rangName)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " " , eventWhere = " ", lasteventWhere = " ";
            if (startTime != null && endTime != null)
            {
                DateTime startTime1 = DateTime.Parse(startTime.ToString()).AddDays(1).AddSeconds(-1);
                DateTime LastStartTime = DateTime.Parse(startTime.ToString()).AddYears(-1);
                DateTime LastendTime = DateTime.Parse(endTime.ToString()).AddYears(-1);

                //sqlwhere += $@"     AND (      (t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR (t.visitovertime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime < to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime >= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitovertime >= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss'))
                //                              OR(t.visitstartime <= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') AND t.visitovertime >=  to_date('{startTime1}', 'yyyy-mm-dd hh24:mi:ss'))
                //                   )";
                sqlwhere += $@" AND  t.visitstartime <= to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')  AND t.visitovertime>= to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss') ";

                eventWhere += $" and e.UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')  and e.UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
              lasteventWhere += $" and e.UpTime>=to_date('{LastStartTime}', 'yyyy-mm-dd hh24:mi:ss')  and e.UpTime<=to_date('{LastendTime}', 'yyyy-mm-dd hh24:mi:ss')";

            }
            if (rangName != null && rangName != "" )
            {
                eventWhere += $" and e.rangName ='{rangName}'";
                lasteventWhere += $" and e.rangName ='{rangName}'";

            }
            #endregion
            string query = $@"select count(0) gdzs from ins_event e where e.deletestatus=0 {eventWhere} --工单总数
                                    union all--工单处理数
                                        select count(0) gdcls from ins_event e  
                                             left join (   select h.eventid,h1.OperId from 
                                                              (SELECT MAX(opertime) AS opertime,h.EventID
                                                                         FROM ins_workorder_oper_history h  GROUP BY h.EventID)  h
                                                                          LEFT OUTER JOIN ins_workorder_oper_history h1 ON h.EventID = h1.EventID AND h.opertime =h1.opertime
                                                         )  HH on hh.eventid=e.eventid where e.deletestatus=0 and hh.operId <> 11 {eventWhere} --事件时间条件
                                    union all --巡查公里数
                                        select  nvl(sum(t.travelmileage),0) sumtravelmileage  from ins_task t where t.taskstate=1 {sqlwhere}--任务时间条件
                                    union all --巡查上报数
                                        select count(0) xcsbs from  ins_event e  where e.deletestatus=0 and e.plan_task_id is not null {eventWhere}
                                    union all --计划任务数
                                         select count(0) jhrws  from ins_task t where t.taskstate=1 {sqlwhere}--任务时间条件 
                                    union all -- 接单率
                                        select case count(e.eventid) when 0 then 0 else round((count(hh.eventid)/count(e.eventid))*100,2)  end as jdl from ins_event e  
                                             left join (   select h.eventid,h1.OperId from 
                                                              (SELECT MAX(opertime) AS opertime,h.EventID
                                                                         FROM ins_workorder_oper_history h  GROUP BY h.EventID)  h
                                                                          LEFT OUTER JOIN ins_workorder_oper_history h1 ON h.EventID = h1.EventID AND h.opertime =h1.opertime
                                                                where  h1.operId not in( 11,2)
                                                         )  HH on hh.eventid=e.eventid where e.deletestatus=0 {eventWhere} --事件时间条件
                                    union all --完成率
                                        select  case count(e.eventid) when 0 then 0 else  round((count(ee.eventid)/count(e.eventid))*100,2)  end as wcl from ins_event e   
                                              left join (select e.eventid from ins_event e  where e.deletestatus=0  and e.isfinish=1 {eventWhere} --事件时间条件
                                              ) ee on ee.eventid=e.eventid where e.deletestatus=0 {eventWhere}--事件时间条件
                                    union all --同比去年 接单率
                                        select case count(e.eventid) when 0 then 0 else round((count(hh.eventid)/count(e.eventid))*100,2)  end as jdl from ins_event e  
                                             left join (   select h.eventid,h1.OperId from 
                                                              (SELECT MAX(opertime) AS opertime,h.EventID
                                                                         FROM ins_workorder_oper_history h  GROUP BY h.EventID)  h
                                                                          LEFT OUTER JOIN ins_workorder_oper_history h1 ON h.EventID = h1.EventID AND h.opertime =h1.opertime
                                                                where  h1.operId not in( 11,2)
                                                         )  HH on hh.eventid=e.eventid where e.deletestatus=0  {lasteventWhere}--事件时间条件
                                    union all --同比去年完成率
                                        select  case count(e.eventid) when 0 then 0 else  round((count(ee.eventid)/count(e.eventid))*100,2)  end as wcl from ins_event e   
                                              left join (select e.eventid from ins_event e  where e.deletestatus=0  and e.isfinish=1 {lasteventWhere}--事件时间条件
                                              ) ee on ee.eventid=e.eventid where e.deletestatus=0 {lasteventWhere}--事件时间条件";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query));
                    List<InspectionOverviewDto> list = new List<InspectionOverviewDto>();
                    InspectionOverviewDto model = new InspectionOverviewDto();
                    model.Gdzs = dt.Rows[0][0].ToString();
                    model.Gdcls = dt.Rows[1][0].ToString();
                    model.Xcgls = dt.Rows[2][0].ToString();
                    model.Xcsbs = dt.Rows[3][0].ToString();
                    model.Jhrws = dt.Rows[4][0].ToString();
                    model.Jdl = dt.Rows[5][0].ToString();
                    model.Wcl = dt.Rows[6][0].ToString();
                    model.SxfJdl = (double.Parse(dt.Rows[5][0].ToString())-double.Parse( dt.Rows[7][0].ToString())).ToString();
                    model.SxfWcl = (double.Parse(dt.Rows[6][0].ToString()) - double.Parse(dt.Rows[8][0].ToString())).ToString();
                    list.Add(model);

                    return MessageEntityTool.GetMessage(list.Count, list, true, "", list.Count);
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
