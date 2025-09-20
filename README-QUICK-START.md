# IKYS - Quick Start Guide

## 🚀 Hızlı Başlatma

### Seçenek 1: Otomatik Script (Önerilen)
```powershell
# PowerShell'i admin olarak açın, proje klasörüne gidin:
cd C:\Users\GÖKTUĞ\IKYS

# Script'i çalıştırın:
.\run-ikys-clean.ps1
```

### Seçenek 2: Manuel Başlatma
```bash
# 1. Tüm process'leri durdur
taskkill /f /im dotnet.exe

# 2. Build artifacts temizle
dotnet clean

# 3. Build
dotnet build

# 4. Çalıştır
dotnet run --project MVC/MVC.csproj --urls "http://localhost:5000"
```

### Seçenek 3: Visual Studio
1. Visual Studio'da "IKYS_Development" profile'ını seçin
2. Ctrl+F5 (Start Without Debugging) tuşuna basın

## 🌐 Test Sayfaları

| URL | Açıklama |
|-----|----------|
| `http://localhost:8080/Home/Welcome` | ✅ Basit hoş geldin sayfası (her zaman çalışır) |
| `http://localhost:8080/` | ✅ Ana dashboard |
| `http://localhost:8080/Auth/Login` | ✅ Giriş sayfası |
| `http://localhost:8080/static-login.html` | ✅ Static HTML giriş sayfası (405 hatası için) |

## 🔑 Test Kullanıcıları

| Rol | Email | Şifre |
|-----|-------|-------|
| Admin | admin@ikys.com | Admin123! |
| Manager | manager@ikys.com | Manager123! |
| Employee | employee@ikys.com | Employee123! |

## 🔧 Sorun Giderme

### Eğer sayfa açılmıyorsa:
1. **Visual Studio'yu kapatın**
2. **Task Manager'dan tüm dotnet.exe process'lerini sonlandırın**
3. **PowerShell'i admin olarak açın**
4. **`.\run-ikys-clean.ps1` script'ini çalıştırın**

### Eğer build hatası alıyorsanız:
1. **Proje path'inin doğru olduğundan emin olun: `C:\Users\GÖKTUĞ\IKYS`**
2. **Visual Studio'yu kapatın**
3. **`dotnet clean` ve `dotnet build` komutlarını çalıştırın**

### Eğer port çakışması varsa:
1. **`netstat -ano | findstr :5000` ile port kontrolü yapın**
2. **Çakışan process'i `taskkill /f /pid [PID]` ile sonlandırın**

## ✅ Başarı Kriterleri

Uygulama başarıyla çalışıyorsa:
- ✅ Build: 0 error (warnings normal)
- ✅ HTTP 200 responses
- ✅ Welcome page erişilebilir
- ✅ Login page çalışıyor
- ✅ Dashboard yükleniyor

## 📞 Destek

Sorun yaşamaya devam ederseniz:
1. **Error mesajlarını tam olarak kopyalayın**
2. **Hangi adımda hata aldığınızı belirtin**
3. **Browser console'undaki hataları kontrol edin**
