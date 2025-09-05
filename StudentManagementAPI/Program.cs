using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StudentManagementAPI.Data;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Kestrel to use HTTP only
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); // Listen for HTTP traffic on port 80
});

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register repositories and services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IExportService, ExportService>();

// Add MVC services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student Management API",
        Version = "v1",
        Description = "An API for managing student information",
        Contact = new OpenApiContact
        {
            Name = "University Admin",
            Email = "admin@university.edu"
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management API v1"));
    
    // Initialize database in development
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            
            // Delete existing database if it exists
            context.Database.EnsureDeleted();
            logger.LogInformation("Existing database deleted successfully.");
            
            // Create fresh database with current model
            var created = context.Database.EnsureCreated();
            if (created)
            {
                logger.LogInformation("Database created successfully with seed data.");
            }
            else
            {
                logger.LogWarning("Database already exists.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw; // Re-throw to prevent app from starting with broken database
        }
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enable serving static files
app.UseStaticFiles();

// app.UseHttpsRedirection(); // Disabled HTTPS redirection for Docker environment

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

// Configure routing for MVC and API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.Run();