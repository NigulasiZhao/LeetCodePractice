using Dapper;
using Dapper.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodePractice.CommonTools.AutoTaskHelper
{
    public class AutomaticSignOut
    {
        public void AutomaticSignOutTask()
        {
            //查询所有的未签到数据
//            string sql = $@"select p.* from INS_ATTENDANCE p  join (
//select p.taskid,max(p.uptime) uptime from INS_ATTENDANCE p group by p.taskid) pp on pp.taskid=p.taskid and pp.uptime=p.uptime where p.groupid=1";
//            try
//            {
//                using (var conn = AutoTaskConnectionFactory.GetDBConn(AutoTaskConnectionFactory.DBConnNames.ORCL))
//                {
//                    var rows = 0;
//                    StringBuilder SqlInsertEqu = new StringBuilder();
//                    List<OracleDynamicParameters> parameterFirelist = new List<OracleDynamicParameters>();
//                    IList<Ins_Attendance> list = conn.Query<Ins_Attendance>(sql).ToList();
//                    //循环
//                    foreach (Ins_Attendance item in list)
//                    {
//                        SqlInsertEqu.Append($@"INSERT INTO Ins_Attendance (AttendanceID,Personid,Personname,DetpID,DetptName,Taskid,TaskName,GroupID,AttendanceType,IsAutomatic,Remark) 
//                          values ('{Guid.NewGuid().ToString()}','{item.Personid}','{item.Personname}','{item.DetpID}','{item.DetptName}','{item.Taskid}','{item.TaskName}',2,'签退',1,'自动签退');");
//                    }
//                    if (!string.IsNullOrEmpty(SqlInsertEqu.ToString()))
//                        rows = conn.Execute("begin  " + SqlInsertEqu.ToString() + " end;");
//                }
//            }
//            catch (Exception e)
//            {
//                LogHelper.Error("执行自动签退报错,错误信息：" + e.Message);
//            }
        }
    }
}
