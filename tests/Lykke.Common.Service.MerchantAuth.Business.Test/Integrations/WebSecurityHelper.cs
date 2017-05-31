using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Lykke.Common.Entities.Security;
using Lykke.Common.Service.MerchantAuth.Business.Interfaces;
using Lykke.Common.Service.MerchantAuth.Business.Test.Models;

namespace Lykke.Common.Service.MerchantAuth.Business.Test.Integrations
{
    public class WebSecurityHelper 
    {
        //private const string ServiceUrl = "http://lykke-dev-pay.azurewebsites.net/api/verify";
        private const string ServiceUrl = "http://localhost:3004/api/auth";
        private static HttpClient _client;
        private static readonly object Lock = new object();

        public WebSecurityHelper()
        {
            lock (Lock)
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                }
            }
        }

        public SecurityErrorType CheckRequest(MerchantAuthRequest request)
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = _client.PostAsync(ServiceUrl, content).Result;
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"The request returne error {(int)result.StatusCode}");
            }

            
            return (SecurityErrorType)int.Parse(result.Content.ReadAsStringAsync().Result);
        }
    }
}
