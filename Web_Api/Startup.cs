using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using NetEscapades.AspNetCore.SecurityHeaders;
using WebApi.Models;

namespace WebApi
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
            // Database
            var sqlConnectionString = Configuration.GetConnectionString("SwitchLook");
            services.AddDbContext<WebApiDataContext>(options =>
                options.UseMySql(
                    sqlConnectionString
                )
            );

            // Identity
            services.AddIdentity<Account, IdentityRole>()
                .AddEntityFrameworkStores<WebApiDataContext>()
                .AddDefaultTokenProviders();

            services.AddCustomHeaders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;

                    return Task.CompletedTask;
                };
            });

            services.AddLogging();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.AddMvc();


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            HeaderPolicyCollection policyCollection = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders()
                .AddCustomHeader("Access-Control-Allow-Origin", "*");

            app.UseCustomHeadersMiddleware(policyCollection);

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    "Default",
                //    "api/{controller}/{action}/{id?}"
                //);
            });

            


        }
    }
}
