using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.Accounts;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;

namespace GISWaterSupplyAndSewageServer.IDAL.Accounts
{
    /// <summary>
    /// 阀门启闭台账
    /// </summary>
    public interface IAccount_Valve : IDependency
    {
        MessageEntity GetStatistics(List<ParameterInfo> parInfo, List<string> fieldsInfo, List<string> fieldsCount, string sqlCondition);
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Gis_Account_Valve model);
        MessageEntity Update(Gis_Account_Valve model);
        MessageEntity Delete(Gis_Account_Valve model);

    }
}
