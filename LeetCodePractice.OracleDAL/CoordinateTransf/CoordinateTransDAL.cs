using GISWaterSupplyAndSewageServer.IDAL.CoordinateTransf;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GISWaterSupplyAndSewageServer.OracleDAL.CoordinateTransf
{
    /// <summary>
    /// 百度、谷歌、高德、WGS84等坐标互转
    /// </summary>
    public class CoordinateTransDAL: ICoordinateTransDAL
    {
        public const double x_PI = (3.14159265358979324 * 3000.0) / 180.0;
        public const double PI = 3.1415926535897932384626;
        public const double a = 6378245.0;
        public const double ee = 0.00669342162296594323;

        /// <summary>
        /// 百度坐标系 (BD-09) 与 火星坐标系 (GCJ-02)的转换
        /// 即 百度 转 谷歌、高德
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns>转换后坐标</returns>
        public MessageEntity Bd09togcj02(double lng, double lat)
        {
            double x = lng - 0.0065;
            double y = lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_PI);
            double gg_lng = z * Math.Cos(theta);
            double gg_lat = z * Math.Cos(theta);
            return MessageEntityTool.GetMessage(1, new double[] { gg_lng, gg_lat }); 
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换
        /// 即谷歌、高德 转 百度
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public MessageEntity Gcj02tobd09(double lng, double lat)
        {
            var z = Math.Sqrt(lng * lng + lat * lat) + 0.00002 * Math.Sin(lat * x_PI);
            var theta = Math.Atan2(lat, lng) + 0.000003 * Math.Cos(lng * x_PI);
            var bd_lng = z * Math.Cos(theta) + 0.0065;
            var bd_lat = z * Math.Sin(theta) + 0.006;
            return MessageEntityTool.GetMessage(1, new double[] { bd_lng, bd_lat });

        }

        /// <summary>
        ///  WGS84转GCj02
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
       
        public MessageEntity Wgs84togcj02(double lng, double lat)
        {
            if (out_of_china(lng, lat))
            {
                return MessageEntityTool.GetMessage(1, new double[] { lng, lat });

            }
            else
            {
                var dlat = transformlat(lng - 105.0, lat - 35.0);
                var dlng = transformlng(lng - 105.0, lat - 35.0);
                var radlat = (lat / 180.0) * PI;
                var magic = Math.Sin(radlat);
                magic = 1 - ee * magic * magic;
                var sqrtmagic = Math.Sqrt(magic);
                dlat = (dlat * 180.0) / (((a * (1 - ee)) / (magic * sqrtmagic)) * PI);
                dlng = (dlng * 180.0) / ((a / sqrtmagic) * Math.Cos(radlat) * PI);
                var mglat = lat + dlat;
                var mglng = lng + dlng;
                return MessageEntityTool.GetMessage(1, new double[] { mglng, mglat });

            }
        }

        /// <summary>
        /// GCJ02 转换为 WGS84
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
     
        public MessageEntity Gcj02towgs84(double lng, double lat)
        {
            if (out_of_china(lng, lat))
            {
                return MessageEntityTool.GetMessage(1, new double[] { lng, lat });
            }
            else
            {
                var dlat = transformlat(lng - 105.0, lat - 35.0);
                var dlng = transformlng(lng - 105.0, lat - 35.0);
                var radlat = (lat / 180.0) * PI;
                var magic = Math.Sin(radlat);
                magic = 1 - ee * magic * magic;
                var sqrtmagic = Math.Sqrt(magic);
                dlat = (dlat * 180.0) / (((a * (1 - ee)) / (magic * sqrtmagic)) * PI);
                dlng = (dlng * 180.0) / ((a / sqrtmagic) * Math.Cos(radlat) * PI);
                var mglat = lat + dlat;
                var mglng = lng + dlng;
                return MessageEntityTool.GetMessage(1, new double[] { lng * 2 - mglng, lat * 2 - mglat });

            }
        }


        /// <summary>
        ///  批量坐标转换 GCJ02 转换为 WGS84
        /// </summary>
        /// <param name="listCoordinate"></param>
        /// <returns></returns>
        public MessageEntity gcj02towgs84List(List<double[]> listCoordinate)
        {
            List<double[]> ReturnList = new List<double[]>();
            if (listCoordinate.Count > 0)
            {
                listCoordinate.ForEach(DataInfo =>
                {
                    MessageEntity mess= Gcj02towgs84(DataInfo[0], DataInfo[1]);
                    object o = mess.Data.Result;
                    double[] lnglat = (double[])o;
                       ReturnList.Add(new double[] { lnglat[0], lnglat[1] });
                });
            }
            return MessageEntityTool.GetMessage(1, ReturnList);
        }
        /// <summary>
        /// 批量坐标转换 WGS84转GCj02
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public MessageEntity wgs84togcj02List(List<double[]> listCoordinate)
        {

            List<double[]> ReturnList = new List<double[]>();

            if (listCoordinate.Count > 0)
            {
                listCoordinate.ForEach(DataInfo =>
                {
                        MessageEntity mess = Wgs84togcj02(DataInfo[0], DataInfo[1]);
                        object o = mess.Data.Result;
                        double[] lnglat = (double[])o;
                        ReturnList.Add(new double[] { lnglat[0], lnglat[1] });
                });
            }
            return MessageEntityTool.GetMessage(1, ReturnList);
        }



        /// <summary>
        ///  批量坐标转换  (BD-09) 与 火星坐标系 (GCJ-02)的转换
        /// 即 百度 转 谷歌、高德
        /// </summary>
        /// <param name="listCoordinate"></param>
        /// <returns></returns>
        public MessageEntity Bd09togcj02List(List<double[]> listCoordinate)
        {
            List<double[]> ReturnList = new List<double[]>();
            if (listCoordinate.Count > 0)
            {
                listCoordinate.ForEach(DataInfo =>
                {
                    MessageEntity mess = Bd09togcj02(DataInfo[0], DataInfo[1]);
                    object o = mess.Data.Result;
                    double[] lnglat = (double[])o;
                    ReturnList.Add(new double[] { lnglat[0], lnglat[1] });
                });
            }
            return MessageEntityTool.GetMessage(1, ReturnList);
        }
        /// <summary>
        /// 批量坐标转换 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换
        /// 即谷歌、高德 转 百度
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public MessageEntity Gcj02tobd09List(List<double[]> listCoordinate)
        {

            List<double[]> ReturnList = new List<double[]>();

            if (listCoordinate.Count > 0)
            {
                listCoordinate.ForEach(DataInfo =>
                {
                    MessageEntity mess = Gcj02tobd09(DataInfo[0], DataInfo[1]);
                    object o = mess.Data.Result;
                    double[] lnglat = (double[])o;
                    ReturnList.Add(new double[] { lnglat[0], lnglat[1] });
                });
            }
            return MessageEntityTool.GetMessage(1, ReturnList);
        }


        /// <summary>
        /// 纬度坐标转换
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        private double transformlat(double lng, double lat)
        {
            var ret = -100.0 + 2.0 * lng + 3.0 * lat + 0.2 * lat * lat + 0.1 * lng * lat + 0.2 * Math.Sqrt(Math.Abs(lng));
            ret += ((20.0 * Math.Sin(6.0 * lng * PI) + 20.0 * Math.Sin(2.0 * lng * PI)) * 2.0) / 3.0;
            ret += ((20.0 * Math.Sin(lat * PI) + 40.0 * Math.Sin((lat / 3.0) * PI)) * 2.0) / 3.0;
            ret += ((160.0 * Math.Sin((lat / 12.0) * PI) + 320 * Math.Sin((lat * PI) / 30.0)) * 2.0) / 3.0;
            return ret;
        }


        /// <summary>
        /// 经度坐标转换
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        private double transformlng(double lng, double lat)
        {
            var ret = 300.0 + lng + 2.0 * lat + 0.1 * lng * lng + 0.1 * lng * lat + 0.1 * Math.Sqrt(Math.Abs(lng));
            ret += ((20.0 * Math.Sin(6.0 * lng * PI) + 20.0 * Math.Sin(2.0 * lng * PI)) * 2.0) / 3.0;
            ret += ((20.0 * Math.Sin(lng * PI) + 40.0 * Math.Sin((lng / 3.0) * PI)) * 2.0) / 3.0;
            ret += ((150.0 * Math.Sin((lng / 12.0) * PI) + 300.0 * Math.Sin((lng / 30.0) * PI)) * 2.0) / 3.0;
            return ret;
        }

        /// <summary>
        /// 判断是否在国内，不在国内则不做偏移
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        private bool out_of_china(double lng, double lat)
        {
            // 纬度3.86~53.55,经度73.66~135.05
            return !(lng > 73.66 && lng < 135.05 && lat > 3.86 && lat < 53.55);
        }
    }
}
