using CampusLearn.Data;
using CampusLearn.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Service Configuration --------------------

// MVC with views
builder.Services.AddControllersWithViews();

// ✅ Add distributed memory cache (required for session)
builder.Services.AddDistributedMemoryCache();

// ✅ Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Database context (PostgreSQL via Npgsql)
builder.Services.AddDbContext<CampusLearnContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CampusLearnDb"))
);

// Swagger for API documentation (dev only)
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
        // c.RoutePrefix = ""; // Uncomment to serve Swagger at root
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

// ✅ Enable session middleware
app.UseSession();

app.UseAuthorization();

// -------------------- Routing --------------------

// API endpoints
app.MapControllers();

// MVC default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LogIn}/{action=Index}/{id?}"
);

app.Run();