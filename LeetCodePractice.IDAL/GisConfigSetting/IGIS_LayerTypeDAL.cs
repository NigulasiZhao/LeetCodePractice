using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting
{
    /// <summary>
    /// 图层类别接口
    /// </summary>
    public interface  IGIS_LayerTypeDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity GetLayerTypeList(string sqlCondition);
        MessageEntity Add(GIS_LayerType model);
        MessageEntity Update(GIS_LayerType model);
        MessageEntity Delete(GIS_LayerType model);
        GIS_LayerType GetInfo(string ID);

    }
}
