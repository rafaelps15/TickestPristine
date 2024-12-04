using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Tickest.Infrastructure;
using Tickest.Infrastructure.Configurations;
using Tickest.Infrastructure.Mvc.Middlewares;
using Tickest.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o IHttpContextAccessor para acessar o contexto HTTP
builder.Services.AddHttpContextAccessor();

// Carregar configuração do JWT
var jwtConfig = builder.Configuration.GetSection("JwtConfiguracao").Get<JwtConfiguration>();
builder.Services.AddSingleton(jwtConfig);

// Configurar autenticação JWT usando o novo modelo
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
            ClockSkew = TimeSpan.Zero // Remove o atraso padrão de 5 minutos
        };
    });

// Adiciona outros serviços e configurações de controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona infraestrutura e persistência de dados
builder.Services.AddApplication()
               .AddInfrastructure(builder.Configuration)
               .AddPersistence(builder.Configuration);

// Configuração de CORS para permitir acesso do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Configura origem para o frontend
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Configuração do Serilog para logging de requisições e erro
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configurações de middleware
app.UseCors("DefaultPolicy");

if (app.Environment.IsDevelopment())
{
    // Habilita o Swagger para documentação de API no ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de tratamento de erros
app.UseMiddleware<ErrorHandlerMiddleware>();

// Log das requisições usando Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection(); // Força redirecionamento para HTTPS
app.UseAuthentication();   // Ativa a autenticação com JWT
app.UseAuthorization();    // Habilita a autorização para rotas protegidas

app.MapControllers(); // Mapeia os controladores

app.Run(); // Inicia a aplicação
