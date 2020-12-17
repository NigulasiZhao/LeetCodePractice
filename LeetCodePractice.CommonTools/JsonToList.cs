using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GisPlateform.CommonTools
{
   public class JsonToList
    {
        /// <summary>
        /// JSON格式数组转化为对应的List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JsonStr">JSON格式数组</param>
        /// <returns></returns>
        public  List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            Serializer.MaxJsonLength = int.MaxValue;
            //设置转化JSON格式时字段长度
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }
    }
}