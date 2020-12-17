using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System.Data;
using GISWaterSupplyAndSewageServer.IDAL.EssentialFactor;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Database;

namespace GISWaterSupplyAndSewageServer.OracleDAL.EssentialFactor
{
    public class EssentialFactorDAL: IEssentialFactorDAL
    {
        public MessageEntity Add(Model.EssentialFactor.GIS_EssentialFactor model)
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

        public MessageEntity Delete(Model.EssentialFactor.GIS_EssentialFactor model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var deleteSql = $"delete GIS_ESSENTIALFACTOR where  pid='{ model.ID}'";

                var excSql = DapperExtentions.MakeDeleteSql(model);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(deleteSql, transaction);
                        rows = conn.Execute(excSql, model);
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

        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = "select * from GIS_ESSENTIALFACTOR t " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

      public List<GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor> GetTree(string pID = "00000000-0000-0000-0000-000000000000", int eFType = 0)
        {
            string errorMsg = "";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    #region 条件
                    string sqlwhere = " where 1=1  ";
                  
                    if (eFType != 0)
                    {
                        sqlwhere += " and eFType=" + eFType;
                    }
                    #endregion
                    string sql = "select id,pid,EFNAME from GIS_ESSENTIALFACTOR t " + sqlwhere;
                    List<Model.EssentialFactor.GIS_EssentialFactor> list = conn.Query<Model.EssentialFactor.GIS_EssentialFactor>(sql).ToList();
                    return list;
                }
                catch (Exception e)
                {
                    errorMsg = e.Message;
                    return null;
                }
            }
           
        }
    
        public MessageEntity Update(Model.EssentialFactor.GIS_EssentialFactor model)
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
