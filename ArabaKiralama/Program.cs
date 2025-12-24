using ArabaKiralama.Data;
using Microsoft.EntityFrameworkCore;

// 1. KRÝTÝK DÜZELTME: PostgreSQL Tarih/Saat Hatasýný Çözer (Efe Baba Özel)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Render'dan gelen gizli veritabaný adresini al
var rawConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
string connectionString;

if (string.IsNullOrEmpty(rawConnectionString))
{
    // YEREL: Bilgisayarýnda çalýþýrken SQLite kullanýr
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
}
else
{
    // 2. KRÝTÝK DÜZELTME: CANLI (RENDER) Baðlantý Ayarlarý
    var uri = new Uri(rawConnectionString);
    var db = uri.AbsolutePath.TrimStart('/');
    var user = uri.UserInfo.Split(':')[0];
    var passwd = uri.UserInfo.Split(':')[1];
    var port = uri.Port > 0 ? uri.Port : 5432;

    // Pooling=false ve Trust Server Certificate Render PostgreSQL için daha güvenlidir
    connectionString = $"Server={uri.Host};Database={db};User Id={user};Password={passwd};Port={port};SSL Mode=Require;Trust Server Certificate=True;Pooling=false;";
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
}

builder.Services.AddControllersWithViews();
var app = builder.Build();

if (app.Environment.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
else { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// 3. KRÝTÝK DÜZELTME: Otomatik Migration ve Veritabaný Onarýmý
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Eðer tablo yapýsýnda SQLite'tan kalan bir uyumsuzluk varsa onarmaya çalýþýr
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Veritabaný Hatasý: " + ex.Message);
    }
}

app.Run();