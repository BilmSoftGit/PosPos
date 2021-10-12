using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pospos.AdminUI.Helpers;
using Pospos.Business;
using Pospos.Business.Factories;
using Pospos.Core;
using Pospos.Core.Caching.Redis;
using Pospos.Core.Helpers;
using Pospos.Core.Modules;
using Pospos.Core.Settings;
using Pospos.Data;
using Pospos.Data.Repositories;
using Pospos.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Pospos.AdminUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
        new CultureInfo("tr"),
        new CultureInfo("en")
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            //Session kullanýmý için...
            services.AddSession();
            //Session süre yönetimi için...
            services.AddDistributedMemoryCache();

            //Google Recapthca için...
            services.AddHttpClient();

            var _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var jsonFileName = $"appsettings.{_env}.json";
            if (_env == null || _env == "Development")
            {
                jsonFileName = "appsettings.json";
            }
            this.Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(jsonFileName).Build();

            services.AddControllers().AddJsonOptions(
                options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                }).ConfigureApiBehaviorOptions(opt =>
                {
                    opt.SuppressModelStateInvalidFilter = true;
                });

            AppSettings settings = new AppSettings();
            ConnectionConfig _connections = new ConnectionConfig();
            Configuration.GetSection("AppSettings").Bind(settings);
            Configuration.GetSection("ConnectionConfig").Bind(_connections);


            services.Add(new ServiceDescriptor(typeof(AppSettings), settings));
            services.Add(new ServiceDescriptor(typeof(ConnectionConfig), _connections));
            //services.AddSingleton<ILogHelper, LogHelper>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HtmlRequestUrl>();
            //services.AddSingleton<MainConnectionManager>();
            //services.AddSingleton<Utility>();
            //services.AddSingleton<SecurityHelper>();
            //services.AddSingleton<RedisContext>();
            //services.AddSingleton<RedisCache>();
            //services.AddSingleton<CacheManager>();
            //services.AddSingleton<LogManager>();

            // services.AddSingleton<UserRepository>();
            //services.AddSingleton<CommonRepository>();

            //services.AddSingleton<UserFactory>();
            //services.AddSingleton<BankFactory>();
            //services.AddSingleton<UserService>();
            //services.AddSingleton<CommonFactory>();
            //services.AddSingleton<CommonService>();
            //services.AddSingleton<CacheFactory>();

            //services.AddSingleton<PaymentProcessRepository>();
            //services.AddSingleton<PaymentProcessService>();
            //services.AddSingleton<PaymentProcessFactory>();

            services.AddCore();
            services.AddRepositories();
            services.AddServices();
            services.AddFactories();
            services.AddSingleton<IRecaptchaValidator, RecaptchaValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Session kullanýmlarý için...
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
