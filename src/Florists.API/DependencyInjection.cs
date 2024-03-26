using Florists.API.Common.Errors;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Reflection;

namespace Florists.API
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddPresentation(
    this IServiceCollection services,
    ConfigurationManager configuration)
    {
      #region Custom
      services.AddMapping();
      #endregion
      services.AddControllers(options =>
      {
        options.Filters.Add(new ProducesAttribute("application/json"));
        options.Filters.Add(new ConsumesAttribute("application/json"));
      });

      services.AddApiVersioning(config =>
      {
        config.ApiVersionReader = new UrlSegmentApiVersionReader();
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
      });

      services.AddSingleton<ProblemDetailsFactory, FloristsProblemDetailsFactory>();

      #region CORS
      services.AddCors(options =>
      {
        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

        options.AddDefaultPolicy(policyBuilder =>
        {
          policyBuilder
          .WithOrigins(allowedOrigins!)
          .WithHeaders("Method", "Authorization", "origin", "accept", "content-type")
          .WithMethods("GET", "POST", "PUT", "DELETE");
        });
      });
      #endregion

      return services;
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
      var config = TypeAdapterConfig.GlobalSettings;
      config.Scan(Assembly.GetExecutingAssembly());

      services.AddSingleton(config);
      services.AddScoped<IMapper, ServiceMapper>();
      return services;
    }
  }
}
