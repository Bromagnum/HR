# İKYS (İnsan Kaynakları Yönetim Sistemi) - Test Raporu

## 📋 Genel Bakış

**Proje:** İnsan Kaynakları Yönetim Sistemi (IKYS)  
**Teknoloji:** ASP.NET Core 8.0 MVC  
**Veritabanı:** SQL Server  
**Test Tarihi:** 20 Eylül 2025  
**Test Ortamı:** Development Environment  

## 🏗️ Sistem Mimarisi

### Katman Yapısı
- **MVC:** Sunum katmanı (Controllers, Views, Models)
- **BLL:** İş mantığı katmanı (Business Logic Layer)
- **DAL:** Veri erişim katmanı (Data Access Layer)

## 🎯 Ana Modüller ve Test Senaryoları

### 1. **🔐 Kimlik Doğrulama ve Yetkilendirme Modülü**

**Controller:** `AuthController`  
**Servis:** `IAuthService`

#### Test Senaryoları:
```
✅ LOGIN TESTI
- URL: /Auth/Login
- Test Kullanıcıları:
  * Admin: admin@ikys.com / Admin123!
  * Manager: manager@ikys.com / Manager123!
  * Employee: employee@ikys.com / Employee123!

📋 Test Adımları:
1. Login sayfasına git
2. Geçerli kimlik bilgileri gir
3. "Giriş Yap" butonuna tıkla
4. Ana sayfaya yönlendirilmeyi kontrol et
5. Navbar'da kullanıcı adını kontrol et

❌ HATA SENARYOLARI:
- Yanlış email/şifre kombinasyonu
- Boş alanlar ile giriş denemesi
```

### 2. **🏠 Ana Sayfa ve Dashboard**

**Controller:** `HomeController`  
**URL:** `/Home/Index`

#### Test Senaryoları:
```
✅ DASHBOARD TESTI
📊 Kontrol Edilecek Kartlar:
1. Personel Sayısı Widget'ı
2. Departman Dağılımı Grafiği
3. İzin Durumu Grafiği
4. İşe Alım Trendi Grafiği
5. Stok Durumu Grafiği

📋 Test Adımları:
1. Admin olarak login ol
2. Dashboard'ın yüklendiğini kontrol et
3. Tüm grafiklerin görüntülendiğini kontrol et
4. Sayısal verilerin doğru gösterildiğini kontrol et
```

### 3. **👥 Personel Yönetimi Modülü**

**Controller:** `PersonController`  
**Servis:** `IPersonService`

#### Test Senaryoları:
```
✅ PERSONEL LİSTESİ
- URL: /Person
- Yetkiler: Admin (Tümü), Manager (Departmanı), Employee (Kendisi)

📋 Test Adımları:
1. Personel listesi sayfasına git
2. Filtreleme özelliklerini test et
3. Arama fonksiyonunu test et
4. Export (PDF/Word) özelliklerini test et

✅ PERSONEL EKLEME
- URL: /Person/Create
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. "Yeni Personel Ekle" butonuna tıkla
2. Tüm zorunlu alanları doldur
3. Form validasyonunu test et
4. Kaydet ve listeye geri dönmeyi kontrol et

✅ PERSONEL DÜZENLEME
- URL: /Person/Edit/{id}

📋 Test Adımları:
1. Bir personelin "Düzenle" butonuna tıkla
2. Bilgileri güncelle
3. Kaydet ve değişikliklerin yansıdığını kontrol et
```

### 4. **🏢 Departman Yönetimi Modülü**

**Controller:** `DepartmentController`  
**Servis:** `IDepartmentService`

#### Test Senaryoları:
```
✅ DEPARTMAN YÖNETİMİ
- URL: /Department
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Departman listesini görüntüle
2. Yeni departman ekle
3. Mevcut departmanı düzenle
4. Hiyerarşik yapıyı kontrol et
5. Export özelliklerini test et
```

### 5. **📋 Pozisyon Yönetimi Modülü**

**Controller:** `PositionController`  
**Servis:** `IPositionService`

#### Test Senaryoları:
```
✅ POZİSYON YÖNETİMİ
- URL: /Position
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Pozisyon listesini görüntüle
2. Yeni pozisyon tanımla
3. Pozisyon gereksinimlerini ekle
4. Maaş aralığı bilgilerini test et
```

### 6. **🗂️ İş İlanları Modülü**

**Controller:** `JobPostingController`  
**Servis:** `IJobPostingService`

#### Test Senaryoları:
```
✅ İŞ İLANI YÖNETİMİ
- URL: /JobPosting
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. İlan listesini görüntüle
2. Yeni iş ilanı oluştur
3. İlan durumunu güncelle (Taslak, Yayında, Süresi Dolmuş)
4. Public görünümü test et: /JobPosting/Public

✅ İŞ BAŞVURUSU
- URL: /JobApplication
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Başvuru listesini görüntüle
2. Başvuru detaylarını incele
3. Başvuru durumunu güncelle
4. CV dosyalarını kontrol et
```

