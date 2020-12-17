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
    /// 设备属性的下拉列表的值
    /// </summary>
    public class EquipmentPorpertyValueController : Controller
    {
        private readonly IEquipmentPorpertyDAL _equipmentPorpertyDAL;
        private readonly IEquipmentPorpertyValueDAL _equipmentPorpertyValueDAL;



        public EquipmentPorpertyValueController(IEquipmentPorpertyDAL equipmentPorpertyDAL, IEquipmentPorpertyValueDAL equipmentPorpertyValueDAL)
        {
            _equipmentPorpertyDAL = equipmentPorpertyDAL;
            _equipmentPorpertyValueDAL = equipmentPorpertyValueDAL;
        }
        /// <summary>
        /// 获取设备属性下拉框列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "VIEWORDER", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _equipmentPorpertyValueDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增设备属性下拉框配置
        /// </summary>
        /// <param name="value"></param>````````````````````````
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] EquipmentPorpertyValue value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            //判断是否重复
            string selectionValue = "'" + value.selectionValue + "'";
            MessageEntity messresult = _equipmentPorpertyValueDAL.GetValueInfo(value.EquipmentPorpertyId, value.ID, selectionValue, 1);
            List<EquipmentPorpertyValue> ptslist = (List<EquipmentPorpertyValue>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不能添加相同名称！");
            }
            return _equipmentPorpertyValueDAL.Add(value);
        }

        /// <summary>
        /// 修改设备属性下拉框配置
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] EquipmentPorpertyValue value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.EquipmentPorpertyId = ID;
            //判断是否重复
            string IDs = "'" + value.ID + "'";
            string selectionValue = "'" + value.selectionValue + "'";
            MessageEntity messresult = _equipmentPorpertyValueDAL.GetValueInfo(value.EquipmentPorpertyId, IDs, selectionValue, 1);
            List<EquipmentPorpertyValue> ptslist = (List<EquipmentPorpertyValue>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不能修改为已存在名称！");
            }
            return _equipmentPorpertyValueDAL.Update(value);
        }

        /// <summary>
        /// 删除设备属性下拉框配置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _equipmentPorpertyValueDAL.Delete(new EquipmentPorpertyValue { ID = ID });
        }
    }
}
