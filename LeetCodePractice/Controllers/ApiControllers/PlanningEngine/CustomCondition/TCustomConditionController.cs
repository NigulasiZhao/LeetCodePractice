using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.CustomCondition;
using GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.CustomCondition;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.CustomCondition
{
    /// <summary>
    /// 用户查询条件
    /// </summary>
    public class TCustomConditionController : BaseController
    {
        private readonly IT_CustomconditionsDAL _t_CustomconditionsDAL;

        public TCustomConditionController(IT_CustomconditionsDAL t_CustomconditionsDAL)
        {
            _t_CustomconditionsDAL = t_CustomconditionsDAL;
        }
        /// <summary>
        /// 获取用户查询条件列表(Headers传递access_token)
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "Createtime", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            if (UniWaterUserInfo != null)
            {
                list.Add(new ParameterInfo
                {
                    ParName = "Userid",
                    ParValue = UniWaterUserInfo._id,
                    DataType = "string",
                    LinkType = "and"
                });
            }
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _t_CustomconditionsDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增用户查询条件(Headers传递access_token)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] T_Customconditions value)
        {
            if (UniWaterUserInfo != null)
            {
                if (value == null)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError);
                }
                value.Userid = UniWaterUserInfo._id;
                value.Createtime = DateTime.Now;
                return _t_CustomconditionsDAL.Add(value);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
        }
        /// <summary>
        /// 修改用户查询条件(Headers传递access_token)
        /// </summary>
        /// <param name="ID">主键ID(Plan_Cycle_Id)</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] T_Customconditions value)
        {
            if (UniWaterUserInfo != null)
            {
                if (value == null)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError);
                }
                value.Id = ID;
                value.Userid = UniWaterUserInfo._id;
                return _t_CustomconditionsDAL.Update(value);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
        }
        /// <summary>
        /// 删除用户查询条件
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _t_CustomconditionsDAL.GetInfo(ID);
            return _t_CustomconditionsDAL.Delete(modeInfo);
        }
    }
}
