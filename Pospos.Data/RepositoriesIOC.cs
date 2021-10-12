using Microsoft.Extensions.DependencyInjection;
using Pospos.Data.Repositories;

namespace Pospos.Data
{
    public static class RepositoriesIOC
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<BankCardBinRepository>();
            services.AddSingleton<BankPosTypeRepository>();
            services.AddSingleton<BankRepository>();
            services.AddSingleton<CommonRepository>();
            services.AddSingleton<InstallmentRepository>();
            services.AddSingleton<PaymentErrorRepository>();
            services.AddSingleton<PaymentProcessRepository>();
            services.AddSingleton<PaymentTypeRepository>();
            services.AddSingleton<PosRepository>();
            services.AddSingleton<PosTypeRepository>();
            services.AddSingleton<SettingRepository>();
            services.AddSingleton<StationPosRepository>();
            services.AddSingleton<StationRepository>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<PosRepository>();
            services.AddSingleton<PaymentProcessCancelRepository>();
        }
    }
}
