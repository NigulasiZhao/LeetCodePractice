using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.Statistics
{
    /// <summary>
    /// 数据质量统计
    /// </summary>
    public class DataQualityController : BaseController
    {
        private readonly IGIS_DataQualifyDAL _gIS_DataQualifyDAL;

        public DataQualityController(IGIS_DataQualifyDAL gIS_DataQualifyDAL)
        {
            _gIS_DataQualifyDAL = gIS_DataQualifyDAL;
        }
        /// <summary>
        /// 数据质量统计
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/Statistics/DataQuality")]
        public MessageEntity Get(DateTime? sTime, DateTime? eTime)
        {

            if (sTime != null && eTime != null && eTime <= sTime)
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "开始时间不能大于结束时间");
            var result = _gIS_DataQualifyDAL.GetData(sTime, eTime, out ErrorType errorType, out string errorString);
            if (errorType != ErrorType.Success)
            {
                return MessageEntityTool.GetMessage(errorType, errorString);
            }
            return MessageEntityTool.GetMessage(result.Records.Count, result);
        }
    }
}
