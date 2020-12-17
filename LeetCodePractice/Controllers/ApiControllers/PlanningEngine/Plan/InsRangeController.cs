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
using static GISWaterSupplyAndSewageServer.CommonTools.ChidrenTree;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.Plan
{
    /// <summary>
    /// 巡检范围管理
    /// </summary>
    public class InsRangeController : ControllerBase
    {
        private readonly IIns_RangeDAL _ins_RangeDAL;

        public InsRangeController(IIns_RangeDAL ins_RangeDAL)
        {
            _ins_RangeDAL = ins_RangeDAL;
        }

        /// <summary>
        /// 获取巡检范围
        /// </summary>
        /// <param name="ParInfo">其他台账查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]查询参数名称Type 1:polygon 2:string ； 区域名称:Range_name;责任部门：Department_name， DeptId 责任人 Person_name， PersonId</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "create_time", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_RangeDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }

        /// <summary>
        /// 获取巡检范围树形目录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetTree(string pID = "00000000-0000-0000-0000-000000000000")
        {

            ChidrenTree childtree = new ChidrenTree();
            List<Ins_Range> list = _ins_RangeDAL.GetTree(pID);
            //List<TreeChildViewModel> treeViewModels = new List<TreeChildViewModel>();
            //treeViewModels = childtree.AddInsRangeChild(list, pID);
            List<TreeModel> treeViewModels = new List<TreeModel>();
            treeViewModels = childtree.ConversionList(list, pID, "Range_id", "Range_parentid", "range_name");
            return MessageEntityTool.GetMessage(treeViewModels.Count(), treeViewModels, true, "", treeViewModels.Count());
        }
        /// <summary>
        /// 新增巡检范围
        /// </summary>
        /// <param name="value">Range_parentid：值为00000000-0000-0000-0000-000000000000代表根节点 type：1:polygon 2:string  shape:范围gis信息 department_name:责任部门 person_name：区域负责人</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Range value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Range_name = value.Range_name.Replace(" ", "");
            if (string.IsNullOrEmpty(value.PersonId) || string.IsNullOrEmpty(value.Person_name) || string.IsNullOrEmpty(value.Department_name) || string.IsNullOrEmpty(value.DeptId))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            if (value.Range_parentid=="" || value.Range_parentid == null)
            {
                value.Range_parentid = "00000000-0000-0000-0000-000000000000";
            }
            //新增区域名称，添加名称重复判断
            MessageEntity messresult = _ins_RangeDAL.IsExistRange(value, 1);
            List<Ins_Range> ptslist = (List<Ins_Range>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许添加重复名称！");
            }
            return _ins_RangeDAL.Add(value);
        }

        /// <summary>
        /// 修改巡检范围
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Range value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Range_name = value.Range_name.Replace(" ", "");
            if (string.IsNullOrEmpty(value.PersonId) || string.IsNullOrEmpty(value.Person_name) || string.IsNullOrEmpty(value.Department_name) || string.IsNullOrEmpty(value.DeptId))
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Range_id = ID;
            //新增区域名称，添加名称重复判断
            MessageEntity messresult = _ins_RangeDAL.IsExistRange(value, 0);
            List<Ins_Range> ptslist = (List<Ins_Range>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许修改为重复名称！");
            }
            return _ins_RangeDAL.Update(value);
        }

        /// <summary>
        /// 删除巡检范围
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            //删除首先判断是否存在子节点
            MessageEntity messresult = _ins_RangeDAL.IsExistRangeChildren(ID);
            List<Ins_Range> ptslist = (List<Ins_Range>)messresult.Data.Result;
            if (ptslist.Count > 0)
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "不允许删除父节点，请先删除子节点数据！");
            }
            return _ins_RangeDAL.Delete(new Ins_Range { Range_id = ID });
        }
    }
}