using DVDRentalAPI.Data;
using DVDRentalAPI.Domain.Entities;
using DVDRentalAPI.Domain.Entities.DTO;
using DVDRentalAPI.Domain.Enums;
using DVDRentalAPI.Domain.Interfaces;
using DVDRentalAPI.Domain.ModelViews;
using DVDRentalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

#region Builder and Swagger
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SQLContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key)) key = "123456";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDVDService, DVDService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Autorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Add your JWT Token here",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home").AllowAnonymous();
#endregion

#region Admins
app.MapGet("/admins", ([FromQuery] int? page, IAdminService adminService) =>
{
    var adms = new List<AdminModelView>();
    var admins = adminService.FindAll(page);
    foreach (var adm in admins)
    {
        adms.Add(new AdminModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
        });
    }
    return Results.Ok(adms);
}).RequireAuthorization().WithTags("Admins");

app.MapGet("/admins/{id}", ([FromRoute] int id, IAdminService adminService) =>
{
    var adm = adminService.FindById(id);
    if (adm == null) return Results.NotFound();
    return Results.Ok(new AdminModelView
    {
        Id = adm.Id,
        Email = adm.Email,
        Profile = adm.Profile
    });
}).RequireAuthorization().WithTags("Admins");

app.MapPost("/admins", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
{
    var validation = new ErrorsHandling
    {
        Messages = new List<string>()
    };
    if (string.IsNullOrEmpty(adminDTO.Email))
        validation.Messages.Add("Email can not be empty");
    if (string.IsNullOrEmpty(adminDTO.Password))
        validation.Messages.Add("Password can not be empty");
    if (adminDTO.Profile == null)
        validation.Messages.Add("Profile can not be empty");
    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);
    var admin = new Admin
    {
        Email = adminDTO.Email,
        Password = adminDTO.Password,
        Profile = adminDTO.Profile.ToString() ?? Profile.Customer.ToString()
    };
    adminService.Create(admin);
    return Results.Created($"/administrador/{admin.Id}", new AdminModelView
    {
        Id = admin.Id,
        Email = admin.Email,
        Profile = admin.Profile
    });
}).RequireAuthorization().WithTags("Admins");

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
}).AllowAnonymous().WithTags("Admins");
#endregion

#region DVD

ErrorsHandling validateDTO(DVDDTO dvdDTO)
{
    var validation = new ErrorsHandling
    {
        Messages = new List<String>()
    };

    if (string.IsNullOrEmpty(dvdDTO.Title))
    {
        validation.Messages.Add("Title can not be empty");
    }

    if (string.IsNullOrEmpty(dvdDTO.Genre))
    {
        validation.Messages.Add("Genre can not be empty");
    }

    if (int.IsNegative(dvdDTO.Year) || dvdDTO.Year == 0)
    {
        validation.Messages.Add("Year can not be either negative or zero");
    }

    if (int.IsNegative(dvdDTO.Duration) || dvdDTO.Duration == 0)
    {
        validation.Messages.Add("Duration can not be either negative or zero");
    }

    return validation;
}

app.MapPost("/dvds", ([FromBody] DVDDTO dvdDTO, IDVDService dvdService) =>
{
    var validation = validateDTO(dvdDTO);

    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    var dvd = new DVD
    {
        Title = dvdDTO.Title,
        Genre = dvdDTO.Genre,
        Duration = dvdDTO.Duration,
        Year = dvdDTO.Year,
    };

    dvdService.Create(dvd);
    return Results.Created("$/dvd/{dvd.Id}", dvd);

}).RequireAuthorization().WithTags("DVDS");

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
    var validation = validateDTO(dvdDTO);

    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    var dvd = dvdService.FindById(id);

    if (dvd == null) return Results.NotFound();

    dvd.Title = dvdDTO.Title;
    dvd.Genre = dvdDTO.Genre;
    dvd.Duration = dvdDTO.Duration;
    dvd.Year = dvdDTO.Year;

    return Results.Ok(dvd);
}).RequireAuthorization().WithTags("DVDS");

app.MapDelete("/dvds/{id}", ([FromRoute] int id, IDVDService dvdService) =>
{
    var dvd = dvdService.FindById(id);

    if (dvd == null) return Results.NotFound();

    dvdService.Delete(dvd);

    return Results.NoContent();
}).RequireAuthorization().WithTags("DVDS");

#endregion

app.UseAuthentication();
app.UseAuthorization();

app.Run();
