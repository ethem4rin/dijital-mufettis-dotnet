using ClosedXML.Excel;
using DijitalMufettis.Application.Interface;
using DijitalMufettis.Domain.Models;
using System.Globalization;

namespace DijitalMufettis.Infrastructure.Excel;

/// <summary>
/// IPdksOkuyucu'nun ClosedXML ile gerçek uygulaması.
/// v1: Standart "tablo" düzenini okur (ilk sayfa, başlık satırı + altındaki satırlar).
/// </summary>
public class PdksOkuyucu : IPdksOkuyucu
{
    public IReadOnlyList<GunlukKayit> Oku(string dosyaYolu)
    {
        var kayitlar = new List<GunlukKayit>();

        using var workbook = new XLWorkbook(dosyaYolu);
        var sayfa = workbook.Worksheets.First();   // v1: ilk sayfayı kullan

        // 1) Başlık satırını bul (ilk dolu satır) ve sütun numaralarını tespit et
        var baslikSatiri = sayfa.RowsUsed().First();

        int? adSutun = null, tarihSutun = null, girisSutun = null, cikisSutun = null;
        foreach (var hucre in baslikSatiri.CellsUsed())
        {
            var baslik = Normalize(hucre.GetString());
            int kolon = hucre.Address.ColumnNumber;

            if (adSutun is null && (baslik.Contains("adsoyad") || baslik.Contains("isim")
                || baslik.Contains("personel") || baslik.Contains("calisan") || baslik == "ad"))
                adSutun = kolon;
            else if (tarihSutun is null && baslik.Contains("tarih"))
                tarihSutun = kolon;
            else if (girisSutun is null && baslik.Contains("giris"))
                girisSutun = kolon;
            else if (cikisSutun is null && baslik.Contains("cikis"))
                cikisSutun = kolon;
        }

        // Ad ve tarih olmazsa okuma anlamsız — erken hata ver
        if (adSutun is null || tarihSutun is null)
            throw new InvalidOperationException(
                "Excel'de 'personel/ad' veya 'tarih' sütunu bulunamadı.");

        // 2) Başlıktan sonraki satırları oku
        foreach (var satir in sayfa.RowsUsed().Skip(1))
        {
            var personel = satir.Cell(adSutun.Value).GetString().Trim();
            if (string.IsNullOrWhiteSpace(personel))
                continue;   // boş satırı atla

            var tarih = TarihCoz(satir.Cell(tarihSutun.Value));
            if (tarih is null)
                continue;   // tarihi çözülemeyen satırı atla

            string? giris = girisSutun is null ? null
                : satir.Cell(girisSutun.Value).GetFormattedString().Trim();
            string? cikis = cikisSutun is null ? null
                : satir.Cell(cikisSutun.Value).GetFormattedString().Trim();

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

    /// <summary>Başlık metnini karşılaştırma için sadeleştirir (Türkçe karakter + boşluk).</summary>
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

    /// <summary>Bir hücreyi tarihe çevirir; çözülemezse null döner.</summary>
    private static DateOnly? TarihCoz(IXLCell hucre)
    {
        if (hucre.DataType == XLDataType.DateTime)
            return DateOnly.FromDateTime(hucre.GetDateTime());

        var metin = hucre.GetString().Trim();
        if (DateTime.TryParse(metin, new CultureInfo("tr-TR"),
                DateTimeStyles.None, out var dt))
            return DateOnly.FromDateTime(dt);

        return null;   // çözülemedi
    }
}