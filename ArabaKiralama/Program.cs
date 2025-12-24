using ArabaKiralama.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Eskisini sil, bunu yapýþtýr:
// Render'daki PostgreSQL adresini kontrol et, yoksa yereldeki SQLite'ý kullan
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ??
                       builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (connectionString.Contains("postgres://"))
        options.UseNpgsql(connectionString); // Render (Canlý)
    else
        options.UseSqlite(connectionString); // Efe Baba'nýn Bilgisayarý (Yerel)
});
// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// OTOMATÝK VERÝTABANI OLUÞTURUCU (Efe Baba Özel)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Eðer tablolar yoksa otomatik oluþturur (Migration'larý basar)
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Hata olursa günlüðe yazar
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný oluþturulurken bir hata oluþtu.");
    }
}

app.Run(); // Bu zaten sende vardý, en sonda kalsýn.
