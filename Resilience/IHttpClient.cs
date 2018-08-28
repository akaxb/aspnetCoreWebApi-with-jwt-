using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Resilience
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync<T>(string uri,T item,string authorizationToken,string requestId=null,string authorizationMethod="Bearer");

        Task<HttpResponseMessage> PostAsync(string uri, Dictionary<string,string> form, string authorizationToken=null, string requestId = null, string authorizationMethod = "Bearer");
    }
}
