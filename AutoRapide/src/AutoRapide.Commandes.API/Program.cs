using System.Reflection;
using AutoRapide.Commandes.API.Data;
using AutoRapide.Commandes.API.Entities;
using AutoRapide.Commandes.API.Interfaces;
using AutoRapide.Commandes.API.Repositories;
using AutoRapide.Commandes.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CommandeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAsyncRepository<Commande>, CommandeRepository>();
builder.Services.AddScoped<ICommandeService, CommandeService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AutoRapide Commandes API",
        Description = "Une API pour permettre de gérer une base de données de commandes de véhicules..",
        Contact = new OpenApiContact
        {
            Name = "Pier-Olivier St-Pierre-Chouinard",
            Url = new Uri("https://github.com/polivierstpch"),
            Email = "pier.olivier.stpch@gmail.com"
        }
        
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<CommandeContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Une erreur est survenue lors de l'initialisation de la base de données");
    }
}

app.Run();
