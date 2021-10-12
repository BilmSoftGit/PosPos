using Microsoft.Extensions.DependencyInjection;
using Pospos.Business.Factories;

namespace Pospos.Business
{
    public static class BusinessIOC
    {
        public static void AddFactories(this IServiceCollection services)
        {
            services.AddScoped<BankFactory>();
            services.AddScoped<CacheFactory>();
            services.AddScoped<CommonFactory>();
            services.AddScoped<PaymentProcessFactory>();
            services.AddScoped<UserFactory>();
            services.AddScoped<PaymentTypeFactory>();
            services.AddScoped<PosFactory>();
            services.AddScoped<StationFactory>();
            services.AddScoped<PaymentProcessCancelFactory>();
        }
    }
}
