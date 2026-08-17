
namespace DijitalMufettis.Domain.Enums;

/// <summary>
/// Bir çalışanın özel yasal durumu. Günlük çalışma sınırını değiştirir.
/// Python'da ozel_durumlar sözlüğünde "Gebe"/"Emziren" string'i olarak tutuluyordu.
/// </summary>
public enum OzelDurum
{
    Yok,       // Özel durum yok — standart limitler geçerli (varsayılan)
    Gebe,      // Günde en fazla 7,5 saat
    Emziren    // Günde en fazla 6 saat
}