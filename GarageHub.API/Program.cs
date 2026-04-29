using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using GarageHub.Infrastructure.Data;
using GarageHub.Application.Interfaces;
using GarageHub.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
<<<<<<< HEAD
builder.Services.AddSwaggerGen();
=======
// builder.Services.AddSwaggerGen();

>>>>>>> 984a606393f26d87ba430efc89509368b5ea5c40

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPartRequestService, PartRequestService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

<<<<<<< HEAD
// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "GarageHubSecretKey12345678901234567890";
var key = Encoding.UTF8.GetBytes(jwtKey);
=======

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
>>>>>>> 984a606393f26d87ba430efc89509368b5ea5c40

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
<<<<<<< HEAD
            IssuerSigningKey = new SymmetricSecurityKey(key)
=======

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
>>>>>>> 984a606393f26d87ba430efc89509368b5ea5c40
        };
    });

builder.Services.AddAuthorization();

<<<<<<< HEAD
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
=======

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
>>>>>>> 984a606393f26d87ba430efc89509368b5ea5c40
});


/* IMPORTANT — THIS WAS MISSING */
var app = builder.Build();

<<<<<<< HEAD
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();  
=======

app.UseHttpsRedirection();

// app.UseSwagger();
// app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseAuthentication();
>>>>>>> 984a606393f26d87ba430efc89509368b5ea5c40
app.UseAuthorization();

app.MapControllers();

app.Run();