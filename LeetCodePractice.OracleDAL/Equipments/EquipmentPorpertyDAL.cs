using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.IDAL.Equipments;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.Equipments;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Equipments
{
   public class EquipmentPorpertyDAL: IEquipmentPorpertyDAL
    {
        /// <summary>
        /// 添加设备属性配置
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(EquipmentPorperty model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {

                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model, out OracleDynamicParameters parameters);
                List<EquipmentPorpertyValue> equipPValueGroup = model.EquipmentPorpertyValueGroup;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    rows = conn.Execute(insertSql, parameters, transaction);
                    equipPValueGroup.ForEach(row =>
                    {
                        row.EquipmentPorpertyId = model.EquipmentPorpertyId;
                        var rows1 = conn.Execute(DapperExtentions.MakeInsertSql(row), row, transaction);
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
        /// 删除设备属性配置信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(EquipmentPorperty model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeDeleteSql(model);
                List<EquipmentPorpertyValue> equipPValueGroup = model.EquipmentPorpertyValueGroup;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    equipPValueGroup.ForEach(row =>
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

        public EquipmentPorperty GetInfo(string ID)
        {
            List<EquipmentPorperty> _ListField = new List<EquipmentPorperty>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<EquipmentPorperty>("select * from EquipmentPorperty t where EquipmentPorpertyId='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField[0].EquipmentPorpertyValueGroup = conn.Query<EquipmentPorpertyValue>("select * from EquipmentPorpertyValue t where EquipmentPorpertyId='" + _ListField[0].EquipmentPorpertyId.ToString() + "'").ToList();
                }
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得设备属性配置信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {

            string sql = @"   select ep.equipmentporpertyid,ep.epname,ep.inputtype from  equipmentporperty ep 
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<EquipmentPorperty>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            foreach (EquipmentPorperty row in ResultList)
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    row.EquipmentPorpertyValueGroup = conn.Query<EquipmentPorpertyValue>("select * from EquipmentPorpertyValue t where EquipmentPorpertyId='" + row.EquipmentPorpertyId.ToString() + "'").ToList();
                }
            }
            return result;
        }


        /// <summary>
        ///修改设备属性配置
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(EquipmentPorperty model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model, out OracleDynamicParameters parameters);
                List<EquipmentPorpertyValue> equipPValueGroup = model.EquipmentPorpertyValueGroup;
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    //删除已经删除的配置项
                    List<string> FieldIDS = model.EquipmentPorpertyValueGroup.Select(Row => Row.ID).ToList();
                    string ids = "'" + string.Join("','", FieldIDS.ToArray()) + "'";
                    if (FieldIDS.Count <= 0)
                    {
                        ids = "'" + "0" + "'";

                    }
                    conn.Execute("delete EquipmentPorpertyValue where EquipmentPorpertyId='" + model.EquipmentPorpertyId + "' and ID not in(" + ids + ")");
                   // 执行修改
                    equipPValueGroup.ForEach(row =>
                    {
                        if (row.ID != "" && row.ID != null && row.ID != "0")
                        {
                            conn.Execute(DapperExtentions.MakeUpdateSql(row).ToString(), row, transaction);
                        }
                        else
                        {
                            row.EquipmentPorpertyId = model.EquipmentPorpertyId;
                            conn.Execute(DapperExtentions.MakeInsertSql(row), row, transaction);

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
