using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;

namespace GISWaterSupplyAndSewageServer.IDAL.Gis_Spatialquery
{
    public interface ISpatialSearchDAL : IDependency
    {
        MessageEntity GetFirstzoneShape(List<ParameterInfo> parInfo, string sqlCondition);
        MessageEntity GetSeconozoneShape(List<ParameterInfo> parInfo, string sqlCondition);
        MessageEntity GetUnitShape(List<ParameterInfo> parInfo, string sqlCondition);

        Dictionary<string, List<object>> GetUserInfo(double lng, double lat, int pointBuffer);
        Dictionary<string, List<object>> GetUserInfo(List<List<double[]>> multilinestring, int lineBuffer);
        Dictionary<string, List<object>> GetUserInfo(List<double[]> multipoint, int lineBuffer);
        IDictionary<string, object> PaginationSearch(string[] keyWord, IList<GIS_LayerSearchField> lsf, IList<double[]> geo, IDictionary<string, int> equipmentCountDic, int[] grids,string[] districts, int offset, int record);
    }
}
