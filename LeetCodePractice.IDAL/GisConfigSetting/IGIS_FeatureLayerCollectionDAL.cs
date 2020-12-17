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
    /// FeatureLayer图层配置接口
    /// </summary>
    public interface IGIS_FeatureLayerCollectionDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(GIS_FeatureLayerCollection model);
        MessageEntity Update(GIS_FeatureLayerCollection model);
        MessageEntity Delete(GIS_FeatureLayerCollection model);
        MessageEntity Get();
    }
}
