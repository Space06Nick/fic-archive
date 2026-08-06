using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FicArchive.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Читаем user-secrets (на будущее, когда подключим подтверждение почты)
builder.Configuration.AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), optional: true);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=FicArchive.db"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // TODO: вернуть true, когда настроим отправку писем для подтверждения аккаунта
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Отправитель писем (на будущее: подтверждение почты, сброс пароля)
builder.Services.AddTransient<IEmailSender<IdentityUser>, FicArchive.Web.Services.EmailSender>();
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, FicArchive.Web.Services.EmailSender>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();