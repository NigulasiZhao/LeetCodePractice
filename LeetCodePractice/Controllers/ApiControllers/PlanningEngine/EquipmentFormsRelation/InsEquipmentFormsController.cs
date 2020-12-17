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
    /// 关系挂接
    /// </summary>
    public class InsEquipmentFormsController : ControllerBase
    {
        private readonly IIns_Equipment_FormsDAL _ins_Equipment_BpmsDAL;

        public InsEquipmentFormsController(IIns_Equipment_FormsDAL ins_Equipment_BpmsDAL)
        {
            _ins_Equipment_BpmsDAL = ins_Equipment_BpmsDAL;
        }
        /// <summary>
        /// 新增关系挂接
        /// </summary>
        /// <param name="value">GroupName：英文名称；ViewOrder：编号；tableID：表单对应表名称；</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] List<Ins_Equipment_Forms> value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _ins_Equipment_BpmsDAL.Add(value);
        }
        /// <summary>
        /// 获取关系挂接列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}] type：挂接类型 1：计划引擎 2.台账</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "ViewOrder", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_Equipment_BpmsDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }

        /// <summary>
        /// 获取关系挂接设备名称下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetCommboboxList()
        {
            var messageEntity = _ins_Equipment_BpmsDAL.GetEquipmentCommboboxList();
            return messageEntity;
        }
        /// <summary>
        /// 删除设备挂接
        /// </summary>
        /// <param name="ID">主键ID(Equipment_form_id)</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _ins_Equipment_BpmsDAL.Delete(new Ins_Equipment_Forms { Equipment_Form_Id = ID });
        }
    }
}