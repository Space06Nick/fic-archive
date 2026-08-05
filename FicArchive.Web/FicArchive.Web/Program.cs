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

    if (!db.Works.Any())
    {
        db.Works.Add(new FicArchive.Web.Models.Work
        {
            Title = "The First Story on FicArchive",
            Summary = "This is a test story to show how the site works.",
            AuthorName = "Space06Nick",
            CreatedAt = DateTime.UtcNow
        });

        db.Works.Add(new FicArchive.Web.Models.Work
        {
            Title = "Adventures in Space",
            Summary = "Science fiction about a journey to distant stars.",
            AuthorName = "Space06Nick",
            CreatedAt = DateTime.UtcNow
        });

        db.SaveChanges();
    }

    if (!db.Chapters.Any() && db.Works.Any())
    {
        var work = db.Works.First();

        db.Chapters.Add(new FicArchive.Web.Models.Chapter
        {
            WorkId = work.Id,
            ChapterNumber = 1,
            Title = "The Beginning of the Journey",
            Content = "It was a dark night when the hero first opened the door to the archive.\n\nSomeday the real text of your first story will live here."
        });

        db.Chapters.Add(new FicArchive.Web.Models.Chapter
        {
            WorkId = work.Id,
            ChapterNumber = 2,
            Title = "First Steps",
            Content = "The hero took the first step and understood there was no turning back.\n\nThousands of pages lay ahead."
        });

        db.SaveChanges();
    }
}

app.Run();