using Dapper;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.EquipmentFormsRelation;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.EquipmentFormsRelation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.EquipmentFormsRelation
{
    public class Ins_Equipment_FormsDAL : IIns_Equipment_FormsDAL
    {
        /// <summary>
        /// 添加关联关系
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(List<Ins_Equipment_Forms> model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                if (model.Count == 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    string DeleteSql = "DELETE Ins_Equipment_Forms WHERE layerName='" + model[0].LayerName + "'";
                    conn.Execute(DeleteSql, transaction);
                    foreach (var item in model)
                    {
                        item.ViewOrder = rows + 1;
                        rows += conn.Execute(DapperExtentions.MakeInsertSql(item), item, transaction);
                    }
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity Delete(Ins_Equipment_Forms model)
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

        public MessageEntity GetEquipmentCommboboxList()
        {
            string errorMsg = "";
            string query = " select distinct layername from Ins_Equipment_Forms  ieb  left join INS_CUSTOMIZE_FORMLIST c on ieb.tableid=c.tableid  where type=1";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_Equipment_Forms> list = conn.Query<Ins_Equipment_Forms>(query).ToList();

                    return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        /// <summary>
        /// 获得关联关系信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select ieb.Equipment_Form_Id,ieb.LayerName,ieb.layerCName,ieb.ViewOrder,ieb.TableID,c.tablename,c.tablecode from Ins_Equipment_Forms ieb  left join INS_CUSTOMIZE_FORMLIST c on ieb.tableid=c.tableid
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Equipment_FormsList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
    }
}
