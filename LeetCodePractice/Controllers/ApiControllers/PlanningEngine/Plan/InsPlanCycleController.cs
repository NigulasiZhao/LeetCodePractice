using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.Plan;
using GISWaterSupplyAndSewageServer.Model.Plan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Plan
{
    /// <summary>
    /// 计划周期
    /// </summary>
    public class InsPlanCycleController : ControllerBase
    {
        private readonly IIns_Plan_CycleDAL _ins_Plan_CycleDAL;

        public InsPlanCycleController(IIns_Plan_CycleDAL ins_Plan_CycleDAL)
        {
            _ins_Plan_CycleDAL = ins_Plan_CycleDAL;
        }
        /// <summary>
        /// 获取计划周期列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "Plan_Cycle_Name", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_Plan_CycleDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增计划周期
        /// </summary>
        /// <param name="value">Plan_Cycle_Name：计划周期名称；CycleTime：周期时长，如1；CycleUnit：单位：天，周，月；CycleHz：频率；DeleteState：是否删除：0未删除，1删除</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Plan_Cycle value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _ins_Plan_CycleDAL.Add(value);
        }
        /// <summary>
        /// 修改计划周期
        /// </summary>
        /// <param name="ID">主键ID(Plan_Cycle_Id)</param>
        /// <param name="value">Plan_Cycle_Name：计划周期名称；CycleTime：周期时长，如1；CycleUnit：单位：天，周，月；CycleHz：频率；DeleteState：是否删除：0未删除，1删除</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Plan_Cycle value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_Cycle_Id = ID;
            return _ins_Plan_CycleDAL.Update(value);
        }
        /// <summary>
        /// 删除计划周期
        /// </summary>
        /// <param name="ID">主键ID(Plan_Cycle_Id)</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _ins_Plan_CycleDAL.GetInfo(ID);
            return _ins_Plan_CycleDAL.Delete(modeInfo);
        }
    }
}