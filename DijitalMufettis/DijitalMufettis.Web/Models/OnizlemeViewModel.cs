using DijitalMufettis.Domain.Models;

namespace DijitalMufettis.Web.Models;

/// <summary>
/// Eşleştirme ekranına gönderilen veri: hangi dosya (GUID) + ham önizleme + öneri.
/// </summary>
public class OnizlemeViewModel
{
    public required string DosyaKimligi { get; init; }     // geçici dosyanın GUID'i
    public required OnizlemeSonucu Onizleme { get; init; }  // satırlar + öneri
}