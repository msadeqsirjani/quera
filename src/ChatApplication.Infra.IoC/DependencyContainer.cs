using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using ChatApplication.Application.Constants;
using ChatApplication.Application.Exceptions;
using ChatApplication.Application.Extensions;
using ChatApplication.Application.Settings;
using ChatApplication.Domain.Common;
using ChatApplication.Infra.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ChatApplication.Infra.IoC;

public static class DependencyContainer
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = context.ModelState.Values.SelectMany(x => x.Errors).First().ErrorMessage;

                    return new UnprocessableEntityObjectResult(Result.WithException(result));
                };
            })
            .AddFluentValidation(options => options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()))
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddOptions();
        services.AddMemoryCache();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Scan(scan => 
            scan.FromCallingAssembly()                    
                .AddClasses()
                .AsMatchingInterface());

        services.Configure<JwtSetting>(configuration.GetSection("JwtSetting"));

        services.AddHttpClient();

        services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build()));

        services.AddEndpointsApiExplorer();

        var setting = new JwtSetting();

        configuration.Bind("JwtSetting", setting);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.IssuerSigningKey)),
                    ValidateIssuer = setting.ValidateIssuer,
                    ValidIssuer = setting.ValidIssuer,
                    ValidateAudience = setting.ValidateAudience,
                    ValidAudience = setting.ValidAudience,
                    RequireExpirationTime = setting.RequireExpirationTime,
                    ValidateLifetime = setting.ValidateLifetime,
                    ClockSkew = TimeSpan.FromDays(1)
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        try
                        {
                            var token = context.HttpContext.GetAuthenticationToken();

                            var handler = new JwtSecurityTokenHandler();

                            handler.ValidateToken(token, options.TokenValidationParameters, out _);

                            return Task.CompletedTask;
                        }
                        catch
                        {
                            throw new UnAuthorizedException(Statement.UnAuthorized);
                        }
                    }
                };
            });

        services.AddAuthorization();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("docs", new OpenApiInfo
            {
                Title = "Chat Application API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer xxx'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
    }

    public static void UseApplications(this WebApplication application)
    {
        application.UseSwagger();
        application.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Chat Application API Documentation";
            options.SwaggerEndpoint("/swagger/docs/swagger.json", "Chat Application API");
            options.RoutePrefix = "docs";
        });
        application.UseCors("CorsPolicy");
        application.UseAuthentication();
        application.UseAuthorization();
        application.MapControllers();
    }
}