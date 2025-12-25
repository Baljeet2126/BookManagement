using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BookManagement.API.Shared.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.UseAllOfToExtendReferenceSchemas();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BookManagement API",
                    Version = "v1.0",
                    Description = "Book Management API (v1.0)",
                })
                ;
            });

            
        }

        public static void UseSwaggerConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Management API v1.0");
                    c.RoutePrefix = string.Empty;
                });
            }
        }
    }
}
