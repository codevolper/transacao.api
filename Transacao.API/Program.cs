using Microsoft.OpenApi.Models;
using Transacao.API.Infrastructure.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("AuthApi", client =>
{
    var baseUrl = builder.Configuration["AuthApi:BaseUrl"] ?? "https://localhost:5001";
    client.BaseAddress = new Uri(baseUrl);
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Injeção de dependências
builder.Services.AddScoped<IValidadorTokenRemoto, ValidadorTokenRemoto>();
builder.Services.AddScoped<Transacao.API.API.Filters.ValidarTokenAttribute>();
builder.Services.AddScoped<Transacao.API.Application.UseCases.IProcessarPagamentoUseCase, Transacao.API.Application.UseCases.ProcessarPagamentoUseCase>();

builder.Services.AddSingleton<Transacao.API.Domain.Interfaces.ITransacaoRepository, Transacao.API.Infrastructure.Persistence.InMemoryTransacaoRepository>();

builder.Services.AddSwaggerGen(s =>
{

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHttpClient("AuthApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"] ?? "https://localhost:60285/");
});

builder.Services.AddHttpClient<Transacao.API.Infrastructure.Clients.IAuthClient, Transacao.API.Infrastructure.Clients.AuthClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"] ?? "https://localhost:60285/");
});

builder.Services.AddHttpClient<Transacao.API.Infrastructure.Clients.IClienteClient, Transacao.API.Infrastructure.Clients.ClienteClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ClienteApi:BaseUrl"] ?? "https://localhost:7175/");
});

// Registrar gateway que delega ao cliente HTTP
builder.Services.AddScoped<Transacao.API.Infrastructure.Gateways.IClienteGateway, Transacao.API.Infrastructure.Gateways.ClienteGateway>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Pagamentos v1");
    c.RoutePrefix = string.Empty; // roda na raiz (/index.html)
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
