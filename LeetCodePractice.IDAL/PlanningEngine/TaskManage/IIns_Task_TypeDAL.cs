using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.TaskManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage
{
    public interface IIns_Task_TypeDAL : IDependency
    {
        
        MessageEntity Add(Ins_Task_Type model);
        MessageEntity Delete(Ins_Task_Type model);
        List<Ins_Task_Type> GetTree(string pID = "00000000-0000-0000-0000-000000000000");
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Update(Ins_Task_Type model);
        MessageEntity IsExistTaskType(Ins_Task_Type model, int isAdd);
        MessageEntity IsExistTaskTypeChildren(string task_Type_id);

    }
}
