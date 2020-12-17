using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.PermissionManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.PermissionManage
{
    /// <summary>
    /// 图层及区域权限相关
    /// </summary>
    public class PermissionForMapController : Controller
    {
        private readonly IPermissionForMapDAL _permissionForMapDAL;

        public PermissionForMapController(IPermissionForMapDAL permissionForMapDAL)
        {
            _permissionForMapDAL = permissionForMapDAL;
        }
        /// <summary>
        /// 组织机构查询接口
        /// </summary>
        /// <param name="Access_Token">返回结果中_id:组织机构ID,level:组织机构类型,name:组织机构名称,pid:父级组织机构ID</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetOrganizationList(string Access_Token)
        {
            var result = _permissionForMapDAL.GetOrganizationList(Access_Token);
            return result;
        }
        /// <summary>
        /// 部门查询接口
        /// </summary>
        /// <param name="Access_Token">返回结果中id:部门ID,NAME:部门名称,PARENTID:父级部门ID,TREECHILDREN:子级部门列表</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetDepartmentList(string Access_Token)
        {
            var result = _permissionForMapDAL.GetDepartmentList(Access_Token);
            return result;
        }
        /// <summary>
        /// 用户查询接口
        /// </summary>
        /// <param name="Access_Token">  返回结果中_id:用户ID,name:用户名称,group:指数组织机构ID</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetUserList(string Access_Token)
        {
            var result = _permissionForMapDAL.GetUserList(Access_Token);
            return result;
        }
        /// <summary>
        /// 获取图层接口 返回结果中ID:图层ID,PARENTID:父级图层ID,NAME:图层名称,TREECHILDREN:子级图层列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetPipeInfoList()
        {
            var result = _permissionForMapDAL.GetPipeInfoList();
            return result;
        }
        /// <summary>
        /// 获取区域接口 返回结果中ID:区域编码,PARENTID:父级区域编码,NAME:区域名称,TREECHILDREN:子级区域列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetAreaInfoList()
        {
            var result = _permissionForMapDAL.GetAreaInfoList();
            return result;
        }
        /// <summary>
        /// 保存组织架构的区域权限
        /// </summary>
        /// <param name="Model">Access_Token,OrganizationId:组织机构ID,OrganizationType:组织机构类型(集团,公司,部门),OCode:区域编码数组</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity SaveAreaPermission([FromBody] SaveAreaPermissionDto Model)
        {
            var result = _permissionForMapDAL.SaveAreaPermission(Model);
            return result;
        }
        /// <summary>
        /// 保存部门图层权限
        /// </summary>
        /// <param name="Model">DepartmentId:部门ID,LayerInfoList:图层列表[{LayerName:图层名称, IsReadonly:是否只读}]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity SavePipePermissionForDepartment([FromBody] SavePipePermissionForDepartmentDto Model)
        {
            var result = _permissionForMapDAL.SavePipePermissionForDepartment(Model);
            return result;
        }
        /// <summary>
        /// 保存用户图层权限
        /// </summary>
        /// <param name="Model">UserId:用户ID,DepartmentId:部门ID,LayerInfoList:图层列表[{LayerName:图层名称, IsReadonly:是否只读}]</param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity SavePipePermissionForUser([FromBody] SavePipePermissionForUserDto Model)
        {
            var result = _permissionForMapDAL.SavePipePermissionForUser(Model);
            return result;
        }
        /// <summary>
        /// 获取机构区域权限信息
        /// </summary>
        /// <param name="OrganizationId">组织机构ID 返回结果中OrganizationId:组织机构ID,OrganizationType:组织机构类型,OCode:区域编码</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetAreaPermissionInfo(string OrganizationId)
        {
            var result = _permissionForMapDAL.GetAreaPermissionInfo(OrganizationId);
            return result;
        }
        /// <summary>
        /// 获取部门图层权限信息
        /// </summary>
        /// <param name="DepartmentId">部门ID 返回结果中DepartmentId:部门id,LayerName:图层Name,IsReadonly:是否只读(0 否 1 是)</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetPipePermissionInfoForDepartment(string DepartmentId)
        {
            var result = _permissionForMapDAL.GetPipePermissionInfoForDepartment(DepartmentId);
            return result;
        }
        /// <summary>
        /// 获取用户图层权限信息
        /// </summary>
        /// <param name="UserId">用户ID 返回结果中UserId:用户ID,LayerName:图层Name,DepartmentId:部门ID,IsReadonly:是否只读(0 否 1 是)</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetPipePermissionInfoForUser(string UserId)
        {
            var result = _permissionForMapDAL.GetPipePermissionInfoForUser(UserId);
            return result;
        }
        /// <summary>
        /// 获取用户所拥有的区域及图层权限信息
        /// </summary>
        /// <param name="UserId">用户ID 返回结果中UserId:用户ID,AreaList:区域权限列表{OrganizationId:组织机构ID,OrganizationType:组织机构类型,OCode:区域编码},LayerList:图层权限列表{UserId:用户ID,LayerName:图层Name,DepartmentId:部门ID,IsReadonly:是否只读(0 否 1 是)}</param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity GetUserPipeAndLayerPermissionInfo(string UserId)
        {
            var result = _permissionForMapDAL.GetUserPipeAndLayerPermissionInfo(UserId);
            return result;
        }
    }
}