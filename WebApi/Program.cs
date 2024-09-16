using Application;
using Serilog;
using Tickest.Domain.Interfaces;
using Tickest.Infrastructure;
using Tickest.Infrastructure.Mvc.Middlewares;
using Tickest.Infrastructure.Services;
using Tickest.Persistence;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IKeyVaultService, KeyVaultService>();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("DefaultPolicy", builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseCors("DefaultPolicy");

if (app.Environment.IsDevelopment())
    app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

