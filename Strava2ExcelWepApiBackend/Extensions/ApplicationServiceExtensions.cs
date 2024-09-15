using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Data;
using Strava2ExcelWebApiBackend.Interfaces;
using Strava2ExcelWebApiBackend.Models;
using Strava2ExcelWebApiBackend.Services;

namespace Strava2ExcelWebApiBackend.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
     IConfiguration config)
        {

            services.AddControllers();

            services.AddHttpClient();

            services.AddDbContext<StravaDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStravaService, StravaService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
