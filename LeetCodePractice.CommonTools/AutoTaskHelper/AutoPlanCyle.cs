using Dapper;
using GISWaterSupplyAndSewageServer.Model.Plan;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.CommonTools.AutoTaskHelper
{
    public class AutoPlanCyle
    {
        public void Add(Ins_Plan_Cycle model)
        {
            using (var conn = AutoTaskConnectionFactory.GetDBConn(AutoTaskConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    StringBuilder SqlInsertEqu = new StringBuilder();
                    var rows = 0;
                    var insertSql = AutoTaskDapperExtentions.MakeInsertSql(model);
                    rows = conn.Execute(insertSql, model);
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.Message);
                }
            }
        }

        public void AddRedis(Ins_Plan_Cycle model)
        {
            //IDatabase _redis = new RedisHelper(Appsettings.app(new string[] { "Redis", "Default", "Connection" }), Appsettings.app(new string[] { "Redis", "Default", "InstanceName" }), int.Parse(Appsettings.app(new string[] { "Redis", "Default", "DefaultDB" }))).GetDatabase();
            //_redis.StringSet(DateTime.Now.ToString(), JsonConvert.SerializeObject(model));
        }
    }
}
