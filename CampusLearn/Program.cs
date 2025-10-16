using CampusLearn.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews(); // For API controllers

builder.Services.AddDbContext<CampusLearnContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("CampusLearnDb"))
);
builder.Services.AddEndpointsApiExplorer();

// Add Swagger with OpenApiInfo
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CampusLearn API",
        Version = "v1",
        Description = "API for the CampusLearn E-Learning Platform"
    });
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CampusLearn API v1");
        c.RoutePrefix = ""; // Swagger at root URL (https://localhost:5001/)
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
app.UseAuthorization();

// Map API controllers (needed for Swagger endpoints to work)
app.MapControllers();

// Map MVC default route (for views if needed)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();