### 7. **🏖️ İzin Yönetimi Modülü**

**Controller:** `LeaveController`  
**Servis:** `ILeaveService`, `ILeaveBalanceService`

#### Test Senaryoları:
```
✅ İZİN BAŞVURUSU
- URL: /Leave/Create
- Yetkiler: Tüm kullanıcılar

📋 Test Adımları:
1. İzin başvuru formu doldur
2. İzin türü seç
3. Tarih aralığı belirle
4. Onay sürecini takip et

✅ İZİN ONAY SÜRECİ
- URL: /Leave
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Bekleyen onayları görüntüle
2. İzni onayla/reddet
3. Onay notları ekle
4. E-mail bildirimlerini kontrol et

✅ İZİN BAKİYESİ
- URL: /LeaveBalance
- Yetkiler: Tüm kullanıcılar

📋 Test Adımları:
1. Yıllık izin bakiyesini kontrol et
2. Kullanılan izinleri görüntüle
3. İzin geçmişini incele
```

### 8. **⏰ Mesai Takip Modülü**

**Controller:** `WorkLogController`  
**Servis:** `IWorkLogService`

#### Test Senaryoları:
```
✅ MESAİ KAYDI
- URL: /WorkLog/CheckIn
- Yetkiler: Tüm kullanıcılar

📋 Test Adımları:
1. Mesaiye giriş yap
2. Mola başlat/bitir
3. Mesaiden çıkış yap
4. Günlük çalışma süresini kontrol et

✅ MESAİ RAPORLARI
- URL: /WorkLog
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Haftalık/Aylık raporları görüntüle
2. Fazla mesai raporunu incele
3. Geç gelme raporunu kontrol et
4. Export özelliklerini test et
```

### 9. **💰 Bordro Modülü**

**Controller:** `PayrollController`  
**Servis:** `IPayrollService`

#### Test Senaryoları:
```
✅ BORDRO YÖNETİMİ
- URL: /Payroll
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Aylık bordro listesini görüntüle
2. Yeni bordro oluştur
3. Maaş hesaplamalarını kontrol et
4. Kesintileri ve ödemeleri test et
5. PDF export işlemini test et

✅ BORDRO ÖZET RAPORLARI
- URL: /Payroll/PeriodSummary
- Yetkiler: Admin

📋 Test Adımları:
1. Dönem bazlı özet raporları
2. Departman karşılaştırmaları
3. Toplam maliyet analizleri
```

### 10. **📊 Performans Değerlendirme Modülü**

**Controller:** `PerformanceReviewController`  
**Servis:** `IPerformanceReviewService`

#### Test Senaryoları:
```
✅ PERFORMANS DEĞERLENDİRME
- URL: /PerformanceReview
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Değerlendirme formu oluştur
2. Hedefleri tanımla
3. Değerlendirme puanlarını gir
4. Gelişim planlarını ekle
```

### 11. **🎓 Eğitim ve Sertifika Modülü**

**Controller:** `EducationController`, `QualificationController`  
**Yetkiler:** Admin, Manager

#### Test Senaryoları:
```
✅ EĞİTİM YÖNETİMİ
- URL: /Education
📋 Test Adımları:
1. Eğitim programları listesi
2. Katılımcı takibi
3. Sertifika durumları

✅ NİTELİK YÖNETİMİ
- URL: /Qualification
📋 Test Adımları:
1. Personel niteliklerini görüntüle
2. Yeni nitelik tanımla
3. Sertifika geçerlilik takibi
```

### 12. **💼 Yetenek ve Beceri Yönetimi**

**Controller:** `SkillManagementController`  
**Servis:** `ISkillManagementService`

#### Test Senaryoları:
```
✅ YETENEK YÖNETİMİ
- URL: /SkillManagement
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Beceri havuzu oluştur
2. Personel becerilerini değerlendir
3. Beceri açığı analizleri
4. Gelişim önerilerini görüntüle
```

### 13. **🏗️ Organizasyon Yapısı**

**Controller:** `OrganizationController`  
**Yetkiler:** Admin

#### Test Senaryoları:
```
✅ ORGANİZASYON YÖNETİMİ
- URL: /Organization
📋 Test Adımları:
1. Organizasyon ağacını görüntüle
2. Hiyerarşik yapıyı güncelle
3. Yeni organizasyon birimi ekle
```

### 14. **📦 Malzeme Yönetimi**

**Controller:** `MaterialController`  
**Servis:** `IMaterialService`

