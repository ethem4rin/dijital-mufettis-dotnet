using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Application.Interface;

/// <summary>
/// PDKS kaynağından ham giriş-çıkış kayıtlarını okuyan bileşenin SÖZLEŞMESİ.
/// Application bu sözleşmeye bağımlıdır; okumanın ClosedXML ile mi, başka bir
/// yolla mı yapıldığını BİLMEZ (Dependency Inversion — SOLID "D").
/// Sözleşmeyi uygulayan somut sınıf Infrastructure katmanında yaşar.
/// </summary>
/// 
public interface IPdksOkuyucu
{
    /// <summary>
    /// verilen dosyayı okur ve ham günlük kayıtları döndürür.
    /// </summary>
    /// <param name="dosyaYolu"> Okunulacak excel dosyasının tam yolu </param>
    /// <return>okunan ham kaytıtlar</return>
    /// 
    IReadOnlyList<GunlukKayit> Oku(string dosyaYolu);
}