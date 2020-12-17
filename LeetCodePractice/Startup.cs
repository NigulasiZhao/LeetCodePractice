using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.App_Start;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
//using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GISWaterSupplyAndSewageServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            #region 注册Appsettings
            services.AddSingleton(new Appsettings(Env.ContentRootPath));
            #endregion
            #region 解决返回JSON首字母小写问题
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            });
            #endregion
            #region 依赖注入
            foreach (var item in GetClassName("LeetCodePractice.OracleDAL"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
            #endregion
            #region 全局路由
            services.AddMvc(opt =>
            {
                opt.UseCentralRoutePrefix(new RouteAttribute("api/[controller]/[action]"));
            });
            #endregion
            #region 过滤器
            Action<MvcOptions> filters = new Action<MvcOptions>(r =>
            {
                //  r.Filters.Add(typeof(ExceptionFilter));
                r.Filters.Add(typeof(APIExceptionHandler));
            });
            services.AddMvc(filters);
            #endregion
            #region Swagger
            if (Env.IsDevelopment() || bool.Parse(Appsettings.app(new string[] { "IsTestEnv" })))
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = "v1.1.0",
                        Title = "Ray WebAPI",
                        Description = "框架集合",
                    });
                    //添加读取注释服务
                    c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GISServerForCore2.0.xml"), true);//添加控制器层注释（true表示显示控制器注释）
                                                                                                                               //增加token参数
                    c.OperationFilter<AddAuthTokenHeaderParameter>();
                });
            }
            #endregion
            #region 解决跨域 配置CORS服务
            services.AddCors(options =>
            {
                options.AddPolicy("cors",

                builder => builder.AllowAnyOrigin().AllowAnyHeader()

                .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")

                );

            });
            #endregion
            # region 解决文件上传大小限制
            //解决文件上传Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;
            });
            #endregion
            services.AddHttpClient();
            #region Redis
            //services.AddSingleton(new RedisHelper(Configuration["Redis:Default:Connection"], Configuration["Redis:Default:DefaultKey"], int.Parse(Configuration["Redis:Default:DefaultDB"])));
            #endregion
            #region Hangfire
            //services.AddHangfire(configuration =>
            //{
            //    configuration.UseRedisStorage(ConnectionMultiplexer.Connect(Configuration["Redis:Default:Connection"]));
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || bool.Parse(Appsettings.app(new string[] { "IsTestEnv" })))
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = new PathString("/uploadFile"),//对外的访问路径
                FileProvider = new PhysicalFileProvider(env.ContentRootPath + "/uploadFile"),//指定实际物理路径

            });
            app.UseRouting();
            #region 配置CORS中间件
            app.UseCors("cors");
            #endregion
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region Swagger
            if (Env.IsDevelopment() || bool.Parse(Appsettings.app(new string[] { "IsTestEnv" })))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                    c.DocExpansion(DocExpansion.None);
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.Run(ctx =>
                {
                    ctx.Response.Redirect("/api/BaseApi/Index"); //可以支持虚拟路径或者index.html这类起始页.
                    return Task.FromResult(0);
                });
            }
            #endregion
            #region Hangfire
            //app.UseHangfireServer();
            //app.UseHangfireDashboard("/hangfire");
            //new HangFireHelper().StartHangFireTask();
            #endregion

        }
        #region 获取程序集中的实现类对应的多个接口
        /// <summary>  
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>  
        /// <param name="assemblyName">程序集</param>
        public Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }
                return result;
            }
            return new Dictionary<Type, Type[]>();
        }
        #endregion
    }
}
