﻿using System.Net.Mime;
using AGooday.AgPay.Common.Utils;
using AGooday.AgPay.Components.Third.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AGooday.AgPay.Components.Third.Channel.UmsPay.Utils
{
    public class UmsPayHttpUtil
    {
        private static readonly string DEFAULT_CHARSET = "UTF-8";
        private static readonly int DEFAULT_TIMEOUT = 60; // 60 秒超时

        public static async Task<string> DoPostJsonAsync(string url, string appid, string appkey, JObject reqParams)
        {
            var client = new AgHttpClient(DEFAULT_TIMEOUT, DEFAULT_CHARSET);
            var body = JsonConvert.SerializeObject(reqParams, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");// 请求时间戳，格式：yyyyMMddHHmmss
            string nonce = Guid.NewGuid().ToString("N");// 随机数
            var authorization = UmsPaySignUtil.GetAuthorization(appid, appkey, timestamp, nonce, body);
            var request = new AgHttpClient.Request()
            {
                Url = url,
                Method = HttpMethod.Post.Method,
                Content = body,
                ContentType = MediaTypeNames.Application.Json,
                Headers = new Dictionary<string, string> { { "Authorization", authorization } }
            };
            try
            {
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content;
                    return result;
                }
                return null;
            }
            catch (Exception e)
            {
                throw ChannelException.SysError(e.Message);
            }
        }
    }
}
