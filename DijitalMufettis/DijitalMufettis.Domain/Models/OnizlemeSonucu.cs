namespace DijitalMufettis.Domain.Models;

/// <summary>
/// Bir Excel dosyasının eşleştirme ekranında gösterilecek ham önizlemesi
/// ve programın otomatik ürettiği sütun haritası önerisi.
/// </summary>
public record OnizlemeSonucu
{
    /// <summary>
    /// Ham satırlar (ilk ~15 satır). Her satır, o satırdaki hücre metinlerinin listesidir.
    /// Yani: "satırların listesi; her satır da hücrelerin listesi" = 2 boyutlu tablo.
    /// </summary>
    public required IReadOnlyList<IReadOnlyList<string>> Satirlar { get; init; }

    /// <summary>Sayfadaki sütun sayısı (önizleme tablosunun genişliği).</summary>
    public required int SutunSayisi { get; init; }

    /// <summary>Otomatik önerilen harita. Tespit edilemezse null.</summary>
    public SutunHaritasi? OnerilenHarita { get; init; }
}