using EduLog.CoreLayer.Services;
using EduLog.CoreLayer.Services.FileManager;
using EduLog.CoreLayer.Services.Posts;
using EduLog.CoreLayer.Utilities;
using EduLog.DataLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<BlogContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EduLogConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddAutoMapper(cfg => { },
    typeof(EduLog.Web.MappingProfiles.AdminPanel.Categories.CategoryProfile).Assembly);

builder.Services.AddAutoMapper(cfg => { },
    typeof(PostProfile).Assembly);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/signin";
        options.LogoutPath = "/auth/signout";
        options.AccessDeniedPath = "/auth/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie = new CookieBuilder
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            SecurePolicy = CookieSecurePolicy.Always
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireUser", policy =>
        policy.RequireRole("User", "Admin"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply migrations automatically (create/update DB if needed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlogContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.UseEndpoints(option =>
{
    option.MapControllerRoute(
        name: "default",
        pattern: "{Area=exists}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();