using LoanDashboard.BackgroundServices;
using LoanDashboard.Hubs;
using LoanDashboard.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ─── Serilog ────────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} — {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/loan-dashboard-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

// ─── Services ────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IStageService, StageService>();

// Background poller — calls SQL every N seconds and pushes via SignalR
builder.Services.AddHostedService<StagePollerService>();

// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors    = builder.Environment.IsDevelopment();
    options.KeepAliveInterval       = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval   = TimeSpan.FromSeconds(60);
});

// MVC + Razor views
builder.Services.AddControllersWithViews();

// ─── Build ───────────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── Middleware pipeline ─────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<LoanDashboardHub>("/hubs/dashboard");

Log.Information("Loan Dashboard MVC starting on {Urls}",
    builder.Configuration["ASPNETCORE_URLS"] ?? "http://localhost:5100");

await app.RunAsync();
