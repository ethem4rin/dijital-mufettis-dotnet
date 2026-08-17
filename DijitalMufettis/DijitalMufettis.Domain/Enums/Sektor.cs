namespace DijitalMufettis.Domain.Enums;

/// <summary>
/// Denetlenen işyerinin sektörü. Günlük/haftalık çalışma limitleri ve
/// gece çalışması istisnaları sektöre göre değişir (bkz. analiz kuralları).
/// </summary>
public enum Sektor
{
    Genel,       // Günlük 11s / haftalık 45s
    Saglik,      // Gece 7,5s aşımı → yazılı onay istisnası
    Turizm,      // Gece 7,5s aşımı → yazılı onay istisnası
    Guvenlik,    // Gece 7,5s aşımı → yazılı onay istisnası
    Madencilik   // Günlük 7,5s / haftalık 37,5s; yer altı fazla çalışma yasak
}