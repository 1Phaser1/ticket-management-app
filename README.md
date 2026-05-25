# Ticket Management App

Ticket Management App, destek taleplerini yonetmek icin gelistirilmis full-stack bir uygulamadir. Proje; ASP.NET Core Web API backend, PostgreSQL veritabani ve Angular standalone component yapisina sahip frontend arayuzunden olusur.

Uygulama ile ticket listeleme, detay goruntuleme, yeni ticket olusturma, ticket guncelleme, durum degistirme, kullanici atama, ticket silme ve yorum ekleme islemleri yapilabilir.

## Teknolojiler

- Angular 20
- ASP.NET Core Web API
- .NET 9
- Entity Framework Core
- PostgreSQL
- Swagger
- TypeScript
- HTML/CSS
- Postman

## Proje Ozellikleri

- Controller-based REST API
- DTO tabanli request/response yapisi
- Entity Framework Core ile PostgreSQL entegrasyonu
- Ticket, kullanici ve yorum entity iliskileri
- Seed data ile ornek kullanici, ticket ve yorum kayitlari
- Swagger UI ile API test imkani
- Angular standalone component mimarisi
- Responsive ticket listeleme arayuzu
- Arama, status filtresi ve AssignedTo filtresi
- Ticket detay ekraninda status ve atama guncelleme
- Ticket detay ekraninda yorum listeleme ve yeni yorum ekleme
- Ticket olusturma ve guncelleme formu
- localStorage ile "Son Gezdiklerim" bonus ozelligi
- Postman collection ile endpoint testleri

## Klasor Yapisi

```text
ticket-management-app/
├── backend/
│   ├── Controllers/
│   ├── Data/
│   ├── Dtos/
│   ├── Models/
│   ├── Program.cs
│   ├── appsettings.json
│   └── backend.csproj
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/
│   │   │   │   ├── models/
│   │   │   │   └── services/
│   │   │   ├── pages/
│   │   │   │   ├── ticket-list/
│   │   │   │   ├── ticket-detail/
│   │   │   │   └── ticket-form/
│   │   │   └── shared/
│   │   │       └── components/
│   │   │           └── recently-visited/
│   │   └── styles.css
│   ├── angular.json
│   └── package.json
├── docs/
│   └── project-notes.md
├── postman/
│   └── ticket-management.postman_collection.json
└── README.md
```

## Kurulum Adimlari

### Gereksinimler

- .NET 9 SDK
- Node.js ve npm
- PostgreSQL
- Angular CLI

### 1. Repository klasorune gecin

```bash
cd C:\Users\kemal\ticket-management-app
```

### 2. Backend paketlerini geri yukleyin

```bash
cd backend
dotnet restore
```

### 3. Frontend paketlerini yukleyin

```bash
cd ..\frontend
npm install
```

## Guvenli Yapilandirma

Gercek secret bilgileri, veritabani sifreleri ve production connection string degerleri GitHub'a gonderilmemelidir. Bu nedenle `backend/appsettings.json` icindeki connection string alani bos tutulur:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  }
}
```

Backend calisirken connection string once `TICKET_DB_CONNECTION` environment variable degerinden okunur. Bu deger yoksa `ConnectionStrings:DefaultConnection` fallback olarak kontrol edilir. Ikisi de bos ise uygulama anlasilir bir hata firlatir.

Local ortamda environment variable PowerShell ile su sekilde tanimlanabilir:

```powershell
$env:TICKET_DB_CONNECTION="Host=localhost;Port=5432;Database=ticket_management_db;Username=postgres;Password=your_password"
```

Ornek connection string degeri [.env.example](.env.example) dosyasinda verilmiştir:

```text
TICKET_DB_CONNECTION=Host=localhost;Port=5432;Database=ticket_management_db;Username=postgres;Password=your_password
```

`.env`, `*.env` ve `backend/appsettings.Development.json` dosyalari root `.gitignore` icinde ignore edilir. Frontend API URL gizli bilgi degildir; tarayici tarafinda calistigi icin kullanici tarafindan gorulebilir.

## PostgreSQL Ayarlari

PostgreSQL tarafinda `ticket_management_db` adli veritabani olusturulmalidir. Backend bu veritabanina `TICKET_DB_CONNECTION` environment variable ile baglanir.

## Migration Komutlari

Backend klasorunde calistirin:

```bash
cd C:\Users\kemal\ticket-management-app\backend
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Daha once migration olusturulduysa yeni migration adi kullanilabilir:

