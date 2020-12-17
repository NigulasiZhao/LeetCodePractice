using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Plan
{
    public class Ins_Range_PointDAL : IIns_Range_PointDAL
    {
        public MessageEntity Add(Ins_Range_Point model)
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

        public MessageEntity Delete(Ins_Range_Point model)
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

        public MessageEntity GetList(string range_id)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(range_id))
            {
                strWhere += $" and range_id ='{range_id}' ";

            }
            string query = $@" select * from Ins_Range_Point where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Range_Point> result = conn.Query<Ins_Range_Point>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity IsExistRangePoint(Ins_Range_Point rangePoint, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(rangePoint.Range_id))
            {
                strWhere += $" and range_id ='{rangePoint.Range_id}' ";

            }
            if (!string.IsNullOrEmpty(rangePoint.Range_point_name))
            {
                strWhere += $" and Range_point_name ='{rangePoint.Range_point_name}' ";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and Range_point_id <>'{rangePoint.Range_point_id}'";
            }
            string query = $@" select * from Ins_Range_Point where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Range_Point> result = conn.Query<Ins_Range_Point>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity Update(Ins_Range_Point model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model);
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
    }
}
