using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.InspectionSettings.Statistics;
using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using System.Data;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.InspectionSettings.Statistics
{
    public class Ins_EventStatisticsDAL : IIns_EventStatisticsDAL
    {
        public MessageEntity GetEventFromEchart(DateTime? startTime, DateTime? endTime)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (startTime != null)
            {
                sqlwhere +=$" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')" ;
            }
            if (endTime != null)
            {
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1);
                sqlwhere += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            #endregion
            string query = string.Format(@" 
select  A.EventFromId  Id , C.EventFromName Name ,count(0) sumcount
FROM            ins_event  A LEFT OUTER JOIN
               ins_eventfrom  C ON A.EventFromId = C.EventFromId where a.DeleteStatus=0
               {0}
        GROUP BY A.EventFromId,C.EventFromName", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EchartDto> eventType = conn.Query<EchartDto>(query).ToList();

                    var rows = 0;
                    foreach (EchartDto item in eventType)
                    {
                        rows += item.SumCount;
                    }
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", rows);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetEventOperEchart(DateTime? startTime, DateTime? endTime)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (startTime != null)
            {
                sqlwhere += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sqlwhere += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            #endregion
            string query = string.Format(@"select operid Id, opername Name ,count(0) sumcount from(
select  case HH.operid  when 11 then '待处理' when 8 then '已完成' else '处理中' end as  opername ,case HH.operid  when 11 then 1 when 8 then 2 else 3 end as  operid 
FROM            ins_event  A LEFT OUTER JOIN
             (   select h.eventid, h1.execpersonid,h1.execpersonname,h1.execdetpid,h1.execdetptname,h1.OperId,o.opername,o.opername2 from 
               (SELECT MAX(opertime) AS ExecUpDateTime,h.EventID
                                 FROM ins_workorder_oper_history h  GROUP BY h.EventID)  h LEFT OUTER JOIN ins_workorder_oper_history h1 ON h.EventID = h1.EventID AND h.ExecUpDateTime =h1.ExecUpDateTime 
                left outer join ins_workorder_oper o on h1.operid=o.operid) HH on hh.eventid=a.eventid where a.DeleteStatus=0
{0}
   ) a
        GROUP BY operid,opername
        ", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EchartDto> eventType = conn.Query<EchartDto>(query).ToList();

                    var rows = 0;
                    foreach (EchartDto item in eventType)
                    {
                        rows += item.SumCount;
                    }
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", rows);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetEventTypeDT(DateTime? startTime, DateTime? endTime)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (startTime != null)
            {
                sqlwhere += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sqlwhere += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            #endregion
            string query = string.Format(@" select a.Eventtypeid , Eventtypename  ,Eventtypename2 ,SL ,E.XJ,round((nvl(SL,0)/XJ)*100,1)||'%' bili
from(  select  A.Eventtypeid , C.Eventtypename  ,C1.Eventtypename Eventtypename2,count(0) sl
                    FROM            ins_event  A  LEFT  JOIN ins_event_type  C ON A.Eventtypeid = C.Event_Type_Id 
                                                  LEFT  JOIN ins_event_type  C1 ON A.Eventtypeid2 = C1.Event_Type_Id   
                                                  where a.DeleteStatus=0 
                              {0}   GROUP BY  A.Eventtypeid , C.Eventtypename  ,C1.Eventtypename
                               )A
                           left join (select e.Eventtypeid ,count(0) XJ from ins_event e where e.DeleteStatus=0   {0}  group by e.Eventtypeid) e on e.Eventtypeid=A.Eventtypeid  ORDER BY Eventtypeid", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<InsEventStaticDto> eventType = conn.Query<InsEventStaticDto>(query).ToList();
                    int Amount = eventType.Sum(t => t.SL);
                    eventType.ForEach(row =>
                    {
                        double percent = Convert.ToDouble(row.XJ) / Convert.ToDouble(Amount);
                        string result = percent.ToString("0.0%");//得到5.882%
                       row.BiliAll= result; 
                    });

                        return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", eventType.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetEventTypeEchart(DateTime? startTime, DateTime? endTime)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (startTime != null)
            {
                sqlwhere += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                sqlwhere += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            #endregion
            string query = string.Format(@"   
select  A.Eventtypeid Id, C.Eventtypename Name ,count(0) sumcount
FROM            ins_event  A LEFT OUTER JOIN
               ins_event_type  C ON A.Eventtypeid = C.Event_Type_Id where a.DeleteStatus=0  
   {0}
        GROUP BY Eventtypeid,Eventtypename", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EchartDto> eventType = conn.Query<EchartDto>(query).ToList();

                    var rows = 0;
                    foreach (EchartDto item in eventType)
                    {
                        rows += item.SumCount;
                    }
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", rows);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetEventTypeTrendTable(string[] monthArry, string yearStr, string startMStr, string endMStr)
        {
            var startTime = yearStr + "-" + ChangeStr(startMStr) + "-" + "01";
            string singleSTime = yearStr + "-" + ChangeStr(startMStr);//例如：2015-11
            var endTime = yearStr + "-" + ChangeStr((Convert.ToInt32(endMStr) + 1).ToString()) + "-" + "01";
            if ((Convert.ToInt32(endMStr) + 1).ToString() == "13")
            {
                endTime = (Convert.ToInt32(yearStr) + 1).ToString() + "-01-01";
            }
            string singetETime = yearStr + "-" + ChangeStr(endMStr);//例如：2015-12
            //string[] monthArry = months;
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn();
            col.ColumnName = "事件类型";
            dt.Columns.Add(col);
            DataColumn coll = new DataColumn();
            coll.ColumnName = "类型";
            dt.Columns.Add(coll);
            DataColumn colll = new DataColumn();
            colll.ColumnName = "小计";
            dt.Columns.Add(colll);
            for (int i = 0; i < monthArry.Length; i++)
            {
                dt.Columns.Add(new DataColumn(monthArry[i]));
            }
            //所有月份所欲事件总计

            //1 先获取事件表中所有的事件类型
            var sqlStr = string.Format("select e.EventTypeId,et.EventTypeName,count(e.EventTypeId) XiaoJi from Ins_Event e left join ins_event_type et on e.EventTypeId=et.Event_Type_Id WHERE e.DeleteStatus=0 and UpTime>=to_date('{0}', 'yyyy-mm-dd hh24:mi:ss') AND UpTime<to_date('{1}', 'yyyy-mm-dd hh24:mi:ss')  GROUP BY e.EventTypeId,et.EventTypeName", startTime, endTime);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    DataTable dt_type = new DataTable();
                    dt_type.Load(conn.ExecuteReader(sqlStr));

                    foreach (DataRow dr_type in dt_type.Rows)
                    {
                        //获取时间段内所有事件记录的数量
                        sqlStr = string.Format("select count(e.EventTypeId) XiaoJi from Ins_Event e left join ins_event_type et on e.EventTypeId=et.Event_Type_Id WHERE e.DeleteStatus=0  and UpTime>=to_date('{0}', 'yyyy-mm-dd hh24:mi:ss') AND UpTime<to_date('{1}', 'yyyy-mm-dd hh24:mi:ss')  ", startTime, endTime);
                        DataTable dtAll = new DataTable();
                        dtAll.Load(conn.ExecuteReader(sqlStr));
                        int allCount = int.Parse(dtAll.Rows[0]["XiaoJi"].ToString());
                        //数量
                        DataRow dr = dt.NewRow();
                        dr["事件类型"] = dr_type["EventTypeName"];
                        dr["类型"] = "数量";
                        //比例
                        DataRow dr_bili = dt.NewRow();
                        dr_bili["事件类型"] = dr_type["EventTypeName"];
                        dr_bili["类型"] = "比例";

                        int typeCount = 0;//此事件类型小计    
                        int typeTimeCount = 0;//总数
                                              //2 循环所有月份  获取对应月份 对应类型 在表中的数量
                        for (int j = 0; j < monthArry.Length; j++)
                        {
                            typeTimeCount = 0;
                            string nowMonth = GetSelectTime(yearStr, monthArry[j]);
                         
                            string startSelectTime = nowMonth + "-01";
                            string endSelectTime =DateTime.Parse(startSelectTime).AddMonths(1).ToString("yyyy-MM-dd");
                            sqlStr = string.Format("select COUNT(EventTypeId) singleMonthNum from Ins_Event WHERE DeleteStatus=0 and UpTime>=to_date('{0}', 'yyyy-mm-dd hh24:mi:ss') AND UpTime<to_date('{1}', 'yyyy-mm-dd hh24:mi:ss') and EventTypeId='{2}' ", startSelectTime, endSelectTime, dr_type["EventTypeId"]);

                            DataTable dt_singleMonth = new DataTable();
                            dt_singleMonth.Load(conn.ExecuteReader(sqlStr));
                            string singleMonthNum = "0";
                            if (dt_singleMonth.Rows.Count > 0)
                            {
                                singleMonthNum = dt_singleMonth.Rows[0]["singleMonthNum"].ToString();
                            }
                            singleMonthNum = singleMonthNum == "0" ? "" : dt_singleMonth.Rows[0]["singleMonthNum"].ToString();
                            dr[monthArry[j]] = singleMonthNum;
                            if (dt_singleMonth.Rows.Count > 0)
                            {
                                typeCount += int.Parse(dt_singleMonth.Rows[0]["singleMonthNum"].ToString());
                            }
                            else { typeCount = 0; }
                            //获取对应月份所有类型的总和
                            sqlStr = string.Format("select count(*) typeTimeNum from Ins_Event where DeleteStatus=0 and UpTime>=to_date('{0}', 'yyyy-mm-dd hh24:mi:ss') AND UpTime<to_date('{1}', 'yyyy-mm-dd hh24:mi:ss')  ", startSelectTime, endSelectTime);

                            DataTable dtTypeTime = new DataTable();
                            dtTypeTime.Load(conn.ExecuteReader(sqlStr));
                            //分母不正确
                            //
                            //
                            if (dtTypeTime.Rows.Count > 0)
                                typeTimeCount += int.Parse(dtTypeTime.Rows[0]["typeTimeNum"].ToString());

                            float typeTimeFloat = 0;
                            if (dt_singleMonth.Rows.Count > 0)
                            {
                                typeTimeFloat = (float)(int.Parse(dt_singleMonth.Rows[0]["singleMonthNum"].ToString())) / typeTimeCount * 100;

                                if (int.Parse(dt_singleMonth.Rows[0]["singleMonthNum"].ToString()) != 0)
                                {
                                    dr_bili[monthArry[j]] = Math.Round(typeTimeFloat, 1) + "%";//当前类型月份查询结果 和 当前月份查询结果 之比
                                }
                                else
                                {
                                    dr_bili[monthArry[j]] = "";
                                }
                            }
                            else
                            {
                                dr_bili[monthArry[j]] = "";
                            }

                        }
                        string typeCountStr = typeCount == 0 ? "" : typeCount.ToString();
                        dr["小计"] = typeCountStr;
                        dt.Rows.Add(dr);
                        float jieguo = (float)typeCount / allCount * 100;
                        dr_bili["小计"] = jieguo == 0 ? "" : Math.Round(jieguo, 1) + "%";
                        dt.Rows.Add(dr_bili);
                    }
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }

            return MessageEntityTool.GetMessage(dt.Rows.Count, dt);
        }
        private string GetSelectTime(string yearStr, string monthStr)
        {
            string monthNum = "";
            switch (monthStr)
            {
                case "1月":
                    monthNum = "01";
                    break;
                case "2月":
                    monthNum = "02";
                    break;
                case "3月":
                    monthNum = "03";
                    break;
                case "4月":
                    monthNum = "04";
                    break;
                case "5月":
                    monthNum = "05";
                    break;
                case "6月":
                    monthNum = "06";
                    break;
                case "7月":
                    monthNum = "07";
                    break;
                case "8月":
                    monthNum = "08";
                    break;
                case "9月":
                    monthNum = "09";
                    break;
                case "10月":
                    monthNum = "10";
                    break;
                case "11月":
                    monthNum = "11";
                    break;
                case "12月":
                    monthNum = "12";
                    break;
                default:
                    break;
            }
            return yearStr + "-" + monthNum;
        }
        //一位变两位
        private string ChangeStr(string s)
        {
            if (s.Length == 1)
            {
                s = "0" + s;
            }
            return s;
        }

        public MessageEntity GetUserReportTable(DateTime? startTime, DateTime? endTime)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (startTime != null)
            {
                sqlwhere += $" and UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1);
                sqlwhere += $" and UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            #endregion
            string query = string.Format(@"select   A.Upname Name, count(0) sumcount
FROM            ins_event  A   where a.DeleteStatus = 0
            {0}
            GROUP BY Upname", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EchartDto> eventType = conn.Query<EchartDto>(query).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", eventType.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetEventByRangName(DateTime? startTime, DateTime? endTime, string rangName)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (startTime != null)
            {
                sqlwhere += $" and a.UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1);
                sqlwhere += $" and a.UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (rangName != null && rangName != "")
            {
                sqlwhere += $" and a.rangName ='{rangName}'";
            }
            #endregion
            string query = string.Format(@" select  a.rangname , count(0) sumcount,count(aa.eventid) successcount
FROM            ins_event  A  
               left join (select aa.eventid,aa.rangname from ins_event aa where aa.deletestatus=0 and aa.isfinish=1) aa on aa.eventid=a.eventid
               where a.deletestatus=0 {0}
        GROUP BY a.rangname
        ", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EventStaticDto> eventType = conn.Query<EventStaticDto>(query).ToList();
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", eventType.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetEventEchartByRangeDate(DateTime? startTime, DateTime? endTime, string rangName)
        {
            string errorMsg = "";
            #region 条件
            string sqlwhere = " ";
            if (startTime != null)
            {
                sqlwhere += $" and e.UpTime>=to_date('{startTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                endTime = DateTime.Parse(endTime.ToString()).AddDays(1);
                sqlwhere += $" and e.UpTime<=to_date('{endTime}', 'yyyy-mm-dd hh24:mi:ss')";
            }
            if (rangName != null && rangName != "")
            {
                sqlwhere += $" and e.rangName ='{rangName}'";
            }
            #endregion
            string query = string.Format($@"         
        select jdl.ym,jdl,wcl from (
                      select  to_char(e.uptime,'YYYY-MM') ym ,case count(e.eventid) when 0 then 0 else round((count(hh.eventid)/count(e.eventid))*100,2)  end as jdl from ins_event e  
                                                           left join (   select h.eventid,h1.OperId from 
                                                                            (SELECT MAX(opertime) AS opertime,h.EventID
                                                                                       FROM ins_workorder_oper_history h  GROUP BY h.EventID)  h
                                                                                        LEFT OUTER JOIN ins_workorder_oper_history h1 ON h.EventID = h1.EventID AND h.opertime =h1.opertime
                                                                              where  h1.operId not in( 11,2)
                                                                       )  HH on hh.eventid=e.eventid where e.deletestatus=0  {sqlwhere} group by  to_char(e.uptime,'YYYY-MM')
                      ) jdl left join (
                                   
                                        select to_char(e.uptime,'YYYY-MM') ym, case count(e.eventid) when 0 then 0 else  round((count(ee.eventid)/count(e.eventid))*100,2)  end as wcl from ins_event e   
                                              left join (select e.eventid from ins_event e  where e.deletestatus=0  and e.isfinish=1  {sqlwhere}
                                              ) ee on ee.eventid=e.eventid where e.deletestatus=0 {sqlwhere} group by  to_char(e.uptime,'YYYY-MM') 
                                       )wcl on jdl.ym=wcl.ym      order by ym asc
        
        ", sqlwhere);
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EventStaticDto> eventType = conn.Query<EventStaticDto>(query).ToList();
                    return MessageEntityTool.GetMessage(eventType.Count, eventType, true, "", eventType.Count);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }
    }
}
