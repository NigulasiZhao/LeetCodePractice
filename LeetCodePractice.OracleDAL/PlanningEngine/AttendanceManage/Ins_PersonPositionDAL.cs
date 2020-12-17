using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.AttendanceManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.AttendanceManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.AttendanceManage
{
    public class Ins_PersonPositionDAL : IIns_PersonPositionDAL
    {
        public MessageEntity Add(Ins_PersonPosition model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
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

        public MessageEntity GetPositionByPersonid(string OffLineTime)
        {
            string errorMsg;
            string query = $@" select p.*,case when  floor((sysdate- p.uptime)*24*60) >{OffLineTime} then '离线' else '在线'  END AS IsOnline from INS_PERSONPOSITION p  join (
select p.personid,p.personname,max(p.uptime) uptime from INS_PERSONPOSITION p group by p.personid,p.personname) pp on pp.personid=p.personid and pp.uptime=p.uptime  ";
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
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public DataTable GetPositionByTaskid(string taskid)
        {
            string errorMsg = "";
            string strWhere = $" and p.taskid = '{taskid}' ";
            string query = $@"select p.positionid, p.groupid,p.taskid,p.positionx,p.positiony,p.uptime ,p.distance,p.minutes,CEIL((sysdate - p.uptime) * 24 *60 )  AS xcminutes from INS_PERSONPOSITION p  join (
select p.taskid,max(p.uptime) uptime from INS_PERSONPOSITION p group by p.taskid) pp on pp.taskid=p.taskid and pp.uptime=p.uptime where 1=1 {strWhere}";
            try
            {
                using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query));
                    return dt;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }

        public MessageEntity Update(string taskid, string minutes, int distance)
        {
            
            var updateSql = $@"update INS_PERSONPOSITION set distance=distance+{distance}  where taskid='{taskid}'";
            var rows = 0;

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                    try
                    {
                        rows= conn.Execute(updateSql);
                       
                        return MessageEntityTool.GetMessage(rows, null, true);
                    }
                    catch (Exception e)
                    {
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                
            }
        }
        public MessageEntity Update(string positionid)
        {

            var updateSql = $@"update INS_PERSONPOSITION set uptime=sysdate   where positionid='{positionid}'";
            var rows = 0;

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    rows = conn.Execute(updateSql);

                    return MessageEntityTool.GetMessage(rows, null, true);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }

            }
        }

        public MessageEntity GetWorkingTimeDistance(string personid, DateTime startTime, DateTime endTime)
        {
            #region 条件
          string   sqlWhere= " where 1=1 ";
            if (startTime != null)
            {
                sqlWhere += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sqlWhere += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (personid != null && personid != "")
            {
                sqlWhere += $" and personid ='{personid}'";
            }
            #endregion
            string errorMsg;
            string query = $@"  select * from(select p.*,row_number() over ( partition by p.personid order by p.uptime asc) rn from INS_PERSONPOSITION p   {sqlWhere} ) where rn=1 
                                 union all
                                 select * from(select p.*,row_number() over ( partition by p.personid order by p.uptime desc) rn from INS_PERSONPOSITION p  {sqlWhere}  ) where rn=1
                                ";
            string distancesql = $@"  select  p.personid,sum(p.distance) distance from(select distinct p.personid,p.distance from INS_PERSONPOSITION p  {sqlWhere} )p group by p.personid";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_PersonPositionDto> result = conn.Query<Ins_PersonPositionDto>(query).ToList();
                    IList<Ins_PersonPositionDto> resultdistance = conn.Query<Ins_PersonPositionDto>(distancesql).ToList();
                    decimal Distance = 0;
                    if (resultdistance.Count > 0)
                    {
                        Distance = resultdistance[0].Distance;
                    }
                    List<Ins_PersonPositionDto> list = new List<Ins_PersonPositionDto>();
                    Ins_PersonPositionDto model = new Ins_PersonPositionDto();

                    if (result.Count > 0)
                    {
                        //第一条为签到上班时间，第二条为结束时间
                        DateTime startT = result[0].UpTime;
                        DateTime endT = result[1].UpTime;
                        if (result[1].GroupID == "1")
                            endT = DateTime.Now;
                        //计算两日期之间小时
                        model.Distance = Distance;
                        TimeSpan timeSpan = endT - startT;
                        var minutes = timeSpan.TotalMinutes;
                        model.WorkTime = ConvertToTime(minutes);
                        list.Add(model);
                    }
                    return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }
        /// <summary>
        /// 把分钟数转化成几小时几分钟（100 -> 01:40）
        /// </summary>
        /// <param name="minutes">待转化的分钟数</param>
        /// <returns>几小时几分钟</returns>
      public  string ConvertToTime(object minutes)
        {
            int M = 0;//待转化的分钟数；
            int h = 0;//小时
            int m = 0;//分钟

            try
            {
                if (minutes.ToString() != "")
                {
                    M = Convert.ToInt32(minutes);
                }
            }
            catch (Exception ex)
            {
               return ex.Message;
            }

            h = M / 60;
            m = M % 60;

            DateTime time = DateTime.Parse("00:00");
            time = time.AddHours(h).AddMinutes(m);

            string R = time.ToString("HH:mm");
            if (R == "00:00")
            {
                R = ""; ;
            }
            return R;

        }
    }
}
