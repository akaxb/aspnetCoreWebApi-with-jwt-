using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using User.API.Data;
using User.API.Filters;
using User.API.Models;
using User.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.API.Jwt;
using User.API.Dtos;
using Consul;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace User.API
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
            //add db service
            services.AddDbContext<MyContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Default"));
                });
            //add custom exception filter
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "AppUser API", Version = "v1" });
            });
            #region add jwt Authentication
            if (bool.Parse(Configuration["JwtAuthentication:IsEnabled"]))
            {
                JwtAuthConfigurer.Configure(services, Configuration);
            }
            #endregion
            //add user repository
            services.AddScoped<IRepository<AppUser, int>, AppUserRepository>();

            //Consul
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppUser API V1");
            });
            app.UseAuthentication();
            app.UseMvc();
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, serviceOptions, consul, applicationLifetime);
            });
            AppUserContextSeed.SeedAsync(app, loggerFactory).Wait();
        }

        private void RegisterService(IApplicationBuilder app,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul,
            IApplicationLifetime appLife)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };
                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
                appLife.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }
        }
    }
}
