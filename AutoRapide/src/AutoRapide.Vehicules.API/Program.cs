using System.Reflection;
using System.Text.Json.Serialization;
using AutoRapide.Vehicules.API.Data;
using AutoRapide.Vehicules.API.Entities;
using AutoRapide.Vehicules.API.Interfaces;
using AutoRapide.Vehicules.API.Repositories;
using AutoRapide.Vehicules.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<VehiculeContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAsyncRepository<Vehicule>, VehiculeRepository>();
builder.Services.AddScoped<IVehiculeService, VehiculeService>();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AutoRapide Véhicules API",
        Description = "Une API pour permettre de gérer une base de données de véhicules.",
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

app.Run();
