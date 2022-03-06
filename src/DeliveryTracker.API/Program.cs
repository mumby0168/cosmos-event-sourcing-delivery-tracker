using CleanArchitecture.Exceptions.AspNetCore;
using DeliveryTracker.API.Endpoints;
using DeliveryTracker.Application.Extensions;
using DeliveryTracker.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options => options.CustomSchemaIds(type => type.FullName));

builder.Services
    .AddApplication()
    .AddInfrastructure();

builder.Services.AddCleanArchitectureExceptionsHandler(options =>
    options.ApplicationName = "DeliveryTracker");

var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI();

app.UseCleanArchitectureExceptionsHandler();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapScheduleAPIEndpoints();

app.Run();