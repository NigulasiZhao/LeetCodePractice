using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Plan
{
    public class Ins_RangeDAL : IIns_RangeDAL
    {
        public MessageEntity Add(Ins_Range model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
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

        public MessageEntity Delete(Ins_Range model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM INS_PLAN WHERE RANGE_ID = '{0}'", model.Range_id);
                int DataRows = conn.ExecuteScalar<int>(GetDataSql);
                if (DataRows > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "已关联巡检计划数据不允许删除");
                }
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

      

        public MessageEntity GetList(List<GISWaterSupplyAndSewageServer.Model.ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select * from Ins_Range
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Range>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public List<Ins_Range> GetTree(string pID = "00000000-0000-0000-0000-000000000000")
        {
            
                string errorMsg = "";
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    try
                    {
                        #region 条件
                        string sqlwhere = " where 1=1  ";
                        #endregion
                        string sql = "select range_id,range_parentid,range_name from Ins_Range  " + sqlwhere;
                        List<Ins_Range> list = conn.Query<Ins_Range>(sql).ToList();
                        return list;
                    }
                    catch (Exception e)
                    {
                        errorMsg = e.Message;
                        return null;
                    }
                }
            
        }

        public MessageEntity IsExistRange(Ins_Range model, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(model.Range_name))
            {
                strWhere += $" and range_name ='{model.Range_name}' ";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and Range_id <>'{model.Range_id}'";
            }
            string query = $@" select Range_id,range_name from Ins_Range where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Range> result = conn.Query<Ins_Range>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity IsExistRangeChildren(string range_id)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(range_id))
            {
                strWhere += $" and range_parentid ='{range_id}' ";

            }

            string query = $@" select * from Ins_Range where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Range> result = conn.Query<Ins_Range>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity Update(Ins_Range model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model, out OracleDynamicParameters parameters);

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
    }
}
