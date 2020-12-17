using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using GISWaterSupplyAndSewageServer.CommonTools;
using Newtonsoft.Json;
using GISWaterSupplyAndSewageServer.IDAL.CoordinateTransf;
using GISWaterSupplyAndSewageServer.Model.Common;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.CoordinateTransf
{
    /// <summary>
    ///  坐标转换
    /// </summary>
    public class CoordinateTransController : Controller
    {
        private readonly ICoordinateTransDAL _coordinateTransDAL;
        private readonly IProjNetCoordinateDAL _projNetCoordinateDAL;
        public CoordinateTransController(ICoordinateTransDAL coordinateTransDAL, IProjNetCoordinateDAL projNetCoordinateDAL)
        {
            _coordinateTransDAL = coordinateTransDAL;
            _projNetCoordinateDAL = projNetCoordinateDAL;

        }
        /// <summary>
        /// 百度坐标系 (BD-09) 与 火星坐标系 (GCJ-02)的转换
        /// 即 百度 转 谷歌、高德
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns>转换后坐标</returns>
        [HttpGet]
        public MessageEntity GetBd09togcj02(double lng, double lat)
        {
            return _coordinateTransDAL.Bd09togcj02(lng, lat);
        }
        /// <summary>
        /// 批量坐标转换  (BD-09) 与 火星坐标系 (GCJ-02)的转换
        /// 即 百度 转 谷歌、高德
        /// </summary>
        /// <param name="listCoordinate"> 坐标数组集合 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Bd09togcj02List([FromBody] CoordinateModel listCoordinate)
        {
            //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }
            return _coordinateTransDAL.Bd09togcj02List(listCoordinate.listCoordinate);

        }
        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换
        /// 即谷歌、高德 转 百度
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetGcj02tobd09(double lng, double lat)
        {
            return _coordinateTransDAL.Gcj02tobd09(lng, lat);

        }
        /// <summary>
        /// 批量火星坐标系 (GCJ-02)与(BD-09)坐标转换
        /// 即谷歌、高德 转 百度
        /// </summary>
        /// <param name="listCoordinate"> 坐标数组集合 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Gcj02tobd09List([FromBody]CoordinateModel listCoordinate)
        {
            //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }
            return _coordinateTransDAL.Gcj02tobd09List(listCoordinate.listCoordinate);

        }
        /// <summary>
        ///  WGS84转GCj02
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Wgs84togcj02(double lng, double lat)
        {
            return _coordinateTransDAL.Wgs84togcj02(lng, lat);

        }
        /// <summary>
        /// 批量 WGS84 转换 GCJ02
        /// </summary>
        /// <param name="listCoordinate">坐标数组集合 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Wgs84togcj02List([FromBody]CoordinateModel listCoordinate)
        {
            //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }
            return _coordinateTransDAL.gcj02towgs84List(listCoordinate.listCoordinate);

        }
        /// <summary>
        /// GCJ02 转换为 WGS84
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Gcj02towgs84(double lng, double lat)
        {
            return _coordinateTransDAL.Gcj02towgs84(lng, lat);

        }
        /// <summary>
        /// 批量GCJ经纬度 转WGS纬度
        /// </summary>
        /// <param name="listCoordinate"> 坐标数组集合 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Gcj02towgs84List([FromBody]CoordinateModel listCoordinate)
        {
            //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }
            return _coordinateTransDAL.gcj02towgs84List(listCoordinate.listCoordinate);

        }
        /// <summary>
        /// 批量转换2000地理坐标转换为WGS84地理坐标系
        /// </summary>
        /// <param name="listCoordinate">源 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity S2000ToWGS84List([FromBody]CoordinateModel listCoordinate)
        { //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }
            return MessageEntityTool.GetMessage(1, _projNetCoordinateDAL.S2000ToWGS84List(listCoordinate.listCoordinate));
        }
        /// <summary>
        /// 2000地理坐标转换为WGS84地理坐标系
        /// </summary>
        /// <param name="pts">源 例如：[[123.11,2.22]] </param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity S2000ToWGS84(string pts)
        {
            List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(pts);
            return MessageEntityTool.GetMessage(1, _projNetCoordinateDAL.S2000ToWGS84(list1[0]));

        }
        /// <summary>
        /// 批量转换WGS84地理坐标转换为2000地理坐标系
        /// </summary>
        /// <param name="listCoordinate">源 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity WGS84ToS2000List([FromBody]CoordinateModel listCoordinate)
        {
            //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }
            return MessageEntityTool.GetMessage(1, _projNetCoordinateDAL.WGS84ToS2000List(listCoordinate.listCoordinate));

        }
        /// <summary>
        /// WGS84地理坐标转换为2000地理坐标系
        /// </summary>
        /// <param name="pts">源 例如：[[123.11,2.22]]</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity WGS84ToS2000(string pts)
        {
            List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(pts);
            return MessageEntityTool.GetMessage(1, _projNetCoordinateDAL.WGS84ToS2000(list1[0]));

        }
        /// <summary>
        /// 批量GCJ-02 转 2000地理坐标系
        /// </summary>
        /// <param name="listCoordinate">源 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity GCJ02ToS2000List([FromBody]CoordinateModel listCoordinate)
        {
            //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }
            return MessageEntityTool.GetMessage(1, _projNetCoordinateDAL.GCJ02ToS2000List(listCoordinate.listCoordinate));

        }
        /// <summary>
        /// GCJ-02 转 2000地理坐标系
        /// </summary>
        /// <param name="pts">源 例如：[[123.11,2.22]]</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GCJ02ToS2000(string pts)
        {
            List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(pts);
            return MessageEntityTool.GetMessage(1, _projNetCoordinateDAL.GCJ02ToS2000(list1[0]));

        }
        /// <summary>
        /// 批量2000地理坐标系 转 GCJ-02
        /// </summary>
        /// <param name="listCoordinate">源 例如：[[123.11,2.22],[124.11,4.22],[125.11,5.22]]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity S2000ToGCJ02List([FromBody]CoordinateModel listCoordinate)
        {
            if (listCoordinate.listCoordinate == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError, "参数不能为空");
            }

            //List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(listCoordinate.listCoordinate);

            return _projNetCoordinateDAL.S2000ToGCJ02List(listCoordinate.listCoordinate);

        }
        /// <summary>
        /// 2000地理坐标系转 GCJ-02
        /// </summary>
        /// <param name="pts">源 例如：[[123.11,2.22]]</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity S2000ToGCJ02(string pts)
        {
            List<double[]> list1 = JsonConvert.DeserializeObject<List<double[]>>(pts);
            return _projNetCoordinateDAL.S2000ToGCJ02(list1[0]);

        }
    }
}
