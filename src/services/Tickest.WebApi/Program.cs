using Serilog;
using Tickest.Application;
using Tickest.Infrastructure;
using Tickest.Infrastructure.Authentication;
using Tickest.Persistence;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddApplication()
               .AddInfrastructure(builder.Configuration)
               .AddPersistence(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseExceptionHandler();
app.UseRequestContextLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors(PresentationExtensions.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
