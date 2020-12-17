using System;
using System.Collections.Generic;
using System.Text;
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
    /// FeatureLayer图层管理类
    /// </summary>
    public class GIS_FeatureLayerCollectionController : Controller
    {
        private readonly IGIS_FeatureLayerCollectionDAL _FeatureLayerCollectionDAL;

        public GIS_FeatureLayerCollectionController(IGIS_FeatureLayerCollectionDAL LayerTypeDAL)
        {
            _FeatureLayerCollectionDAL = LayerTypeDAL;
        }

        /// <summary>
        /// 获取FeatureLayer列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "OrderNO", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _FeatureLayerCollectionDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }


        /// <summary>
        /// 新增Featurelayer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] GIS_FeatureLayerCollection value)
        {
            //byte[] byts = new byte[System.Web.HttpContext.Current.Request.InputStream.Length];
            //System.Web.HttpContext.Current.Request.InputStream.Read(byts, 0, byts.Length);
            //string reqStr = System.Text.Encoding.Default.GetString(byts);
            //reqStr = System.Web.HttpUtility.UrlDecode(reqStr, Encoding.UTF8);

            //string str = Convert.ToBase64String(byts);
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _FeatureLayerCollectionDAL.Add(value);
        }

        /// <summary>
        /// 修改FeatureLayer
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] GIS_FeatureLayerCollection value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _FeatureLayerCollectionDAL.Update(value);
        }

        /// <summary>
        /// 删除FeatureLayer
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _FeatureLayerCollectionDAL.Delete(new GIS_FeatureLayerCollection { ID = ID });
        }
    }
}
