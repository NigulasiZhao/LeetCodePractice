using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using GISWaterSupplyAndSewageServer.IDAL.Plan;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Plan
{
    /// <summary>
    /// 设备类型下拉框
    /// </summary>
    public class InsGisFeatureLayerListController : ControllerBase
    {
        private readonly IGIS_FeatureLayerCollectionDAL _gIS_FeatureLayerCollectionDAL;

        public InsGisFeatureLayerListController(IGIS_FeatureLayerCollectionDAL gIS_FeatureLayerCollectionDAL)
        {
            _gIS_FeatureLayerCollectionDAL = gIS_FeatureLayerCollectionDAL;
        }



        /// <summary>
        /// 获取所有设备类型-下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get()
        {
            MessageEntity messresult = _gIS_FeatureLayerCollectionDAL.Get();
            List<GIS_FeatureLayerCollection> ptslist = (List<GIS_FeatureLayerCollection>)messresult.Data.Result;
            if (ptslist.Count <= 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "没有设备类型记录！");
            }
            List<GisFeatureLayerList> alllist = new List<GisFeatureLayerList>();
            ptslist.ForEach(row =>
            {
                List<GisFeatureLayerList> list = JsonConvert.DeserializeObject<List<GisFeatureLayerList>>(row.featureLayers);
                foreach (GisFeatureLayerList item in list)
                {
                    if (!alllist.Contains(item))
                    {
                        alllist.Add(item);
                    }
                }
          
            });
            var json = JsonConvert.SerializeObject(alllist);
            return MessageEntityTool.GetMessage(1, json, true, "完成");
        }


    }
}