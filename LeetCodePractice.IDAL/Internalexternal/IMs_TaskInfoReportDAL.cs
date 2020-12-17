using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.IDAL.Internalexternal
{
    public interface IMs_TaskInfoReportDAL : IDependency
    {
        MessageEntity Add(Ms_TaskManagement model);

    }
}