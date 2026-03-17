using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace Application.Abstractions
{
    public interface IModule
    {
        string ModuleName { get; }
        void RegisterServices(IServiceCollection services, IConfiguration configuration);
        void UseModule(IApplicationBuilder app, IWebHostEnvironment env);
    }

    public interface IIdentityModule: IModule {}
}
