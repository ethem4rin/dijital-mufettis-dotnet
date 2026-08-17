# Dijital Müfettiş — C# / .NET Sürümü

PDKS (Personel Devam Kontrol Sistemi) Excel kayıtlarını **4857 Sayılı İş Kanunu**'na göre
denetleyen; yasal ihlalleri, tahmini idari para cezalarını ve hak edişleri raporlayan
web uygulaması.

> Bu proje, mevcut Python masaüstü sürümünün **ASP.NET Core MVC** ile yeniden yazımıdır.
> Öğrenme amaçlı geliştirilmektedir (Clean Architecture, SOLID, katmanlı mimari).

## Mimari (Clean Architecture)

```
DijitalMufettis.Web            → ASP.NET Core MVC (kullanıcı arayüzü)
DijitalMufettis.Infrastructure → Excel okuma (ClosedXML) ve dış dünya
DijitalMufettis.Application    → İş mantığı: denetim motoru + arayüzler (interface)
DijitalMufettis.Domain         → Çekirdek veri modelleri (kimseye bağımlı değil)
```

Bağımlılık yönü daima içeriye doğrudur: `Web → Infrastructure → Application → Domain`.

## Teknoloji

- .NET 9 / C#
- ASP.NET Core MVC
- ClosedXML (Excel okuma)

## Durum

🚧 Geliştirme aşamasında.

---
İbrahim Ethem Arın — © 2026
