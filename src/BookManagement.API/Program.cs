using Asp.Versioning;
using BookManagement.API.Middlewares;
using BookManagement.API.Modules.Books;
using BookManagement.API.Modules.History;
using BookManagement.API.Shared.Configurations;
using BookManagement.API.Shared.Events;

namespace BookManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
         .AddApiVersioning(options =>
         {
             options.DefaultApiVersion = new ApiVersion(1, 0);
             options.AssumeDefaultVersionWhenUnspecified = true;
             options.ReportApiVersions = true;
             options.ApiVersionReader = ApiVersionReader.Combine(
                 new QueryStringApiVersionReader("api-version"),
                 new UrlSegmentApiVersionReader());
         })
         .AddApiExplorer(options =>
         {
             options.GroupNameFormat = "'v'VVV";
             options.SubstituteApiVersionInUrl = true;
         });

            // Core framework services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration();
            // Modules
            builder.Services
                .AddBooksModule(builder.Configuration)
                .AddHistoryModule(builder.Configuration);

            // Event bus (shared)
            builder.Services.AddScoped<IEventBus, InMemoryEventBus>();

            var app = builder.Build();

            app.UseSwaggerConfiguration();

            // Middleware
            app.UseGlobalExceptionHandling();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}
