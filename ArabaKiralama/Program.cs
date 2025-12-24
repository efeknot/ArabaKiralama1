using ArabaKiralama.Data;
using Microsoft.EntityFrameworkCore;

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
    // CANLI (RENDER): PostgreSQL linkini parçalar ve formatýný düzeltir
    var uri = new Uri(rawConnectionString);
    var db = uri.AbsolutePath.TrimStart('/');
    var user = uri.UserInfo.Split(':')[0];
    var passwd = uri.UserInfo.Split(':')[1];
    var port = uri.Port > 0 ? uri.Port : 5432;

    connectionString = $"Server={uri.Host};Database={db};User Id={user};Password={passwd};Port={port};SSL Mode=Require;Trust Server Certificate=True;";
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

// OTOMATÝK VERÝTABANI KURUCU
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Hata: " + ex.Message);
    }
}
app.Run();