using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISWaterSupplyAndSewageServer.CommonTools
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }

        public Appsettings(string contentPath)
        {
            //var builder = new ConfigurationBuilder()
            //.SetBasePath(env.ContentRootPath)
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            string Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })
               .Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }
    }
}
