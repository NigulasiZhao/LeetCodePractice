using Dapper;
using GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.OracleDAL.GisConfigSetting
{
    public class GIS_LayerTypeDAL : IGIS_LayerTypeDAL
    {
        /// <summary>
        /// 添加图层类别
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(GIS_LayerType model)
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

        /// <summary>
        /// 删除图层类别
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(GIS_LayerType model)
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
                    if (model.ISSystem == 1)
                    {
                        return MessageEntityTool.GetMessage(1, null, true, "系统字段不允许删除");
                    }
                    else
                    {
                        rows = conn.Execute(excSql, model);
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        /// <summary>
        /// 获取图层类别并分页
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = "select * from GIS_LayerType t " + sqlCondition;
            DapperExtentions.EntityForSqlToPager<GIS_LayerType>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }
        public GIS_LayerType GetInfo(string ID)
        {
            List<GIS_LayerType> _ListField = new List<GIS_LayerType>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<GIS_LayerType>("select * from GIS_LayerType t where ID='" + ID + "'").ToList();
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
        /// 修改图层类别
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(GIS_LayerType model)
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
                    if(model.ISSystem == 1)
                    {
                        rows = conn.Execute("update GIS_LAYERTYPE t  set t.TypeCName=@TypeCName   where t.ID=@ID ", new { TypeCName = model.TypeCName, ID =model.ID });
                    }
                    else
                    {
                        rows = conn.Execute(excSql, model);
                    }
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }

        public MessageEntity GetLayerTypeList(string sqlCondition)
        {
            try
            {
                List<GIS_LayerType> _ListField = new List<GIS_LayerType>();
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    _ListField = conn.Query<GIS_LayerType>("select * from GIS_LayerType t "+sqlCondition).ToList();
                }
                Dictionary<string, object> returnValue = new Dictionary<string, object>();
                _ListField.ForEach(row =>
                {
                  
                        returnValue.Add(row.TypeName, row.TypeCode);
                    
                });
                return MessageEntityTool.GetMessage(1, returnValue, true, "完成");
            }
            catch (Exception ex)
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, ex.Message);
            }
        }
    }
}
