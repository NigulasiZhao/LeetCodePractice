using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;


namespace GISWaterSupplyAndSewageServer.IDAL.Gis_AlarmSetting
{
   public interface IGis_AlarmSettingDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Model.AlarmSetting.GIS_Alarm_Setting model);
        MessageEntity Update(Model.AlarmSetting.GIS_Alarm_Setting model);
        MessageEntity Delete(Model.AlarmSetting.GIS_Alarm_Setting model);
    }
}
