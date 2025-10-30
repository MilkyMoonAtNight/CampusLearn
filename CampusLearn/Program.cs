using CampusLearn.Data;
using CampusLearn.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Service Configuration --------------------

// MVC with views
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Database context (PostgreSQL via Npgsql)
builder.Services.AddDbContext<CampusLearnContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CampusLearnDb"))
);

// Authentication + Authorization (cookie-based)
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/LogIn/Index";         // where unauthenticated users are sent
        options.AccessDeniedPath = "/LogIn/Denied"; // where unauthorized users are sent
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Health checks (so /healthz works)
builder.Services.AddHealthChecks();

// CORS (optional: allow firmware/dashboard if they’re on a different origin)
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowLocal",
        p => p
            .WithOrigins(
                "http://localhost:5014",   // MVC site (adjust to your port)
                "http://localhost:3000",   // e.g. separate frontend if you use one
                "http://192.168.0.0"       // sample; replace/remove as needed
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Swagger (dev only)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CampusLearn API",
        Version = "v1",
        Description = "API for the CampusLearn E-Learning Platform"
    });
});

// Custom services
builder.Services.AddSingleton<IAnnouncementsStore, AnnouncementsStore>();

var app = builder.Build();

// -------------------- Middleware Pipeline --------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CampusLearn API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowLocal");     // only needed if cross-origin calls are real
app.UseSession();

app.UseAuthentication();       // <-- must be before Authorization
app.UseAuthorization();

// -------------------- Routing --------------------

// Health
app.MapHealthChecks("/healthz");

// API endpoints (attribute-routed controllers)
app.MapControllers();

// MVC default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LogIn}/{action=Index}/{id?}"
);

app.Run();
