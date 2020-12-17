using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static GISWaterSupplyAndSewageServer.CommonTools.ChidrenTree;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Plan
{
   /// <summary>
   /// 计划模板大类
   /// </summary>
    public class InsPlanTemplateTypeController : ControllerBase
    {
        private readonly IIns_Plan_TemplatetypeDAL _ins_Plan_TemplatetypeDAL;

        public InsPlanTemplateTypeController(IIns_Plan_TemplatetypeDAL ins_Plan_TemplatetypeDAL)
        {
            _ins_Plan_TemplatetypeDAL = ins_Plan_TemplatetypeDAL;
        }
        /// <summary>
        /// 获取计划模板大类数据
        /// </summary>
        /// <param name="ParInfo">其他台账查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]查询参数名称Type 1:polygon 2:string</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "templatetype_code", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_Plan_TemplatetypeDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }

        /// <summary>
        /// 获取模板大类树形目录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetTree(string pID = "00000000-0000-0000-0000-000000000000")
        {
            ChidrenTree childtree = new ChidrenTree();
            List<Ins_Plan_Templatetype> list = _ins_Plan_TemplatetypeDAL.GetTree(pID);
            //List<TreeChildViewModel> treeViewModels = new List<TreeChildViewModel>();
            //treeViewModels = childtree.AddPlanTempChild(list, pID);
            List<TreeModel> treeViewModels = new List<TreeModel>();
            treeViewModels = childtree.ConversionList(list, pID, "Plan_templatetype_id", "Templatetype_parentid", "Templatetype_name", "Templatetype_code");
            return MessageEntityTool.GetMessage(treeViewModels.Count(), treeViewModels, true, "", treeViewModels.Count());
        }


        /// <summary>
        /// 新增模板大类
        /// </summary>
        /// <param name="value">Templatetype_code：模板编号 Templatetype_name：模板分类名称  Templatetype_parentid：父节点id 值为00000000-0000-0000-0000-000000000000代表根节点</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Plan_Templatetype value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            MessageEntity messresult = _ins_Plan_TemplatetypeDAL.IsExistPlanTemplatetype(value, 1);
            List<Ins_Plan_Templatetype> ptslist = (List<Ins_Plan_Templatetype>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许添加重复模板名称！");
            }
            return _ins_Plan_TemplatetypeDAL.Add(value);
        }

        /// <summary>
        /// 修改模板大类
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Plan_Templatetype value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_templatetype_id = ID;
            MessageEntity messresult = _ins_Plan_TemplatetypeDAL.IsExistPlanTemplatetype(value, 0);
            List<Ins_Plan_Templatetype> ptslist = (List<Ins_Plan_Templatetype>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许修改为重复模板名称！");
            }
            return _ins_Plan_TemplatetypeDAL.Update(value);
        }

        /// <summary>
        /// 删除模板大类
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            //删除首先判断是否存在子节点
            MessageEntity messresult = _ins_Plan_TemplatetypeDAL.IsExistPlanChildren(ID);
            List<Ins_Plan_Templatetype> ptslist = (List<Ins_Plan_Templatetype>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许删除存在子模板数据，请先删除子节点数据！");
            }
            return _ins_Plan_TemplatetypeDAL.Delete(new Ins_Plan_Templatetype { Plan_templatetype_id = ID });
        }
    }
}