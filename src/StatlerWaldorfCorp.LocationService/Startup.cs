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
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;
using Microsoft.Extensions.Configuration.Json;

namespace StatlerWaldorfCorp.LocationService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _loggerFactory = loggerFactory;
            _loggerFactory.AddConsole(LogLevel.Information);
            _loggerFactory.AddDebug();

            _logger = _loggerFactory.CreateLogger("Startup");
        }

        public static IConfiguration Configuration { get; set; }

        private ILogger _logger;
        private ILoggerFactory _loggerFactory;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = Configuration.GetSection("Db:ConnectionString").Value;
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<LocationDbContext>(options => options.UseNpgsql(connectionString));
            _logger.LogInformation($"Using '{connectionString}' for Db connection string");
            services.AddScoped<ILocationRecordRepository, LocationRecordRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<LocationDbContext>();
                    if (context.Database.GetPendingMigrations() != null)
                    {
                        context.Database.Migrate();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to migrate or seed database");
            }

            app.UseMvc();
        }
    }
}
