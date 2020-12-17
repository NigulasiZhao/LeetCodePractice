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
    /// 搜素字段配置接口
    /// </summary>
    public interface IGIS_SearchFieldDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(GIS_SearchField model);
        MessageEntity Update(GIS_SearchField model);
        MessageEntity Delete(GIS_SearchField model);
    }
}
