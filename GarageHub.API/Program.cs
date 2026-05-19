using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using GarageHub.Infrastructure.Repositories;
using GarageHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ── Database ──────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──────────────────────────────────────────────
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IPartRepository, PartRepository>();

// ── Services ──────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPartRequestService, PartRequestService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHostedService<NotificationBackgroundService>();

// ── JWT ───────────────────────────────────────────────────────
// ── JWT ───────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("Jwt:Key is missing from appsettings.json");

// ✅ Add this to see detailed JWT errors in development
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // ← allow http in dev
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
        NameClaimType = ClaimTypes.NameIdentifier,
        ClockSkew = TimeSpan.FromMinutes(5)
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            // ✅ Now shows full error with ShowPII = true
            Console.WriteLine($"❌ Auth failed: {context.Exception.GetType().Name}: {context.Exception.Message}");
            if (context.Exception.InnerException != null)
                Console.WriteLine($"   Inner: {context.Exception.InnerException.Message}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($" Challenge issued for: {context.Request.Path}");
            Console.WriteLine($"   Error: {context.Error}");
            Console.WriteLine($"   Description: {context.ErrorDescription}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = context.Principal?.FindFirst(
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
            Console.WriteLine($"✅ Token valid - UserId: {userId}, Role: {role}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ── CORS ──────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .WithOrigins("http://localhost:5173")
              .WithOrigins("http://localhost:5174")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ═════════════════════════════════════════════════════════════
var app = builder.Build();
// ═════════════════════════════════════════════════════════════

// ── Swagger ───────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



// ── Database setup ────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // ✅ Only apply pending migrations — never EnsureDeleted in production
        await db.Database.MigrateAsync();
        Console.WriteLine("✅ Database migrations applied");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Migration warning: {ex.Message}");
        // If migrations fail, try EnsureCreated as fallback
        await db.Database.EnsureCreatedAsync();
        Console.WriteLine("✅ Database schema ensured");
    }
}

// ── Seed admin ────────────────────────────────────────────────
await SeedAdminAsync(app);

// ── Middleware pipeline (ORDER MATTERS) ───────────────────────
app.UseHttpsRedirection();
app.UseRouting();                   // ← routing support
app.UseCors("AllowFrontend");       // ← CORS first (MUST be before auth)
app.UseAuthentication();            // ← then authentication
app.UseAuthorization();             // ← then authorization

// ── Global error handling ─────────────────────────────────────
app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error?.Error is KeyNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        await context.Response.WriteAsJsonAsync(new
        {
            message = error?.Error?.Message ?? "An unexpected error occurred",
            statusCode = context.Response.StatusCode
        });
    });
});

app.MapControllers();               // ← then route mapping

// ── Catch 404 for unmapped routes ─────────────────────────────
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            message = $"Endpoint not found: {context.Request.Method} {context.Request.Path}",
            statusCode = 404
        });
    }
});


// ── Diagnostic endpoints ──────────────────────────────────────
app.MapGet("/api/diagnostics/db-status", async (AppDbContext db) =>
{
    try
    {
        var connected = await db.Database.CanConnectAsync();
        var userCount = await db.Users.CountAsync();
        return Results.Ok(new { Database = connected ? "Connected" : "Disconnected", UserCount = userCount });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database error: {ex.Message}");
    }
}).AllowAnonymous();

app.MapGet("/api/diagnostics/users", async (AppDbContext db) =>
{
    try
    {
        var users = await db.Users
            .Select(u => new { u.UserId, u.Email, u.Role, u.FirstName, u.LastName })
            .ToListAsync();
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error fetching users: {ex.Message}");
    }
}).AllowAnonymous();

// Health check endpoint
app.MapGet("/api/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
   .AllowAnonymous();

// API info endpoint
app.MapGet("/api/info", () => Results.Ok(new { 
    Name = "Garage Hub API", 
    Version = "1.0.0", 
    Environment = app.Environment.EnvironmentName,
    Timestamp = DateTime.UtcNow 
})).AllowAnonymous();

app.Run(); // ← app.Run() is LAST, nothing after this runs

// ─── Seed Method ─────────────────────────────────────────────
async Task SeedAdminAsync(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var adminEmail = "smartsikchya.noreply@gmail.com";
    var adminUser = await db.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

    if (adminUser == null)
    {
        var admin = new User
        {
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            Phone = "+1234567890",
            Role = "admin",
            CreatedAt = DateTime.UtcNow
        };
        admin.PasswordHashText = BCrypt.Net.BCrypt.HashPassword("Admin@123456");

        db.Users.Add(admin);
        await db.SaveChangesAsync();
        Console.WriteLine("✅ Admin created successfully");
    }
    else
    {
        if (adminUser.Role != "admin")
        {
            adminUser.Role = "admin";
            await db.SaveChangesAsync();
            Console.WriteLine("✅ Admin role fixed");
        }
        else
        {
            Console.WriteLine($"✅ Admin exists (Id: {adminUser.UserId}) — all good");
        }
    }
}
