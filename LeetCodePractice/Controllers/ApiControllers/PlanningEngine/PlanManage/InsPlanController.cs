using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using GISWaterSupplyAndSewageServer.Model.BPM;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.UniWater;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.PlanManage
{
    /// <summary>
    /// 计划管理
    /// </summary>
    public class InsPlanController : BaseController
    {
        private readonly IIns_PlanDAL _ins_PlanDAL;
        private readonly IIns_TaskManageDAL _iIns_TaskManageDAL;
        public InsPlanController(IIns_PlanDAL ins_PlanDAL, IIns_TaskManageDAL iIns_TaskManageDAL)
        {
            _ins_PlanDAL = ins_PlanDAL;
            _iIns_TaskManageDAL = iIns_TaskManageDAL;

        }

        /// <summary>
        /// 获取计划管理列表
        /// </summary>
        /// <param name="planttemplate_id"></param>
        ///  <param name="rangids">区域ids 用逗号分隔['1','2']</param>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Get(string ParInfo, [FromBody] List<string>? rangids = null, string sort = "Create_Time", string ordering = "desc", int num = 20, int page = 1, string? planttemplate_id = null)
        {
            string Ids = "";
            if (rangids != null)
            {
                Ids = "'" + string.Join("','", rangids.ToArray()) + "'";
                if (rangids.Count <= 0)
                {
                    Ids = "'" + "0" + "'";
                }
            }
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _ins_PlanDAL.GetList(planttemplate_id, Ids, list, sort, ordering, num, page, sqlCondition);
            return result;
        }
        /// <summary>
        /// 新增计划管理(Headers传递access_token)
        /// </summary>
        /// <param name="value">Plan_Name：计划名称；PlanCycleId：计划周期主键Id；IsNormalPlan：是否常规 0:常规 1:临时；IsFeedBack：是否反馈 0:需反馈 1:仅到位；
        /// Plan_Type_Id：巡检计划类型主键Id；Range_Name:巡检区域名称 网格区域时默认最后一个名称；Geometry：区域坐标信息；MoveType：巡检方式1：车巡，2：徒步；
        /// Plan_TemplateType_Id：计划模板大类id；PlanTtemplate_Id：计划模板id；
        /// Assign_State：分派状态 0：未分派 1 分派中 2分派失败 3分派成功；Range_Id：巡检区域id(包含网格id或区域id)；Ins_Plan_EquipmentList：设备类型列表[{ 
        /// LayerName：设备设施 ；tableID：自定义表单id；Ins_Plan_Equipment_InfoList：设备信息列表[{
        /// Equipment_Info_Code：设施编号eid；Equipment_Info_Name：设施名称；Address：地址；Caliber：口径；Lon：经度；Lat：纬度；EquType：类型 1：设备 2：管线;GlobID:设施id }]}]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] Ins_Plan value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            if (UniWaterUserInfo != null)
            {
                value.Create_Person_Id = UniWaterUserInfo._id;
                value.Create_Person_Name = UniWaterUserInfo.Name;
                value.Create_Time = DateTime.Now;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "access_token失效，获取用户信息失败！");
            }
            return _ins_PlanDAL.Add(value);
        }
        /// <summary>
        /// 修改计划管理
        /// </summary>
        /// <param name="ID">计划管理主键ID(Plan_Id)</param>
        /// <param name="value">Plan_Name：计划名称；PlanCycleId：计划周期主键Id；IsNormalPlan：是否常规 0:常规 1:临时；IsFeedBack：是否反馈 0:需反馈 1:仅到位；
        /// Plan_Type_Id：巡检计划类型主键Id；Range_Name:巡检区域名称 网格区域时默认最后一个名称；Geometry：区域坐标信息；MoveType：巡检方式1：车巡，2：徒步；
        /// Plan_TemplateType_Id：计划模板大类id；PlanTtemplate_Id：计划模板id；Create_Person_Id：新建人ID；Create_Person_Name：新建人姓名；Create_Time：新建时间；
        /// Assign_State：分派状态 0：未分派 1 分派中 2分派失败 3分派成功；Range_Id：巡检区域id(包含网格id或区域id)；Ins_Plan_EquipmentList：设备类型列表[{ 
        /// LayerName：设备设施 ；tableID：自定义表单id；Ins_Plan_Equipment_InfoList：设备信息列表[{
        /// Equipment_Info_Code：设施编号；Equipment_Info_Name：设施名称；Address：地址；Caliber：口径；Lon：经度；Lat：纬度；LayerName：设备设施 }]}]</param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] Ins_Plan value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.Plan_Id = ID;
            return _ins_PlanDAL.Update(value);
        }
        /// <summary>
        /// 删除计划管理
        /// </summary>
        /// <param name="ID">计划管理主键ID(Plan_Id)</param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            var modeInfo = _ins_PlanDAL.GetInfo(ID);

            //首先判断任务是否分派，已分派不允许删除
            DataTable dt = _iIns_TaskManageDAL.IsAssignTask(ID);
            int TaskCount = _ins_PlanDAL.GetTaskCount(ID);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["AssignState"].ToString() == "1" && TaskCount != 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "该计划下的任务已被分派，不允许删除！");
                }
            }
            return _ins_PlanDAL.Delete(modeInfo);
        }
        /// <summary>
        /// 计划管理派发
        /// </summary>
        /// <param name="model">Plan_Id：计划ID；Plan_Name：计划名称；TaskName：任务名称；task_Type_Id:任务类型id；DepartmentId：部门ID；
        /// DepartmentName：部门名称；ExecPersonId:分派人员Id；ExecPersonName：分派人员名称；StartDate：开始时间；
        /// EndDate：结束时间；TaskDescription：任务描述；creator_nm：创建人姓名；creator_id：创建人id；creator_gid：创建人公司ID；
        /// creator_gnm：创建人公司名称；category：类别；definitionId：定义ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageEntity> DistributedPlan([FromBody] DistributedPlanBPM model)
        {
            LogHelper.Info("计划分派开始调用时间" + DateTime.Now);

            if (model.EndDate != null)
                model.EndDate = model.EndDate.AddDays(1).AddSeconds(-1);

            if (UniWaterUserInfo != null)
            {
                model.CreatorId = UniWaterUserInfo._id;
                model.CreatorName = UniWaterUserInfo.Name;
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "access_token失效，获取用户信息失败！");
            }
            LogHelper.Info("计划分派请求uniwater结束时间" + DateTime.Now);

            return await _ins_PlanDAL.DistributedPlan(model);
        }
        /// <summary>
        /// 计划管理派发(重发)
        /// </summary>
        /// <param name="PlanId">计划管理主键ID(Plan_Id)</param>
        /// <returns></returns>
        //[HttpPost]
        //public MessageEntity RepeatDistributedPlan(string PlanId)
        //{
        //    return _ins_PlanDAL.RepeatDistributedPlan(PlanId);
        //}
    }
}