using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;

namespace GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting
{
    /// <summary>
    /// 搜索功能接口
    /// </summary>
    public interface IGIS_PublicSearchDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        GIS_PublicSearch GetInfo(string ID);
        List<GIS_PublicSearch> GetPublicSearchInfo();
        
        MessageEntity Add(GIS_PublicSearch model);
        MessageEntity Update(GIS_PublicSearch model);
        MessageEntity Delete(GIS_PublicSearch model);
    }
}
