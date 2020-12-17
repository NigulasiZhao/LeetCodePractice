using Dapper;
using GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.GisConfigSetting
{
    public class GIS_SearchFieldDAL : IGIS_SearchFieldDAL
    {
        /// <summary>
        /// 添加图层类别
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(GIS_SearchField model)
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

        /// <summary>
        /// 删除图层类别
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(GIS_SearchField model)
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

        /// <summary>
        /// 获取图层类别并分页
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = "select * from GIS_SearchField t " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<dynamic>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        /// <summary>
        /// 修改图层类别
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(GIS_SearchField model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeUpdateSql(model);
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
    }
}
