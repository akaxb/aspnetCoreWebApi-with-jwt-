using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using User.Identity.Dtos;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private string url;

        private readonly IHttpClient _httpClient;

        private readonly ILogger<UserService> _logger;

        public UserService(IHttpClient httpClient, 
            IDnsQuery dnsQuery, 
            IOptions<ServiceDiscoveryOptions> options,
            ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            var address = dnsQuery.ResolveService("service.consul", options.Value.ServiceName);
            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;
            //host = host.Contains('.') ? host.Split('.')[0] : host;
            var port = address.First().Port;
            url = $"http://{host}:{port}";
            _logger = logger;
        }

        public async Task<int> CreateOrCheckAsync(string phone)
        {
            var form = new Dictionary<string, string> {
                { "phone",phone} };
            try
            {
                var response = await _httpClient.PostAsync($"{url}/api/appuser/check-or-create", form);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var userId = await response.Content.ReadAsStringAsync();
                    int.TryParse(userId, out int id);
                    return id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateOrCheckAsync重试失败--- {ex.Message}");
                throw ex;
            }
            return 0;
        }
    }
}
