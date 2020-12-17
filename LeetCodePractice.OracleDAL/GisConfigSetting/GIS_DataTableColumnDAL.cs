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
    public class GIS_DataTableColumnDAL : IGIS_DataTableColumnDAL
    {
        public MessageEntity Add(GIS_DataTableColumn model)
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

        public MessageEntity Delete(GIS_DataTableColumn model)
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
                    if (model.ISSystem == 1)
                    {
                        return MessageEntityTool.GetMessage(1, null, true, "系统字段不允许删除");
                    }
                    else
                    {
                        rows = conn.Execute(excSql, model);
                    }
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
            
            string sql = "select * from gis_datatablecolumn t " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<GIS_DataTableColumn>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
        public GIS_DataTableColumn GetInfo(string ID)
        {
            List<GIS_DataTableColumn> _ListField = new List<GIS_DataTableColumn>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<GIS_DataTableColumn>("select * from GIS_DataTableColumn t where ID='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
               
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }
        public MessageEntity Update(GIS_DataTableColumn model)
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
                   

                    if (model.ISSystem == 1)
                    {
                        rows = conn.Execute(" update GIS_DATATABLECOLUMN t set t.align =@align,t.text =@text,t.controltype =@ControlType ,t.orderno =@OrderNO ,t.istempinfo =@ISTempInfo ,t.isanalysis =@ISAnalysis, where t.id = @ID ", new { align = model.align, text = model.text, ControlType = model.ControlType, OrderNO = model.OrderNO, ISTempInfo = model.ISTempInfo, ISAnalysis = model.ISAnalysis, ID = model.ID });
                    }
                    else
                    {
                        rows = conn.Execute(excSql, model);
                    }
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
