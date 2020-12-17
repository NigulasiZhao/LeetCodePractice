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
    ///企业投标信息DAL层
    /// </summary>
    public class Ins_EnterprisetenderDAL : IIns_EnterprisetenderDAL
    {
        /// <summary>
        /// 添加企业投标信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Ins_Enterprisetender model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM Ins_Enterprisetender WHERE Tendercode = '{0}' ", model.Tendercode);
                int CodeCount = conn.ExecuteScalar<int>(GetDataSql);
                if (CodeCount > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "投标编号重复！");
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
        ///修改企业投标信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Ins_Enterprisetender model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                string GetDataSql = string.Format(@"SELECT Count(0) FROM Ins_Enterprisetender WHERE Tendercode = '{0}' AND ID <> '{1}'", model.Tendercode, model.Id);
                int CodeCount = conn.ExecuteScalar<int>(GetDataSql);
                if (CodeCount > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "投标编号重复！");
                }
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
        /// 删除企业投标信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Ins_Enterprisetender model)
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
        /// 根据ID获取企业投标信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ins_Enterprisetender GetInfo(string ID)
        {
            List<Ins_Enterprisetender> _ListField = new List<Ins_Enterprisetender>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Ins_Enterprisetender>("select * from Ins_Enterprisetender t where Id='" + ID + "'").ToList();
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
        /// 获得企业投标信息列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition, string SearchConditions)
        {
            string SearchConditionsSql = "";
            if (!string.IsNullOrEmpty(SearchConditions))
            {
                SearchConditionsSql = $" and ( Projectname like '%{SearchConditions}%' or Tendercode like '%{SearchConditions}%' or Tenderway like '%{SearchConditions}%')";
            }
            string sql = @"   select Creatordepartmentid,Creatordepartmentname,Creatorid,Creatorname,Creatortime,Enterpriseid,Id,Istender,Monitoringunit,Projectname,Tendercode,Tenderdate,Tendertiem,Tenderway,Totalamount from Ins_Enterprisetender ipc 
" + sqlCondition + SearchConditionsSql;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Enterprisetender>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

    }
}