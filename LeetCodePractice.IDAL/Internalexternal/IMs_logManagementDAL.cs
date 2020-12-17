using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.IDAL.Internalexternal
{
    public interface IMs_logManagementDAL : IDependency
    {
        MessageEntity GetList(List<Model.ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Ms_logManagement model);
        MessageEntity AddList(List<Ms_logManagement> model);
    }
}
