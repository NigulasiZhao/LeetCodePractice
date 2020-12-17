using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.IDAL.Equipments;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Equipments
{
      public class EquipmentPorpertyMappingDAL : IEquipmentPorpertyMappingDAL
    {
        public MessageEntity Add(EquipmentPorpertyMapping model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity Delete(EquipmentPorpertyMapping model)
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

        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"select epm.id, e.equipmentid,e.ename,ep.equipmentporpertyid,ep.epname,ep.inputtype,ep.isEdit,epm.nullable,epv.selectionvalue,epv.vieworder,epv.equipmentid as epvequipmentid
                          from EQUIPMENTPORPERTYMAPPING epm
                            left join EQUIPMENT e on epm.equipmentid = e.equipmentid
                            left join equipmentporperty ep on ep.equipmentporpertyid = epm.equipmentporpertyid
                            left join equipmentporpertyvalue epv on epv.equipmentporpertyid = ep.equipmentporpertyid and(epv.equipmentid = e.equipmentid or epv.equipmentid = 0)
                            " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<EquipmentPorpertyMappingModel>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
        public List<EquipmentPorpertyMappingModel> GetEquipmentPorpertyMapping( string eName, string ePname)
        {
            string sqlwhere = "where 1=1 ";
            if (!string.IsNullOrEmpty(eName))
            {
                sqlwhere += $" and e.ename='{eName}'";
            }
            if (!string.IsNullOrEmpty(ePname))
            {
                sqlwhere += $" and ep.epname='{ePname}'";
            }
            sqlwhere += " order by e.equipmentid asc";
            string sql = @" select  e.equipmentid,e.ename,ep.equipmentporpertyid,ep.epname,ep.inputtype,epm.nullable,ep.isEdit
                          from EQUIPMENTPORPERTYMAPPING epm
                            left join EQUIPMENT e on epm.equipmentid = e.equipmentid
                            left join equipmentporperty ep on ep.equipmentporpertyid = epm.equipmentporpertyid " + sqlwhere;
            string errorMsg = "";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EquipmentPorpertyMappingModel> list = conn.Query<EquipmentPorpertyMappingModel>(sql).ToList();

                    return list;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }
        /// <summary>
        /// 根据属性id和属性值获取属性下拉框内容
        /// </summary>
        /// <param name="equipmentPorpertyId"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public MessageEntity GetValueInfo(string equipmentPorpertyId, int equipmentId, string id,int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(equipmentId.ToString()))
            {
                strWhere += $" and equipmentId ={equipmentId}";
            }
            if (!string.IsNullOrEmpty(equipmentPorpertyId.ToString()))
            {
                strWhere += $" and equipmentPorpertyId= '{equipmentPorpertyId}' ";
            }
            if (isAdd == 0)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    strWhere += $" and ID not in({id}) ";
                }
            }
            string query = $@" select equipmentId from EquipmentPorpertyMapping where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<EquipmentPorpertyMapping> result = conn.Query<EquipmentPorpertyMapping>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        public MessageEntity Update(EquipmentPorpertyMapping model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeUpdateSql(model);
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
        public List<EquipmentPorpertyMappingList> GetEquipment()
        {
            string sql = @"select e.equipmentid,e.ename from EQUIPMENT e  ";
            string errorMsg = "";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EquipmentPorpertyMappingList> list = conn.Query<EquipmentPorpertyMappingList>(sql).ToList();

                    return list;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }
        public List<EquipmentPorpertyList> GetEquipmentPorperty()
        {
            string sql = @"select e.equipmentid,e.ename,ep.equipmentporpertyid,ep.epname from EQUIPMENTPORPERTYMAPPING epm
                            left join EQUIPMENT e on epm.equipmentid = e.equipmentid
                            left join equipmentporperty ep on ep.equipmentporpertyid = epm.equipmentporpertyid where ep.inputtype='s' ";
            string errorMsg = "";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EquipmentPorpertyList> list = conn.Query<EquipmentPorpertyList>(sql).ToList();

                    return list;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }

        public List<EquipmentPorpertyValue> GetEquipmentPorpertyValue()
        {
            string sql = @"  select e.equipmentid, ep.equipmentporpertyid, epv.selectionvalue, epv.vieworder
                                  from EQUIPMENTPORPERTYMAPPING epm
                                    left join EQUIPMENT e on epm.equipmentid = e.equipmentid
                                    left join equipmentporperty ep on ep.equipmentporpertyid = epm.equipmentporpertyid
                                    left join equipmentporpertyvalue epv on epv.equipmentporpertyid = ep.equipmentporpertyid and(epv.equipmentid = e.equipmentid or epv.equipmentid = 0) ";
            string errorMsg = "";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    List<EquipmentPorpertyValue> list = conn.Query<EquipmentPorpertyValue>(sql).ToList();

                    return list;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
        }

       
    }
}
