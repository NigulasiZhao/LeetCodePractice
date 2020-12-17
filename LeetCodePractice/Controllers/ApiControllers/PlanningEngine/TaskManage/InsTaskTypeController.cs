using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.TaskManage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static GISWaterSupplyAndSewageServer.CommonTools.ChidrenTree;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.TaskManage
{
    /// <summary>
    /// 任务分类
    /// </summary>
    public class InsTaskTypeController : ControllerBase
    {
        private readonly IIns_Task_TypeDAL _iIns_Task_TypeDAL;

        public InsTaskTypeController(IIns_Task_TypeDAL iIns_Task_TypeDAL)
        {
            _iIns_Task_TypeDAL = iIns_Task_TypeDAL;
        }
        /// <summary>
        /// 获取分类数据
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]查询参数名称Task_type_id 分类id</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "operateDate", string ordering = "asc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _iIns_Task_TypeDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }

        /// <summary>
        /// 获取任务分类树形目录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetTree(string pID = "00000000-0000-0000-0000-000000000000")
        {
            ChidrenTree childtree = new ChidrenTree();
            List<Ins_Task_Type> list = _iIns_Task_TypeDAL.GetTree(pID);
            //List<TreeChildViewModel> treeViewModels = new List<TreeChildViewModel>();
            //treeViewModels = childtree.AddPlanTempChild(list, pID);
            List<TreeModel> treeViewModels = new List<TreeModel>();
            treeViewModels = childtree.ConversionList(list, pID, "Task_type_id", "ParentTypeId", "Task_type_name", "Task_type_code");
            return MessageEntityTool.GetMessage(treeViewModels.Count(), treeViewModels, true, "", treeViewModels.Count());
        }


        /// <summary>
        /// 新增任务分类
        /// </summary>
        /// <param name="value">Task_type_code：编号 Task_type_name：名称  ParentTypeId：父节点id 值为00000000-0000-0000-0000-000000000000代表根节点</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Task_Type value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            MessageEntity messresult = _iIns_Task_TypeDAL.IsExistTaskType(value, 1);
            List<Ins_Task_Type> ptslist = (List<Ins_Task_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许添加重复名称或编号！");
            }
            return _iIns_Task_TypeDAL.Add(value);
        }

        /// <summary>
        /// 修改任务分类
        /// </summary>
        /// <param name="ID">Task_type_id 分类id</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Task_Type value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Task_type_id = ID;
            MessageEntity messresult = _iIns_Task_TypeDAL.IsExistTaskType(value, 0);
            List<Ins_Task_Type> ptslist = (List<Ins_Task_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许修改为重复名称或编号！");
            }
            return _iIns_Task_TypeDAL.Update(value);
        }

        /// <summary>
        /// 删除任务分类
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            //删除首先判断是否存在子节点
            MessageEntity messresult = _iIns_Task_TypeDAL.IsExistTaskTypeChildren(ID);
            List<Ins_Task_Type> ptslist = (List<Ins_Task_Type>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许删除存在子分类数据，请先删除子节点数据！");
            }
            return _iIns_Task_TypeDAL.Delete(new Ins_Task_Type { Task_type_id = ID });
        }
    }
}