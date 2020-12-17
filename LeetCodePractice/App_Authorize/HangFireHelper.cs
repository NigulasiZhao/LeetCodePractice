using LeetCodePractice.CommonTools;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeetCodePractice
{
    /// <summary>
    /// HangFire帮助类
    /// </summary>
    public class HangFireHelper
    {
        /// <summary>
        /// 开始HangFire后台任务
        /// </summary>
        public void StartHangFireTask()
        {
            RecurringJob.AddOrUpdate(() => AutoTackDemo(), "10 58 9 * * *", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate(() => AutomaticSignOutTask(), "0 0 3 * * *", TimeZoneInfo.Local);

        }
        #region 自动任务相关
        /// <summary>
        /// 
        /// </summary>
        public static void AutomaticSignOutTask()
        {
            //AutomaticSignOut Service = new AutomaticSignOut();
            //Service.AutomaticSignOutTask();
        }
        /// <summary>
        /// 自动任务Demo
        /// </summary>
        /// <returns></returns>
        public static void AutoTackDemo()
        {
            //try
            //{
            //    IIns_Plan_CycleDAL dal = new Ins_Plan_CycleDAL();
            //    ////数据库操作
            //    //AutoPlanCyle Service = new AutoPlanCyle();
            //    dal.Add(new LeetCodePractice.Model.Plan.Ins_Plan_Cycle
            //    {
            //        Plan_Cycle_Name = DateTime.Now.ToString(),
            //        CycleTime = "1024",
            //        CycleUnit = "天",
            //        CycleHz = 1,
            //        DeleteState = 0
            //    });
            //}
            //catch (Exception e)
            //{

            //    LogHelper.Debug(e.Message);
            //}
            //Redis操作
            //IDatabase _redis = new RedisHelper(Appsettings.app(new string[] { "Redis", "Default", "Connection" }), Appsettings.app(new string[] { "Redis", "Default", "InstanceName" }), int.Parse(Appsettings.app(new string[] { "Redis", "Default", "DefaultDB" }))).GetDatabase();
            //_redis.StringSet(DateTime.Now.ToString(), "1");
        }
        #endregion
    }
}
