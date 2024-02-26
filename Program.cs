using Microsoft.AspNetCore.Identity;
using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//// Add session services
//builder.Services.AddDistributedMemoryCache(); // This is required to store session data in memory
//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = ".ClothingStore.Session";
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout as needed
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

// Add ApplicationDbContext to the services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClothingStoreDB")));

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();