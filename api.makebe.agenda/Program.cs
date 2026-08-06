using api.makebe.agenda.applications.Meddleweres;
using api.makebe.agenda.Configurations;
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
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var urls = builder.Configuration["Urls"] ?? "http://localhost:5222";
builder.WebHost.UseUrls(urls);
builder.Logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.None);
builder.Services.InitializeInfraRepositoryBootstrapper();
builder.Services.InitializeRepositoryBootstrapper();
builder.Services.InitializeDataBootstrapper();
builder.Services.InitializeApplicationsAutomapperBootstrapper();
builder.Services.InitializeServicesBootstrapper(builder.Configuration);
builder.Services.InitializeDomainServiceBootstrapper();
builder.Services.InitializeInfraServiceCrossCuttingBootstrapper();
builder.Services.InitializeInfraEventBootstrapper();
builder.Services.InitializeLibDependencyInjection();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ApiSecurityOptions>(
    builder.Configuration.GetSection(ApiSecurityOptions.SectionName));


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

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100000000;

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
app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\":\"Erro interno no servidor.\"}");
    });
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<LogResponseMiddleware>();
app.UseMiddleware<ApiSecurityMiddleware>();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
