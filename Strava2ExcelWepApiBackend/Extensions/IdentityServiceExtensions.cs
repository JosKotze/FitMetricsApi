using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Strava2ExcelWebApiBackend.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdendityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Jwt:Issuer"] ?? "FitMetricsProd",
                    ValidAudience = config["Jwt:Audience"] ?? "FitMetricsAPIProd"
                };

                // Add debug logging for authentication failures
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Authentication failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated successfully");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}