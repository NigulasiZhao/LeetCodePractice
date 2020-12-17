using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.SQLServerDAL
{
    public class BaseDAL
    {
        public int DeleteEntity(object entity, ConnectionFactory.DBConnNames dBConnName, out MessageEntity messageEntity)
        {
            using (var conn = ConnectionFactory.GetDBConn(dBConnName))
            {
                var rows = 0;
                var result = DapperExtentions.MakeDeleteSql(entity);
                if (string.IsNullOrEmpty(result))
                {
                    messageEntity = MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                    return 0;
                }
                try
                {
                    rows = conn.Execute(result, entity);
                    messageEntity = MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    messageEntity = MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }

                return rows;
            }
        }

        public int InsertEntity(object entity, ConnectionFactory.DBConnNames dBConnName, out MessageEntity messageEntity)
        {
            using (var conn = ConnectionFactory.GetDBConn(dBConnName))
            {
                var rows = 0;
                var result = DapperExtentions.MakeInsertSql(entity);
                if (string.IsNullOrEmpty(result))
                {
                    messageEntity = MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                    return 0;
                }
                try
                {
                    rows = conn.Execute(result, entity);
                    messageEntity = MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    messageEntity = MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }

                return rows;
            }
        }

        public int UpdateEntity(object entity, ConnectionFactory.DBConnNames dBConnName, out MessageEntity messageEntity)
        {
            using (var conn = ConnectionFactory.GetDBConn(dBConnName))
            {
                var rows = 0;
                var updateSql = DapperExtentions.MakeUpdateSql(entity);
                if (string.IsNullOrEmpty(updateSql))
                {
                    messageEntity = MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                    return 0;
                }
                try
                {
                    rows = conn.Execute(updateSql, entity);
                    messageEntity = MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    messageEntity = MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }

                return rows;
            }
        }
    }
}
