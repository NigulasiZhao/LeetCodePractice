using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GisPlateform.CommonTools;
using GisPlateform.Model;
using GisPlateform.Model.BaseEntity;
using GisPlateformForCore.IDAL.PlanningEngine.InspectionSettings;
using GisPlateformForCore.Model.PlanningEngine.InspectionSettings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISServerForCore2._0.Controllers.ApiControllers.PlanningEngine.InspectionSettings
{
    /// <summary>
    /// 事件类型管理
    /// </summary>
    public class InsEventTypeController :  ControllerBase
    {
        private readonly IIns_Event_TypeDAL _eventTypeDAL;
        public InsEventTypeController(IIns_Event_TypeDAL eventTypeDAL)
        {
            _eventTypeDAL = eventTypeDAL;

        }  /// <summary>
           ///  获取事件分类
           /// </summary>
           /// <param name="sort">排序(默认EventTypeName)</param>
           /// <param name="ordering">asc/desc</param>
           /// <param name="num">每页多少行</param>
           /// <param name="page">第几页</param>
           /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string sort = "EventTypeName", string ordering = "desc", int num = 20, int page = 1)
        {
            var messageEntity = _eventTypeDAL.GetEventType(sort, ordering, num, page);
            return messageEntity;
        }
        /// <summary>
        /// 获取事件分类列表
        /// </summary>
        /// <returns>EventTypeId 事件类型eventTypeId EventTypeName 事件类型Name</returns>
        [HttpGet]
        public MessageEntity GetCommboboxList()
        {
            var messageEntity = _eventTypeDAL.GetEventTypeCommboBoxList();
            return messageEntity;
        }
        /// <summary>
        /// 获取事件分类列表(包含事件内容)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetEventTypeContent()
        {
            var messageEntity = _eventTypeDAL.GetEventTypeContent();
            return messageEntity;
        }
        /// <summary>
        /// 添加事件类型
        /// </summary>
        /// <param name="eventType"> string EventTypeName 事件名称
        /// /int ExecTime 执行时间/int ParentTypeId 上级分类Id(不用传)/</param>
        /// <returns></returns>
        // POST api/<controller>
        [HttpPost]
        public MessageEntity Post([FromBody]Ins_Event_Type eventType)
        {
            eventType.ParentTypeId = "00000000-0000-0000-0000-000000000000";
            eventType.EventTypeName = eventType.EventTypeName.Replace(" ", "");
            MessageEntity messresult = _eventTypeDAL.IsExistEventType(eventType, 1);
            List<Ins_Event_Type> ptslist = (List<Ins_Event_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许添加重复事件类型名称！");
            }
            var messageEntity = _eventTypeDAL.AddEventType(eventType);
            return messageEntity;
        }

        /// <summary>
        /// 修改事件类型
        /// </summary>
        /// <param name="eventTypeId">事件类型eventTypeId</param>
        /// <param name="eventType"> string EventTypeName 事件名称/int ExecTime 执行时间/int ParentTypeId 上级分类Id(不用传)/</param>
        [HttpPut]
        public MessageEntity Put(string eventTypeId, [FromBody]Ins_Event_Type eventType)
        {
            if (string.IsNullOrEmpty(eventType.EventTypeName))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            eventType.ParentTypeId = "00000000-0000-0000-0000-000000000000";
            eventType.Event_Type_id = eventTypeId;
            eventType.EventTypeName = eventType.EventTypeName.Replace(" ", "");
            MessageEntity messresult = _eventTypeDAL.IsExistEventType(eventType, 0);
            List<Ins_Event_Type> ptslist = (List<Ins_Event_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许修改为重复事件类型名称！");
            }
            var messageEntity = _eventTypeDAL.UpdateEventType(eventType);

            return messageEntity;
        }

        /// <summary>
        /// 删除事件类型
        /// </summary>
        /// <param name="eventTypeId">事件类型eventTypeId</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string eventTypeId)
        {
            Ins_Event_Type eventType = new Ins_Event_Type
            {
                Event_Type_id = eventTypeId
            };
            //判断是否被事件内容应用
            MessageEntity messresult = _eventTypeDAL.IsExistEventcontent(eventTypeId);
            List<Ins_Event_Type> ptslist = (List<Ins_Event_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "该类型下存在内容,请先删除对应事件内容！");
            }
            var messageEntity = _eventTypeDAL.DeleteEventType(eventType);

            return messageEntity;

        }
    }
}