using System.Collections.Generic;
using System.Linq;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.Equipments;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Equipments
{
    /// <summary>
    /// 获取所有设备的下拉框内容信息
    /// </summary> 
    public class EquipmentPorpertyByMappingController : Controller
    {
        private readonly IEquipmentPorpertyMappingDAL _equipmentPorpertyMappingDAL;


        public EquipmentPorpertyByMappingController(IEquipmentPorpertyMappingDAL equipmentPorpertyMappingDAL)
        {
            _equipmentPorpertyMappingDAL = equipmentPorpertyMappingDAL;
        }
        /// <summary>
        /// 获取所有设备的下拉框内容信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get()
        {
            //获取到所有的设备
            List<EquipmentPorpertyMappingList> listequiment = _equipmentPorpertyMappingDAL.GetEquipment();
            //获取到设备所有下拉框属性
            List<EquipmentPorpertyList> listPorperty = _equipmentPorpertyMappingDAL.GetEquipmentPorperty();
            //获取到所有的设备下拉框对应的内容
            List<EquipmentPorpertyValue> listPorpertyValue = _equipmentPorpertyMappingDAL.GetEquipmentPorpertyValue();
            foreach (EquipmentPorpertyMappingList item in listequiment)
            {
                //获取对应设备的下拉框属性
                List<EquipmentPorpertyList> equlist = listPorperty.Where(x => x.EName == item.EName).ToList();
                foreach (EquipmentPorpertyList model in equlist)
                {
                    //获取设备对应属性的下拉框内容
                    List<EquipmentPorpertyValue> valuelist = listPorpertyValue.Where(x => x.EquipmentId == model.EquipmentId && x.EquipmentPorpertyId == model.EquipmentPorpertyId).ToList();
                    model.EquipmentPorpertyValueGroup = valuelist;
                }
                item.EquipmentPorpertyGroup = equlist;
            }
            var json = JsonConvert.SerializeObject(listequiment);
            return MessageEntityTool.GetMessage(1, json, true, "完成");
        }
      
    }
}