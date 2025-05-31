using Application;
using Serilog;
using Tickest.Infrastructure;
using Tickest.Infrastructure.Authentication;
using Tickest.Infrastructure.Mvc.Middlewares;
using Tickest.Persistence;
using Tickest.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Configurar JWT e outros serviços
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar serviços customizados via extensões (Application, Infrastructure, Persistence)
builder.Services.AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddPersistence(builder.Configuration);

// Configurar CORS para frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:4200")
                     .AllowAnyHeader()
                     .AllowAnyMethod());
});

// Configurar Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

await app.Services.RunDatabaseSeedingAsync();

// Middleware
app.UseCors("DefaultPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
