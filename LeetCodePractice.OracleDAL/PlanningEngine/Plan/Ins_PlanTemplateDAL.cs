using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.Plan;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.Plan
{
    public class Ins_PlanTemplateDAL : IIns_PlanTemplateDAL
    {
        /// <summary>
        /// 添加计划模板
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Ins_Plantemplate model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {

                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model, out OracleDynamicParameters parameters);
                List<Ins_Plan_EquipmentForm> featureGroup = model.EquipmentFormGroup;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(insertSql, parameters, transaction);
                    featureGroup.ForEach(row =>
                    {
                        row.Planttemplate_id = model.Planttemplate_id;
                        var rows1 = conn.Execute(DapperExtentions.MakeInsertSql(row, out OracleDynamicParameters parameters1), parameters1, transaction);
                    });
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
        /// 删除计划模板
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Ins_Plantemplate model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeDeleteSql(model);
                List<Ins_Plan_EquipmentForm> featureGroup = model.EquipmentFormGroup;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    featureGroup.ForEach(row =>
                    {
                        conn.Execute(DapperExtentions.MakeDeleteSql(row).ToString(), row, transaction);
                    });
                    rows = conn.Execute(insertSql, model, transaction);
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity Get(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @" select pt. plan_templatetypeid,
                                   t.templatetype_code,
                                   t.templatetype_name,
                                   pt.planttemplate_id,
                                   pt.planttemplate_name,p.layernames,p.tablenames,
                                   PT.CREATE_PERSON_ID,
                                   pt.create_time,
                                   pt.create_person_name,
                                   pt.update_time,
                                   pt.update_person_name
                              from INS_PLANTEMPLATE pt
                              left join ins_plan_templatetype t
                                on pt.plan_templatetypeid = t.plan_templatetype_id
                            left join 
                            (SELECT e.planttemplate_id,
                                listagg (e.layername, ',') WITHIN GROUP (ORDER BY e.planttemplate_id) layernames,
                                listagg (f.tablename, ',') WITHIN GROUP (ORDER BY e.planttemplate_id) tablenames
                            FROM ins_plan_equipmentform e  left join ins_customize_formlist f on f.tableid=e.tableid
                            group by e.planttemplate_id) p on p.planttemplate_id=pt.planttemplate_id" + sqlCondition;
            DapperExtentions.EntityForSqlToPager<Ins_PlantemplateList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public Ins_Plantemplate GetInfo(string ID)
        {
            List<Ins_Plantemplate> _ListField = new List<Ins_Plantemplate>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Ins_Plantemplate>("select * from Ins_Plantemplate t where Planttemplate_id='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField[0].EquipmentFormGroup = conn.Query<Ins_Plan_EquipmentForm>("select * from Ins_Plan_EquipmentForm t where Planttemplate_id='" + _ListField[0].Planttemplate_id.ToString() + "'").ToList();
                }
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得计划模板
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {

            string sql = @" select t. plan_templatetype_id,
                                   t.templatetype_code,
                                   t.templatetype_name,
                                   pt.planttemplate_id,
                                   pt.planttemplate_name,p.layernames,p.tablenames,
                                   pt.create_time,
                                   pt.create_person_name,pt.create_person_id,
                                   pt.update_time,
                                   pt.update_person_name,pt.update_person_id
                              from INS_PLANTEMPLATE pt
                              left join ins_plan_templatetype t
                                on pt.plan_templatetypeid = t.plan_templatetype_id
                            left join 
                            (SELECT e.planttemplate_id,
                                listagg (e.layername, ',') WITHIN GROUP (ORDER BY e.planttemplate_id) layernames,
                                listagg (f.tablename, ',') WITHIN GROUP (ORDER BY e.planttemplate_id) tablenames
                            FROM ins_plan_equipmentform e  left join ins_customize_formlist f on f.tableid=e.tableid
                            group by e.planttemplate_id) p on p.planttemplate_id=pt.planttemplate_id" + sqlCondition;
            List<Ins_PlantemplateList> ResultList = DapperExtentions.EntityForSqlToPager<Ins_PlantemplateList>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            //取得当前对应的数据列表信息
            foreach (Ins_PlantemplateList row in ResultList)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    row.EquipmentFormGroup = conn.Query<Ins_Plan_EquipmentFormList>("select t.*,c.tablecode,c.tablename from Ins_Plan_EquipmentForm t  left join INS_CUSTOMIZE_FORMLIST c on t.tableid=c.tableid where t.Planttemplate_id='" + row.Planttemplate_id.ToString() + "'").ToList();
                }
            }
            return result;
        }

        public MessageEntity IsExistPlanTemplate(Ins_Plantemplate plantemplate, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(plantemplate.PlantTemplate_name))
            {
                strWhere += $" and PlantTemplate_name ='{plantemplate.PlantTemplate_name}' ";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and Planttemplate_id <>'{plantemplate.Planttemplate_id}' and Plan_templatetypeid ='{plantemplate.Plan_templatetypeid}'";
            }
            string query = $@" select PlantTemplate_name from Ins_Plantemplate where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Plantemplate> result = conn.Query<Ins_Plantemplate>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity Update(Ins_Plantemplate model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model, out OracleDynamicParameters parameters);
                List<Ins_Plan_EquipmentForm> featureGroup = model.EquipmentFormGroup;

                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    //删除已经删除的配置项
                    List<string> FieldIDS = model.EquipmentFormGroup.Select(Row => Row.Equipment_Form_Id).ToList();
                    List<string> FieldIDSnew = new List<string>(); ;
                    FieldIDS.ForEach(row =>
                    {
                        FieldIDSnew.Add(row);
                    });
                    FieldIDS.ForEach(row =>
                    {
                        if (row == null)
                        {
                            FieldIDSnew.Remove(row);
                        }
                    });
                    string Ids = "'" + string.Join("','", FieldIDSnew.ToArray()) + "'";
                    if (FieldIDS.Count <= 0)
                    {
                        Ids = "'" + "0" + "'";

                    }
                    conn.Execute("delete Ins_Plan_EquipmentForm where Planttemplate_id='" + model.Planttemplate_id + "' and EQUIPMENT_FORM_ID not in(" + Ids + ")");
                    //执行修改
                    featureGroup.ForEach(row =>
                    {
                        row.Planttemplate_id = model.Planttemplate_id;
                        if (row.Equipment_Form_Id != "" && row.Equipment_Form_Id != null && row.Equipment_Form_Id != "0")
                        {
                            conn.Execute(DapperExtentions.MakeUpdateSql(row, out OracleDynamicParameters parameters1).ToString(), parameters1, transaction);
                        }
                        else
                        {
                            conn.Execute(DapperExtentions.MakeInsertSql(row, out OracleDynamicParameters parameters1).ToString(), parameters1, transaction);
                        }
                    });
                    rows = conn.Execute(insertSql, parameters, transaction);
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
    }
}
