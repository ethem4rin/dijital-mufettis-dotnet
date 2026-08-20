using DijitalMufettis.Application.Interfaces;
using DijitalMufettis.Application.Kurallar;
using DijitalMufettis.Domain.Enums;
using DijitalMufettis.Domain.Models;
using DijitalMufettis.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DijitalMufettis.Web.Controllers;

public class DenetimController : Controller
{
    private readonly IPdksOkuyucu _okuyucu;
    private readonly GunlukSinirKurali _gunlukSinirKurali;

    public DenetimController(IPdksOkuyucu okuyucu, GunlukSinirKurali gunlukSinirKurali)
    {
        _okuyucu = okuyucu;
        _gunlukSinirKurali = gunlukSinirKurali;
    }

    private static string GeciciKlasor()
    {
        var klasor = Path.Combine(Path.GetTempPath(), "DijitalMufettis");
        Directory.CreateDirectory(klasor);
        return klasor;
    }

    [HttpGet]
    public IActionResult Yukle() => View();

    [HttpPost]
    public IActionResult Onizle(IFormFile dosya)
    {
        if (dosya is null || dosya.Length == 0)
            return RedirectToAction(nameof(Yukle));

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

    [HttpPost]
    public IActionResult Kayitlar(
        string dosyaKimligi,
        Sektor sektor,
        int baslikSatiri, int adSutun, int? soyadSutun,
        int tarihSutun, int? girisSutun, int? cikisSutun)
    {
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
        System.IO.File.Delete(yol);

        // Kuralı çalıştır → ihlalleri bul
        var ayarlar = new DenetimAyarlari { Sektor = sektor };
        var ihlaller = _gunlukSinirKurali.Uygula(kayitlar, ayarlar).ToList();

        var vm = new SonucViewModel
        {
            Kayitlar = kayitlar,
            Ihlaller = ihlaller
        };
        return View(vm);
    }
}