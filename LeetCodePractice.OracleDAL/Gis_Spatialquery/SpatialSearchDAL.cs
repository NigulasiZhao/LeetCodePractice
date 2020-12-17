using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.IDAL.CoordinateTransf;
using GISWaterSupplyAndSewageServer.IDAL.Gis_Spatialquery;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using NPOI.OpenXmlFormats;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Gis_Spatialquery
{
    public class SpatialSearchDAL : ISpatialSearchDAL
    {
        private readonly IProjNetCoordinateDAL _projNetCoordinateDAL;
        private readonly IPipeDAL _pipeDAL;
        private readonly object _equipmentCountDicLock = new object();
        private Dictionary<string, int> EquipmentCountDic = new Dictionary<string, int>();
        public SpatialSearchDAL(IProjNetCoordinateDAL projNetCoordinateDAL, IPipeDAL pipeDAL)
        {
            _projNetCoordinateDAL = projNetCoordinateDAL;

            _pipeDAL = pipeDAL;
        }
        public MessageEntity GetFirstzoneShape(List<ParameterInfo> parInfo, string sqlCondition)
        {
            string errorMsg = "";
            try
            {

                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE))
                {
                    string sql = @"select objectid, oid, ocode, otitle, a_responman, a_responphone, a_admin,a_adminphone,a_supervision,a_supervisionphone, a_company, a_level, a_areadesc,  
     createtime, createuser, edittime, edituser, border_width,  area_transparency, border_colour,area_colour,sde.st_astext(shape) shape from sde.MANAGE_FIRSTZONE 
 " + sqlCondition;
                    List<dynamic> eventType = conn.Query<dynamic>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }

        }

        public Dictionary<string, List<object>> GetUserInfo(double lng, double lat, int pointBuffer)
        {
            List<object> userinfo = new List<object>();
            List<object> meterInfo = new List<object>();
            List<object> communityInfo = new List<object>();
            List<object> buildingInfo = new List<object>();


            using IDbConnection conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE);
            var meterCode = conn.Query<string>($@"select t.ocode
          from (select t.ocode,
                       st_distance(t.shape,
                                   (select st_geometry('POINT ({lng} {lat})',
                                                       4326)
                                      from dual),
                                   'Meter') as distance
                  from KM_METER t) t
         where t.distance < {pointBuffer}
         order by t.distance");

            if (meterCode.Count() > 0)
            {
                using IDbConnection kMconn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.KM);

                var meterRelatedData = CreateMeterData(meterCode, kMconn);

                _pipeDAL.CreateMeterRelatedData(meterRelatedData, out userinfo, out meterInfo, out communityInfo, out buildingInfo, kMconn);
            }
            Dictionary<string, List<object>> infoList = new Dictionary<string, List<object>>
            {
                { "userinfo", userinfo },
                { "meterInfo", meterInfo },
                { "communityInfo", communityInfo },
                { "buildingInfo", buildingInfo }
            };
            return infoList;
        }

        public Dictionary<string, List<object>> GetUserInfo(List<List<double[]>> multilinestring, int lineBuffer)
        {
            List<object> userinfo = new List<object>();
            List<object> meterInfo = new List<object>();
            List<object> communityInfo = new List<object>();
            List<object> buildingInfo = new List<object>();


            string wktStr = "multilinestring (";

            for (int i = 0; i < multilinestring.Count; i++)
            {
                for (int j = 0; j < multilinestring[i].Count; j++)
                {
                    if (j == 0)
                    {
                        wktStr += $"({multilinestring[i][j][0]} {multilinestring[i][j][1]},";
                    }
                    else if (j != multilinestring[i].Count() - 1)
                    {
                        wktStr += $" {multilinestring[i][j][0]} {multilinestring[i][j][1]},";
                    }
                    else
                    {
                        wktStr += $" {multilinestring[i][j][0]} {multilinestring[i][j][1]})";
                    }
                }
                if (i == multilinestring.Count - 1)
                {
                    wktStr += ")";
                }
                else
                {
                    wktStr += ",";
                }
            }
            using IDbConnection conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE);
            //var meterCode = conn.Query<string>($@" select  t.ocode from  KM_METER t WHERE sde.st_intersects ((select st_buffer ((select st_geometry('{wktStr}',4326) from dual), {lineBuffer}, 'Meter') from dual),t.shape)=1");

            OracleDynamicParameters dynParams = new OracleDynamicParameters();
            var sql = $@"DECLARE
                  str varchar2(32767);
                BEGIN
                  str := '{wktStr}';
                 OPEN :rslt1 FOR select  t.ocode from  KM_METER t WHERE sde.st_intersects ((select st_buffer ((select st_geometry(str,4326) from dual), {lineBuffer}, 'Meter') from dual),t.shape)=1;
 
                END;";

            dynParams.Add(":rslt1", null, OracleMappingType.RefCursor, ParameterDirection.Output);
            var meterCode = conn.Query<string>(sql, param: dynParams);
            if (meterCode.Count() > 0)
            {
                using IDbConnection kMconn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.KM);

                var meterRelatedData = CreateMeterData(meterCode, kMconn);

                _pipeDAL.CreateMeterRelatedData(meterRelatedData, out userinfo, out meterInfo, out communityInfo, out buildingInfo, kMconn);
            }
            Dictionary<string, List<object>> infoList = new Dictionary<string, List<object>>
            {
                { "userinfo", userinfo },
                { "meterInfo", meterInfo },
                { "communityInfo", communityInfo },
                { "buildingInfo", buildingInfo }
            };
            return infoList;
        }

        public Dictionary<string, List<object>> GetUserInfo(List<double[]> multipoint, int pointBuffer)
        {
            List<object> userinfo = new List<object>();
            List<object> meterInfo = new List<object>();
            List<object> communityInfo = new List<object>();
            List<object> buildingInfo = new List<object>();


            string wktStr = "multipoint (";

            for (int i = 0; i < multipoint.Count; i++)
            {

                if (i == multipoint.Count - 1)
                {
                    wktStr += $"{multipoint[i][0]} {multipoint[i][1]})";
                }
                else
                {
                    wktStr += $"{multipoint[i][0]} {multipoint[i][1]}, ";
                }
            }
            using IDbConnection conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE);
            //var meterCode = conn.Query<string>($@" select  t.ocode from  KM_METER t WHERE sde.st_intersects ((select st_buffer ((select st_geometry('{wktStr}',4326) from dual), {lineBuffer}, 'Meter') from dual),t.shape)=1");

            OracleDynamicParameters dynParams = new OracleDynamicParameters();
            var sql = $@"DECLARE
                  str varchar2(32767);
                BEGIN
                  str := '{wktStr}';
                 OPEN :rslt1 FOR select  t.ocode from  KM_METER t WHERE sde.st_intersects ((select st_buffer ((select st_geometry(str,4326) from dual), {pointBuffer}, 'Meter') from dual),t.shape)=1;
 
                END;";

            dynParams.Add(":rslt1", null, OracleMappingType.RefCursor, ParameterDirection.Output);
            var meterCode = conn.Query<string>(sql, param: dynParams);
            if (meterCode.Count() > 0)
            {
                using IDbConnection kMconn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.KM);

                var meterRelatedData = CreateMeterData(meterCode, kMconn);

                _pipeDAL.CreateMeterRelatedData(meterRelatedData, out userinfo, out meterInfo, out communityInfo, out buildingInfo, kMconn);
            }
            Dictionary<string, List<object>> infoList = new Dictionary<string, List<object>>
            {
                { "userinfo", userinfo },
                { "meterInfo", meterInfo },
                { "communityInfo", communityInfo },
                { "buildingInfo", buildingInfo }
            };
            return infoList;
        }


        public IDictionary<string, object> PaginationSearch(string[] keyWords, IList<GIS_LayerSearchField> lsf, IList<double[]> geo, IDictionary<string, int> equipmentCountDic, int[] grids,string[] districts, int offset, int record)
        {
            List<object> data = new List<object>();
            bool hasSpatialData = geo.Count > 0;
            bool hasKeyWords = keyWords.Length > 0;
            string wktStr = string.Empty;

            //拼接plygon
            if (hasSpatialData)
            {
                wktStr = "polygon ((";
                for (int i = 0; i < geo.Count; i++)
                {
                    if (i == geo.Count - 1)
                    {
                        wktStr += $"{geo[i][0]} {geo[i][1]}))";
                    }
                    else
                    {
                        wktStr += $"{geo[i][0]} {geo[i][1]}, ";
                    }
                }
            }
            using IDbConnection conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE);
            var tasks = new List<Task>();
            Task t;
            //查询所有设备Count
            if (equipmentCountDic == null || equipmentCountDic.Count() <= 0)
            {
                equipmentCountDic = new Dictionary<string, int>();

                OracleDynamicParameters dynParams;
                for (int k = 0; k < lsf.Count; k++)
                {
                    if (lsf[k].Fields.Count >= 1&& lsf[k].Parameters.Count() >= 1)
                    {
                        dynParams = new OracleDynamicParameters();
                        var countSql = this.CreatePaginationSql(dynParams, lsf[k], true, keyWords, wktStr, grids, districts);
                        if (conn.State != ConnectionState.Open)
                        {
                            tasks.ForEach(p => p.Dispose());
                            throw new Exception("网络连接有问题");
                        }
                        
                        t = Task.Run(() =>
                        {
                            
                            var count = conn.Query(countSql, dynParams).ToList().Select(x => (IDictionary<string, object>)x).ToList().First().First();
                            AddEquipmentCountDic(count.Key, Convert.ToInt32(count.Value));

                        });
                        tasks.Add(t);
                    }
                    else
                    {
                        throw new Exception($"GIS_LayerSearchField.json数据有误--{lsf[k].Name}没有Fields或");
                    }
                }
                Task.WaitAll(tasks.ToArray());
                equipmentCountDic = this.EquipmentCountDic;
            }


            OracleDynamicParameters dynParams4Layers = new OracleDynamicParameters();
            //根据设备数量 计算每个layer需要查询的分页数据
            string layerInfoSql = "BEGIN ";
            GIS_LayerSearchField gIS_Layer;
            if (equipmentCountDic.Values.Sum() < offset)
            {
                throw new Exception("offset参数不能大于总行数");
            }
            equipmentCountDic = new Dictionary<string, int>(equipmentCountDic.OrderBy(p => p.Key).ToArray());
            foreach (var item in equipmentCountDic)
            {

                //当前表需要查询的数量
                int currentLayerRecord = 0;
                //如果当前layer的数量够offset的数量 则减去offset 然后继续
                if (item.Value <= offset)
                {
                    offset -= item.Value;
                    continue;
                }
                //offset计算完毕
                else
                {
                    //当前layer需要能够查询的数量
                    currentLayerRecord = item.Value - offset;
                    //如果当前layer数量不小于需要查询的行数 则直接查询并跳出循环
                    gIS_Layer = lsf.Where(p => p.Name == item.Key).FirstOrDefault();
                    if (currentLayerRecord >= record)
                    {
                        layerInfoSql = CreatePaginationSql(dynParams4Layers, gIS_Layer, false, keyWords, wktStr, grids, districts, offset, record);
                        var layerinfo = conn.Query<dynamic>(layerInfoSql, dynParams4Layers).ToList();
                        data.Add(layerinfo);
                        break;
                    }
                    //如果当前layer数量小于 则将总的需要的行数减去当前layer计算后的行数
                    else
                    {
                        record -= currentLayerRecord;
                        layerInfoSql = CreatePaginationSql(dynParams4Layers, gIS_Layer, false, keyWords, wktStr, grids, districts, offset, currentLayerRecord);
                        var layerinfo = conn.Query<dynamic>(layerInfoSql, dynParams4Layers).ToList();

                        data.Add(layerinfo);
                    }

                    offset = 0;
                }
            }
            //layerInfoSql += "END;";
            //using var multi4Layers = conn.QueryMultiple(layerInfoSql, dynParams4Layers);

            //while (!multi4Layers.IsConsumed)
            //{
            //    //读取当前结果集
            //    data.Add(multi4Layers.Read());
            //}
            IDictionary<string, object> result = new Dictionary<string, object>
            {
                { "geoData", data },
                { "equipmentCountDic", equipmentCountDic }
            };
            return result;
        }

        private void AddEquipmentCountDic(string key, int value)
        {
            lock (_equipmentCountDicLock)
            {
                this.EquipmentCountDic.Add(key, value);
            }
        }

        private string CreatePaginationSql(OracleDynamicParameters dynParams, GIS_LayerSearchField lsf, bool isCountOnly, string[] keyWords, string wktStr, int[] grids,string[] districts, int? offset = null, int? record = null)
        {
            string spatialWhere = !string.IsNullOrEmpty(wktStr) ? $" and sde.st_intersects (shape,(select st_geometry('{wktStr}',4547) from dual))=1 and st_isempty(shape) = 0 " : "";
            string keyWordsWhere = string.Empty;
            string gridsWhere = string.Empty;
            string districtsWhere = string.Empty;
            if (keyWords.Length > 0)
            {
                keyWordsWhere = " and (";
                for (int j = 0; j < lsf.Parameters.Count(); j++)
                {
                    if (j != 0)
                        keyWordsWhere += " or ";

                    for (int i = 0; i < keyWords.Length; i++)
                    {
                        if (i == 0)
                            keyWordsWhere += $" {lsf.Parameters[j]} like :keyWord{i} ";
                        else
                            keyWordsWhere += $" or {lsf.Parameters[j]} like :keyWord{i} ";
                        if (!dynParams.ParameterNames.Contains($"keyWord{i}"))
                        {
                            dynParams.Add($":keyWord{i}", $"%{keyWords[i]}%", OracleMappingType.Varchar2, ParameterDirection.Input);
                        }
                    }
                }
                keyWordsWhere += " )";
            }

            if (grids.Length > 0)
            {
                string gridInSql = "";
                for (int i = 0; i < grids.Length; i++)
                {
                    gridInSql += $",'{grids[i]}'";
                }
                gridInSql = gridInSql.Remove(0, 1);

                gridsWhere = $" and grid in ({gridInSql}) ";
            }

            if (districts.Length > 0)
            {
                string districtInSql = "";
                for (int i = 0; i < districts.Length; i++)
                {
                    districtInSql += $",'{districts[i]}'";
                }
                districtInSql = districtInSql.Remove(0, 1);

                districtsWhere = $" and district in ({districtInSql}) ";
            }

            string sql;
            if (isCountOnly)
            {
                sql = $" select Count(0) as {lsf.Name} from {lsf.ClassName} where 1=1 {spatialWhere} {keyWordsWhere} {gridsWhere} {districtsWhere}";
            }
            else
            {
                var fields = string.Join(",", lsf.Fields.Keys);
                //dynParams.Add($":rslt{lsf.TableName}", null, OracleMappingType.RefCursor, ParameterDirection.Output);
                //sql = $" OPEN :rslt{lsf.TableName} FOR select {fields + ",shape"} from (select {fields + ",st_astext(shape) as shape"},rownum rowno from {lsf.TableName} where rownum<{offset.Value + record } {spatialWhere} {keyWordsWhere} {gridsWhere}) where rowno >={offset.Value};";
                dynParams.Add($":rslt{lsf.ClassName}", null, OracleMappingType.RefCursor, ParameterDirection.Output);
                sql = $" select {fields},shape,'{lsf.Name}' as \"className\" from (select {fields + ",st_astext(shape) as shape"},rownum rowno from {lsf.ClassName} where rownum<{offset.Value + record } {spatialWhere} {keyWordsWhere} {gridsWhere}) where rowno >={offset.Value}";
            }
            return sql;
        }

        public MessageEntity GetSeconozoneShape(List<ParameterInfo> parInfo, string sqlCondition)
        {

            string errorMsg = "";
            try
            {

                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE))
                {
                    string sql = @"select objectid, oid, ocode, otitle, a_responman, a_responphone, a_admin,a_adminphone,a_supervision,a_supervisionphone, a_company, a_level, a_areadesc,  
createtime, createuser, edittime, edituser, border_width,  area_transparency, border_colour,area_colour,a_firstcode,a_firstoid,sde.st_astext(shape) shape from sde.manage_secondzone 
 " + sqlCondition;
                    List<dynamic> eventType = conn.Query<dynamic>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }

        }

        public MessageEntity GetUnitShape(List<ParameterInfo> parInfo, string sqlCondition)
        {
            string errorMsg = "";
            try
            {

                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.SDE))
                {
                    string sql = @"select objectid,  oid,ocode, otitle,wg_responman, wg_responphone, wg_admin,  wg_adminphone, wg_supervision, wg_supervisionphone,wg_company, wg_geigdesc, createtime, createuser,edittime, edituser, border_width, area_transparency, border_colour,
                            area_colour,a_firstcode, a_firstoid, a_secondcode, a_secondoid,sde.st_astext(shape) shape from sde.manage_unit  " + sqlCondition;
                    List<dynamic> eventType = conn.Query<dynamic>(sql).ToList();

                    return MessageEntityTool.GetMessage(eventType.Count(), eventType, true, "", eventType.Count());
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }

        }

        private List<dynamic> CreateMeterData(IEnumerable<string> meterCodes, IDbConnection kMconn)
        {
            string querySql = $@"SELECT    ugp.M_CODE,
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
                                                      vmm.mi_shape_lat,
                                                      vmm.mi_shape_lng 
                                                    FROM
                                                      km.v_user_gis_pipe ugp
                                                      left JOIN km.vcls_user cvu ON cvu.ocode = ugp.user_code
                                                      left JOIN km.vdw_md_mapitems vmm ON vmm.mi_tar_clsid = 53 
                                                      AND vmm.mi_tar_oid = cvu.oid
                                                    WHERE  ugp.M_CODE in :ids";
            var meterRelatedData = kMconn.Query<dynamic>(querySql, new { ids = meterCodes }).ToList();
            return meterRelatedData;
        }
    }
}
