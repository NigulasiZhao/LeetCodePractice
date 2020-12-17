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
    /// 地图基础数据配置字段接口
    /// </summary>
    public interface IGIS_DataTableColumnDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page,string sqlCondition);
        MessageEntity Add(GIS_DataTableColumn model);
        MessageEntity Update(GIS_DataTableColumn model);
        MessageEntity Delete(GIS_DataTableColumn model);
        GIS_DataTableColumn GetInfo(string ID);
    }
}
