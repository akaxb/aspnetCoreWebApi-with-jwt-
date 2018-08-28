using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Wrap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Resilience
{
    public class ResilienceHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;
        //根据uri的origin创建policy
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;
        //把policy打包成wrap进行本地缓存
        private readonly ConcurrentDictionary<string, PolicyWrap> _policyWrappers;
        private ILogger<ResilienceHttpClient> _logger;
        private IHttpContextAccessor _httpContextAccessor;


        public ResilienceHttpClient(Func<string, IEnumerable<Policy>> policyCreator, ILogger<ResilienceHttpClient> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _policyCreator = policyCreator;
            _policyWrappers = new ConcurrentDictionary<string, PolicyWrap>();
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Post, uri, item);
            return await DoPostPutAsync(HttpMethod.Post, uri, func, authorizationToken, requestId, authorizationMethod);
        }

        private Task<HttpResponseMessage> DoPostPutAsync(HttpMethod method, string uri, Func<HttpRequestMessage> func, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }
            // a new StringContent must be created for each retry 
            // as it is disposed after each call
            var origin = GetOriginFromUri(uri);
            return HttpInvoker(origin, async () =>
            {
                var requestMessage = func();
                SetAuthorizationHeader(requestMessage);
                //requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }
                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }
                var response = await _httpClient.SendAsync(requestMessage);
                // raise exception if HttpResponseCode 500 
                // needed for circuit breaker to track fails
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }
                return response;
            });
        }


        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method, string uri, T item)
        {
            return new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
            };
        }
        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string uri, Dictionary<string, string> form)
        {
            return new HttpRequestMessage(method, uri) { Content = new FormUrlEncodedContent(form) };
        }


        private async Task<T> HttpInvoker<T>(string origin, Func<Task<T>> action)
        {
            var normalizedOrigin = NormalizeOrigin(origin);
            if (!_policyWrappers.TryGetValue(normalizedOrigin, out PolicyWrap policyWrap))
            {
                policyWrap = Policy.WrapAsync(_policyCreator(normalizedOrigin).ToArray());
                _policyWrappers.TryAdd(normalizedOrigin, policyWrap);
            }

            // Executes the action applying all 
            // the policies defined in the wrapper
            return await policyWrap.ExecuteAsync(action, new Context(normalizedOrigin));
        }

        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();
        }

        private static string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;
        }
        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, Dictionary<string, string> form, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Post, uri, form);
            return await DoPostPutAsync(HttpMethod.Post, uri, func, authorizationToken, requestId, authorizationMethod);
        }
    }
}
