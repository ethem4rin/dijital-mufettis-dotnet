namespace DijitalMufettis.Domain.Models;

/// <summary>
/// Bir PDKS sayfasının nasıl okunacağını tarif eden "harita".
/// Kullanıcının eşleştirme ekranında yaptığı seçimleri taşır.
/// (Python'daki 'harita' sözlüğünün + baslik_index'in type-safe karşılığı.)
/// Satır/sütun numaraları 1-tabanlıdır (Excel/ClosedXML ile uyumlu).
/// </summary>
public record SutunHaritasi
{
    /// <summary>Başlık satırının numarası (veriler bunun ALTINDAN başlar).</summary>
    public required int BaslikSatiri { get; init; }

    /// <summary>Ad (veya ad-soyad birleşik) sütununun numarası.</summary>
    public required int AdSutun { get; init; }

    /// <summary>Soyad sütunu. null ise ad-soyad tek sütunda birleşiktir.</summary>
    public int? SoyadSutun { get; init; }

    /// <summary>Tarih sütununun numarası.</summary>
    public required int TarihSutun { get; init; }

    /// <summary>Giriş saati sütunu. null ise okunmaz.</summary>
    public int? GirisSutun { get; init; }

    /// <summary>Çıkış saati sütunu. null ise okunmaz.</summary>
    public int? CikisSutun { get; init; }
}