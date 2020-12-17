using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.Plan;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Plan
{
    /// <summary>
    /// 计划模板设备关联bpm管理
    /// </summary>
    public class InsPlanEquipmentFormController : ControllerBase
    {
        private readonly IIns_Plan_EquipmentFormDAL _ins_Plan_EquipmentFormDAL;

        public InsPlanEquipmentFormController(IIns_Plan_EquipmentFormDAL ins_Plan_EquipmentFormDAL)
        {
            _ins_Plan_EquipmentFormDAL = ins_Plan_EquipmentFormDAL;
        }



        /// <summary>
        /// 新增设备关联bpm
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Plan_EquipmentForm value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }

            return _ins_Plan_EquipmentFormDAL.Add(value);
        }

      

        /// <summary>
        /// 删除设备关联bpm
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _ins_Plan_EquipmentFormDAL.Delete(new Ins_Plan_EquipmentForm { Planttemplate_id = ID });
        }
    }
}