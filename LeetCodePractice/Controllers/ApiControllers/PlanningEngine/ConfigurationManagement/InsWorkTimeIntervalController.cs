using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.ConfigurationManagement;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.ConfigurationManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.ConfigurationManagement
{
 /// <summary>
 /// 工作时段
 /// </summary>
    public class InsWorkTimeIntervalController : ControllerBase
    {
        private readonly IIns_WorkTimeIntervalDAL _ins_WorkTimeIntervalDAL;

        public InsWorkTimeIntervalController(IIns_WorkTimeIntervalDAL ins_WorkTimeIntervalDAL)
        {
            _ins_WorkTimeIntervalDAL = ins_WorkTimeIntervalDAL;
        }
        /// <summary>
        /// 获取工作时段列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "Start_Time", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_WorkTimeIntervalDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增工作时段
        /// </summary>
        /// <param name="value">Task_Name：任务名称；Start_Time：开始时间；End_Time：结束时间；Is_monitor：是否轨迹监控；remark：备注；</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_WorkTimeInterval value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _ins_WorkTimeIntervalDAL.Add(value);
        }
        /// <summary>
        /// 修改工作时段
        /// </summary>
        /// <param name="ID">主键ID(WorkTimeID)</param>
        /// <param name="value">Task_Name：任务名称；Start_Time：开始时间；End_Time：结束时间；Is_monitor：是否轨迹监控；remark：备注；</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_WorkTimeInterval value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.WorkTimeID = ID;
            return _ins_WorkTimeIntervalDAL.Update(value);
        }
        /// <summary>
        /// 删除工作时段
        /// </summary>
        /// <param name="ID">主键ID(WorkTimeID)</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _ins_WorkTimeIntervalDAL.Delete(new Ins_WorkTimeInterval { WorkTimeID = ID });
        }
    }
}