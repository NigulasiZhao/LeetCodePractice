using System.Collections.Generic;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.GisConfigSetting
{
    /// <summary>
    /// 图层类别管理管理
    /// </summary>
    public class GIS_LayerTypeController : Controller
    {

        private readonly IGIS_LayerTypeDAL _LayerTypeDAL;

        public GIS_LayerTypeController(IGIS_LayerTypeDAL LayerTypeDAL)
        {
            _LayerTypeDAL = LayerTypeDAL;
        }

        /// <summary>
        /// 获取GIS图层类型列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "CreatedTime", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _LayerTypeDAL.GetList(list, sort, ordering, num, page,sqlCondition);
            return result;
        }
        /// <summary>
        /// 获取图层类型列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetLayerType(string ParInfo)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _LayerTypeDAL.GetLayerTypeList(sqlCondition);
            return result;
        }

        /// <summary>
        /// 新增图层类别
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] GIS_LayerType value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }

            return _LayerTypeDAL.Add(value);
        }

        /// <summary>
        /// 修改图层类别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] GIS_LayerType value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _LayerTypeDAL.Update(value);
        }

        /// <summary>
        /// 删除图层类别
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _LayerTypeDAL.GetInfo(ID);
            return _LayerTypeDAL.Delete(modeInfo);
        }
    }
}
