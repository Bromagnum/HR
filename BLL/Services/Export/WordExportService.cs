using BLL.DTOs;
using BLL.Services.Export;
using BLL.Utilities;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace BLL.Services.Export
{
    public class WordExportService : IWordExportService
    {
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly ILeaveService _leaveService;
    private readonly IPerformanceReviewService _performanceReviewService;
    private readonly ILogger<WordExportService> _logger;

        public WordExportService(
            IPersonService personService,
            IDepartmentService departmentService,
            ILeaveService leaveService,
            IPerformanceReviewService performanceReviewService,
            ILogger<WordExportService> logger)
        {
            _personService = personService;
            _departmentService = departmentService;
            _leaveService = leaveService;
            _performanceReviewService = performanceReviewService;
            _logger = logger;
        }

        public async Task<Result<byte[]>> GeneratePersonReportAsync(int personId)
        {
            try
            {
                var personResult = await _personService.GetByIdAsync(personId);
                if (!personResult.IsSuccess)
                    return Result<byte[]>.Fail($"Personel bulunamadı: {personResult.Message}");

                var person = personResult.Data;
                using var memoryStream = new MemoryStream();
                
                using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
                {
                    // Ana belge parçası oluştur
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // Başlık
                    AddHeading(body, "PERSONEL RAPORU");
                    AddEmptyLine(body);

                    // Personel bilgileri
                    AddHeading(body, "Kişisel Bilgiler", 2);
                    AddParagraph(body, $"Ad Soyad: {person.FirstName} {person.LastName}");
                    AddParagraph(body, $"E-posta: {person.Email}");
                    AddParagraph(body, $"Telefon: {person.Phone ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Adres: {person.Address ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Doğum Tarihi: {person.BirthDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"TC Kimlik No: {person.TcKimlikNo ?? "Belirtilmemiş"}");
                    AddEmptyLine(body);

                    // İş bilgileri
                    AddHeading(body, "İş Bilgileri", 2);
                    AddParagraph(body, $"Personel No: {person.EmployeeNumber ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"İşe Başlama Tarihi: {person.HireDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Pozisyon: {person.Position ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Departman: {person.DepartmentName ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Maaş: {person.Salary?.ToString("C", new CultureInfo("tr-TR")) ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Durum: {(person.IsActive ? "Aktif" : "Pasif")}");
                    AddEmptyLine(body);

                    // Rapor tarihi
                    AddParagraph(body, $"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}");

                    mainPart.Document.Save();
                }

                var result = memoryStream.ToArray();
                _logger.LogInformation($"Personel raporu oluşturuldu. PersonId: {personId}");
                return Result<byte[]>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Personel raporu oluşturulurken hata oluştu. PersonId: {personId}");
                return Result<byte[]>.Fail($"Rapor oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateDepartmentReportAsync(int departmentId)
        {
            try
            {
                var departmentResult = await _departmentService.GetByIdAsync(departmentId);
                if (!departmentResult.IsSuccess)
                    return Result<byte[]>.Fail($"Departman bulunamadı: {departmentResult.Message}");

                var department = departmentResult.Data;
                using var memoryStream = new MemoryStream();
                
                using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // Başlık
                    AddHeading(body, "DEPARTMAN RAPORU");
                    AddEmptyLine(body);

                    // Departman bilgileri
                    AddHeading(body, "Departman Bilgileri", 2);
                    AddParagraph(body, $"Departman Adı: {department.Name}");
                    AddParagraph(body, $"Açıklama: {department.Description ?? "Belirtilmemiş"}");
                    // Note: Location and Budget properties are not available in DepartmentDetailDto
                    // AddParagraph(body, $"Lokasyon: {department.Location ?? "Belirtilmemiş"}");
                    // AddParagraph(body, $"Bütçe: {department.Budget?.ToString("C", new CultureInfo("tr-TR")) ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Durum: {(department.IsActive ? "Aktif" : "Pasif")}");
                    AddParagraph(body, $"Oluşturma Tarihi: {department.CreatedAt:dd.MM.yyyy}");
                    AddEmptyLine(body);

                    // Rapor tarihi
                    AddParagraph(body, $"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}");

                    mainPart.Document.Save();
                }

                var result = memoryStream.ToArray();
                _logger.LogInformation($"Departman raporu oluşturuldu. DepartmentId: {departmentId}");
                return Result<byte[]>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Departman raporu oluşturulurken hata oluştu. DepartmentId: {departmentId}");
                return Result<byte[]>.Fail($"Rapor oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateLeaveReportAsync(DateTime startDate, DateTime endDate, int? departmentId = null)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                
                using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // Başlık
                    AddHeading(body, "İZİN RAPORU");
                    AddEmptyLine(body);

                    // Rapor bilgileri
                    AddHeading(body, "Rapor Bilgileri", 2);
                    AddParagraph(body, $"Rapor Dönemi: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
                    if (departmentId.HasValue)
                    {
                        var departmentResult = await _departmentService.GetByIdAsync(departmentId.Value);
                        if (departmentResult.IsSuccess)
                            AddParagraph(body, $"Departman: {departmentResult.Data.Name}");
                    }
                    else
                    {
                        AddParagraph(body, "Departman: Tüm Departmanlar");
                    }
                    AddEmptyLine(body);

                    // İzin özeti (Bu kısımda gerçek izin verilerini alacak servis çağrıları yapılacak)
                    AddHeading(body, "İzin Özeti", 2);
                    AddParagraph(body, "Toplam İzin Sayısı: [Dinamik olarak hesaplanacak]");
                    AddParagraph(body, "Onaylanan İzinler: [Dinamik olarak hesaplanacak]");
                    AddParagraph(body, "Bekleyen İzinler: [Dinamik olarak hesaplanacak]");
                    AddParagraph(body, "Reddedilen İzinler: [Dinamik olarak hesaplanacak]");
                    AddEmptyLine(body);

                    // Rapor tarihi
                    AddParagraph(body, $"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}");

                    mainPart.Document.Save();
                }

                var result = memoryStream.ToArray();
                _logger.LogInformation($"İzin raporu oluşturuldu. Dönem: {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}, DepartmentId: {departmentId}");
                return Result<byte[]>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"İzin raporu oluşturulurken hata oluştu. Dönem: {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}");
                return Result<byte[]>.Fail($"Rapor oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GeneratePerformanceReportAsync(int personId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var personResult = await _personService.GetByIdAsync(personId);
                if (!personResult.IsSuccess)
                    return Result<byte[]>.Fail($"Personel bulunamadı: {personResult.Message}");

                var person = personResult.Data;
                using var memoryStream = new MemoryStream();
                
                using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // Başlık
                    AddHeading(body, "PERFORMANS RAPORU");
                    AddEmptyLine(body);

                    // Personel bilgileri
                    AddHeading(body, "Personel Bilgileri", 2);
                    AddParagraph(body, $"Ad Soyad: {person.FirstName} {person.LastName}");
                    AddParagraph(body, $"Departman: {person.DepartmentName ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Pozisyon: {person.Position ?? "Belirtilmemiş"}");
                    AddEmptyLine(body);

                    // Dönem bilgileri
                    AddHeading(body, "Değerlendirme Dönemi", 2);
                    AddParagraph(body, $"Başlangıç Tarihi: {startDate:dd.MM.yyyy}");
                    AddParagraph(body, $"Bitiş Tarihi: {endDate:dd.MM.yyyy}");
                    AddEmptyLine(body);

                    // Performans değerlendirmesi
                    AddHeading(body, "Performans Değerlendirmesi", 2);
                    
                    var performanceResult = await _performanceReviewService.GetByPersonIdAsync(personId);
                    if (performanceResult.IsSuccess && performanceResult.Data!.Any())
                    {
                        var latestReview = performanceResult.Data!.OrderByDescending(r => r.CreatedAt).First();
                        AddParagraph(body, $"Son Değerlendirme: {latestReview.ReviewPeriodName}");
                        AddParagraph(body, $"Genel Skor: {latestReview.OverallScore}/5");
                        AddParagraph(body, $"Durum: {latestReview.StatusText}");
                        AddParagraph(body, $"Değerlendiren: {latestReview.ReviewerName}");
                        if (latestReview.SubmittedAt.HasValue)
                        {
                            AddParagraph(body, $"Tamamlanma Tarihi: {latestReview.SubmittedAt.Value:dd.MM.yyyy}");
                        }
                    }
                    else
                    {
                        AddParagraph(body, "Henüz performans değerlendirmesi bulunmamaktadır.");
                    }
                    AddEmptyLine(body);

                    // Rapor tarihi
                    AddParagraph(body, $"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}");

                    mainPart.Document.Save();
                }

                var result = memoryStream.ToArray();
                _logger.LogInformation($"Performans raporu oluşturuldu. PersonId: {personId}, Dönem: {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}");
                return Result<byte[]>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Performans raporu oluşturulurken hata oluştu. PersonId: {personId}");
                return Result<byte[]>.Fail($"Rapor oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GeneratePayrollReportAsync(int personId, int year, int month)
        {
            try
            {
                var personResult = await _personService.GetByIdAsync(personId);
                if (!personResult.IsSuccess)
                    return Result<byte[]>.Fail($"Personel bulunamadı: {personResult.Message}");

                var person = personResult.Data;
                using var memoryStream = new MemoryStream();
                
                using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // Başlık
                    AddHeading(body, "BORDRO");
                    AddEmptyLine(body);

                    // Personel bilgileri
                    AddHeading(body, "Personel Bilgileri", 2);
                    AddParagraph(body, $"Ad Soyad: {person.FirstName} {person.LastName}");
                    AddParagraph(body, $"Personel No: {person.EmployeeNumber ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Departman: {person.DepartmentName ?? "Belirtilmemiş"}");
                    AddParagraph(body, $"Pozisyon: {person.Position ?? "Belirtilmemiş"}");
                    AddEmptyLine(body);

                    // Dönem bilgileri
                    AddHeading(body, "Bordro Dönemi", 2);
                    AddParagraph(body, $"Yıl: {year}");
                    AddParagraph(body, $"Ay: {new DateTime(year, month, 1):MMMM}");
                    AddEmptyLine(body);

                    // Maaş bilgileri
                    AddHeading(body, "Maaş Bilgileri", 2);
                    AddParagraph(body, $"Brüt Maaş: {person.Salary?.ToString("C", new CultureInfo("tr-TR")) ?? "Belirtilmemiş"}");
                    AddParagraph(body, "Kesintiler: [Bordro modülü geliştirildiğinde hesaplanacak]");
                    AddParagraph(body, "Net Maaş: [Bordro modülü geliştirildiğinde hesaplanacak]");
                    AddEmptyLine(body);

                    // Rapor tarihi
                    AddParagraph(body, $"Bordro Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}");

                    mainPart.Document.Save();
                }

                var result = memoryStream.ToArray();
                _logger.LogInformation($"Bordro oluşturuldu. PersonId: {personId}, Dönem: {year}/{month}");
                return Result<byte[]>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Bordro oluşturulurken hata oluştu. PersonId: {personId}");
                return Result<byte[]>.Fail($"Rapor oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateOrganizationReportAsync()
        {
            try
            {
                using var memoryStream = new MemoryStream();
                
                using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // Başlık
                    AddHeading(body, "ORGANİZASYON RAPORU");
                    AddEmptyLine(body);

                    // Genel bilgiler
                    AddHeading(body, "Genel Bilgiler", 2);
                    AddParagraph(body, $"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}");
                    AddEmptyLine(body);

                    // Departman özeti
                    AddHeading(body, "Departman Özeti", 2);
                    AddParagraph(body, "[Dinamik olarak departman sayısı ve detayları gösterilecek]");
                    AddEmptyLine(body);

                    // Personel özeti
                    AddHeading(body, "Personel Özeti", 2);
                    AddParagraph(body, "[Dinamik olarak toplam personel sayısı gösterilecek]");
                    AddEmptyLine(body);

                    // İstatistikler
                    AddHeading(body, "İstatistikler", 2);
                    AddParagraph(body, "[Çeşitli organizasyon istatistikleri gösterilecek]");

                    mainPart.Document.Save();
                }

                var result = memoryStream.ToArray();
                _logger.LogInformation("Organizasyon raporu oluşturuldu");
                return Result<byte[]>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Organizasyon raporu oluşturulurken hata oluştu");
                return Result<byte[]>.Fail($"Rapor oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        #region Helper Methods

        private void AddHeading(Body body, string text, int level = 1)
        {
            var paragraph = new Paragraph();
            var run = new Run();
            var runProperties = new RunProperties();

            // Başlık seviyesine göre formatlamayı ayarla
            runProperties.Bold = new Bold();
            runProperties.FontSize = new FontSize() { Val = level switch
            {
                1 => "24",
                2 => "18", 
                3 => "14",
                _ => "12"
            }};

            run.AppendChild(runProperties);
            run.AppendChild(new Text(text));
            paragraph.AppendChild(run);

            // Paragraf merkezleme (sadece ana başlık için)
            if (level == 1)
            {
                var paragraphProperties = new ParagraphProperties();
                paragraphProperties.Justification = new Justification() { Val = JustificationValues.Center };
                paragraph.PrependChild(paragraphProperties);
            }

            body.AppendChild(paragraph);
        }

        private void AddParagraph(Body body, string text)
        {
            var paragraph = new Paragraph();
            var run = new Run();
            run.AppendChild(new Text(text));
            paragraph.AppendChild(run);
            body.AppendChild(paragraph);
        }

        private void AddEmptyLine(Body body)
        {
            var paragraph = new Paragraph();
            var run = new Run();
            run.AppendChild(new Text(""));
            paragraph.AppendChild(run);
            body.AppendChild(paragraph);
        }

        #endregion
    }
}
