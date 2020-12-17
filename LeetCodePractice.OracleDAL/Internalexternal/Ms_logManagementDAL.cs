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
    public class Ms_logManagementDAL : IMs_logManagementDAL
    {
        public MessageEntity Add(Ms_logManagement model)
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

        public MessageEntity AddList(List<Ms_logManagement> loglist)
        {
            var rows = 0;
            try
            {
                loglist.ForEach(row => {
                    using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                    {
                        var insertSql = DapperExtentions.MakeInsertSql(row);
                        var num = conn.Execute(insertSql, row);
                        rows++;
                    }
                });
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
            return MessageEntityTool.GetMessage(rows);
        }

        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"select Lid, Operationtype,case Operationtype when 1 then '登陆' when 2 then '菜单点击' when 3 then '设备编辑' when 4 then '管线编辑' when 5 then '文件导入' when 6 then '内业系统派发' 
                                 else '其他' end as OperationtypeName,
                                   Operatorid, Operatorname,Operatortime,Newvalue,Oldvalue, Operationfield from Ms_logManagement  " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<Ms_logManagementList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
    }
}
