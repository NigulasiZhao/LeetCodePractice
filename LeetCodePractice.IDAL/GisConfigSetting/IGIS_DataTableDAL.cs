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
    /// 地图基础数据配置接口
    /// </summary>
   public interface IGIS_DataTableDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(GIS_DataTable model);
        MessageEntity Update(GIS_DataTable model);
        MessageEntity Delete(GIS_DataTable model);
        MessageEntity GetDataTable(List<GIS_DataTable> list);
        GIS_DataTable GetInfo(string ID);
    }
}
