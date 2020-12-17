using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;

namespace GISWaterSupplyAndSewageServer.IDAL.EssentialFactor
{
   public interface IEssentialFactorDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Model.EssentialFactor.GIS_EssentialFactor model);
        MessageEntity Update(Model.EssentialFactor.GIS_EssentialFactor model);
        MessageEntity Delete(Model.EssentialFactor.GIS_EssentialFactor model);
        List<GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor> GetTree(string pID= "00000000-0000-0000-0000-000000000000", int eFType = 0);
    }
}
