using Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tickest", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando o esquema Bearer.
                      Digite 'Bearer [espaço] e então seu token no campo abaixo.
                      Exemplo: 'Bearer abcdef12345'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
        },
        new List<String>()
        }
    });
});


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
