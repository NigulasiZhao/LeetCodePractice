using System.Collections.Generic;
using System.Linq;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.Equipments;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Equipments
{
    /// <summary>
    /// 设备属性
    /// </summary> 
    public class EquipmentPorpertyController : Controller
    {
        private readonly IEquipmentPorpertyDAL _equipmentPorpertyDAL;
        private readonly IEquipmentPorpertyValueDAL _equipmentPorpertyValueDAL;

        public EquipmentPorpertyController(IEquipmentPorpertyDAL equipmentPorpertyDAL, IEquipmentPorpertyValueDAL equipmentPorpertyValueDAL)
        {
            _equipmentPorpertyDAL = equipmentPorpertyDAL;
            _equipmentPorpertyValueDAL = equipmentPorpertyValueDAL;
        }
        /// <summary>
        /// 获取设备属性列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "InputType", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _equipmentPorpertyDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增设备属性配置
        /// </summary>
        /// <param name="value"></param>````````````````````````
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] EquipmentPorperty value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            //判断是否重复
            List<string> FieldIDS = value.EquipmentPorpertyValueGroup.Select(Row => Row.ID).ToList();
            List<string> selectionValuelist = value.EquipmentPorpertyValueGroup.Select(Row => Row.selectionValue).ToList();
            string ids = "'" + string.Join("','", FieldIDS.ToArray()) + "'";
            string selectionValues = "'" + string.Join("','", selectionValuelist.ToArray()) + "'";
            //删除区域之前首先判断，该区域下是否存在计划，如果存在不允许删除
            MessageEntity messresult = _equipmentPorpertyValueDAL.GetValueInfo(value.EquipmentPorpertyId, ids, selectionValues, 1);
            List<EquipmentPorpertyValue> ptslist = (List<EquipmentPorpertyValue>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不能修改为已存在名称！");
            }
            return _equipmentPorpertyDAL.Add(value);
        }
        /// <summary>
        /// 修改设备属性配置
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] EquipmentPorperty value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.EquipmentPorpertyId = ID;
            //判断是否重复
            List<string> FieldIDS = value.EquipmentPorpertyValueGroup.Select(Row => Row.ID).ToList();
            List<string> selectionValuelist = value.EquipmentPorpertyValueGroup.Select(Row => Row.selectionValue).ToList();
            string ids = "'" + string.Join("','", FieldIDS.ToArray()) + "'";
            string selectionValues = "'" + string.Join("','", selectionValuelist.ToArray()) + "'";
            //删除区域之前首先判断，该区域下是否存在计划，如果存在不允许删除
            MessageEntity messresult = _equipmentPorpertyValueDAL.GetValueInfo(value.EquipmentPorpertyId, ids, selectionValues, 0);
            List<EquipmentPorpertyValue> ptslist = (List<EquipmentPorpertyValue>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不能修改为已存在名称！");
            }
            return _equipmentPorpertyDAL.Update(value);
        }
        /// <summary>
        /// 删除设备属性配置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _equipmentPorpertyDAL.GetInfo(ID);
            return _equipmentPorpertyDAL.Delete(modeInfo);
        }
    }
}
