using Microsoft.EntityFrameworkCore;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Services;
using Sowing_O2.Repositories;
using static Sowing_O2.Utilities.Encriptacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISecurityService, SecurityService>();


// Registrar el contexto de la base de datos
builder.Services.AddDbContext<SowingO2PruebaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar UsuarioRepositories y UsuarioService
builder.Services.AddScoped<UsuarioRepositories>();
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sowing O2 API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
