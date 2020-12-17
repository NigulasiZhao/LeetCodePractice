using Dapper;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Database;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace GisPlateformForCore.OracleDAL.PlanningEngine.InspectionSettings
{
    public class EventStartDAL : IEventStartDAL
    {
        public MessageEntity GetEventFromComboBoxList()
        {
            string errorMsg = "";
            string query = " select EventFromId,EventFromName from Ins_EventFrom";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_EventFrom> eventType = conn.Query<Ins_EventFrom>(query).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity GetUrgencyComboBoxList()
        {
            string errorMsg = "";
            string query = " select Urgent_Level_Id,UrgencyName from Ins_Urgent_Level";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<Ins_Urgent_Level> eventType = conn.Query<Ins_Urgent_Level>(query).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
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
