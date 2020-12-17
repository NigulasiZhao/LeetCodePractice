using Dapper;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Database;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.ResultDto;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GisPlateformForCore.OracleDAL.PlanningEngine.InspectionSettings
{
    public class Ins_EventDAL :IIns_EventDAL
    {
        /// <summary>
        ///  事件工单查询和(个人处理-待办处理)查询
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="EventFromId"></param>
        /// <param name="eventType"></param>
        /// <param name="EventID"></param>
        /// <param name="OperId"></param>
        /// <param name="IsValid"></param>
        /// <param name="DeptId"></param>
        /// <param name="EventContenct"></param>
        /// <param name="ExecPersonId"></param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public MessageEntity GetEventWorkorderListForMaintain(DateTime? startTime, DateTime? endTime, string? rangName, int? EventFromId, string? eventType, string? EventID, string OperId, int? IsValid, string? DeptId, string EventContenct, string ExecPersonId, string X, string Y, string sort, string ordering, int num, int page)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "EventID";
            }

            if (string.IsNullOrEmpty(ordering))
            {
                ordering = "desc";
            }

            string sql = string.Format(@"select * from (select a.eventid, a.uptime,a.eventupdatetime,a.eventfromid,c.eventfromname,a.eventcode,a.exectime,
                                A.UrgencyId,d.urgencyname,a.isvalid,J.ISVALIDNAME
                                ,A.EventTypeId, A.EventTypeId2,B.EventTypeName, B1.EventTypeName AS EventTypeName2,
                                a.updeptid,a.updeptname,a.uppersonid,a.upname,a.linkman,a.linkcall,a.eventaddress,a.eventx,a.eventy,a.eventdesc,a.eventpictures,a.eventaudio,a.eventvideo,a.RangName,
                                --步骤处理人 处理部门
                                hh.execpersonid,hh.execpersonname,hh.execdetpid,hh.execdetptname,hh.OperId,hh.opername,hh.opername2
                               
FROM           ins_event  A LEFT OUTER JOIN
               ins_event_type  B ON A.Eventtypeid = B.Event_Type_Id LEFT OUTER JOIN
               ins_event_type  B1 ON A.EventTypeId2 = B1.Event_Type_Id LEFT OUTER JOIN
               ins_eventfrom  C ON A.EventFromId = C.EventFromId LEFT OUTER JOIN
               ins_urgent_level  D ON A.UrgencyId = D.URGENT_LEVEL_ID LEFT OUTER JOIN
               ins_isvalidstatus  J ON A.IsValid = J.IsValidID LEFT OUTER JOIN
             (   select h.eventid, h1.execpersonid,h1.execpersonname,h1.execdetpid,h1.execdetptname,h1.OperId,o.opername,o.opername2 from 
               (SELECT MAX(opertime) AS opertime,h.EventID
                                 FROM ins_workorder_oper_history h  GROUP BY h.EventID)  h LEFT OUTER JOIN ins_workorder_oper_history h1 ON h.EventID = h1.EventID AND h.opertime =h1.opertime 
                left outer join ins_workorder_oper o on h1.operid=o.operid) HH on hh.eventid=a.eventid where a.DeleteStatus=0) v
        ");
            #region 条件
            sql += " where 1=1 ";
            if (startTime != null)
            {
                sql += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sql +=$" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (rangName != null && rangName != "")
            {
                sql += $" and rangName ='{rangName}'";
            }
            if (DeptId != null)//部门id
            {
                sql += " and updeptid='" + DeptId + "'";
            }
            if (eventType != null)//事件类型
            {
                sql += " and EventTypeId='" + eventType + "' ";
            }
            if (!string.IsNullOrEmpty(EventContenct))//事件查找  编号 上报人  类型
            {
                sql += " and (EventCode like '%" + EventContenct + "%'or UpName like '%" + EventContenct + "%' or EventFromName like '%" + EventContenct + "%' )";
            }
            if (EventFromId != null)//事件来源
            {
                sql += " and EventFromId=" + EventFromId;
            }
            if (OperId != null && OperId != "")//步骤id  事件处理状态
            {
                if (OperId == "1")
                {
                    sql += " and OperId is null";
                }
                else
                {
                    sql += " and OperId in(" + OperId + ") and IsValid<>0";
                }
            }
            if (IsValid != null)//是否有效
            {
                if (IsValid == 1)
                {
                    sql += " and IsValid>'0'";
                }
                else
                {
                    sql += " and IsValid='" + IsValid + "'";
                }

            }
            if (EventID != null)//事件id
            {
                sql += " and EventID='" + EventID + "'";
            }
            if (ExecPersonId != null)
            {

                sql += " and ExecPersonId='" + ExecPersonId +  "' and OperId not in(7,8)";
            }
            #endregion

            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_EventList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
          //app调用，需要查询出当前距离设备多远
            if(X!=null && Y != null)
            {
                foreach (Ins_EventList row in ResultList)
                {
                    if (!string.IsNullOrEmpty(row.EventX) && !string.IsNullOrEmpty(row.EventY))
                    {
                        using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE))
                        {
                            try
                            {
                                string distancesql = $@"  select trunc(sde.st_distance(sde.st_geometry('POINT ({X} {Y})', 4547),
                    sde.st_geometry('POINT ({row.EventX} {row.EventY})',4547) , 'kilometer'),2) as distance
                                    from dual";
                                DataTable distancedt = new DataTable();
                                distancedt.Load(conn.ExecuteReader(distancesql));
                                if (distancedt != null && distancedt.Rows.Count > 0)
                                {
                                    row.Distance = decimal.Parse(distancedt.Rows[0][0].ToString());
                                }
                            }
                            catch (Exception e)
                            {
                                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                            }
                        }
                    }
                      
                }
            }
            return result;
        }
        public MessageEntity GetEventWorkorderStepForMaintain(string eventID)
        {
            string sqlwhere = "";
            if (eventID != null)//事件id
            {
                sqlwhere += " and  A.EventID='" + eventID + "'";

            }
            string sql = $@"select a.eventid, a.eventcode,a.eventaddress,a.uptime,c.eventfromname,a.upname,a.updeptname,B.EventTypeName, B1.EventTypeName AS EventTypeName2,
                               d.urgencyname,a.exectime,a.linkman,a.linkcall,a.execdetptname as detptname ,a.execpersonname as personname,a.eventdesc,a.eventx,a.eventy,a.PreEndTime,
                                a.eventpictures,a.eventvideo,a.eventaudio,hh.IsValid, j.IsValidName,
                                --转理信息
                                hh.dispatchpersonid, hh.dispatchpersonname,hh.DISPATCHDETPID,hh.DISPATCHDETPTNAME ,hh.opertime,hh.pictures,
                                --步骤处理人 处理部门
                                hh.execpersonid,hh.execpersonname,hh.execdetpid,hh.execdetptname,hh.OperId,o.opername,o.opername2,hh.operremarks,hh.satisfaction,po.PostponeTime,po.Cause
                               
FROM           ins_event  A LEFT OUTER JOIN
               ins_workorder_oper_history hh on a.eventid=hh.eventid   left outer join
               ins_workorder_oper o on hh.operid=o.operid   left outer join
               ins_event_type  B ON A.Eventtypeid = B.Event_Type_Id LEFT OUTER JOIN
               ins_event_type  B1 ON A.EventTypeId2 = B1.Event_Type_Id LEFT OUTER JOIN
               ins_eventfrom  C ON A.EventFromId = C.EventFromId LEFT OUTER JOIN
               ins_urgent_level  D ON A.UrgencyId = D.URGENT_LEVEL_ID LEFT OUTER JOIN
               ins_isvalidstatus  J ON hh.IsValid = J.IsValidID LEFT OUTER JOIN
               Ins_PostponeOrder po ON po.historyid=hh.historyid where 1=1  {sqlwhere} order by hh.opertime asc
             ";
           
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_EventStepDto> result = conn.Query<Ins_EventStepDto>(sql).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }


        /// <summary>
        /// 个人处理-已办处理
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="OwnID">当前登陆人员ID</param>
        /// <param name="sort">排序字段默认EventID</param>
        /// <param name="ordering">desc/asc</param>
        /// <param name="num">默认20</param>
        /// <param name="page">默认1</param>
        /// <returns></returns>
        public MessageEntity GetEventListOwn(string OwnID, string? eventType, string? operId, DateTime? startTime, DateTime? endTime, string sort, string ordering, int num, int page)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "EventID";
            }

            if (string.IsNullOrEmpty(ordering))
            {
                ordering = "desc";
            }
      

            string sql = string.Format($@"select * from (select a.eventid, a.uptime,a.eventfromid,c.eventfromname,a.eventcode,a.exectime,a.eventupdatetime,
                                A.UrgencyId,d.urgencyname,a.isvalid,J.ISVALIDNAME
                                ,A.EventTypeId, A.EventTypeId2,B.EventTypeName, B1.EventTypeName AS EventTypeName2,
                                a.updeptid,a.updeptname,a.uppersonid,a.upname,a.linkman,a.linkcall,a.eventaddress,a.eventx,a.eventy,a.eventdesc,a.eventpictures,a.eventaudio,a.eventvideo,
                                --步骤处理人 处理部门
                                hh.execpersonid,hh.execpersonname,hh.execdetpid,hh.execdetptname,hh.OperId,hh.opername,hh.opername2
                               
FROM           ins_event  A LEFT OUTER JOIN
               ins_event_type  B ON A.Eventtypeid = B.Event_Type_Id LEFT OUTER JOIN
               ins_event_type  B1 ON A.EventTypeId2 = B1.Event_Type_Id LEFT OUTER JOIN
               ins_eventfrom  C ON A.EventFromId = C.EventFromId LEFT OUTER JOIN
               ins_urgent_level  D ON A.UrgencyId = D.URGENT_LEVEL_ID LEFT OUTER JOIN
               ins_isvalidstatus  J ON A.IsValid = J.IsValidID LEFT OUTER JOIN
             (   select h.eventid, h1.execpersonid,h1.execpersonname,h1.execdetpid,h1.execdetptname,h1.OperId,o.opername,o.opername2 from 
               (SELECT MAX(opertime) AS opertime,h.EventID
                                 FROM ins_workorder_oper_history h  GROUP BY h.EventID)  h LEFT OUTER JOIN ins_workorder_oper_history h1 ON h.EventID = h1.EventID AND h.opertime =h1.opertime 
                left outer join ins_workorder_oper o on h1.operid=o.operid) HH on hh.eventid=a.eventid
 
           where a.DeleteStatus=0) v
         join (SELECT EventID as ID FROM ins_workorder_oper_history WHERE  (DispatchPersonID ='{OwnID}' OR ExecPersonId ='{OwnID}') and EventID IS NOT NULL GROUP BY EventID) AA   ON AA.ID = v.EventID
        ");
            #region 条件
            sql += " where 1=1 ";
          
            if (eventType != null)//事件类型
            {
                sql += " and EventTypeId='" + eventType + "' ";
            }
            if (operId != null && operId != "")//步骤id  事件处理状态
            {
                if (operId == "1")
                {
                    sql += " and operId is null";
                }
                else
                {
                    sql += " and operId in(" + operId + ") and IsValid<>0";
                }
            }
            if (startTime != null)
            {
                sql += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sql += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            #endregion

            DapperExtentions.EntityForSqlToPager<Ins_EventList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);

            return result;
        }

        public MessageEntity PostEvent(string plan_Task_Id, Ins_Event m_Event, Ins_WorkOrder_Oper_History workOrder_Oper_History)
        {
            //string eventID = Guid.NewGuid().ToString();
            string historyID = Guid.NewGuid().ToString();

            string m_eventInsertSQL = $@"INSERT INTO INS_Event
           (eventID,EventCode,
            EventAddress,
            UpTime,
            UppersonId,
            UpName,
            UpDeptId,
           UpDeptName,
            EventTypeId,
            EventTypeId2,
            EventFromId,
            UrgencyId,
            HandlerLevelId,
            EventDesc,
            EventX,
            EventY,
            EventUpdateTime,
            IsValid,
            DeleteStatus,
            ExecTime,
            LinkMan,
            LinkCall,RangName,plan_Task_Id,
            EventPictures,EventAudio,EventVideo)
     VALUES
           ('{m_Event.EventID}',(SELECT '{m_Event.EventCode}'  ||  to_char(sysdate,'yyyymmdd')|| nvl(replace(lPAD( MAX( substr(eventcode,11,3)+ 1),3), ' ','0'),'001') from ins_event where  UpTime>= to_date( to_char(sysdate, 'YYYY-MM-DD'), 'YYYY-MM-DD'))
           ,'{m_Event.EventAddress}'
           ,to_date('{m_Event.UpTime}', 'YYYY-mm-dd HH24:Mi:SS')
           ,'{m_Event.UppersonId}'
           ,'{m_Event.UpName}'
           ,'{m_Event.UpDeptId}'
            ,'{m_Event.UpDeptName}'
           ,'{m_Event.EventTypeId}'
           ,'{m_Event.EventTypeId2}'
           ,{m_Event.EventFromId}
           ,{m_Event.UrgencyId}
           ,{m_Event.HandlerLevelId}
           ,'{m_Event.EventDesc}'
           ,'{m_Event.EventX}'
           ,'{m_Event.EventY}'
           ,to_date('{m_Event.EventUpdateTime}', 'YYYY-mm-dd HH24:Mi:SS')
           ,{m_Event.IsValid}
           ,'{m_Event.DeleteStatus}'
           ,{m_Event.ExecTime}
           ,'{m_Event.LinkMan}'
           ,'{m_Event.LinkCall}'
           ,'{m_Event.RangName}'
           ,'{m_Event.Plan_task_id}'
           ,'{m_Event.EventPictures}','{m_Event.EventAudio}','{m_Event.EventVideo}')";


            string workOrder_Oper_HistoryInsertSQL = $@"INSERT INTO INS_WorkOrder_Oper_History
           (historyId,EventID,
            OperId,
            OperTime,
            ExecPersonId,execPersonName,
            DispatchPersonID,dispatchPersonName,DispatchDetpID,DispatchDetptName,
            ExecDetpID,execDetptName)
     VALUES
           ('{historyID}','{m_Event.EventID}'
           ,11
           ,to_date('{workOrder_Oper_History.OperTime}', 'YYYY-mm-dd HH24:Mi:SS')
           ,'{workOrder_Oper_History.ExecPersonId}'
           ,'{workOrder_Oper_History.ExecPersonName}'
           ,'{workOrder_Oper_History.DispatchPersonID}'
           ,'{workOrder_Oper_History.DispatchPersonName}'
           ,'{workOrder_Oper_History.DispatchDetpID}'
           ,'{workOrder_Oper_History.DispatchDetptName}'
           ,'{workOrder_Oper_History.ExecDetpID}'
           ,'{workOrder_Oper_History.ExecDetptName}'

           )";
         
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {

                    int rows = 0;
                    try
                    {
                        rows = conn.Execute(m_eventInsertSQL);
                        rows = conn.Execute(workOrder_Oper_HistoryInsertSQL);
                        if (plan_Task_Id != null && plan_Task_Id !="")
                        {
                           var  updateplantask = $@"update  INS_plan_task set IsReport=1 where plan_task_id='{plan_Task_Id}'";
                            rows = conn.Execute(updateplantask);
                        }
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(rows);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
                }
        }
        /// <summary>
        ///  事件工单流转操作（分派工单2)
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="execTime">处理时间（单位：小时)</param>
        /// <returns></returns>
        public MessageEntity WorkListAssign(string eventID,  string execTime, Ins_WorkOrder_Oper_History model)
        {
            var updateSql = $"update  INS_Event set IsValid=1,EventUpdateTime=sysdate,dispatchPersonId='{ model.DispatchPersonID}',dispatchPersonName='{model.DispatchPersonName}',execPersonId='{model.ExecPersonId}',execPersonName='{model.ExecPersonName}',ExecDetpID='{model.ExecDetpID}',execDetptName='{model.ExecDetptName}',orderTime=sysdate,preEndTime=to_date('{DateTime.Now.AddHours(Convert.ToInt32(execTime)).ToString()}', 'YYYY-mm-dd HH24:Mi:SS') where EventID='{eventID}'";
            //插入分派工单记录
            var rows = 0;
            var insertSql = DapperExtentions.MakeInsertSql(model);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateSql, transaction);
                        //插入份派工单记录
                        rows = conn.Execute(insertSql, model);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 是否执行过分派2 接单3 到场4 处置5 完工6 审核完成7 8
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool IsExecute(string eventID, string StepNum, out string errorMsg)
        {
            errorMsg = "";
            string sql = $@" select OperId,rn from( select  OperId ,row_number() over ( partition by eventid order by vieworder desc) rn from INS_WorkOrder_Oper_History  where EventID='{eventID}' ) where rn=1
";

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {

                    List<dynamic> list = conn.Query<dynamic>(sql).ToList();

                    if (list.Count > 0 && list[0].OperId == Int32.Parse(StepNum))
                    {
                        return true;
                    }

                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {

                    errorMsg = e.Message;
                    return false;
                }
            }
        }

        /// <summary>
        /// 是否执行过 退单4  延期 5  延期确认6 延期确认退回7 
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool IsValid(string eventID, string isValid, out string errorMsg)
        {
            errorMsg = "";
            string sql = $@" select IsValid from( select  IsValid ,row_number() over ( partition by eventid order by vieworder desc) rn from INS_WorkOrder_Oper_History  where EventID='{eventID}' ) where rn=1";

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {

                    List<dynamic> list = conn.Query<dynamic>(sql).ToList();

                    if (list.Count > 0 && list[0].IsValid == Int32.Parse(isValid))
                    {
                        return true;
                    }

                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {

                    errorMsg = e.Message;
                    return false;
                }
            }
        }
        /// <summary>
        ///  事件工单流转操作（接单3)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="workOrder_Oper_History"></param>
        /// <returns></returns>
        public MessageEntity WorkListReceipt(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History)
        {
            var rows = 0;
            var insertSql = DapperExtentions.MakeInsertSql(workOrder_Oper_History);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                    try
                    {
                        //插入
                        rows = conn.Execute(insertSql, workOrder_Oper_History);
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
            }
        }
        /// <summary>
        ///  事件工单流转操作（到场4)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="workOrder_Oper_History"></param>
        /// <returns></returns>
        public MessageEntity WorkListPresent(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History)
        {
            var rows = 0;
            var insertSql = DapperExtentions.MakeInsertSql(workOrder_Oper_History);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    //插入
                    rows = conn.Execute(insertSql, workOrder_Oper_History);
                    return MessageEntityTool.GetMessage(1, null, true);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public DataTable IsExistPostponeOrderByDay(string eventId, string complishTime)
        {
            string errorMsg = "";
            #region 条件
            string strWhere = " ";
            if (!string.IsNullOrEmpty(eventId))
            {
                strWhere += string.Format(@" and eventId = '{0}'", eventId);
            }

            if (!string.IsNullOrEmpty(complishTime))
            {
                DateTime EndDate=DateTime.Parse(complishTime).AddDays(1).AddSeconds(-1);
                strWhere +=$" and PostponeTime >= to_date('{complishTime}', 'yyyy-mm-dd hh24:mi:ss')";
                strWhere += $" and PostponeTime <= to_date('{EndDate}', 'yyyy-mm-dd hh24:mi:ss')";

            }
            #endregion
            //延期申请数据
            string query = string.Format(@"   SELECT eventid  FROM Ins_PostponeOrder  where 1=1 {0}", strWhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query)); //此类别下时间段内隐患名称出现的个数

                    return dt;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }
        /// <summary>
        /// 事件工单流转操作（延期)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="operRemarks"></param>
        /// <param name="complishTime"></param>
        /// <param name="iAdminID"></param>
        /// <param name="iAdminName"></param>
        /// <returns></returns>
        public MessageEntity WordListDelay(string eventID, string operRemarks, string complishTime, string dispatchPersonID, string dispatchPersonName, string dispatchDetpID, string dispatchDetptName)
        {
            string ID = Guid.NewGuid().ToString();
            string historyID = Guid.NewGuid().ToString();
            string postponeID = Guid.NewGuid().ToString();
            string query1 = $"select OperId,rn from( select  OperId ,row_number() over ( partition by eventid order by ExecUpDateTime desc) rn from INS_WorkOrder_Oper_History  where EventID='{eventID}' ) where rn=1";
            string query = $"select  ExecPersonId,execpersonname,ExecDetpID,ExecDetptName from( select   ExecPersonId,execpersonname,ExecDetpID,ExecDetptName  ,row_number() over ( partition by eventid order by ExecUpDateTime desc) rn from INS_WorkOrder_Oper_History  where EventID='{eventID}'and operid=11 ) where rn=1";
            var updateeventSql = $"update INS_Event Set IsValid=5,Remark_Back = '{operRemarks}' where EventID='{eventID}'";
             string sql = $@"Insert INTO Ins_PostponeOrder(postponeID,EventID, Cause, PostponeTime, ApplicationTime,HistoryId) VALUES('{postponeID}','{eventID}', '{operRemarks.Trim()}', to_date('{complishTime} 23:59:59', 'YYYY-mm-dd HH24:Mi:SS'), sysdate,'{historyID}')";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                DataTable dt = new DataTable();
                dt.Load(conn.ExecuteReader(query));
                DataTable dt1 = new DataTable();
                dt1.Load(conn.ExecuteReader(query1));
                string ExecPersonId="", Execpersonname = "", ExecDetpID = "", ExecDetptName = "";
                int OperId = int.Parse( dt1.Rows[0][0].ToString());
                if (dt!=null && dt.Rows.Count > 0)
                {
                    ExecPersonId = dt.Rows[0]["ExecPersonId"].ToString();
                    Execpersonname = dt.Rows[0]["execpersonname"].ToString();
                    ExecDetpID = dt.Rows[0]["ExecDetpID"].ToString();
                    ExecDetptName = dt.Rows[0]["ExecDetptName"].ToString();

                }
                string insertSql = $@"INSERT INTO INS_WorkOrder_Oper_History (HistoryId,EventID,OperId,OperTime,OperRemarks,DispatchPersonID,DispatchPersonName,dispatchDetpID,dispatchDetptName,ExecPersonId,ExecPersonName,ExecDetpID,ExecDetptName,IsValid )
                                VALUES ('{historyID}', '{eventID}',{OperId},sysdate,'{operRemarks}','{dispatchDetpID}','{dispatchPersonName}','{dispatchDetpID}','{dispatchDetptName}','{ExecPersonId}','{Execpersonname}','{ExecDetpID}','{ExecDetptName}',5 ) ";

                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateeventSql, transaction);
                        var ii = conn.Execute(insertSql, transaction);
                        conn.Execute(sql, transaction);

                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 事件工单流转操作（延期确认)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="complishTime"></param>
        /// <param name="iAdminName"></param>
        /// <param name="iAdminID"></param>
        /// <param name="operRemarks"></param>
        /// <returns></returns>
        public MessageEntity WorkListDelayExec(string eventID, string complishTime, string dispatchPersonID, string dispatchPersonName, string dispatchDetpID, string dispatchDetptName, string operRemarks = "")
        {
            string historyID = Guid.NewGuid().ToString();

            string query = $"select  ExecPersonId,execpersonname,ExecDetpID,ExecDetptName from INS_Event where EventID='{eventID}' ";
            string query1 = $"select OperId,rn from( select  OperId ,row_number() over ( partition by eventid order by ExecUpDateTime desc) rn from INS_WorkOrder_Oper_History  where EventID='{eventID}' ) where rn=1";

            var updateevsql = $"update Ins_PostponeOrder Set AuditStatus=2 where EventID='{eventID}'";
            var updateSql = $"update INS_Event Set IsValid=1,PreEndTime=to_date('{complishTime} 23:59:59', 'YYYY-mm-dd HH24:Mi:SS') where EventID='{eventID}'";

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                DataTable dt = new DataTable();
                dt.Load(conn.ExecuteReader(query));
                DataTable dt1 = new DataTable();
                dt1.Load(conn.ExecuteReader(query1));
                string ExecPersonId = "", Execpersonname = "", ExecDetpID = "", ExecDetptName = "";
                int OperId = int.Parse(dt1.Rows[0][0].ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    ExecPersonId = dt.Rows[0]["ExecPersonId"].ToString();
                    Execpersonname = dt.Rows[0]["execpersonname"].ToString();
                    ExecDetpID = dt.Rows[0]["ExecDetpID"].ToString();
                    ExecDetptName = dt.Rows[0]["ExecDetptName"].ToString();
                }
                    string insertSql = $@"INSERT INTO INS_WorkOrder_Oper_History (HistoryId,EventID,OperId,OperTime,DispatchPersonID,DispatchPersonName,dispatchDetpID,dispatchDetptName,ExecPersonId,ExecPersonName,ExecDetpID,ExecDetptName,IsValid,OperRemarks ) VALUES 
                                                                              ( '{historyID}','{eventID}',{OperId},sysdate,'{dispatchDetpID}','{dispatchPersonName}','{dispatchDetpID}','{dispatchDetptName}','{ExecPersonId}','{Execpersonname}','{ExecDetpID}','{ExecDetptName}',6,'{operRemarks}') ";

                    using (var transaction = conn.BeginTransaction())
                    {

                        try
                        {
                            conn.Execute(updateevsql, transaction);
                            conn.Execute(updateSql, transaction);
                            var ii = conn.Execute(insertSql, transaction);

                            transaction.Commit();
                            return MessageEntityTool.GetMessage(1, null, true);
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                        }
                    }

                
            }
        }

        /// <summary>
        ///  事件工单流转操作（延期确认退回)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="complishTime"></param>
        /// <param name="iAdminName"></param>
        /// <param name="iAdminID"></param>
        /// <param name="OperRemarks"></param>
        /// <returns></returns>
        public MessageEntity WorkListDelayReturn(string eventID, string complishTime, string dispatchPersonID, string dispatchPersonName, string dispatchDetpID, string dispatchDetptName, string OperRemarks = "")
        {
            string historyID = Guid.NewGuid().ToString();

            string query = $"select  ExecPersonId,execpersonname,ExecDetpID,ExecDetptName from INS_Event where EventID='{eventID}' ";
            string query1 = $"select OperId,rn from( select  OperId ,row_number() over ( partition by eventid order by ExecUpDateTime desc) rn from INS_WorkOrder_Oper_History  where EventID='{eventID}' ) where rn=1";

            var updateevsql = $"update Ins_PostponeOrder Set AuditStatus=2 where EventID='{eventID}'";
            var updateSql = $"update INS_Event Set IsValid=7,PreEndTime=to_date('{complishTime} 23:59:59', 'YYYY-mm-dd HH24:Mi:SS') where EventID='{eventID}'";

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                DataTable dt = new DataTable();
                dt.Load(conn.ExecuteReader(query));
                DataTable dt1 = new DataTable();
                dt1.Load(conn.ExecuteReader(query1));
                string ExecPersonId = "", Execpersonname = "", ExecDetpID = "", ExecDetptName = "";
                int OperId = int.Parse(dt1.Rows[0][0].ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    ExecPersonId = dt.Rows[0]["ExecPersonId"].ToString();
                    Execpersonname = dt.Rows[0]["execpersonname"].ToString();
                    ExecDetpID = dt.Rows[0]["ExecDetpID"].ToString();
                    ExecDetptName = dt.Rows[0]["ExecDetptName"].ToString();
                }
                string insertSql = $@"INSERT INTO INS_WorkOrder_Oper_History (HistoryId,EventID,OperId,OperTime,DispatchPersonID,DispatchPersonName,dispatchDetpID,dispatchDetptName,ExecPersonId,ExecPersonName,ExecDetpID,ExecDetptName,IsValid,OperRemarks ) VALUES 
                                                                              ( '{historyID}','{eventID}',{OperId},sysdate,'{dispatchDetpID}','{dispatchPersonName}','{dispatchDetpID}','{dispatchDetptName}','{ExecPersonId}','{Execpersonname}','{ExecDetpID}','{ExecDetptName}',7,'{OperRemarks}') ";

                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateevsql, transaction);
                        conn.Execute(updateSql, transaction);
                        var ii = conn.Execute(insertSql, transaction);

                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }


            }
        }

        /// <summary>
        /// 事件工单流转操作（退单)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="iAdminID"></param>
        /// <param name="iAdminName"></param>
        /// <param name="backDesc"></param>
        /// <returns></returns>
        public MessageEntity WordListBackExec(string eventID, string iAdminID, string iAdminName, string detpID, string detpName, string backDesc)
        {
            
            string historyID = Guid.NewGuid().ToString();
            string backId = Guid.NewGuid().ToString();

            string query = $"select  ExecPersonId,execpersonname,ExecDetpID,ExecDetptName from( select   ExecPersonId,execpersonname,ExecDetpID,ExecDetptName  ,row_number() over ( partition by eventid order by ExecUpDateTime desc) rn from INS_WorkOrder_Oper_History  where EventID='{eventID}'and operid=11 ) where rn=1";
            
            var updateSql = $"update INS_Event Set  IsValid=4, OrderStatus=1,Remark_Back ='{backDesc}' where EventID='{eventID}'";
            string insertBackSql = $@"insert into INS_M_WorkOrder_Back (backId,DeptId,deptName,PersonId,personName,eventId,BackTime,BackRemarks)
                                             values ('{backId}','{detpID}','{detpName}','{iAdminID}','{iAdminName}','{eventID}',sysdate,'{backDesc}') ";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                DataTable dt = new DataTable();
                dt.Load(conn.ExecuteReader(query));
                string ExecPersonId = "", Execpersonname = "", ExecDetpID = "", ExecDetptName = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    ExecPersonId = dt.Rows[0]["ExecPersonId"].ToString();
                    Execpersonname = dt.Rows[0]["execpersonname"].ToString();
                    ExecDetpID = dt.Rows[0]["ExecDetpID"].ToString();
                    ExecDetptName = dt.Rows[0]["ExecDetptName"].ToString();
                }
                string insertSql = $@"INSERT INTO INS_WorkOrder_Oper_History (HistoryId,EventID,OperId,OperTime,DispatchPersonID,DispatchPersonName,dispatchDetpID,dispatchDetptName,ExecPersonId,ExecPersonName,ExecDetpID,ExecDetptName,IsValid,OperRemarks ) VALUES 
                                                                              ( '{historyID}','{eventID}',11,sysdate,'{iAdminID}','{iAdminName}','{detpID}','{detpName}','{ExecPersonId}','{Execpersonname}','{ExecDetpID}','{ExecDetptName}',4,'{backDesc}') ";

                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(insertBackSql, transaction);
                        conn.Execute(updateSql, transaction);
                        var ii = conn.Execute(insertSql, transaction);

                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }


            }
        }
        /// <summary>
        /// 事件工单流转操作（处置5)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="workOrder_Oper_History"></param>
        /// <returns></returns>
        public MessageEntity WorkListChuZhi(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History)
        {

            var rows = 0;
            var insertSql = DapperExtentions.MakeInsertSql(workOrder_Oper_History);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    //插入
                    rows = conn.Execute(insertSql, workOrder_Oper_History);
                    return MessageEntityTool.GetMessage(1, null, true);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 事件工单流转操作（完工6)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="workOrder_Oper_History"></param>
        /// <returns></returns>
        public MessageEntity WorkListFinished(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History)
        {
            var rows = 0;
            var insertSql = DapperExtentions.MakeInsertSql(workOrder_Oper_History);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    //插入
                    rows = conn.Execute(insertSql, workOrder_Oper_History);
                    return MessageEntityTool.GetMessage(1, null, true);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 事件工单流转操作（审核 7 8)
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="workOrder_Oper_History"></param>
        /// <returns></returns>
        public MessageEntity WorkListAudit(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History)
        {
            var rows = 0;
            var insertSql = DapperExtentions.MakeInsertSql(workOrder_Oper_History);
            var updatesql = $" update ins_event set IsFinish=1 where eventID='{eventID}'";

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        //插入
                        rows = conn.Execute(insertSql, workOrder_Oper_History);
                        workOrder_Oper_History.OperId = 8;
                        workOrder_Oper_History.OperTime = DateTime.Parse(DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"));
                        var insertSql1 = DapperExtentions.MakeInsertSql(workOrder_Oper_History);
                        rows = conn.Execute(insertSql1, workOrder_Oper_History);
                        conn.Execute(updatesql);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
                }
        }
        /// <summary>
        ///  事件工单作废
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="execPersonId"></param>
        /// <param name="execPersonName"></param>
        /// <param name="execDetpID"></param>
        /// <param name="execDetpName"></param>
        /// <param name="OperId"></param>
        /// <returns></returns>
        public MessageEntity WorkorderInvalid(string eventID, Ins_WorkOrder_Oper_History workOrder_Oper_History)
        {
            var updateSql =$"update  M_Event set IsValid=0 where DeleteStatus=0 and EventID='{eventID}'";

            var rows = 0;
            var insertSql = DapperExtentions.MakeInsertSql(workOrder_Oper_History);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                {
                    conn.Execute(updateSql);
                    //插入
                    rows = conn.Execute(insertSql, workOrder_Oper_History);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }

    }

}
