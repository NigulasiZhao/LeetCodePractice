using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.EquipmentFormsRelation;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.EquipmentFormsRelation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.EquipmentFormsRelation
{
   /// <summary>
   /// 表单列表
   /// </summary>
    public class InsCustomizeFormlistController : ControllerBase
    {
        private readonly IIns_Customize_FormlistDAL _ins_Customize_FormlistDAL;

        public InsCustomizeFormlistController(IIns_Customize_FormlistDAL ins_Customize_FormlistDAL)
        {
            _ins_Customize_FormlistDAL = ins_Customize_FormlistDAL;
        }
        /// <summary>
        /// 获取表单列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]  查询参数 表单名称:tableName</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "tableName", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_Customize_FormlistDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
    }
    }