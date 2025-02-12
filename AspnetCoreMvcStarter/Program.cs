using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AspnetCoreMvcStarter.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AspnetCoreMvcStarterContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AspnetCoreMvcStarterContext") ?? throw new InvalidOperationException("Connection string 'AspnetCoreMvcStarterContext' not found.")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
  .AddEntityFrameworkStores<AspnetCoreMvcStarterContext>()
  .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
  options.LoginPath = "/Auth/Login"; // Account/Login <- default
  options.AccessDeniedPath = "/Auth/AccessDenied"; // Trang 403, đăng nhập rồi nhưng không phải admin hoặc ...
  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var scope = app.Services.CreateScope();
// tạo mới role
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
// tạo mới user
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

if (!await roleManager.RoleExistsAsync("Admin"))
{
  await roleManager.CreateAsync(new IdentityRole("Admin"));
}

var adminUser = new IdentityUser
{
  UserName = "luyendh@example.com",
  Email = "luyendh@example.com",
  EmailConfirmed = true
};

if (await userManager.FindByEmailAsync(adminUser.Email) == null)
{
  var result = await userManager.CreateAsync(adminUser, "Admin@123");
  if (result.Succeeded)
  {
    await userManager.AddToRoleAsync(adminUser, "Admin");
  }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
