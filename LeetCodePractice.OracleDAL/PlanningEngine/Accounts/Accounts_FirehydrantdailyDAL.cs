/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/9/28 9:49:33
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

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine
{
    /// <summary>
    ///消火栓日常巡查记录表DAL层
    /// </summary>
    public class Accounts_FirehydrantdailyDAL : IAccounts_FirehydrantdailyDAL
    {
        /// <summary>
        /// 添加消火栓日常巡查记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Accounts_Firehydrantdaily model)
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
        /// <summary>
        ///修改消火栓日常巡查记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Accounts_Firehydrantdaily model)
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
        /// 删除消火栓日常巡查记录表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Accounts_Firehydrantdaily model)
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
        /// 根据ID获取消火栓日常巡查记录表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Accounts_Firehydrantdaily GetInfo(string ID)
        {
            List<Accounts_Firehydrantdaily> _ListField = new List<Accounts_Firehydrantdaily>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Accounts_Firehydrantdaily>("select Firehydrantaddress,Firehydrantcode,Firehydrantvender,Id,Iscomplete,Isdirty,Isfillbrand,Isfixedbrand,Isheight,Islackconnect,Isleakwater,Ismisscover,Isold,Isshort,Istilt,Isunlocation,Otherqueation,Outwatertime,Photos,Pressure,Rangename,Remark,Systemtime,Globid,Rangename_Fgs,Mi_Shape  from Accounts_Firehydrantdaily t where Id='" + ID + "'").ToList();
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
        /// 获得消火栓日常巡查记录表列表
        /// </summary>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql, string mi_Shape)
        {
            if (!string.IsNullOrEmpty(searchSql))
            {
                sqlCondition = sqlCondition + " and " + searchSql;
            }
            if (!string.IsNullOrEmpty(mi_Shape))
            {
                sqlCondition += $" and sde.st_intersects (shape,(select sde.st_geometry('{mi_Shape}',4547) from dual))=1 and sde.st_isempty(shape) = 0 ";
            }
                string sql = @"   select Firehydrantaddress,Firehydrantcode,Firehydrantvender,Id,Iscomplete,Isdirty,Isfillbrand,Isfixedbrand,Isheight,Islackconnect,Isleakwater,Ismisscover,Isold,Isshort,Istilt,Isunlocation,Otherqueation,Outwatertime,Photos,Pressure,Rangename,Remark,Systemtime,Globid,Rangename_Fgs,Mi_Shape from Accounts_Firehydrantdaily ipc 
" + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Accounts_Firehydrantdaily>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
        /// <summary>
        /// 获得高风险管段巡查记录统计表
        /// </summary>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetCountList(string sort, string ordering, int num, int page, string sqlCondition, string searchSql, string groupByFields, string mi_Shape)
        {
            if (!string.IsNullOrEmpty(searchSql))
            {
                sqlCondition = sqlCondition + " and " + searchSql;
            }
            if (!string.IsNullOrEmpty(mi_Shape))
            {
                sqlCondition += $" and sde.st_intersects (shape,(select sde.st_geometry('{mi_Shape}',4547) from dual))=1 and sde.st_isempty(shape) = 0 ";
            }
            if (string.IsNullOrEmpty(groupByFields))
            {
                return new MessageEntity();
            }
            else
            {
                string sql = string.Format(@" select {0} ,COUNT(T.ID) AS ALLCOUNT  from ACCOUNTS_FIREHYDRANTDAILY t 
                            {1}
                            GROUP BY {2}", groupByFields, sqlCondition, groupByFields);
                var ResultList = DapperExtentions.EntityForSqlToPager<dynamic>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
                return result;
            }
        }
    }
}