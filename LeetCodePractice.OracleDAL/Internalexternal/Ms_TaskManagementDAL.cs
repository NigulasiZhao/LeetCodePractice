using Dapper;
using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GISWaterSupplyAndSewageServer.OracleDAL.Internalexternal
{
    public class Ms_TaskManagementDAL : IMs_TaskManagementDAL
    {
        public MessageEntity Add(Ms_TaskManagement model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(insertSql, model, transaction);
                    conn.Execute("update ms_filestore set ispost=1 where fid='" + model.Fid + "'", transaction);
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity GetAppTaskInfo(string fID)
        {
            string errorMsg = "";
            string sql = @"select * from Ms_TaskManagement ";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<dynamic> eventType = conn.Query<dynamic>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetFileInfoByTaskid(string taskId)
        {
            string errorMsg = "";
            string sql = $@"select f.* from ms_taskmanagement m left join ms_filestore f on m.fid=f.fid where m.taskid='{taskId}'";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ms_FileStore> eventType = conn.Query<Ms_FileStore>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetList(List<ParameterInfo> parInfo, string describe, string sort, string ordering, int num, int page, string sqlCondition)
        {
            if (!string.IsNullOrEmpty(describe))
            {
                sqlCondition += $" and f.filedescribe like '%{describe}%'";
            }

            string sql = @"select t.taskid, f.uploadername,f.uploadetime, case f.ispost when 1 then '已派发' else '未派发' end as ispost,
t.dispatchpersonname,t.taskdate,t.TaskDescribe,t.taskname,f.filedescribe,f.uploadpath,f.FId,f.uploaderId,t.ExecPersonId,t.ExecPersonName,t.DispatchPersonId
 from MS_FILESTORE f left join ms_taskmanagement t on f.fid = t.fid  " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<Ms_TaskFileList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public MessageEntity Update(Ms_TaskManagement model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeUpdateSql(model);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(excSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
    }
}
