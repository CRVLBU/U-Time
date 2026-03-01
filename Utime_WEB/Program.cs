using Utime_WEB.Services;
using Utime_WEB.Services.IServices;
using Utime_WEB;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddHttpClient<IActivityService,ActivityService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddHttpClient<IUserAuth, UserAuth>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserAuth, UserAuth>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllersWithViews();
builder.Services.AddSession(u =>
{
    u.IdleTimeout = TimeSpan.FromMinutes(10);
    u.Cookie.IsEssential = true;
    u.Cookie.HttpOnly = true;
});
builder.Services.AddAuthentication(u =>
{
    u.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/UserAuth/Login";
    options.LogoutPath = "/UserAuth/LogOut";
    options.AccessDeniedPath = "/UserAuth/AccessDenied";
});
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

app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
