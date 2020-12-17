using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.BPM;
using GISWaterSupplyAndSewageServer.Model.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Form;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage;
using GISWaterSupplyAndSewageServer.Model.ResultDto;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.PlanManage
{

    public class Ins_PlanDAL : IIns_PlanDAL
    {
        private readonly IHttpClientFactory _httpClientFactory;
        List<Ins_Plan_Task> plantasklist;
        List<Ins_Form_FireHydrant> formfirelist;
        List<Ins_Form_Valve> formvalvelist;
        List<Ins_Form_LeakDetection> formleakdetectionlist;
        List<Ins_Form_StoreWater> formStoreWaterlist;
        List<Ins_Form_Pipelineequ> formPipelineequlist;
        List<Ins_Form_Pump> formPumplist;
        int range = 1000;
        bool isSucessPlantask = true;
        bool isSucessFormFire = true;
        bool isSucessFormValve = true;
        bool isSucessLeakDetection = true;
        bool isSucessFormStoreWater = true;
        bool isSucessFormPipelineequ = true;
        bool isSucessFormPump = true;

        public Ins_PlanDAL(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// 添加计划管理
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Ins_Plan model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                model.Plan_Id = Guid.NewGuid().ToString();
                //DapperExtentions.MakeInsertSql(model, out OracleDynamicParameters parameters);
                var insertSql =
               //                string.Format($@"INSERT INTO INS_PLAN(PLAN_ID,PLAN_NAME,PLANCYCLEID,ISNORMALPLAN,ISFEEDBACK,PLAN_TYPE_ID,RANGE_NAME,GEOMETRY,SHAPE,MOVETYPE,
               //PLAN_TEMPLATETYPE_ID,PLANTTEMPLATE_ID,CREATE_PERSON_ID,CREATE_PERSON_NAME,CREATE_TIME,ASSIGN_STATE,RANGE_ID) VALUES('{model.Plan_Id}','{model.Plan_Name}','{model.PlanCycleId}'
               //,{model.IsNormalPlan},{model.IsFeedBack},'{model.Plan_Type_Id}','{model.Range_Name}','{model.Geometry}',(select sde.st_geometry('{model.WKTGeometry}',4547)  from dual),{model.MoveType},'{model.Plan_TemplateType_Id}','{model.PlanTtemplate_Id}'
               //,'{model.Create_Person_Id}','{model.Create_Person_Name}',to_date('{model.Create_Time}','YYYY-mm-dd HH24:Mi:SS'),{model.Assign_State},'{model.Range_Id}')");

               $@"DECLARE  
                                                               GeometryStr clob;
                                                               WKTGeometryStr clob;
                                                             BEGIN  
                                                               GeometryStr := '{model.Geometry}';
                                                               WKTGeometryStr := '{model.WKTGeometry}';
                                                               INSERT INTO INS_PLAN(PLAN_ID,PLAN_NAME,PLANCYCLEID,ISNORMALPLAN,ISFEEDBACK,PLAN_TYPE_ID,RANGE_NAME,GEOMETRY,SHAPE,MOVETYPE,
PLAN_TEMPLATETYPE_ID,PLANTTEMPLATE_ID,CREATE_PERSON_ID,CREATE_PERSON_NAME,CREATE_TIME,ASSIGN_STATE,RANGE_ID) VALUES('{model.Plan_Id}','{model.Plan_Name}','{model.PlanCycleId}'
,{model.IsNormalPlan},{model.IsFeedBack},'{model.Plan_Type_Id}','{model.Range_Name}',GeometryStr,(select sde.st_geometry(WKTGeometryStr,4547)  from dual),{model.MoveType},'{model.Plan_TemplateType_Id}','{model.PlanTtemplate_Id}'
,'{model.Create_Person_Id}','{model.Create_Person_Name}',to_date('{model.Create_Time}','YYYY-mm-dd HH24:Mi:SS'),{model.Assign_State},'{model.Range_Id}'); 
                                                               COMMIT;  
                                                             END;  ";


                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(insertSql, null, transaction);
                    model.Ins_Plan_EquipmentList.ForEach(row =>
                    {
                        row.Plan_Id = model.Plan_Id;
                        var insertSql1 = DapperExtentions.MakeInsertSql(row, out OracleDynamicParameters parameters);
                        var rows1 = conn.Execute(insertSql1, parameters, transaction);
                        row.Ins_Plan_Equipment_InfoList.ForEach(detail =>
                        {
                            detail.Plan_Equipment_Id = row.Plan_Equipment_Id;
                            if (detail.EquType == 2)
                            {
                                detail.Geometry = detail.Lon;
                                detail.Lon = "";
                                detail.Lat = "";

                            }
                            var insertEqu =
                            //    string.Format($@"INSERT INTO INS_PLAN_EQUIPMENT_INFO(EQUIPMENT_INFO_ID,PLAN_EQUIPMENT_ID,EQUIPMENT_INFO_CODE,
                            //    EQUIPMENT_INFO_NAME,ADDRESS,CREATE_TIME,GLOBID,EQUTYPE,CALIBER,GEOMETRY,LON,LAT,SHAPE) VALUES('{Guid.NewGuid()}','{row.Plan_Equipment_Id}',
                            //'{detail.Equipment_Info_Code}','{detail.Equipment_Info_Name}','{detail.Address}',to_date('{DateTime.Now}','YYYY-mm-dd HH24:Mi:SS'),'{detail.GlobID}'
                            //,{detail.EquType},'{detail.Caliber}','{detail.Geometry}','{detail.Lon}','{detail.Lat}',(select sde.st_geometry('{detail.WKTGeometry}',4547)  from dual))");
                            $@"DECLARE  
                                                               GeometryStr clob;
                                                               WKTGeometryStr clob;  
                                                             BEGIN  
                                                               GeometryStr := '{detail.Geometry}';
                                                               WKTGeometryStr := '{detail.WKTGeometry}';
                                                               INSERT INTO INS_PLAN_EQUIPMENT_INFO(EQUIPMENT_INFO_ID,PLAN_EQUIPMENT_ID,EQUIPMENT_INFO_CODE,
                            EQUIPMENT_INFO_NAME,ADDRESS,CREATE_TIME,GLOBID,EQUTYPE,CALIBER,GEOMETRY,LON,LAT,SHAPE) VALUES('{Guid.NewGuid()}','{row.Plan_Equipment_Id}',
                        '{detail.Equipment_Info_Code}','{detail.Equipment_Info_Name}','{detail.Address}',to_date('{DateTime.Now}','YYYY-mm-dd HH24:Mi:SS'),'{detail.GlobID}'
                        ,{detail.EquType},'{detail.Caliber}',GeometryStr,'{detail.Lon}','{detail.Lat}',(select sde.st_geometry(WKTGeometryStr,4547)  from dual)); 
                                                               COMMIT;  
                                                             END;  ";
                            var rows2 = conn.Execute(insertEqu, null, transaction);
                        });
                    });
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(1);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 删除计划管理
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Ins_Plan model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                //string GetDataSql = string.Format(@"SELECT Count(0) FROM INS_TASK WHERE Plan_Id = '{0}'", model.Plan_Id);
                //int DataRows = GetTaskCount(model.Plan_Id);
                //if (DataRows > 0)
                //{
                //    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "已关联巡检任务数据不允许删除");
                //}
                var excSql = DapperExtentions.MakeDeleteSql(model);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    model.Ins_Plan_EquipmentList.ForEach(row =>
                    {
                        var rows1 = conn.Execute(DapperExtentions.MakeDeleteSql(row), row, transaction);
                        row.Ins_Plan_Equipment_InfoList.ForEach(detail =>
                        {
                            var rows2 = conn.Execute(DapperExtentions.MakeDeleteSql(detail), detail, transaction);
                        });
                    });

                    var deletetask = $@"delete  INS_Task  where plan_id='{model.Plan_Id}'";
                    rows = conn.Execute(deletetask);
                    rows = conn.Execute(excSql, model, transaction);

                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 获取计划任务数量
        /// </summary>
        /// <returns></returns>
        public int GetTaskCount(string Plan_Id)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM INS_TASK WHERE Plan_Id = '{0}'", Plan_Id);
                int DataRows = conn.ExecuteScalar<int>(GetDataSql);
                return DataRows;
            }
        }
        /// <summary>
        /// 根据ID获取计划管理
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ins_Plan GetInfo(string ID)
        {
            List<Ins_Plan> _ListField = new List<Ins_Plan>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Ins_Plan>("select plan_id,plan_name,plancycleid,isnormalplan,isfeedback,plan_type_id,range_name,geometry,movetype,plan_templatetype_id,planttemplate_id,create_person_id,create_person_name,create_time,assign_state,range_id from Ins_Plan t where Plan_Id='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField[0].Ins_Plan_EquipmentList = conn.Query<Ins_Plan_Equipment>("select * from Ins_Plan_Equipment t where Plan_Id='" + _ListField[0].Plan_Id.ToString() + "'").ToList();
                    _ListField[0].Ins_Plan_EquipmentList.ForEach(row =>
                    {
                        row.Ins_Plan_Equipment_InfoList = conn.Query<Ins_Plan_Equipment_Info>("select ipei.equipment_info_id,ipei.plan_equipment_id,ipei.equipment_info_code,ipei.equipment_info_name,ipei.address,ipei.create_time,ipei.globid,ipei.equtype,ipei.caliber,ipei.geometry,ipei.lon,ipei.lat,ipe.LayerName from Ins_Plan_Equipment_Info ipei left join Ins_Plan_Equipment ipe on ipei.Plan_Equipment_Id = ipe.Plan_Equipment_Id where ipei.Plan_Equipment_Id='" + row.Plan_Equipment_Id.ToString() + "'").ToList();
                    });
                }
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得计划管理信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(string planttemplate_id, string rangids, List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            if (planttemplate_id != null)
            {
                sqlCondition += $" and ip.Planttemplate_id='{planttemplate_id}'";
            }

            if (rangids != null && rangids != "" && rangids != "'0'")
            {
                sqlCondition += $" and ip.range_id in ({rangids})";
            }
            string sql = @"  select ip.plan_type_id,
       pt.plan_type_name,
       ip.plan_id,
       ip.plan_name,
       iPT.PLAN_TEMPLATETYPE_ID,
       ipt.templatetype_name,
       ip.range_id,
       ip.range_name,
       ip.isnormalplan,
       case ip.isnormalplan
         when 0 then
          '常规'
         when 1 then
          '临时'
         else
          '其他'
       end as isnormalplanName,
       pla.planttemplate_id,
       pla.planttemplate_name,
       pc.plan_cycle_id,
       pc.plan_cycle_name,
       ip.isfeedback,
       case ip.isfeedback
         when 0 then
          '需反馈'
         when 1 then
          '仅到位'
         else
          '其他'
       end as isfeedbackname,
       ip.movetype,
       case ip.movetype
         when 1 then
          '车巡'
         when 2 then
          '徒步'
         else
          '其他'
       end as movetypename,
       case ip.assign_state
         when 0 then
          '未分派'
         when 1 then
          '已分派'
         else
          '其他'
       end as assign_statename,
       nvl(t.assignstate, 0) assignstate,
       ip.assign_state,
       ip.create_person_id,
       ip.create_person_name,
       ip.create_time,
       sumcount,
       ip.geometry
  from Ins_Plan ip
  left join ins_plan_cycle pc
    on pc.plan_cycle_id = ip.plancycleid
  left join (select distinct t.plan_id, t.assignstate
               from ins_task t
              where t.assignstate = 1) t
    on t.plan_id = ip.plan_id
  left join ins_plan_type pt
    on pt.plan_type_id = ip.plan_type_id
  left join ins_plantemplate pla
    on pla.planttemplate_id = ip.planttemplate_id
  left join ins_plan_templatetype ipt
    on ipt.plan_templatetype_id =ip.plan_templatetype_id
  left join (select count(0) sumcount, e.plan_id
               from ins_plan_equipment e
              group by e.plan_id) e
    on e.plan_id = ip.plan_id " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_PlanDto>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            foreach (Ins_PlanDto row in ResultList)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    row.Ins_Plan_EquipmentList = conn.Query<Ins_Plan_EquipmentDto>("select t.*,f.tablename from Ins_Plan_Equipment t left join ins_customize_formlist f on f.tableid=t.tableid where t.Plan_Id='" + row.Plan_Id.ToString() + "'").ToList();
                    //row.Ins_Plan_EquipmentList.ForEach(row =>
                    //{
                    //    row.Ins_Plan_Equipment_InfoList = conn.Query<Ins_Plan_Equipment_Info>("select ipei.*,ipe.LayerName from Ins_Plan_Equipment_Info ipei left join Ins_Plan_Equipment ipe on ipei.Plan_Equipment_Id = ipe.Plan_Equipment_Id where ipei.Plan_Equipment_Id='" + row.Plan_Equipment_Id.ToString() + "'").ToList();
                    //});
                }
            }
            return result;
        }
        /// <summary>
        ///修改计划管理信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Ins_Plan model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql =
                    $@"DECLARE  
                                                               GeometryStr clob;
                                                               WKTGeometryStr clob;   
                                                             BEGIN  
                                                               GeometryStr := '{model.Geometry}';
                                                               WKTGeometryStr := '{model.WKTGeometry}';
                                                               UPDATE INS_PLAN T SET 
                                                                               T.PLAN_NAME='{model.Plan_Name}'
                                                                              ,T.PLANCYCLEID='{model.PlanCycleId}'
                                                                              ,T.ISNORMALPLAN={model.IsNormalPlan}
                                                                              ,T.ISFEEDBACK={model.IsFeedBack}
                                                                              ,T.PLAN_TYPE_ID='{model.Plan_Type_Id}'
                                                                              ,T.RANGE_NAME='{model.Range_Name}'
                                                                              ,T.SHAPE=(select sde.st_geometry(WKTGeometryStr,4547)  from dual)
                                                                              ,T.GEOMETRY=GeometryStr
                                                                              ,T.MOVETYPE={model.MoveType}
                                                                              ,T.PLAN_TEMPLATETYPE_ID='{model.Plan_TemplateType_Id}'
                                                                              ,T.PLANTTEMPLATE_ID='{model.PlanTtemplate_Id}'
                                                                              ,T.ASSIGN_STATE={model.Assign_State}
                                                                              ,T.RANGE_ID='{model.Range_Id}'
                                                                              WHERE t.PLAN_ID = '{model.Plan_Id}'; 
                                                               COMMIT;  
                                                             END;  ";
                //string.Format($@"UPDATE INS_PLAN T SET 
                //                                 T.PLAN_NAME='{model.Plan_Name}'
                //                                ,T.PLANCYCLEID='{model.PlanCycleId}'
                //                                ,T.ISNORMALPLAN={model.IsNormalPlan}
                //                                ,T.ISFEEDBACK={model.IsFeedBack}
                //                                ,T.PLAN_TYPE_ID='{model.Plan_Type_Id}'
                //                                ,T.RANGE_NAME='{model.Range_Name}'
                //                                ,T.SHAPE=(select sde.st_geometry('{model.WKTGeometry}',4547)  from dual)
                //                                ,T.GEOMETRY='{model.Geometry}'
                //                                ,T.MOVETYPE={model.MoveType}
                //                                ,T.PLAN_TEMPLATETYPE_ID='{model.Plan_TemplateType_Id}'
                //                                ,T.PLANTTEMPLATE_ID='{model.PlanTtemplate_Id}'
                //                                ,T.CREATE_PERSON_ID='{model.Create_Person_Id}'
                //                                ,T.CREATE_PERSON_NAME='{model.Create_Person_Name}'
                //                                ,T.CREATE_TIME=to_date('{model.Create_Time}','YYYY-mm-dd HH24:Mi:SS')
                //                                ,T.ASSIGN_STATE={model.Assign_State}
                //                                ,T.RANGE_ID='{model.Range_Id}'
                //                                WHERE t.PLAN_ID = '{model.Plan_Id}'");
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(insertSql, null, transaction);
                    //先删除设备信息
                    rows = conn.Execute($" delete INS_plan_equipment_info e where e.plan_equipment_id in(select p.plan_equipment_id from INS_plan_equipment p left join INS_PLAN pa on pa.plan_id = p.plan_id where p.plan_id = '{model.Plan_Id}' )", transaction);
                    rows = conn.Execute($"delete INS_plan_equipment where plan_id='{model.Plan_Id}'", transaction);

                    //后添加
                    model.Ins_Plan_EquipmentList.ForEach(row =>
                    {
                        row.Plan_Id = model.Plan_Id;
                        var insertSql1 = DapperExtentions.MakeInsertSql(row, out OracleDynamicParameters parameters);
                        var rows1 = conn.Execute(insertSql1, parameters, transaction);
                        row.Ins_Plan_Equipment_InfoList.ForEach(detail =>
                        {
                            detail.Plan_Equipment_Id = row.Plan_Equipment_Id;
                            if (detail.EquType == 2)
                            {
                                detail.Geometry = detail.Lon;
                                detail.Lon = "";
                                detail.Lat = "";

                            }
                            var insertEqu =
                         //    string.Format($@"INSERT INTO INS_PLAN_EQUIPMENT_INFO(EQUIPMENT_INFO_ID,PLAN_EQUIPMENT_ID,EQUIPMENT_INFO_CODE,
                         //    EQUIPMENT_INFO_NAME,ADDRESS,CREATE_TIME,GLOBID,EQUTYPE,CALIBER,GEOMETRY,LON,LAT,SHAPE) VALUES('{Guid.NewGuid()}','{row.Plan_Equipment_Id}',
                         //'{detail.Equipment_Info_Code}','{detail.Equipment_Info_Name}','{detail.Address}',to_date('{DateTime.Now}','YYYY-mm-dd HH24:Mi:SS'),'{detail.GlobID}'
                         //,{detail.EquType},'{detail.Caliber}','{detail.Geometry}','{detail.Lon}','{detail.Lat}',(select sde.st_geometry('{detail.WKTGeometry}',4547)  from dual))");
                         $@"DECLARE  
                                                               GeometryStr clob;
                                                               WKTGeometryStr clob;  
                                                             BEGIN  
                                                               GeometryStr := '{detail.Geometry}';
                                                               WKTGeometryStr := '{detail.WKTGeometry}';
                                                               INSERT INTO INS_PLAN_EQUIPMENT_INFO(EQUIPMENT_INFO_ID,PLAN_EQUIPMENT_ID,EQUIPMENT_INFO_CODE,
                            EQUIPMENT_INFO_NAME,ADDRESS,CREATE_TIME,GLOBID,EQUTYPE,CALIBER,GEOMETRY,LON,LAT,SHAPE) VALUES('{Guid.NewGuid()}','{row.Plan_Equipment_Id}',
                        '{detail.Equipment_Info_Code}','{detail.Equipment_Info_Name}','{detail.Address}',to_date('{DateTime.Now}','YYYY-mm-dd HH24:Mi:SS'),'{detail.GlobID}'
                        ,{detail.EquType},'{detail.Caliber}',GeometryStr,'{detail.Lon}','{detail.Lat}',(select sde.st_geometry(WKTGeometryStr,4547)  from dual)); 
                                                               COMMIT;  
                                                             END;  ";
                            var rows2 = conn.Execute(insertEqu, null, transaction);
                        });
                    });
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(1);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }

        }
        /// <summary>
        /// 计划管理派发
        /// </summary>
        /// <returns></returns>
        public async Task<MessageEntity> DistributedPlan(DistributedPlanBPM model)
        {
            LogHelper.Info("计划分派开始时间" + DateTime.Now);
            range = int.Parse(Appsettings.app(new string[] { "Range" }));
            Thread t1, t2, t3, t4, t5, t6, t7;
            MessageEntity result = new MessageEntity();
            List<DistributedPlanInfo> PlanInfoList = new List<DistributedPlanInfo>();
            List<DistributedPlanInfo> PlanInfoEquList = new List<DistributedPlanInfo>();
            List<Ins_Plan_Equipment> PlanEqumentList = new List<Ins_Plan_Equipment>();

            Ins_Plan_Cycle Ins_Plan_CycleModel = new Ins_Plan_Cycle();
            try
            {
                #region 计划相关信息处理
                //首先获取计划下的设备类型
                string GetPlanEqumentSql = $"select plan_equipment_id from ins_plan_equipment e where e.plan_id='{model.Plan_Id}'";

                //计划所含设备信息查询语句
                string GetPlanInfoSql = string.Format(@"SELECT
                                                            IP.Plan_Id,
                                                        	IP.Plan_Name,
                                                        	IP.PlanCycleId,
                                                        	IP.IsNormalPlan,
                                                        	IP.IsFeedBack,
                                                        	IP.Plan_Type_Id,
                                                        	IP.Range_Name,
                                                        	IP.Geometry,
                                                        	IP.MoveType,
                                                        	IP.Plan_TemplateType_Id,
                                                        	IP.PlanTtemplate_Id,
                                                        	IPE.LayerName,
                                                        	IPE.LayerCName,
                                                            IPE.TableId,
                                                        	IPEI.Equipment_Info_Id,
                                                        	IPEI.Equipment_Info_Code,
                                                        	IPEI.Equipment_Info_Name,
                                                        	IPEI.Address,
                                                        	IPEI.Caliber,
                                                        	IPEI.Lon,
                                                        	IPEI.Lat,IPEI.Geometry as GXGeometry,IPEI.equType,IPE.plan_equipment_id
                                                        FROM
                                                        	INS_PLAN_EQUIPMENT_INFO IPEI
                                                        	LEFT JOIN INS_PLAN_EQUIPMENT IPE ON IPEI.PLAN_EQUIPMENT_ID = IPE.PLAN_EQUIPMENT_ID
                                                        	LEFT JOIN INS_PLAN IP ON IPE.PLAN_ID = IP.PLAN_ID
                                                        WHERE IP.PLAN_ID = '{0}' ", model.Plan_Id);
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    string strSqlSelect = string.Format(@"SELECT
                                                        	count( 0 ) as count
                                                        FROM
                                                        	INS_PLAN_TASK lt 
                                                        WHERE
                                                        	lt.PLAN_ID = '{0}' 
                                                        	AND (
                                                        		( lt.START_TIME >= to_date( '{1}', 'yyyy-mm-dd hh24:mi:ss' ) AND lt.START_TIME <= to_date( '{1}', 'yyyy-mm-dd hh24:mi:ss' ) ) 
                                                        		OR ( lt.START_TIME <= to_date( '{1}', 'yyyy-mm-dd hh24:mi:ss' ) AND lt.END_TIME >= to_date( '{2}', 'yyyy-mm-dd hh24:mi:ss' ) ) 
                                                        		OR ( lt.START_TIME >= to_date( '{1}', 'yyyy-mm-dd hh24:mi:ss' ) AND lt.END_TIME <= to_date( '{2}', 'yyyy-mm-dd hh24:mi:ss' ) ) 
                                                        	OR ( lt.END_TIME >= to_date( '{2}', 'yyyy-mm-dd hh24:mi:ss' ) AND lt.END_TIME <= to_date( '{2}', 'yyyy-mm-dd hh24:mi:ss' ) ) 
                                                        	) ", model.Plan_Id, model.StartDate, model.EndDate);
                    int dtSelect = conn.Query<int>(strSqlSelect).FirstOrDefault();
                    //当该计划已经对某人进行时间段内分派后直接返回
                    if (dtSelect > 0)
                    {
                        return MessageEntityTool.GetMessage(ErrorType.OprationError, null, $"该计划已被分派，不能多次分派", "提示");
                    }
                    // 查询计划所含设备类型
                    PlanEqumentList = conn.Query<Ins_Plan_Equipment>(GetPlanEqumentSql).ToList();
                    //查询计划所含设备信息
                    LogHelper.Info("查询计划设备开始时间" + DateTime.Now);
                    PlanInfoList = conn.Query<DistributedPlanInfo>(GetPlanInfoSql).ToList();
                    LogHelper.Info("查询计划设备结束时间" + DateTime.Now);
                    if (PlanInfoList.Count == 0)
                    {
                        return MessageEntityTool.GetMessage(ErrorType.SqlError, "未查询到计划信息");
                    }
                    //查询对应周期信息
                    Ins_Plan_CycleModel = conn.Query<Ins_Plan_Cycle>("select * from Ins_Plan_Cycle t where PLAN_CYCLE_ID='" + PlanInfoList[0].PlanCycleId + "'").ToList()[0];

                }
                var isNormalPlan = PlanInfoList[0].IsNormalPlan == 0 ? "CGRW" : "LSRW";
                if (model.StartDate > model.EndDate)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "结束时间不能早于开始时间");
                }
                //周期时间段拆分
                List<Dictionary<string, string>> CycleList = new CalculateCycle().CalculateMainMethod(model.StartDate, model.EndDate, int.Parse(Ins_Plan_CycleModel.CycleTime), Ins_Plan_CycleModel.CycleHz, Ins_Plan_CycleModel.CycleUnit, "");
                if (CycleList.Count == 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "周期换算失败");
                }
                #endregion
                #region 记录到计划任务表中
                List<Ins_Task> task = null;
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    string tasksql = $"SELECT '{isNormalPlan}'  ||  to_char(sysdate,'yyyymmdd')|| nvl(replace(lPAD( MAX( substr(taskcode,13,4)+ 1),4), ' ','0'),'0001') as taskcode from ins_task where  operateDate>= to_date( to_char(sysdate, 'YYYY-MM-DD'), 'YYYY-MM-DD')";
                    task = conn.Query<Ins_Task>(tasksql).ToList();
                }
                LogHelper.Info("循环获取实体开始时间" + DateTime.Now);

                #region  循环获取实体
                //  IDbTransaction transaction = conn.BeginTransaction();
                List<Ins_Task> tasklist = new List<Ins_Task>();
                plantasklist = new List<Ins_Plan_Task>();
                formvalvelist = new List<Ins_Form_Valve>();
                formfirelist = new List<Ins_Form_FireHydrant>();
                formleakdetectionlist = new List<Ins_Form_LeakDetection>();
                formStoreWaterlist = new List<Ins_Form_StoreWater>();
                formPumplist = new List<Ins_Form_Pump>();
                formPipelineequlist = new List<Ins_Form_Pipelineequ>();
                var num = "";
                for (int i = 0; i < CycleList.Count; i++)
                {
                    //string TaskId = Guid.NewGuid().ToString();
                    Ins_Task TaskModel = new Ins_Task();
                    if (i == 0)
                    {
                        num = task[0].TaskCode.Substring(4, 12);
                        TaskModel.TaskCode = task[0].TaskCode;
                    }
                    else
                    {

                        var code = long.Parse(num) + 1;
                        num = code.ToString();
                        TaskModel.TaskCode = isNormalPlan + code.ToString();

                    }
                    TaskModel.TaskId = Guid.NewGuid().ToString();
                    TaskModel.TaskName = model.TaskName;
                    TaskModel.ProraterDeptName = model.DepartmentName;
                    TaskModel.ProraterDeptId = model.DepartmentId;
                    TaskModel.ProraterId = model.ExecPersonId;
                    TaskModel.ProraterName = model.ExecPersonName;
                    TaskModel.VisitStarTime = DateTime.Parse(CycleList[i]["StartDay"]);
                    TaskModel.VisitOverTime = DateTime.Parse(CycleList[i]["EndDay"]);
                    TaskModel.Frequency = Ins_Plan_CycleModel.CycleHz.ToString();
                    TaskModel.Descript = model.TaskDescription;
                    TaskModel.Operator = model.CreatorName;
                    TaskModel.OperatorId = model.CreatorId;
                    TaskModel.OperateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    TaskModel.TaskState = 1;
                    TaskModel.Plan_Id = model.Plan_Id;
                    TaskModel.Plan_Name = model.Plan_Name;
                    TaskModel.Remark = model.Remark;
                    TaskModel.IsFinish = 0;
                    TaskModel.AssignState = 0;
                    TaskModel.Task_TypeId = model.Task_Type_Id;
                    tasklist.Add(TaskModel);
                    // conn.Execute(DapperExtentions.MakeInsertSql(TaskModel), TaskModel, transaction);

                    Ins_Plan_Task InsertModel = null;
                    Ins_Form_Valve ValveModel = null;
                    Ins_Form_FireHydrant FireHydrantModel = null;
                    Ins_Form_LeakDetection LeakDetectionModel = null;
                    Ins_Form_StoreWater StoreWaterModel = null;
                    Ins_Form_Pipelineequ PipelineequModel = null;
                    Ins_Form_Pump PumpModel = null;
                    for (int ii = 0; ii < PlanEqumentList.Count; ii++)
                    {
                        PlanInfoEquList = PlanInfoList.Where(x => x.Plan_Equipment_Id == PlanEqumentList[ii].Plan_Equipment_Id).ToList();
                        LogHelper.Info("设备类型和条数:" + PlanEqumentList[ii].Plan_Equipment_Id + "----" + PlanInfoEquList.Count);

                        for (int j = 0; j < PlanInfoEquList.Count; j++)
                        {
                            string plan_task_id = Guid.NewGuid().ToString();
                            InsertModel = new Ins_Plan_Task();
                            InsertModel.Plan_Task_Id = plan_task_id;
                            InsertModel.Equipment_Info_Id = PlanInfoEquList[j].Equipment_Info_Id;
                            InsertModel.Plan_Id = PlanInfoEquList[j].Plan_Id;
                            InsertModel.Start_Time = DateTime.Parse(CycleList[i]["StartDay"]);
                            InsertModel.TaskName = model.TaskName;
                            InsertModel.End_Time = DateTime.Parse(CycleList[i]["EndDay"]);
                            InsertModel.TableId = PlanInfoEquList[j].TableId;
                            InsertModel.IsSuccess = 0;
                            InsertModel.TaskId = TaskModel.TaskId;
                            InsertModel.Creator_Nm = model.CreatorName;
                            InsertModel.Creator_Id = model.CreatorId;
                            InsertModel.DepartmentId = model.DepartmentId;
                            InsertModel.DepartmentName = model.DepartmentName;
                            InsertModel.ExecPersonId = model.ExecPersonId;
                            InsertModel.ExecPersonName = model.ExecPersonName;
                            InsertModel.TaskDescription = model.TaskDescription;
                            InsertModel.IsFinish = 0;
                            plantasklist.Add(InsertModel);
                            //  conn.Execute(DapperExtentions.MakeInsertSql(InsertModel), InsertModel, transaction);

                            if (PlanInfoEquList[j].TableId == "Ins_Form_Valve")
                            {
                                ValveModel = new Ins_Form_Valve();
                                ValveModel.ID = Guid.NewGuid().ToString();
                                ValveModel.Plan_task_id = InsertModel.Plan_Task_Id;
                                ValveModel.TaskId = TaskModel.TaskId;
                                ValveModel.TaskName = model.TaskName;
                                ValveModel.GlobID = PlanInfoEquList[j].GlobID;
                                ValveModel.LayerName = PlanInfoEquList[j].Equipment_Info_Name;
                                if (PlanInfoEquList[j].EquType == 2)
                                {
                                    ValveModel.Geometry = PlanInfoEquList[j].Lon;
                                    ValveModel.X = "";
                                    ValveModel.Y = "";

                                }
                                else
                                {
                                    ValveModel.X = PlanInfoEquList[j].Lon;
                                    ValveModel.Y = PlanInfoEquList[j].Lat;
                                }
                                formvalvelist.Add(ValveModel);
                                //   var insertEqu = DapperExtentions.MakeInsertSql(ValveModel, out OracleDynamicParameters parameters1);
                                // var rows2 = conn.Execute(insertEqu, parameters1, transaction);


                            }
                            else if (PlanInfoEquList[j].TableId == "Ins_Form_FireHydrant")
                            {
                                FireHydrantModel = new Ins_Form_FireHydrant();
                                FireHydrantModel.ID = Guid.NewGuid().ToString();
                                FireHydrantModel.Plan_task_id = InsertModel.Plan_Task_Id;
                                FireHydrantModel.TaskId = TaskModel.TaskId;
                                FireHydrantModel.TaskName = model.TaskName;
                                FireHydrantModel.GlobID = PlanInfoEquList[j].GlobID;
                                FireHydrantModel.LayerName = PlanInfoEquList[j].Equipment_Info_Name;
                                if (PlanInfoEquList[j].EquType == 2)
                                {
                                    FireHydrantModel.Geometry = PlanInfoEquList[j].Lon;
                                    FireHydrantModel.X = "";
                                    FireHydrantModel.Y = "";

                                }
                                else
                                {
                                    FireHydrantModel.X = PlanInfoEquList[j].Lon;
                                    FireHydrantModel.Y = PlanInfoEquList[j].Lat;
                                }
                                formfirelist.Add(FireHydrantModel);

                            }
                            else if (PlanInfoEquList[j].TableId == "Ins_Form_LeakDetection")
                            {
                                LeakDetectionModel = new Ins_Form_LeakDetection();
                                LeakDetectionModel.ID = Guid.NewGuid().ToString();
                                LeakDetectionModel.Plan_task_id = InsertModel.Plan_Task_Id;
                                LeakDetectionModel.TaskId = TaskModel.TaskId;
                                LeakDetectionModel.TaskName = model.TaskName;
                                LeakDetectionModel.LayerName = PlanInfoEquList[j].LayerName;
                                formleakdetectionlist.Add(LeakDetectionModel);
                                break;//管线时不存具体管线信息
                            }
                            else if (PlanInfoEquList[j].TableId == "Ins_Form_StoreWater")
                            {
                                StoreWaterModel = new Ins_Form_StoreWater();
                                StoreWaterModel.ID = Guid.NewGuid().ToString();
                                StoreWaterModel.Plan_task_id = InsertModel.Plan_Task_Id;
                                StoreWaterModel.TaskId = TaskModel.TaskId;
                                StoreWaterModel.TaskName = model.TaskName;
                                StoreWaterModel.GlobID = PlanInfoEquList[j].GlobID;
                                StoreWaterModel.LayerName = PlanInfoEquList[j].Equipment_Info_Name;
                                if (PlanInfoEquList[j].EquType == 2)
                                {
                                    StoreWaterModel.X = "";
                                    StoreWaterModel.Y = "";
                                }
                                else
                                {
                                    StoreWaterModel.X = PlanInfoEquList[j].Lon;
                                    StoreWaterModel.Y = PlanInfoEquList[j].Lat;
                                }
                                formStoreWaterlist.Add(StoreWaterModel);

                            }
                            else if (PlanInfoEquList[j].TableId == "Ins_Form_PipelineEqu")
                            {
                                PipelineequModel = new Ins_Form_Pipelineequ();
                                PipelineequModel.ID = Guid.NewGuid().ToString();
                                PipelineequModel.Plan_task_id = InsertModel.Plan_Task_Id;
                                PipelineequModel.TaskId = TaskModel.TaskId;
                                PipelineequModel.TaskName = model.TaskName;
                                PipelineequModel.GlobID = PlanInfoEquList[j].GlobID;
                                PipelineequModel.LayerName = PlanInfoEquList[j].Equipment_Info_Name;
                                if (PlanInfoEquList[j].EquType == 2)
                                {
                                    PipelineequModel.X = "";
                                    PipelineequModel.Y = "";
                                }
                                else
                                {
                                    PipelineequModel.X = PlanInfoEquList[j].Lon;
                                    PipelineequModel.Y = PlanInfoEquList[j].Lat;
                                }
                                formPipelineequlist.Add(PipelineequModel);

                            }
                            else if (PlanInfoEquList[j].TableId == "Ins_Form_Pump")
                            {
                                PumpModel = new Ins_Form_Pump();
                                PumpModel.ID = Guid.NewGuid().ToString();
                                PumpModel.Plan_task_id = InsertModel.Plan_Task_Id;
                                PumpModel.TaskId = TaskModel.TaskId;
                                PumpModel.TaskName = model.TaskName;
                                PumpModel.GlobID = PlanInfoEquList[j].GlobID;
                                PumpModel.LayerName = PlanInfoEquList[j].Equipment_Info_Name;
                                if (PlanInfoEquList[j].EquType == 2)
                                {
                                    PumpModel.X = "";
                                    PumpModel.Y = "";
                                }
                                else
                                {
                                    PumpModel.X = PlanInfoEquList[j].Lon;
                                    PumpModel.Y = PlanInfoEquList[j].Lat;
                                }
                                formPumplist.Add(PumpModel);

                            }
                        }

                    }
                }
                #endregion

                LogHelper.Info("循环获取实体结束时间" + DateTime.Now);

                // transaction.Commit();
                StringBuilder tasksql1 = new StringBuilder();
                StringBuilder plantasksql = new StringBuilder();
                StringBuilder formvalvesql = new StringBuilder();
                StringBuilder formFiresql = new StringBuilder();
                var parametervalve = new OracleDynamicParameters();
                var parameterFire = new OracleDynamicParameters();
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    //int range = 2000;//每批的数量
                    #region   生成任务
                    //遍历任务集合
                    tasklist.ForEach(row =>
                {
                    tasksql1.Append($@"INSERT INTO Ins_Task (TaskId,TaskCode,TaskName,ProraterDeptName,ProraterDeptId,ProraterId,ProraterName,VisitStarTime,VisitOverTime,Frequency,Descript,OperatorId,Operator,TaskState,Plan_Id,Plan_Name,Remark,IsFinish,AssignState,Task_TypeId)
                                                        values
                                                          ('{row.TaskId}', '{row.TaskCode}', '{row.TaskName}','{row.ProraterDeptName}', '{row.ProraterDeptId}','{row.ProraterId}', '{row.ProraterName}',to_date('{row.VisitStarTime}', 'YYYY-mm-dd HH24:Mi:SS'),
                                                           to_date('{row.VisitOverTime}', 'YYYY-mm-dd HH24:Mi:SS'), '{row.Frequency}', '{row.Descript}', '{row.OperatorId}','{row.Operator}', {row.TaskState},'{row.Plan_Id}',
                                                           '{row.Plan_Name}', '{row.Remark}', {row.IsFinish}, {row.AssignState},'{row.Task_TypeId}');");
                });

                    conn.Execute("begin  " + tasksql1.ToString() + " end;");

                    #endregion
                    LogHelper.Info("生成数据开始时间" + DateTime.Now);

                    // 执行一个无返回值的任务
                    List<Thread> ThreadList = new List<Thread>();
                    //创建前台工作线程
                    t1 = new Thread(Task1);
                    t2 = new Thread(Task2);
                    t3 = new Thread(Task3);
                    t4 = new Thread(Task4);
                    t5 = new Thread(Task5);//蓄水設施
                    t6 = new Thread(Task6);//共用管道及附属设备
                    t7 = new Thread(Task7);//泵房

                    t1.Start();
                    ThreadList.Add(t1);//保存起来
                    if (formfirelist.Count > 0)
                    {
                        ThreadList.Add(t2);//保存起来
                        t2.Start();
                    }

                    if (formvalvelist.Count > 0)
                    {
                        ThreadList.Add(t3);//保存起来
                        t3.Start();
                    }
                    if (formleakdetectionlist.Count > 0)
                    {
                        ThreadList.Add(t4);//保存起来
                        t4.Start();
                    }
                    if (formStoreWaterlist.Count > 0)
                    {
                        ThreadList.Add(t5);//保存起来
                        t5.Start();
                    }
                    if (formPipelineequlist.Count > 0)
                    {
                        ThreadList.Add(t6);//保存起来
                        t6.Start();
                    }
                    if (formPumplist.Count > 0)
                    {
                        ThreadList.Add(t7);//保存起来
                        t7.Start();
                    }


                    //判断线程是bai否结束du
                    List<Thread> List2Remove = new List<Thread>();
                    while (ThreadList.Count > 0)
                    {
                        foreach (Thread a in ThreadList)
                        {
                            if (a.IsAlive == false) List2Remove.Add(a);

                        }
                        foreach (Thread a in List2Remove)
                        {
                            if (a.IsAlive == false) ThreadList.Remove(a);
                        }
                    }
                    LogHelper.Info("生成数据结束时间" + DateTime.Now);

                    if (ThreadList.Count == 0 && isSucessPlantask == true && isSucessFormFire == true && isSucessFormValve == true && isSucessLeakDetection == true)
                    {
                        t1.Interrupt();
                        if (formfirelist.Count > 0)
                            t2.Interrupt();
                        if (formvalvelist.Count > 0)
                            t3.Interrupt();
                        if (formleakdetectionlist.Count > 0)
                            t4.Interrupt();
                        if (formStoreWaterlist.Count > 0)
                            t5.Interrupt();
                        if (formPipelineequlist.Count > 0)
                            t6.Interrupt();
                        if (formPumplist.Count > 0)
                            t7.Interrupt();
                        conn.Execute(string.Format(@"UPDATE Ins_Plan SET ASSIGN_STATE =1 WHERE PLAN_ID = '{0}'", model.Plan_Id));
                        LogHelper.Info("计划分派完成时间" + DateTime.Now);
                        return MessageEntityTool.GetMessage(0, null, true, "计划派发成功", 0);
                    }
                    else
                    {
                        t1.Interrupt();
                        if (formfirelist.Count > 0)
                            t2.Interrupt();
                        if (formvalvelist.Count > 0)
                            t3.Interrupt();
                        if (formleakdetectionlist.Count > 0)
                            t4.Interrupt();
                        if (formStoreWaterlist.Count > 0)
                            t5.Interrupt();
                        if (formPipelineequlist.Count > 0)
                            t6.Interrupt();
                        if (formPumplist.Count > 0)
                            t7.Interrupt();
                        conn.Execute($@" begin 
                         delete INS_FORM_FIREHYDRANT p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete ins_form_valve p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete Ins_Form_LeakDetection p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete Ins_Form_StoreWater p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete Ins_Form_PipelineEqu p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete Ins_Form_Pump p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete  ins_plan_task p where p.plan_id='{model.Plan_Id}';
                         delete  INS_TASK p where p.plan_id='{model.Plan_Id}'; 
                               end;");

                        LogHelper.Info("线程执行异常开始时间" + DateTime.Now);
                        return MessageEntityTool.GetMessage(8, "计划派发失败");

                    }

                }


                #endregion //记录数据结束
            }
            catch (Exception e)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    conn.Execute($@" begin 
                         delete INS_FORM_FIREHYDRANT p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete ins_form_valve p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete Ins_Form_LeakDetection p where p.taskid in(select taskid from INS_TASK where plan_id='{model.Plan_Id}');
                         delete  ins_plan_task p where p.plan_id='{model.Plan_Id}';
                         delete  INS_TASK p where p.plan_id='{model.Plan_Id}'; 
                               end;");
                }
                LogHelper.Info("执行任务开始时间" + DateTime.Now + "异常信息" + e.Message);
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }
        public void Task1()
        {
            isSucessPlantask = ExcutePlanTask(plantasklist, range);
        }
        public void Task2()
        {
            isSucessFormFire = ExcuteFormFire(formfirelist, range);
        }
        public void Task3()
        {
            isSucessFormValve = ExcuteValve(formvalvelist, range);
        }
        public void Task4()
        {
            isSucessLeakDetection = ExcuteLeakDetection(formleakdetectionlist, range);
        }
        public void Task5()
        {
            isSucessFormStoreWater = ExcuteStoreWater(formStoreWaterlist, range);
        }
        public void Task6()
        {
            isSucessFormPipelineequ = ExcutePipelineequ(formPipelineequlist, range);
        }
        public void Task7()
        {
            isSucessFormPump = ExcutePump(formPumplist, range);
        }
        public bool ExcutePlanTask(List<Ins_Plan_Task> plantasklist, int range)
        {
            StringBuilder plantasksql = new StringBuilder();
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    #region 分批次处理任务明细数据
                    //分批次处理任务明细数据
                    int plantasklistcount = plantasklist.Count;
                    List<Ins_Plan_Task> plantasklistRe = new List<Ins_Plan_Task>();
                    plantasklist.ForEach(row =>
                    {
                        plantasklistRe.Add(row);
                    });
                    if (plantasklist != null && plantasklist.Count > 0)
                    {
                        int times = plantasklist.Count / range + (plantasklist.Count % range > 0 ? 1 : 0);//交互次数
                        for (int i = 0; i < times; i++)
                        {
                            plantasksql = new StringBuilder();
                            var tmpIds = plantasklist.GetRange(i * range, (i + 1) * range > plantasklistcount ? (plantasklistcount - i * range) : range);
                            //取出本次执行条数
                            List<Ins_Plan_Task> sortedList1 = (from a in plantasklistRe select a).Take(tmpIds.Count).ToList();
                            sortedList1.ForEach(row =>
                            {
                                plantasksql.Append($@"   INSERT INTO Ins_Plan_Task (Plan_Task_Id,Equipment_Info_Id,Plan_Id,Start_Time,End_Time,TaskName,TableId,IsSuccess,Creator_Nm,Creator_Id,DepartmentId,DepartmentName,ExecPersonId,ExecPersonName,TaskDescription,IsFinish,TaskId)
                                                                        values ('{row.Plan_Task_Id}','{row.Equipment_Info_Id}','{row.Plan_Id}',
                                                          to_date('{row.Start_Time}', 'YYYY-mm-dd HH24:Mi:SS'),to_date('{row.End_Time}', 'YYYY-mm-dd HH24:Mi:SS'),'{row.TaskName}','{row.TableId}',{row.IsSuccess},'{row.Creator_Nm}','{row.Creator_Id}','{row.DepartmentId}','{row.DepartmentName}','{row.ExecPersonId}','{row.ExecPersonName}','{row.TaskDescription}',{row.IsFinish},'{row.TaskId}');");
                            });

                            conn.Execute("begin  " + plantasksql.ToString() + " end;");

                            //拼接成功以后移除本次数据
                            plantasklistRe.RemoveRange(0, tmpIds.Count);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Info("生成任务明细异常:" + ex.Message);
                return false;
            }
            #endregion
        }
        public bool ExcuteFormFire(List<Ins_Form_FireHydrant> formfirelist, int range)
        {
            StringBuilder formFiresql = new StringBuilder();
            var parameterFire = new OracleDynamicParameters();
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    #region 分批处理填单明细数据 消火栓
                    //分批处理填单明细数据
                    if (formfirelist != null && formfirelist.Count > 0)
                    {
                        int listcount = formfirelist.Count;
                        List<Ins_Form_FireHydrant> formfirelistRe = new List<Ins_Form_FireHydrant>();
                        formfirelist.ForEach(row =>
                        {
                            formfirelistRe.Add(row);
                        });
                        int times = formfirelist.Count / range + (formfirelist.Count % range > 0 ? 1 : 0);//交互次数
                        for (int i = 0; i < times; i++)
                        {
                            formFiresql = new StringBuilder();
                            var tmpIds = formfirelist.GetRange(i * range, (i + 1) * range > listcount ? (listcount - i * range) : range);
                            //取出本次执行条数
                            List<Ins_Form_FireHydrant> sortedList1 = (from a in formfirelistRe select a).Take(tmpIds.Count).ToList();
                            int k = 0;
                            sortedList1.ForEach(row =>
                            {
                                parameterFire.Add($"clob{k}", row.Geometry);
                                formFiresql.Append($@"
       INSERT INTO Ins_Form_FireHydrant (ID,TaskId,TaskName,IsExistProblem,DetailAddress,IsMissCover,IsOldStain,IsHeight,IsleakWater,IsTilt,IsFixedBrand,IsRectification,WorkCode,GlobID,LayerName,X,Y,Geometry,Plan_task_id,IsFillForm) 
                                 values ('{row.ID}','{row.TaskId}','{row.TaskName}',{row.IsExistProblem},'{row.DetailAddress}',{row.IsMissCover},{row.IsOldStain},{row.IsHeight},{row.IsleakWater},{row.IsTilt},{row.IsFixedBrand},{row.IsRectification},'{row.WorkCode}','{row.GlobID}','{row.LayerName}','{row.X}','{row.Y}',:clob{k},'{row.Plan_task_id}',{row.IsFillForm}); ");
                                k++;
                            });

                            conn.Execute("begin  " + formFiresql.ToString() + " end;", parameterFire);

                            //拼接成功以后移除本次数据
                            formfirelistRe.RemoveRange(0, tmpIds.Count);
                        }

                    }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Info("生成消火栓明细异常:" + ex.Message);
                return false;
            }
        }
        public bool ExcuteValve(List<Ins_Form_Valve> formvalvelist, int range)
        {
            StringBuilder formvalvesql = new StringBuilder();
            var parametervalve = new OracleDynamicParameters();
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    #region 分批处理填单明细数据 阀门
                    //分批处理填单明细数据
                    if (formvalvelist != null && formvalvelist.Count > 0)
                    {
                        int listcount = formvalvelist.Count;
                        List<Ins_Form_Valve> formvalvelistRe = new List<Ins_Form_Valve>();
                        formvalvelist.ForEach(row =>
                        {
                            formvalvelistRe.Add(row);
                        });
                        int times = formvalvelist.Count / range + (formvalvelist.Count % range > 0 ? 1 : 0);//交互次数
                        for (int i = 0; i < times; i++)
                        {
                            formvalvesql = new StringBuilder();
                            var tmpIds = formvalvelist.GetRange(i * range, (i + 1) * range > listcount ? (listcount - i * range) : range);
                            //取出本次执行条数
                            List<Ins_Form_Valve> sortedList1 = (from a in formvalvelistRe select a).Take(tmpIds.Count).ToList();
                            int kk = 0;
                            sortedList1.ForEach(row =>
                            {
                                parametervalve.Add($"clob{kk}", row.Geometry);
                                formvalvesql.Append($@"
   INSERT INTO Ins_Form_Valve (ID,TaskId,TaskName,IsExistProblem,DetailAddress,IsCover,IsLose,IsDamage,IsRectification,WorkCode,GlobID,LayerName,X,Y,Geometry,Plan_task_id,IsFillForm) 
                             values ('{row.ID}','{row.TaskId}','{row.TaskName}',{row.IsExistProblem},'{row.DetailAddress}',{row.IsCover},{row.IsLose},{row.IsDamage},{row.IsRectification},'{row.WorkCode}','{row.GlobID}','{row.LayerName}','{row.X}','{row.Y}',:clob{kk},'{row.Plan_task_id}',{row.IsFillForm}); ");
                                kk++;
                            });

                            conn.Execute("begin  " + formvalvesql.ToString() + " end;", parametervalve);

                            //拼接成功以后移除本次数据
                            formvalvelistRe.RemoveRange(0, tmpIds.Count);
                        }

                    }

                    #endregion
                }
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.Info("生成消火栓明细异常:" + ex.Message);
                return false;
            }
        }
        public bool ExcuteLeakDetection(List<Ins_Form_LeakDetection> formleakdetectionlist, int range)
        {
            StringBuilder formvalvesql = new StringBuilder();
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    #region 分批处理填单明细数据 阀门
                    //分批处理填单明细数据
                    if (formleakdetectionlist != null && formleakdetectionlist.Count > 0)
                    {
                        int listcount = formleakdetectionlist.Count;
                        List<Ins_Form_LeakDetection> formvalvelistRe = new List<Ins_Form_LeakDetection>();
                        formleakdetectionlist.ForEach(row =>
                        {
                            formvalvelistRe.Add(row);
                        });
                        int times = formleakdetectionlist.Count / range + (formleakdetectionlist.Count % range > 0 ? 1 : 0);//交互次数
                        for (int i = 0; i < times; i++)
                        {
                            formvalvesql = new StringBuilder();
                            var tmpIds = formleakdetectionlist.GetRange(i * range, (i + 1) * range > listcount ? (listcount - i * range) : range);
                            //取出本次执行条数
                            List<Ins_Form_LeakDetection> sortedList1 = (from a in formvalvelistRe select a).Take(tmpIds.Count).ToList();
                            sortedList1.ForEach(row =>
                            {
                                formvalvesql.Append($@"
   INSERT INTO INS_FORM_LEAKDETECTION (ID,TaskId,TaskName,WorkCode,LayerName,remark,caliber,Plan_task_id,IsFillForm) 
                             values ('{row.ID}','{row.TaskId}','{row.TaskName}','{row.WorkCode}','{row.LayerName}','{row.Remark}','{row.Caliber}','{row.Plan_task_id}',{row.IsFillForm}); ");
                            });

                            conn.Execute("begin  " + formvalvesql.ToString() + " end;");

                            //拼接成功以后移除本次数据
                            formvalvelistRe.RemoveRange(0, tmpIds.Count);
                        }

                    }

                    #endregion
                }
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.Info("生成检漏明细异常:" + ex.Message);
                return false;
            }
        }
        public bool ExcuteStoreWater(List<Ins_Form_StoreWater> formStoreWaterlist, int range)
        {
            StringBuilder formvalvesql = new StringBuilder();
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    #region 分批处理填单明细数据 阀门
                    //分批处理填单明细数据
                    if (formStoreWaterlist != null && formStoreWaterlist.Count > 0)
                    {
                        int listcount = formStoreWaterlist.Count;
                        List<Ins_Form_StoreWater> formvalvelistRe = new List<Ins_Form_StoreWater>();
                        formStoreWaterlist.ForEach(row =>
                        {
                            formvalvelistRe.Add(row);
                        });
                        int times = formStoreWaterlist.Count / range + (formStoreWaterlist.Count % range > 0 ? 1 : 0);//交互次数
                        for (int i = 0; i < times; i++)
                        {
                            formvalvesql = new StringBuilder();
                            var tmpIds = formStoreWaterlist.GetRange(i * range, (i + 1) * range > listcount ? (listcount - i * range) : range);
                            //取出本次执行条数
                            List<Ins_Form_StoreWater> sortedList1 = (from a in formvalvelistRe select a).Take(tmpIds.Count).ToList();
                            sortedList1.ForEach(row =>
                            {
                                formvalvesql.Append($@"
                                   INSERT INTO Ins_Form_StoreWater (ID,TaskId,TaskName,WorkCode,GlobID,LayerName,X,Y,Plan_task_id,IsFillForm) 
                             values ('{row.ID}','{row.TaskId}','{row.TaskName}','{row.WorkCode}','{row.GlobID}','{row.LayerName}','{row.X}','{row.Y}','{row.Plan_task_id}',{row.IsFillForm}); ");
                            });

                            conn.Execute("begin  " + formvalvesql.ToString() + " end;");

                            //拼接成功以后移除本次数据
                            formvalvelistRe.RemoveRange(0, tmpIds.Count);
                        }

                    }

                    #endregion
                }
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.Info("生成检漏明细异常:" + ex.Message);
                return false;
            }
        }
        public bool ExcutePipelineequ(List<Ins_Form_Pipelineequ> formPipelineequlist, int range)
        {
            StringBuilder formvalvesql = new StringBuilder();
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    #region 分批处理填单明细数据 阀门
                    //分批处理填单明细数据
                    if (formPipelineequlist != null && formPipelineequlist.Count > 0)
                    {
                        int listcount = formPipelineequlist.Count;
                        List<Ins_Form_Pipelineequ> formvalvelistRe = new List<Ins_Form_Pipelineequ>();
                        formPipelineequlist.ForEach(row =>
                        {
                            formvalvelistRe.Add(row);
                        });
                        int times = formPipelineequlist.Count / range + (formPipelineequlist.Count % range > 0 ? 1 : 0);//交互次数
                        for (int i = 0; i < times; i++)
                        {
                            formvalvesql = new StringBuilder();
                            var tmpIds = formPipelineequlist.GetRange(i * range, (i + 1) * range > listcount ? (listcount - i * range) : range);
                            //取出本次执行条数
                            List<Ins_Form_Pipelineequ> sortedList1 = (from a in formvalvelistRe select a).Take(tmpIds.Count).ToList();
                            sortedList1.ForEach(row =>
                            {
                                formvalvesql.Append($@"
                                   INSERT INTO Ins_Form_Pipelineequ (ID,TaskId,TaskName,WorkCode,GlobID,LayerName,X,Y,Plan_task_id,IsFillForm) 
                             values ('{row.ID}','{row.TaskId}','{row.TaskName}','{row.WorkCode}','{row.GlobID}','{row.LayerName}','{row.X}','{row.Y}','{row.Plan_task_id}',{row.IsFillForm}); ");
                            });

                            conn.Execute("begin  " + formvalvesql.ToString() + " end;");

                            //拼接成功以后移除本次数据
                            formvalvelistRe.RemoveRange(0, tmpIds.Count);
                        }

                    }

                    #endregion
                }
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.Info("生成检漏明细异常:" + ex.Message);
                return false;
            }
        }
        public bool ExcutePump(List<Ins_Form_Pump> formPumplist, int range)
        {
            StringBuilder formvalvesql = new StringBuilder();
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    #region 分批处理填单明细数据 阀门
                    //分批处理填单明细数据
                    if (formPumplist != null && formPumplist.Count > 0)
                    {
                        int listcount = formPumplist.Count;
                        List<Ins_Form_Pump> formvalvelistRe = new List<Ins_Form_Pump>();
                        formPumplist.ForEach(row =>
                        {
                            formvalvelistRe.Add(row);
                        });
                        int times = formPumplist.Count / range + (formPumplist.Count % range > 0 ? 1 : 0);//交互次数
                        for (int i = 0; i < times; i++)
                        {
                            formvalvesql = new StringBuilder();
                            var tmpIds = formPumplist.GetRange(i * range, (i + 1) * range > listcount ? (listcount - i * range) : range);
                            //取出本次执行条数
                            List<Ins_Form_Pump> sortedList1 = (from a in formvalvelistRe select a).Take(tmpIds.Count).ToList();
                            sortedList1.ForEach(row =>
                            {
                                formvalvesql.Append($@"
                                   INSERT INTO Ins_Form_Pump (ID,TaskId,TaskName,WorkCode,GlobID,LayerName,X,Y,Plan_task_id,IsFillForm) 
                             values ('{row.ID}','{row.TaskId}','{row.TaskName}','{row.WorkCode}','{row.GlobID}','{row.LayerName}','{row.X}','{row.Y}','{row.Plan_task_id}',{row.IsFillForm}); ");
                            });

                            conn.Execute("begin  " + formvalvesql.ToString() + " end;");

                            //拼接成功以后移除本次数据
                            formvalvelistRe.RemoveRange(0, tmpIds.Count);
                        }

                    }

                    #endregion
                }
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.Info("生成检漏明细异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 计划管理重发
        /// </summary>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        public MessageEntity RepeatDistributedPlan(string PlanId)
        {
            //BPMCreateOrder(PlanId);
            return MessageEntityTool.GetMessage(0, null, true, "计划重发中", 0);
        }
        /// <summary>
        /// 调用BPM接口创建工单
        /// </summary>
        /// <param name="PlanId"></param>
        //public async void BPMCreateOrder(string PlanId)
        //{
        //    string BPMUrl = Appsettings.app(new string[] { "BPMDomain" });
        //    List<dynamic> AllBpmProcess = new List<dynamic>();
        //    List<DistributedPlanInfo> PlanInfoList = new List<DistributedPlanInfo>();
        //    try
        //    {
        //        #region 从任务表中查询到需要进行创建流程的数据
        //        string QuerySql = string.Format(@"SELECT
        //                                    	IPT.Plan_Id,
        //                                    	IP.Plan_Name,
        //                                    	IP.PlanCycleId,
        //                                    	IP.IsNormalPlan,
        //                                    	IP.IsFeedBack,
        //                                    	IP.Plan_Type_Id,
        //                                    	IP.Range_Name,
        //                                    	IP.Geometry,
        //                                    	IP.MoveType,
        //                                    	IP.Plan_TemplateType_Id,
        //                                    	IP.PlanTtemplate_Id,
        //                                    	IPE.GroupName,
        //                                    	IPT.Bpm_Id,
        //                                    	IPT.Bpm_Name,
        //                                    	IPE.GroupCname,
        //                                    	IPT.Bpm_Code,
        //                                    	IPT.Equipment_Info_Id,
        //                                    	IPEI.Equipment_Info_Code,
        //                                    	IPEI.Equipment_Info_Name,
        //                                    	IPEI.Address,
        //                                    	IPEI.Caliber,
        //                                    	IPEI.Lon,
        //                                    	IPEI.Lat,
        //                                    	IPT.Start_Time,
        //                                    	IPT.Task_Name,
        //                                    	IPT.End_Time,
        //                                    	IPT.Creator_Nm,
        //                                    	IPT.Creator_Id,
        //                                    	IPT.Creator_Gid,
        //                                    	IPT.Creator_Gnm,
        //                                    	IPT.Category,
        //                                    	IPT.DepartmentId,
        //                                    	IPT.DepartmentName,
        //                                    	IPT.ExecPersonId,
        //                                    	IPT.ExecPersonName,
        //                                    	IPT.TaskDescription,
        //                                        IPT.Plan_Task_Id 
        //                                    FROM
        //                                    	INS_PLAN_TASK IPT
        //                                    	LEFT JOIN INS_PLAN_EQUIPMENT_INFO IPEI ON IPT.EQUIPMENT_INFO_ID = IPEI.EQUIPMENT_INFO_ID
        //                                    	LEFT JOIN INS_PLAN_EQUIPMENT IPE ON IPEI.PLAN_EQUIPMENT_ID = IPE.PLAN_EQUIPMENT_ID
        //                                    	LEFT JOIN INS_PLAN IP ON IPT.PLAN_ID = IP.PLAN_ID WHERE IPT.ISSUCCESS = 0 AND IPT.Plan_Id = '{0}'", PlanId);
        //        using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
        //        {
        //            //查询计划所含设备信息
        //            PlanInfoList = conn.Query<DistributedPlanInfo>(QuerySql).ToList();
        //        }
        //        #endregion
        //        #region 获取BPM流程信息
        //        using (WebClient webClient = new WebClient())
        //        {
        //            //获取BPM所有流程信息接口地址
        //            string Url = BPMUrl + "/bpm/customize-api/definition/categorized";
        //            #region
        //            //发送数据
        //            string responseData = Encoding.UTF8.GetString(webClient.DownloadData(Url));
        //            AllBpmProcess = JsonConvert.DeserializeObject<List<dynamic>>(responseData);
        //            if (AllBpmProcess.Count == 0)
        //            {
        //                MessageEntityTool.GetMessage(ErrorType.SqlError, "未查询到BPM流程信息");
        //            }
        //            #endregion
        //        }
        //        #endregion
        //        #region 调用BPM接口
        //        LogHelper.Info("开始调用接口，需要处理数量:" + PlanInfoList.Count + "条");
        //        using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
        //        {
        //            conn.Execute(string.Format(@"UPDATE Ins_Plan SET ASSIGN_STATE = 1 WHERE PLAN_ID = '{0}'", PlanId), null);
        //            for (int j = 0; j < PlanInfoList.Count; j++)
        //            {
        //                dynamic BpmProcessModel = AllBpmProcess.FirstOrDefault(e => e.key == PlanInfoList[j].Bpm_Code);
        //                if (BpmProcessModel == null)
        //                {
        //                    continue;
        //                }
        //                using (WebClient webClient = new WebClient())
        //                {
        //                    string data = JsonConvert.SerializeObject(new
        //                    {
        //                        creator_nm = PlanInfoList[j].creator_nm,
        //                        creator_id = PlanInfoList[j].creator_id,
        //                        creator_gid = PlanInfoList[j].creator_gid,
        //                        creator_gnm = PlanInfoList[j].creator_gnm,
        //                        _category = PlanInfoList[j].category,
        //                        definitionId = BpmProcessModel.id,
        //                        Plan_Id = PlanInfoList[j].Plan_Id,
        //                        Plan_Name = PlanInfoList[j].Plan_Name,
        //                        TaskName = PlanInfoList[j].Task_Name,
        //                        DepartmentId = PlanInfoList[j].DepartmentId,
        //                        DepartmentName = PlanInfoList[j].DepartmentName,
        //                        DistributedId = PlanInfoList[j].DistributedId,
        //                        DistributedName = PlanInfoList[j].DistributedName,
        //                        StartDate = PlanInfoList[j].Start_Time,
        //                        EndDate = PlanInfoList[j].End_Time,
        //                        TaskDescription = PlanInfoList[j].TaskDescription,
        //                        PlanCycleId = PlanInfoList[j].PlanCycleId,
        //                        IsNormalPlan = PlanInfoList[j].IsNormalPlan,
        //                        IsFeedBack = PlanInfoList[j].IsFeedBack,
        //                        Plan_Type_Id = PlanInfoList[j].Plan_Type_Id,
        //                        Range_Name = PlanInfoList[j].Range_Name,
        //                        Geometry = PlanInfoList[j].Geometry,
        //                        MoveType = PlanInfoList[j].MoveType,
        //                        Plan_TemplateType_Id = PlanInfoList[j].Plan_TemplateType_Id,
        //                        PlanTtemplate_Id = PlanInfoList[j].PlanTtemplate_Id,
        //                        GroupName = PlanInfoList[j].GroupName,
        //                        Bpm_Id = PlanInfoList[j].Bpm_Id,
        //                        Bpm_Name = PlanInfoList[j].Bpm_Name,
        //                        GroupCname = PlanInfoList[j].GroupCname,
        //                        Equipment_Info_Code = PlanInfoList[j].Equipment_Info_Code,
        //                        Equipment_Info_Name = PlanInfoList[j].Equipment_Info_Name,
        //                        Address = PlanInfoList[j].Address,
        //                        Caliber = PlanInfoList[j].Caliber,
        //                        Lon = PlanInfoList[j].Lon,
        //                        Lat = PlanInfoList[j].Lat,
        //                        _next_assignee = PlanInfoList[j].DistributedId,
        //                    });

        //                    #region
        //                    //请求头
        //                    webClient.Headers.Add("Content-Type", "application/json");
        //                    //发送数据
        //                    string responseData = await webClient.UploadStringTaskAsync(new Uri(BPMUrl + "/bpm/customize-api/" + BpmProcessModel.key + "/create-order"), "POST", data);
        //                    BpmResult BpmResultModel = JsonConvert.DeserializeObject<BpmResult>(responseData);
        //                    if (BpmResultModel.Code == 0)
        //                    {
        //                        conn.Execute(string.Format(@"UPDATE Ins_Plan_Task SET ISSUCCESS = 1 WHERE PLAN_TASK_ID = '{0}'", PlanInfoList[j].Plan_Task_Id));
        //                    }
        //                    #endregion
        //                }
        //            }
        //            conn.Execute(string.Format(@"UPDATE Ins_Plan SET ASSIGN_STATE = 3 WHERE PLAN_ID = '{0}'", PlanId));
        //            LogHelper.Info("完成调用,无失败");
        //        }
        //        #endregion
        //    }
        //    catch (Exception e)
        //    {
        //        using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
        //        {
        //            conn.Execute(string.Format(@"UPDATE Ins_Plan SET ASSIGN_STATE = 2 WHERE PLAN_ID = '{0}'", PlanId));
        //            LogHelper.Info("异常结束调用,错误信息:" + e.Message);
        //        }
        //    }
        //}

        public MessageEntity IsExistPlan(string planTypeId)
        {
            string errorMsg = "";

            string query = $@" SELECT * from Ins_Plan lp where lp.plan_type_id= '{planTypeId}'";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Plan> result = conn.Query<Ins_Plan>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }
    }
}
