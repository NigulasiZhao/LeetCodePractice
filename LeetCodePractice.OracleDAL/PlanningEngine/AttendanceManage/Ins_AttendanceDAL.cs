using Dapper;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.AttendanceManage
{
    public class Ins_AttendanceDAL : IIns_AttendanceDAL
    {
        public MessageEntity Add(Ins_Attendance model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity GetLastAttendance(string Personid)
        {
            string errorMsg,sqlWhere="";
            if (!string.IsNullOrEmpty(Personid))
            {
                sqlWhere += $" and personId='{Personid}'";
            }
            string query = $@"select * from (select t.*,ROW_NUMBER() over( order by t.uptime desc) as rowsnum from INS_ATTENDANCE t where 1=1 {sqlWhere} ) where rowsnum=1  ";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Attendance> result = conn.Query<Ins_Attendance>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetLastAttendanceByTaskid(string taskid)
        {
            string errorMsg, sqlWhere = "";
            if (!string.IsNullOrEmpty(taskid))
            {
                sqlWhere += $" and taskid='{taskid}'";
            }
            string query = $@"select * from (select t.*,ROW_NUMBER() over( order by t.uptime desc) as rowsnum from INS_ATTENDANCE t where 1=1 {sqlWhere} ) where rowsnum=1  ";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Attendance> result = conn.Query<Ins_Attendance>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select * from Ins_Attendance  
" + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Attendance>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
    }
}
