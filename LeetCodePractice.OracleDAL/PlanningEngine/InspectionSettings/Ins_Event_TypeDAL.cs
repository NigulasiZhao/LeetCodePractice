using Dapper;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Database;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.ResultDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GisPlateformForCore.OracleDAL.PlanningEngine.InspectionSettings
{
    public class Ins_Event_TypeDAL : IIns_Event_TypeDAL
    {
        public MessageEntity AddEventType(Ins_Event_Type eventType)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(eventType);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql, eventType);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity DeleteEventType(Ins_Event_Type eventType)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeDeleteSql(eventType);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(excSql, eventType);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity GetEventType(string sort, string ordering, int num, int page)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "eventTypeName";
            }

            if (string.IsNullOrEmpty(ordering))
            {
                ordering = "asc";
            }
            string sqlstr = @"select * from INS_Event_Type where ParentTypeId='00000000-0000-0000-0000-000000000000'";
            DapperExtentions.EntityForSqlToPager<Ins_Event_Type>(sqlstr, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);

            return result;
        }

        public MessageEntity GetEventTypeCommboBoxList()
        {
            string errorMsg = "";
            string query = " select event_Type_id,EventTypeName from INS_Event_Type where ParentTypeId='00000000-0000-0000-0000-000000000000'";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_Event_Type> eventType = conn.Query<Ins_Event_Type>(query).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        //public MessageEntity IsExistEvent(string eventTypeId)
        //{
        //    string errorMsg = "";
        //    string strWhere = "";
        //    if (!string.IsNullOrEmpty(eventTypeId.ToString()))
        //    {
        //        strWhere += $" and EventTypeId2 ={eventTypeId} ";

        //    }
        //    string query = $@" SELECT EventTypeId2 FROM INS_Event  where IsValid <>0 and DeleteStatus=0  {strWhere}";
        //    try
        //    {
        //        using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.PipeInspectionBase_Gis_OutSide))
        //        {
        //            IList<dynamic> result = conn.Query<dynamic>(query).ToList();
        //            return MessageEntityTool.GetMessage(1, result);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        errorMsg = e.Message;
        //        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
        //    }
        //}

        public MessageEntity IsExistEventcontent(string eventTypeId)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(eventTypeId.ToString()))
            {
                strWhere += $" and ParentTypeId ='{eventTypeId}' ";

            }
            string query = $@" select * from INS_Event_Type where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Event_Type> result = conn.Query<Ins_Event_Type>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity IsExistEventType(Ins_Event_Type eventType, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(eventType.EventTypeName))
            {
                strWhere += $" and EventTypeName ='{eventType.EventTypeName}' ";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and event_Type_id <>'{eventType.Event_Type_id}' and ParentTypeId ='{eventType.ParentTypeId}'";
            }
            string query = $@" select EventTypeName from INS_Event_Type where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Event_Type> result = conn.Query<Ins_Event_Type>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }
        public MessageEntity IsExistEventContent(Ins_Event_Type eventType, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(eventType.EventTypeName))
            {
                strWhere += $" and EventTypeName ='{eventType.EventTypeName}' and  ParentTypeId='{eventType.ParentTypeId}'";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and event_Type_id <>'{eventType.Event_Type_id}' and ParentTypeId ='{eventType.ParentTypeId}'";
            }
            string query = $@" select EventTypeName from INS_Event_Type where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Event_Type> result = conn.Query<Ins_Event_Type>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity UpdateEventType(Ins_Event_Type eventType)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeUpdateSql(eventType);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(excSql, eventType);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        public DataTable GetEventExecTime(string eventTypeID)
        {
            string errorMsg = "";
            string sql = $" select * from INS_Event_Type where event_type_id='{eventTypeID}'";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt_Type = new DataTable();
                    dt_Type.Load(conn.ExecuteReader(sql));

                    return dt_Type;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }

        }
        /// <summary>
        /// 根据事件id获取时间内容
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public MessageEntity GetEventContentInfo(string eventTypeId, string sort, string ordering, int num, int page)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "EventTypeName";
            }

            if (string.IsNullOrEmpty(ordering))
            {
                ordering = "asc";
            }
            string sqlstr = @"select b.event_Type_id,A.eventTypeName ParentTypeName,b.EventTypeName,b.ParentTypeId,b.ExecTime from INS_Event_Type a left join INS_Event_Type b on b.ParentTypeId = a.event_Type_id where 1=1 and b.ParentTypeId  <>  '00000000-0000-0000-0000-000000000000'";
            if (eventTypeId != null)
            {
                sqlstr += " and b.ParentTypeId=( select event_Type_id from INS_Event_Type where  event_Type_id ='" + eventTypeId + "')";
            }
            DapperExtentions.EntityForSqlToPager<dynamic>(sqlstr, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);

            return result;
        }

        public MessageEntity IsExistEvent(string eventTypeId)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(eventTypeId.ToString()))
            {
                strWhere += $" and EventTypeId2 ='{eventTypeId}' ";

            }
            string query = $@" SELECT EventTypeId2 FROM Ins_Event  where IsValid <>0 and DeleteStatus=0  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Event> result = conn.Query<Ins_Event>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetEventTypeContent()
        {
            try
            {
                List<InsEventTypeDto> _ListField = new List<InsEventTypeDto>();
                List<InsEventTypeDto> alllist = new List<InsEventTypeDto>();

                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    alllist = conn.Query<InsEventTypeDto>("select * from Ins_Event_Type  ").ToList();

                    _ListField = conn.Query<InsEventTypeDto>("select * from Ins_Event_Type  where ParentTypeId='00000000-0000-0000-0000-000000000000'").ToList();
                }
                Dictionary<string, object> returnValue = new Dictionary<string, object>();
                _ListField.ForEach(row =>
                {
                   
                        using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                        {
                            var list = alllist.Where(x => x.ParentTypeId == row.Event_Type_id).ToList(); ;
                            row.EventContentList = list;  
                    }
                   
                });
                return MessageEntityTool.GetMessage(1, _ListField, true, "完成");
            }
            catch (Exception ex)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, ex.Message);
            }
        }
    }
}
