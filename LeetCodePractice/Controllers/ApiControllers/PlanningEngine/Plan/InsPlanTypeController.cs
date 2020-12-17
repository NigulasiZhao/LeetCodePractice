using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.Plan;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Plan
{
    /// <summary>
    /// 巡检类型管理
    /// </summary>
    public class InsPlanTypeController : ControllerBase
    {
        private readonly IIns_Plan_TypeDAL _ins_Plan_TypeDAL;
        private readonly IIns_PlanDAL _ins_PlanDAL;
        public InsPlanTypeController(IIns_Plan_TypeDAL ins_Plan_TypeDAL, IIns_PlanDAL ins_PlanDAL)
        {
            _ins_Plan_TypeDAL = ins_Plan_TypeDAL;
            _ins_PlanDAL = ins_PlanDAL;

        }
        /// <summary>
        /// 获取巡检类型列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "Plan_Type_Name", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_Plan_TypeDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增巡检类型
        /// </summary>
        /// <param name="value">Plan_Type_Name：类型名称 ParentTypeId：父节点id ； 根节点：0；Operater：操作人姓名 </param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Plan_Type value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_Type_Name = value.Plan_Type_Name.Replace(" ", "");
            if (string.IsNullOrEmpty(value.Plan_Type_Name))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            //添加名称重复判断
            MessageEntity messresult = _ins_Plan_TypeDAL.IsExistPlanType(value, 1);
            List<Ins_Plan_Type> ptslist = (List<Ins_Plan_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许添加重复名称！");
            }
            return _ins_Plan_TypeDAL.Add(value);
        }
        /// <summary>
        /// 修改巡检类型
        /// </summary>
        /// <param name="ID">主键ID(Plan_Type_Id)</param>
        /// <param name="value">Plan_Type_Name：类型名称 ParentTypeId：父节点id 根节点：0；Operater：操作人姓名</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Plan_Type value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_Type_Name = value.Plan_Type_Name.Replace(" ", "");
            if (string.IsNullOrEmpty(value.Plan_Type_Name))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_Type_Id = ID;
            //添加名称重复判断
            MessageEntity messresult = _ins_Plan_TypeDAL.IsExistPlanType(value, 0);
            List<Ins_Plan_Type> ptslist = (List<Ins_Plan_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许修改重复名称！");
            }
            return _ins_Plan_TypeDAL.Update(value);
        }
        /// <summary>
        /// 删除巡检类型
        /// </summary>
        /// <param name="ID">主键ID(Plan_Type_Id)</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            //删除计划类别之前判断有没有对应的计划，如果存在不允许删除
            MessageEntity messresult = _ins_PlanDAL.IsExistPlan(ID);
            List<Ins_Plan> ptslist = (List<Ins_Plan>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "请先删除对应计划后再进行删除！");
            }
            var modeInfo = _ins_Plan_TypeDAL.GetInfo(ID);
            return _ins_Plan_TypeDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 获取巡检类型及其区域信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetTreeList()
        {
            return _ins_Plan_TypeDAL.GetTreeList();
        }
    }
}