using DijitalMufettis.Domain.Enums;

namespace DijitalMufettis.Domain.Models;
/// <summary>
/// tespit edilen tek bir yasal ihlali temseil eder değişmez
/// bir olgufur oluşuturulduktan sonra alanları değiştirilemez
/// </summary>
public record Ihlal
{
    /// <summary>İhlalin ait olduğu personelin ad-soyadı.</summary>
    public required string Personel { get; init; }

    /// <summary>İhlalin gerçekleştiği gün.</summary>
    public required DateOnly Tarih { get; init; }

    /// <summary>İhlalin kategorisi (filtreleme/sayım/renklendirme bu alana göre yapılır).</summary>
    public required IhlalKategorisi Kategori { get; init; }

    /// <summary>İhlalin serbest-metin başlığı, ör. "Günlük 11 Saat Sınırı Aşımı".</summary>
    public required string Tip { get; init; }

    /// <summary>İhlalin insan-okur açıklaması (rapordaki detay metni).</summary>
    public required string Detay { get; init; }
}
