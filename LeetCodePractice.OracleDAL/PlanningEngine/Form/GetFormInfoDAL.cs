using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Form;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Form
{
    public class GetFormInfoDAL : IGetFormInfoDAL
    {
        public MessageEntity Get(string plan_task_id, string tableId)
        {
            string errorMsg = "";
            string query =$" select * from {tableId} where plan_task_id='{plan_task_id}'";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    if (tableId == "Ins_Form_Valve")
                    {
                        List<Ins_Form_Valve> list = conn.Query<Ins_Form_Valve>(query).ToList();
                        return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());

                    }
                    else if (tableId == "Ins_Form_FireHydrant")
                    {
                        List<Ins_Form_FireHydrant> list = conn.Query<Ins_Form_FireHydrant>(query).ToList();
                        return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());

                    }
                    else if (tableId == "Ins_Form_LeakDetection")
                    {
                        List<Ins_Form_LeakDetection> list = conn.Query<Ins_Form_LeakDetection>(query).ToList();
                        return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());

                    }
                    else if (tableId == "Ins_Form_StoreWater")
                    {
                        List<Ins_Form_StoreWater> list = conn.Query<Ins_Form_StoreWater>(query).ToList();
                        return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());

                    }
                    else if (tableId == "Ins_Form_PipelineEqu")
                    {
                        List<Ins_Form_Pipelineequ> list = conn.Query<Ins_Form_Pipelineequ>(query).ToList();
                        return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());

                    }
                    else if (tableId == "Ins_Form_Pump")
                    {
                        List<Ins_Form_Pump> list = conn.Query<Ins_Form_Pump>(query).ToList();
                        return MessageEntityTool.GetMessage(list.Count(), list, true, "", list.Count());

                    }
                    else
                    {
                        return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"无填单数据", "提示");
                    }

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
