using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Tickest.Infrastructure;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Mvc.Middlewares;
using Tickest.Infrastructure.Services.Auth;
using Tickest.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Carregar configuração do JWT
var jwtConfig = builder.Configuration.GetSection("JwtConfiguracao").Get<JwtConfiguracao>();

// Configurar autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // Validar o emissor do token
        ValidateAudience = true,  // Validar o público do token
        ValidateLifetime = true,  // Validar a expiração do token
        ValidateIssuerSigningKey = true,  // Validar a chave de assinatura do token
        ValidIssuer = jwtConfig.Emissor,  // Configurar o emissor válido
        ValidAudience = jwtConfig.Audiencia,  // Configurar o público válido
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.ChaveSecreta))  // Configurar a chave secreta
    };
});

// Adicionar outros serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, IAuthService>();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("DefaultPolicy", builder =>
        {
            builder.WithOrigins("http://localhost:4200")  // Permitir origens específicas
                .AllowAnyHeader()  // Permitir qualquer cabeçalho
                .AllowAnyMethod();  // Permitir qualquer método
        });
    });

// Configurar o Serilog para logging
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configurar o CORS
app.UseCors("DefaultPolicy");

// Configurar o Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicionar middleware para tratamento de erros
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configurar logging de requisições com Serilog
app.UseSerilogRequestLogging();

// Configurar redirecionamento para HTTPS
app.UseHttpsRedirection();

// Adicionar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
