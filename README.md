# 📰 NewsApp API

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-339933?style=for-the-badge&logo=nuget&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

A robust, scalable, and secure RESTful Web API designed for a comprehensive News Portal application. Built with ASP.NET Core, it features Role-Based Access Control (RBAC), JSON Web Token (JWT) authentication, and a clean, layered architecture utilizing the Generic Repository Pattern.

## ✨ Features

* **Authentication & Authorization:** Secure user registration and login using ASP.NET Core Identity and JWT.
* **Role Management:** Granular access control (e.g., Admin, Author, User) for endpoints.
* **News & Category Management:** Full CRUD operations for news articles and categories (Admin/Author restricted).
* **Interactive Comment System:** Authenticated users can comment on news. Users can delete their own comments, while Admins have global moderation rights.
* **Data Transfer Objects (DTO):** Seamless object mapping between database models and API responses using AutoMapper.
* **Generic Repository Pattern:** Clean and maintainable data access layer.

## 🛠️ Tech Stack

* **Framework:** .NET (ASP.NET Core Web API)
* **Language:** C#
* **ORM:** Entity Framework Core
* **Database:** MS SQL Server
* **Authentication:** ASP.NET Core Identity & JWT Bearer
* **Mapping:** AutoMapper
* **API Documentation:** Swagger / OpenAPI (v6.5.0)

## 🚀 Getting Started

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
* SQL Server (or LocalDB) running.

### Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/yourusername/NewsApp.API.git](https://github.com/yourusername/NewsApp.API.git)
    cd NewsApp.API
    ```

2.  **Configure the Database & JWT:**
    Open `appsettings.json` (or `appsettings.Development.json`) and update the `ConnectionStrings` and `Jwt:Key`:
    ```json
    "ConnectionStrings": {
      "sqlCon": "Server=(localdb)\\mssqllocaldb;Database=NewsAppDb;Trusted_Connection=True;"
    },
    "Jwt": {
      "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
      "Issuer": "http://localhost",
      "Audience": "http://localhost"
    }
    ```

3.  **Apply Migrations:**
    Create the database schema by running the following command in the Package Manager Console (PMC):
    ```powershell
    Update-Database
    ```

4.  **Run the Project:**
    Press `F5` in Visual Studio or run via CLI:
    ```bash
    dotnet run
    ```
    Navigate to `https://localhost:<port>/swagger` to access the API documentation.

## 📌 API Endpoints Summary

* `POST /api/Auth/Register` - Register a new user.
* `POST /api/Auth/Login` - Authenticate and get JWT token.
* `GET/POST/PUT/DELETE /api/Category` - Category management.
* `GET/POST/PUT/DELETE /api/News` - News management.
* `GET/POST/DELETE /api/Comment` - Comment management.
* `GET/PUT/DELETE /api/User` - Admin user management panel.

---

# 📰 NewsApp API (Türkçe)

Haber Portalı uygulamaları için tasarlanmış; güçlü, ölçeklenebilir ve güvenli bir RESTful Web API projesi. ASP.NET Core ile geliştirilen bu sistem, Rol Tabanlı Erişim Kontrolü (RBAC), JSON Web Token (JWT) yetkilendirmesi ve Generic Repository desenini kullanan temiz bir katmanlı mimari sunar.

## ✨ Özellikler

* **Kimlik Doğrulama ve Yetkilendirme:** ASP.NET Core Identity ve JWT kullanarak güvenli üyelik ve giriş işlemleri.
* **Rol Yönetimi:** Uç noktalar (endpoint) için detaylı erişim denetimi (örn: Yönetici, Yazar, Üye).
* **Haber ve Kategori Yönetimi:** Haberler ve kategoriler için tam CRUD (Ekle, Oku, Güncelle, Sil) işlemleri.
* **Etkileşimli Yorum Sistemi:** Giriş yapmış kullanıcılar haberlere yorum yapabilir. Kullanıcılar kendi yorumlarını silebilirken, Yöneticilerin tüm yorumlar üzerinde denetim yetkisi vardır.
* **Veri Transfer Objeleri (DTO):** AutoMapper entegrasyonu ile veritabanı modelleri ve API çıktıları arasında güvenli veri eşleşmesi.
* **Generic Repository Deseni:** Temiz, sürdürülebilir ve tekrar kullanılabilir veri erişim katmanı.

## 🛠️ Teknolojiler

* **Çatı (Framework):** .NET (ASP.NET Core Web API)
* **Dil:** C#
* **ORM:** Entity Framework Core
* **Veritabanı:** MS SQL Server
* **Yetkilendirme:** ASP.NET Core Identity & JWT Bearer
* **Eşleştirme:** AutoMapper
* **API Belgelendirmesi:** Swagger / OpenAPI (v6.5.0)

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
* Bilgisayarınızda [.NET SDK](https://dotnet.microsoft.com/download) yüklü olmalıdır.
* SQL Server (veya LocalDB) çalışır durumda olmalıdır.

### Adımlar

1.  **Projeyi bilgisayarınıza indirin:**
    ```bash
    git clone [https://github.com/kullaniciadiniz/NewsApp.API.git](https://github.com/kullaniciadiniz/NewsApp.API.git)
    cd NewsApp.API
    ```

2.  **Veritabanı ve JWT Ayarlarını Yapılandırın:**
    `appsettings.json` (veya `appsettings.Development.json`) dosyasını açıp veritabanı bağlantı metnini ve güvenli JWT anahtarınızı güncelleyin:
    ```json
    "ConnectionStrings": {
      "sqlCon": "Server=(localdb)\\mssqllocaldb;Database=NewsAppDb;Trusted_Connection=True;"
    },
    "Jwt": {
      "Key": "EnAz32KarakterUzunlugundaOlanCokGizliBirSifreBelirleyin!",
      "Issuer": "http://localhost",
      "Audience": "http://localhost"
    }
    ```

3.  **Veritabanını Oluşturun:**
    Package Manager Console (PMC) üzerinden şu komutu çalıştırarak veritabanı tablolarını oluşturun:
    ```powershell
    Update-Database
    ```

4.  **Projeyi Başlatın:**
    Visual Studio üzerinden `F5` tuşuna basın veya terminalden şu komutu girin:
    ```bash
    dotnet run
    ```
    API uç noktalarını test etmek için tarayıcınızda `https://localhost:<port>/swagger` adresine gidin.
