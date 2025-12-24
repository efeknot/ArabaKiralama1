using ArabaKiralama.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Render'daki PostgreSQL adresini kontrol et
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ??
                       builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("postgres://"))
    {
        // RENDER ÝÇÝN ÖZEL PARÇALAMA (Format Hatasýný Çözer)
        var databaseUri = new Uri(connectionString);
        var userInfo = databaseUri.UserInfo.Split(':');

        var npgsqlConnectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};" +
                                     $"Database={databaseUri.AbsolutePath.TrimStart('/')};" +
                                     $"Username={userInfo[0]};Password={userInfo[1]};" +
                                     $"SSL Mode=Require;Trust Server Certificate=True";

        options.UseNpgsql(npgsqlConnectionString); // Render (Canlý)
    }
    else
    {
        options.UseSqlite(connectionString); // Yerel Bilgisayar
    }
});

builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// OTOMATÝK VERÝTABANI OLUÞTURUCU
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
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný oluþturulurken hata!");
    }
}

app.Run();