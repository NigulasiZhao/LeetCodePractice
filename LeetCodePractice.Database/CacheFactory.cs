using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Database
{
    public class CacheFactory
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private static CacheFactory _instance;

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

        public CacheFactory()
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

        public static CacheFactory GetInstance()
        {
            #region  获取实例

            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        return _instance = new CacheFactory();
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

                if (CacheFactory.GetInstance().IsPropertyExist(key))
                {
                    properties = CacheFactory.GetInstance().GetProperties(key);
                }
                else
                {
                    properties = type.GetProperties();
                    CacheFactory.GetInstance().SetProperties(key, properties);
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
                if (CacheFactory.GetInstance().IsAttributeExist(key))
                {
                    objects = CacheFactory.GetInstance().GetCustomAttributes(key);
                }

                if (objects == null)
                {
                    objects = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                    CacheFactory.GetInstance().SetCustomAttributes(key, objects);
                }

                return objects;
            }
            #endregion
        }

    }
}
