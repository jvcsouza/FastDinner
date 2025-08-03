using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using FastDinner.Infrastructure;
using FastDinner.Application;
using FastDinner.Api.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using FastDinner.Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new StructureMapProvider());

//builder.Services.AddSingleton<IDependencyResolver, DependencyResolver>(
//    options => new DependencyResolver(options.CreateScope()));

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.OperationFilter<AddRequiredHeaderParameter>();
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() { In = ParameterLocation.Header });

});

var app = builder.Build();
{
    //if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler("/error");

    app.UseMiddleware<ApplicationMiddleware>();
    app.UseMiddleware<AuthenticationMiddleware>();
    app.UseWelcomePage("/");
    //app.UseWelcomePage("/wwwroot/browser");

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    //app.UseDefaultFiles();
    //app.UseStaticFiles();

    app.MapControllers();

    //app.MapFallbackToFile("index.html");

    //DependencyResolver.InitializeIoc(app.Services);

    app.Run();
}

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-restaurant-id",
            In = ParameterLocation.Header,
            Required = true,
            Description = "Restaurante to connect",
        });

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-tenant-name",
            In = ParameterLocation.Header,
            Required = false,
            Description = "Tenant to connect",
        });
    }
}
