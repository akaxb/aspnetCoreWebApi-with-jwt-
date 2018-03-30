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
                JwtAuthConfigurer.Configure(services,Configuration);
            }
            #endregion
            //add user repository
            services.AddScoped<IRepository<AppUser, int>, AppUserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
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
            AppUserContextSeed.SeedAsync(app, loggerFactory).Wait();
        }
    }
}
