namespace DijitalMufettis.Domain.Models;

/// <summary>
/// Excel'den okunan tek bir ham giriş-çıkış satırı. Analiz motorunun GİRDİSİDİR.
/// (Python'daki df satırının karşılığı: ad_soyad, tarih, giris, cikis)
/// Giriş/çıkış saatleri ham metin olarak tutulur; ayrıştırma analiz aşamasında yapılır.
/// </summary>
public record GunlukKayit
{
    /// <summary>Personelin ad-soyadı.</summary>
    public required string Personel { get; init; }

    /// <summary>Kaydın günü.</summary>
    public required DateOnly Tarih { get; init; }

    /// <summary>Ham giriş saati metni (ör. "08:00"). Hücre boşsa null.</summary>
    public string? Giris { get; init; }

    /// <summary>Ham çıkış saati metni (ör. "17:30"). Hücre boşsa null.</summary>
    public string? Cikis { get; init; }
}