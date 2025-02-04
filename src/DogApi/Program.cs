using DogApi.Endpoints.Authentication.Config;
using DogApi.Endpoints.Authentication.Services;
using DogApi.Endpoints.Breeds.Validators;
using FastEndpoints.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace DogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Configure Serilog
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            builder.Logging.ClearProviders(); // Clear default logging providers
            builder.Host.UseSerilog(); // Register Serilog

            try
            {
                Log.Information("Starting Dog API");
               
                // Add services to the container.
                builder.Services.AddControllers();

                // Register HttpClient for API calls
                builder.Services.AddHttpClient();

                // Register FastEndpoints
                builder.Services.AddFastEndpoints();

                builder.Services.SwaggerDocument(config =>
                {
                    config.DocumentSettings = s =>
                    {
                        s.Title = "Dog API Microservice";
                        s.Version = "v1";
                        s.Description = "An API to manage dog breeds and related information. \n" +
                                       "For more information, visit [Dog API](https://dog.ceo/dog-api/) and " +
                                       "check out the [GitHub repository](https://github.com/dog-api) for source code.";
                       s.DocumentName = "DogAPI V1";
                    };
                });

                // Load JWT Settings
                builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret))
                    };
                });

                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("Authenticated", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });

                    options.AddPolicy("AdminOnly", policy =>
                    {
                        policy.RequireRole("Admin");
                    });
                });

                builder.Services.AddSingleton<UserStore>();

                builder.Services.AddSingleton<IFlagsmithClient>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    var apiKey = configuration["Flagsmith:EnvironmentApiKey"];

                    return new FlagsmithClient(apiKey);
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseOpenApi();
                    app.UseSwaggerUi(s => s.ConfigureDefaults());
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                // Use FastEndpoints middleware
                app.UseFastEndpoints()
                    .UseSwaggerGen();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Dog API terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
