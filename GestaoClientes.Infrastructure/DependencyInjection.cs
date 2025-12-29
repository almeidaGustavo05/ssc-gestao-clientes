using GestaoClientes.Domain.Clientes;
using GestaoClientes.Domain.Common;
using GestaoClientes.Infrastructure.Mappings;
using GestaoClientes.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace GestaoClientes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(ClienteMap).Assembly.GetExportedTypes());
            var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var cfg = new Configuration();
            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = connectionString;
                x.Driver<SQLite20Driver>();
                x.Dialect<SQLiteDialect>();
                x.LogSqlInConsole = true;
                x.LogFormattedSql = true;
            });
            cfg.AddMapping(domainMapping);

            new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg).Execute(false, true);

            var sessionFactory = cfg.BuildSessionFactory();

            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            return services;
        }
    }
}
