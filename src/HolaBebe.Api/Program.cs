using HolaBebe.Application.Mapping;
using HolaBebe.Application;
using HolaBebe.Application.Dtos;
using HolaBebe.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HolaBebe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

MappingConfig.Register();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("User"));

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

var api = app.MapGroup("/api/v1");

api.MapGet("/me/profile", async (HttpContext http, IProfileService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var profile = await svc.GetProfileAsync(userId, ct);
        return profile is null ? Results.NotFound() : Results.Ok(profile);
    })
    .WithName("GetMyProfile")
    .WithSummary("Get current user profile")
    .WithDescription("Returns the authenticated user's profile. If none exists, returns 404.")
    .Produces<UserProfileDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

api.MapPut("/me/profile", async (UserProfileDto dto, HttpContext http, IProfileService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var profile = await svc.UpsertProfileAsync(dto, userId, ct);
        return Results.Ok(profile);
    })
    .WithName("UpsertMyProfile")
    .WithSummary("Create or update user profile")
    .Produces<UserProfileDto>(StatusCodes.Status200OK)
    .WithOpenApi();

api.MapPost("/pregnancies", async (PregnancyDto dto, HttpContext http, IPregnancyService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var preg = await svc.CreatePregnancyAsync(dto, userId, ct);
        return Results.Created($"/api/v1/pregnancies/{preg.Id}", preg);
    })
    .WithName("CreatePregnancy")
    .WithSummary("Create pregnancy record")
    .Produces<PregnancyDto>(StatusCodes.Status201Created)
    .WithOpenApi();

api.MapGet("/pregnancies/current", async (HttpContext http, IPregnancyService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var preg = await svc.GetCurrentPregnancyAsync(userId, ct);
        return preg is null ? Results.NotFound() : Results.Ok(preg);
    })
    .WithName("GetCurrentPregnancy")
    .WithSummary("Get current pregnancy")
    .Produces<PregnancyDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

static Guid GetUserId(ClaimsPrincipal user, IConfiguration cfg)
{
    var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (Guid.TryParse(claim, out var id))
    {
        return id;
    }
    var fallback = cfg.GetSection("User").Get<UserSettings>()?.FallbackUserId ?? string.Empty;
    if (Guid.TryParse(fallback, out id))
    {
        return id;
    }
    using var md5 = System.Security.Cryptography.MD5.Create();
    var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fallback));
    return new Guid(hash);
}

app.Run();
