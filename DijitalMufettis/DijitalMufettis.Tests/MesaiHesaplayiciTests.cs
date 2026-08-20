using DijitalMufettis.Application.Hesaplama;
using Xunit;

namespace DijitalMufettis.Tests;

public class MesaiHesaplayiciTests
{
    // Test edilecek nesne (her test taze bir tane kullanır)
    private readonly MesaiHesaplayici _hesaplayici = new();

    [Fact]
    public void NormalGunduzVardiyasi_NetCalismayiDogruHesaplar()
    {
        // Arrange (hazırla) + Act (çalıştır)
        var sonuc = _hesaplayici.Hesapla("09:00", "17:00");

        // Assert (doğrula): 8s ham − 60dk mola = 7s net
        Assert.NotNull(sonuc);
        Assert.Equal(7.0, sonuc!.NetCalismaSaati, 2);
    }

    [Fact]
    public void GeceYarisiniAsanVardiya_ErtesiGuneSarkarVeGeceHesaplanir()
    {
        var sonuc = _hesaplayici.Hesapla("22:00", "06:00");

        Assert.NotNull(sonuc);
        Assert.Equal(7.0, sonuc!.NetCalismaSaati, 2);   // 8s ham − 60dk = 7s
        Assert.Equal(8.0, sonuc.GeceCalismaSaati, 2);   // 22:00–06:00 tamamı gece
    }

    [Fact]
    public void TarihSaatFormatindakiHucreyiAyristirir()
    {
        var sonuc = _hesaplayici.Hesapla("2026-07-31 09:59:21", "2026-07-31 19:00:02");

        Assert.NotNull(sonuc);
        Assert.Equal(8.02, sonuc!.NetCalismaSaati, 2);
    }

    [Fact]
    public void GirisBossa_NullDoner()
    {
        var sonuc = _hesaplayici.Hesapla(null, "17:00");

        Assert.Null(sonuc);
    }
}