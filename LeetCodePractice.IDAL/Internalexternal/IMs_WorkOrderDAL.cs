using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.IDAL.Internalexternal
{
    public interface IMs_WorkOrderDAL : IDependency
    {
        MessageEntity Add(Ms_WorkOrder model);
        MessageEntity GetList();
        MessageEntity Update(string workorderid, string handlePerson, string isPostcomplete);
        MessageEntity GetWorkOrderByTaskid(string taskId);
        MessageEntity GetWorkOrderByWorkorderid(string workorderid);


    }
}
