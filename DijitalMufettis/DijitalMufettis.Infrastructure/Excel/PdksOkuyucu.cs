using ClosedXML.Excel;
using DijitalMufettis.Application.Interfaces;
using DijitalMufettis.Domain.Models;
using System.Globalization;

namespace DijitalMufettis.Infrastructure.Excel;

/// <summary>
/// IPdksOkuyucu'nun ClosedXML uygulaması.
/// İki aşamalı: Onizle (ham önizleme + otomatik öneri) → Oku (kullanıcı haritasıyla).
/// </summary>
public class PdksOkuyucu : IPdksOkuyucu
{
    // ============ 1) ÖNİZLEME ============
    public OnizlemeSonucu Onizle(string dosyaYolu)
    {
        using var workbook = new XLWorkbook(dosyaYolu);
        var sayfa = workbook.Worksheets.First();

        int sutunSayisi = sayfa.LastColumnUsed()?.ColumnNumber() ?? 0;
        int sonSatir = Math.Min(15, sayfa.LastRowUsed()?.RowNumber() ?? 0);

        // İlk 15 satırı GERÇEK satır numarasıyla oku (önizleme numarası = Excel numarası)
        var satirlar = new List<IReadOnlyList<string>>();
        for (int r = 1; r <= sonSatir; r++)
        {
            var satir = sayfa.Row(r);
            var hucreler = new List<string>();
            for (int k = 1; k <= sutunSayisi; k++)
                hucreler.Add(satir.Cell(k).GetFormattedString().Trim());
            satirlar.Add(hucreler);
        }

        return new OnizlemeSonucu
        {
            Satirlar = satirlar,
            SutunSayisi = sutunSayisi,
            OnerilenHarita = HaritaOner(sayfa)   // otomatik tahmin
        };
    }

    // ============ 2) HARİTAYLA OKUMA ============
    public IReadOnlyList<GunlukKayit> Oku(string dosyaYolu, SutunHaritasi harita)
    {
        var kayitlar = new List<GunlukKayit>();

        using var workbook = new XLWorkbook(dosyaYolu);
        var sayfa = workbook.Worksheets.First();

        // Başlık satırının ALTINDAN itibaren oku
        foreach (var satir in sayfa.RowsUsed().Where(r => r.RowNumber() > harita.BaslikSatiri))
        {
            var ad = satir.Cell(harita.AdSutun).GetString().Trim();
            var soyad = harita.SoyadSutun is null
                ? ""
                : satir.Cell(harita.SoyadSutun.Value).GetString().Trim();
            var personel = string.IsNullOrWhiteSpace(soyad) ? ad : $"{ad} {soyad}";

            if (string.IsNullOrWhiteSpace(personel))
                continue;

            var tarih = TarihCoz(satir.Cell(harita.TarihSutun));
            if (tarih is null)
                continue;

            string? giris = harita.GirisSutun is null ? null
                : satir.Cell(harita.GirisSutun.Value).GetFormattedString().Trim();
            string? cikis = harita.CikisSutun is null ? null
                : satir.Cell(harita.CikisSutun.Value).GetFormattedString().Trim();

            kayitlar.Add(new GunlukKayit
            {
                Personel = personel,
                Tarih = tarih.Value,
                Giris = string.IsNullOrWhiteSpace(giris) ? null : giris,
                Cikis = string.IsNullOrWhiteSpace(cikis) ? null : cikis
            });
        }

        return kayitlar;
    }

    // ============ 3) OTOMATİK HARİTA ÖNERİSİ (eski başlık-arama mantığımız) ============
    private static SutunHaritasi? HaritaOner(IXLWorksheet sayfa)
    {
        foreach (var satir in sayfa.RowsUsed().Take(20))
        {
            int? ad = null, soyad = null, tarih = null, giris = null, cikis = null;

            foreach (var hucre in satir.CellsUsed())
            {
                var b = Normalize(hucre.GetString());
                int k = hucre.Address.ColumnNumber;

                if (ad is null && (b == "ad" || b == "adi" || b.Contains("adsoyad")
                    || b.Contains("isim") || b.Contains("personel") || b.Contains("calisan")))
                    ad = k;
                else if (soyad is null && b.Contains("soyad")) soyad = k;
                else if (tarih is null && (b.Contains("tarih") || b == "gun")) tarih = k;
                else if (giris is null && b.Contains("giris")) giris = k;
                else if (cikis is null && b.Contains("cikis")) cikis = k;
            }

            if (ad is not null && tarih is not null)
            {
                return new SutunHaritasi
                {
                    BaslikSatiri = satir.RowNumber(),
                    AdSutun = ad.Value,
                    SoyadSutun = soyad,
                    TarihSutun = tarih.Value,
                    GirisSutun = giris,
                    CikisSutun = cikis
                };
            }
        }
        return null;   // öneremedik → kullanıcı elle seçsin
    }

    // ============ Yardımcılar ============
    private static string Normalize(string s)
    {
        s = s.Trim()
             .Replace("İ", "i").Replace("I", "i").Replace("ı", "i")
             .Replace("Ş", "s").Replace("ş", "s")
             .Replace("Ğ", "g").Replace("ğ", "g")
             .Replace("Ü", "u").Replace("ü", "u")
             .Replace("Ö", "o").Replace("ö", "o")
             .Replace("Ç", "c").Replace("ç", "c");
        return s.ToLowerInvariant().Replace(" ", "");
    }

    private static DateOnly? TarihCoz(IXLCell hucre)
    {
        if (hucre.DataType == XLDataType.DateTime)
            return DateOnly.FromDateTime(hucre.GetDateTime());

        var metin = hucre.GetString().Trim();
        if (DateTime.TryParse(metin, new CultureInfo("tr-TR"),
                DateTimeStyles.None, out var dt))
            return DateOnly.FromDateTime(dt);

        return null;
    }
}