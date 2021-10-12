using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pospos.Api.Filters;
using Pospos.Business;
using Pospos.Core;
using Pospos.Core.Caching.Redis;
using Pospos.Core.Helpers;
using Pospos.Core.Modules;
using Pospos.Core.Settings;
using Pospos.Data;
using Pospos.Data.Repositories;
using Pospos.Integration;
using Pospos.Service;
using System;
using System.IO;

namespace PosPos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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


            InjectClasses(services);

            services.AddAuthentication(option =>
            {
                option.DefaultScheme = "CustomScheme";
                option.DefaultAuthenticateScheme = "CustomScheme";
            }).AddScheme<CustomAuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", null);
            services.AddScoped<CustomAuthenticationHandler>();
            services.AddScoped<ExceptionHandlerFilterAttribute>();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo() { Title = "Pospos Payment Api", Version = "v1", Description = "this service contains all payment endpoints" });
                s.CustomSchemaIds(x => x.FullName);
                s.AddSecurityDefinition("token", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Insert Jwt token into field",
                    Name = "token",
                    Type = SecuritySchemeType.ApiKey
                });
                //s.OperationFilter<RequiredHeaderParameterFilter>();
                s.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "token"
                        }
                    },new string[]{ }
                }
                });
            });
        }

        private void InjectClasses(IServiceCollection services)
        {
            AppSettings settings = new AppSettings();
            ConnectionConfig _connections = new ConnectionConfig();
            Configuration.GetSection("AppSettings").Bind(settings);
            Configuration.GetSection("ConnectionConfig").Bind(_connections);

            services.Add(new ServiceDescriptor(typeof(AppSettings), settings));
            services.Add(new ServiceDescriptor(typeof(ConnectionConfig), _connections));
            services.AddCore();
            services.AddIntegration();
            services.AddServices();
            services.AddRepositories();
            services.AddFactories();


            services.AddSingleton<TraceLogMiddleWare>();

            //#region repositories

            //services.AddScoped<UserRepository>();
            //services.AddScoped<BankCardBinRepository>();
            //services.AddScoped<BankPosTypeRepository>();
            //services.AddScoped<BankRepository>();
            //services.AddScoped<InstallmentRepository>();
            //services.AddScoped<PaymentErrorRepository>();
            //services.AddScoped<PaymentProcessRepository>();
            //services.AddScoped<PaymentTypeRepository>();
            //services.AddScoped<PosRepository>();
            //services.AddScoped<PosTypeRepository>();
            //services.AddScoped<SettingRepository>();
            //services.AddScoped<StationPosRepository>();
            //services.AddScoped<StationRepository>();

            //#endregion

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            //app.UseTraceLogMiddleware();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Pospos Api");
            });
            //app.UseHttpsRedirection();
            app.UseMiddleware<TraceLogMiddleWare>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
