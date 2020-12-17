/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/2 17:04:16
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using System.Linq;
using GISWaterSupplyAndSewageServer.Model;
using Dapper.Oracle;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.CustomCondition
{
    /// <summary>
    ///用户自定义查询条件DAL层
    /// </summary>
    public class Ins_CustomconditionsDAL : IIns_CustomconditionsDAL
    {
        /// <summary>
        /// 添加用户自定义查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Ins_Customconditions model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                try
                {
                    string sqlText = $@"INSERT INTO Ins_Customconditions(Id,Functionid,Userid,Createtime,ConditionName,Conditiondetail) VALUES('{Guid.NewGuid().ToString()}','{model.Functionid}','{model.Userid}',to_date('{model.Createtime}', 'yyyy-mm-dd hh24:mi:ss'),'{model.ConditionName}',:text)";
                    using (OracleCommand cmd = new OracleCommand(sqlText, (OracleConnection)conn))
                    {
                        OracleParameter oracleParameter = new OracleParameter("text", OracleDbType.Clob);
                        oracleParameter.Value = model.Conditiondetail;
                        cmd.Parameters.Add(oracleParameter);
                        rows = cmd.ExecuteNonQuery();
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        ///修改用户自定义查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Ins_Customconditions model)
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
        /// <summary>
        /// 删除用户自定义查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Ins_Customconditions model)
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
        /// <summary>
        /// 根据ID获取用户自定义查询条件
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ins_Customconditions GetInfo(string ID)
        {
            List<Ins_Customconditions> _ListField = new List<Ins_Customconditions>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Ins_Customconditions>("select * from Ins_Customconditions t where Id='" + ID + "'").ToList();
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
        /// <summary>
        /// 获得用户自定义查询条件列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select Conditiondetail,Createtime,Functionid,Id,Userid from Ins_Customconditions  
" + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Customconditions>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        /// <summary>
        /// 获得用户自定义查询条件
        /// </summary>
        /// <param name="parInfo">参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetFirstCondition(string UserId, string Functionid)
        {
            string sql = string.Format(@"   select Conditiondetail,Createtime,Functionid,Id,Userid,ConditionName from Ins_Customconditions where Userid = '{0}' and Functionid = '{1}' ORDER BY Createtime DESC ", UserId, Functionid);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                List<Ins_Customconditions> list = conn.Query<Ins_Customconditions>(sql).ToList();
                if (list != null)
                {
                    return MessageEntityTool.GetMessage(list.Count, list);
                }
                else
                {
                    return MessageEntityTool.GetMessage(0);
                }
            }
        }
    }
}