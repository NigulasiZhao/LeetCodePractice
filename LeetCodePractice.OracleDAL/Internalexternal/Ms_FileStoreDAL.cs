using Dapper;
using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Internalexternal
{
    public class Ms_FileStoreDAL : IMs_FileStoreDAL
    {
        public MessageEntity Add(Ms_FileStore model)
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
                    rows = conn.Execute(insertSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity GetFileInfoByid(string fID)
        {
            string errorMsg = "";
            string sql = $@"select * from MS_FILESTORE where fid='{fID}'";
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

        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = "select Fid, Uploaderid,Uploadername, Uploadetime, Uploadpath,fileDescribe, ispost,case ispost when 1 then '已派发' else '未派发' end as ispostname,Filekey from Ms_FileStore " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<Ms_FileStore>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public MessageEntity IsExistFileHash(string fileKey)
        {
            string errorMsg = "";
            string sql = $@"select filekey from MS_FILESTORE where filekey='{fileKey}'";
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
    }
}
