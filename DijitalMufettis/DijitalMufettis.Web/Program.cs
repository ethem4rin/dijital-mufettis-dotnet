using DijitalMufettis.Application.Interface;
using DijitalMufettis.Infrastructure.Excel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPdksOkuyucu, PdksOkuyucu>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();  /// <sumary>
//
//Projendeki wwwroot klasörünün içindeki CSS, JavaScript ve resim dosyalarýný dýþarýya (tarayýcýya) açar.
//Bu olmazsa web siten sadece düz, renksiz metinlerden oluþur.
//
//</sumary>
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

/// summary>
/// Kullanýcý adres çubuðuna hiçbir þey yazmadan ana siteye (seninsiten.com) girerse, 
///onu nereye göndereceðini belirler.
///</summary >
app.Run();
