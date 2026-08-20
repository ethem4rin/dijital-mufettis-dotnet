namespace DijitalMufettis.Domain.Models;

/// <summary>
/// Bir günlük giriş-çıkıştan hesaplanan mesai bilgisi.
/// (Python tek_vardiya_hesapla'nın döndürdüğü net/gece/mola üçlüsü.)
/// </summary>
public record MesaiHesabi
{
    /// <summary>Mola düşüldükten sonraki net çalışma (saat, ör. 8.02).</summary>
    public required double NetCalismaSaati { get; init; }

    /// <summary>Gece dönemine (20:00–06:00) denk gelen çalışma (saat).</summary>
    public required double GeceCalismaSaati { get; init; }

    /// <summary>Düşülen mola (dakika).</summary>
    public required double MolaDakika { get; init; }
}