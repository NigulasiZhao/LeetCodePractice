using Dapper;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.OracleDAL
{
    public class GCJ2WGSUtilsDAL : BaseDAL, IGCJ2WGSUtilsDAL
    {
        #region  --------   GCJ02  转换  WGS84

        //单坐标转换
        public MessageEntity gcj02towgs84(double wgs_lat, double wgs_lon)
        {
            var lat = WGSLat(wgs_lon, wgs_lat);
            var lon = WGSLon(wgs_lon, wgs_lat);
            return MessageEntityTool.GetMessage(1, new double[] { lon, lat });
        }

        //批量坐标转换
        public MessageEntity gcj02towgs84List(List<double[]> listCoordinate)
        {
            List<double[]> ReturnList = new List<double[]>();
            if (listCoordinate.Count > 0)
            {
                listCoordinate.ForEach(DataInfo =>
                {
                    var lat = WGSLat(DataInfo[0], DataInfo[1]);
                    var lon = WGSLon(DataInfo[0], DataInfo[1]);
                    ReturnList.Add(new double[] { lon, lat });
                });
            }
            return MessageEntityTool.GetMessage(1, ReturnList);
        }




        //输入GCJ经纬度 转WGS纬度
        public double WGSLat(double lat, double lon)
        {
            double PI = 3.14159265358979324;//圆周率
            double a = 6378245.0;//克拉索夫斯基椭球参数长半轴a
            double ee = 0.00669342162296594323;//克拉索夫斯基椭球参数第一偏心率平方
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * PI);
            return lat - dLat;
        }

        //输入GCJ经纬度 转WGS经度
        public double WGSLon(double lat, double lon)
        {
            double PI = 3.14159265358979324;//圆周率
            double a = 6378245.0;//克拉索夫斯基椭球参数长半轴a
            double ee = 0.00669342162296594323;//克拉索夫斯基椭球参数第一偏心率平方
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * PI);
            return lon - dLon;
        }

        //转换经度所需
        public static double transformLon(double x, double y)
        {
            double PI = 3.14159265358979324;//圆周率
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * PI) + 20.0 * Math.Sin(2.0 * x * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * PI) + 40.0 * Math.Sin(x / 3.0 * PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * PI) + 300.0 * Math.Sin(x / 30.0 * PI)) * 2.0 / 3.0;
            return ret;
        }
        //转换纬度所需
        public static double transformLat(double x, double y)
        {
            double PI = 3.14159265358979324;//圆周率
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * PI) + 20.0 * Math.Sin(2.0 * x * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * PI) + 40.0 * Math.Sin(y / 3.0 * PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * PI) + 320 * Math.Sin(y * PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }
        #endregion


        #region  -------- WGS84 转换 GCJ02--------------
        /**
        * 判断是否中国境内
        * @param lon
        * @param lat
        * @return */
        public static bool out_of_china(double lon, double lat)
        {
            // 纬度3.86~53.55,经度73.66~135.05
            return !(lon > 73.66 && lon < 135.05 && lat > 3.86 && lat < 53.55);
        }
        /**
         * 经度转换
         * @param lon
         * @param lat
         * @return
         */
        public static double TransformLon(double lon, double lat)
        {

            double PI = 3.1415926535897932384626;
            double ret = 300.0 + lon + 2.0 * lat + 0.1 * lon * lon + 0.1 * lon * lat + 0.1 * Math.Sqrt(Math.Abs(lon));
            ret += (20.0 * Math.Sin(6.0 * lon * PI) + 20.0 * Math.Sin(2.0 * lon * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lon * PI) + 40.0 * Math.Sin(lon / 3.0 * PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(lon / 12.0 * PI) + 300.0 * Math.Sin(lon / 30.0 * PI)) * 2.0 / 3.0;
            return ret;
        }
        /**
         * 纬度转换
         * @param lon
         * @param lat
         * @return
         */
        public static double TransformLat(double lon, double lat)
        {
            double PI = 3.1415926535897932384626;
            double ret = -100.0 + 2.0 * lon + 3.0 * lat + 0.2 * lat * lat + 0.1 * lon * lat + 0.2 * Math.Sqrt(Math.Abs(lon));
            ret += (20.0 * Math.Sin(6.0 * lon * PI) + 20.0 * Math.Sin(2.0 * lon * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lat * PI) + 40.0 * Math.Sin(lat / 3.0 * PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(lat / 12.0 * PI) + 320 * Math.Sin(lat * PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }
        /// <summary>
        /// WGS84 转换 GCJ02
        /// </summary>
        /// <param name="wgs_lon"></param>
        /// <param name="wgs_lat"></param>
        /// <returns></returns>
        public MessageEntity wgs84togcj02(double wgs_lon, double wgs_lat)
        {
            double ee = 0.00669342162296594323;
            double PI = 3.1415926535897932384626;
            double a = 6378245.0;
            if (out_of_china(wgs_lon, wgs_lat))
            {
                return MessageEntityTool.GetMessage(1, new double[] { wgs_lon, wgs_lat });
            }
            else
            {
                double dlat = TransformLat(wgs_lon - 105.0, wgs_lat - 35.0);
                double dlon = TransformLon(wgs_lon - 105.0, wgs_lat - 35.0);
                double radlat = wgs_lat / 180.0 * PI;
                double magic = Math.Sin(radlat);
                magic = 1 - ee * magic * magic;
                double sqrtmagic = Math.Sqrt(magic);
                dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                dlon = (dlon * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * PI);
                double mglat = wgs_lat + dlat;
                double mglon = wgs_lon + dlon;
                return MessageEntityTool.GetMessage(1, new double[] { mglon, mglat });
            }
        }

        /// <summary>
        /// 批量坐标转换
        /// </summary>
        /// <param name="listCoordinate"></param>
        /// <returns></returns>
        public MessageEntity wgs84togcj02List(List<double[]> listCoordinate)
        {
            double ee = 0.00669342162296594323;
            double PI = 3.1415926535897932384626;
            double a = 6378245.0;

            List<double[]> ReturnList = new List<double[]>();

            if (listCoordinate.Count > 0)
            {
                listCoordinate.ForEach(DataInfo =>
                {
                    if (out_of_china(DataInfo[0], DataInfo[1]))
                    {
                        //return MessageEntityTool.GetMessage(1, new double[] { DataInfo[0], DataInfo[1] });

                        ReturnList.Add(new double[] { DataInfo[0], DataInfo[1] });

                    }
                    else
                    {
                        double dlat = TransformLat(DataInfo[0] - 105.0, DataInfo[1] - 35.0);
                        double dlon = TransformLon(DataInfo[0] - 105.0, DataInfo[1] - 35.0);
                        double radlat = DataInfo[1] / 180.0 * PI;
                        double magic = Math.Sin(radlat);
                        magic = 1 - ee * magic * magic;
                        double sqrtmagic = Math.Sqrt(magic);
                        dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                        dlon = (dlon * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * PI);
                        double mglat = DataInfo[1] + dlat;
                        double mglon = DataInfo[0] + dlon;
                        ReturnList.Add(new double[] { mglon, mglat });
                    }
                });
            }
            return MessageEntityTool.GetMessage(1, ReturnList);
        }

        #endregion
    }
}
