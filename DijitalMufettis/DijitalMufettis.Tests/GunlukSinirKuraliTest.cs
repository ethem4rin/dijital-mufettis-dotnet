using DijitalMufettis.Application.Hesaplama;
using DijitalMufettis.Application.Kurallar;
using DijitalMufettis.Domain.Enums;
using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Tests;

public class GunlukSinirKuraliTests
{
    private readonly GunlukSinirKurali _kural = new(new MesaiHesaplayici());

    [Fact]
    public void LimitiAsanGun_IhlalUretir()
    {
        var kayitlar = new List<GunlukKayit>
        {
            // 08:00–21:00 = 13s ham − 90dk mola = 11.5s net > 11 → ihlal
            new() { Personel = "Ahmet Yılmaz", Tarih = new DateOnly(2026, 7, 15),
                    Giris = "08:00", Cikis = "21:00" }
        };

        var ihlaller = _kural.Uygula(kayitlar, new DenetimAyarlari { Sektor = Sektor.Genel }).ToList();

        Assert.Single(ihlaller);
        Assert.Equal(IhlalKategorisi.GunlukSinir, ihlaller[0].Kategori);
        Assert.Equal("Ahmet Yılmaz", ihlaller[0].Personel);
    }

    [Fact]
    public void LimitAltiGun_IhlalUretmez()
    {
        var kayitlar = new List<GunlukKayit>
        {
            // 09:00–17:00 = 8s ham − 60dk = 7s net < 11 → ihlal yok
            new() { Personel = "Ayşe Demir", Tarih = new DateOnly(2026, 7, 15),
                    Giris = "09:00", Cikis = "17:00" }
        };

        var ihlaller = _kural.Uygula(kayitlar, new DenetimAyarlari { Sektor = Sektor.Genel }).ToList();

        Assert.Empty(ihlaller);
    }
}