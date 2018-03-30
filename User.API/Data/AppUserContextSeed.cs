using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Data
{
    public class AppUserContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder builder,ILoggerFactory loggerFactory)
        {
            var _logger = loggerFactory.CreateLogger<AppUserContextSeed>();
            try
            {
                using (var scope = builder.ApplicationServices.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MyContext>();
                    context.Database.Migrate();
                    if (!context.AppUsers.Any())
                    {
                        var user = new AppUser
                        {
                            Name = "xubo",
                            Phone = "18112791157"
                        };
                        await context.AppUsers.AddAsync(user);
                        await context.SaveChangesAsync();
                        _logger.LogInformation("send data success");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
