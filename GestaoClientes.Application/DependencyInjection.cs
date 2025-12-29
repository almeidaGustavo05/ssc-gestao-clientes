using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GestaoClientes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
