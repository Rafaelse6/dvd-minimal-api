using DVDRentalAPI.Data;
using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Entities.DTO;
using DVDRentalAPI.Domain.Interfaces;
using DVDRentalAPI.Domain.ModelViews;
using DVDRentalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#region Builder and Swagger
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SQLContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDVDService, DVDService>();

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
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Admins
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    if (adminService.Login(loginDTO) != null)
    {
        return Results.Ok("Logged");
    }
    else
    {
        return Results.Unauthorized();
    }
}).WithTags("Admins");
#endregion

#region DVD
app.MapPost("/dvds", ([FromBody] DVDDTO dvdDTO, IDVDService dvdService) =>
{

    var dvd = new DVD
    {
        Title = dvdDTO.Title,
        Genre = dvdDTO.Genre,
        Duration = dvdDTO.Duration,
        Year = dvdDTO.Year,
        ReleaseDate = dvdDTO.ReleaseDate,
    };

    dvdService.Create(dvd);
    return Results.Created("$/dvd/{dvd.Id}", dvd);

}).WithTags("DVDS");

app.MapGet("/dvds", ([FromQuery] int? page, IDVDService dvdService) =>
{
    var dvds = dvdService.GetAllDVDs(page);

    return Results.Ok(dvds);
}).WithTags("DVDS");

app.MapGet("/dvds/{id}", ([FromRoute] int? id, IDVDService dvdService) =>
{
    var dvd = dvdService.FindById(id);

    if (dvd == null) return Results.NotFound();

    return Results.Ok(dvd);
}).WithTags("DVDS");

app.MapPut("/dvds/{id}", ([FromRoute] int? id, DVDDTO dvdDTO, IDVDService dvdService) =>
{
    var dvd = dvdService.FindById(id);

    if (dvd == null) return Results.NotFound();

    dvd.Title = dvdDTO.Title;
    dvd.Genre = dvdDTO.Genre;
    dvd.Duration = dvdDTO.Duration;
    dvd.Year = dvdDTO.Year;
    dvd.ReleaseDate = dvdDTO.ReleaseDate;

    return Results.Ok(dvd);
}).WithTags("DVDS");

#endregion

app.Run();
