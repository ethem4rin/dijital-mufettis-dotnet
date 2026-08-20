using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Application.Hesaplama;

/// <summary>
/// Bir günlük giriş-çıkış saatinden net çalışma, gece çalışması ve molayı hesaplar.
/// Python'daki tek_vardiya_hesapla'nın C# karşılığı.
/// </summary>
public class MesaiHesaplayici
{
    public MesaiHesabi? Hesapla(string? girisMetni, string? cikisMetni)
    {
        var giris = SaatCoz(girisMetni);
        var cikis = SaatCoz(cikisMetni);
        if (giris is null || cikis is null)
            return null;   // saat ayrıştırılamadı → hesap yapılamaz

        // 1) Aynı referans güne oturt (gece yarısı mantığı için)
        var gun = DateOnly.FromDateTime(DateTime.Today);
        var girisDt = gun.ToDateTime(giris.Value);
        var cikisDt = gun.ToDateTime(cikis.Value);

        // 2) Çıkış girişten küçükse ertesi güne sarkmıştır (22:00 → 06:00)
        if (cikisDt < girisDt)
            cikisDt = cikisDt.AddDays(1);

        double hamDakika = (cikisDt - girisDt).TotalMinutes;
        double hamSaat = hamDakika / 60.0;

        // 3) Kademeli mola (yasal varsayılanlar)
        double mola;
        if (hamSaat <= 4.0) mola = 15;
        else if (hamSaat <= 7.5) mola = 30;
        else if (hamSaat <= 11.0) mola = 60;
        else mola = 90;

        double netDakika = Math.Max(0, hamDakika - mola);
        double netSaat = Math.Round(netDakika / 60.0, 2);

        // 4) Gece çalışması: [giriş,çıkış] ∩ [20:00, ertesi 06:00]
        var geceBas = gun.ToDateTime(new TimeOnly(20, 0));
        var geceBit = gun.ToDateTime(new TimeOnly(6, 0)).AddDays(1);
        var kesisimBas = girisDt > geceBas ? girisDt : geceBas;
        var kesisimBit = cikisDt < geceBit ? cikisDt : geceBit;

        double geceSaat = 0;
        if (kesisimBas < kesisimBit)
            geceSaat = Math.Round((kesisimBit - kesisimBas).TotalMinutes / 60.0, 2);

        return new MesaiHesabi
        {
            NetCalismaSaati = netSaat,
            GeceCalismaSaati = geceSaat,
            MolaDakika = mola
        };
    }

    /// <summary>"2026-07-31 09:59:21" veya "09:59" metninden saati çıkarır.</summary>
    private static TimeOnly? SaatCoz(string? metin)
    {
        if (string.IsNullOrWhiteSpace(metin))
            return null;

        var parca = metin.Trim();
        if (parca.Contains(' '))
            parca = parca.Split(' ')[^1];   // boşluk varsa son parçayı al (saat kısmı)
        if (parca.Length >= 5)
            parca = parca.Substring(0, 5);  // ilk 5 karakter = "HH:mm"

        return TimeOnly.TryParse(parca, out var saat) ? saat : null;
    }
}