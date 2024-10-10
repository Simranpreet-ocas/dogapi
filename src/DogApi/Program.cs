using FastEndpoints.Swagger;

namespace DogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register HttpClient for API calls
            builder.Services.AddHttpClient();

            // Register FastEndpoints
            builder.Services.AddFastEndpoints();

            builder.Services.SwaggerDocument();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerGen();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Use FastEndpoints middleware
            app.UseFastEndpoints();

            app.MapControllers();

            app.Run();
        }
    }
}
