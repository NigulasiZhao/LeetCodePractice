using Dapper;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.InspectionSettings;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using GISWaterSupplyAndSewageServer.Model.UniWater;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RabbitMQ.Client;
using System.Threading;
using System.Collections.Concurrent;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.TaskManage
{
    public class Ins_TaskManageDAL : IIns_TaskManageDAL
    {
        /// <summary>
        /// 任务明细完成方法
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public MessageEntity TaskDetailComplete(Ins_Task_Completedetail Model)
        {
            //查询改任务下是否还存在未完成的任务明细
            string GetAllTaskDetail = string.Format(@"SELECT COUNT(0) as count FROM INS_PLAN_TASK WHERE TASKID = '{0}' AND ISFINISH = 0 AND PLAN_TASK_ID <> '{1}' ", Model.Taskid, Model.Plan_Task_Id);
            string GetTaskDetail = string.Format(@"SELECT COUNT(0) as count FROM INS_PLAN_TASK WHERE TASKID = '{0}' AND ISFINISH = 1 AND PLAN_TASK_ID = '{1}' ", Model.Taskid, Model.Plan_Task_Id);
            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                IDbTransaction transaction = conn.BeginTransaction();
                int IsCompleteState = conn.Query<int>(GetTaskDetail).FirstOrDefault();
                if (IsCompleteState > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "同一任务明细不能重复完成");
                }
                int dtSelect = conn.Query<int>(GetAllTaskDetail).FirstOrDefault();
                if (dtSelect == 0)
                {
                    string UpateTaskStateSql = string.Format(@"UPDATE INS_TASK SET ISFINISH = 1 WHERE TASKID = '{0}' ", Model.Taskid);
                    conn.Execute(UpateTaskStateSql, null, transaction);
                }
                string UpateTaskDetailStateSql = string.Format(@"UPDATE INS_PLAN_TASK SET ISFINISH = 1 WHERE Plan_Task_Id = '{0}' ", Model.Plan_Task_Id);
                conn.Execute(UpateTaskDetailStateSql, null, transaction);
                conn.Execute(DapperExtentions.MakeInsertSql(Model), Model, transaction);
                transaction.Commit();
            }
            return MessageEntityTool.GetMessage(0, null, true, "任务完成成功", 0);
        }
        /// <summary>
        /// 获得任务信息列表
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity Get(List<ParameterInfo> parInfo, string rangids, string task_Type_id, string taskState, DateTime? startTime, DateTime? endTime, string sort, string ordering, int num, int page, string sqlCondition)
        {
            //任务状态  1：已派发 2：未派发  3：已完成   4：已作废  5 处理中（已分派未完成）
            if (taskState != null)
            {
                if (taskState == "1")
                    sqlCondition += " and assignstate=1 and taskstate=1";
                if (taskState == "2")
                    sqlCondition += " and assignstate=0";
                if (taskState == "3")
                    sqlCondition += " and isfinish=1 and taskstate=1";
                if (taskState == "4")
                    sqlCondition += " and taskstate=0";
                if (taskState == "5")
                    sqlCondition += " and assignstate=1 and isfinish=0 and taskstate=1";
            }
            if (task_Type_id != null)
            {
                sqlCondition += $" and t.task_type_id in({task_Type_id}) ";
            }

            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlCondition += $" and t.range_id in ({rangids})";
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
            string sql = @"select * from(SELECT
                                    case when  it.taskstate=0  then '已作废'  when  it.isfinish =0 then '未完成' when  it.isfinish =1 then  '已完成' else '其他'end as IsFinishName,
                                    case it.assignstate when 0 then '未派发' when 1 then  '已派发' else '其他'end as assignstateName,
                                    case it.taskstate when 0 then '已作废' when 1 then  '启用' else '其他'end as taskstateName,
                                       IT.TASKNAME,it.taskcode,
                                      it.proratername,it.proraterid,it.proraterdeptname,it.proraterdeptid, allcount,nvl(completecount,0)completecount, case nvl(allcount,0) when 0 then 0  else round((nvl(completecount,0)/allcount)*100,2)   end as wcl ,ty.task_type_name,it.visitstartime,it.visitovertime,
                                      p.plan_id,p.Plan_Name, p.range_id, p.range_name,p.geometry,  IT.TASKID,ty.task_type_id,
                                          IT.FREQUENCY,
                                          IT.DESCRIPT,
                                          IT.OPERATOR,
                                          IT.OPERATEDATE,
                                          IT.TASKSTATE,
                                          IT.REMARK,it.ASSIGNSTATE,it.isfinish,pc.plan_cycle_name
                                       
                                        FROM
                                          INS_TASK  IT 
                                          left join ins_task_type ty on ty.task_type_id=IT.Task_TypeId
                                          left join INS_plan p on p.plan_id=it.plan_id
                                          left join ins_plan_cycle pc on pc.plan_cycle_id=p.plancycleid
                                          left join (select pt.taskid,count(0) completecount from INS_plan_task pt where pt.isfinish=1 group by pt.taskid) pt on pt.taskid=IT.taskid
                                          left join (select pt.taskid,count(0) allcount from INS_plan_task pt  group by pt.taskid) ptc on ptc.taskid=IT.taskid  ) t " + sqlCondition;
            List<Ins_TaskDto> ResultList = DapperExtentions.EntityForSqlToPager<Ins_TaskDto>(sql, sort, ordering, num, page, out MessageEntity result, Database.ConnectionFactory.DBConnNames.ORCL);
            //取得当前对应的数据列表信息
            //foreach (Ins_TaskDto row in ResultList)
            //{
            //    using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            //    {
            //        row.EquipmentDetailInfoList = conn.Query<EquipmentDetailInfoDto>(@"select pt.plan_task_id,e.equipment_info_code,e.equipment_info_name,p.layername,pt.operatedate,  case pt.isfinish when 0 then '未完成' when 1 then  '已完成' else '其他'end as IsFinishName,e.caliber,e.address,e.lon,e.lat,e.globid  from  ins_plan_task pt
            //                                                            left join INS_plan_equipment_info e on e.equipment_info_id = pt.equipment_info_id 
            //                                                            left join INS_plan_equipment p on p.plan_equipment_id=e.plan_equipment_id where pt.TaskId = '" + row.TaskId.ToString() + "'").ToList();

            //    }
            //}

            return result;
        }
        /// <summary>
        /// 获取任务所包含设备信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <param name="sqlCondition"></param>
        /// <returns></returns>
        public MessageEntity GetEquipmentInfo(string taskId, string IsFillForm, string sort, string ordering, int num, int page, string X, string Y)
        {
            
            #region 勾股定理计算
            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                string GetTaskEquipmentInfoSql = $@"SELECT t.taskid,
                                                           t.taskname,
                                                           pt.plan_task_id,pt.isfinish IsFillForm,
                                                           pt.tableid,
                                                           e.equipment_info_code,
                                                           e.equipment_info_name,
                                                           pt.operatedate,
                                                           CASE pt.isfinish
                                                             WHEN 0 THEN
                                                              '未完成'
                                                             WHEN 1 THEN
                                                              '已完成'
                                                             ELSE
                                                              '其他'
                                                           END AS IsFinishName,
                                                           e.caliber,
                                                           e.address,
                                                           e.globid,
                                                           pt.isreport,
                                                           p.layername,
                                                           e.EquType,
                                                           e.Geometry,
                                                           CASE pt.tableid
                                                             WHEN 'Ins_Form_LeakDetection' THEN
                                                              case
                                                                when c.x is null then
                                                                 '0'
                                                                else
                                                                 c.x
                                                              end
                                                             ELSE
                                                              case
                                                                when e.lon is null then
                                                                 '0'
                                                                else
                                                                 e.lon
                                                              end
                                                           END AS lon,
                                                           CASE pt.tableid
                                                             WHEN 'Ins_Form_LeakDetection' THEN
                                                              case
                                                                when c.y is null then
                                                                 '0'
                                                                else
                                                                 c.y
                                                              end
                                                             ELSE
                                                              case
                                                                when e.lat is null then
                                                                 '0'
                                                                else
                                                                 e.lat
                                                              end
                                                           END AS lat
                                                     
                                                      FROM INS_Task t
                                                      LEFT JOIN ins_plan_task pt
                                                        ON pt.taskid = t.taskid
                                                      LEFT JOIN INS_plan_equipment_info e
                                                        ON e.equipment_info_id = pt.equipment_info_id
                                                      LEFT JOIN INS_plan_equipment p
                                                        ON p.plan_equipment_id = e.plan_equipment_id
                                                      LEFT JOIN ins_task_completedetail c
                                                        ON pt.plan_task_id = c.plan_task_id
                                                     where t.taskid = '{taskId}'  ";
                List<EquipmentDetailInfoDto> Returnlist = DapperExtentions.EntityForSqlToPager<EquipmentDetailInfoDto>(GetTaskEquipmentInfoSql, sort, ordering, num, page, out MessageEntity result, Database.ConnectionFactory.DBConnNames.ORCL);
                if (IsFillForm != null && IsFillForm != "")
                {
                    Returnlist = Returnlist.Where(x => x.IsFillForm == int.Parse(IsFillForm)).ToList();
                }
                if (!string.IsNullOrEmpty(X) && !string.IsNullOrEmpty(Y))
                {
                    //根据勾股定理计算两点之间距离  转换成公里
                    foreach (var item in Returnlist)
                    {
                        item.Distance = (decimal)(Math.Round(Math.Sqrt(Math.Pow(double.Parse(item.Lon) - double.Parse(X), 2.0) + Math.Pow(double.Parse(item.Lat) - double.Parse(Y), 2.0))/100d, 1) /10d);
                    }
                    Returnlist = Returnlist.OrderBy(e => e.Distance).ToList();
                }
                //foreach (EquipmentDetailInfoDto item in Returnlist)
                //{
                //    string query = $" select nvl(IsFillForm,0) IsFillForm,ID from { item.TableId} where Plan_Task_Id='{item.Plan_Task_Id}'";
                //    DataTable dt = new DataTable();
                //    dt.Load(conn.ExecuteReader(query));
                //    item.IsFillForm = 0;
                //    if (dt.Rows.Count > 0)
                //    {
                //        item.IsFillForm = int.Parse(dt.Rows[0]["IsFillForm"].ToString());
                //        item.ID = dt.Rows[0]["ID"].ToString();
                //    }
                //}
               
                return MessageEntityTool.GetMessage(Returnlist.Count(), Returnlist, true, "", Returnlist.Count());
            }
            #endregion
        }

        /// <summary>
        /// 获取任务信息及所属任务明细
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public MessageEntity GetTaskPlanInfo(string taskId)
        {
            MessageEntity Result = new MessageEntity();
            Result.Data = new DataInfo();
            //设备实体队列对象创建
            List<GetTaskDetailInfoDto> TaskDetailInfoList = new List<GetTaskDetailInfoDto>();
            //巡检任务详情
            Ins_Task TaskInfo = new Ins_Task();
            TaskDetailInfoList = GetTaskDetailInfo(taskId);
            try
            {
                using (var connOutSide = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
                {

                    string GetTaskSql = string.Format(@"SELECT
                                                        	TASKID,
                                                        	TASKNAME,
                                                        	PRORATERDEPTNAME,
                                                        	PRORATERDEPTID,
                                                        	PRORATERID,
                                                        	PRORATERNAME,
                                                        	VISITSTARTIME,
                                                        	VISITOVERTIME,
                                                        	FREQUENCY,
                                                        	DESCRIPT,
                                                        	OPERATOR,
                                                        	OPERATEDATE,
                                                        	TASKSTATE,
                                                        	PLAN_ID,
                                                        	PLAN_NAME,
                                                        	REMARK,
                                                        	ISFINISH,
                                                        	ASSIGNSTATE 
                                                        FROM
                                                        	INS_TASK WHERE TASKID = '{0}' ", taskId);
                    TaskInfo = connOutSide.Query<Ins_Task>(GetTaskSql).FirstOrDefault();
                    TaskInfo.TaskDetailInfoList = TaskDetailInfoList;
                    Result.Data.Result = TaskInfo;
                    Result.Flag = true;
                    Result.Msg = "请求成功";
                }

            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
            return Result;
        }
        /// <summary>
        /// 获取任务明细关联的设备信息
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public List<GetTaskDetailInfoDto> GetTaskDetailInfo(string TaskId)
        {
            List<GetTaskDetailInfoDto> list = new List<GetTaskDetailInfoDto>();
            string GetEquipmentSql = string.Format(@"SELECT
                                                            	IPT.PLAN_TASK_ID,
                                                            	IPT.EQUIPMENT_INFO_ID,
                                                            	IPT.PLAN_ID,
                                                            	IPT.START_TIME,
                                                            	IPT.TASKNAME,
                                                            	IPT.END_TIME,
                                                            	IPT.TABLEID,
                                                            	IPT.ISSUCCESS,
                                                            	IPT.CREATOR_ID,
                                                            	IPT.CREATOR_NM,
                                                            	IPT.DEPARTMENTID,
                                                            	IPT.DEPARTMENTNAME,
                                                            	IPT.EXECPERSONID,
                                                            	IPT.EXECPERSONNAME,
                                                            	IPT.TASKDESCRIPTION,
                                                            	IPT.ISFINISH,
                                                                IPT.TASKID,
                                                            	IPEI.PLAN_EQUIPMENT_ID,
                                                            	IPEI.EQUIPMENT_INFO_CODE,
                                                            	IPEI.EQUIPMENT_INFO_NAME,
                                                            	IPEI.ADDRESS,
                                                            	IPEI.CALIBER,
                                                            	IPEI.LON,
                                                            	IPEI.LAT,
                                                            	IPEI.CREATE_TIME
                                                            FROM
                                                            	INS_PLAN_TASK IPT
                                                            	LEFT JOIN INS_PLAN_EQUIPMENT IPE ON IPT.PLAN_ID = IPE.PLAN_ID
                                                            	LEFT JOIN INS_PLAN_EQUIPMENT_INFO IPEI ON IPEI.PLAN_EQUIPMENT_ID = IPE.PLAN_EQUIPMENT_ID 
                                                            	AND IPT.EQUIPMENT_INFO_ID = IPEI.EQUIPMENT_INFO_ID 
                                                            WHERE IPT.TASKID = '{0}' ", TaskId);
            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                list = conn.Query<GetTaskDetailInfoDto>(GetEquipmentSql).ToList();
            }
            return list;
        }

        public DataTable GetTaskCount(string taskId)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(taskId.ToString()))
            {
                strWhere += $" and t.taskId = '{taskId}' ";
            }
            string query = $@" select t.taskid,t.proraterid,allEquCount,nvl(CompletedCount,0) CompletedCount from ins_task t left join (
select lt.taskid,count(0) allEquCount from INS_plan_task lt group by lt.taskid)lt on lt.taskid=t.taskid
left join (
select lt.taskid,nvl(count(0),0) CompletedCount from INS_plan_task lt where lt.isfinish=1 group by lt.taskid)lt on lt.taskid=t.taskid where 1=1 {strWhere}";
            try
            {
                using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query));

                    return dt;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }

        public MessageEntity TaskCompleted(string taskId, string proraterId, string taskName, int? isFinsh)
        {
            

            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                string excSql = $" update ins_task   set isfinish={isFinsh} where taskid='{taskId}'";

                try
                {
                    rows = conn.Execute(excSql);
                    string IsEnableRabbit = Appsettings.app(new string[] { "IsEnableRabbit" });
                    if (IsEnableRabbit == "True")
                    {
                        #region 推送 任務完成時  恢復消息提醒 
                        string ExchangeName = Appsettings.app(new string[] { "RabbitExchangeName" });
                        string RoteKey = Appsettings.app(new string[] { "RabbitRoteKsy" });

                        RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory
                        {
                            UserName = Appsettings.app(new string[] { "RabbitUserName" }),
                            Password = Appsettings.app(new string[] { "RabbitPassword" }),
                            HostName = Appsettings.app(new string[] { "RabbitUrl" }),
                            Port = int.Parse(Appsettings.app(new string[] { "RabbitPort" }))
                        };
                        var connection = factory.CreateConnection();
                        var channel = connection.CreateModel();
                        channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
                        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        string PostData = JsonConvert.SerializeObject(new
                        {
                            type = "event",
                            status = "recover",
                            data = new
                            {
                                type = "bpm",
                                push = "app",
                                sourceid = taskId,
                                users = new string[] { proraterId },
                                title = "巡检任务",
                                content = taskName,
                                begin = 10,
                                recovery = Convert.ToInt64(ts.TotalSeconds)
                            }
                        });
                        var sendBytes = Encoding.UTF8.GetBytes(PostData);
                        channel.BasicPublish(ExchangeName, RoteKey, null, sendBytes);
                        #endregion
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }

        }

        public MessageEntity Delete(string taskId)
        {
            Ins_Task TaskInfo = new Ins_Task();

            string updateTask = string.Format(@" delete ins_task  where TaskId in ('{0}') ", taskId);
            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;

                try
                {
                    string IsEnableRabbit = Appsettings.app(new string[] { "IsEnableRabbit" });
                    if (IsEnableRabbit == "True")
                    {
                        #region 推送 任務完成時  恢復消息提醒 
                        string GetTaskSql = string.Format(@"SELECT 
                                                        	TASKID,
                                                        	TASKNAME,
                                                        	PRORATERDEPTNAME,
                                                        	PRORATERDEPTID,
                                                        	PRORATERID,
                                                        	PRORATERNAME,
                                                        	VISITSTARTIME,
                                                        	VISITOVERTIME,
                                                        	FREQUENCY,
                                                        	DESCRIPT,
                                                        	OPERATOR,
                                                        	OPERATEDATE,
                                                        	TASKSTATE,
                                                        	PLAN_ID,
                                                        	PLAN_NAME,
                                                        	REMARK,
                                                        	ISFINISH,
                                                        	ASSIGNSTATE 
                                                        FROM
                                                        	INS_TASK WHERE TASKID = '{0}' ", taskId);
                        TaskInfo = conn.Query<Ins_Task>(GetTaskSql).FirstOrDefault();
                        rows = conn.Execute(updateTask);
                        string ExchangeName = Appsettings.app(new string[] { "RabbitExchangeName" });
                        string RoteKey = Appsettings.app(new string[] { "RabbitRoteKsy" });

                        RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory
                        {
                            UserName = Appsettings.app(new string[] { "RabbitUserName" }),
                            Password = Appsettings.app(new string[] { "RabbitPassword" }),
                            HostName = Appsettings.app(new string[] { "RabbitUrl" }),
                            Port = int.Parse(Appsettings.app(new string[] { "RabbitPort" }))
                        };
                        var connection = factory.CreateConnection();
                        var channel = connection.CreateModel();
                        channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
                        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        string PostData = JsonConvert.SerializeObject(new
                        {
                            type = "event",
                            status = "recover",
                            data = new
                            {
                                type = "bpm",
                                push = "app",
                                sourceid = taskId,
                                users = new string[] { TaskInfo.ProraterId },
                                title = "巡检任务",
                                content = TaskInfo.TaskName,
                                begin = 10,
                                recovery = Convert.ToInt64(ts.TotalSeconds)
                            }
                        });
                        var sendBytes = Encoding.UTF8.GetBytes(PostData);
                        channel.BasicPublish(ExchangeName, RoteKey, null, sendBytes);

                        #endregion
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
            }
        }
        public MessageEntity Cancel(string taskId)
        {
            IList<Ins_Task> TaskInfo = new List<Ins_Task>();

            string updateTask = string.Format(@" update ins_task  set TaskState = 0 where TaskId in ({0}) ", taskId);
            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;

                try
                {
                    rows = conn.Execute(updateTask);
                    string IsEnableRabbit = Appsettings.app(new string[] { "IsEnableRabbit" });
                    if (IsEnableRabbit == "True")
                    {
                        LogHelper.Info("作废任务id" + taskId );
                        #region 推送 任務完成時  恢復消息提醒 
                        string GetTaskSql = string.Format(@"SELECT 
                                                        	TASKID,
                                                        	TASKNAME,
                                                        	PRORATERDEPTNAME,
                                                        	PRORATERDEPTID,
                                                        	PRORATERID,
                                                        	PRORATERNAME,
                                                        	VISITSTARTIME,
                                                        	VISITOVERTIME,
                                                        	FREQUENCY,
                                                        	DESCRIPT,
                                                        	OPERATOR,
                                                        	OPERATEDATE,
                                                        	TASKSTATE,
                                                        	PLAN_ID,
                                                        	PLAN_NAME,
                                                        	REMARK,
                                                        	ISFINISH,
                                                        	ASSIGNSTATE 
                                                        FROM
                                                        	INS_TASK WHERE TASKID in ({0}) ", taskId);
                        TaskInfo = conn.Query<Ins_Task>(GetTaskSql).ToList();
                        foreach (var item in TaskInfo)
                        {
                            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            string PostData = JsonConvert.SerializeObject(new
                            {
                                type = "event",
                                status = "recover",
                                data = new
                                {
                                    type = "bpm",
                                    push = "app",
                                    sourceid = item.TaskId,
                                    users = new string[] { item.ProraterId },
                                    title = "巡检任务",
                                    content = item.TaskName,
                                    begin = 10,
                                    recovery = Convert.ToInt64(ts.TotalSeconds)
                                }
                            });

                            RabbitMessage(PostData);
                        }
                        //string ExchangeName = Appsettings.app(new string[] { "RabbitExchangeName" });
                        //string RoteKey = Appsettings.app(new string[] { "RabbitRoteKsy" });

                        //RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory
                        //{
                        //    UserName = Appsettings.app(new string[] { "RabbitUserName" }),
                        //    Password = Appsettings.app(new string[] { "RabbitPassword" }),
                        //    HostName = Appsettings.app(new string[] { "RabbitUrl" }),
                        //    Port = int.Parse(Appsettings.app(new string[] { "RabbitPort" }))
                        //};
                        //var connection = factory.CreateConnection();
                        //var channel = connection.CreateModel();
                        //channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
                        //TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        //string PostData = JsonConvert.SerializeObject(new
                        //{
                        //    type = "event",
                        //    status = "recover",
                        //    data = new
                        //    {
                        //        type = "bpm",
                        //        push = "app",
                        //        sourceid = taskId,
                        //        users = new string[] { TaskInfo.ProraterId },
                        //        title = "巡检任务",
                        //        content = TaskInfo.TaskName,
                        //        begin = 10,
                        //        recovery = Convert.ToInt64(ts.TotalSeconds)
                        //    }
                        //});
                        //var sendBytes = Encoding.UTF8.GetBytes(PostData);
                        //channel.BasicPublish(ExchangeName, RoteKey, null, sendBytes);
                       // LogHelper.Info("作废任务：" + PostData + "作废成功");

                        #endregion
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
            }
        }

        public MessageEntity AssignTask(string taskId)
        {
            //
            IList<Ins_Task> TaskInfo = new List<Ins_Task>();
            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                
                #region MyRegion
                var rows = 0;
                string updateTask = string.Format(@" update ins_task  set AssignState =1 where TaskId in ({0}) ", taskId);
                try
                {
                    rows = conn.Execute(updateTask);
                    string IsEnableRabbit = Appsettings.app(new string[] { "IsEnableRabbit" });
                    if (IsEnableRabbit == "True")
                    {
                        #region 推送
                        string GetTaskSql = string.Format(@"SELECT 
                                                        	TASKID,
                                                        	TASKNAME,
                                                        	PRORATERDEPTNAME,
                                                        	PRORATERDEPTID,
                                                        	PRORATERID,
                                                        	PRORATERNAME,
                                                        	VISITSTARTIME,
                                                        	VISITOVERTIME,
                                                        	FREQUENCY,
                                                        	DESCRIPT,
                                                        	OPERATOR,
                                                        	OPERATEDATE,
                                                        	TASKSTATE,
                                                        	PLAN_ID,
                                                        	PLAN_NAME,
                                                        	REMARK,
                                                        	ISFINISH,
                                                        	ASSIGNSTATE 
                                                        FROM
                                                        	INS_TASK WHERE TASKID in ({0}) ", taskId);
                        TaskInfo = conn.Query<Ins_Task>(GetTaskSql).ToList();
                        foreach (var item in TaskInfo)
                        {
                            string ExchangeName = Appsettings.app(new string[] { "RabbitExchangeName" });
                            string RoteKey = Appsettings.app(new string[] { "RabbitRoteKsy" });

                            RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory
                            {
                                UserName = Appsettings.app(new string[] { "RabbitUserName" }),
                                Password = Appsettings.app(new string[] { "RabbitPassword" }),
                                HostName = Appsettings.app(new string[] { "RabbitUrl" }),
                                Port = int.Parse(Appsettings.app(new string[] { "RabbitPort" }))
                            };
                            var connection = factory.CreateConnection();
                            var channel = connection.CreateModel();
                            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
                            string PostData = JsonConvert.SerializeObject(new
                            {
                                type = "event",
                                status = "insert",
                                data = new
                                {
                                    type = "bpm",
                                    push = "app",
                                    sourceid = item.TaskId,
                                    users = new string[] { item.ProraterId },
                                    title = "巡检任务",
                                    content = item.TaskName,
                                    begin = 10
                                }
                            });
                            var sendBytes = Encoding.UTF8.GetBytes(PostData);
                            channel.BasicPublish(ExchangeName, RoteKey, null, sendBytes);
                        }
                        #endregion
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                #endregion
            }
        }

        public string SHA256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }

        public DataTable IsAssignTask(string plan_id)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(plan_id.ToString()))
            {
                strWhere += $" and plan_id = '{plan_id}' ";
            }
            string query = $@" select AssignState from  ins_task where 1=1 {strWhere}";
            try
            {
                using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query));

                    return dt;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }
        public DataTable IsAssign(string taskId)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(taskId.ToString()))
            {
                strWhere += $" and taskId = '{taskId}' ";
            }
            string query = $@" select AssignState from  ins_task where 1=1 {strWhere}";
            try
            {
                using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query));

                    return dt;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }

        public MessageEntity ReAssignTask(string taskIds, string proraterDeptName, string proraterDeptId, string proraterName, string proraterId)
        {
            Ins_Task TaskInfo = new Ins_Task();

            using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
            {
                string IsEnableRabbit = Appsettings.app(new string[] { "IsEnableRabbit" });
                if (IsEnableRabbit == "True")
                {
                    #region 推送
                    string GetTaskSql = string.Format(@"SELECT 
                                                        	TASKID,
                                                        	TASKNAME,
                                                        	PRORATERDEPTNAME,
                                                        	PRORATERDEPTID,
                                                        	PRORATERID,
                                                        	PRORATERNAME,
                                                        	VISITSTARTIME,
                                                        	VISITOVERTIME,
                                                        	FREQUENCY,
                                                        	DESCRIPT,
                                                        	OPERATOR,
                                                        	OPERATEDATE,
                                                        	TASKSTATE,
                                                        	PLAN_ID,
                                                        	PLAN_NAME,
                                                        	REMARK,
                                                        	ISFINISH,
                                                        	ASSIGNSTATE 
                                                        FROM
                                                        	INS_TASK WHERE TASKID = '{0}' ", taskIds);
                    TaskInfo = conn.Query<Ins_Task>(GetTaskSql).FirstOrDefault();
                    string ExchangeName = Appsettings.app(new string[] { "RabbitExchangeName" });
                    string RoteKey = Appsettings.app(new string[] { "RabbitRoteKsy" });

                    RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory
                    {
                        UserName = Appsettings.app(new string[] { "RabbitUserName" }),
                        Password = Appsettings.app(new string[] { "RabbitPassword" }),
                        HostName = Appsettings.app(new string[] { "RabbitUrl" }),
                        Port = int.Parse(Appsettings.app(new string[] { "RabbitPort" }))
                    };
                    var connection = factory.CreateConnection();
                    var channel = connection.CreateModel();
                    channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
                    string PostData = JsonConvert.SerializeObject(new
                    {
                        type = "event",
                        status = "insert",
                        data = new
                        {
                            type = "bpm",
                            push = "app",
                            sourceid = taskIds,
                            users = new string[] { proraterId },
                            title = "巡检任务",
                            content = TaskInfo.TaskName,
                            begin = 10
                        }
                    });
                    var sendBytes = Encoding.UTF8.GetBytes(PostData);
                    channel.BasicPublish(ExchangeName, RoteKey, null, sendBytes);
                    #endregion
                }
                var rows = 0;
                string updateTask = $@" update ins_task  set AssignState = 1,ProraterId='{proraterId}',ProraterName='{proraterName}',ProraterDeptId='{proraterDeptId}',ProraterDeptName='{proraterDeptName}' where TaskId in ('{taskIds}') ";
                try
                {
                    rows = conn.Execute(updateTask);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void RabbitMessage(string Message)
        {
            LogHelper.Debug("进入RabbitMQ消息推送，时间:" + DateTime.Now);
            string ExchangeName = Appsettings.app(new string[] { "RabbitExchangeName" });
            string RoteKey = Appsettings.app(new string[] { "RabbitRoteKsy" });
            RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory
            {
                UserName = Appsettings.app(new string[] { "RabbitUserName" }),
                Password = Appsettings.app(new string[] { "RabbitPassword" }),
                HostName = Appsettings.app(new string[] { "RabbitUrl" }),
                Port = int.Parse(Appsettings.app(new string[] { "RabbitPort" }))
            };
            LogHelper.Debug("RabbitMQ消息推送，配置参数:ExchangeName:" + ExchangeName + ";RoteKey:" + RoteKey + ";UserName:" + Appsettings.app(new string[] { "RabbitUserName" })
                + ";Password:" + Appsettings.app(new string[] { "RabbitPassword" }) + ";HostName:" + Appsettings.app(new string[] { "RabbitUrl" })
                + ";Port:" + Appsettings.app(new string[] { "RabbitPort" }));
            var connection = factory.CreateConnection();
            LogHelper.Debug("开启RabbitMQ连接，时间:" + DateTime.Now);
            var channel = connection.CreateModel();
            LogHelper.Debug("开启RabbitMQ信道，时间:" + DateTime.Now);
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
            LogHelper.Debug("绑定RabbitMQ交换机，时间:" + DateTime.Now);
            channel.ConfirmSelect();
            LogHelper.Debug("开启RabbitMQ消息确认模式，时间:" + DateTime.Now);
            var sendBytes = Encoding.UTF8.GetBytes(Message);
            LogHelper.Debug("进行RabbitMQ消息格式装换，时间:" + DateTime.Now + "消息内容：" + Message);
            EventHandler<RabbitMQ.Client.Events.BasicReturnEventArgs> evreturn = new EventHandler<RabbitMQ.Client.Events.BasicReturnEventArgs>((o, basic) =>
            {
                //Console.WriteLine(basic.ReplyCode); //消息失败的code
                //Console.WriteLine(basic.ReplyText); //描述返回原因的文本
                LogHelper.Info("转派任务推送失败ReplyCode：" + basic.ReplyCode + "返回原因的文本：" + basic.ReplyText + "推送数据：" + Message);
            });
            channel.BasicReturn += evreturn;
            LogHelper.Debug("绑定RabbitMQ消息队列匹配回调，时间:" + DateTime.Now);
            var outstandingConfirms = new ConcurrentDictionary<ulong, string>();
            void cleanOutstandingConfirms(ulong sequenceNumber, bool multiple)
            {
                if (multiple)
                {
                    var confirmed = outstandingConfirms.Where(k => k.Key <= sequenceNumber);
                    foreach (var entry in confirmed)
                        outstandingConfirms.TryRemove(entry.Key, out _);
                }
                else
                    outstandingConfirms.TryRemove(sequenceNumber, out _);
            }
            channel.BasicAcks += (sender, ea) =>
            {
                cleanOutstandingConfirms(ea.DeliveryTag, ea.Multiple);
                LogHelper.Debug($"BasicAcks,DeliveryTag: {ea.DeliveryTag}, multiple: {ea.Multiple}");
            };
            channel.BasicNacks += (sender, ea) =>
            {
                outstandingConfirms.TryGetValue(ea.DeliveryTag, out string body);
                LogHelper.Debug($"BasicNacks,DeliveryTag: {ea.DeliveryTag}, multiple: {ea.Multiple}");
                cleanOutstandingConfirms(ea.DeliveryTag, ea.Multiple);
            };
            outstandingConfirms.TryAdd(channel.NextPublishSeqNo, "0");
            channel.BasicPublish(ExchangeName, RoteKey, true, null, sendBytes);
            LogHelper.Debug("推送RabbitMQ消息，时间:" + DateTime.Now);
            var isOk = channel.WaitForConfirms();
            LogHelper.Debug("确认RabbitMQ交换机匹配状态，时间:" + DateTime.Now + ";结果:" + isOk);
            if (!WaitUntil(60, () => outstandingConfirms.IsEmpty))
                LogHelper.Debug("未在60秒内对所有推送消息进行发送确认");
            channel.Dispose();
            LogHelper.Debug("方法执行完毕，时间:" + DateTime.Now);
        }
        private static bool WaitUntil(int numberOfSeconds, Func<bool> condition)
        {
            int waited = 0;
            while (!condition() && waited < numberOfSeconds * 1000)
            {
                Thread.Sleep(100);
                waited += 100;
            }

            return condition();
        }
    }
}
