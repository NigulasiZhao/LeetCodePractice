using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using Dapper;
using Dapper.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Database;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Internalexternal
{
    public class Ms_WorkOrderDAL : IMs_WorkOrderDAL
    {
        public MessageEntity Add(Ms_WorkOrder model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {

                StringBuilder SqlInsertEqu = new StringBuilder();
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

        public MessageEntity GetList()
        {
            string errorMsg = "";
            string sql = @"select w.workorderid,w.taskid,w.taskname ,w.handleperson,w.equipmentjson,w.uploadetime,case  w.ispostcomplete  when '0' then '未分派'  when '1' then '已分派'  when '2' then '已完成' else '未分派'  end as ispostcomplete from Ms_WorkOrder w order by w.uploadeTime desc";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ms_WorkOrder> eventType = conn.Query<Ms_WorkOrder>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetWorkOrderByTaskid(string taskId)
        {
            string errorMsg = "";
            string sql = $@"select * from Ms_WorkOrder where taskid='{taskId}'";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ms_WorkOrder> eventType = conn.Query<Ms_WorkOrder>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetWorkOrderByWorkorderid(string workorderid)
        {
            string errorMsg = "";
            string sql = $@"select case  ispostcomplete  when '0' then '未分派'  when '1' then '已分派'  when '2' then '已完成' else '未分派'  end as ispostcomplete from Ms_WorkOrder where workorderid='{workorderid}'";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ms_WorkOrder> eventType = conn.Query<Ms_WorkOrder>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity Update(string workorderid, string handlePerson, string isPostcomplete)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    string sql = $"update  Ms_WorkOrder set handleperson='{handlePerson}',ispostcomplete='{isPostcomplete}' where workorderid='{workorderid}'";
                    var rows = conn.Execute(sql);

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
