using Microsoft.EntityFrameworkCore;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Services;
using Sowing_O2.Repositories;
using static Sowing_O2.Utilities.Encriptacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sowing_O2.Dtos;
using Sowing_O2.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISecurityService, SecurityService>();

builder.Services.AddDbContext<SowingO2PruebaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<UsuarioRepositories>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<TokenRevocadoService>();
builder.Services.AddScoped<TokenRevocadoRepositories>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<RecuperacionTokenRepository>();
builder.Services.AddScoped<RecuperarContrasenaService>();



var bindJwtSettings = new JwtSettingsDto();
builder.Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
builder.Services.AddSingleton(bindJwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
        ValidateIssuer = bindJwtSettings.ValidateIssuer,
        ValidIssuer = bindJwtSettings.ValidIssuer,
        ValidateAudience = bindJwtSettings.ValidateAudience,
        ValidAudience = bindJwtSettings.ValidAudience,
        RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
        ValidateLifetime = bindJwtSettings.ValidateLifetime,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sowing O2 API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

