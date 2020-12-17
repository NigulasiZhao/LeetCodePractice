using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.UniWater;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Plan
{
    /// <summary>
    /// 计划模板管理
    /// </summary>
    public class InsPlantemplateController : BaseController
    {
        private readonly IIns_PlanTemplateDAL _ins_PlanTemplateDAL;

        public InsPlantemplateController(IIns_PlanTemplateDAL ins_PlanTemplateDAL)
        {
            _ins_PlanTemplateDAL = ins_PlanTemplateDAL;
        }
        ///// <summary>
        ///// 获取计划模板
        ///// </summary>
        ///// <param name="ParInfo">其他台账查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]查询参数名称  模板大类:templatetype_name 模板名称:planttemplate_name 创建人：create_person_name  更新人:update_person_name</param>
        ///// <param name="sort"></param>
        ///// <param name="ordering"></param>
        ///// <param name="num"></param>
        ///// <param name="page"></param>
        ///// <returns></returns>
       // [HttpGet]
        //public MessageEntity Get(string ParInfo, string sort = "create_time", string ordering = "desc", int num = 20, int page = 1)
        //{
        //    List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
        //    SqlCondition sqlWhere = new SqlCondition();
        //    string sqlCondition = sqlWhere.getParInfo(list);
        //    var result = _ins_PlanTemplateDAL.Get(list, sort, ordering, num, page, sqlCondition);
        //    return result;
        //}
        /// <summary>
        /// 获取计划模板（包含设备信息与自定义表单）
        /// </summary>
        /// <param name="ParInfo">其他台账查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetList(string ParInfo, string sort = "create_time", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_PlanTemplateDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }


        /// <summary>
        /// 新增计划模板(Headers传递access_token)
        /// </summary>
        /// <param name="value">Plan_templatetype_id：模板分类 PlantTemplate_name：模板名称</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Plantemplate value)
        {
            //string access_token = HttpContext.Request.Headers["access_token"];
            //LogHelper.Info("access_token:" + access_token + "--Headers" + HttpContext.Request.Headers);

            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            
            value.PlantTemplate_name = value.PlantTemplate_name.Replace(" ", "");
            MessageEntity messresult = _ins_PlanTemplateDAL.IsExistPlanTemplate(value, 1);
            List<Ins_Plantemplate> ptslist = (List<Ins_Plantemplate>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许添加重复计划模板名称！");
            }
           
            if (UniWaterUserInfo !=null)
            {
                value.Create_person_id = UniWaterUserInfo._id;
                value.Create_person_name = UniWaterUserInfo.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "access_token失效，获取用户信息失败！");
            }
            //同种分类下不能存在重复
            return _ins_PlanTemplateDAL.Add(value);
        }
        /// <summary>
        /// 修改计划模板
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Plantemplate value)
        {

            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Planttemplate_id = ID;
            //同种分类下不能存在重复
            value.PlantTemplate_name = value.PlantTemplate_name.Replace(" ", "");
            MessageEntity messresult = _ins_PlanTemplateDAL.IsExistPlanTemplate(value,0);
            List<Ins_Plantemplate> ptslist = (List<Ins_Plantemplate>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许修改为已存在的计划模板名称!");
            }
            if (UniWaterUserInfo != null)
            {
                value.Update_person_id = UniWaterUserInfo._id;
                value.Update_person_name = UniWaterUserInfo.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "access_token失效，获取用户信息失败！");
            }
            return _ins_PlanTemplateDAL.Update(value);
        }


        /// <summary>
        /// 删除计划模板
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {

            var modeInfo = _ins_PlanTemplateDAL.GetInfo(ID);
            return _ins_PlanTemplateDAL.Delete(modeInfo);
        }
    }
}