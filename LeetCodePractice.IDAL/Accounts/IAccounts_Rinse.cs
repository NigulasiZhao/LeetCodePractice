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
    /// 管网冲洗台账
    /// </summary>  
    public interface IAccounts_Rinse : IDependency
    {
        MessageEntity GetStatistics(List<ParameterInfo> parInfo, List<string> fieldsInfo, List<string> fieldsCount, string sqlCondition);
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Gis_Accounts_Rinse model);
        MessageEntity Update(Gis_Accounts_Rinse model);
        MessageEntity Delete(Gis_Accounts_Rinse model);
    }
}
