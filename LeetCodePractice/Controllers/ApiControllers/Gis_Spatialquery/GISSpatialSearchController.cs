using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.Gis_Spatialquery;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Gis_Spatialquery
{
    /// <summary>
    /// GIS分区空间查询接口
    /// </summary>
    public class GISSpatialSearchController : Controller
    {
        private readonly ISpatialSearchDAL _spatialSearchDAL;

        public GISSpatialSearchController(ISpatialSearchDAL spatialSearchDAL)
        {
            _spatialSearchDAL = spatialSearchDAL;

        }
        /// <summary>
        /// 获取一级分区空间信息
        /// </summary>
        /// <param name="parInfo">查询条件 [{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetFirstzoneShape(string parInfo)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(parInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            return _spatialSearchDAL.GetFirstzoneShape(list, sqlCondition);
        }
        /// <summary>
        /// 获取二级分区空间信息
        /// </summary>
        /// <param name="parInfo">查询条件 [{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetSeconozoneShape(string parInfo)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(parInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            return _spatialSearchDAL.GetSeconozoneShape(list, sqlCondition);
        }
        /// <summary>
        /// 获取网格空间信息
        /// </summary>
        /// <param name="parInfo">查询条件 [{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetUnitShape(string parInfo)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(parInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            return _spatialSearchDAL.GetUnitShape(list, sqlCondition);
        }
    }
}
