using Microsoft.AspNetCore.Authentication.Cookies;
using grp_management.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using grp_management.Hubs;
using grp_management.Data;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Dashboard", "");
});
builder.Services.AddControllers();

// Add IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add AppDbContext for EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
    });

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    });

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<MessageHub>("/messageHub");

// Seed database
using (var scope = app.Services.CreateScope())
    {
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        if (!context.Users.Any(u => u.Username == "admin"))
        {
            context.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = "Admin",
                Email = "admin@example.com",
                CreatedAt = DateTime.UtcNow
            });
            context.SaveChanges();
        }

        if (!context.Users.Any(u => u.Username == "user"))
        {
            context.Users.Add(new User
            {
                Username = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                Role = "User",
                Email = "user@example.com",
                CreatedAt = DateTime.UtcNow
            });
            context.SaveChanges();
        }
        
        var groupNames = new[] { "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota", "Kappa", "Lambda", "Mu" };
        foreach (var name in groupNames)
        {
            if (!context.Groups.Any(g => g.Name == name))
            {
                context.Groups.Add(new Group { Name = name, Description = $"{name} group description", CreatedAt = DateTime.UtcNow });
            }
        }
        context.SaveChanges();

        if (!context.Templates.Any())
        {
            context.Templates.Add(new Template { TemplateName = "Welcome", TemplateMsg = "Hi {{name}}, welcome to the {{group}} group!", TemplateType = "General" });
            context.Templates.Add(new Template { TemplateName = "Announcement", TemplateMsg = "Hi all, new announcement for {{group}}: {{message}}", TemplateType = "Announcement" });
        }
        context.SaveChanges();

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
