using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Tickest.Application.Users.CriarUsuario;
using Tickest.Infrastructure;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Interfaces;
using Tickest.Infrastructure.Mvc.Middlewares;
using Tickest.Infrastructure.Services.Auth;
using Tickest.Infrastructure.Services.Authentication;
using Tickest.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Carregar configuração do JWT
var jwtConfig = builder.Configuration.GetSection("JwtConfiguracao").Get<JwtConfiguracao>();

// Configurar autenticação JWT
ConfigureJwtAuthentication(builder.Services, jwtConfig);

// Adicionar outros serviços
ConfigureServices(builder.Services);

// Configurar CORS
ConfigureCors(builder.Services);

// Configurar o Serilog para logging
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configurar o pipeline da aplicação
ConfigureAppPipeline(app);

app.Run();

// Métodos de configuração

/// <summary>
/// Configura a autenticação JWT com os parâmetros fornecidos.
/// </summary>
/// <param name="services">Coleção de serviços a serem configurados.</param>
/// <param name="jwtConfig">Configurações do JWT.</param>
void ConfigureJwtAuthentication(IServiceCollection services, JwtConfiguracao jwtConfig)
{
    services.AddAuthentication(options =>
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
}

/// <summary>
/// Adiciona serviços ao contêiner de injeção de dependência.
/// </summary>
/// <param name="services">Coleção de serviços a serem configurados.</param>
void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Injetar dependências de serviço
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUsuarioValidator, UsuarioValidator>();

    // Adicionar módulos da aplicação
    services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPersistence(builder.Configuration);
}

/// <summary>
/// Configura o CORS (Cross-Origin Resource Sharing) para permitir requisições de origens específicas.
/// </summary>
/// <param name="services">Coleção de serviços a serem configurados.</param>
void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("DefaultPolicy", builder =>
        {
            builder.WithOrigins("http://localhost:4200")  // Permitir origens específicas
                .AllowAnyHeader()  // Permitir qualquer cabeçalho
                .AllowAnyMethod();  // Permitir qualquer método
        });
    });
}

/// <summary>
/// Configura o pipeline da aplicação, incluindo middlewares e mapeamento de controladores.
/// </summary>
/// <param name="app">Instância da aplicação ASP.NET Core.</param>
void ConfigureAppPipeline(WebApplication app)
{
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
}
