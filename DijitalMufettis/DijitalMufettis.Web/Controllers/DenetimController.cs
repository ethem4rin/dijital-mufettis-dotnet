using DijitalMufettis.Application.Interfaces;
using DijitalMufettis.Domain.Models;
using DijitalMufettis.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DijitalMufettis.Web.Controllers;

public class DenetimController : Controller
{
    private readonly IPdksOkuyucu _okuyucu;

    public DenetimController(IPdksOkuyucu okuyucu)
    {
        _okuyucu = okuyucu;
    }

    // Yüklenen dosyaların geçici saklandığı klasör (yoksa oluşturur)
    private static string GeciciKlasor()
    {
        var klasor = Path.Combine(Path.GetTempPath(), "DijitalMufettis");
        Directory.CreateDirectory(klasor);
        return klasor;
    }

    // ADIM 1 — GET /Denetim/Yukle : yükleme formu
    [HttpGet]
    public IActionResult Yukle() => View();

    // ADIM 2 — POST /Denetim/Onizle : dosyayı kaydet, önizleme + öneri göster
    [HttpPost]
    public IActionResult Onizle(IFormFile dosya)
    {
        if (dosya is null || dosya.Length == 0)
            return RedirectToAction(nameof(Yukle));

        // Dosyayı GUID adıyla geçici klasöre kaydet
        var kimlik = Guid.NewGuid().ToString();
        var yol = Path.Combine(GeciciKlasor(), kimlik + ".xlsx");
        using (var akis = System.IO.File.Create(yol))
        {
            dosya.CopyTo(akis);
        }

        var onizleme = _okuyucu.Onizle(yol);

        var vm = new OnizlemeViewModel
        {
            DosyaKimligi = kimlik,
            Onizleme = onizleme
        };
        return View(vm);
    }

    // ADIM 3 — POST /Denetim/Kayitlar : kullanıcının seçtiği haritayla oku
    [HttpPost]
    public IActionResult Kayitlar(
        string dosyaKimligi,
        int baslikSatiri, int adSutun, int? soyadSutun,
        int tarihSutun, int? girisSutun, int? cikisSutun)
    {
        // Güvenlik: kimlik gerçek bir GUID mi? (kötü niyetli yol girişini engeller)
        if (!Guid.TryParse(dosyaKimligi, out _))
            return RedirectToAction(nameof(Yukle));

        var yol = Path.Combine(GeciciKlasor(), dosyaKimligi + ".xlsx");
        if (!System.IO.File.Exists(yol))
            return RedirectToAction(nameof(Yukle));

        var harita = new SutunHaritasi
        {
            BaslikSatiri = baslikSatiri,
            AdSutun = adSutun,
            SoyadSutun = soyadSutun,
            TarihSutun = tarihSutun,
            GirisSutun = girisSutun,
            CikisSutun = cikisSutun
        };

        var kayitlar = _okuyucu.Oku(yol, harita);

        System.IO.File.Delete(yol);   // iş bitti, geçici dosyayı temizle

        return View(kayitlar);
    }
}