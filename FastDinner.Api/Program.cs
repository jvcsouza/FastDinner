using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FastDinner.Infrastructure;
using FastDinner.Application;
using FastDinner.Api.Middleware;
using FastDinner.Application.Common.Interfaces.Services;
using FastDinner.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDependencyResolver, DependencyResolver>(
    options => new DependencyResolver(options.CreateScope()));

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler("/error");

    app.UseMiddleware<ApplicationMiddleware>();
    app.UseMiddleware<AuthenticationMiddleware>();
    app.UseWelcomePage("/");

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}


