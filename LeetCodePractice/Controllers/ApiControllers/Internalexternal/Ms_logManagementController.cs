using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Newtonsoft.Json;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;
using System.Data;
using System.Text;
using GISWaterSupplyAndSewageServer.IDAL.Internalexternal;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Internalexternal
{
    /// <summary>
    /// 日志管理(内外业)
    /// </summary>

    public class Ms_logManagementController : Controller
    {
        private readonly IMs_logManagementDAL _ms_logManagement;

        public Ms_logManagementController(IMs_logManagementDAL ms_logManagement)
        {
            _ms_logManagement = ms_logManagement;
        }
        /// <summary>
        /// 获取日志记录信息(OPERATIONTYPE 1.登陆2.菜单点击3.设备编辑4.管线编辑5.文件导入 6:内业系统派发)
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition|startTime|endTime",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "OPERATORTIME", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ms_logManagement.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增日志记录
        /// </summary>
        /// <param name="operationType">操作类型1.登陆(operatorName登陆于operatorTime)2.菜单点击(operatorName点击newValue菜单于operatorTime)3.设备编辑(operatorName将operationField从oldValue改为newValue于operatorTime)4.管线编辑5.文件导入(operatorName将newValue导入于operatorTime)</param>
        /// <param name="operatorId">操作人id</param>
        /// <param name="operatorName">操作人姓名</param>
        /// <param name="operationField">操作字段</param>
        /// <param name="newValue">新值</param>
        /// <param name="oldValue">旧值</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post(int operationType, string operatorId, string operatorName, string operationField, string newValue = "", string oldValue = "")
        {
            Ms_logManagement value = new Ms_logManagement();
            value.operationType = operationType;
            value.operatorId = operatorId;
            value.operatorName = operatorName;
            value.newValue = newValue;
            value.oldValue = oldValue;
            value.operationField = operationField;
            return _ms_logManagement.Add(value);
        }
    }
}

