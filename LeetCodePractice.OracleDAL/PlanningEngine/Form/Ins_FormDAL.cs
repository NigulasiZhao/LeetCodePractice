using Dapper;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Form;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.InspectionSettings;
using NPOI.HSSF.Record.PivotTable;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Form
{
    public class Ins_FormDAL : IIns_FormDAL
    {

        public MessageEntity PostFireHydrant(Ins_Form_FireHydrantModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame)
        {


            //修改表单
            var updateSql = $@"update  Ins_Form_FireHydrant set IsFillForm=1,IsExistProblem='{model.IsExistProblem}',detailAddress='{model.DetailAddress}',IsMissCover='{model.IsMissCover}'
,IsOldStain='{model.IsOldStain}' ,IsHeight='{model.IsHeight}' ,IsleakWater='{model.IsleakWater}' ,IsTilt='{model.IsTilt}' ,IsFixedBrand='{model.IsFixedBrand}' ,IsRectification='{model.IsRectification}' ,workCode='{model.WorkCode}' ,imagePath='{imagePath}'    where Plan_task_id='{model.Plan_task_id}'";
            var updateplantask = $@"update  INS_plan_task set IsFinish=1,IsFillForm=1,OPERATEDATE=sysdate where plan_task_id='{model.Plan_task_id}'";
            var updatetask = $@"update  INS_task set travelMileage=@travelMileage,LastX=@LastX,LastY=@LastY where taskId='{model.TaskId}'";

            var rows = 0;


            var insertSql = "";
            if (!IsExistTaskdetail(model.Plan_task_id))
            {
                insertSql = DapperExtentions.MakeInsertSql(taskdetailmode);
            }
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {

                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateSql, transaction);
                        conn.Execute(updateplantask, transaction);

                        if (insertSql != "")
                        {
                            rows = conn.Execute(insertSql, taskdetailmode);
                        }
                        CalKilometer(model.TaskId, x, y);
                        transaction.Commit();

                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        LogHelper.Info("消火栓填单异常:" + e.Message);

                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }

        public MessageEntity PostValve(Ins_Form_ValveModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame)
        {
            //修改表单
            var updateSql = $@"update  Ins_Form_Valve set IsFillForm=1,IsExistProblem='{model.IsExistProblem}',detailAddress='{model.DetailAddress}',IsCover='{model.IsCover}'
,IsLose='{model.IsLose}' ,IsDamage='{model.IsDamage}' ,IsRectification='{model.IsRectification}' ,workCode='{model.WorkCode}' ,imagePath='{imagePath}'    where Plan_task_id='{model.Plan_task_id}'";
            var updatetask = $@"update  INS_plan_task set IsFinish=1 ,IsFillForm=1,OPERATEDATE=sysdate where plan_task_id='{model.Plan_task_id}'";
            var rows = 0;

            var insertSql = "";
            if (!IsExistTaskdetail(model.Plan_task_id))
            {
                insertSql = DapperExtentions.MakeInsertSql(taskdetailmode);
            }
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateSql, transaction);
                        conn.Execute(updatetask, transaction);

                        //插入份派工单记录
                        if (insertSql != "")
                        {
                            rows = conn.Execute(insertSql, taskdetailmode);
                        }
                        CalKilometer(model.TaskId, x, y);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }
        public MessageEntity PostLeakDetection(Ins_Form_LeakDetectionModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame)
        {
            var updateSql = ""; var leakinsertsql = "";
            //判断检漏是否填单
            if (!string.IsNullOrEmpty(model.GlobID) && !IsExistLeakDetection(model.GlobID))
            {
                leakinsertsql = $@"insert into Ins_Form_LeakDetection(id,Taskid,Taskname,Layername,Plan_Task_Id,Isfillform,Imagepath,Remark,Workcode,Globid,Detailaddress)" +
                    $" values('{Guid.NewGuid().ToString()}','{taskdetailmode.TaskId}','{model.TaskName}','{model.LayerName}','{model.Plan_task_id}',1,'{imagePath}','{model.Remark}','{model.WorkCode}','{model.GlobID}','{model.DetailAddress}')";

            }
            else
            {
                string sqlwhere = "";
                if (!string.IsNullOrEmpty(model.GlobID))
                    sqlwhere = $@"  where  GlobID='{model.GlobID}'";
                else
                    sqlwhere = $@"  where Plan_task_id='{model.Plan_task_id}'";

                updateSql = $@"update  Ins_Form_LeakDetection set updateNum =updateNum+1, IsFillForm=1,remark='{model.Remark}',GlobID='{model.GlobID}' ,workCode='{model.WorkCode}' ,imagePath='{imagePath}' {sqlwhere}  ";

            }

            var updatetask = $@"update  INS_plan_task set IsFinish=1 ,IsFillForm=1,OPERATEDATE=sysdate where plan_task_id='{model.Plan_task_id}'";
            var rows = 0;
            var insertSql = "";
            if (!IsExistTaskdetail(model.Plan_task_id))
            {
                insertSql = DapperExtentions.MakeInsertSql(taskdetailmode);
            }

            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        if (updateSql != "")
                            conn.Execute(updateSql, transaction);
                        if (leakinsertsql != "")
                            conn.Execute(leakinsertsql, model, transaction);
                        conn.Execute(updatetask, transaction);

                        if (insertSql != "")
                        {
                            rows = conn.Execute(insertSql, taskdetailmode);
                        }
                        CalKilometer(model.TaskId, x, y);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }
        public bool IsExistLeakDetection(string globID)
        {
            string errorMsg = "";
            string strWhere = $" and t.globID = '{globID}' ";
            string query = $@" select count(0) from Ins_Form_LeakDetection t where 1=1 {strWhere}";
            try
            {
                using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query));
                    int count = string.IsNullOrEmpty(dt.Rows[0][0].ToString()) ? 0 : int.Parse(dt.Rows[0][0].ToString());
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
        }
        /// <summary>
        /// 是否存在任务完成明细
        /// </summary>
        /// <param name="plan_task_id"></param>
        /// <returns></returns>
        public bool IsExistTaskdetail(string plan_task_id)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(plan_task_id.ToString()))
            {
                strWhere += $" and t.plan_task_id = '{plan_task_id}' ";
            }
            string query = $@" select count(0) from Ins_Task_CompleteDetail t where 1=1 {strWhere}";
            try
            {
                using (var conn = Database.ConnectionFactory.GetDBConn(Database.ConnectionFactory.DBConnNames.ORCL))
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(query));
                    int count = string.IsNullOrEmpty(dt.Rows[0][0].ToString()) ? 0 : int.Parse(dt.Rows[0][0].ToString());
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
        }
        public MessageEntity CalKilometer(string taskid, string x, string y)
        {
            LogHelper.Info("任务id" + taskid + "计算距离调用");
            string sql = $@"select t.x,t.y from INS_TASK_COMPLETEDETAIL t where t.taskid='{taskid}' order by t.uptime desc";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {

                int rows = 0;
                try
                {
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(sql));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        decimal Distance = (decimal)(Math.Round(Math.Sqrt(Math.Pow(double.Parse(dt.Rows[0]["x"].ToString()) - double.Parse(x), 2.0) + Math.Pow(double.Parse(dt.Rows[0]["y"].ToString().ToString()) - double.Parse(y), 2.0)) / 100d, 1) / 10d);
                        //    string distancesql = $@"  select sde.st_distance(sde.st_geometry('POINT ({x} {y})', 4547),
                        //sde.st_geometry('POINT ({dt.Rows[0]["x"]} {dt.Rows[0]["y"]})',4547) , 'kilometer') as distance
                        //                from dual";
                        //        DataTable distancedt = new DataTable();
                        //        distancedt.Load(conn.ExecuteReader(distancesql));
                        //    if (distancedt != null && distancedt.Rows.Count > 0)
                        //    {
                        LogHelper.Info("生成数据" + taskid + "计算距离:----" + Distance);

                        string updatetask = $@" update INS_TASK set  travelMileage =travelMileage+{Distance} where taskid='{taskid}'";
                        rows = conn.Execute(updatetask);
                        // }

                    }


                    return MessageEntityTool.GetMessage(1, null, true);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity PostStoreWater(Ins_Form_StoreWaterModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame, string upDeptId, string upDeptName)
        {
            //修改表单
            var updateSql = $@"update Ins_Form_StoreWater set IsFillForm=1,externalEqu={model.ExternalEqu},subsidiaryEqu={model.SubsidiaryEqu},waterLevel={model.WaterLevel},paintRepair={model.PaintRepair},workCode='{model.WorkCode}' ,imagePath='{imagePath}'    where Plan_task_id='{model.Plan_task_id}'";
            var updateplantask = $@"update  INS_plan_task set IsFinish=1 ,IsFillForm=1,OPERATEDATE=sysdate,DepartmentId='{upDeptId}',DepartmentName='{upDeptName}',ExecPersonId='{iadminid}',ExecPersonName='{iadminame}' where plan_task_id='{model.Plan_task_id}'";
            var rows = 0;

            var insertSql = "";
            if (!IsExistTaskdetail(model.Plan_task_id))
            {
                insertSql = DapperExtentions.MakeInsertSql(taskdetailmode);
            }
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateSql, transaction);
                        if (model.IsFillForm != 1)
                        {
                            conn.Execute(updateplantask, transaction);
                        }
                        //插入份派工单记录
                        if (insertSql != "")
                        {
                            rows = conn.Execute(insertSql, taskdetailmode);
                        }
                        CalKilometer(model.TaskId, x, y);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Info("蓄水设施填单异常:" + e.Message);

                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }

        public MessageEntity PostPump(Ins_Form_PumpModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame, string upDeptId, string upDeptName)
        {
            var updateSql = $@"update Ins_Form_Pump set IsFillForm=1,waterPump={model.Waterpump},motor={model.Motor},electric={model.Electric},variousValve={model.Variousvalve},variousPipe={model.Variouspipe},motorPowerSupply={model.Motorpowersupply},unitFoundation={model.Unitfoundation},sewageEqu={model.Sewageequ},measureMeter={model.Measuremeter},paintRepair={model.Paintrepair},workCode='{model.WorkCode}' ,imagePath='{imagePath}'    where Plan_task_id='{model.Plan_task_id}'";
            var updateplantask = $@"update  INS_plan_task set IsFinish=1 ,IsFillForm=1,OPERATEDATE=sysdate,DepartmentId='{upDeptId}',DepartmentName='{upDeptName}',ExecPersonId='{iadminid}',ExecPersonName='{iadminame}' where plan_task_id='{model.Plan_task_id}'";
            var rows = 0;

            var insertSql = "";
            if (!IsExistTaskdetail(model.Plan_task_id))
            {
                insertSql = DapperExtentions.MakeInsertSql(taskdetailmode);
            }
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateSql, transaction);
                        if (model.IsFillForm != 1)
                        {
                            conn.Execute(updateplantask, transaction);
                        }
                        //插入份派工单记录
                        if (insertSql != "")
                        {
                            rows = conn.Execute(insertSql, taskdetailmode);
                        }
                        CalKilometer(model.TaskId, x, y);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Info("泵房填单异常:" + e.Message);

                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }

        public MessageEntity PostPipelineequ(Ins_Form_PipelineequModel model, Ins_Task_CompleteDetail taskdetailmode, string imagePath, string x, string y, string iadminid, string iadminame, string upDeptId, string upDeptName)
        {
            var updateSql = $@"update Ins_Form_PipelineEqu set IsFillForm=1,variousValve={model.Variousvalve},variousPipe={model.Variouspipe},jpressureValve={model.Jpressurevalve},jpressureValveGl={model.Jpressurevalvegl},automaticPqValve={model.Automaticpqvalve},paintRepair={model.Paintrepair},workCode='{model.WorkCode}' ,imagePath='{imagePath}'    where Plan_task_id='{model.Plan_task_id}'";
            var updateplantask = $@"update  INS_plan_task set IsFinish=1 ,IsFillForm=1,OPERATEDATE=sysdate,DepartmentId='{upDeptId}',DepartmentName='{upDeptName}',ExecPersonId='{iadminid}',ExecPersonName='{iadminame}' where plan_task_id='{model.Plan_task_id}'";
            var rows = 0;

            var insertSql = "";
            if (!IsExistTaskdetail(model.Plan_task_id))
            {
                insertSql = DapperExtentions.MakeInsertSql(taskdetailmode);
            }
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                using (var transaction = conn.BeginTransaction())
                {

                    try
                    {
                        conn.Execute(updateSql, transaction);
                        if (model.IsFillForm != 1)
                        {
                            conn.Execute(updateplantask, transaction);
                        }
                        //插入份派工单记录
                        if (insertSql != "")
                        {
                            rows = conn.Execute(insertSql, taskdetailmode);
                        }
                        CalKilometer(model.TaskId, x, y);
                        transaction.Commit();
                        return MessageEntityTool.GetMessage(1, null, true);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Info("共用管道及附属设备填单异常:" + e.Message);

                        transaction.Rollback();
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                    }
                }
            }
        }

    }
}
