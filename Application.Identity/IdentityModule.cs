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
    public class IdentityModule : ModuleWithDbContext<IdentityDbContext>
    {
        public override string ModuleName => "Identity";
        protected override string SchemaName => "identity";

        protected override void RegisterModuleServices(IServiceCollection services, IConfiguration configuration)
        {
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

        protected override async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

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
