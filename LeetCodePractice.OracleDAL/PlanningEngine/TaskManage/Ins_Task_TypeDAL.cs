using Dapper;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.TaskManage
{
    public class Ins_Task_TypeDAL : IIns_Task_TypeDAL
    {

        public MessageEntity Add(Ins_Task_Type model)
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

        public MessageEntity Delete(Ins_Task_Type model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM INS_TASK WHERE TASK_TYPEID = '{0}'", model.Task_type_id);
                int DataRows = conn.ExecuteScalar<int>(GetDataSql);
                if (DataRows > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "已关联巡检任务数据不允许删除");
                }
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
            string sql = @"   select * from Ins_Task_Type
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Task_Type>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public List<Ins_Task_Type> GetTree(string pID = "00000000-0000-0000-0000-000000000000")
        {
            string errorMsg = "";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    #region 条件
                    string sqlwhere = " where 1=1  ";
                    #endregion
                    string sql = "select t.task_type_id,t.task_type_name,t.parenttypeid,t.task_type_code from Ins_Task_Type t " + sqlwhere;
                    List<Ins_Task_Type> list = conn.Query<Ins_Task_Type>(sql).ToList();
                    return list;
                }
                catch (Exception e)
                {
                    errorMsg = e.Message;
                    return null;
                }
            }
        }

        public MessageEntity Update(Ins_Task_Type model)
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
        public MessageEntity IsExistTaskType(Ins_Task_Type model, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            //if (!string.IsNullOrEmpty(model.ParentTypeId))
            //{
            //    strWhere += $" and ParentTypeId ='{model.ParentTypeId}' ";

            //}
            if (!string.IsNullOrEmpty(model.Task_type_name))
            {
                strWhere += $" and ( Task_type_name ='{model.Task_type_name}' or Task_type_code ='{model.Task_type_code}')";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and Task_type_id <>'{model.Task_type_id}'";
            }
            string query = $@" select * from Ins_Task_Type where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Task_Type> result = conn.Query<Ins_Task_Type>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity IsExistTaskTypeChildren(string task_Type_id)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(task_Type_id))
            {
                strWhere += $" and ParentTypeId ='{task_Type_id}' ";

            }

            string query = $@" select * from Ins_Task_Type where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Task_Type> result = conn.Query<Ins_Task_Type>(query).ToList();
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
