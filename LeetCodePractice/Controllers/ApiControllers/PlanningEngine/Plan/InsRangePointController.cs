using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
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
   /// 巡检范围关键点
   /// </summary>
    public class InsRangePointController : ControllerBase
    {
        private readonly IIns_Range_PointDAL _ins_Range_PointDAL;

        public InsRangePointController(IIns_Range_PointDAL ins_Range_PointDAL)
        {
            _ins_Range_PointDAL = ins_Range_PointDAL;
        }

        /// <summary>
        /// 根据巡检范围获取关键点信息
        /// </summary>
        /// <param name="range_id">巡检范围id</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string range_id)
        {
            var result = _ins_Range_PointDAL.GetList(range_id);
            return result;
        }


        /// <summary>
        /// 新增关键点
        /// </summary>
        /// <param name="value">Range_id:巡检范围主键  Range_point_name:关键点名称  Lon：经度  Lat：纬度</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Range_Point value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Range_point_name = value.Range_point_name.Replace(" ", "");
            if (string.IsNullOrEmpty(value.Range_point_name) || string.IsNullOrEmpty(value.Lat) || string.IsNullOrEmpty(value.Lon))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            MessageEntity messresult = _ins_Range_PointDAL.IsExistRangePoint(value, 1);
            List<Ins_Range_Point> ptslist = (List<Ins_Range_Point>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许添加重复关键点！");
            }
            return _ins_Range_PointDAL.Add(value);
        }

        /// <summary>
        /// 修改关键点
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Range_Point value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Range_point_name = value.Range_point_name.Replace(" ", "");
            if (string.IsNullOrEmpty(value.Range_point_name) || string.IsNullOrEmpty(value.Lat) || string.IsNullOrEmpty(value.Lon))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Range_point_id = ID;
            MessageEntity messresult = _ins_Range_PointDAL.IsExistRangePoint(value, 0);
            List<Ins_Range_Point> ptslist = (List<Ins_Range_Point>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许修改为重复关键点！");
            }
            return _ins_Range_PointDAL.Update(value);
        }

        /// <summary>
        /// 删除关键点
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _ins_Range_PointDAL.Delete(new Ins_Range_Point { Range_point_id = ID });
        }
    }
}