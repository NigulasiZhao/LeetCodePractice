using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.UniWater
{
    public class Access_TokenResult
    {
        /// <summary>
        /// 凭据类型
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 应用凭据
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 刷新凭据
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 失效周期（秒）- 当前为86400秒
        /// </summary>
        public int expires_in { get; set; }
    }
}
