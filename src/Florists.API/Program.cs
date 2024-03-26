using Florists.API;
using Florists.Application;
using Florists.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddPresentation(builder.Configuration)
  .AddInfrastructure(builder.Configuration)
  .AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
