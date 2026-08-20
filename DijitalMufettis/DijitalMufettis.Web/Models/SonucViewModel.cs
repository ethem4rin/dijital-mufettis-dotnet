using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Web.Models;

/// <summary>
/// Denetim sonucu ekranına gönderilen veri: okunan kayıtlar + tespit edilen ihlaller.
/// </summary>
public class SonucViewModel
{
    public required IReadOnlyList<GunlukKayit> Kayitlar { get; init; }
    public required IReadOnlyList<Ihlal> Ihlaller { get; init; }
}