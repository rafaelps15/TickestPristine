using Application;
using Serilog;
using Tickest.Domain.Interfaces;
using Tickest.Infrastructure;
using Tickest.Infrastructure.Authentication;
using Tickest.Infrastructure.Mvc.Middlewares;
using Tickest.Persistence;
using Tickest.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Carregar a configuração do JWT e registrá-la para injeção de dependência usando IOptions
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Adiciona o IHttpContextAccessor para acessar o contexto HTTP
builder.Services.AddHttpContextAccessor();

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

//// Chama o DatabaseSeeder para inicializar os dados
//using (var scope = app.Services.CreateScope())
//{
//    var dataBaseSeeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
//    dataBaseSeeder.Seed();
//} 

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

using (var scope = app.Services.CreateScope())
{
    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        await seeder.SeedAsync(CancellationToken.None);
    }
    catch (Exception ex)
    {

        Log.Error(ex, "Erro durante o seeding de dados.");
    }

}

app.Run(); // Inicia a aplicação
