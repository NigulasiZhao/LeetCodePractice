using Microsoft.AspNetCore.Http;
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
    public class FileFactory
    {
        public string getFileUrl([FromBody]string[] base64Image)
        {
            string eventPictures = string.Empty;
            DateTime dateNow = DateTime.Now;
            FileFactory fileFactory = new FileFactory();
            Random random = new Random();
            if (base64Image != null && base64Image.Length > 0)
            {
                foreach (string item in base64Image)
                {
                    string[] arr = item.Split('|');
                    string imageStream = arr[0];//base64编码的文件
                    string formate = arr[1];//文件格式带点
                    if (imageStream == "")
                    {
                        return null;
                    }
                    string relativeDir = "uploadFile/EventsImg/" + dateNow.Year + "/" + dateNow.Month + "/" + dateNow.Day;
                    string imageName = string.Format("{0:yyyyMMddHHmmss}", dateNow);
                    string relativePath = string.Format("{0}/{1}{2}", relativeDir, imageName + random.Next(1, 100), formate);
                    try
                    {
                        if (fileFactory.UploadFile(imageStream, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath)))
                        {
                            eventPictures += relativePath + '|';
                        }
                    }
                    catch (Exception e)
                    {
                        return e.ToString();
                    }
                    return eventPictures;
                }
            }
            return null;
        }
        public bool UploadFile(string base64String, string fullPath)
        {

            try
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64String)))
                {//有时候为了避免流指针定位错误，显式定义一下指针位置到也可以
                    ms.Seek(0, SeekOrigin.Begin);
                    //路径首先判断是否存在，不存在创建
                    string path = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (Stream fs = new FileStream(fullPath, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string UploadFiles(IFormFileCollection files)
        {
            string relativeImagePath = "", relativeVideoPath="", relativePath="";
            try
            {
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        if (file.Length > 0)
                        {
                            DateTime dateNow1 = DateTime.Now;
                            //全路径 
                            string FullFullName = file.FileName;
                            string relativeDir = "uploadFile/EventsVideo/" + dateNow1.Year + "/" + dateNow1.Month + "/" + dateNow1.Day;
                            string imageName = string.Format("{0:yyyyMMddHHmmssfff}", dateNow1);
                            string AllPath = string.Format("{0}", relativeDir);

                            relativePath = string.Format("{0}/{1}", relativeDir, imageName + "." + FullFullName.Split('.')[1]);
                            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath)))
                            {
                                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath));
                            }
                            using (var stream = new FileStream(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AllPath), imageName + "." + FullFullName.Split('.')[1]), FileMode.Create))
                            {
                                file.OpenReadStream().CopyTo(stream);
                            }
                            if (FullFullName.Contains("mp4") || FullFullName.Contains("MOV") || FullFullName.Contains("AMV") || FullFullName.Contains("DV") || FullFullName.Contains("WMV") || FullFullName.Contains("AVI") || FullFullName.Contains("Mpg") || FullFullName.Contains("RM") || FullFullName.Contains("RMVB"))
                            {
                                relativeVideoPath += relativePath + '|';
                            }
                            else if (FullFullName.Contains("jpeg") || FullFullName.Contains("png") || FullFullName.Contains("gif") || FullFullName.Contains("jpg"))
                            {
                                relativeImagePath += relativePath + '|';
                            }
                        }
                    }
                    return relativeImagePath + "-" + relativeVideoPath;

                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
    }

}
