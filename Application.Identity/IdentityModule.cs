using Application.Abstractions;
using Application.Identity.Data;
using Application.Identity.Models;
using Application.Identity.Services;
using Application.Shared.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;
using System.Text;

namespace Application.Identity
{
    public class IdentityModule : IModule
    {
        public string ModuleName => "Identity";
        private string _schemaName = "identity";

        public void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            _schemaName = configuration.GetSection("DatabaseSchemas")?["Identity"] ?? "identity";

            var baseConnectionString = configuration.GetConnectionString("PostgresConnection");

            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(baseConnectionString);
            connectionStringBuilder.SearchPath = _schemaName;
            var connectionString = connectionStringBuilder.ToString();

            Console.WriteLine($"[Identity] Connection string: {connectionString}");

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", _schemaName);
                    npgsqlOptions.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName);
                });
            });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization();
        }

        public void UseModule(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            dbContext.Database.Migrate();

            SeedData(dbContext).GetAwaiter().GetResult();
        }

        private async Task SeedData(IdentityDbContext dbContext)
        {

            if (await dbContext.Roles.AnyAsync())
            {
                return;
            }

            string adminRoleName = "Admin";

            var roles = new[]
            {
                new Role { Id = Guid.NewGuid(), Name = adminRoleName, Description = "Администратор" },
                new Role { Id = Guid.NewGuid(), Name = "User", Description = "Обычный пользователь" }
            };

            await dbContext.Roles.AddRangeAsync(roles);

            var permissions = new[]
            {
                new Permission { Id = Guid.NewGuid(), Name = "users.view", Description = "Просмотр пользователя" },
                new Permission { Id = Guid.NewGuid(), Name = "users.create", Description = "Создание пользователя" }
            };

            await dbContext.Permissions.AddRangeAsync(permissions);

            var adminRole = roles.Where(r => r.Name == adminRoleName).FirstOrDefault();

            foreach(var permission in permissions)
            {
                await dbContext.RolePermissions.AddAsync(new RolePermission
                {
                    RoleId = adminRole.Id,
                    PermissionId = permission.Id
                });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
