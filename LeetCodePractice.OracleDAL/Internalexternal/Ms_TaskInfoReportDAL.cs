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
   public class Ms_TaskInfoReportDAL: IMs_TaskInfoReportDAL
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
    }
}
