using HolaBebe.Application.Mapping;
using HolaBebe.Application;
using HolaBebe.Application.Dtos;
using HolaBebe.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HolaBebe.Infrastructure;
using HolaBebe.Domain;
using System.Collections.Generic;
using HolaBebe.Api;

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

api.MapPost("/me/profile", async (UserProfileDto dto, HttpContext http, IProfileService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        try
        {
            var profile = await svc.CreateProfileAsync(dto, userId, ct);
            return Results.Created("/api/v1/me/profile", profile);
        }
        catch (InvalidOperationException)
        {
            return Results.Conflict();
        }
    })
    .WithName("CreateMyProfile")
    .WithSummary("Create user profile")
    .Produces<UserProfileDto>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status409Conflict)
    .Produces(StatusCodes.Status400BadRequest)
    .WithOpenApi();

api.MapPut("/me/profile", async (UserProfileDto dto, HttpContext http, IProfileService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var profile = await svc.UpdateProfileAsync(dto, userId, ct);
        return profile is null ? Results.NotFound() : Results.Ok(profile);
    })
    .WithName("UpdateMyProfile")
    .WithSummary("Update user profile")
    .Produces<UserProfileDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status400BadRequest)
    .WithOpenApi();

api.MapPost("/pregnancies", async (PregnancyCreateDto dto, HttpContext http, IPregnancyService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        try
        {
            var preg = await svc.CreatePregnancyAsync(dto, userId, ct);
            return Results.Created($"/api/v1/pregnancies/{preg.Id}", preg);
        }
        catch (InvalidOperationException)
        {
            return Results.Conflict();
        }
    })
    .WithName("CreatePregnancy")
    .WithSummary("Create pregnancy record")
    .Produces<PregnancyDto>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status409Conflict)
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

api.MapPatch("/pregnancies/{id}", async (Guid id, PregnancyUpdateDto dto, HttpContext http, IPregnancyService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var preg = await svc.UpdatePregnancyAsync(id, dto, userId, ct);
        return preg is null ? Results.NotFound() : Results.Ok(preg);
    })
    .WithName("UpdatePregnancy")
    .WithSummary("Update pregnancy record")
    .Produces<PregnancyDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

api.MapGet("/pregnancies/{id}/fruit-size", async (Guid id, HttpContext http, IPregnancyService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var fruit = await svc.GetFruitSizeAsync(id, userId, ct);
        return fruit is null ? Results.NotFound() : Results.Ok(fruit);
    })
    .WithName("GetFruitSize")
    .WithSummary("Get baby size as fruit")
    .Produces<FruitSizeDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

api.MapGet("/pregnancies/{id}/week/{n:int}", async (Guid id, int n, HttpContext http, IPregnancyService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var summary = await svc.GetWeeklySummaryAsync(id, n, userId, ct);
        return summary is null ? Results.NotFound() : Results.Ok(summary);
    })
    .WithName("GetWeeklySummary")
    .WithSummary("Get weekly content summary")
    .Produces<WeeklySummaryDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

api.MapGet("/calendar-events", async (DateTime? startDateFrom, DateTime? startDateTo, CalendarType? type, HttpContext http, ICalendarService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var list = new List<CalendarEventDto>();
        await foreach (var ev in svc.GetEventsAsync(userId, startDateFrom, startDateTo, type, ct))
        {
            list.Add(ev);
        }
        return Results.Ok(list);
    })
    .WithName("ListCalendarEvents")
    .WithSummary("List calendar events")
    .Produces<IList<CalendarEventDto>>(StatusCodes.Status200OK)
    .WithOpenApi();

api.MapPost("/calendar-events", async (CalendarEventCreateDto dto, HttpContext http, ICalendarService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var ev = await svc.CreateEventAsync(dto, userId, ct);
        return Results.Created($"/api/v1/calendar-events/{ev.Id}", ev);
    })
    .WithName("CreateCalendarEvent")
    .WithSummary("Create calendar event")
    .Produces<CalendarEventDto>(StatusCodes.Status201Created)
    .WithOpenApi();

api.MapGet("/calendar-events/{id}", async (Guid id, HttpContext http, ICalendarService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var ev = await svc.GetEventAsync(id, userId, ct);
        return ev is null ? Results.NotFound() : Results.Ok(ev);
    })
    .WithName("GetCalendarEvent")
    .WithSummary("Get calendar event")
    .Produces<CalendarEventDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

api.MapPatch("/calendar-events/{id}", async (Guid id, CalendarEventUpdateDto dto, HttpContext http, ICalendarService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var ev = await svc.UpdateEventAsync(id, dto, userId, ct);
        return ev is null ? Results.NotFound() : Results.Ok(ev);
    })
    .WithName("UpdateCalendarEvent")
    .WithSummary("Update calendar event")
    .Produces<CalendarEventDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

api.MapDelete("/calendar-events/{id}", async (Guid id, HttpContext http, ICalendarService svc, IConfiguration cfg, CancellationToken ct) =>
    {
        var userId = GetUserId(http.User, cfg);
        var ok = await svc.DeleteEventAsync(id, userId, ct);
        return ok ? Results.NoContent() : Results.NotFound();
    })
    .WithName("DeleteCalendarEvent")
    .WithSummary("Delete calendar event")
    .Produces(StatusCodes.Status204NoContent)
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
