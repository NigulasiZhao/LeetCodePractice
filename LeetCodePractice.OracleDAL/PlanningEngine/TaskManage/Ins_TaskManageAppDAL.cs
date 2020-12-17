using Dapper;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.TaskManage
{
    public class Ins_TaskManageAppDAL : IIns_TaskManageAppDAL
    {
        public MessageEntity Get(string iAdminID,string taskName, string taskid, string isFinish,string task_Type_id, DateTime? startTime, DateTime? endTime, string sort, string ordering, int num, int page)
        {
            string sqlCondition = "  ";
            if(iAdminID != null&& iAdminID != "")
            {
                sqlCondition += $" and proraterid='{iAdminID}' ";
            }
            if (taskid != null)
            {
                sqlCondition += $" and t.taskid='{taskid}' ";
            }
            if (taskName != null)
            {
                sqlCondition += $" and t.taskName like'%{taskName}%' ";
            }
            if (task_Type_id != null)
            {
                sqlCondition += $" and ty.task_type_id in({task_Type_id}) ";
            }
            if (isFinish != null && isFinish != "")
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
            string sql = $@"select * from ( select t.taskid,t.taskcode,t.taskname,ty.task_type_name,t.visitstartime,t.visitovertime,case when  sysdate<t.visitstartime then '未开始'when t.isfinish=0 and  sysdate>t.visitovertime then '已超时' when  t.isfinish=0 then '执行中' when  t.isfinish=1 then  '已完成' else '其他'end as IsFinishName
                                  ,p.plan_id,p.plan_name,t.proraterid,t.proratername,p.Geometry , p.moveType,case p.isfeedback when 0 then '需反馈' when 1 then  '仅到位' else '其他'end as isfeedback,allcount,nvl(completecount,0)completecount ,nvl(reportcount,0)reportcount, case nvl(allcount,0) when 0 then 0  else round((nvl(completecount,0)/allcount)*100,2)   end as wcl 
                         from INS_TASK t
                         left join ins_plan p on p.plan_id=t.plan_id 
                         left join ins_task_type ty on ty.task_type_id=t.task_typeid
                         left join (select pt.taskid,count(0) completecount from INS_plan_task pt where pt.isfinish=1 group by pt.taskid) pt on pt.taskid=t.taskid
                         left join (select pt.taskid,count(0) allcount from INS_plan_task pt  group by pt.taskid) ptc on ptc.taskid=t.taskid
                         left join (select pt.taskid,count(0) reportcount from INS_plan_task pt where pt.isreport=1 group by pt.taskid) pts on pts.taskid=t.taskid where 1=1 and t.taskstate=1 and assignState=1 {sqlCondition}) t " ;
            try
            {
                List<Ins_TaskDto> ResultList = DapperExtentions.EntityForSqlToPager<Ins_TaskDto>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
                //取得当前对应的数据列表信息
                //foreach (Ins_TaskDto row in ResultList)
                //{
                //    using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                //    {
                //        row.EquipmentDetailInfoList = conn.Query<EquipmentDetailInfoDto>(@" select t.taskid,t.taskname,pt.tableid,e.equipment_info_name||'('||t.taskcode||')' as equipmentinfo,e.equipment_info_code,e.equipment_info_name,e.caliber,e.address,e.lon,e.lat,e.globid,pt.Plan_Task_Id,pt.isreport,pe.layername  from INS_TASK t left join ins_plan_task pt on t.taskid=pt.taskid  
                //                                                        left join INS_plan_equipment_info e on e.equipment_info_id = pt.equipment_info_id left join INS_plan_equipment pe on pe.plan_equipment_id=e.plan_equipment_id where t.TaskId = '" + row.TaskId.ToString() + "'"+ sqlEquipment).ToList();
                //        foreach (EquipmentDetailInfoDto item in row.EquipmentDetailInfoList)
                //        {
                //            string query = $" select nvl(IsFillForm,0) IsFillForm,ID from { item.TableId} where Plan_Task_Id='{item.Plan_Task_Id}'";
                //            DataTable dt = new DataTable();
                //            dt.Load(conn.ExecuteReader(query));
                //            item.IsFillForm = 0;
                //            if (dt.Rows.Count > 0)
                //            {
                //                item.IsFillForm =int.Parse( dt.Rows[0]["IsFillForm"].ToString());
                //                item.ID = dt.Rows[0]["ID"].ToString();
                //            }

                //        }
                //    }
                //}
                return result;
            }
            catch(Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetWorkCode(string tableId)
        {
            string errorMsg = "";
            string query = $"SELECT 'GDBH'  ||  to_char(sysdate,'yyyymmdd')|| nvl(replace(lPAD( MAX( substr(workCode,13,6)+ 1),6), ' ','0'),'000001') as workCode from {tableId} where  operateDate>= to_date( to_char(sysdate, 'YYYY-MM-DD'), 'YYYY-MM-DD')";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<dynamic> eventType = conn.Query<dynamic>(query).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
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
