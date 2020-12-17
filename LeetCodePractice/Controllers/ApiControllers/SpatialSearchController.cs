using GISWaterSupplyAndSewageServer.AttributePack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using GISWaterSupplyAndSewageServer.IDAL.Gis_Spatialquery;
using GISWaterSupplyAndSewageServer.CommonTools;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using System.Text;
using NPOI.SS.Formula.Functions;
using GISWaterSupplyAndSewageServer.IDAL.Gis_Spatialquery;

namespace GISWaterSupplyAndSewageServer.Controllers
{
    /// <summary>
    /// GIS空间查讯接口
    /// </summary>
    public class SpatialSearchController : Controller
    {
        private readonly IPipeDAL _pipeDAL;
        private readonly ISpatialSearchDAL _spatialSearchDAL;
        private readonly IWebHostEnvironment _webHostEnvironment;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeDAL"></param>
        /// <param name="spatialSearchDAL"></param>

        public SpatialSearchController(IPipeDAL pipeDAL, ISpatialSearchDAL spatialSearchDAL, IWebHostEnvironment webHostEnvironment)
        {
            _pipeDAL = pipeDAL;
            _spatialSearchDAL = spatialSearchDAL;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// 根据管网id活得所有关联阀门和管网
        /// </summary>
        /// <param name="pipeId">管网id (','号分割)</param>
        /// <returns></returns>
        //[Route("SpatialSearch/GetRealatedValveAndPipeByPipeId")]
        [HttpGet]
        public MessageEntity GetRealatedValveAndPipeByPipeId(string pipeId)
        {
            if (string.IsNullOrEmpty(pipeId))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            var pipeIdArray = pipeId.Split(',');
            var result = _pipeDAL.GetRealatedValveAndPipeByPipeId(pipeIdArray, out string errorMsg);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, errorMsg);
            }

            return MessageEntityTool.GetMessage(result.Count, result, true, "完成", result.Count);
        }

