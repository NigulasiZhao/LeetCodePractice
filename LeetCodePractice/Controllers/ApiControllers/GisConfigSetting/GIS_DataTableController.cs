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
    ///  GIS基础数据结构配置表
    /// </summary>
    public class GIS_DataTableController : Controller
    {
        private readonly IGIS_DataTableDAL _masterTableDAL;
       
        public GIS_DataTableController(IGIS_DataTableDAL masterTableDAL)
        {
            _masterTableDAL = masterTableDAL;
        }

        /// <summary>
        /// 获取要素主表信息
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "CREATEDTIME", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _masterTableDAL.GetList(list, sort, ordering, num, page,sqlCondition);
            return result;
        }


        /// <summary>
        /// 新增基础数据配置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] GIS_DataTable value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }

            return _masterTableDAL.Add(value);
        }

        /// <summary>
        /// 修改基础数据配置
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] GIS_DataTable value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _masterTableDAL.Update(value);
        }

        /// <summary>
        /// 删除基础数据配置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _masterTableDAL.GetInfo(ID);
            return _masterTableDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 取得地图配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private MessageEntity GetDataTable()
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>("[]");
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
           var result= _masterTableDAL.GetList(list, "CREATEDTIME", "desc", 10000, 1, sqlCondition);
            List<GIS_DataTable> ResultList = result.Data.Result as List<GIS_DataTable>;// 反转结果数据信息
           var result1= _masterTableDAL.GetDataTable(ResultList);
            return result1;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileFormat">文件格式，默认.txt</param>
        /// <param name="fileName">下载文件名 默认基础数据配置列表</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity DownloadFile(string fileFormat = ".txt", string fileName = "基础数据配置列表")
        {

            MessageEntity result = GetDataTable();
            Dictionary<string, object> ResultList = result.Data.Result as Dictionary<string, object>;// 反转结果数据信息
            string json = JsonConvert.SerializeObject(ResultList);
            UploadTxt ut = new UploadTxt();
            var uploadurl = ut.WriteTxt(json, fileName, fileFormat);
            return MessageEntityTool.GetMessage(1, null, true, uploadurl);
        }
    }
}
