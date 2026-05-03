using GarageHub.Application.Interfaces;
using GarageHub.Application.Services;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPartRequestService, PartRequestService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPartService, PartService>();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "GarageHubSecretKey12345678901234567890";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        NameClaimType = ClaimTypes.NameIdentifier
    };

    // ✅ ADD THIS — prints exact auth failure reason
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Debug.WriteLine($"❌ Auth failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            Debug.WriteLine($"❌ Forbidden — token valid but role check failed");
            Debug.WriteLine($"   Claims: {string.Join(", ", context.HttpContext.User.Claims.Select(c => $"{c.Type}={c.Value}"))}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Debug.WriteLine($"✅ Token validated for: {context.HttpContext.User.Identity?.Name}");
            Debug.WriteLine($"   Roles: {string.Join(", ", context.HttpContext.User.Claims.Where(c => c.Type == "role").Select(c => c.Value))}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)  // ← allow any origin in dev
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

// Seed roles and admin user
await SeedAdminAsync(app);

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// ─── Seed Method ────────────────────────────────────────────
async Task SeedAdminAsync(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();

    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<User>>();

    // Create roles
    foreach (var role in new[] { "admin", "staff", "customer" })
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var r = await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
            Debug.WriteLine(r.Succeeded ? $"✅ Role '{role}' created" : $"❌ Role '{role}' failed");
        }
        else
        {
            Debug.WriteLine($"ℹ️  Role '{role}' already exists");
        }
    }

    var adminEmail = "smartsikchya.noreply@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        // Create fresh admin
        var admin = new User
        {
            Email = adminEmail,
            UserName = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true,
            Phone = "+1234567890",
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(admin, "Admin@123456");
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                Debug.WriteLine($"❌ {error.Description}");
        }
        else
        {
            var roleResult = await userManager.AddToRoleAsync(admin, "admin");
            Debug.WriteLine(roleResult.Succeeded
                ? "✅ Admin created + role assigned"
                : "❌ Admin created but role assignment failed");
        }
    }
    else
    {
        Debug.WriteLine($"ℹ️  Admin user already exists (Id: {adminUser.Id})");

        // ✅ Always check and fix the role
        var roles = await userManager.GetRolesAsync(adminUser);
        Debug.WriteLine($"ℹ️  Current roles: [{string.Join(", ", roles)}]");

        if (!roles.Contains("admin"))
        {
            var roleResult = await userManager.AddToRoleAsync(adminUser, "admin");
            Debug.WriteLine(roleResult.Succeeded
                ? "✅ Admin role assigned to existing user"
                : $"❌ Failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }
        else
        {
            Debug.WriteLine("✅ Admin already has 'admin' role — all good");
        }
    }
}