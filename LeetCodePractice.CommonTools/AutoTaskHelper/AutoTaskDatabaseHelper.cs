using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.Model.AttributePack;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using static GISWaterSupplyAndSewageServer.CommonTools.AutoTaskHelper.AutoTaskConnectionFactory;
using ColumnAttribute = GISWaterSupplyAndSewageServer.Model.AttributePack.ColumnAttribute;

namespace GISWaterSupplyAndSewageServer.CommonTools.AutoTaskHelper
{
    public class AutoTaskCacheFactory
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private static AutoTaskCacheFactory _instance;

        /// <summary>
        /// 进程锁
        /// </summary>
        private static readonly object Lock = new object();

        /// <summary>
        /// 存放类型的属性
        /// </summary>
        private readonly Dictionary<string, PropertyInfo[]> _properties;

        /// <summary>
        /// 存放属性的特性
        /// </summary>
        private readonly Dictionary<string, object[]> _customAttributes;

        /// <summary>
        /// 存放实体对象
        /// </summary>
        //private readonly Dictionary<string, object> _entity; 

        public AutoTaskCacheFactory()
        {
            _properties = new Dictionary<string, PropertyInfo[]>();
            _customAttributes = new Dictionary<string, object[]>();
            //_entity = new Dictionary<string, object>();
        }

        #region  设置集合操作

        /// <summary>
        /// 获取类型的属性
        /// </summary>
        public PropertyInfo[] GetProperties(string key)
        {
            return _properties[key];
        }

        /// <summary>
        /// 获取属性的特性
        /// </summary>
        public object[] GetCustomAttributes(string key)
        {
            return _customAttributes[key];
        }

        ///// <summary>
        ///// 获取实体
        ///// </summary>
        //public object GetEntity(string key)
        //{
        //    return _entity[key];
        //}

        public void SetProperties(string key, PropertyInfo[] propertyInfos)
        {
            _properties.Add(key, propertyInfos);
        }

        public void SetCustomAttributes(string key, object[] objects)
        {
            _customAttributes.Add(key, objects);
        }

        //public void SetEntity(string key, object entity)
        //{
        //    _entity.Add(key,entity);
        //}

        public bool IsPropertyExist(string key)
        {
            return _properties.ContainsKey(key);
        }

        public bool IsAttributeExist(string key)
        {
            return _customAttributes.ContainsKey(key);
        }

        //public bool IsEntityExist(string key)
        //{
        //    return _entity.ContainsKey(key);
        //}

        #endregion

