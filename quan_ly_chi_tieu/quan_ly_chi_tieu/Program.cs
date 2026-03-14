using Microsoft.EntityFrameworkCore;
using quan_ly_chi_tieu.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (!context.Currencies.Any(c => c.Code == "VND"))
        {
            context.Currencies.Add(new quan_ly_chi_tieu.Models.Currency 
            { 
                Code = "VND", 
                Name = "Việt Nam Đồng", 
                Symbol = "đ", 
                ExchangeRateToVnd = 1, 
                IsActive = true 
            });
        }
        if (!context.Currencies.Any(c => c.Code == "USD"))
        {
            context.Currencies.Add(new quan_ly_chi_tieu.Models.Currency 
            { 
                Code = "USD", 
                Name = "US Dollar", 
                Symbol = "$", 
                ExchangeRateToVnd = 25000, 
                IsActive = true 
            });
        }
        context.SaveChanges();
    }
    catch (Exception ex)
    {
        // Log error or handle as needed
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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();