```bash
dotnet ef migrations add AddTicketManagementTables
dotnet ef database update
```

## Backend Calistirma

```bash
cd C:\Users\kemal\ticket-management-app\backend
dotnet run --urls http://localhost:5004
```

Backend API varsayilan olarak asagidaki base URL ile kullanilir:

```text
http://localhost:5004
```

## Swagger URL

Backend calisirken Swagger UI:

```text
http://localhost:5004/swagger
```

Swagger uzerinden tum endpointler test edilebilir.

## Frontend Calistirma

```bash
cd C:\Users\kemal\ticket-management-app\frontend
npm start
```

Angular uygulamasi:

```text
http://localhost:4200
```

Varsayilan frontend route:

```text
http://localhost:4200/tickets
```

## API Endpointleri

Base URL:

```text
http://localhost:5004
```

| Method | Endpoint | Aciklama |
| --- | --- | --- |
| GET | `/api/Tickets` | Ticket listesini getirir |
| GET | `/api/Tickets/{id}` | Ticket detayini getirir |
| POST | `/api/Tickets` | Yeni ticket olusturur |
| PUT | `/api/Tickets/{id}` | Ticket bilgilerini gunceller |
| PATCH | `/api/Tickets/{id}/status` | Ticket durumunu gunceller |
| PATCH | `/api/Tickets/{id}/assign` | Ticket atamasini gunceller |
| DELETE | `/api/Tickets/{id}` | Ticket siler |
| POST | `/api/Tickets/{ticketId}/comments` | Ticket'a yorum ekler |
| GET | `/api/Users` | Kullanici listesini getirir |

## API Endpoint Ornekleri

### Ticket Listesi

```http
GET http://localhost:5004/api/Tickets
```

### Ticket Detayi

```http
GET http://localhost:5004/api/Tickets/1
```

### Yeni Ticket Olusturma

```http
POST http://localhost:5004/api/Tickets
Content-Type: application/json

{
  "title": "Login ekraninda hata",
  "description": "Kullanici dogru bilgilerle giris yapamiyor.",
  "status": "Open",
  "priority": "High",
  "assignedToId": 1
}
```

### Ticket Guncelleme

```http
PUT http://localhost:5004/api/Tickets/1
Content-Type: application/json

{
  "title": "Login ekraninda hata",
  "description": "Login hatasi detayli olarak incelendi.",
  "status": "In Progress",
  "priority": "High",
  "assignedToId": 2
}
```

### Status Guncelleme

```http
PATCH http://localhost:5004/api/Tickets/1/status
Content-Type: application/json

{
  "status": "Resolved"
}
```

### Ticket Atama

```http
PATCH http://localhost:5004/api/Tickets/1/assign
Content-Type: application/json

{
  "assignedToId": 1
}
```

### Yorum Ekleme

```http
POST http://localhost:5004/api/Tickets/1/comments
Content-Type: application/json

{
  "text": "Problem tekrar uretildi, loglar inceleniyor."
}
```

### Kullanici Listesi

```http
GET http://localhost:5004/api/Users
```

## Postman Collection

Postman collection dosyasi:

```text
postman/ticket-management.postman_collection.json
```

Postman icinde import edilerek tum backend endpointleri test edilebilir. Collection icinde `baseUrl` degiskeni varsayilan olarak `http://localhost:5004` seklinde tanimlidir.

## Frontend Route'lari

| Route | Aciklama |
| --- | --- |
| `/tickets` | Ticket listesi |
| `/tickets/new` | Yeni ticket olusturma |
| `/tickets/:id` | Ticket detayi |
| `/tickets/:id/edit` | Ticket guncelleme |

## Build Kontrolu

Backend:

```bash
cd backend
dotnet build
```

Frontend:

```bash
cd frontend
npm run build
```

Her iki build komutu da teslim oncesi calistirilmalidir.
