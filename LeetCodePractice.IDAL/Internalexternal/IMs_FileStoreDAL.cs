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
        public interface IMs_FileStoreDAL : IDependency
        {
            MessageEntity GetList(List<Model.ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
            MessageEntity Add(Ms_FileStore model);
            MessageEntity GetFileInfoByid(string fID);
            MessageEntity IsExistFileHash(string fileKey);
    }
}
