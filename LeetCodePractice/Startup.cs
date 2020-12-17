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
            #region ע��Appsettings
            services.AddSingleton(new Appsettings(Env.ContentRootPath));
            #endregion
            #region �������JSON����ĸСд����
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //��ʹ���շ���ʽ��key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            });
            #endregion
            #region ����ע��
            foreach (var item in GetClassName("LeetCodePractice.OracleDAL"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
            #endregion
            #region ȫ��·��
            services.AddMvc(opt =>
            {
                opt.UseCentralRoutePrefix(new RouteAttribute("api/[controller]/[action]"));
            });
            #endregion
            #region ������
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
                        Description = "��ܼ���",
                    });
                    //��Ӷ�ȡע�ͷ���
                    c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GISServerForCore2.0.xml"), true);//��ӿ�������ע�ͣ�true��ʾ��ʾ������ע�ͣ�
                                                                                                                               //����token����
                    c.OperationFilter<AddAuthTokenHeaderParameter>();
                });
            }
            #endregion
            #region ������� ����CORS����
            services.AddCors(options =>
            {
                options.AddPolicy("cors",

                builder => builder.AllowAnyOrigin().AllowAnyHeader()

                .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")

                );

            });
            #endregion
            # region ����ļ��ϴ���С����
            //����ļ��ϴ�Multipart body length limit 134217728 exceeded
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
                RequestPath = new PathString("/uploadFile"),//����ķ���·��
                FileProvider = new PhysicalFileProvider(env.ContentRootPath + "/uploadFile"),//ָ��ʵ������·��

            });
            app.UseRouting();
            #region ����CORS�м��
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
                    ctx.Response.Redirect("/api/BaseApi/Index"); //����֧������·������index.html������ʼҳ.
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
        #region ��ȡ�����е�ʵ�����Ӧ�Ķ���ӿ�
        /// <summary>  
        /// ��ȡ�����е�ʵ�����Ӧ�Ķ���ӿ�
        /// </summary>  
        /// <param name="assemblyName">����</param>
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
