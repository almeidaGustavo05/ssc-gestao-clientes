using GestaoClientes.Application.Features.Clientes.Commands.CriaCliente;
using GestaoClientes.Domain.Clientes;
using GestaoClientes.Domain.Common;
using GestaoClientes.Infrastructure;
using GestaoClientes.Infrastructure.Mappings;
using GestaoClientes.Infrastructure.Repositories;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuração NHibernate (SQLite)
var mapper = new ModelMapper();
mapper.AddMappings(typeof(ClienteMap).Assembly.GetExportedTypes());
var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

var cfg = new Configuration();
cfg.DataBaseIntegration(x =>
{
    x.ConnectionString = "Data Source=gestao_clientes.db;Version=3;";
    x.Driver<SQLite20Driver>();
    x.Dialect<SQLiteDialect>();
    x.LogSqlInConsole = true;
    x.LogFormattedSql = true;
});
cfg.AddMapping(domainMapping);

new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg).Execute(false, true);

var sessionFactory = cfg.BuildSessionFactory();

builder.Services.AddSingleton(sessionFactory);
builder.Services.AddScoped(factory => sessionFactory.OpenSession());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositórios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriaClienteCommand).Assembly));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
