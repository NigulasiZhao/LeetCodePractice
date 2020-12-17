using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.Statistics;
using GISWaterSupplyAndSewageServer.Model.Statistics;
using Npgsql.TypeHandlers.NetworkHandlers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Westwind.Utilities.Data;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Statistics
{
    public class GIS_DataQualifyDAL : IGIS_DataQualifyDAL
    {
        public DataQualify GetData(DateTime? startTime, DateTime? endTime, out ErrorType errorType, out string errorString)
        {
            errorString = string.Empty;
            errorType = ErrorType.Success;
            string sqlWhere = "1=1";
            if (startTime != null)
            {
                sqlWhere += $" and t.finish_date >=to_timestamp('{startTime}','yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sqlWhere += $" and t.finish_date <=to_timestamp('{endTime}','yyyy-mm-dd hh24:mi:ss')";
            }

            string query = $@"
begin 
open :阀门 for
select ms.otitle,
       count(0) as total,
       sum(case
             when t.subtype is null then
              0
             else
              1
           end) as subtype,
       sum(case
             when t.d_s is null then
              0
             else
              1
           end) as d_s,
       sum(case
             when t.USETYPE is null then
              0
             else
              1
           end) as USETYPE,
       sum(case
             when t.SUR_H  is null then
              0
             else
              1
           end) as SUR_H,
       sum(case
             when t.CEN_DEEP  is null then
              0
             else
              1
           end) as CEN_DEEP ,
       sum(case
             when t.TOP_H is null then
              0
             else
              1
           end) as TOP_H,
       sum(case
             when t.finish_date is null
             then 0
             else
              1
           end) as finish_date
     
  from GSSS_VALVE t
  left join MANAGE_GRID mg
    on mg.ocode = t.grid
  left join manage_firstzone ms
    on mg.a_firstcode = ms.ocode
    where {sqlWhere}
 group by ms.otitle order by ms.otitle;
open :管网 for
select ms.otitle,
       count(0) as total,
       sum(case
             when t.d_s is null then
              0
             else
              1
           end) as d_s,
       sum(case
             when t.material is null then
              0
             else
              1
           end) as material,
        sum(case
             when t.finish_date is null
             then 0
             else
              1
           end) as finish_date,
       sum(case
             when t.DATATYPE is null then
              0
             else
              1
           end) as DATATYPE
  from (select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_ORDINLN t
        union
        select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_QUALILN t
        union
        select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_BACTNLN t
        union
        select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_OTHERLIN t
        union
        select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_PRESSTNLN t
        union
        select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_COMTNLN t
        union
        select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_DIFQTNLN t
        union
        select t.d_s, t.material, t.FINISH_DATE, t.DATATYPE, t.grid
          from GSGX_MIDTNLN t) t
  left join MANAGE_GRID mg
    on mg.ocode = t.grid
  left join manage_firstzone ms
    on mg.a_firstcode = ms.ocode
    where {sqlWhere}
 group by ms.otitle order by ms.otitle;
open :消防栓 for
select ms.otitle,
       count(0) as total,
       sum(case
             when t.LANE_WAY is null then
              0
             else
              1
           end) as LANE_WAY,
       sum(case
             when t.ADDR is null then
              0
             else
              1
           end) as ADDR,
       sum(case
             when t.subtype is null then
              0
             else
              1
           end) as subtype,
       sum(case
             when t.SUR_H is null then
              0
             else
              1
           end) as SUR_H,
       sum(case
             when t.D_S is null then
              0
             else
              1
           end) as D_S,
       sum(case
             when t.managedept is null then
              0
             else
              1
           end) as managedept
  from (select LANE_WAY,
               LANE_WAY as ADDR,
               subtype,
               SUR_H,
               D_S,
               MANAGER  as managedept,
               grid,
               ADD_DATE as finish_date
          from GSSS_FIREPLUG
        union
        select LANE_WAY, ADDR, subtype, SUR_H, D_S, managedept, grid,finish_date
          from GSSS_FIREPLUG_CK
        union
        select LANE_WAY, ADDR, subtype, SUR_H, D_S, managedept, grid,finish_date
          from GSSS_FIREPLUG_SZ) t
  left join MANAGE_GRID mg
    on mg.ocode = t.grid
  left join manage_firstzone ms
    on mg.a_firstcode = ms.ocode
    where {sqlWhere}
 group by ms.otitle order by ms.otitle;
end;
";

            try
            {

                using var conn = (OracleConnection)ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE);
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;

                cmd.Parameters.Add(":阀门", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(":管网", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(":消防栓", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                var reader = cmd.ExecuteReader();

                DataTable dt;
                int loop = 0;
                Dictionary<string, Dictionary<string, IList<string>>> keyValuePairs = new Dictionary<string, Dictionary<string, IList<string>>>();
                IList<KeyValuePair<string, IList<string>>> childKeyValuePairs;
                IList<string> titles;
                IList<int> totalList;
                IList<decimal> rowTotal;
                decimal field = 0;
                while (!reader.IsClosed)
                {
                    childKeyValuePairs = new List<KeyValuePair<string, IList<string>>>();
                    titles = new List<string>();
                    dt = new DataTable();
                    dt.Load(reader);
                    rowTotal = new List<decimal>();
                    totalList = new List<int>();
                    for (int i = 2; i <= dt.Columns.Count - 1; i++)
                    {
                        childKeyValuePairs.Add(new KeyValuePair<string, IList<string>>(dt.Columns[i].ColumnName, new List<string>()));
                        rowTotal.Add(0);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        titles.Add(dt.Rows[i][0] == DBNull.Value ? "其它" : dt.Rows[i][0].ToString());

                        for (int j = 1; j <= dt.Rows[i].ItemArray.Count() - 1; j++)
                        {

                            if (j == 1)
                            {
                                totalList.Add(Convert.ToInt32(dt.Rows[i][j]));
                            }
                            else
                            {
                                field = Math.Round(Convert.ToDecimal(dt.Rows[i][j]) * 100);

                                childKeyValuePairs[j - 2].Value.Add((field / totalList[i]).ToString("#.00") + "%");
                                rowTotal[j - 2] += field;
                            }
                        }
                    }

                    for (int i = 0; i < childKeyValuePairs.Count; i++)
                    {
                        childKeyValuePairs[i].Value.Add((rowTotal[i] / totalList.Sum()).ToString("#.00") + "%");
                    }

                    titles.Add("合计");
                    childKeyValuePairs.Insert(0, new KeyValuePair<string, IList<string>>("Titles", titles));

                    keyValuePairs.Add(cmd.Parameters[loop].ParameterName.Replace(":", ""), childKeyValuePairs.ToDictionary(x => x.Key, x => x.Value));

                    loop++;

                }
                return new DataQualify() { Records = keyValuePairs };
            }
            catch (Exception e)
            {
                errorType = ErrorType.SystemError;
                errorString = e.Message;
                return null;
            }
        }

        public DataQualify GetFacilityCount(DateTime? startTime, DateTime? endTime, out ErrorType errorType, out string errorString)
        {
            errorString = string.Empty;
            errorType = ErrorType.Success;
            string sqlWhere = "1=1";
            if (startTime != null)
            {
                sqlWhere += $" and t.finish_date >=to_timestamp('{startTime}','yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sqlWhere += $" and t.finish_date <=to_timestamp('{endTime}','yyyy-mm-dd hh24:mi:ss')";
            }

            string query = $@"
begin 
open :供水管线 for
select ms.otitle,sum(t.length) as record from (
select t.length,t.finish_date,t.grid from sde.GSGX_ORDINLN t 
union all
select t.length,t.finish_date,t.grid from sde.GSGX_QUALILN t 
union all
select t.length,t.finish_date,t.grid from sde.GSGX_BACTNLN t 
union all
select t.length,t.finish_date,t.grid from sde.GSGX_OTHERLIN t 
union all
select t.length,t.finish_date,t.grid from sde.GSGX_PRESSTNLN t ) t 
left join sde. MANAGE_GRID mg on mg.ocode = t.grid left join sde. manage_firstzone ms on mg.a_firstcode = ms.ocode
where {sqlWhere}
 group by ms.otitle order by ms.otitle;

open :原水管线 for
select ms.otitle,sum(t.length) as record from (
select t.length,t.finish_date,t.grid from sde.ysgx_channel t 
union all
select t.length,t.finish_date,t.grid from sde.ysgx_desigln t 
union all
select t.length,t.finish_date,t.grid from sde.ysgx_dismaln t 
union all
select t.length,t.finish_date,t.grid from sde.ysgx_ordinln t 
union all
select t.length,t.finish_date,t.grid from sde.ysgx_plannln t )t 
left join sde. MANAGE_GRID mg on mg.ocode = t.grid left join sde. manage_firstzone ms on mg.a_firstcode = ms.ocode
where {sqlWhere}
 group by ms.otitle order by ms.otitle;
 
open :小区管线 for
select ms.otitle,sum(t.length) as record from sde.GSGX_COMTNLN t left join sde. MANAGE_GRID mg on mg.ocode = t.grid left join sde. manage_firstzone ms on mg.a_firstcode = ms.ocode 
where {sqlWhere}
 group by ms.otitle order by ms.otitle;

open :市政消防栓 for
select ms.otitle,count(0) as record from sde.GSSS_FIREPLUG_SZ t left join  sde.MANAGE_GRID mg on mg.ocode = t.grid left join sde. manage_firstzone ms on mg.a_firstcode = ms.ocode
where {sqlWhere} 
group by ms.otitle order by ms.otitle;

open :参考消防栓 for
select ms.otitle,count(0) as record from sde.GSSS_FIREPLUG_CK t left join  sde.MANAGE_GRID mg on mg.ocode = t.grid left join sde. manage_firstzone ms on mg.a_firstcode = ms.ocode 
where {sqlWhere}
group by ms.otitle order by ms.otitle;

open :消防栓 for
select ms.otitle,count(0) as record from sde.GSSS_FIREPLUG t left join  sde.MANAGE_GRID mg on mg.ocode = t.grid left join sde. manage_firstzone ms on mg.a_firstcode = ms.ocode
where {sqlWhere.Replace("finish_date", "ADD_DATE")}
 group by ms.otitle order by ms.otitle;
end;
";

            try
            {

                using var conn = (OracleConnection)ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE);
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;

                cmd.Parameters.Add(":管线供水", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(":管线原水", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(":管线小区", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(":市政消防栓", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(":参考消防栓", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(":消防栓", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                var reader = cmd.ExecuteReader();

                DataTable dt;
                int loop = 0;
                Dictionary<string, Dictionary<string, IList<string>>> keyValuePairs = new Dictionary<string, Dictionary<string, IList<string>>>();
                IList<KeyValuePair<string, IList<string>>> childKeyValuePairs;
                IList<string> titles;
                IList<string> rowData;
                //IList<decimal> rowTotal;
                decimal field = 0;
                while (!reader.IsClosed)
                {
                    childKeyValuePairs = new List<KeyValuePair<string, IList<string>>>();
                    titles = new List<string>();
                    rowData = new List<string>();
                    field = 0;
                    dt = new DataTable();
                    dt.Load(reader);
                    //rowTotal = new List<decimal>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        titles.Add(dt.Rows[i][0] == DBNull.Value ? "其它" : dt.Rows[i][0].ToString());
                        field += Convert.ToDecimal(dt.Rows[i][1]);
                        rowData.Add(dt.Rows[i][1].ToString());

                        if (i == dt.Rows.Count - 1)
                        {
                            rowData.Add(field.ToString());
                        }
                    }

                    titles.Add("合计");
                    childKeyValuePairs.Insert(0, new KeyValuePair<string, IList<string>>("Titles", titles));
                    childKeyValuePairs.Add(new KeyValuePair<string, IList<string>>("Data", rowData.Count!=0?rowData:new List<string>() { "0" }));

                    keyValuePairs.Add(cmd.Parameters[loop].ParameterName.Replace(":", ""), childKeyValuePairs.ToDictionary(x => x.Key, x => x.Value));

                    loop++;

                }
                return new DataQualify() { Records = keyValuePairs };
            }
            catch (Exception e)
            {
                errorType = ErrorType.SystemError;
                errorString = e.Message;
                return null;
            }
        }
    }
}
