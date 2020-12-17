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
    public class Ins_Plan_EquipmentFormDAL : IIns_Plan_EquipmentFormDAL
    {
        public MessageEntity Add(Ins_Plan_EquipmentForm model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    string DeleteSql = "DELETE Ins_Plan_EquipmentForm WHERE Planttemplate_id='" + model.Planttemplate_id + "'";
                    rows = conn.Execute(DeleteSql, transaction);
                    rows = conn.Execute(DapperExtentions.MakeInsertSql(model), model, transaction);

                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity Delete(Ins_Plan_EquipmentForm model)
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
    }
}
