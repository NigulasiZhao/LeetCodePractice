
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL
{
    public interface ICommonDAL : IDependency
    {
        int EntityForSqlToPager(string sql, string sort, string ordering, int num, int page, out MessageEntity messageEntity);
        //Model.P_Admin GetUserInfo(string loginContent, string password, out string errorMsg);
        //Model.P_Admin GetUserInfo(string userId, out string errorMsg);
        //Model.P_Admin GetUserInfoByName(string userName, out string errorMsg);
        //Model.P_Admin IsUserExist(string userName,string cJobNumber,int adminId, out string errorMsg);
        //MessageEntity GetUsers(string userName, int? roleId,int? deptId, string sort, string ordering, int num, int page);
        //List<Model.P_Function> GetUserAuthority(string userId, int systemType, out string errorMsg);
        //List<Model.P_Function> GetRoleAuthority(string roleId, int systemType, out string errorMsg);
        //List<Model.P_Admin> GetUserInfoListByDeptId(string deptId, string roleId, out string errorMsg);
        //List<Model.P_Admin> GetUserInfoListByDeptIdNoDelete(string deptId, string roleId, out string errorMsg);
        //MessageEntity AddUser(Model.P_Admin admin);
        //MessageEntity UpdateUser(Model.P_Admin admin);
        //bool UpdateUserPassword(Model.AdminPassword admin,out string errorMsg);
        //MessageEntity DeleteUser(int id);
        //MessageEntity GetAdmin();
    }
}