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
 /// 移动终端
 /// </summary>
    public class InsMoveTerminalController : ControllerBase
    {
        private readonly IIns_MoveTerminalDAL _ins_MoveTerminalDAL;

        public InsMoveTerminalController(IIns_MoveTerminalDAL ins_MoveTerminalDAL)
        {
            _ins_MoveTerminalDAL = ins_MoveTerminalDAL;
        }
        /// <summary>
        /// 获取移动终端列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "purchaseDate", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_MoveTerminalDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增移动终端
        /// </summary>
        /// <param name="value">Operating_system：操作系统；PurchaseDate：购买日期；EquipmentName：购买设备名称；EquipmentType：设备类型；DepartmentId：部门id；DepartmentName：部门名称；PersonID：人员id；Personname：人员名称；</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_MoveTerminal value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _ins_MoveTerminalDAL.Add(value);
        }
        /// <summary>
        /// 修改移动终端
        /// </summary>
        /// <param name="ID">主键ID(MoveId)</param>
        /// <param name="value">Operating_system：操作系统；PurchaseDate：购买日期；EquipmentName：购买设备名称；EquipmentType：设备类型；DepartmentId：部门id；DepartmentName：部门名称；PersonID：人员id；Personname：人员名称；</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_MoveTerminal value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.MoveId = ID;
            return _ins_MoveTerminalDAL.Update(value);
        }
        /// <summary>
        /// 删除移动终端
        /// </summary>
        /// <param name="ID">主键ID(MoveId)</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _ins_MoveTerminalDAL.Delete(new Ins_MoveTerminal { MoveId = ID });
        }
    }
}