# Ticket Management Project Notes

## Projeden Ne Anlasildi?

Bu proje, staj degerlendirmesi icin hazirlanan bir Ticket/Talep Yonetimi uygulamasidir. Amac, kullanicilarin destek taleplerini listeleyebildigi, detaylarini gorebildigi, yeni ticket olusturabildigi, mevcut ticketlari guncelleyebildigi, durum ve atama islemlerini yonetebildigi tam calisan bir backend API ve Angular frontend arayuzu gelistirmektir.

Uygulama iki ana parcadan olusur:

- Backend: ASP.NET Core Web API ile gelistirilmis REST API.
- Frontend: Angular standalone component yapisiyla gelistirilmis responsive kullanici arayuzu.

## Kullanilan Teknolojiler

- Angular 20
- ASP.NET Core Web API
- PostgreSQL
- EF Core
- Swagger

## Backend Mimarisi

Backend, controller-based ASP.NET Core Web API mimarisiyle gelistirilmiştir. Minimal API kullanilmamis, endpointler controller siniflari uzerinden tanimlanmistir.

Temel katmanlar:

- `Models`: Veritabani entity modelleri bulunur. `User`, `Ticket`, `Comment` modelleri uygulamanin ana veri yapisini olusturur.
- `Dtos`: API istek ve cevaplarinda kullanilan DTO siniflari bulunur. Entity modelleri dogrudan frontend'e acilmaz.
- `Data`: `AppDbContext` bulunur. EF Core DbSet tanimlari, entity iliskileri ve seed data burada yonetilir.
- `Controllers`: HTTP endpointlerini saglayan controller siniflari bulunur. `TicketsController` ve `UsersController` API davranisini yonetir.

Entity iliskileri:

- Bir ticket istege bagli olarak bir kullaniciya atanabilir.
- Bir ticket birden fazla yoruma sahip olabilir.
- Ticket silindiginde ilgili yorumlar cascade delete ile silinir.
- Atanan kullanici silinirse ticket uzerindeki atama `null` olur.

Seed data olarak 3 kullanici, 5 ticket ve 3 yorum eklenmistir. Bu sayede veritabani olusturulduktan sonra uygulama ornek veriyle test edilebilir.

## Frontend Mimarisi

Frontend, Angular standalone component yaklasimina uygun sekilde gelistirilmistir. Sayfalar route bazli ayrilmis ve API erisimi servis katmanina tasinmistir.

Temel katmanlar:

- `core/models`: TypeScript interface dosyalari bulunur. API response ve request tipleri burada tanimlanir.
- `core/services`: HTTP servisleri ve localStorage tabanli ziyaret gecmisi servisi bulunur.
- `pages/ticket-list`: Ticket listeleme, arama ve filtreleme arayuzu.
- `pages/ticket-detail`: Ticket detayi, durum degistirme, atama degistirme ve yorum ekleme arayuzu.
- `pages/ticket-form`: Ticket olusturma ve guncelleme formu.
- `shared/components/recently-visited`: Son gezilen sayfalari gosteren bonus component.

`app.config.ts` icinde `provideHttpClient()` kullanilarak HTTP client aktif hale getirilir. `app.routes.ts` icinde asagidaki route yapisi kullanilir:

- `/tickets`
- `/tickets/new`
- `/tickets/:id`
- `/tickets/:id/edit`

Varsayilan route `/tickets` sayfasina yonlendirilir.

## Gelistirme Adimlari

1. Backend proje yapisi incelendi ve WeatherForecast ornek endpointleri kaldirildi.
2. `Models`, `Dtos`, `Data`, `Controllers` klasorleri olusturuldu.
3. `User`, `Ticket`, `Comment` modelleri validation kurallariyla eklendi.
4. DTO dosyalari olusturularak API veri sozlesmeleri ayrildi.
5. `AppDbContext` icinde DbSet tanimlari, iliskiler ve seed data eklendi.
6. `TicketsController` ve `UsersController` ile CRUD endpointleri yazildi.
7. PostgreSQL baglantisi, EF Core, Swagger ve CORS `Program.cs` icinde ayarlandi.
8. Angular frontend icin model, service ve page component yapisi olusturuldu.
9. Ticket listeleme, detay, olusturma ve guncelleme ekranlari gelistirildi.
10. localStorage tabanli "Son Gezdiklerim" bonus ozelligi eklendi.
11. Frontend API endpointleri backend controller endpointleriyle uyumlu hale getirildi.
12. Backend ve frontend build kontrolleri yapildi.

## CRUD Islemleri

Ticket yonetimi icin temel CRUD islemleri desteklenir:

- Create: `POST /api/Tickets` ile yeni ticket olusturulur.
- Read: `GET /api/Tickets` ile ticket listesi, `GET /api/Tickets/{id}` ile ticket detayi alinir.
- Update: `PUT /api/Tickets/{id}` ile ticket bilgileri guncellenir.
- Delete: `DELETE /api/Tickets/{id}` ile ticket silinir.

Ek islemler:

- `PATCH /api/Tickets/{id}/status`: Ticket durumu hizli sekilde guncellenir.
- `PATCH /api/Tickets/{id}/assign`: Ticket bir kullaniciya atanir veya atama kaldirilir.
- `POST /api/Tickets/{ticketId}/comments`: Ticket detayina yeni yorum eklenir.
- `GET /api/Users`: Atama listelerinde kullanilacak kullanicilar alinir.

## Recently Visited Bonus Ozelligi

Frontend tarafinda kullanicinin ziyaret ettigi ticket sayfalari localStorage'da saklanir. Bu ozellik `VisitedPagesService` tarafindan yonetilir.

Calisma sekli:

- Kullanici ticket listesi, ticket detayi veya ticket formu sayfalarina girdiginde sayfa bilgisi kaydedilir.
- Kayitlar localStorage icinde tutulur.
- Ayni sayfa tekrar ziyaret edilirse kayit en uste tasinir.
- Son ziyaret edilen sayfalar sag alt kosedeki "Son Gezdiklerim" kutusunda listelenir.
- Kullanici listedeki bir kayda tiklayarak ilgili route'a geri donebilir.

Bu bonus ozellik, kullanicinin uygulama icinde son baktigi ticketlara hizli donmesini saglar.
