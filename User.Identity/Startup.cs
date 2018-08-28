using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience;
using User.Identity.Dtos;
using User.Identity.Infrastructure;
using User.Identity.Services;

namespace User.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddIdentityServer()
                    .AddExtensionGrantValidator<Authentication.SmsCodeValidator>()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryClients(Config.GetClients())
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApiResources());
            services.AddScoped<IAuthCodeService, AuthCodeService>();
            services.AddScoped<IUserService, UserService>();
            //services.AddSingleton(new System.Net.Http.HttpClient());
            //注册全局变量ResilienceClientFactory
            services.AddSingleton(typeof(ResilienceClientFactory), (sp) =>
             {
                 var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                 var httpContextAccessor = sp.GetService<IHttpContextAccessor>();
                 return new ResilienceClientFactory(logger, httpContextAccessor, 5, 5);
             });
            //注册全局变量IhttpClient
            services.AddSingleton<IHttpClient>(sp =>
            {
                var factory = sp.GetRequiredService<ResilienceClientFactory>();
                return factory.GetResilienceHttpClient();
            });
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IDnsQuery>(p =>
            {
                return new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}
