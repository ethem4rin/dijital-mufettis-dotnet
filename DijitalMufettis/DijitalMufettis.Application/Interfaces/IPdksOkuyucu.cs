using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Application.Interfaces;

/// <summary>
/// PDKS kaynağından ham kayıtları okuyan bileşenin sözleşmesi.
/// İki aşamalı: önce Onizle (kullanıcı eşleştirsin), sonra Oku (haritayla).
/// </summary>
public interface IPdksOkuyucu
{
    /// <summary>
    /// Dosyanın ham önizlemesini (ilk birkaç satır) ve otomatik önerilen
    /// sütun haritasını döndürür. Kullanıcı eşleştirme ekranında bunu görür/düzeltir.
    /// </summary>
    OnizlemeSonucu Onizle(string dosyaYolu);

    /// <summary>
    /// Verilen sütun haritasına göre dosyayı okur ve günlük kayıtları döndürür.
    /// </summary>
    IReadOnlyList<GunlukKayit> Oku(string dosyaYolu, SutunHaritasi harita);
}