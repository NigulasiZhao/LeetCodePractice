using Dapper;
using GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.GisConfigSetting
{
    public class GIS_PublicSearchDAL : IGIS_PublicSearchDAL
    {
        /// <summary>
        /// 搜索配置添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageEntity Add(GIS_PublicSearch model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model);
                List<GIS_SearchField> featureGroup = model.SearchField;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(insertSql, model, transaction);
                    featureGroup.ForEach(row =>
                    {
                        row.SearchID = model.ID;
                        var rows1 = conn.Execute(DapperExtentions.MakeInsertSql(row), row, transaction);
                    });
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }


        }

        /// <summary>
        /// 搜索配置删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageEntity Delete(GIS_PublicSearch model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeDeleteSql(model);
                List<GIS_SearchField> featureGroup = model.SearchField;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    featureGroup.ForEach(row =>
                    {
                        conn.Execute(DapperExtentions.MakeDeleteSql(row).ToString(), row, transaction);
                    });
                    rows = conn.Execute(insertSql, model, transaction);
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        /// <summary>
        /// 获取当前搜索对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public GIS_PublicSearch GetInfo(string ID)
        {
            List<GIS_PublicSearch> _ListField = new List<GIS_PublicSearch>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<GIS_PublicSearch>("select * from gis_publicsearch t where ID='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField[0].SearchField = conn.Query<GIS_SearchField>("select * from GIS_SearchField t where SearchID='" + _ListField[0].ID.ToString() + "'").ToList();
                }
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取当前搜索对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<GIS_PublicSearch> GetPublicSearchInfo()
        {
            List<GIS_PublicSearch> _ListField = new List<GIS_PublicSearch>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<GIS_PublicSearch>("select * from gis_publicsearch ").ToList();
            }
            if (_ListField.Count > 0)
            {
                
                return _ListField;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获得搜索配置信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = "select * from gis_publicsearch " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<GIS_PublicSearch>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            foreach (GIS_PublicSearch row in ResultList)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    row.SearchField = conn.Query<GIS_SearchField>("select * from GIS_SearchField t where SearchID='" + row.ID.ToString() + "'").ToList();
                }
            }
            return result;
        }

        public MessageEntity Update(GIS_PublicSearch model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model);
                List<GIS_SearchField> featureGroup = model.SearchField;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();

                    List<string> FieldIDS = model.SearchField.Select(Row => Row.ID).ToList();
                    string Ids = "'" + string.Join("','", FieldIDS.ToArray()) + "'";
                    if (FieldIDS.Count <= 0)
                    {
                        Ids = "'" + "0" + "'";

                    }
                    conn.Execute("delete GIS_SearchField where SearchID='" + model.ID + "' and ID not in (" + Ids + ")");

                    featureGroup.ForEach(row =>
                    {
                        if (row.ID != "" && row.ID != null && row.ID != "0")
                        {
                            conn.Execute(DapperExtentions.MakeUpdateSql(row).ToString(), row, transaction);
                        }
                        else
                        {
                            conn.Execute(DapperExtentions.MakeInsertSql(row).ToString(), row, transaction);
                        }
                    });
                    rows = conn.Execute(insertSql, model, transaction);
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