#### Test Senaryoları:
```
✅ MALZEME YÖNETİMİ
- URL: /Material
- Yetkiler: Admin, Manager

📋 Test Adımları:
1. Malzeme envanterini görüntüle
2. Stok durumunu kontrol et
3. Yeni malzeme ekle
4. Stok hareketlerini takip et
```

### 15. **👤 Kullanıcı Yönetimi**

**Controller:** `UserManagementController`  
**Yetkiler:** Admin

#### Test Senaryoları:
```
✅ KULLANICI YÖNETİMİ
- URL: /UserManagement
📋 Test Adımları:
1. Kullanıcı listesini görüntüle
2. Yeni kullanıcı oluştur
3. Rol atamalarını yap
4. Kullanıcı durumunu güncelle (Aktif/Pasif)
5. Şifre sıfırlama işlemleri
```

## 🔧 API Testleri

### REST API Endpoints
```
📡 API Base URL: /api/

✅ TEST EDİLECEK ENDPOINT'LER:
- GET /api/Person - Personel listesi
- GET /api/Department - Departman listesi
- GET /api/Leave - İzin listesi
- GET /api/PerformanceReview - Performans değerlendirmeleri

📋 API Test Adımları:
1. Postman veya benzer tool kullan
2. Authorization header'ı ekle
3. JSON response formatını kontrol et
4. HTTP status code'ları doğrula
```

## 🚀 Performans Testleri

### Sayfa Yükleme Performansı
```
⏱️ Test Edilecek Sayfalar:
- Dashboard (Home/Index) - Max 3 saniye
- Personel Listesi - Max 2 saniye
- Bordro Listesi - Max 2 saniye
- Raporlar - Max 5 saniye

📊 Ölçüm Araçları:
- Browser Developer Tools
- Network tab ile resource yükleme süreleri
- Performance tab ile JavaScript performansı
```

## 🔒 Güvenlik Testleri

### Yetkilendirme Testleri
```
🛡️ Test Senaryoları:
1. Yetkisiz sayfaya erişim denemesi
2. URL manipülasyonu ile yetki aşımı
3. Farklı rol ile sayfa erişimi
4. Logout sonrası session kontrolü

📋 Test Adımları:
1. Employee kullanıcısı ile Admin sayfalarına erişim dene
2. URL'de ID değiştirerek başkasının verisine erişim dene
3. Session timeout işlemlerini kontrol et
```

## 📊 Rapor ve Export Testleri

### Export Özellikleri
```
📄 Test Edilecek Export Türleri:
- PDF Export (Personel listeleri, Bordrolar)
- Excel Export (Tüm listeler)
- Word Export (Raporlar)

📋 Test Adımları:
1. Export butonlarına tıkla
2. Dosya indirme işlemini kontrol et
3. İndirilen dosyanın açılabilirliğini test et
4. İçerik doğruluğunu kontrol et
```

## 🌐 Tarayıcı Uyumluluğu

### Desteklenen Tarayıcılar
```
🌍 Test Edilecek Tarayıcılar:
- Chrome (Son sürüm)
- Firefox (Son sürüm)
- Edge (Son sürüm)
- Safari (Mac üzerinde)

📱 Responsive Test:
- Desktop (1920x1080)
- Tablet (768x1024)
- Mobile (375x667)
```

## ⚠️ Bilinen Sorunlar ve Çözümler

### Port ve Process Yönetimi
```
❌ Sorun: "Could not copy apphost.exe" hatası
✅ Çözüm: 
1. taskkill /f /im dotnet.exe
2. dotnet clean
3. dotnet build

❌ Sorun: 405 Method Not Allowed
✅ Çözüm: Anti-forgery token kontrolü ve doğru routing
```

## 📋 Test Checklist

### Günlük Test Rutini
```
☐ Login işlemleri tüm roller ile
☐ Dashboard widget'ları yükleme
☐ Temel CRUD işlemleri
☐ Export özellikleri
☐ Search ve filter işlevleri
☐ Responsive tasarım
☐ Performance metrikleri
☐ Hata sayfaları (404, 500)
```

### Haftalık Kapsamlı Test
```
☐ Tüm modüllerin detaylı testi
☐ API endpoint'lerinin testi
☐ Güvenlik kontrollerinin testi
☐ Database backup ve restore
☐ Log dosyalarının incelenmesi
☐ Sistem kaynak kullanımı
```

## 📞 Test Desteği

**Test Hesapları:**
- **Admin:** admin@ikys.com / Admin123!
- **Manager:** manager@ikys.com / Manager123!  
- **Employee:** employee@ikys.com / Employee123!

**Test URL'leri:**
- **Development:** http://localhost:8080
- **Visual Studio:** https://localhost:7139

---

**Rapor Hazırlayan:** AI Assistant  
**Güncelleme Tarihi:** 20 Eylül 2025  
**Versiyon:** 1.0
