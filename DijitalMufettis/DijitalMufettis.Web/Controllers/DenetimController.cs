
using DijitalMufettis.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DijitalMufettis.Web.Controllers;

public class DenetimController : Controller
{
    private readonly IPdksOkuyucu _okuyucu;

    // 👇 DI CANLI: konteyner buraya bir PdksOkuyucu enjekte edecek
    public DenetimController(IPdksOkuyucu okuyucu)
    {
        _okuyucu = okuyucu;
    }

    // GET /Denetim/Yukle  → yükleme formunu gösterir
    [HttpGet]
    public IActionResult Yukle()
    {
        return View();
    }

    // POST /Denetim/Kayitlar  → yüklenen dosyayı okur, kayıtları gösterir
    [HttpPost]
    public IActionResult Kayitlar(IFormFile dosya)
    {
        if (dosya is null || dosya.Length == 0)
            return RedirectToAction(nameof(Yukle));   // dosya yoksa forma dön

        // 1) Yüklenen dosyayı geçici bir yola kaydet
        var geciciYol = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
        using (var akis = System.IO.File.Create(geciciYol))
        {
            dosya.CopyTo(akis);
        }

        // 2) DI ile gelen okuyucuyu kullan  ← işte o "pil"i burada kullanıyoruz
        var kayitlar = _okuyucu.Oku(geciciYol);

        // 3) Geçici dosyayı temizle
        System.IO.File.Delete(geciciYol);

        // 4) Kayıtları View'e model olarak gönder
        return View(kayitlar);
    }
}