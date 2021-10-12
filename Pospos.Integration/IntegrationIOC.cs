using Microsoft.Extensions.DependencyInjection;

namespace Pospos.Integration
{
    public static class IntegrationIOC
    {
        public static void AddIntegration(this IServiceCollection services)
        {
            services.AddSingleton<ESTIntegrator>();
            services.AddSingleton<GarantiIntegrator>();
            services.AddSingleton<YapiKrediIntegrator>();
        }
    }
}
