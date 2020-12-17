using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCodePractice.CommonTools
{
    public class UploadTxt
    {
        /// <summary>
        /// 将信息写入txt文件
        /// </summary>
        /// <param name="json"></param>
        /// <param name="fileName">下载文件名</param>
        public string WriteTxt(string json, string fileName, string fileFormat)
        {
            try
            {
                //如果传过strList无内容，直接返回，不写日志
                if (json == "" || json == null) return null;
                //获取本地服务器路径
                string strDicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upload/JsonFile/");
                if (fileFormat == "" || fileFormat == "undefined")
                {
                    fileFormat = ".txt";
                }
                string relativeDir = "upload/JsonFile/" + string.Format("{0:yyyy年MM月dd日HH时mm分ss秒}", DateTime.Now) + fileName + fileFormat;
                //创建下载文件路径
                string strPath = strDicPath + string.Format("{0:yyyy年MM月dd日HH时mm分ss秒}", DateTime.Now) + fileName + fileFormat;
                //如果服务器路径不存在，就创建一个
                if (!Directory.Exists(strDicPath)) Directory.CreateDirectory(strDicPath);
                //如果下载文件不存在，创建一个
                if (!File.Exists(strPath)) using (FileStream fs = File.Create(strPath)) ;
                //读取下载文件中的信息
                string str = File.ReadAllText(strPath);
                StringBuilder sb = new StringBuilder();
                //将json信息写入sb
                sb.AppendLine(json);
                //将json信息写入txt
                File.WriteAllText(strPath, sb.ToString() + str);
                return relativeDir;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}
