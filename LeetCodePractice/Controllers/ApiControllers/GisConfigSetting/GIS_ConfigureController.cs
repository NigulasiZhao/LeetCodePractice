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
  /// GIS地图配置
  /// </summary>
    public class GIS_ConfigureController : Controller
    {
        private readonly IGIS_ConfigureDAL _ConfigureDAL;

        public GIS_ConfigureController(IGIS_ConfigureDAL LayerTypeDAL)
        {
            _ConfigureDAL = LayerTypeDAL;
        }

        /// <summary>
        /// 获取GIS地图配置列表
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
            var result = _ConfigureDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }

        /// <summary>
        /// 取得地图配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetMapConfig()
        {
            var result = _ConfigureDAL.GetMapConfig();
            return result;
        }

        /// <summary>
        /// 新增地图配置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] GIS_Configure value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _ConfigureDAL.Add(value);
        }

        /// <summary>
        /// 修改地图配置
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] GIS_Configure value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _ConfigureDAL.Update(value);
        }

        /// <summary>
        /// 删除地图配置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _ConfigureDAL.GetInfo(ID);
            return _ConfigureDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 下载地图整体配置
        /// </summary>
        /// <param name="fileFormat">文件格式，默认.txt</param>
        /// <param name="fileName">下载文件名 默认地图整体配置列表</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity DownloadFile(string fileFormat = ".txt", string fileName = "GIS地图整体配置列表")
        {

            MessageEntity result = GetMapConfig();
            Dictionary<string, object> ResultList = result.Data.Result as Dictionary<string, object>;// 反转结果数据信息
            string json = JsonConvert.SerializeObject(ResultList);
            UploadTxt ut = new UploadTxt();
            var uploadurl = ut.WriteTxt(json, fileName, fileFormat);
            return MessageEntityTool.GetMessage(1, null, true, uploadurl);
        }
    }
}
