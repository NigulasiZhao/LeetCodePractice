using System.Collections.Generic;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Newtonsoft.Json;
using GISWaterSupplyAndSewageServer.IDAL.Accounts;
using GISWaterSupplyAndSewageServer.Model.Accounts;
using GISWaterSupplyAndSewageServer.CommonTools;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Accounts
{

    /// <summary>
    /// 阀门台账
    /// </summary>   
    public class Account_ValveController : Controller
    {
        private readonly IAccount_Valve _valveDAL;
        /// <summary>
        /// 阀门台账
        /// </summary>
        public Account_ValveController(IAccount_Valve valveDAL)
        {
            _valveDAL = valveDAL;
        }

        /// <summary>
        /// 获取阀门台账
        /// </summary>
        /// <param name="ParInfo">阀门台账对象列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "ID", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _valveDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }

        /// <summary>
        /// 根据统计字段，条件等信息进行数据统计
        /// </summary>
        /// <param name="parInfo">where条件</param>
        /// <param name="fieldsInfo">分组字段列表</param>
        /// <param name="fieldsCount">统计字段</param>
        /// <returns>统计结果</returns>
        [HttpGet]
        public MessageEntity GetStatistics(string parInfo, string fieldsInfo, string fieldsCount)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(parInfo);
            List<string> list1 = JsonConvert.DeserializeObject<List<string>>(fieldsInfo);
            List<string> list2 = JsonConvert.DeserializeObject<List<string>>(fieldsCount);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _valveDAL.GetStatistics(list, list1, list2, sqlCondition);

            return result;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody]Gis_Account_Valve value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _valveDAL.Add(value);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody]Gis_Account_Valve value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _valveDAL.Update(value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _valveDAL.Delete(new Gis_Account_Valve { ID = ID });
        }
    }
}
