using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GisPlateform.CommonTools;
using GisPlateform.Model;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISServerForCore2._0.Controllers.ApiControllers.PlanningEngine.InspectionSettings
{
    /// <summary>
    /// 历史事件记录
    /// </summary>
    public class InsEventHistoryController : ControllerBase
    {
        private readonly IIns_EventHistoryDAL _iIns_EventHistoryDAL;
        public InsEventHistoryController(IIns_EventHistoryDAL iIns_EventHistoryDAL)
        {
            _iIns_EventHistoryDAL = iIns_EventHistoryDAL;

        }  /// <summary>
           ///  获取历史上报事件
           /// </summary>
           /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition|startTime|endTime",LinkType:and|or}]upTime ：上报时间   eventTypeId：事件类型id</param>
           /// <param name="sort"></param>
           /// <param name="ordering"></param>
           /// <param name="num"></param>
           /// <param name="page"></param>
           /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo,string sort = "IsFinish", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iIns_EventHistoryDAL.Get(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
    }
}