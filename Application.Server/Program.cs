using Application.Users;
using Application.Abstractions;
using Application.Shared.Database;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Application.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Приложение API", Version = "v1" });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Заголовки JWT используют Bearer схемы",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddSingleton<ISchemaManager>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                return new PostgresSchemaManager(connectionString);
            });

            var modules = DiscoverModules();
            foreach (var module in modules)
            {
                module.RegisterServices(builder.Services, builder.Configuration);
                Console.WriteLine("Зарегистрирован модуль: " + module.ModuleName);
            }

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var schemaManager = scope.ServiceProvider.GetService<ISchemaManager>();
                var schemas = builder.Configuration
                    .GetSection("DatabaseSchemas")
                    .Get<Dictionary<string, string>>()?.Values.ToArray() ?? new[] { "identity", "users", "product" };

                await schemaManager.EnsureSchemasExistAsync(schemas);
                Console.WriteLine("Все базы данных созданы");
            }

            foreach (var module in modules)
            {
                await module.UseModuleAsync(app, app.Environment);
                Console.WriteLine($"Модуль инициализирован: {module.ModuleName}");
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");

                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    return;
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static IEnumerable<IModule> DiscoverModules()
        {
            var modules = new List<IModule>();

            AddModule(new Identity.IdentityModule(), modules);
            AddModule(new Users.UsersModule(), modules);

            return modules;
        }

        private static void AddModule(IModule module, List<IModule> modules)
        {
            try
            {
                modules.Add(module);
                Console.WriteLine("Добавлен модуль: Identity");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось создать IdentityModule: {ex.Message}");
            }
        }
    }
}
