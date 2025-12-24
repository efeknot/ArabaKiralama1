using ArabaKiralama.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// VERÝTABANI AYARI (Otomatik Algýlama)
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (string.IsNullOrEmpty(connectionString))
    {
        // YEREL: Senin bilgisayarýnda SQLite kullanýr
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    else
    {
        // CANLI (RENDER): PostgreSQL linkini parçalar ve baðlar
        var uri = new Uri(connectionString);
        var db = uri.AbsolutePath.TrimStart('/');
        var user = uri.UserInfo.Split(':')[0];
        var passwd = uri.UserInfo.Split(':')[1];
        var connStr = $"Server={uri.Host};Database={db};User Id={user};Password={passwd};Port={uri.Port};SSL Mode=Require;Trust Server Certificate=True;";
        options.UseNpgsql(connStr);
    }
});

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Hata Sayfasý Ayarý
if (app.Environment.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
else { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// OTOMATÝK TABLO OLUÞTURUCU
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Tablolarý otomatik kurar
    }
    catch (Exception ex)
    {
        Console.WriteLine("Veritabaný Hatasý: " + ex.Message);
    }
}

app.Run();