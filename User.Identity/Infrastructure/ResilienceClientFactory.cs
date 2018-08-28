using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace User.Identity.Infrastructure
{

    public class ResilienceClientFactory
    {
        private ILogger<ResilienceHttpClient> _logger;
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 重试次数
        /// </summary>
        private int _retryCount;
        /// <summary>
        /// 熔断前允许发生的异常次数
        /// </summary>
        private int _exceptionCountAllowedBeforeBreaking;

        public ResilienceClientFactory(ILogger<ResilienceHttpClient> logger, IHttpContextAccessor httpContextAccessor, int retryCount, int exceptionCountAllowedBeforeBreaking)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _exceptionCountAllowedBeforeBreaking = exceptionCountAllowedBeforeBreaking;
        }

        public ResilienceHttpClient GetResilienceHttpClient() =>
new ResilienceHttpClient((origin) => CreatePolicies(origin), _logger, _httpContextAccessor);

        private Policy[] CreatePolicies(string origin)
        {
            return new Policy[] {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(_retryCount,retryAttempt=>TimeSpan.FromSeconds(Math.Pow(2,retryAttempt))
                ,(exception,timeSpan,retryCount,context)=>{
                    var msg=$"第{retryCount}次重试"
                    +$"of {context.PolicyKey}"
                    +$"at {context.ExecutionKey}"
                    +$"due to {exception}";
                    _logger.LogWarning(msg);
                    _logger.LogDebug(msg);
                }
                ),
                Policy.Handle<HttpRequestException>().CircuitBreakerAsync(_exceptionCountAllowedBeforeBreaking,
                TimeSpan.FromMinutes(1),
                (exception,duration)=>{
                    _logger.LogTrace("熔断打开");
                },()=>{
                    _logger.LogTrace("熔断关闭");
                })
            };
        }
    }
}
