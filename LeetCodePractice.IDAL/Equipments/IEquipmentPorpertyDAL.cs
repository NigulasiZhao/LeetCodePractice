
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
    public interface IEquipmentPorpertyDAL : IDependency
    {
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
        EquipmentPorperty GetInfo(string ID);
        MessageEntity Add(EquipmentPorperty model);
        MessageEntity Update(EquipmentPorperty model);
        MessageEntity Delete(EquipmentPorperty model);
    }
}
