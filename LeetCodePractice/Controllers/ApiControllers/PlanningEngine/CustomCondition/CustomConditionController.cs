using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.CustomCondition;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.CustomCondition
{
    /// <summary>
    /// 用户自定义查询条件
    /// </summary>
    public class CustomConditionController : BaseController
    {
        private readonly IIns_CustomconditionsDAL _ins_CustomconditionsDAL;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ins_CustomconditionsDAL"></param>
        public CustomConditionController(IIns_CustomconditionsDAL ins_CustomconditionsDAL)
        {
            _ins_CustomconditionsDAL = ins_CustomconditionsDAL;
        }

        /// <summary>
        /// 保存自定义查询条件(Headers传递access_token)
        /// </summary>
        /// <param name="Model">userid,createtime,id不传</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity SaveCustomconditions([FromBody] Ins_Customconditions Model)
        {
            if (UniWaterUserInfo != null)
            {
                Model.Userid = UniWaterUserInfo._id;
                Model.Createtime = DateTime.Now;
                var result = _ins_CustomconditionsDAL.Add(Model);
                return result;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
        }
        /// <summary>
        /// 查询自定义查询条件(Headers传递access_token)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetCustomconditionsModel(string Functionid)
        {
            if (UniWaterUserInfo != null)
            {
                var result = _ins_CustomconditionsDAL.GetFirstCondition(UniWaterUserInfo._id, Functionid);
                return result;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "获取用户信息失败！");
            }
        }
        /// <summary>
        /// 修改自定义查询条件(Headers传递access_token)
        /// </summary>
        /// <param name="ID">主键ID(Id)</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Customconditions value)
        {
            if (UniWaterUserInfo != null)
            {
                if (value == null)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError);
                }
                value.Id = ID;
                value.Userid = UniWaterUserInfo._id;
                return _ins_CustomconditionsDAL.Update(value);
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
            var modeInfo = _ins_CustomconditionsDAL.GetInfo(ID);
            return _ins_CustomconditionsDAL.Delete(modeInfo);
        }
    }
}
