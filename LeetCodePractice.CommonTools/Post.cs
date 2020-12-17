using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Runtime.Remoting.Contexts;

namespace GisPlateform.CommonTools
{
    public class   Post
    {
        /// <summary>
        /// 发送请求，返回服务器响应
        /// </summary>
        /// <param name="strUrl">相对网址</param>
        /// <param name="strParm">参数</param>
        /// <returns></returns>
        public string PostModel(string strUrl, string strParm)
        {
            string retrunString = "";
            try
            {
                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string[] url = path.Split('/');
                string httpURL = url[0] + "//" + url[2] + "/";
                // string httpURL = path.Split('P')[0];
                strUrl = httpURL + strUrl;
                //创建HttpWebRequest对象

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strUrl);//目标主机ip地址

                //模拟POST的数据
                Encoding utf8 = Encoding.UTF8;
                byte[] data = utf8.GetBytes(strParm);

                //设置请求头信息
                string cookieheader = string.Empty;
                CookieContainer cookieCon = new CookieContainer();
                request.Method = "POST";
                //设置cookie，若没有可以不设置
                request.CookieContainer = cookieCon;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (Stream newStream = request.GetRequestStream())
                {
                    //把请求数据 写入请求流中
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                }
                //获得HttpWebResponse对象
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //获得响应流
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    //输入响应流信息
                    retrunString = readStream.ReadToEnd();

                    response.Close();
                    receiveStream.Close();
                    readStream.Close();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return retrunString;

        }
    }
}
