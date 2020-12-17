using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.Accounts;
using GISWaterSupplyAndSewageServer.IDAL.Accounts;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Dapper;
using GISWaterSupplyAndSewageServer.Model;
using System.Data;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Database;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Accounts
{
    /// <summary>
    /// 阀门启闭台账
    /// </summary>
    public class Account_Valve : IAccount_Valve
    {
        public MessageEntity Add(Model.Accounts.Gis_Account_Valve model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model, out OracleDynamicParameters parameters);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql, parameters);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity Delete(Model.Accounts.Gis_Account_Valve model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeDeleteSql(model);
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

        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
           
            string sql = "select * from Gis_ACCOUNT_VALVE t" + sqlCondition;
            DapperExtentions.EntityForSqlToPager<Model.Accounts.Gis_Account_Valve>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        /// <summary>
        /// 根据统计字段，条件等信息进行数据统计
        /// </summary>
        /// <param name="parInfo">where条件</param>
        /// <param name="fieldsInfo">分组字段列表</param>
        /// <param name="fieldsCount">统计字段</param>
        /// <returns>统计结果</returns>
        public MessageEntity GetStatistics(List<ParameterInfo> parInfo, List<string> fieldsInfo, List<string> fieldsCount, string sqlCondition)
        {
            string sqlFields = string.Join(",", fieldsInfo.ToArray());//显示字段
          
            List<string> groupFields = new List<string>();
            //字段统计
            fieldsCount.ForEach(InfoDetials =>
            {
                groupFields.Add("count(" + InfoDetials + ")" + InfoDetials);

            });
            string sql = "select " + sqlFields + "," + string.Join(",", groupFields.ToArray()) + " from Gis_ACCOUNT_VALVE t" + sqlCondition + " group by " + sqlFields;

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                //var ResultQuery = conn.Query(sql);
                List<Gis_Account_Valve> ResultQuery = conn.Query<Gis_Account_Valve>(sql).ToList();

                return MessageEntityTool.GetMessage(ResultQuery.ToList().Count, ResultQuery, true, "完成");
            }
        }

        public MessageEntity Update(Model.Accounts.Gis_Account_Valve model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeUpdateSql(model, out OracleDynamicParameters parameters);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(excSql, parameters);
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
