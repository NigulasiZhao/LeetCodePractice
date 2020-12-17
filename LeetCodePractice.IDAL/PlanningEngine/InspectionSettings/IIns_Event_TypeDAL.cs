using GisPlateform.IDAL;

using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings
{
    public interface IIns_Event_TypeDAL : IDependency
    {
        MessageEntity GetEventType(string sort, string ordering, int num, int page);
        MessageEntity GetEventTypeCommboBoxList();
        MessageEntity GetEventTypeContent();
        MessageEntity AddEventType(Ins_Event_Type eventType);
        MessageEntity UpdateEventType(Ins_Event_Type eventType);
        MessageEntity DeleteEventType(Ins_Event_Type eventType);
        MessageEntity IsExistEventType(Ins_Event_Type eventType, int isAdd);
        MessageEntity IsExistEventContent(Ins_Event_Type eventType, int isAdd);
        MessageEntity IsExistEventcontent(string eventTypeId);
        DataTable GetEventExecTime(string eventTypeID);

        //  MessageEntity IsExistEvent(string eventTypeId);

        //事件内容
        MessageEntity GetEventContentInfo(string? eventTypeId, string sort, string ordering, int num, int page);
        MessageEntity IsExistEvent(string eventTypeId);


    }
}
