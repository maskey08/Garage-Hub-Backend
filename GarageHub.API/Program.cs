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

// Database - Use environment variable for connection string if available
var connectionString = Environment.GetEnvironmentVariable("GARAGEHUB_DB_CONNECTION") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

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
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Database - PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Services
builder.Services.AddScoped<IVendorService, VendorService>();

// CORS - allow frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Apply pending migrations
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            Debug.WriteLine("Checking database state...");

            // Check if the database exists
            bool canConnect = await dbContext.Database.CanConnectAsync();
            if (!canConnect)
            {
                Debug.WriteLine("Database cannot connect, creating...");
                await dbContext.Database.EnsureCreatedAsync();
            }
            else
            {
                Debug.WriteLine("✅ Database connection established");

                // Try to get migrations to apply
                var pending = await dbContext.Database.GetPendingMigrationsAsync();
                if (pending.Any())
                {
                    Debug.WriteLine($"Applying {pending.Count()} pending migrations...");
                    try
                    {
                        await dbContext.Database.MigrateAsync();
                        Debug.WriteLine("✅ Migrations applied");
                    }
                    catch (Exception migEx) when (migEx.Message.Contains("already exists"))
                    {
                        Debug.WriteLine("✅ Tables already exist, skipping migration");
                    }
                }
                else
                {
                    Debug.WriteLine("✅ All migrations already applied");
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("already exists"))
            {
                Debug.WriteLine("✅ Database schema is complete");
            }
            else
            {
                Debug.WriteLine($"❌ Database check failed: {ex.Message}");
                throw;
            }
        }
    }
}
catch (Exception ex)
{
    Debug.WriteLine($"❌ Fatal error: {ex.Message}");
    throw;
}

// Seed roles and admin user
try
{
    await SeedAdminAsync(app);
}
catch (Npgsql.PostgresException pgEx) when (pgEx.SqlState == "28P01")
{
    Debug.WriteLine($"❌ PostgreSQL authentication failed: {pgEx.Message}");
    Debug.WriteLine($"❌ Please verify PostgreSQL credentials:");
    Debug.WriteLine($"   - Check the password in appsettings.json matches your PostgreSQL 'postgres' user");
    Debug.WriteLine($"   - Or set GARAGEHUB_DB_CONNECTION environment variable with correct credentials");
    Debug.WriteLine($"   - Example: Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=YOUR_ACTUAL_PASSWORD");
    throw;
}
catch (Exception ex)
{
    Debug.WriteLine($"❌ Seeding failed: {ex.Message}");
    Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
    throw;
}

app.UseCors("AllowFrontend");
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