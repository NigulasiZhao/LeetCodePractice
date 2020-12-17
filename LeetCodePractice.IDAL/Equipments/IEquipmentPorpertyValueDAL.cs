using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.IDAL.Equipments
{
 public interface IEquipmentPorpertyValueDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity GetValueInfo(string equipmentPorpertyId, string ids, string selectionValues, int isAdd);
        MessageEntity Add(EquipmentPorpertyValue model);
        MessageEntity Update(EquipmentPorpertyValue model);
        MessageEntity Delete(EquipmentPorpertyValue model);
    }
}
