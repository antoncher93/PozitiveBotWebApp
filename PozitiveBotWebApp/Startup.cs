using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PozitiveBotWebApp.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PozitiveBotWebApp.Logging;
using System.IO;
using Positive.SqlDbContext;
using Pozitive.Services;
using Pozitive.Entities.Repos;
using Pozitive.Entities;

namespace PozitiveBotWebApp
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            string token = Configuration["Token"];

            services
                .AddPozitiveSqlServer(connection)
                .AddPositiveBotServices(token)
                //.AddTelegramBotClient(Configuration)
                .AddControllersWithViews()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IBot bot)
        {
            loggerFactory
                .AddFile("bot.log");

            var logger = loggerFactory.CreateLogger("FileLogger");

            logger.LogInformation("Processing Start");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            logger.LogInformation("Authorization");

            app.UseAuthorization();
            app.UseAuthentication();
            logger.LogInformation("Authorization End");

            var url = string.Format(Configuration["Url"], @"api/message/update");
            bot.Start(url);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
