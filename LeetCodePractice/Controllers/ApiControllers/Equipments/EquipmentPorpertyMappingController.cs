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
    /// 点位/管线对应表操作
    /// </summary>
    public class EquipmentPorpertyMappingController : Controller
    {
        private readonly IEquipmentPorpertyMappingDAL _equipmentPorpertyMappingDAL;


        public EquipmentPorpertyMappingController(IEquipmentPorpertyMappingDAL equipmentPorpertyMappingDAL)
        {
            _equipmentPorpertyMappingDAL = equipmentPorpertyMappingDAL;
        }
        /// <summary>
        /// 获取点位/管线对应表列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "EQUIPMENTID", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _equipmentPorpertyMappingDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增 点位/管线
        /// </summary>
        /// <param name="value"></param>````````````````````````
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] EquipmentPorpertyMapping value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            //判断是否重复
            MessageEntity messresult = _equipmentPorpertyMappingDAL.GetValueInfo(value.EquipmentPorpertyId, value.EquipmentId, value.ID, 1);
            List<EquipmentPorpertyMapping> ptslist = (List<EquipmentPorpertyMapping>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不能添加相同名称！");
            }
            return _equipmentPorpertyMappingDAL.Add(value);
        }

        /// <summary>
        /// 修改 点位/管线
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] EquipmentPorpertyMapping value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            //判断是否重复
            MessageEntity messresult = _equipmentPorpertyMappingDAL.GetValueInfo(value.EquipmentPorpertyId, value.EquipmentId, ID, 1);
            List<EquipmentPorpertyMapping> ptslist = (List<EquipmentPorpertyMapping>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不能修改为已存在名称！");
            }
            return _equipmentPorpertyMappingDAL.Update(value);
        }

        /// <summary>
        /// 删除 点位/管线
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _equipmentPorpertyMappingDAL.Delete(new EquipmentPorpertyMapping { ID = ID });
        }
    }
}
