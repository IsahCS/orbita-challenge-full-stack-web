using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI.Data;
using StudentEnrollmentAPI.Repositories;
using StudentEnrollmentAPI.Services;
using StudentEnrollmentAPI.Mappings;
using StudentEnrollmentAPI.Validators;
using StudentEnrollmentAPI.DTOs;
using FluentValidation;

namespace StudentEnrollmentAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IValidator<StudentCreateDto>, StudentCreateDtoValidator>();
            services.AddScoped<IValidator<StudentUpdateDto>, StudentUpdateDtoValidator>();
            
            return services;
        }
        
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseProvider = configuration.GetValue<string>("DatabaseProvider");

            switch (databaseProvider?.ToLower())
            {
                case "sqlserver":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString));
                    break;
                
                case "mysql":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
                    break;
                
                case "postgresql":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(connectionString));
                    break;
                
                case "sqlite":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlite(connectionString));
                    break;
                
                case "inmemory":
                default:
                    //mantém InMemory como padrão para desenvolvimento
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase(connectionString ?? "StudentEnrollmentDB"));
                    break;
            }
            
            return services;
        }
    }
}
