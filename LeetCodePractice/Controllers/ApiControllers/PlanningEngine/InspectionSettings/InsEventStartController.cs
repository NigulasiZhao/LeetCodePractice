using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using Microsoft.AspNetCore.Mvc;

namespace GISServerForCore2._0.Controllers.ApiControllers.PlanningEngine.InspectionSettings
{
   /// <summary>
   /// 事件来源  紧急程度
   /// </summary>
    public class InsEventStartController : ControllerBase
    {
        private readonly IEventStartDAL _iEventStartDAL;
        public InsEventStartController(IEventStartDAL iEventStartDAL)
        {
            _iEventStartDAL = iEventStartDAL;

        }
        /// <summary>
        /// 紧急程度下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetUrgencyComboBoxList()
        {
            return _iEventStartDAL.GetUrgencyComboBoxList();
        }
        /// <summary>
        /// 事件来源下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventFromComboBoxList()
        {
            return _iEventStartDAL.GetEventFromComboBoxList();
        }
    }
}