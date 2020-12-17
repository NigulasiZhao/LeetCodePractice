using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.PermissionManage
{
    public interface IPermissionForMapDAL
    {
        /// <summary>
        /// 组织机构查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <param name="SearchType">0：查询集团公司，1：查询部门</param>
        /// <returns></returns>
        MessageEntity GetOrganizationList(string Access_Token);
        /// <summary>
        /// 部门查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        MessageEntity GetDepartmentList(string Access_Token);
        /// <summary>
        /// 用户查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <returns></returns>
        MessageEntity GetUserList(string Access_Token);
        /// <summary>
        /// 获取图层接口
        /// </summary>
        /// <returns></returns>
        MessageEntity GetPipeInfoList();

        /// <summary>
        /// 获取区域接口
        /// </summary>
        /// <returns></returns>
        MessageEntity GetAreaInfoList();
        /// <summary>
        /// 保存组织架构的区域权限
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        MessageEntity SaveAreaPermission(SaveAreaPermissionDto Model);
        /// <summary>
        /// 保存部门图层权限
        /// </summary>
        /// <returns></returns>
        MessageEntity SavePipePermissionForDepartment(SavePipePermissionForDepartmentDto Model);
        /// <summary>
        /// 保存部门图层权限
        /// </summary>
        /// <returns></returns>
        MessageEntity SavePipePermissionForUser(SavePipePermissionForUserDto Model);
        /// <summary>
        /// 获取机构区域权限信息
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <returns></returns>
        MessageEntity GetAreaPermissionInfo(string OrganizationId);
        /// <summary>
        /// 获取部门图层权限信息
        /// </summary>
        /// <returns></returns>
        MessageEntity GetPipePermissionInfoForDepartment(string DepartmentId);
        /// <summary>
        /// 获取用户图层权限信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        MessageEntity GetPipePermissionInfoForUser(string UserId);
        /// <summary>
        /// 获取用户所拥有的区域及图层权限信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        MessageEntity GetUserPipeAndLayerPermissionInfo(string UserId);
    }
}
