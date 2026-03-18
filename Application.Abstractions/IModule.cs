using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Application.Abstractions
{
    public interface IModule
    {
        string ModuleName { get; }
        void RegisterServices(IServiceCollection services, IConfiguration configuration);
        Task UseModuleAsync(IApplicationBuilder app, IWebHostEnvironment env);
    }

    public interface IIdentityModule: IModule {}

    public abstract class ModuleWithDbContext<TDbContext> : IModule
        where TDbContext : DbContext
    {
        public abstract string ModuleName { get; }
        protected abstract string SchemaName { get; }

        public virtual void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            var schema = configuration.GetSection("DatabaseSchemas")?[ModuleName] ?? SchemaName;

            var baseConnectionString = configuration.GetConnectionString("PostgresConnection");

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(baseConnectionString);
            connectionStringBuilder.SearchPath = schema;
            var connectionString = connectionStringBuilder.ToString();

            Console.WriteLine($"[{ModuleName}] Подключение: {connectionString}");

            // Регистрация DbContext
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", schema);
                    npgsqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
                });
            });

            // Дополнительные сервисы модуля
            RegisterModuleServices(services, configuration);
        }

        public virtual async Task UseModuleAsync(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            dbContext.Database.Migrate();

            await SeedDataAsync(scope.ServiceProvider);
        }

        protected abstract void RegisterModuleServices(IServiceCollection services, IConfiguration configuration);

        protected virtual async Task SeedDataAsync(IServiceProvider serviceProvider) { }
    }
}
