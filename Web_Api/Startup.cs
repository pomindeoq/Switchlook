using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using NetEscapades.AspNetCore.SecurityHeaders;
using WebApi.Mangers;
using WebApi.Models;
using WebApi.Models.Accounts;

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
            var sqlConnectionString = "server=164.132.233.40;userid=switchlook_db;password=teoy3RroLKqqWpm0;database=switchlook_dev;";
            services.AddDbContext<WebApiDataContext>(options =>
                options.UseMySql(
                    sqlConnectionString
                )
            );

            // Identity
            services.AddIdentity<Account, IdentityRole>()
                .AddEntityFrameworkStores<WebApiDataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;

                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };

                });

            services.AddCustomHeaders();

            services.ConfigureApplicationCookie(options =>
            {
                var protectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"c:\shared-auth-ticket-keys\"));
                options.DataProtectionProvider = protectionProvider;
                options.TicketDataFormat = new TicketDataFormat(protectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "Cookies", "v2"));
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;

                    return Task.CompletedTask;
                };
            });

            services.AddTransient<PointManager>();

            services.AddLogging();

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
