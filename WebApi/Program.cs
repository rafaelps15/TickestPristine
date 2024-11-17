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

// Carregar configuração do JWT
var jwtConfig = builder.Configuration.GetSection("JwtConfiguracao").Get<JwtConfiguration>();
builder.Services.AddSingleton(jwtConfig); // Adicionando a configuração do JWT ao contêiner

// Configurar autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    // Usando a configuração do JWT para validar o token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),// Utilizando a chave secreta do JWT
        ClockSkew = TimeSpan.Zero // Remove atraso padrão de 5 min para validação do token
    };
});

// Adicionar outros serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

// Adicionar a infraestrutura e a persistência
builder.Services.AddApplication()
               .AddInfrastructure(builder.Configuration)
               .AddPersistence(builder.Configuration);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200")// Configurando origem para o frontend
               .AllowAnyHeader()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Configurar o Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configurar middleware
app.UseCors("DefaultPolicy");
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger no ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de tratamento de erros
app.UseMiddleware<ErrorHandlerMiddleware>();

// Logs das requisições com Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();// Redirecionar para HTTPS
app.UseAuthentication();// Ativar autenticação JWT
app.UseAuthorization();// Habilitar autorização

app.MapControllers();// Mapeia os controladores

app.Run();
