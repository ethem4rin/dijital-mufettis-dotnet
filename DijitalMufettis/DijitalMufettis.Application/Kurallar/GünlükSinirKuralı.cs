using DijitalMufettis.Application.Hesaplama;
using DijitalMufettis.Domain.Enums;
using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Application.Kurallar;

/// <summary>
/// Md. 63 — Günlük net çalışma sınırı aşımı (genel 11s, madencilik 7,5s).
/// </summary>
public class GunlukSinirKurali : IDenetimKurali
{
    private readonly MesaiHesaplayici _hesaplayici;

    public GunlukSinirKurali(MesaiHesaplayici hesaplayici)
    {
        _hesaplayici = hesaplayici;
    }

    public IEnumerable<Ihlal> Uygula(IReadOnlyList<GunlukKayit> kayitlar, DenetimAyarlari ayarlar)
    {
        double limit = ayarlar.Sektor == Sektor.Madencilik ? 7.5 : 11.0;

        foreach (var kayit in kayitlar)
        {
            var hesap = _hesaplayici.Hesapla(kayit.Giris, kayit.Cikis);
            if (hesap is null)
                continue;

            if (hesap.NetCalismaSaati > limit)
            {
                yield return new Ihlal
                {
                    Personel = kayit.Personel,
                    Tarih = kayit.Tarih,
                    Kategori = IhlalKategorisi.GunlukSinir,
                    Tip = $"Günlük {limit:0.#} Saat Sınırı Aşımı",
                    Detay = $"{kayit.Personel}, {kayit.Tarih:dd.MM.yyyy} tarihinde net " +
                            $"{hesap.NetCalismaSaati:0.##} saat çalışarak günlük yasal " +
                            $"sınırı ({limit:0.#} saat) aşmıştır."
                };
            }
        }
    }
}