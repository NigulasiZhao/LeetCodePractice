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
    public class Ins_Plan_TemplatetypeDAL : IIns_Plan_TemplatetypeDAL
    {
        public MessageEntity Add(Ins_Plan_Templatetype model)
        {
            string Code = "MBFL0001";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetCodeSql = "SELECT Templatetype_code FROM Ins_Plan_Templatetype ORDER BY Templatetype_code DESC";
                Ins_Plan_Templatetype Ins_Plan_TemplatetypeModel = conn.Query<Ins_Plan_Templatetype>("SELECT Templatetype_code FROM Ins_Plan_Templatetype WHERE Templatetype_code LIKE 'MBFL%' ORDER BY Templatetype_code DESC").FirstOrDefault();
                if (Ins_Plan_TemplatetypeModel != null)
                {
                    if (Ins_Plan_TemplatetypeModel.Templatetype_code.Contains("MBFL"))
                    {
                        Code = "MBFL" + (Convert.ToInt32(Ins_Plan_TemplatetypeModel.Templatetype_code.Replace("MBFL", "")) + 1).ToString().PadLeft(4, '0');
                    }
                }
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                model.Templatetype_code = Code;
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

        public MessageEntity Delete(Ins_Plan_Templatetype model)
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
            string sql = @"   select * from Ins_Plan_Templatetype
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Plan_Templatetype>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public List<Ins_Plan_Templatetype> GetTree(string pID = "00000000-0000-0000-0000-000000000000")
        {
            string errorMsg = "";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    #region 条件
                    string sqlwhere = " where 1=1  ";
                    #endregion
                    string sql = "select plan_templatetype_id,templatetype_parentid,templatetype_name,templatetype_code from Ins_Plan_Templatetype t " + sqlwhere;
                    List<Ins_Plan_Templatetype> list = conn.Query<Ins_Plan_Templatetype>(sql).ToList();
                    return list;
                }
                catch (Exception e)
                {
                    errorMsg = e.Message;
                    return null;
                }
            }
        }

        public MessageEntity Update(Ins_Plan_Templatetype model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetCodeSql = string.Format("SELECT * FROM Ins_Plan_Templatetype WHERE Plan_templatetype_id = '{0}'", model.Plan_templatetype_id);
                Ins_Plan_Templatetype Ins_Plan_TemplatetypeModel = conn.Query<Ins_Plan_Templatetype>(GetCodeSql).FirstOrDefault();
                var rows = 0;
                model.Templatetype_code = Ins_Plan_TemplatetypeModel.Templatetype_code;
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
        public MessageEntity IsExistPlanTemplatetype(Ins_Plan_Templatetype planTemplatetype, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(planTemplatetype.Templatetype_parentid))
            {
                strWhere += $" and Templatetype_parentid ='{planTemplatetype.Templatetype_parentid}' ";

            }
            if (!string.IsNullOrEmpty(planTemplatetype.Templatetype_name))
            {
                strWhere += $" and ( Templatetype_name ='{planTemplatetype.Templatetype_name}')";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and Plan_templatetype_id <>'{planTemplatetype.Plan_templatetype_id}'";
            }
            string query = $@" select * from Ins_Plan_Templatetype where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Plan_Templatetype> result = conn.Query<Ins_Plan_Templatetype>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity IsExistPlanChildren(string plan_templatetype_id)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(plan_templatetype_id))
            {
                strWhere += $" and Templatetype_parentid ='{plan_templatetype_id}' ";

            }

            string query = $@" select * from Ins_Plan_Templatetype where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Plan_Templatetype> result = conn.Query<Ins_Plan_Templatetype>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
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
