# İKYS Sistem Mimarisi

## 🏗️ Genel Mimari

İKYS projesi 3-katmanlı mimari (3-Tier Architecture) kullanılarak geliştirilmiştir.

```
┌─────────────────────┐
│   Presentation      │  ← MVC Layer (Controllers, Views, ViewModels)
│      Layer          │
├─────────────────────┤
│   Business Logic    │  ← BLL Layer (Services, DTOs, Validation)
│      Layer          │
├─────────────────────┤
│   Data Access       │  ← DAL Layer (Repositories, Context, Entities)
│      Layer          │
├─────────────────────┤
│     Database        │  ← SQL Server
└─────────────────────┘
```

## 📊 Veri Akışı

```
User Request → Controller → Service → Repository → Database
                    ↓         ↓          ↓
                ViewModel ← DTO ← Entity ← Data
```

## 🔧 Kullanılan Tasarım Desenleri

1. **Repository Pattern**: Veri erişim soyutlaması
2. **Unit of Work Pattern**: Transaction yönetimi
3. **DTO Pattern**: Katmanlar arası veri transferi
4. **Result Pattern**: Hata yönetimi
5. **Dependency Injection**: Bağımlılık enjeksiyonu

## 📁 Proje Yapısı

```
İKYS/
├── DAL/                    # Data Access Layer
├── BLL/                    # Business Logic Layer
├── MVC/                    # Presentation Layer
├── Documentation/          # Dokümantasyon
└── HR.sln                 # Solution dosyası
```
