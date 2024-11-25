using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SubsidyReconciliation.Data;
using SubsidyReconciliation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsidyReconciliation
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
            AppConfigurationHelper.ApplicationName = this.Configuration.GetSection("AppSettings").GetSection("ApplicationName").Value;
            AppConfigurationHelper.BaseAddress = this.Configuration.GetSection("AppSettings").GetSection("BaseAddress").Value;
            AppConfigurationHelper.ByPass = this.Configuration.GetSection("AppSettings").GetSection("ByPass").Value;
            AppConfigurationHelper.CompanyName = this.Configuration.GetSection("AppSettings").GetSection("CompanyName").Value;
            AppConfigurationHelper.CompanyWebsite = this.Configuration.GetSection("AppSettings").GetSection("CompanyWebsite").Value;
            //AppConfigurationHelper.TokenUrl = this.Configuration.GetSection("OAuthSettings").GetSection("TokenUrl").Value;

            services.AddDbContext<ApplicationDbContext>((Action<DbContextOptionsBuilder>)(options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.CommandTimeout(30000))));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Login";
        });
            //services.AddSingleton<EmailService>();
            services.AddControllersWithViews();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddSession((Action<SessionOptions>)(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(35.0);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }));
            services.AddDistributedMemoryCache();
            services.AddControllers().AddNewtonsoftJson();
            services.AddAuthorization();

            services.AddHttpContextAccessor(); // Add this line to use IHttpContextAccessor
            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            services.AddHttpClient();


            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            //services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseSession();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });


        }
    }
}
