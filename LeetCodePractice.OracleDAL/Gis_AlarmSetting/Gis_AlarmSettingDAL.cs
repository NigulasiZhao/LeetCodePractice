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
using GISWaterSupplyAndSewageServer.IDAL.Gis_AlarmSetting;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Database;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Gis_AlarmSetting
{
   public class Gis_AlarmSettingDAL:IGis_AlarmSettingDAL
    {
        public MessageEntity Add(Model.AlarmSetting.GIS_Alarm_Setting model)
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

        public MessageEntity Delete(Model.AlarmSetting.GIS_Alarm_Setting model)
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
            string sql = "select * from GIS_ALARM_SETTING t " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<Model.AlarmSetting.GIS_Alarm_Setting>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public MessageEntity Update(Model.AlarmSetting.GIS_Alarm_Setting model)
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
