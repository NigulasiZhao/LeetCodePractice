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
    public class GIS_FeatureLayerCollectionDAL : IGIS_FeatureLayerCollectionDAL
    {
        /// <summary>
        /// 添加FeatureLayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageEntity Add(GIS_FeatureLayerCollection model)
        {
            var insertSql = DapperExtentions.MakeInsertSql(model,out OracleDynamicParameters parameters);

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
            {
                return MessageEntityTool.GetMessage(conn.Execute(insertSql, parameters));
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }
        }

        /// <summary>
        /// 删除FeatureLayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageEntity Delete(GIS_FeatureLayerCollection model)
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

        public MessageEntity Get()
        {
            string errorMsg = "";
            string sql = "select * from GIS_FEATURELAYERCOLLECTION t ";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {

                    List<GIS_FeatureLayerCollection> result = conn.Query<GIS_FeatureLayerCollection>(sql).ToList();

                    return MessageEntityTool.GetMessage(result.Count(), result, true, "", result.Count());


                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        /// <summary>
        /// 获取FeatureLayer列表
        /// </summary>
        /// <param name="parInfo">搜索参数</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">每页行数</param>
        /// <param name="page">当前页码</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string errorMsg = "";
            string sql = "select * from GIS_FEATURELAYERCOLLECTION t " + sqlCondition;
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {

                    List<GIS_FeatureLayerCollection> result = conn.Query<GIS_FeatureLayerCollection>(sql).ToList();

                    return MessageEntityTool.GetMessage(result.Count(), result, true, "", result.Count());


                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }


        }

        /// <summary>
        /// 修改FeatureLayer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageEntity Update(GIS_FeatureLayerCollection model)
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
