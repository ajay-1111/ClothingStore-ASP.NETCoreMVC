using Microsoft.AspNetCore.Identity;
using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    // Configure session options as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Add ApplicationDbContext to the services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClothingStoreDB")));

// Add Identity services
builder.Services.AddIdentity<RegisterUserEntity, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

var app = builder.Build();

// Apply any pending migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Handle any errors
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use session middleware
app.UseSession();

// Use authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
