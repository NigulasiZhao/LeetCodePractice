using Dapper;
using Dapper.Oracle;
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
   
    public class GIS_ConfigureDAL : IGIS_ConfigureDAL
    {
        /// <summary>
        /// 添加基础图层配置
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(GIS_Configure model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {

                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model, out OracleDynamicParameters parameters);
                List<GIS_FeatureLayerCollection> featureGroup = model.FeatureLayerGroup;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(insertSql, parameters, transaction);
                    featureGroup.ForEach(row =>
                    {
                        row.ConfigID = model.ID;
                        var rows1 = conn.Execute(DapperExtentions.MakeInsertSql(row, out OracleDynamicParameters parameters1), parameters1, transaction);
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
        /// 删除基础图层配置信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(GIS_Configure model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeDeleteSql(model);
                List<GIS_FeatureLayerCollection> featureGroup = model.FeatureLayerGroup;
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

        public GIS_Configure GetInfo(string ID)
        {
            List<GIS_Configure> _ListField = new List<GIS_Configure>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<GIS_Configure>("select * from GIS_Configure t where ID='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField[0].FeatureLayerGroup = conn.Query<GIS_FeatureLayerCollection>("select * from GIS_FeatureLayerCollection t where ConfigID='" + _ListField[0].ID.ToString() + "'").ToList();
                }
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得基础图层配置配置信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page,string sqlCondition)
        {

            string sql = "select * from GIS_Configure " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<GIS_Configure>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            foreach (GIS_Configure row in ResultList)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    row.FeatureLayerGroup = conn.Query<GIS_FeatureLayerCollection>("select * from GIS_FeatureLayerCollection t where ConfigID='" + row.ID.ToString() + "'").ToList();
                }
            }
            return result;
        }

        /// <summary>
        /// 获取地图配置
        /// </summary>
        /// <returns></returns>
        public MessageEntity GetMapConfig()
        {
            try
            {
                List<GIS_Configure> _ListField = new List<GIS_Configure>();
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField = conn.Query<GIS_Configure>("select * from GIS_Configure t").ToList();
                }
                Dictionary<string, object> returnValue = new Dictionary<string, object>();
                _ListField.ForEach(row =>
                {
                    if (row.IsFeatureLayer == 1)
                    {
                        using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                        {
                            var featureCollection = conn.Query<GIS_FeatureLayerCollection>("select * from GIS_FeatureLayerCollection t where ConfigID='" + row.ID.ToString() + "'").ToList();
                            returnValue.Add(row.TableName, featureCollection);
                        }
                    }
                    else
                    {
                        returnValue.Add(row.TableName, row.ConfigContent);
                    }
                });
                return MessageEntityTool.GetMessage(1, returnValue, true, "完成");
            }
            catch (Exception ex)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, ex.Message);
            }

        }

        /// <summary>
        /// 添加基础图层配置
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(GIS_Configure model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model, out OracleDynamicParameters parameters);
                List<GIS_FeatureLayerCollection> featureGroup = model.FeatureLayerGroup;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    //删除已经删除的配置项
                    List<string> FieldIDS = model.FeatureLayerGroup.Select(Row => Row.ID).ToList();
                    string Ids = "'" + string.Join("','", FieldIDS.ToArray()) + "'";
                    if (FieldIDS.Count <= 0)
                    {
                        Ids = "'" + "0" + "'";

                    }
                    conn.Execute("delete GIS_FeatureLayerCollection where ConfigID='" + model.ID + "' and ID not in(" + Ids + ")");
                    //执行修改
                    featureGroup.ForEach(row =>
                    {
                        if (row.ID != "" && row.ID != null && row.ID != "0")
                        {
                            conn.Execute(DapperExtentions.MakeUpdateSql(row, out OracleDynamicParameters parameters1).ToString(), parameters1, transaction);
                        }
                        else
                        {
                            conn.Execute(DapperExtentions.MakeInsertSql(row, out OracleDynamicParameters parameters1).ToString(), parameters1, transaction);
                        }
                    });
                    rows = conn.Execute(insertSql, parameters, transaction);
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
