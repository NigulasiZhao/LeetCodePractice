using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.AttendanceManage
{
    public class Ins_TaskCoordinatsDAL : IIns_TaskCoordinatsDAL
    {
        #region
        //public Dictionary<string, double[]> GetCoordinatsByTaskid(string taskId, out ErrorType errorType, out string errorString)
        //{
        //    errorString = string.Empty;
        //    errorType = ErrorType.Success;
        //    Dictionary<string, double[]> result = new Dictionary<string, double[]>();

        //    string query = $@" select t.uptime,t.positionx,t.positiony from INS_PERSONPOSITION t where t.taskid=:taskId order by t.uptime asc ";

        //    try
        //    {
        //        using var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL);
        //        var coordinatsList = conn.Query(query, new { taskId }).ToList();
        //        if (coordinatsList.Count > 0)
        //        {
        //            result = new Dictionary<string, double[]>();
        //            double[] coordinats = new double[2] { 0, 0 };
        //            double lon = 0;
        //            double lat = 0;
        //            string upTime = string.Empty;
        //            foreach (var item in coordinatsList)
        //            {
        //                if (double.TryParse(item.POSITIONX as String, out lon) && double.TryParse(item.POSITIONY as String, out lat))
        //                {
        //                    coordinats = new double[] { lon, lat };
        //                }
        //                upTime = item.UPTIME.ToString();
        //                if (!result.Keys.Any(p => p == upTime))
        //                {
        //                    result.Add(upTime, coordinats);
        //                }

        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        errorType = ErrorType.SystemError;
        //        errorString = e.Message;
        //        return null;
        //    }
        //}
        #endregion
        public MessageEntity GetCoordinatsByTaskid(string taskId, DateTime? startTime, DateTime? endTime, out ErrorType errorType, out string errorString)
        {
            errorString = string.Empty;
            errorType = ErrorType.Success;
            string sqlWhere = "  ";
            if (startTime != null)
            {
                sqlWhere += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sqlWhere += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }

            string query = $@" select t.uptime,t.positionx,t.positiony from INS_PERSONPOSITION t where t.taskid='{taskId}' {sqlWhere}order by t.uptime asc ";

            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_PersonPositionDto> list = conn.Query<Ins_PersonPositionDto>(query).ToList();
                    return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());
                }
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
