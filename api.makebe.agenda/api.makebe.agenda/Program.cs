using api.makebe.agenda.infra.crosscutting.ioc.Domains;
using api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Repositorys;
using api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
InfraRepositoryBootstrapper.Initialize(builder.Services);
RepositoryBootstrapper.Initialize(builder.Services);
InfraServiceBootstrapper.Initialize(builder.Services);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
