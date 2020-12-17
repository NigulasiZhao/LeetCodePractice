using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.InspectionSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Form
{
   public interface IIns_FormDAL : IDependency
    {
        MessageEntity PostFireHydrant(Ins_Form_FireHydrantModel model, Ins_Task_CompleteDetail taskdetailmode,string imagePath, string x, string y, string iadminid, string iadminame);
        MessageEntity PostValve(Ins_Form_ValveModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame);
        MessageEntity PostLeakDetection(Ins_Form_LeakDetectionModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame);
        MessageEntity PostStoreWater(Ins_Form_StoreWaterModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame, string upDeptId, string upDeptName);
        MessageEntity PostPump(Ins_Form_PumpModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame, string upDeptId, string upDeptName);
        MessageEntity PostPipelineequ(Ins_Form_PipelineequModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame, string upDeptId, string upDeptName);

    }
}
