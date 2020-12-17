using Dapper;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.IDAL.CoordinateTransf;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Gis_Spatialquery
{
    public class PipeDAL : IPipeDAL
    {
        private readonly IProjNetCoordinateDAL _projNetCoordinateDAL;
        public PipeDAL(IProjNetCoordinateDAL projNetCoordinateDAL)
        {
            _projNetCoordinateDAL = projNetCoordinateDAL;
        }
        public Pipe Get(string latitude, string longitude, out string errMessge)
        {
            throw new NotImplementedException();
        }

        public List<string> GetCaliber(out string errMessge)
        {
            throw new NotImplementedException();
        }

        public string GetCurveHeng(DataSet sender)
        {
            throw new NotImplementedException();
        }

        public List<string> Getinstallation_addresses(out string errMessge)
        {
            throw new NotImplementedException();
        }

        public List<object> GetLayers(out string errMessge)
        {
            throw new NotImplementedException();
        }

        public List<string> GetMaterial_science(out string errMessge)
        {
            throw new NotImplementedException();
        }

        public List<PointTable> GetPipeAndPointByPage(string layer, string installation_address, string material_science, string minCaliber, string maxCaliber, DateTime? startCompletion_date, DateTime? endCompletion_date, string sort, string ordering, int num, int page, out string errMessge, out MessageEntity result)
        {
            throw new NotImplementedException();
        }

        public List<dynamic> GetPipeAndPointStatistics(string layer, string installation_address, string material_science, string caliber, DateTime? startCompletion_date, DateTime? endCompletion_date, string sort, string ordering, string[] groupFields, out string errMessge, out MessageEntity result)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<dynamic>> GetRealatedValveAndPipeByPipeId(string[] pipeIds, out string errorMsg)
        {
            Dictionary<string, List<dynamic>> result = new Dictionary<string, List<dynamic>>();
            errorMsg = string.Empty;
            string pipeField = @" objectid,
       fcode,
       usid,
       prjid,
       subtype,
       d_s,
       material,
       con_type,
       cov_type,
       use,
       usetype,
       cen_deep,
       is_press,
       work_stats,
       manufacturer,
       archive_id,
       finish_date,
       constructer,
       lane_way,
       addr,
       district,
       managedept,
       rec_num,
       arc_num,
       datatype,
       write_date,
       write_person,
       remark,
       sid,
       eid,
       gid,
       enabled,
       prjtype,
       add_person,
       add_date,
       cid,
       gname,
       grid,
       length,
       rfid,
       globalid,
       sde.st_astext (shape)  as shape ";
            string vavleField = @" objectid,
       fcode,
       usid,
       eid,
       prjid,
       subtype,
       model,
       usetype,
       d_s,
       turn_on,
       turnangle,
       turnnum,
       well_type,
       manufacturer,
       sur_h,
       top_h,
       cen_deep,
       well_stats,
       is_press,
       is_ccf,
       onoff_stats,
       work_stats,
       archive_id,
       finish_date,
       constructer,
       lane_way,
       addr,
       district,
       managedept,
       sy_angle,
       principal,
       ptel,
       datatype,
       write_date,
       write_person,
       remark,
       gid,
       sid,
       cid,
       x,
       y,
       enabled,
       add_person,
       add_date,
       szfmid,
       gname,
       grid,
       cur_turnnum,
       rfid,
       globalid,
      sde.st_astext (shape) as shape";

            string query = @" select t.yslineeid,
                                     t.gslineeid,
                                     t.otlineeid,      
                                     t.qulineeid,
                                     t.balineeid,
                                     t.prlineeid,
                                     t.colineeid,
                                     t.ysnodeseid,
                                     t.gsnodeseid,
                                     t.yslineseid,
                                     t.gslineseid,
                                     t.otlineseid,
                                     t.qulineseid,
                                     t.balineseid,
                                     t.prlineseid,
                                     t.colineseid
                              from NETANALYSTRESULT t WHERE t.yslineeid IN :ids or t.gslineeid IN :ids or t.otlineeid IN :ids or t.qulineeid IN :ids or t.balineeid IN :ids or t.prlineeid IN :ids or t.colineeid IN :ids";
            try
            {
                using (IDbConnection conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE))
                {
                    var valueAndPipeList = conn.Query<dynamic>(query, new { ids = pipeIds }).ToList();
                    string gsVavleQuerySql = "";
                    string ysVavleQuerySql = "";
                    string gsPipeQuerySql = "";
                    string ysPipeQuerySql = "";
                    string otPipeQuerySql = "";
                    string quPipeQuerySql = "";
                    string baPipeQuerySql = "";
                    string prPipeQuerySql = "";
                    string coPipeQuerySql = "";
                    foreach (var valueAndPipe in valueAndPipeList)
                    {
                        if (valueAndPipe != null)
                        {
                            //供水阀门
                            if (valueAndPipe.GSNODESEID != null && valueAndPipe.GSNODESEID != "")
                            {
                                string eid = "'" + valueAndPipe.GSNODESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(gsVavleQuerySql))
                                    gsVavleQuerySql = $@"SELECT   {vavleField}  FROM GSSS_VALVE WHERE globalid in ({eid}) ";
                                else
                                    gsVavleQuerySql += $@" union all SELECT  {vavleField} FROM GSSS_VALVE WHERE globalid in ({eid}) ";
                            }
                            //原水阀门
                            if (valueAndPipe.YSNODESEID != null && valueAndPipe.YSNODESEID != "")
                            {
                                string eid = "'" + valueAndPipe.YSNODESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(ysVavleQuerySql))
                                    ysVavleQuerySql = $@"SELECT   {vavleField}  FROM YSSS_VALVE WHERE globalid in ({eid}) ";
                                else
                                    ysVavleQuerySql += $@" union all SELECT  {vavleField} FROM YSSS_VALVE WHERE globalid in ({eid}) ";
                            }
                            //其他管线
                            if (valueAndPipe.OTLINESEID != null && valueAndPipe.OTLINESEID != "")
                            {
                                string eid = "'" + valueAndPipe.OTLINESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(otPipeQuerySql))
                                    otPipeQuerySql = $@"SELECT  {pipeField} FROM GSGX_OTHERLIN WHERE globalid in ({eid}) ";
                                else
                                    otPipeQuerySql += $@" union all  SELECT {pipeField} FROM GSGX_OTHERLIN WHERE globalid in ({eid}) ";
                            }
                            //原水管线
                            if (valueAndPipe.YSLINESEID != null && valueAndPipe.YSLINESEID != "")
                            {
                                string eid = "'" + valueAndPipe.YSLINESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(ysPipeQuerySql))
                                    ysPipeQuerySql = $@"SELECT  {pipeField} FROM YSGX_ORDINLN WHERE globalid in ({eid}) ";
                                else
                                    ysPipeQuerySql += $@" union all  SELECT {pipeField} FROM YSGX_ORDINLN WHERE globalid in ({eid}) ";
                            }
                            //供水管线
                            if (valueAndPipe.GSLINESEID != null && valueAndPipe.GSLINESEID != "")
                            {
                                string eid = "'" + valueAndPipe.GSLINESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(gsPipeQuerySql))
                                    gsPipeQuerySql = $@"SELECT  {pipeField} FROM GSGX_ORDINLN WHERE globalid in ({eid}) ";
                                else
                                    gsPipeQuerySql += $@" union all  SELECT {pipeField} FROM GSGX_ORDINLN WHERE globalid in ({eid}) ";
                            }
                            //高质管线
                            if (valueAndPipe.QULINESEID != null && valueAndPipe.QULINESEID != "")
                            {
                                string eid = "'" + valueAndPipe.QULINESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(quPipeQuerySql))
                                    quPipeQuerySql = $@"SELECT  {pipeField} FROM GSGX_QUALILN WHERE globalid in ({eid}) ";
                                else
                                    quPipeQuerySql += $@" union all  SELECT {pipeField} FROM GSGX_QUALILN WHERE globalid in ({eid}) ";
                            }
                            //表后管线
                            if (valueAndPipe.BALINESEID != null && valueAndPipe.BALINESEID != "")
                            {
                                string eid = "'" + valueAndPipe.BALINESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(baPipeQuerySql))
                                    baPipeQuerySql = $@"SELECT  {pipeField} FROM GSGX_BACTNLN WHERE globalid in ({eid}) ";
                                else
                                    baPipeQuerySql += $@" union all  SELECT {pipeField} FROM GSGX_BACTNLN WHERE globalid in ({eid}) ";
                            }
                            //加压管线
                            if (valueAndPipe.PRLINESEID != null && valueAndPipe.PRLINESEID != "")
                            {
                                string eid = "'" + valueAndPipe.PRLINESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(prPipeQuerySql))
                                    prPipeQuerySql = $@"SELECT  {pipeField} FROM GSGX_PRESSTNLN WHERE globalid in ({eid}) ";
                                else
                                    prPipeQuerySql += $@" union all  SELECT {pipeField} FROM GSGX_PRESSTNLN WHERE globalid in ({eid}) ";
                            }
                            //小区管线
                            if (valueAndPipe.COLINESEID != null && valueAndPipe.COLINESEID != "")
                            {
                                string eid = "'" + valueAndPipe.COLINESEID.Replace(",", "','") + "'";

                                if (string.IsNullOrEmpty(coPipeQuerySql))
                                    coPipeQuerySql = $@"SELECT  {pipeField} FROM GSGX_COMTNLN WHERE globalid in ({eid}) ";
                                else
                                    coPipeQuerySql += $@" union all  SELECT {pipeField} FROM GSGX_COMTNLN WHERE globalid in ({eid}) ";
                            }
                        }
                    }
                    //供水阀门
                    List<string> globalids = new List<string>();
                    if (!string.IsNullOrEmpty(gsVavleQuerySql))
                    {
                        var gsValvesInfo = conn.Query<dynamic>(gsVavleQuerySql).ToList();
                        result.Add("gsValvesInfo", gsValvesInfo);
                        //foreach (var item in gsValvesInfo)
                        //{
                        //    if (!string.IsNullOrEmpty(item.GLOBALID))
                        //    {
                        //        if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                        //        {
                        //            globalids.Add("'" + item.GLOBALID + "'");
                        //        }
                        //    }
                        //}
                    }
                    //原水阀门
                    if (!string.IsNullOrEmpty(ysVavleQuerySql))
                    {
                        var ysValvesInfo = conn.Query<dynamic>(ysVavleQuerySql).ToList();
                        result.Add("ysValvesInfo", ysValvesInfo);
                        //foreach (var item in ysValvesInfo)
                        //{
                        //    if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                        //    {
                        //        globalids.Add("'" + item.GLOBALID + "'");
                        //    }
                        //}
                    }
                    //其他管线
                    if (!string.IsNullOrEmpty(otPipeQuerySql))
                    {
                        var otPipeInfo = conn.Query<dynamic>(otPipeQuerySql).ToList();
                        result.Add("otPipeInfo", otPipeInfo);
                        foreach (var item in otPipeInfo)
                        {
                            if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                            {
                                globalids.Add("'" + item.GLOBALID + "'");
                            }
                        }
                    }
                    //原水管线
                    if (!string.IsNullOrEmpty(ysPipeQuerySql))
                    {
                        var ysPipeInfo = conn.Query<dynamic>(ysPipeQuerySql).ToList();
                        result.Add("ysPipeInfo", ysPipeInfo);
                        foreach (var item in ysPipeInfo)
                        {
                            if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                            {
                                globalids.Add("'" + item.GLOBALID + "'");
                            }
                        }
                    }
                    //供水管线
                    if (!string.IsNullOrEmpty(gsPipeQuerySql))
                    {
                        var gsPipesInfo = conn.Query<dynamic>(gsPipeQuerySql).ToList();
                        result.Add("gsPipesInfo", gsPipesInfo);
                        foreach (var item in gsPipesInfo)
                        {
                            if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                            {
                                globalids.Add("'" + item.GLOBALID + "'");
                            }
                        }
                    }
                    //高质管线
                    if (!string.IsNullOrEmpty(quPipeQuerySql))
                    {
                        var quPipesInfo = conn.Query<dynamic>(quPipeQuerySql).ToList();
                        result.Add("quPipesInfo", quPipesInfo);
                        foreach (var item in quPipesInfo)
                        {
                            if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                            {
                                globalids.Add("'" + item.GLOBALID + "'");
                            }
                        }
                    }
                    //表后管线
                    if (!string.IsNullOrEmpty(baPipeQuerySql))
                    {
                        var baPipesInfo = conn.Query<dynamic>(baPipeQuerySql).ToList();
                        result.Add("baPipesInfo", baPipesInfo);
                        foreach (var item in baPipesInfo)
                        {
                            if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                            {
                                globalids.Add("'" + item.GLOBALID + "'");
                            }
                        }
                    }
                    //加压管线
                    if (!string.IsNullOrEmpty(prPipeQuerySql))
                    {
                        var prPipesInfo = conn.Query<dynamic>(prPipeQuerySql).ToList();
                        result.Add("prPipesInfo", prPipesInfo);
                        foreach (var item in prPipesInfo)
                        {
                            if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                            {
                                globalids.Add("'" + item.GLOBALID + "'");
                            }
                        }
                    }
                    //小区管线
                    if (!string.IsNullOrEmpty(coPipeQuerySql))
                    {
                        var coPipesInfo = conn.Query<dynamic>(coPipeQuerySql).ToList();
                        result.Add("coPipesInfo", coPipesInfo);
                        foreach (var item in coPipesInfo)
                        {
                            if (globalids.FindAll(e => e == "'" + item.GLOBALID + "'").Count == 0)
                            {
                                globalids.Add("'" + item.GLOBALID + "'");
                            }
                        }
                    }
                    if (globalids.Count > 0)
                    {
                        string[] globalidsArr = globalids.ToArray();
                        string globalidsStr = string.Join(',', globalidsArr);
                        using (IDbConnection kMconn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.KM))
                        {
                            string QuerySql = $@"SELECT distinct
                                                        ugp.M_CODE,
                                                        ugp.M_POS,
                                                        ugp.M_POSDESC,
                                                        ugp.M_DN,
                                                        ugp.M_PROPERTY,
                                                        ugp.m_installdate  as M_INSTALLDA,
                                                        ugp.USERNO,
                                                        ugp.MP_WGOID,
                                                        ugp.SUBTYPE,
                                                        ugp.tmp_yc_address as TMP_YC_ADDR,
                                                        ugp.USER_OTITLE,
                                                        ugp.user_oid,
                                                    	vm.globalid,
                                                    	vmm.mi_shape_lat,
                                                    	vmm.mi_shape_lng 
                                                    FROM
                                                    	km.v_user_gis_pipe ugp
                                                    	INNER JOIN km.vcls_meter vm ON vm.oid = ugp.OID
                                                    	LEFT JOIN km.vcls_user cvu ON cvu.ocode = ugp.user_code
                                                    	LEFT JOIN km.vdw_md_mapitems vmm ON vmm.mi_tar_clsid = 53 
                                                    	AND vmm.mi_tar_oid = cvu.oid 
                                                    WHERE vm.globalid in ({globalidsStr})";
                            var pipeRelated = kMconn.Query<dynamic>(QuerySql).ToList();
                            List<object> userinfo = new List<object>();
                            List<object> meterInfo = new List<object>();
                            List<int> userId = new List<int>();
                            if (pipeRelated.Count > 0)
                            {
                                foreach (var item in pipeRelated)
                                {
                                    if (null != item.MI_SHAPE_LNG && null != item.MI_SHAPE_LAT)
                                    {
                                        double[] coordinates = new double[] { (double)item.MI_SHAPE_LNG, (double)item.MI_SHAPE_LAT };
                                        ((IDictionary<string, object>)item).Add("Coordinates", _projNetCoordinateDAL.GCJ02ToS2000(coordinates));
                                    }
                                    else
                                    {
                                        ((IDictionary<string, object>)item).Add("Coordinates", "");
                                    }
                                    userinfo.Add(new { item.USERNO, item.MP_WGOID, item.SUBTYPE, item.TMP_YC_ADDR, item.USER_OTITLE, item.Coordinates });
                                    meterInfo.Add(new { item.M_CODE, item.M_POS, item.M_POSDESC, item.M_DN, item.M_PROPERTY, item.M_INSTALLDA, item.Coordinates });
                                    userId.Add((int)item.USER_OID);
                                }
                            }
                            if (userId.Count > 0)
                            {
                                //小区
                                var communityList = kMconn.Query<dynamic>(@"select t.* from KM.V_USER_GIS_COMMUNITY t where t.USER_OID IN :ids", new { ids = userId }).ToList();
                                var communityIds = CreateGEOResult(communityList);
                                result.Add("CommunityInfo", communityList);

                                //楼宇
                                var buildingList = kMconn.Query<dynamic>(@"select t.* from KM.v_user_gis_building t where t.USER_OID IN :ids", new { ids = userId }).ToList();
                                var buildingIds = CreateGEOResult(buildingList);
                                result.Add("buildingInfo", buildingList);

                                //小区|楼宇下的用户
                                if (buildingIds.Count > 0 || communityIds.Count > 0)
                                {
                                    var userList = kMconn.Query<dynamic>("select distinct OCODE as USERNO,'' as MP_WGOID ,'' as SUBTYPE,TMP_YC_ADDRESS  as TMP_YC_ADDR ,OTITLE as USER_OTITLE,'' as \"Coordinates\" from km. Vdw_Md_Object_Map_V2  v left join  KM.Vcls_User  u on v.MP_ID = u.oid where v.mp_children_cls_id = 53 start with  v.MP_id in :ids connect by prior MP_id = MP_pervid", new { ids = buildingIds.Concat(communityIds) }).ToList();
                                    if (userList.Count > 0)
                                    {
                                        userinfo = userinfo.Concat(userList).ToList();
                                    }

                                }
                            }
                            else
                            {
                                result.Add("CommunityInfo", new List<object>());
                                result.Add("buildingInfo", new List<object>());
                            }
                            result.Add("UserInfo", userinfo);
                            result.Add("MeterInfo", meterInfo);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }

            return result;
        }

        /// <summary>
        /// 创建康明小区/楼宇geo实体
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public List<int> CreateGEOResult(List<dynamic> records)
        {
            List<int> ids = new List<int>();
            foreach (var item in records)
            {
                if (null != item.MI_SHAPE_LNG && null != item.MI_SHAPE_LAT)
                {
                    ((IDictionary<string, object>)item).Add("Coordinates", _projNetCoordinateDAL.GCJ02ToS2000(new double[] { (double)item.MI_SHAPE_LNG, (double)item.MI_SHAPE_LAT }));
                }
                else
                {
                    ((IDictionary<string, object>)item).Add("Coordinates", "");
                }
                if (!string.IsNullOrEmpty(item.MI_SHAPE) && item.MI_SHAPE != "[]")
                {
                    var wktList = JsonConvert.DeserializeObject(item.MI_SHAPE);
                    List<double[]> coordinatesList = new List<double[]>();
                    List<IList<double[]>> polygonList = new List<IList<double[]>>();
                    foreach (var wkt in wktList)
                    {
                        coordinatesList.Clear();
                        foreach (var coordinates in wkt.geometry.coordinates[0])
                        {
                            coordinatesList.Add(new double[] { coordinates[0], coordinates[1] });
                        }
                        polygonList.Add(_projNetCoordinateDAL.GCJ02ToS2000List(coordinatesList));

                    }
                    ((IDictionary<string, object>)item).Add("Polygon", polygonList);
                }
                else
                {
                    ((IDictionary<string, object>)item).Add("Polygon", Array.Empty<object>());
                }
                ids.Add((int)item.OID);
            }
            return ids;
        }

        /// <summary>
        /// 创建水表关联实体
        /// </summary>
        /// <param name="pipeRelated"></param>
        /// <param name="userinfo"></param>
        /// <param name="meterInfo"></param>
        /// <param name="communityInfo"></param>
        /// <param name="buildingInfo"></param>
        /// <param name="kMconn">康明数据库</param>
        public void CreateMeterRelatedData(List<dynamic> pipeRelated, out List<object> userinfo, out List<object> meterInfo, out List<object> communityInfo, out List<object> buildingInfo, IDbConnection kMconn)
        {
            communityInfo = new List<object>();
            buildingInfo = new List<object>();
            userinfo = new List<object>();
            meterInfo = new List<object>();
            List<int> userId = new List<int>();
            if (pipeRelated.Count > 0)
            {
                int currentUserId = 0;
                foreach (var item in pipeRelated)
                {
                    if (null != item.MI_SHAPE_LNG && null != item.MI_SHAPE_LAT)
                    {
                        double[] coordinates = new double[] { (double)item.MI_SHAPE_LNG, (double)item.MI_SHAPE_LAT };
                        ((IDictionary<string, object>)item).Add("Coordinates", _projNetCoordinateDAL.GCJ02ToS2000(coordinates));
                    }
                    else
                    {
                        ((IDictionary<string, object>)item).Add("Coordinates", "");
                    }
                    currentUserId = (int)item.USER_OID;
                    if (!userId.Contains(currentUserId))
                    {
                        userId.Add(currentUserId);
                        userinfo.Add(new { item.USERNO, item.MP_WGOID, item.SUBTYPE, item.TMP_YC_ADDR, item.USER_OTITLE, item.Coordinates });
                        meterInfo.Add(new { item.M_CODE, item.M_POS, item.M_POSDESC, item.M_DN, item.M_PROPERTY, item.M_INSTALLDA, item.Coordinates });
                    }
                }
            }
            if (userId.Count > 0)
            {

                var pipeDal = new PipeDAL(_projNetCoordinateDAL);
                //小区
                communityInfo = kMconn.Query<dynamic>(@"select distinct t.OID,
                t.OCODE,
                t.OTITLE,
                t.s_chapmanstate,
                t.mi_shape_lng,
                t.mi_shape_lat,
                dbms_lob.substr(t.mi_shape, 4000, 1) as mi_shape,
                '' as user_oid
  from KM.V_USER_GIS_COMMUNITY t where t.USER_OID IN :ids and  t.OID is not null", new { ids = userId }).ToList();
                var communityIds = pipeDal.CreateGEOResult(communityInfo);


                //楼宇
                buildingInfo = kMconn.Query<dynamic>(@"select distinct t.OID,
       t.OCODE,
       t.OTITLE,
       t.s_chapmanstate,
       t.mi_shape_lng,
       t.mi_shape_lat,
       dbms_lob.substr(t.mi_shape, 4000, 1) as mi_shape,
       '' as user_oid
        from KM.v_user_gis_building t where t.USER_OID IN :ids and  t.OID is not null", new { ids = userId }).ToList();
                var buildingIds = pipeDal.CreateGEOResult(buildingInfo);

                //小区|楼宇下的用户
                if (buildingIds.Count > 0 || communityIds.Count > 0)
                {
                    var userList = kMconn.Query<dynamic>("select distinct OCODE as USERNO,'' as MP_WGOID ,'' as SUBTYPE,TMP_YC_ADDRESS  as TMP_YC_ADDR ,OTITLE as USER_OTITLE,'' as \"Coordinates\" from km. Vdw_Md_Object_Map_V2  v left join  KM.Vcls_User  u on v.MP_ID = u.oid where v.mp_children_cls_id = 53 start with  v.MP_id in :ids connect by prior MP_id = MP_pervid", new { ids = buildingIds.Concat(communityIds) }).ToList();
                    if (userList.Count > 0)
                    {
                        userinfo= userinfo.Concat(userList).ToList();
                    }

                }
            }
        }
    }

}