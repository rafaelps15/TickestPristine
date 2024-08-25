using Serilog;
using Application;
using Tickest.Infrastructure.Mvc.Middlewares;
using Tickest.Persistence;
using Tickest.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