        /// <summary>
        /// 获取横断面PNG
        /// </summary>
        /// <param name="value">能够转化为DataTable的Json</param>
        /// <returns></returns>
        //[Route("SpatialSearch/GetCurveHeng")]
        [HttpPost]
        public MessageEntity GetCurveHeng(object value)
        {
            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(value.ToString());
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                var result = _pipeDAL.GetCurveHeng(ds);

                return MessageEntityTool.GetMessage(1, Request.HttpContext.Connection.RemoteIpAddress + result);
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, e.Message);
            }


        }
        /// <summary>
        /// 根据4326坐标查询附近小区/楼宇/用户
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        [Route("~/api/SpatialSearch/UserInfo/Get")]
        [HttpGet]
        public MessageEntity GetUserInfo(double lng, double lat)
        {
            if (lng == 0 || lat == 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "参数不能为0");
            }
            int.TryParse(Appsettings.app(new string[] { "SpatialSearch", "PointBuffer" }), out int pointBuffer);
            if (pointBuffer == 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "请检查SpatialSearch:PointBuffer配置项", "配置项有误");
            }
            try
            {
                return MessageEntityTool.GetMessage(1, _spatialSearchDAL.GetUserInfo(lng, lat, pointBuffer));
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, e.Message);
            }
        }

        /// <summary>
        /// 根据4326polylines查询附近小区/楼宇/用户
        /// </summary>
        /// <param name="multilinestring"></param>
        /// <returns></returns>
        [Route("~/api/SpatialSearch/GetUserInfoByPolylines/POST")]
        [HttpPost]
        public MessageEntity GetUserInfoByPolylines([FromBody]List<List<double[]>> multilinestring)
        {

            int.TryParse(Appsettings.app(new string[] { "SpatialSearch", "LineBuffer" }), out int lineBuffer);
            if (lineBuffer == 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "请检查SpatialSearch:LineBuffer", "配置项有误");
            }

            //var obj =JsonConvert.DeserializeObject<List<List<double[]>>>(multilinestring);
            try
            {
                return MessageEntityTool.GetMessage(1, _spatialSearchDAL.GetUserInfo(multilinestring, lineBuffer));
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, e.Message);
            }
        }

        /// <summary>
        /// 根据4326Points查询附近小区/楼宇/用户
        /// </summary>
        /// <param name="multilpoint"></param>
        /// <returns></returns>
        [Route("~/api/SpatialSearch/GetUserInfoByPoints/POST")]
        [HttpPost]
        public MessageEntity GetUserInfoByPoints([FromBody]List<double[]> multilpoint)
        {

            int.TryParse(Appsettings.app(new string[] { "SpatialSearch", "pointBuffer" }), out int pointBuffer);
            if (pointBuffer == 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "请检查SpatialSearch:pointBuffer", "配置项有误");
            }

            //var obj =JsonConvert.DeserializeObject<List<List<double[]>>>(multilinestring);
            try
            {
                return MessageEntityTool.GetMessage(1, _spatialSearchDAL.GetUserInfo(multilpoint, pointBuffer));
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, e.Message);
            }
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="keyWords">>' '分割</param>
        /// <param name="offset">>=0</param>
        /// <param name="record">>0</param>
        /// <param name="data">参数集合{EquipmentCount:设备数量,Polygon:区域,ClassNames:要查询的设备列表,Grids:分区id}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/api/SpatialSearch/PaginationSearch/Post")]
        public MessageEntity PaginationSearch([FromBody]SpatialSearchPaginationForm data, string keyWords = "", int offset = 0, int record = 10)
        {
            Dictionary<string, int> equipmentCount = new Dictionary<string, int>();
            List<double[]> polygon = new List<double[]>();
            int[] grids = new int[0];
            string[] districts = new string[0];
            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
            List<GIS_LayerSearchField> lsf = new List<GIS_LayerSearchField>();

            if (record < 1 || record > 50 || offset < 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "record请保持在1-50之间");
            }

            try
            {
                using StreamReader sr = new StreamReader(_webHostEnvironment.ContentRootPath + "/LayerSearchField.json", Encoding.GetEncoding("GB2312"));
                lsf = JsonConvert.DeserializeObject<List<GIS_LayerSearchField>>(sr.ReadToEnd());
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, e.Message, "请检查LayerSearchField格式");
            }

            if (data != null)
            {
                if (data.Polygon != null)
                {
                    if (data.Polygon.Last()[0] != data.Polygon.First()[0] || data.Polygon.Last()[1] != data.Polygon.First()[1] || data.Polygon.Count() < 4 || data.Polygon.Any(p => p.Length != 2))
                    {
                        return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "请检查polygon格式");
                    }
                    else
                    {
                        polygon = data.Polygon;
                    }
                }

                if (data.ClassNames != null && data.ClassNames.Count >= 1)
                {
                    lsf = lsf.Where(p => data.ClassNames.Contains(p.Name)).ToList();
                }

                if (data.Grids != null && data.Grids.Length > 0)
                {
                    grids = data.Grids;
                }
                if (data.Districts != null && data.Districts.Length > 0)
                {
                    districts = data.Districts;
                }
                
                if (data.EquipmentCount != null && data.EquipmentCount.Count >= 1)
                {
                    equipmentCount = data.EquipmentCount;
                }
            }

            try
            {
                var result = _spatialSearchDAL.PaginationSearch(string.IsNullOrEmpty(keyWords) ? new string[0] : keyWords.TrimStart().TrimEnd().Split(' '), lsf, polygon, equipmentCount, grids, districts, offset, record);
                return MessageEntityTool.GetMessage(1, result);
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, e.Message);
            }

        }

        /// <summary>
        /// 分页查询所支持layer和字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/SpatialSearch/EquipmentList/GET")]
        public MessageEntity EquipmentList()
        {
            try
            {
                EncodingProvider provider = CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(provider);
                using StreamReader sr = new StreamReader(_webHostEnvironment.ContentRootPath + "/LayerSearchField.json", Encoding.GetEncoding("GB2312"));
                string json = sr.ReadToEnd();
                List<GIS_LayerSearchField> lsf = JsonConvert.DeserializeObject<List<GIS_LayerSearchField>>(json);
                return MessageEntityTool.GetMessage(lsf.Count, lsf);
            }
            catch (Exception e)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, e.Message, "请检查LayerSearchField格式");
            }
        }
    }
}