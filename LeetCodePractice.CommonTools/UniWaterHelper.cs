using GISWaterSupplyAndSewageServer.Model.UniWater;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.CommonTools
{
    public class UniWaterHelper
    {
        /// <summary>
        /// 获取App端用户信息
        /// </summary>
        /// <param name="App"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        public HdUser GetUniWaterInfoForApp(string App, string Authorization, string Url = "")
        {
            if (string.IsNullOrEmpty(App) || string.IsNullOrEmpty(Authorization))
            {
                return null;
            }
            try
            {
                string UniwaterUrl = string.IsNullOrEmpty(Url) ? Appsettings.app(new string[] { "UniwaterUrl" }) : Url.TrimEnd('/');
                string url = UniwaterUrl + "/app/v1.0/userinfo.json";
                var responseData = PostResponse(url, "{}", Authorization, App).Result;
                var resault = JsonConvert.DeserializeObject(responseData);
                HdUser user = JsonConvert.DeserializeObject<HdUser>(((dynamic)resault).Response.ToString());
                return user;
            }
            catch (Exception e)
            {
                LogHelper.Error("获取App端用户信息:" + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取Web端用户信息
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        public HdUser GetUniWaterInfoForWeb(string AccessToken, string Url = "")
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return null;
            }
            try
            {
                string UniwaterUrl = string.IsNullOrEmpty(Url) ? Appsettings.app(new string[] { "UniwaterUrl" }) : Url.TrimEnd('/');
                string data = JsonConvert.SerializeObject(new
                {
                    access_token = AccessToken,
                    type = "user"
                });
                string url = UniwaterUrl + "/hdl/oauth/v1.0/access_check.json";
                var responseData = PostResponse(url, data).Result;
                var resault = JsonConvert.DeserializeObject(responseData);
                UserAccessResult accessResult = JsonConvert.DeserializeObject<UserAccessResult>(responseData);
                return accessResult.User;
            }
            catch (Exception e)
            {
                LogHelper.Error("获取Web端用户信息:" + e.Message);
                return null;
            }
        }

        #region 发送请求
        public readonly HttpClient client = new HttpClient();
        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postString"></param>
        /// <returns></returns>
        public async Task<string> PostResponse(string strUrl, string data)
        {
            string retrunString = "";
            try
            {
                var buffer = Encoding.UTF8.GetBytes(data);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(strUrl, byteContent).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Task<string> t = response.Content.ReadAsStringAsync();
                    return t.Result;
                }
            }
            catch (Exception e)
            {
                retrunString = e.Message;
            }
            return retrunString;
        }
        /// <summary>
        /// 发送post请求 带Headers
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postString"></param>
        /// <returns></returns>
        public async Task<string> PostResponse(string strUrl, string data, string Authorization, string app)
        {
            string retrunString = "";
            try
            {
                var buffer = Encoding.UTF8.GetBytes(data);
                var byteContent = new ByteArrayContent(buffer);
                client.DefaultRequestHeaders.Add("Authorization", Authorization);
                client.DefaultRequestHeaders.Add("app", app);

                var response = await client.PostAsync(strUrl, byteContent).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Task<string> t = response.Content.ReadAsStringAsync();
                    return t.Result;
                }
            }
            catch (Exception e)
            {
                retrunString = e.Message;
            }
            return retrunString;
        }
        #endregion
    }
}
