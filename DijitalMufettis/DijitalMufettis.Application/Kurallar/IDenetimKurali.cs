using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Application.Kurallar;

/// <summary>
/// Tüm denetim kurallarının ortak sözleşmesi (Strategy pattern).
/// Her kural, kayıtları ve ayarları alır; 0 veya daha fazla ihlal üretir.
/// </summary>
public interface IDenetimKurali
{
    IEnumerable<Ihlal> Uygula(IReadOnlyList<GunlukKayit> kayitlar, DenetimAyarlari ayarlar);
}