        public static AutoTaskCacheFactory GetInstance()
        {
            #region  获取实例

            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        return _instance = new AutoTaskCacheFactory();
                    }
                }
            }
            return _instance;

            #endregion
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性集合</returns>
        public static PropertyInfo[] CacheProperties(Type type)
        {
            #region  设置属性

            try
            {
                PropertyInfo[] properties;
                var key = type.Namespace + "." + type.Name;

                if (AutoTaskCacheFactory.GetInstance().IsPropertyExist(key))
                {
                    properties = AutoTaskCacheFactory.GetInstance().GetProperties(key);
                }
                else
                {
                    properties = type.GetProperties();
                    AutoTaskCacheFactory.GetInstance().SetProperties(key, properties);
                }

                return properties;
            }
            catch (Exception e)
            {
                return null;
            }

            #endregion
        }



        /// <summary>
        /// 设置CustomAttributes属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="property">属性</param>
        /// <returns>CustomAttributes</returns>
        public static object[] CacheCustomAttributes(Type type, PropertyInfo property)
        {
            #region  缓存设置CustomAttributes属性

            var key = type.Namespace + "." + type.Name + "." + property.Name;
            lock (key)
            {
                object[] objects = null;
                if (AutoTaskCacheFactory.GetInstance().IsAttributeExist(key))
                {
                    objects = AutoTaskCacheFactory.GetInstance().GetCustomAttributes(key);
                }

                if (objects == null)
                {
                    objects = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                    AutoTaskCacheFactory.GetInstance().SetCustomAttributes(key, objects);
                }

                return objects;
            }
            #endregion
        }

    }

    public class AutoTaskConnectionFactory
    {
        private static IConfiguration _configuration;
        public enum DBConnNames
        {
            GISWaterSupplyAndSewageServer,
            PipeInspection_Smart_Water,
            PipeInspectionBase_Gis_DB,
            PipeInspectionBase_Gis_OutSide,
            PipeInspectionSmartNet,
            GISDB,
            ORCL,
            SDE,
            KM,
            CSSDE
        }
        public static IDbConnection GetDBConn(DBConnNames dbConnName)
        {
            return CreateConnection(Appsettings.app(new string[] { dbConnName.ToString() }));
        }

        private static IDbConnection CreateConnection(string connString)
        {
            IDbConnection conn = null;
            string DBName = Appsettings.app(new string[] { "DBName" });
            switch (DBName)
            {
                case "SQLServer":
                    conn = new SqlConnection(connString);
                    break;
                case "OracleDAL":
                    conn = new OracleConnection(connString);
                    break;
                default:
                    conn = new SqlConnection(connString);
                    break;
            }

            conn.Open();
            return conn;
        }

        public static IDisposable GetDBConn(object dBConnName)
        {
            throw new NotImplementedException();
        }
    }

    public class AutoTaskDapperExtentions
    {
        //ZL依赖注入
        public static IConfiguration _configuration { get; set; }
        private static readonly string DBName = Appsettings.app(new string[] { "DBName" });
        private static OracleParameter[] _parameters;

        /// <summary>
        /// 获取自动新增数据SQL
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>sql语句</returns>
        public static string MakeInsertSql(Object entity)
        {
            #region  拼接SQL
            if (entity == null)
            {
                return string.Empty;

            }
            object obj;
            var type = entity.GetType();
            var properties = AutoTaskCacheFactory.CacheProperties(type);
            var strSql = new StringBuilder();
            var strColumn = new StringBuilder();
            var strValue = new StringBuilder();
            var tableContents = type.GetCustomAttributes(typeof(TableAttribute), true);
            var tableAttribute = tableContents[0] as TableAttribute;

            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                //获取验证特性
                var columnAttributes = AutoTaskCacheFactory.CacheCustomAttributes(type, property);
                var value = property.GetValue(entity, null);

                if (columnAttributes.Length > 0)
                { //获取定义的第一个验证特性
                    var columnAttribute = columnAttributes[0] as GISWaterSupplyAndSewageServer.Model.AttributePack.ColumnAttribute;
                    //if (columnAttribute == null)
                    //    continue;
                    //如果字段不需要执行插入操作，那么忽略
                    if (columnAttribute.FilterType == FilterType.IsNotInsert || columnAttribute.FilterType == FilterType.IsNotEdit)
                        continue;
                    if (DBName.Contains("Oracle"))
                    {
                        if (columnAttribute.PrimaryKeyType == PrimaryKeyType.Identity || columnAttribute.PrimaryKeyType == PrimaryKeyType.Sequence || columnAttribute.PrimaryKeyType == PrimaryKeyType.Guid)
                        {
                            //获取主键id
                            //string ID = SetParameter(property, columnAttribute, tableAttribute, property.Name, value, i, "", out obj, true);
                            string ID = Guid.NewGuid().ToString();
                            property.SetValue(entity, ID);
                        }
                    }
                    else
                    {

                        if (columnAttribute.PrimaryKeyType == PrimaryKeyType.Identity || columnAttribute.PrimaryKeyType == PrimaryKeyType.Sequence)
                            continue;
                    }
                }
                strColumn.Append(property.Name + ",");
                if (DBName.Contains("Oracle"))
                {
                    strValue.AppendFormat(":{0},", property.Name);
                }
                else
                {
                    strValue.AppendFormat("@{0},", property.Name);
                }
            }

            if (strColumn.ToString().EndsWith(","))
            {
                strColumn.Remove(strColumn.Length - 1, 1);
                strValue.Remove(strValue.Length - 1, 1);
            }
            if (strColumn.Length > 0 && strValue.Length > 0)
            {
                strSql.Append("INSERT INTO " + entity.GetType().Name + " (" + strColumn + ") values (" + strValue + ")");
            }
            if (DBName.Contains("Oracle"))
            {

            }
            return strSql.Length > 0 ? strSql.ToString() : string.Empty;

            #endregion
        }
        /// <summary>
        /// 获取自动新增数据SQL
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>sql语句</returns>
        public static string MakeInsertSql(Object entity, out OracleDynamicParameters parameters)
        {
            #region  拼接SQL
            //object obj;
            if (entity == null)
            {
                parameters = null;
                return string.Empty;


            }
            parameters = new OracleDynamicParameters();
            object obj;
            var type = entity.GetType();
            var properties = AutoTaskCacheFactory.CacheProperties(type);
            var strSql = new StringBuilder();
            var strColumn = new StringBuilder();
            var strValue = new StringBuilder();
            var tableContents = type.GetCustomAttributes(typeof(TableAttribute), true);
            var tableAttribute = tableContents[0] as TableAttribute;

            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                //获取验证特性
                var columnAttributes = AutoTaskCacheFactory.CacheCustomAttributes(type, property);
                var value = property.GetValue(entity, null);

                if (columnAttributes.Length > 0)
                { //获取定义的第一个验证特性
                    var columnAttribute = columnAttributes[0] as ColumnAttribute;
                    //if (columnAttribute == null)
                    //    continue;
                    //如果字段不需要执行插入操作，那么忽略
                    if (columnAttribute.FilterType == FilterType.IsNotInsert || columnAttribute.FilterType == FilterType.IsNotEdit)
                        continue;
                    if (DBName.Contains("Oracle"))
                    {
                        if (columnAttribute.PrimaryKeyType == PrimaryKeyType.Identity || columnAttribute.PrimaryKeyType == PrimaryKeyType.Sequence || columnAttribute.PrimaryKeyType == PrimaryKeyType.Guid)
                        {
                            //获取主键id
                            //string ID = SetParameter(property, columnAttribute, tableAttribute, property.Name, value, i, "", out obj, true);
                            string ID = Guid.NewGuid().ToString();
                            property.SetValue(entity, ID);
                            if (property.PropertyType.FullName == "System.String")
                            {
                                parameters.Add(property.Name, ID, OracleMappingType.NVarchar2, ParameterDirection.Input);
                            }
                        }
                        else if (columnAttribute.DataType == DataType.IsClob)
                        {
                            if (property.PropertyType.FullName == "System.String")
                            {
                                parameters.Add(property.Name, value, OracleMappingType.Clob, ParameterDirection.Input);
                            }
                        }
                        else if (columnAttribute.DataType == DataType.IsString)
                        {
                            if (property.PropertyType.FullName == "System.String")
                            {
                                parameters.Add(property.Name, value, OracleMappingType.NVarchar2, ParameterDirection.Input);
                            }
                        }
                        else if (columnAttribute.DataType == DataType.IsDateTime)
                        {
                            if (property.PropertyType.FullName == "System.DateTime")
                            {
                                parameters.Add(property.Name, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), OracleMappingType.Date, ParameterDirection.Input);
                            }
                            else
                            {
                                parameters.Add(property.Name, value, OracleMappingType.Date, ParameterDirection.Input);
                            }
                        }
                    }

                    else
                    {

                        if (columnAttribute.PrimaryKeyType == PrimaryKeyType.Identity || columnAttribute.PrimaryKeyType == PrimaryKeyType.Sequence)
                            continue;
                    }
                }
                else
                {
                    #region 輸出參數賦值
                    if (property.PropertyType.FullName == "System.Int32")
                    {
                        parameters.Add(property.Name, value, OracleMappingType.Int32, ParameterDirection.Input);
                    }
                    if (property.PropertyType.FullName == "System.String")
                    {
                        parameters.Add(property.Name, value, OracleMappingType.NVarchar2, ParameterDirection.Input);
                    }
                    if (property.PropertyType.FullName == "System.Clob")
                    {
                        parameters.Add(property.Name, value, OracleMappingType.Clob, ParameterDirection.Input);
                    }
                    if (property.PropertyType.FullName == "System.Decimal")
                    {
                        parameters.Add(property.Name, value, OracleMappingType.Decimal, ParameterDirection.Input);
                    }
                    if (property.PropertyType.FullName == "System.Single")
                    {
                        parameters.Add(property.Name, value, OracleMappingType.Decimal, ParameterDirection.Input);
                    }
                    if (property.PropertyType.FullName == "System.DateTime")
                    {
                        parameters.Add(property.Name, value, OracleMappingType.Date, ParameterDirection.Input);
                    }
                    #endregion
                }

                strColumn.Append(property.Name + ",");
                if (DBName.Contains("Oracle"))
                {
                    strValue.AppendFormat(":{0},", property.Name);
                }
                else
                {
                    strValue.AppendFormat("@{0},", property.Name);
                }
            }

            if (strColumn.ToString().EndsWith(","))
            {
                strColumn.Remove(strColumn.Length - 1, 1);
                strValue.Remove(strValue.Length - 1, 1);
            }
            if (strColumn.Length > 0 && strValue.Length > 0)
            {
                strSql.Append("INSERT INTO " + entity.GetType().Name + " (" + strColumn + ") values (" + strValue + ")");
            }
            if (DBName.Contains("Oracle"))
            {

            }
            return strSql.Length > 0 ? strSql.ToString() : string.Empty;

            #endregion
        }

        /// 获取自动根据主键修改数据SQL
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>sql语句</returns>
        public static string MakeUpdateSql(Object entity)
        {
            #region  拼接SQL
            if (entity == null)
                return string.Empty;

            var type = entity.GetType();
            var properties = AutoTaskCacheFactory.CacheProperties(type);
            var strSql = new StringBuilder();
            var strColumn = new StringBuilder();
            var strWhere = new StringBuilder("1=1");

            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                //获取验证特性
                var columnAttributes = AutoTaskCacheFactory.CacheCustomAttributes(type, property);

                if (columnAttributes.Length > 0)
                {
                    var validateAttribute = columnAttributes[0] as ColumnAttribute;

                    //如果字段不需要执行更新操作，那么忽略
                    if (validateAttribute.FilterType == FilterType.IsNotUpdate || validateAttribute.FilterType == FilterType.IsNotEdit)
                        continue;

                    if (validateAttribute.FilterType == FilterType.IsPrimaryKey)
                    {
                        if (DBName.Contains("Oracle"))
                        {
                            strWhere.AppendFormat(" and " + property.Name + "=" + ":{0}", property.Name);

                        }
                        else
                        {
                            strWhere.AppendFormat(" and " + property.Name + "=" + "@{0}", property.Name);

                        }
                    }
                    else
                    {
                        if (DBName.Contains("Oracle"))
                        {
                            strColumn.AppendFormat(property.Name + "=" + ":{0}", property.Name);
                        }
                        else
                        {
                            strColumn.AppendFormat(property.Name + "=" + "@{0}", property.Name);
                        }
                        if (i >= properties.Length - 1)
                            continue;
                        strColumn.Append(",");
                    }
                }
                else
                {
                    if (DBName.Contains("Oracle"))
                    {
                        strColumn.AppendFormat(property.Name + "=" + ":{0}", property.Name);

                    }
                    else
                    {
                        strColumn.AppendFormat(property.Name + "=" + "@{0}", property.Name);
                    }
                    if (i >= properties.Length - 1)
                        continue;
                    strColumn.Append(",");
                }
            }

            if (strColumn.ToString().EndsWith(","))
                strColumn.Remove(strColumn.Length - 1, 1);

            if (strColumn.Length > 0 && strWhere.Length > 0)
            {
                if (DBName.Contains("Oracle"))
                {
                    strSql.Append("UPDATE " + entity.GetType().Name + " SET " + strColumn + " WHERE " + strWhere);
                }
                else
                {
                    strSql.Append("UPDATE " + entity.GetType().Name + " SET " + strColumn + " WHERE " + strWhere + " ;");

                }
            }


            return strSql.Length > 0 ? strSql.ToString() : string.Empty;

            #endregion
        }

        /// 获取自动根据主键修改数据SQL
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>sql语句</returns>
        public static string MakeUpdateSql(Object entity, out OracleDynamicParameters parameters)
        {
            #region  拼接SQL
            if (entity == null)
            {
                parameters = null;
                return string.Empty;
            }
            parameters = new OracleDynamicParameters();

            var type = entity.GetType();
            var properties = AutoTaskCacheFactory.CacheProperties(type);
            var strSql = new StringBuilder();
            var strColumn = new StringBuilder();
            var strWhere = new StringBuilder("1=1");

            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                //获取验证特性
                var columnAttributes = AutoTaskCacheFactory.CacheCustomAttributes(type, property);
                var value = property.GetValue(entity, null);

                if (columnAttributes.Length > 0)
                {
                    var validateAttribute = columnAttributes[0] as ColumnAttribute;

                    //如果字段不需要执行更新操作，那么忽略
                    if (validateAttribute.FilterType == FilterType.IsNotUpdate || validateAttribute.FilterType == FilterType.IsNotEdit)
                        continue;

                    if (validateAttribute.FilterType == FilterType.IsPrimaryKey)
                    {
                        if (DBName.Contains("Oracle"))
                        {
                            strWhere.AppendFormat(" and " + property.Name + "=" + ":{0}", property.Name);

                        }
                        else
                        {
                            strWhere.AppendFormat(" and " + property.Name + "=" + "@{0}", property.Name);

                        }
                        if (property.PropertyType.FullName == "System.String")
                        {
                            parameters.Add(property.Name, value, OracleMappingType.NVarchar2, ParameterDirection.Input);
                        }
                    }
                    else
                    {
                        if (DBName.Contains("Oracle"))
                        {
                            strColumn.AppendFormat(property.Name + "=" + ":{0}", property.Name);
                        }
                        else
                        {
                            strColumn.AppendFormat(property.Name + "=" + "@{0}", property.Name);
                        }
                        if (validateAttribute.DataType == DataType.IsClob)
                        {
                            if (property.PropertyType.FullName == "System.String")
                            {
                                parameters.Add(property.Name, value, OracleMappingType.Clob, ParameterDirection.Input);
                            }
                        }
                        else if (validateAttribute.DataType == DataType.IsDateTime)
                        {
                            if (property.PropertyType.FullName == "System.DateTime")
                            {
                                parameters.Add(property.Name, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), OracleMappingType.Date, ParameterDirection.Input);
                            }
                            else
                            {
                                parameters.Add(property.Name, value, OracleMappingType.Date, ParameterDirection.Input);
                            }
                        }
                        if (i >= properties.Length - 1)
                            continue;
                        strColumn.Append(",");
                    }
                }
                else
                {
                    if (DBName.Contains("Oracle"))
                    {
                        strColumn.AppendFormat(property.Name + "=" + ":{0}", property.Name);

                        #region 輸出參數賦值
                        if (property.PropertyType.FullName == "System.Int32")
                        {
                            parameters.Add(property.Name, value, OracleMappingType.Int32, ParameterDirection.Input);
                        }
                        if (property.PropertyType.FullName == "System.String")
                        {
                            parameters.Add(property.Name, value, OracleMappingType.NVarchar2, ParameterDirection.Input);
                        }
                        if (property.PropertyType.FullName == "System.Clob")
                        {
                            parameters.Add(property.Name, value, OracleMappingType.Clob, ParameterDirection.Input);
                        }
                        if (property.PropertyType.FullName == "System.Decimal")
                        {
                            parameters.Add(property.Name, value, OracleMappingType.Decimal, ParameterDirection.Input);
                        }
                        if (property.PropertyType.FullName == "System.Single")
                        {
                            parameters.Add(property.Name, value, OracleMappingType.Decimal, ParameterDirection.Input);
                        }
                        if (property.PropertyType.FullName == "System.DateTime")
                        {
                            parameters.Add(property.Name, value, OracleMappingType.Date, ParameterDirection.Input);
                        }
                        #endregion
                    }
                    else
                    {
                        strColumn.AppendFormat(property.Name + "=" + "@{0}", property.Name);
                    }
                    if (i >= properties.Length - 1)
                        continue;
                    strColumn.Append(",");
                }
            }

            if (strColumn.ToString().EndsWith(","))
                strColumn.Remove(strColumn.Length - 1, 1);

            if (strColumn.Length > 0 && strWhere.Length > 0)
            {
                if (DBName.Contains("Oracle"))
                {
                    strSql.Append("UPDATE " + entity.GetType().Name + " SET " + strColumn + " WHERE " + strWhere);
                }
                else
                {
                    strSql.Append("UPDATE " + entity.GetType().Name + " SET " + strColumn + " WHERE " + strWhere + " ;");

                }
            }


            return strSql.Length > 0 ? strSql.ToString() : string.Empty;

            #endregion
        }
        /// 获取自动根据主键删除数据SQL
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>sql语句</returns>
        public static string MakeDeleteSql(Object entity)
        {
            #region  拼接SQL
            if (entity == null)
                return string.Empty;

            var type = entity.GetType();
            var properties = AutoTaskCacheFactory.CacheProperties(type);
            var strSql = new StringBuilder();
            var strWhere = new StringBuilder(" 1=1 ");

            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                //获取验证特性
                var validateContent = AutoTaskCacheFactory.CacheCustomAttributes(type, property);
                //获取属性的值 
                var value = property.GetValue(entity, null);

                if (validateContent.Length < 1)
                    continue;

                ColumnAttribute validateAttribute = validateContent[0] as ColumnAttribute;

                if (validateAttribute == null || validateAttribute.FilterType != FilterType.IsPrimaryKey)
                    continue;
                if (DBName.Contains("Oracle"))
                {
                    strWhere.AppendFormat(" and " + property.Name + "=" + ":{0} ", property.Name);
                }
                else
                {
                    strWhere.AppendFormat(" and " + property.Name + "=" + "@{0} ", property.Name);
                }
            }

            if (strWhere.Length > 0)
            {
                if (DBName.Contains("Oracle"))
                {
                    strSql.Append("DELETE " + entity.GetType().Name + " WHERE " + strWhere);
                }
                else
                {
                    strSql.Append("DELETE " + entity.GetType().Name + " WHERE " + strWhere + " ; ");
                }
            }
            else
            {
                return "";
            }


            return strSql.Length > 0 ? strSql.ToString() : string.Empty;

            #endregion
        }

        /// <summary>
        /// 如果遇到没有构建返回实体或查询语句很复杂不太知道怎么构建实体 可以把T定义为dynamic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql查询语句</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">asc/desc</param>
        /// <param name="num">每页多少行</param>
        /// <param name="page">第几页</param>
        /// <param name="messageEntity"></param>
        /// <returns></returns>
        public static List<T> EntityForSqlToPager<T>(string sql, string sort, string ordering, int num, int page, out MessageEntity messageEntity, DBConnNames dbname = DBConnNames.GISWaterSupplyAndSewageServer)
        {
            int beginNum = (page - 1) * (num);
            //if (beginNum == 0) { beginNum = 1; }
            int endNum = beginNum + num;
            string sqlString = "";
            if (DBName.Contains("Oracle"))
            {
                sqlString = "SELECT * FROM(SELECT  ROW_NUMBER() OVER ( Order by " + sort + " " + ordering + " ) AS Pos,T.*  FROM (" + sql + ")  T)  where Pos > " + beginNum.ToString() + " and Pos <= " + endNum.ToString();
            }
            else
            {
                sqlString = "SELECT * FROM(SELECT * , ROW_NUMBER() OVER ( Order by " + sort + " " + ordering + " ) AS Pos FROM (" + sql + ") as T) AS TT where TT.Pos > " + beginNum.ToString() + " and TT.Pos <= " + endNum.ToString();
            }
            string sqlCountString = "select count(0) as ROWSCOUNT from (" + sql + " )  TT";

            try
            {

                using (IDbConnection conn = AutoTaskConnectionFactory.GetDBConn(dbname))
                {
                    dynamic result = conn.Query<T>(sqlString);
                    dynamic resultTotalResult = conn.Query<dynamic>(sqlCountString).FirstOrDefault();

                    messageEntity = MessageEntityTool.GetMessage(result.Count, result, true, "完成", Convert.ToInt32(((IDictionary<string, object>)resultTotalResult)["ROWSCOUNT"]));
                    return result;
                }
            }
            catch (Exception e)
            {
                messageEntity = MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
            return null;

        }
    }
}
