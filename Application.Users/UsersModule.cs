using Application.Abstractions;
using Application.Shared.Database;
using Application.Users.Data;
using Application.Users.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
    public class UsersModule : IModule
    {
        public string ModuleName => "Users";
        private string _schemaName = "users";

        public void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            _schemaName = configuration.GetSection("DatabaseSchemas") ? ["Users"] ?? "users";

            var baseConnectionString = configuration.GetConnectionString("PostgresConnection");

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(baseConnectionString);
            connectionStringBuilder.SearchPath = _schemaName;
            var connectionString = connectionStringBuilder.ToString();

            Console.WriteLine($"[Users] Connection string: {connectionString}");

            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", _schemaName);
                    npgsqlOptions.MigrationsAssembly(typeof(UsersDbContext).Assembly.FullName);
                });
            });

            services.AddScoped<IUserProfileService, UserProfileService>();
        }

        public void UseModule(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
