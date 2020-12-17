using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.TaskManage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.PlanningEngine.TaskManage
{
    public class InsCancelTaskNewsController : Controller
    {
        /// <summary>
        /// 手动关闭推送消息
        /// </summary>
        /// <param name="taskId">任务id </param>
        /// <param name="proraterId">人员id</param>
        /// <param name="taskName">任务名称 </param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity CancelNews(string taskId, string proraterId, string taskName)
        {
            try
            {
                string ExchangeName = Appsettings.app(new string[] { "RabbitExchangeName" });
                string RoteKey = Appsettings.app(new string[] { "RabbitRoteKsy" });

                RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory
                {
                    UserName = Appsettings.app(new string[] { "RabbitUserName" }),
                    Password = Appsettings.app(new string[] { "RabbitPassword" }),
                    HostName = Appsettings.app(new string[] { "RabbitUrl" }),
                    Port = int.Parse(Appsettings.app(new string[] { "RabbitPort" }))
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string PostData = JsonConvert.SerializeObject(new
                {
                    type = "event",
                    status = "recover",
                    data = new
                    {
                        type = "bpm",
                        push = "app",
                        sourceid = taskId,
                        users = new string[] { proraterId },
                        title = "巡检任务",
                        content = taskName,
                        begin = 10,
                        recovery = Convert.ToInt64(ts.TotalSeconds)
                    }
                });
                var sendBytes = Encoding.UTF8.GetBytes(PostData);
                channel.BasicPublish(ExchangeName, RoteKey, null, sendBytes);
            }catch(Exception ex)
            {

                return MessageEntityTool.GetMessage(ErrorType.OprationError, "", ex.Message);
            }

            return MessageEntityTool.GetMessage(1, true);
        }
    }
}