using Microsoft.Extensions.DependencyInjection;

namespace Pospos.Service
{
    public static class ServicesIOC
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<BankCardBinService>();
            services.AddSingleton<CommonService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<BankService>();
            services.AddSingleton<InstallmentService>();
            services.AddSingleton<PaymentProcessService>();
            services.AddSingleton<PaymentTypeService>();
            services.AddSingleton<PosService>();
            services.AddSingleton<StationService>();
            services.AddSingleton<PaymentProcessCancelService>();
        }
    }
}
