using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Database;

namespace GISWaterSupplyAndSewageServer.OracleDAL.GisConfigSetting
{
   public class GIS_TableDAL: IGIS_DataTableDAL
    {
        public MessageEntity Add(GIS_DataTable model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {

                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model);
                List<GIS_DataTableColumn> featureGroup = model.GisDataTableColumn;
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
                        row.TableID = model.ID;
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

        public MessageEntity Delete(GIS_DataTable model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;

                var excSql = DapperExtentions.MakeDeleteSql(model);
                List<GIS_DataTableColumn> featureGroup = model.GisDataTableColumn;
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }


                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(excSql, model);
                    featureGroup.ForEach(row =>
                    {
                        conn.Execute(DapperExtentions.MakeDeleteSql(row).ToString(), row, transaction);
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
        public GIS_DataTable GetInfo(string ID)
        {
            List<GIS_DataTable> _ListField = new List<GIS_DataTable>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<GIS_DataTable>("select * from gis_datatable t where ID='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField[0].GisDataTableColumn = conn.Query<GIS_DataTableColumn>("select * from gis_datatablecolumn t where tableid='" + _ListField[0].ID.ToString() + "'").ToList();
                }
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }
        public MessageEntity GetList(List<Model.ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = "select * from GIS_DATATABLE " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<GIS_DataTable>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            //取得当前对应的数据列表信息
            //var ResultList = result.Data.Result as List<GIS_DataTable>;// 反转结果数据信息
            foreach (GIS_DataTable row in ResultList)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    row.GisDataTableColumn = conn.Query<GIS_DataTableColumn>("select * from GIS_DataTableColumn t where tableid='" + row.ID.ToString() + "'").ToList();
                }
            }
            return result;
        }
        
        /// <summary>
        /// 获取基础配置
        /// </summary>
        /// <returns></returns>
        public MessageEntity GetDataTable(List<GIS_DataTable> list)
        {
            try
            {
                Dictionary<string, object> returnValue = new Dictionary<string, object>();
                list.ForEach(row =>
                {
                    
                        returnValue.Add(row.TableName, row.GisDataTableColumn);
                    
                });
                return MessageEntityTool.GetMessage(1, returnValue, true, "完成");
            }
            catch (Exception ex)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, ex.Message);
            }

        }
        public MessageEntity Update(GIS_DataTable model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model);
                List<GIS_DataTableColumn> featureGroup = model.GisDataTableColumn;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    //删除已经删除的配置项
                    List<string> FieldIDS = model.GisDataTableColumn.Select(Row => Row.ID).ToList();
                    string Ids = "'" + string.Join("','", FieldIDS.ToArray()) + "'";
                    if (FieldIDS.Count<=0)
                    {
                        Ids = "'" + "0" + "'";

                    }
                    conn.Execute("delete gis_datatablecolumn where TableID='" + model.ID + "' and ID not in ("+ Ids+")");
                    //执行修改
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
