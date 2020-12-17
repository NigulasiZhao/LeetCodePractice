using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GISWaterSupplyAndSewageServer.App_Authorize
{
    public class RequestCheck
    {
        public ErrorType RequestLoginStateCheck(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return ErrorType.NoLogin;
            }
            else
            {
                try
                {
                    UserInfoCache.Authorize = JwtHelper.GetJwtDecode(token);
                }
                catch
                {
                    return ErrorType.NotAvilebalToken;
                }
                //if (UserInfoCache.Authorize.ExpireTime < DateTime.Now)
                //{
                //    //暂时不需要验证时间 需要打开验证权限功能就取消下面注释
                //    return ErrorType.OutOfTime;
                //}
                //else
                //{
                //    //暂时不需要验证权限 需要打开验证权限功能就取消下面注释

                //    if (!RequestAuthorizeCheck(UserInfoCache.Authorize.UserId, rawUrl))
                //    {
                //        return ErrorType.NoAuthority;
                //    }
                //}
            }
            return ErrorType.Success;
        }

    }
}