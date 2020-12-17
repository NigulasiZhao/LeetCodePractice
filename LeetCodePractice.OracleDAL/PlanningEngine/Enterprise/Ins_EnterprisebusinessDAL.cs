/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/11/25 11:52:38
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Enterprise
{
    /// <summary>
    ///企业业务信息DAL层
    /// </summary>
    public class Ins_EnterprisebusinessDAL : IIns_EnterprisebusinessDAL
    {
        /// <summary>
        /// 添加企业业务信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Ins_Enterprisebusiness model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM Ins_Enterprisebusiness WHERE Checkcode = '{0}'", model.Checkcode);
                int CodeCount = conn.ExecuteScalar<int>(GetDataSql);
                if (CodeCount > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "考评编号重复！");
                }
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
        /// <summary>
        ///修改企业业务信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Ins_Enterprisebusiness model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM Ins_Enterprisebusiness WHERE Checkcode = '{0}' AND ID <> '{1}'", model.Checkcode, model.Id);
                int CodeCount = conn.ExecuteScalar<int>(GetDataSql);
                if (CodeCount > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "考评编号重复！");
                }
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
        /// 删除企业业务信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Ins_Enterprisebusiness model)
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
        /// 根据ID获取企业业务信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ins_Enterprisebusiness GetInfo(string ID)
        {
            List<Ins_Enterprisebusiness> _ListField = new List<Ins_Enterprisebusiness>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Ins_Enterprisebusiness>("select * from Ins_Enterprisebusiness t where Id='" + ID + "'").ToList();
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
        /// 获得企业业务信息列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition, string SearchConditions)
        {
            string SearchConditionsSql = "";
            if (!string.IsNullOrEmpty(SearchConditions))
            {
                SearchConditionsSql = $" and ( Projectname like '%{SearchConditions}%' or Checkcode like '%{SearchConditions}%' or Checkcontent like '%{SearchConditions}%')";
            }
            string sql = @"   select Address,Checkcode,Checkcontent,Checkdate,Checkscore,Checkunit,Creatordepartmentid,Creatordepartmentname,Creatorid,Creatorname,Creatortime,Enterpriseid,Id,Monitoringunit,Projectname from Ins_Enterprisebusiness ipc 
" + sqlCondition + SearchConditionsSql;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Enterprisebusiness>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

    }
}