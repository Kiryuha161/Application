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
    public class UsersModule : ModuleWithDbContext<UsersDbContext>
    {
        public override string ModuleName => "Users";
        protected override string SchemaName => "users";

        protected override void RegisterModuleServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
