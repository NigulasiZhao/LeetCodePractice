/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/10/10 15:15:28
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
    ///管线拆除、报废登记表DAL层
    /// </summary>
    public class Accounts_PipescrapdailyDAL : IAccounts_PipescrapdailyDAL
    {
        /// <summary>
        /// 添加管线拆除、报废登记表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Accounts_Pipescrapdaily model)
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
        ///修改管线拆除、报废登记表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Accounts_Pipescrapdaily model)
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
        /// 删除管线拆除、报废登记表
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Accounts_Pipescrapdaily model)
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
        /// 根据ID获取管线拆除、报废登记表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Accounts_Pipescrapdaily GetInfo(string ID)
        {
            List<Accounts_Pipescrapdaily> _ListField = new List<Accounts_Pipescrapdaily>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Accounts_Pipescrapdaily>("select Auditleader,Auditleaderqzdate,Auditreviewer,Auditreviewerqzdate,Companymanager,Companymanagerqzdate,Constructioncompany,Fillingcompany,Fillingdate,Financeagent,Financeagentqzdate,Financeleader,Financeleaderqzdate,Globid,Id,Installplace,Originalvalue,Pipechief,Primary_Completeddate,Primary_Projectcode,Primary_Projectreference,Projectcode,Projectname,Rangename,Reason,Scrapcaliber,Scrapdismantle,Scrapnum,Scrapyj,Scrapyz,Scrapzj,Sycompanyjbr,Sycompanyld,Sycompanyqzdate,Sycompanyyj,Systemtime,Unitprice,Zyinstallgg,Pipecode,Rangename_Fgs,Mi_Shape from Accounts_Pipescrapdaily t where Id='" + ID + "'").ToList();
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
        /// 获得管线拆除、报废登记表列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
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
            string sql = @"   select Auditleader,Auditleaderqzdate,Auditreviewer,Auditreviewerqzdate,Companymanager,Companymanagerqzdate,Constructioncompany,Fillingcompany,Fillingdate,Financeagent,Financeagentqzdate,Financeleader,Financeleaderqzdate,Globid,Id,Installplace,Originalvalue,Pipechief,Primary_Completeddate,Primary_Projectcode,Primary_Projectreference,Projectcode,Projectname,Rangename,Reason,Scrapcaliber,Scrapdismantle,Scrapnum,Scrapyj,Scrapyz,Scrapzj,Sycompanyjbr,Sycompanyld,Sycompanyqzdate,Sycompanyyj,Systemtime,Unitprice,Zyinstallgg,Pipecode,Rangename_Fgs,Mi_Shape from Accounts_Pipescrapdaily ipc 
" + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Accounts_Pipescrapdaily>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
        /// <summary>
        /// 获得管线拆除、报废登记统计表
        /// </summary>
        /// <param name="parInfo">参数信息</param>
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
                string sql = string.Format(@" select {0} ,COUNT(T.ID) AS ALLCOUNT from ACCOUNTS_PIPESCRAPDAILY t 
                             {1}
                            GROUP BY  {2}", groupByFields, sqlCondition, groupByFields);
                var ResultList = DapperExtentions.EntityForSqlToPager<dynamic>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
                return result;
            }
        }
    }
}