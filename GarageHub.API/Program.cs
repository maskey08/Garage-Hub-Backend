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

// ── JWT ───────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("Jwt:Key is missing from appsettings.json");

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
        NameClaimType = ClaimTypes.NameIdentifier,
        ClockSkew = TimeSpan.Zero // Reduce clock skew tolerance
    };

    // Handle token from both Authorization header and cookies
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // First try Authorization header
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            
            // If no header token, try cookie
            if (string.IsNullOrEmpty(token))
            {
                token = context.Request.Cookies["AuthToken"];
            }
            
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ Auth failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            Console.WriteLine("❌ Forbidden — role check failed");
            Console.WriteLine($"   Claims: {string.Join(", ", context.HttpContext.User.Claims.Select(c => $"{c.Type}={c.Value}"))}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"✅ Token valid for user: {context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
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
app.UseCors("AllowFrontend");       // ← CORS first
app.UseAuthentication();            // ← then auth
app.UseAuthorization();             // ← then authorization
app.MapControllers();

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