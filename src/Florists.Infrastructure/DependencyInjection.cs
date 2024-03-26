using Dapper;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Persistence;
using Florists.Infrastructure.Policies;
using Florists.Infrastructure.Repositories;
using Florists.Infrastructure.Services;
using Florists.Infrastructure.Settings;
using Florists.Infrastructure.TypeHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Florists.Infrastructure
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      ConfigurationManager configuration)
    {
      services
        .AddPersistence(configuration)
        .AddAuth(configuration)
        .AddServices()
        .AddRepositories();
      return services;
    }

    public static IServiceCollection AddPersistence(
      this IServiceCollection services,
      ConfigurationManager configuration)
    {
      SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
      SqlMapper.RemoveTypeMap(typeof(Guid));
      SqlMapper.RemoveTypeMap(typeof(Guid?));

      var mySqlSettings = new MySqlSettings();

      configuration.Bind(
        MySqlSettings.SectionName,
        mySqlSettings);

      mySqlSettings.ConnectionString = configuration
        .GetConnectionString("MySqlConnection")!;

      services.AddSingleton(
        Options.Create(mySqlSettings));

      services.Configure<MySqlSettings>(
        configuration.GetSection(MySqlSettings.SectionName));

      services.AddSingleton<IDataAccess, DataAccess>();

      return services;
    }

    public static IServiceCollection AddRepositories(
      this IServiceCollection services)
    {
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IInventoryRepository, InventoryRepository>();
      services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
      services.AddScoped<IProductRepository, ProductRepository>();
      services.AddScoped<IProductTransactionRepository, ProductTransactionRepository>();
      return services;
    }

    public static IServiceCollection AddServices(
      this IServiceCollection services)
    {
      services.AddSingleton<IDateTimeService, DateTimeService>();
      services.AddSingleton<IPasswordService, PasswordService>();
      services.AddSingleton<ITokenService, TokenService>();

      return services;
    }

    public static IServiceCollection AddAuth(
      this IServiceCollection services,
      ConfigurationManager configuration)
    {
      var tokenSettings = new TokenSettings();

      configuration.Bind(
        TokenSettings.SectionName,
        tokenSettings);

      services.AddSingleton(
        Options.Create(tokenSettings));

      services.Configure<TokenSettings>(
        configuration.GetSection(TokenSettings.SectionName));


      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.TokenValidationParameters =
         new TokenValidationParameters()
         {
           ValidateAudience = true,
           ValidAudience = tokenSettings.Audience,
           ValidateIssuer = true,
           ValidIssuer = tokenSettings.Issuer,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenSettings.Key))
         };
      });

      services.AddAuthorization(options =>
      {
        options.AddPolicy(
          ForAdminPolicy.Name,
          policy => policy.AddForAdminPolicy());
      });

      return services;
    }
  }
}
