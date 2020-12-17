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
    /// GIS基础配置表接口
    /// </summary>
    public interface IGIS_ConfigureDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page,string sqlCondition);

        MessageEntity GetMapConfig();

        GIS_Configure GetInfo(string ID);
        MessageEntity Add(GIS_Configure model);
        MessageEntity Update(GIS_Configure model);
        MessageEntity Delete(GIS_Configure model);
    }
}
