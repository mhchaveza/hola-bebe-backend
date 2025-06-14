using HolaBebe.Application.Mapping;
using HolaBebe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

MappingConfig.Register();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hola Bebe API");

app.Run();
