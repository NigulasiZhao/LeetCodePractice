using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.AttributePack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers
{
    /// <summary>
    /// 管网类查询
    /// </summary>
    public class PipeController : BaseApiController
    {
        private readonly IPipeDAL _pipeDAL;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeDAL"></param>
        public PipeController(IPipeDAL pipeDAL)
        {
            _pipeDAL = pipeDAL;
        }

        /// <summary>
        /// 获取管网图层
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionAttribute(IsCheckAuthority = false)]
        public MessageEntity GetLayers()
        {
            var layers = _pipeDAL.GetLayers(out string errMessge);
            if (layers != null && layers.Count > 0)
            {
                return MessageEntityTool.GetMessage(layers.Count, layers, true, "完成", layers.Count);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError);
            }
        }

        /// <summary>
        /// 获取材质
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionAttribute(IsCheckAuthority = false)]
        public MessageEntity GetMaterial_science()
        {
            var material_science = _pipeDAL.GetMaterial_science(out string errMessge);
            if (material_science != null && material_science.Count > 0)
            {
                material_science.Insert(0, "全部");
                return MessageEntityTool.GetMessage(material_science.Count, material_science, true, "完成", material_science.Count);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError);
            }
        }


        /// <summary>
        /// 获取口径
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionAttribute(IsCheckAuthority = false)]
        public MessageEntity GetCaliber()
        {
            var result = _pipeDAL.GetCaliber(out string errMessge);
            if (result != null && result.Count > 0)
            {
                result.Insert(0, "全部");
                return MessageEntityTool.GetMessage(result.Count, result, true, "完成", result.Count);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError);
            }
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionAttribute(IsCheckAuthority = false)]
        public MessageEntity GetInstallation_address()
        {
            var result = _pipeDAL.Getinstallation_addresses(out string errMessge);
            if (result != null && result.Count > 0)
            {
                result.Insert(0, "全部");
                return MessageEntityTool.GetMessage(result.Count, result, true, "完成", result.Count);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.SystemError);
            }
        }

        /// <summary>
        /// 获取管网信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionAttribute(IsCheckAuthority = false)]
        public MessageEntity GetPipeAndPointByPage(string layerequipment_type, string installation_address, string material_science, string caliber, string startCompletion_date, string endCompletion_date, string sort, string ordering, int num, int page)
        {
            MessageEntity message;
            if (!string.IsNullOrEmpty(startCompletion_date) && !string.IsNullOrEmpty(endCompletion_date))
            {
                if (!DateTime.TryParse(startCompletion_date, out DateTime sDate) || !DateTime.TryParse(endCompletion_date, out DateTime eDate))
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "日期格式有误");
                }
                if (sDate > eDate)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "起始日期不能大于结束日期");
                }
                var result = _pipeDAL.GetPipeAndPointByPage(layerequipment_type, installation_address, material_science, caliber, "", sDate, eDate, sort, ordering, num, page, out string errMessge, out message);

            }
            else
            {
                var result = _pipeDAL.GetPipeAndPointByPage(layerequipment_type, installation_address, material_science, caliber, "", null, null, sort, ordering, num, page, out string errMessge, out message);

            }
            return message;
        }

        /// <summary>
        /// 获取管网统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionAttribute(IsCheckAuthority = false)]
        public MessageEntity GetPipeAndPointStatistic(string equipment_type, string installation_address, string material_science, string caliber, string startCompletion_date, string endCompletion_date, string sort, string ordering, string[] groupFields)
        {
            MessageEntity mEntity;
            if (groupFields != null)
                groupFields = groupFields.Where(s => !string.IsNullOrEmpty(s)).ToArray();


            if (!string.IsNullOrEmpty(startCompletion_date) && !string.IsNullOrEmpty(endCompletion_date))
            {
                if (!DateTime.TryParse(startCompletion_date, out DateTime sDate) || !DateTime.TryParse(endCompletion_date, out DateTime eDate))
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "日期格式有误");
                }
                if (sDate > eDate)
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "起始日期不能大于结束日期");
                }

                var result = _pipeDAL.GetPipeAndPointStatistics(equipment_type, installation_address, material_science, caliber, sDate, eDate, sort, ordering, groupFields, out string errMessge, out mEntity);
            }
            else
            {
                var result = _pipeDAL.GetPipeAndPointStatistics(equipment_type, installation_address, material_science, caliber, null, null, sort, ordering, groupFields, out string errMessge, out mEntity);
            }


            return mEntity;


        }

        /// <summary>
        /// 获取管网信息通过坐标(EPSG:4547)
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">精度</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string latitude, string longitude)
        {

            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                string regexExpression = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
                if (Regex.IsMatch(latitude, regexExpression) && Regex.IsMatch(longitude, regexExpression))
                {
                    var pipe = _pipeDAL.Get(latitude, longitude, out string errMessage);
                    if (pipe != null)
                    {
                        return MessageEntityTool.GetMessage(1, pipe);
                    }
                    else
                    {
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, errMessage);
                    }
                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "坐标格式有误");
                }
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError, "", "坐标格式有误");
            }
        }
    }
}