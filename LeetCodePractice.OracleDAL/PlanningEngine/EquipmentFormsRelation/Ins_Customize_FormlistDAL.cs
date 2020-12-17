using Dapper;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.EquipmentFormsRelation;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.EquipmentFormsRelation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.EquipmentFormsRelation
{
    public class Ins_Customize_FormlistDAL : IIns_Customize_FormlistDAL
    {

        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select formlist_id,tableid,tablename,tablecode from INS_customize_formlist 
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Customize_Formlist>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
            throw new NotImplementedException();
        }
    }
}
