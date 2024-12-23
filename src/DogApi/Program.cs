using DogApi.Endpoints.Authentication.Config;
using DogApi.Endpoints.Authentication.Services;
using DogApi.Endpoints.Breeds.Validators;
using FastEndpoints.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(builder.Configuration)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .WriteTo.Seq(builder.Configuration["Serilog:WriteTo:1:Args:serverUrl"])
            //    .CreateLogger();
            builder.Host.UseSerilog((context, configuration) =>
                configuration.WriteTo.Debug()
                .MinimumLevel.Information()
            );

            try
            {
                Log.Information("Starting Dog API");
               
                // Add services to the container.
                builder.Services.AddControllers();

                // Register HttpClient for API calls
                builder.Services.AddHttpClient();

                // Register FastEndpoints
                builder.Services.AddFastEndpoints();

                builder.Services.SwaggerDocument();

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

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwaggerGen();
                    app.UseSwaggerUi();
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                // Use FastEndpoints middleware
                app.UseFastEndpoints();

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
