using Dapper;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Database;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GisPlateformForCore.OracleDAL.PlanningEngine.InspectionSettings
{
    public class Ins_Event_LeakDAL : IIns_Event_LeakDAL
    {
        public MessageEntity Post(Ins_Event_Leak m_Event)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(m_Event);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql, m_Event);
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
