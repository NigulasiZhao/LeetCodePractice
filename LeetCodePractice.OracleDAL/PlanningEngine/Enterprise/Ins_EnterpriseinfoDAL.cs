/**********************************************
** Class_Kind:      DotNet Entity Class
** Creater:         Entity Class Generator
** Create Date:     2020/11/25 11:52:38
** Description:     Entity Class
** Version:         Entity Class Generator 1.0.0
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Enterprise.Dto;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Enterprise
{
    /// <summary>
    ///企业信息DAL层
    /// </summary>
    public class Ins_EnterpriseinfoDAL : IIns_EnterpriseinfoDAL
    {
        /// <summary>
        /// 添加企业信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Ins_Enterpriseinfo model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM INS_ENTERPRISEINFO WHERE Enterprisecode = '{0}'", model.Enterprisecode);
                int CodeCount = conn.ExecuteScalar<int>(GetDataSql);
                if (CodeCount > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "企业编码重复！");
                }
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = $@"DECLARE  
                                     GeometryStr clob;
                                   BEGIN  
                                     GeometryStr := '{model.Mishape}';
                                     INSERT INTO INS_ENTERPRISEINFO(id, 
                                                          enterprisename, 
                                                          enterprisecode, 
                                                          enterprisetype, 
                                                          behalfperson, 
                                                          enterpriselevel, 
                                                          tel, 
                                                          fax, 
                                                          address, 
                                                          mishape, 
                                                          shape, 
                                                          other, 
                                                          remark, 
                                                          creatorid, 
                                                          creatorname, 
                                                          creatordepartmentid, 
                                                          creatordepartmentname, 
                                                          creatortime) VALUES('{Guid.NewGuid()}','{model.Enterprisename}','{model.Enterprisecode}'
                                  ,'{model.Enterprisetype}','{model.Behalfperson}','{model.Enterpriselevel}','{model.Tel}','{model.Fax}','{model.Address}',
                                  GeometryStr,(select sde.st_geometry(GeometryStr,4547)  from dual),'{model.Other}','{model.Remark}','{model.Creatorid}'
                                  ,'{model.Creatorname}','{model.Creatordepartmentid}','{model.Creatordepartmentname}',to_date('{model.Creatortime}','YYYY-mm-dd HH24:Mi:SS')); 
                                     COMMIT;  
                                   END;  ";
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql);
                    return MessageEntityTool.GetMessage(1);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        ///修改企业信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Ins_Enterpriseinfo model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                string GetDataSql = string.Format(@"SELECT Count(0) FROM INS_ENTERPRISEINFO WHERE Enterprisecode = '{0}' AND ID <> '{1}'", model.Enterprisecode, model.Id);
                int CodeCount = conn.ExecuteScalar<int>(GetDataSql);
                if (CodeCount > 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.OprationError, "", "企业编码重复！");
                }
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = $@"DECLARE  
                                     GeometryStr clob;
                                   BEGIN  
                                     GeometryStr := '{model.Mishape}';
                                     UPDATE INS_ENTERPRISEINFO SET enterprisename='{model.Enterprisename}', 
                                                          enterprisecode='{model.Enterprisecode}', 
                                                          enterprisetype='{model.Enterprisetype}', 
                                                          behalfperson='{model.Behalfperson}', 
                                                          enterpriselevel='{model.Enterpriselevel}', 
                                                          tel='{model.Tel}', 
                                                          fax='{model.Fax}', 
                                                          address='{model.Address}', 
                                                          mishape=GeometryStr, 
                                                          shape=(select sde.st_geometry(GeometryStr,4547)  from dual), 
                                                          other='{model.Other}', 
                                                          remark='{model.Remark}'
                                              WHERE ID= '{model.Id}';
                                     COMMIT;  
                                   END;  ";
                //DapperExtentions.MakeInsertSql(model);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql);
                    return MessageEntityTool.GetMessage(1);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 删除企业信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Ins_Enterpriseinfo model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeDeleteSql(model);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(excSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 根据ID获取企业信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ins_Enterpriseinfo GetInfo(string ID)
        {
            List<Ins_Enterpriseinfo> _ListField = new List<Ins_Enterpriseinfo>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Ins_Enterpriseinfo>("select Address,Behalfperson,Creatordepartmentid,Creatordepartmentname,Creatorid,Creatorname,Creatortime,Enterprisecode,Enterpriselevel,Enterprisename,Enterprisetype,Fax,Id,Mishape,Other,Remark,Tel from Ins_Enterpriseinfo t where Id='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得企业信息列表
        /// </summary>
        /// <param name="parInfo">参数信息</param>/// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>/// <param name="num">当前页</param>/// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition, string SearchConditions)
        {
            string SearchConditionsSql = "";
            if (!string.IsNullOrEmpty(SearchConditions))
            {
                SearchConditionsSql = $" and ( Enterprisename like '%{SearchConditions}%' or Tel like '%{SearchConditions}%' or Behalfperson like '%{SearchConditions}%')";
            }
            string sql = @"   select Address,Behalfperson,Creatordepartmentid,Creatordepartmentname,Creatorid,Creatorname,Creatortime,Enterprisecode,Enterpriselevel,Enterprisename,Enterprisetype,Fax,Id,Mishape,Other,Remark,Tel from Ins_Enterpriseinfo ipc 
" + sqlCondition + SearchConditionsSql;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Enterpriseinfo>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }


        public MessageEntity GetStatisticalData(DateTime? startDate = null, DateTime? endDate = null)
        {
            string CoditionSql = "";
            if (startDate != null)
            {
                CoditionSql = " and CreatorTime >= to_date('" + startDate.Value + "','YYYY-mm-dd HH24:Mi:SS')";
            }
            if (endDate != null)
            {
                CoditionSql = CoditionSql + " and CreatorTime <= to_date('" + DateTime.Parse(endDate.Value.ToString()).AddDays(1).AddSeconds(-1).ToString() + "','YYYY-mm-dd HH24:Mi:SS')";
            }
            string GetEnterpriseCountSql = @"SELECT  COUNT(0) FROM Ins_Enterpriseinfo where 1=1" + CoditionSql;

            string GetTenderCountSql = @"SELECT  COUNT(0) FROM Ins_Enterprisetender where 1=1" + CoditionSql;

            string GetEnterpriseTypeSql = $@"SELECT  Enterprisetype,COUNT(0) as EnterpriseTypeCount FROM Ins_Enterpriseinfo where 1=1 {CoditionSql} group by Enterprisetype";

            string GetEnterpriseScoreSql = $@"SELECT IE.ID as Enterpriseid,IE.EnterpriseName,IE.MISHAPE,A.Checkscore FROM 
                                                    (SELECT  Enterpriseid,avg(Checkscore) as Checkscore FROM Ins_Enterprisebusiness where 1=1 {CoditionSql} group by Enterpriseid) A 
                                                    INNER JOIN INS_ENTERPRISEINFO IE ON A.Enterpriseid= IE.ID ";

            string GetEnterpriseDescRankSql = $@"SELECT A.Enterpriseid, B.Enterprisename, A.AvgScore,row_number() over(order by A.AvgScore desc) as Ranking
                                                 FROM (SELECT avg(Checkscore) AS AvgScore, Enterpriseid
                                                         FROM Ins_Enterprisebusiness
                                                        group by Enterpriseid) A
                                                INNER JOIN Ins_Enterpriseinfo B
                                                   ON A.Enterpriseid = B.id
                                                where rownum <= 5 {CoditionSql}
                                                ORDER BY A.AvgScore desc";

            string GetEnterpriseAscRankSql = $@"SELECT A.Enterpriseid, B.Enterprisename, A.AvgScore,row_number() over(order by A.AvgScore desc) as Ranking
                                                 FROM (SELECT avg(Checkscore) AS AvgScore, Enterpriseid
                                                         FROM Ins_Enterprisebusiness
                                                        group by Enterpriseid) A
                                                INNER JOIN Ins_Enterpriseinfo B
                                                   ON A.Enterpriseid = B.id
                                                where rownum <= 5 {CoditionSql}
                                                ORDER BY A.AvgScore asc";
            StatisticalDataOutput output = new StatisticalDataOutput();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                List<int> EnterpriseCountList = conn.Query<int>(GetEnterpriseCountSql).ToList();
                List<int> TenderCountList = conn.Query<int>(GetTenderCountSql).ToList();
                List<EnterpriseTypeStatistical> EnterpriseTypeList = conn.Query<EnterpriseTypeStatistical>(GetEnterpriseTypeSql).ToList();
                List<EnterpriseAvgScoreStatistical> EnterpriseScoreList = conn.Query<EnterpriseAvgScoreStatistical>(GetEnterpriseScoreSql).ToList();
                List<EnterpriseRankStatistical> DescRankList = conn.Query<EnterpriseRankStatistical>(GetEnterpriseDescRankSql).ToList();
                List<EnterpriseRankStatistical> AscRankList = conn.Query<EnterpriseRankStatistical>(GetEnterpriseAscRankSql).ToList();

                output.EnterpriseCount = EnterpriseCountList.Count == 0 ? 0 : EnterpriseCountList[0];
                output.TenderCount = TenderCountList.Count == 0 ? 0 : TenderCountList[0];
                output.EnterpriseTypeList = EnterpriseTypeList;
                output.DescRankList = DescRankList;
                output.AscRankList = AscRankList;

                if (EnterpriseScoreList.Count > 0)
                {
                    output.EnterpriseScoreList = new List<EnterpriseScoreStatistical>();
                    List<EnterpriseAvgScoreStatistical> list = EnterpriseScoreList.Where(e => e.Checkscore < 60).ToList();
                    EnterpriseScoreStatistical model = new EnterpriseScoreStatistical();
                    model.EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore < 60);
                    model.EnterpriseScoreType = "60分以下";
                    model.DataList.AddRange(list);
                    output.EnterpriseScoreList.Add(model);

                    List<EnterpriseAvgScoreStatistical> list1 = EnterpriseScoreList.Where(e => e.Checkscore >= 60 && e.Checkscore < 70).ToList();
                    EnterpriseScoreStatistical model1 = new EnterpriseScoreStatistical();
                    model1.EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 60 && e.Checkscore < 70);
                    model1.EnterpriseScoreType = "60到70分";
                    model1.DataList.AddRange(list1);
                    output.EnterpriseScoreList.Add(model1);

                    List<EnterpriseAvgScoreStatistical> list2 = EnterpriseScoreList.Where(e => e.Checkscore >= 70 && e.Checkscore < 80).ToList();
                    EnterpriseScoreStatistical model2 = new EnterpriseScoreStatistical();
                    model2.EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 70 && e.Checkscore < 80);
                    model2.EnterpriseScoreType = "70到80分";
                    model2.DataList.AddRange(list2);
                    output.EnterpriseScoreList.Add(model2);

                    List<EnterpriseAvgScoreStatistical> list3 = EnterpriseScoreList.Where(e => e.Checkscore >= 80 && e.Checkscore < 90).ToList();
                    EnterpriseScoreStatistical model3 = new EnterpriseScoreStatistical();
                    model3.EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 80 && e.Checkscore < 90);
                    model3.EnterpriseScoreType = "80到90分";
                    model3.DataList.AddRange(list3);
                    output.EnterpriseScoreList.Add(model3);

                    List<EnterpriseAvgScoreStatistical> list4 = EnterpriseScoreList.Where(e => e.Checkscore >= 90 && e.Checkscore <= 100).ToList();
                    EnterpriseScoreStatistical model4 = new EnterpriseScoreStatistical();
                    model4.EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 90 && e.Checkscore <= 100);
                    model4.EnterpriseScoreType = "90到100分";
                    model4.DataList.AddRange(list4);
                    output.EnterpriseScoreList.Add(model4);

                    //output.EnterpriseScoreList.Add(new EnterpriseScoreStatistical
                    //{
                    //    EnterpriseScoreType = "60分以下",
                    //    EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore < 60),
                    //});
                    //output.EnterpriseScoreList.Add(new EnterpriseScoreStatistical
                    //{
                    //    EnterpriseScoreType = "60到70分",
                    //    EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 60 && e.Checkscore < 70)
                    //});
                    //output.EnterpriseScoreList.Add(new EnterpriseScoreStatistical
                    //{
                    //    EnterpriseScoreType = "70到80分",
                    //    EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 70 && e.Checkscore < 80)
                    //});
                    //output.EnterpriseScoreList.Add(new EnterpriseScoreStatistical
                    //{
                    //    EnterpriseScoreType = "80到90分",
                    //    EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 80 && e.Checkscore < 90)
                    //});
                    //output.EnterpriseScoreList.Add(new EnterpriseScoreStatistical
                    //{
                    //    EnterpriseScoreType = "90到100分",
                    //    EnterpriseScoreCount = EnterpriseScoreList.Count(e => e.Checkscore >= 90 && e.Checkscore <= 100)
                    //});
                }
                else
                {
                    output.EnterpriseScoreList = new List<EnterpriseScoreStatistical>();
                }
            }
            return MessageEntityTool.GetMessage(1, output);
        }
    }
}