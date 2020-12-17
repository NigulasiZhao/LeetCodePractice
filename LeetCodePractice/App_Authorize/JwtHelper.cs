using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.CommonTools;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace GISWaterSupplyAndSewageServer.App_Authorize
{
    public class JwtHelper
    {
        //私钥  web.config中配置
        //"GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="payload">不敏感的用户数据</param>
        /// <returns></returns>
        public static string SetJwtEncode(Dictionary<string, object> payload)
        {
            string salt = Appsettings.app(new string[] { "Salt" });
            //_configuration.GetSection("Salt").ToString();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, salt);
            return token;
        }

        /// <summary>
        /// 根据jwtToken  获取实体
        /// </summary>
        /// <param name="token">jwtToken</param>
        /// <returns></returns>
        public static Authorize GetJwtDecode(string token)
        {
            string salt = Appsettings.app(new string[] { "Salt" });
            //_configuration.GetSection("Salt").ToString();
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm Algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, Algorithm);
            var userInfo = decoder.DecodeToObject<Authorize>(token, salt, verify: true);//token为之前生成的字符串
            return userInfo;
        }
    }
}
