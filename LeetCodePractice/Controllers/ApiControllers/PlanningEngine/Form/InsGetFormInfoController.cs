using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Form;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Form
{
 /// <summary>
 /// 获取填单数据
 /// </summary>
        public class InsGetFormInfoController : ControllerBase
        {
            private readonly IGetFormInfoDAL _iGetFormInfoDAL;
            public InsGetFormInfoController(IGetFormInfoDAL iGetFormInfoDAL)
            {
            _iGetFormInfoDAL = iGetFormInfoDAL;

            }  /// <summary>
               ///  根据tableid和plan_task_id获取填单数据
               /// </summary>
               /// <param name="plan_task_id">任务明细id</param>
               /// <param name="tableId">表单表名</param>

               /// <returns></returns>
        [HttpGet]
            public MessageEntity Get(string plan_task_id, string tableId)
            {
                var messageEntity = _iGetFormInfoDAL.Get(plan_task_id,tableId);
                return messageEntity;
            }
          
        }
    }