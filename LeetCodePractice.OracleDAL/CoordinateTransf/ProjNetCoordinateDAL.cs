using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.IDAL.CoordinateTransf;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using ProjNet.CoordinateSystems;

namespace GISWaterSupplyAndSewageServer.OracleDAL.CoordinateTransf
{
    /// <summary>
    /// 全球坐标转换集合
    /// 该类只实现
    /// </summary>
    public class ProjNetCoordinateDAL : IProjNetCoordinateDAL
    {
        public const string wkt4490 = "GEOGCS[\"China Geodetic Coordinate System 2000\",DATUM[\"China_2000\",SPHEROID[\"CGCS2000\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"1024\"]],AUTHORITY[\"EPSG\",\"1043\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4490\"]]";
        public const string wkt4547 = "PROJCS[\"CGCS2000 / 3-degree Gauss-Kruger CM 114E\",GEOGCS[\"China Geodetic Coordinate System 2000\",DATUM[\"China_2000\",SPHEROID[\"CGCS2000\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"1024\"]],AUTHORITY[\"EPSG\",\"1043\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4490\"]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",114],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AUTHORITY[\"EPSG\",\"4547\"]]";

        //定义系统坐标映射工厂，例如：构建4490坐标，4547坐标等信息
        public ProjNet.CoordinateSystems.CoordinateSystemFactory CFAuto = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
        //定义坐标转换工厂，例如将 4490 坐标转换为WGS84
        public ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory TransFactory = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

        CoordinateTransDAL coorTrans = new CoordinateTransDAL();

        /// <summary>
        /// 批量转换2000地理坐标转换为WGS84地理坐标系
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public IList<double[]> S2000ToWGS84List(List<double[]> pts)
        {
            var src = CFAuto.CreateFromWkt(wkt4490);
            var tgt = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
            var trans = TransFactory.CreateFromCoordinateSystems(src, tgt);
            IList<double[]> result = trans.MathTransform.TransformList(pts);
            return result;
        }

        /// <summary>
        /// 2000地理坐标转换为WGS84地理坐标系
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public double[] S2000ToWGS84(double[] pts)
        {
            var src = CFAuto.CreateFromWkt(wkt4490);
            var tgt = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
            var trans = TransFactory.CreateFromCoordinateSystems(src, tgt);
            double[] result = trans.MathTransform.Transform(pts);
            return result;
        }

        /// <summary>
        /// 批量转换WGS84地理坐标转换为2000地理坐标系
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public IList<double[]> WGS84ToS2000List(List<double[]> pts)
        {
            var tgt = CFAuto.CreateFromWkt(wkt4490);
            var src = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
            var trans = TransFactory.CreateFromCoordinateSystems(src, tgt);
            IList<double[]> result = trans.MathTransform.TransformList(pts);
            return result;
        }

        /// <summary>
        /// WGS84地理坐标转换为2000地理坐标系
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public double[] WGS84ToS2000(double[] pts)
        {
            var tgt = CFAuto.CreateFromWkt(wkt4490);
            var src = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
            var trans = TransFactory.CreateFromCoordinateSystems(src, tgt);
            double[] result = trans.MathTransform.Transform(pts);
            return result;
        }
        /// <summary>
        /// 批量GCJ-02 转 2000地理坐标系
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public IList<double[]> GCJ02ToS2000List(List<double[]> pts)
        {
            MessageEntity messresult = coorTrans.gcj02towgs84List(pts);
            List<double[]> ptslist = (List<double[]>)messresult.Data.Result;
            IList<double[]> result = WGS84ToS2000List(ptslist);
            return result;
        }

        /// <summary>
        ///GCJ-02 转 2000地理坐标系
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public double[] GCJ02ToS2000(double[] pts)
        {
            MessageEntity messresult = coorTrans.Gcj02towgs84(pts[0], pts[1]);
            double[] ptsarr = (double[])messresult.Data.Result;
            double[] result = WGS84ToS2000(ptsarr);
            return result;
        }

        /// <summary>
        ///2000地理坐标系 转 GCJ-02
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public MessageEntity S2000ToGCJ02(double[] pts)
        {
            double[] messresult = S2000ToWGS84(pts);
            MessageEntity result = coorTrans.Wgs84togcj02(messresult[0], messresult[1]);
            return MessageEntityTool.GetMessage(1, result.Data.Result);
        }

        /// <summary>
        /// 批量2000地理坐标系 转 GCJ-02
        /// </summary>
        /// <param name="pts">源</param>
        /// <returns></returns>
        public MessageEntity S2000ToGCJ02List(List<double[]> pts)
        {

            List<double[]> ptslist = (List<double[]>)S2000ToWGS84List(pts);
            MessageEntity result = coorTrans.wgs84togcj02List(ptslist);
            return MessageEntityTool.GetMessage(1, result.Data.Result);
        }
    }
}
