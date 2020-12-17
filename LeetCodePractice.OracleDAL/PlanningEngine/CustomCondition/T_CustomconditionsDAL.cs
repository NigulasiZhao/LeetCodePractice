/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/3 16:02:14
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using GISWaterSupplyAndSewageServer.Model;
using System.Text;
using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.CustomCondition
{
    /// <summary>
    ///用户查询条件DAL层
    /// </summary>
    public class T_CustomconditionsDAL : IT_CustomconditionsDAL
    {
        /// <summary>
        /// 添加用户查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(T_Customconditions model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.CSSDE))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                try
                {
                    string sqlText = $@"INSERT INTO T_Customconditions(ID,SEARCHTYPE,SEARCHNAME,USERID,CREATETIME,REMARK,SEARCHCONDITON) VALUES('{Guid.NewGuid().ToString()}','{model.Searchtype}','{model.Searchname}'
                                        ,'{model.Userid}',to_date('{model.Createtime}', 'yyyy-mm-dd hh24:mi:ss'),'{model.Remark}',:text)";
                    using (OracleCommand cmd = new OracleCommand(sqlText, (OracleConnection)conn))
                    {
                        OracleParameter oracleParameter = new OracleParameter("text", OracleDbType.Clob);
                        oracleParameter.Value = model.Searchconditon;
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
        ///修改用户查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(T_Customconditions model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.CSSDE))
            {
                var rows = 0;
                try
                {
                    string sqlText = $@"UPDATE T_Customconditions SET SEARCHTYPE = '{model.Searchtype}',SEARCHNAME= '{model.Searchname}',REMARK='{model.Remark}', SEARCHCONDITON = :text
                                        WHERE ID = '{model.Id}'";
                    using (OracleCommand cmd = new OracleCommand(sqlText, (OracleConnection)conn))
                    {
                        OracleParameter oracleParameter = new OracleParameter("text", OracleDbType.Clob);
                        oracleParameter.Value = model.Searchconditon;
                        cmd.Parameters.Add(oracleParameter);
                        rows = cmd.ExecuteNonQuery();
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
                //var insertSql = DapperExtentions.MakeUpdateSql(model);
                //if (string.IsNullOrEmpty(insertSql))
                //{
                //    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                //}
                //try
                //{
                //    rows = conn.Execute(insertSql, model);
                //    return MessageEntityTool.GetMessage(rows);
                //}
                //catch (Exception e)
                //{
                //    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                //}
            }
        }
        /// <summary>
        /// 删除用户查询条件
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(T_Customconditions model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.CSSDE))
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
        /// 根据ID获取用户查询条件
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T_Customconditions GetInfo(string ID)
        {
            List<T_Customconditions> _ListField = new List<T_Customconditions>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.CSSDE))
            {
                _ListField = conn.Query<T_Customconditions>("select * from T_Customconditions t where Id='" + ID + "'").ToList();
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
        /// 获得用户查询条件列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select Createtime,Id,Remark,Searchconditon,Searchname,Searchtype,Userid from T_Customconditions " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<T_Customconditions>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.CSSDE);
            return result;
        }

    }
}