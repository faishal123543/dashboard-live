using Fintech.Backoffice.Application.Mappings;
using Fintech.Backoffice.Domain.Interfaces;
using Fintech.Backoffice.Infrastructure.Persistence;
using Fintech.Backoffice.Web.Filters;
using Fintech.Backoffice.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC with global auth filter
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SessionAuthFilter>();
});

// Session for auth state
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".FintechDashboard.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// Application services
builder.Services.AddSingleton<IOtpService, OtpService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session MUST come before MVC pipeline
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
