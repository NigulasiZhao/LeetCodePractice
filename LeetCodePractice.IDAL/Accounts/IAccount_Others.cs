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
    /// 其他台账
    /// </summary> 
    public interface IAccount_Others : IDependency
    {
        MessageEntity GetStatistics(List<ParameterInfo> parInfo, List<string> fieldsInfo, List<string> fieldsCount, string sqlCondition);
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page,string sqlCondition);
        MessageEntity Add(Gis_Account_Others model);
        MessageEntity Update(Gis_Account_Others model);
        MessageEntity Delete(Gis_Account_Others model);
    }
}
