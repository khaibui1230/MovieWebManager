using Microsoft.EntityFrameworkCore;
using Movie.DataAccess.Data;
using Movie.DataAccess.Repository;
using Movie.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Movie.Untility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Movie.Models;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Ghi log ra console (terminal)
builder.Logging.AddDebug(); // Ghi log vào cửa sổ debug (VS)

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//add stripe
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Add the service of Facebook authentication
builder.Services.AddAuthentication().AddFacebook(options =>
    {
        options.AppId = "1244748133610574";
        options.AppSecret = "8776502d8059add4201d59cbe7a498e5";
    }
);

// add the cookies for the web
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Add the Session services
// Add session services
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make session cookie essential
});

// Add a serrvice to usse implement category    
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add service to send Email when create an account
builder.Services.AddScoped<IEmailSender, EmailSender>();
// add razer page for login/ regis
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();