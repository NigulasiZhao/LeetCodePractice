using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    ///  GIS基础数据结构字段列表
    /// </summary>
    public class GIS_DataTableColumnController : Controller
    {
        private readonly IGIS_DataTableColumnDAL _DataTableColumnDAL;

        public GIS_DataTableColumnController(IGIS_DataTableColumnDAL DataTableColumnDAL)
        {
            _DataTableColumnDAL = DataTableColumnDAL;
        }

        /// <summary>
        /// 获取GIS数据结构字段列表
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "orderno", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _DataTableColumnDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }


        /// <summary>
        /// 新增数据结构字段
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] GIS_DataTableColumn value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }

            return _DataTableColumnDAL.Add(value);
        }

        /// <summary>
        /// 修改数据结构字段
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] GIS_DataTableColumn value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _DataTableColumnDAL.Update(value);
        }

        /// <summary>
        /// 删除数据结构字段
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _DataTableColumnDAL.GetInfo(ID);

            return _DataTableColumnDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileFormat">文件格式，默认.txt</param>
        /// <param name="fileName">下载文件名 默认GIS数据结构字段列表</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity DownloadFile(string fileFormat = ".txt", string fileName= "GIS数据结构字段列表")
        {
         
            MessageEntity result =  Get("[]", "orderno", "asc", 10000, 1);
            List<GIS_DataTableColumn> ResultList = result.Data.Result as List<GIS_DataTableColumn>;// 反转结果数据信息
            string json = JsonConvert.SerializeObject(ResultList);
            UploadTxt ut = new UploadTxt();
            var uploadurl= ut.WriteTxt(json, fileName, fileFormat);
            return MessageEntityTool.GetMessage(1, null, true, uploadurl);
        }
     
    }
}
