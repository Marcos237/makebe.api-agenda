using api.makebe.agenda.applications.Meddleweres;
using api.makebe.agenda.infra.crosscutting.ioc.Applications;
using api.makebe.agenda.infra.crosscutting.ioc.Data;
using api.makebe.agenda.infra.crosscutting.ioc.Domains;
using api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Repositorys;
using api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Services;
using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Events;
using lib.makebe.Applications.IOC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:80");
builder.Services.InitializeInfraRepositoryBootstrapper();
builder.Services.InitializeRepositoryBootstrapper();
builder.Services.InitializeDataBootstrapper();
builder.Services.InitializeApplicationsAutomapperBootstrapper();
builder.Services.InitializeServicesBootstrapper();
builder.Services.InitializeDomainServiceBootstrapper();
builder.Services.InitializeInfraServiceCrossCuttingBootstrapper();
builder.Services.InitializeInfraEventBootstrapper();
builder.Services.InitializeLibDependencyInjection();


var chave = builder.Configuration["SysKey"]!.ToString();
var key = Encoding.ASCII.GetBytes(chave);
var url = builder.Configuration["urlMakebe"] ?? "https://makebe2.ddns.net";


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<NotificationFilter>();
});

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy",
        policy =>
        {
            policy.WithOrigins(url)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });

    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProductionPolicy");
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseMiddleware<LogResponseMiddleware>();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler("/Error");
app.Run();