using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Internalexternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.IDAL.Internalexternal
{
    public interface IMs_TaskManagementDAL : IDependency
    {
        MessageEntity GetList(List<Model.ParameterInfo> parInfo,string describe, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity Add(Ms_TaskManagement model);
        MessageEntity Update(Ms_TaskManagement model);
        MessageEntity GetAppTaskInfo(string fID);
        MessageEntity GetFileInfoByTaskid(string taskId);
        

    }
}
