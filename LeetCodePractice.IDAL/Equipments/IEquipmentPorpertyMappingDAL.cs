using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.IDAL.Equipments
{
    public interface IEquipmentPorpertyMappingDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        MessageEntity GetValueInfo(string equipmentPorpertyId, int equipmentId, string id, int isAdd);
        MessageEntity Add(EquipmentPorpertyMapping model);
        MessageEntity Update(EquipmentPorpertyMapping model);
        MessageEntity Delete(EquipmentPorpertyMapping model);
        List<EquipmentPorpertyMappingModel> GetEquipmentPorpertyMapping(string eName, string ePname);
        List<EquipmentPorpertyMappingList> GetEquipment();

        List<EquipmentPorpertyList> GetEquipmentPorperty();
        List<EquipmentPorpertyValue> GetEquipmentPorpertyValue();
    }
}
