using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace GISWaterSupplyAndSewageServer.CommonTools
{
    public class ImageFactory
    {


        public bool SaveBase64Img(string base64Img, string fullPath)
        {
            try
            {

                byte[] arr = Convert.FromBase64String(base64Img);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                string path = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                bmp.Save(fullPath);
                ms.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getPictureUrl([FromBody]string[] base64Image)
        {
            string eventPictures = string.Empty;
            DateTime dateNow = DateTime.Now;
            ImageFactory imageFactory = new ImageFactory();
            Random random = new Random();
            if (base64Image != null && base64Image.Length > 0)
            {
                foreach (string item in base64Image)
                {
                    string[] arr = item.Split('|');
                    string imageStream = arr[0];//base64编码的图片
                    string formate = arr[1];//图片格式带点
                    if (imageStream == "")
                    {
                        return null;
                    }
                    string relativeDir = "uploadFile/EventsImg/" + dateNow.Year + "/" + dateNow.Month + "/" + dateNow.Day;
                    string imageName = string.Format("{0:yyyyMMddHHmmss}", dateNow);
                    string relativePath = string.Format("{0}/{1}{2}", relativeDir, imageName + random.Next(1, 100), formate);
                    try
                    {
                        if (Base64ToFileAndSave(imageStream, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath)))
                        {
                            eventPictures += relativePath + '|';
                        }
                    }
                    catch (Exception e)
                    {
                        return e.ToString();
                    }
                }
                return eventPictures;

            }
            return null;
        }

        /// <summary>
        /// Base64字符串转换成文件
        /// </summary>
        /// <param name="strInput">base64字符串</param>
        /// <param name="fileName">保存文件的绝对路径</param>
        /// <returns></returns>
        public  bool Base64ToFileAndSave(string strInput, string fileName)
        {
            bool bTrue = false;

            try
            {
                string path = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                byte[] buffer = Convert.FromBase64String(strInput);
                FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                bTrue = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bTrue;
        }
    
}
}
