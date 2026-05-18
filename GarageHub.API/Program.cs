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
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

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
    var userProfileService = scope.ServiceProvider
        .GetRequiredService<IUserProfileService>();

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

            // Sync admin to the custom 'users' table
            await userProfileService.CreateUserProfileAsync(admin, "admin");
        }
    }
    else
    {
        Debug.WriteLine($"ℹ️  Admin user already exists (Id: {adminUser.Id})");

        // ✅ Always check and fix the FirstName and LastName
        bool needsUpdate = false;
        if (string.IsNullOrWhiteSpace(adminUser.FirstName) || adminUser.FirstName != "Admin")
        {
            adminUser.FirstName = "Admin";
            needsUpdate = true;
        }
        if (string.IsNullOrWhiteSpace(adminUser.LastName) || adminUser.LastName != "User")
        {
            adminUser.LastName = "User";
            needsUpdate = true;
        }

        if (needsUpdate)
        {
            var updateResult = await userManager.UpdateAsync(adminUser);
            Debug.WriteLine(updateResult.Succeeded
                ? "✅ Admin FirstName/LastName updated"
                : $"❌ Failed to update admin: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
        }

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

        // ✅ Sync admin profile to the custom 'users' table
        await userProfileService.UpdateUserProfileAsync(adminUser, "admin");
    }
}

// ─── Diagnostic Endpoints ─────────────────────────────────────
// These are public endpoints for debugging and checking the state of the application.
// They should be secured or removed in the production environment.

app.MapGet("/api/diagnostics/db-status", async (AppDbContext db) =>
{
    var connected = await db.Database.CanConnectAsync();
    var userCount = await db.Users.CountAsync();
    var roleCount = await db.Roles.CountAsync();
    var appointmentCount = await db.Appointments.CountAsync();
    var partRequestCount = await db.PartRequests.CountAsync();
    var reviewCount = await db.Reviews.CountAsync();

    return Results.Ok(new
    {
        Database = connected ? "Connected" : "Disconnected",
        UserCount = userCount,
        RoleCount = roleCount,
        AppointmentCount = appointmentCount,
        PartRequestCount = partRequestCount,
        ReviewCount = reviewCount
    });
});

app.MapGet("/api/diagnostics/users", async (UserManager<User> userManager) =>
{
    var users = userManager.Users
        .Select(u => new { u.Id, u.Email, u.UserName, u.FirstName, u.LastName, u.Phone, u.CreatedAt })
        .ToList();

    return Results.Ok(users);
});

app.MapGet("/api/diagnostics/roles", async (RoleManager<IdentityRole<int>> roleManager) =>
{
    var roles = roleManager.Roles
        .Select(r => new { r.Id, r.Name })
        .ToList();

    return Results.Ok(roles);
});

app.MapGet("/api/diagnostics/user-roles", async (AppDbContext db) =>
{
    var users = await db.Users.ToListAsync();
    var userRolesList = new List<object>();

    foreach (var user in users)
    {
        var userRoles = await db.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new
            {
                UserId = user.Id,
                UserEmail = user.Email,
                RoleId = r.Id,
                RoleName = r.Name
            })
            .ToListAsync();

        if (userRoles.Any())
        {
            userRolesList.AddRange(userRoles);
        }
    }

    return Results.Ok(userRolesList);
});