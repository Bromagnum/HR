using BLL.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace BLL.Services.Export;

public class PdfExportService : IPdfExportService
{
    private readonly CultureInfo _turkishCulture;

    public PdfExportService()
    {
        _turkishCulture = new CultureInfo("tr-TR");
        
        // QuestPDF License Configuration
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> ExportPersonnelListAsync(IEnumerable<PersonListDto> personnel, bool includeDetails = true)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Personel Listesi")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Summary
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Toplam Personel: {personnel.Count()}").FontSize(12);
                            row.RelativeItem().Text($"Rapor Tarihi: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", _turkishCulture)}").FontSize(12);
                        });

                        // Personnel Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Ad Soyad
                                columns.RelativeColumn(2); // Departman
                                columns.RelativeColumn(2); // Pozisyon
                                columns.RelativeColumn(1); // Durum
                                if (includeDetails)
                                {
                                    columns.RelativeColumn(1); // Telefon
                                    columns.RelativeColumn(2); // Email
                                }
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Ad Soyad").SemiBold();
                                header.Cell().Element(CellStyle).Text("Departman").SemiBold();
                                header.Cell().Element(CellStyle).Text("Pozisyon").SemiBold();
                                header.Cell().Element(CellStyle).Text("Durum").SemiBold();
                                if (includeDetails)
                                {
                                    header.Cell().Element(CellStyle).Text("Telefon").SemiBold();
                                    header.Cell().Element(CellStyle).Text("Email").SemiBold();
                                }
                            });

                            // Data
                            foreach (var person in personnel)
                            {
                                table.Cell().Element(CellStyle).Text($"{person.FirstName} {person.LastName}");
                                table.Cell().Element(CellStyle).Text(person.DepartmentName ?? "-");
                                table.Cell().Element(CellStyle).Text(person.PositionName ?? "-");
                                table.Cell().Element(CellStyle).Text(person.IsActive ? "Aktif" : "Pasif");
                                if (includeDetails)
                                {
                                    table.Cell().Element(CellStyle).Text(person.Phone ?? "-");
                                    table.Cell().Element(CellStyle).Text(person.Email ?? "-");
                                }
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportPersonnelDetailAsync(int personId, PersonDetailDto person)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Text($"Personel Detay Raporu - {person.FirstName} {person.LastName}")
                    .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(15);

                        // Basic Information
                        column.Item().Text("Kişisel Bilgiler").FontSize(14).SemiBold();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table.Cell().Text("Ad Soyad:").SemiBold();
                            table.Cell().Text($"{person.FirstName} {person.LastName}");
                            table.Cell().Text("Email:").SemiBold();
                            table.Cell().Text(person.Email ?? "-");
                            table.Cell().Text("Telefon:").SemiBold();
                            table.Cell().Text(person.Phone ?? "-");
                            table.Cell().Text("Doğum Tarihi:").SemiBold();
                            table.Cell().Text(person.BirthDate?.ToString("dd.MM.yyyy", _turkishCulture) ?? "-");
                            table.Cell().Text("TC Kimlik No:").SemiBold();
                            table.Cell().Text(person.TcKimlikNo ?? "-");
                        });

                        // Employment Information
                        column.Item().Text("İş Bilgileri").FontSize(14).SemiBold();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table.Cell().Text("Departman:").SemiBold();
                            table.Cell().Text(person.DepartmentName ?? "-");
                            table.Cell().Text("Pozisyon:").SemiBold();
                            table.Cell().Text(person.Position ?? "-");
                            table.Cell().Text("İşe Giriş Tarihi:").SemiBold();
                            table.Cell().Text(person.HireDate?.ToString("dd.MM.yyyy", _turkishCulture) ?? "-");
                            table.Cell().Text("Durum:").SemiBold();
                            table.Cell().Text(person.IsActive ? "Aktif" : "Pasif");
                        });

                        // Address Information
                        if (!string.IsNullOrEmpty(person.Address))
                        {
                            column.Item().Text("Adres Bilgileri").FontSize(14).SemiBold();
                            column.Item().Text(person.Address);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text($"Rapor Tarihi: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", _turkishCulture)}");
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportDepartmentListAsync(IEnumerable<DepartmentListDto> departments, bool includeHierarchy = true)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Departman Listesi")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Summary
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Toplam Departman: {departments.Count()}").FontSize(12);
                            row.RelativeItem().Text($"Aktif Departman: {departments.Count(d => d.IsActive)}").FontSize(12);
                        });

                        // Department Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Departman Adı
                                columns.RelativeColumn(2); // Açıklama
                                columns.RelativeColumn(1); // Durum
                                columns.RelativeColumn(1); // Personel Sayısı
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Departman Adı").SemiBold();
                                header.Cell().Element(CellStyle).Text("Açıklama").SemiBold();
                                header.Cell().Element(CellStyle).Text("Durum").SemiBold();
                                header.Cell().Element(CellStyle).Text("Personel").SemiBold();
                            });

                            // Data
                            foreach (var department in departments)
                            {
                                table.Cell().Element(CellStyle).Text(department.Name);
                                table.Cell().Element(CellStyle).Text(department.Description ?? "-");
                                table.Cell().Element(CellStyle).Text(department.IsActive ? "Aktif" : "Pasif");
                                table.Cell().Element(CellStyle).Text(department.EmployeeCount.ToString());
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportOrganizationChartAsync(IEnumerable<DepartmentListDto> departments)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Organizasyon Şeması")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        var activeDepts = departments.Where(d => d.IsActive).ToList();
                        
                        foreach (var dept in activeDepts)
                        {
                            column.Item().Border(1).Padding(10).Column(deptColumn =>
                            {
                                deptColumn.Item().Text(dept.Name).FontSize(12).SemiBold();
                                deptColumn.Item().Text(dept.Description ?? "").FontSize(10);
                                deptColumn.Item().Text($"Personel Sayısı: {dept.EmployeeCount}").FontSize(9);
                            });
                            column.Item().Height(10);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text($"Rapor Tarihi: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", _turkishCulture)}");
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportLeaveReportAsync(IEnumerable<LeaveListDto> leaves, DateTime? startDate = null, DateTime? endDate = null)
    {
        var filteredLeaves = leaves.AsEnumerable();
        if (startDate.HasValue)
            filteredLeaves = filteredLeaves.Where(l => l.StartDate >= startDate.Value);
        if (endDate.HasValue)
            filteredLeaves = filteredLeaves.Where(l => l.EndDate <= endDate.Value);

        var leaveList = filteredLeaves.ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("İzin Raporu")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Summary
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Toplam İzin: {leaveList.Count}").FontSize(12);
                            row.RelativeItem().Text($"Tarih Aralığı: {startDate?.ToString("dd.MM.yyyy", _turkishCulture) ?? "Başlangıç"} - {endDate?.ToString("dd.MM.yyyy", _turkishCulture) ?? "Bitiş"}").FontSize(12);
                        });

                        // Leave Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Personel
                                columns.RelativeColumn(2); // İzin Türü
                                columns.RelativeColumn(2); // Başlangıç
                                columns.RelativeColumn(2); // Bitiş
                                columns.RelativeColumn(1); // Gün
                                columns.RelativeColumn(1); // Durum
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Personel").SemiBold();
                                header.Cell().Element(CellStyle).Text("İzin Türü").SemiBold();
                                header.Cell().Element(CellStyle).Text("Başlangıç").SemiBold();
                                header.Cell().Element(CellStyle).Text("Bitiş").SemiBold();
                                header.Cell().Element(CellStyle).Text("Gün").SemiBold();
                                header.Cell().Element(CellStyle).Text("Durum").SemiBold();
                            });

                            // Data
                            foreach (var leave in leaveList)
                            {
                                table.Cell().Element(CellStyle).Text(leave.PersonName ?? "-");
                                table.Cell().Element(CellStyle).Text(leave.LeaveTypeName ?? "-");
                                table.Cell().Element(CellStyle).Text(leave.StartDate.ToString("dd.MM.yyyy", _turkishCulture));
                                table.Cell().Element(CellStyle).Text(leave.EndDate.ToString("dd.MM.yyyy", _turkishCulture));
                                table.Cell().Element(CellStyle).Text(leave.TotalDays.ToString());
                                table.Cell().Element(CellStyle).Text(GetLeaveStatusText(leave.Status));
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportLeaveBalanceReportAsync(IEnumerable<LeaveBalanceListDto> leaveBalances)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("İzin Bakiye Raporu")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Leave Balance Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Personel
                                columns.RelativeColumn(2); // İzin Türü
                                columns.RelativeColumn(1); // Kullanılan
                                columns.RelativeColumn(1); // Kalan
                                columns.RelativeColumn(1); // Toplam
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Personel").SemiBold();
                                header.Cell().Element(CellStyle).Text("İzin Türü").SemiBold();
                                header.Cell().Element(CellStyle).Text("Kullanılan").SemiBold();
                                header.Cell().Element(CellStyle).Text("Kalan").SemiBold();
                                header.Cell().Element(CellStyle).Text("Toplam").SemiBold();
                            });

                            // Data
                            foreach (var balance in leaveBalances)
                            {
                                table.Cell().Element(CellStyle).Text(balance.PersonName ?? "-");
                                table.Cell().Element(CellStyle).Text(balance.LeaveTypeName ?? "-");
                                table.Cell().Element(CellStyle).Text(balance.UsedDays.ToString());
                                table.Cell().Element(CellStyle).Text(balance.RemainingDays.ToString());
                                table.Cell().Element(CellStyle).Text(balance.AllocatedDays.ToString());
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text($"Rapor Tarihi: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", _turkishCulture)}");
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportMaterialStockReportAsync(IEnumerable<MaterialListDto> materials, bool includeStockAnalysis = true)
    {
        var materialList = materials.ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Malzeme Stok Raporu")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Summary
                        if (includeStockAnalysis)
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Toplam Malzeme: {materialList.Count}").FontSize(12);
                                row.RelativeItem().Text($"Toplam Değer: {materialList.Sum(m => m.TotalValue):C}").FontSize(12);
                            });
                        }

                        // Material Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Malzeme Adı
                                columns.RelativeColumn(1); // Kategori
                                columns.RelativeColumn(1); // Stok
                                columns.RelativeColumn(1); // Birim
                                columns.RelativeColumn(1); // Fiyat
                                columns.RelativeColumn(1); // Durum
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Malzeme Adı").SemiBold();
                                header.Cell().Element(CellStyle).Text("Kategori").SemiBold();
                                header.Cell().Element(CellStyle).Text("Stok").SemiBold();
                                header.Cell().Element(CellStyle).Text("Birim").SemiBold();
                                header.Cell().Element(CellStyle).Text("Birim Fiyat").SemiBold();
                                header.Cell().Element(CellStyle).Text("Durum").SemiBold();
                            });

                            // Data
                            foreach (var material in materialList)
                            {
                                table.Cell().Element(CellStyle).Text(material.Name);
                                table.Cell().Element(CellStyle).Text(material.Category ?? "-");
                                table.Cell().Element(CellStyle).Text(material.StockQuantity.ToString());
                                table.Cell().Element(CellStyle).Text(material.Unit ?? "-");
                                table.Cell().Element(CellStyle).Text(material.UnitPrice.ToString("C", _turkishCulture));
                                table.Cell().Element(CellStyle).Text(material.StockStatus ?? "Normal");
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportPositionReportAsync(IEnumerable<PositionListDto> positions)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Pozisyon Raporu")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Position Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Pozisyon Adı
                                columns.RelativeColumn(2); // Departman
                                columns.RelativeColumn(2); // Maaş Aralığı
                                columns.RelativeColumn(1); // Durum
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Pozisyon Adı").SemiBold();
                                header.Cell().Element(CellStyle).Text("Departman").SemiBold();
                                header.Cell().Element(CellStyle).Text("Maaş Aralığı").SemiBold();
                                header.Cell().Element(CellStyle).Text("Durum").SemiBold();
                            });

                            // Data
                            foreach (var position in positions)
                            {
                                table.Cell().Element(CellStyle).Text(position.Name);
                                table.Cell().Element(CellStyle).Text(position.DepartmentName ?? "-");
                                table.Cell().Element(CellStyle).Text($"{position.MinSalary:C} - {position.MaxSalary:C}");
                                table.Cell().Element(CellStyle).Text(position.IsActive ? "Aktif" : "Pasif");
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text($"Rapor Tarihi: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", _turkishCulture)}");
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportDashboardSummaryAsync(DashboardSummaryDto summary)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Text("Dashboard Özet Raporu")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Key Metrics
                        column.Item().Text("Anahtar Metrikler").FontSize(14).SemiBold();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Toplam Personel:").SemiBold();
                            table.Cell().Text(summary.TotalPersonnel.ToString());
                            table.Cell().Text("Aktif Departman:").SemiBold();
                            table.Cell().Text(summary.ActiveDepartments.ToString());
                            table.Cell().Text("Bekleyen İzin Talebi:").SemiBold();
                            table.Cell().Text(summary.PendingLeaveRequests.ToString());
                            table.Cell().Text("Düşük Stoklu Malzeme:").SemiBold();
                            table.Cell().Text(summary.LowStockMaterials.ToString());
                        });

                        // Financial Summary
                        column.Item().Text("Mali Özet").FontSize(14).SemiBold();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Aylık Bordro Toplamı:").SemiBold();
                            table.Cell().Text(summary.MonthlyPayrollTotal.ToString("C", _turkishCulture));
                            table.Cell().Text("Ortalama Maaş:").SemiBold();
                            table.Cell().Text(summary.AverageEmployeeSalary.ToString("C", _turkishCulture));
                            table.Cell().Text("Toplam Malzeme Değeri:").SemiBold();
                            table.Cell().Text(summary.TotalMaterialValue.ToString("C", _turkishCulture));
                        });

                        // Chart Data Summaries
                        if (summary.DepartmentDistribution.Any())
                        {
                            column.Item().Text("Departman Dağılımı").FontSize(14).SemiBold();
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                });

                                foreach (var item in summary.DepartmentDistribution)
                                {
                                    table.Cell().Text(item.Label);
                                    table.Cell().Text($"{item.Value} {item.Unit}");
                                }
                            });
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text($"Rapor Tarihi: {summary.GeneratedAt.ToString("dd.MM.yyyy HH:mm", _turkishCulture)}");
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    public async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, string title, string fileName) where T : class
    {
        var properties = typeof(T).GetProperties();
        var dataList = data.ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text(title)
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        // Generic Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var prop in properties.Take(6)) // Limit to 6 columns for readability
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            // Header
                            table.Header(header =>
                            {
                                foreach (var prop in properties.Take(6))
                                {
                                    header.Cell().Element(CellStyle).Text(prop.Name).SemiBold();
                                }
                            });

                            // Data
                            foreach (var item in dataList)
                            {
                                foreach (var prop in properties.Take(6))
                                {
                                    var value = prop.GetValue(item)?.ToString() ?? "-";
                                    table.Cell().Element(CellStyle).Text(value);
                                }
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text($"Rapor Tarihi: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", _turkishCulture)}");
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container.DefaultTextStyle(x => x.FontSize(9)).PaddingVertical(5).PaddingHorizontal(10).Border(1).BorderColor(Colors.Grey.Lighten2);
    }

    private static string GetLeaveStatusText(DAL.Entities.LeaveStatus status)
    {
        return status switch
        {
            DAL.Entities.LeaveStatus.Pending => "Beklemede",
            DAL.Entities.LeaveStatus.Approved => "Onaylandı",
            DAL.Entities.LeaveStatus.Rejected => "Reddedildi",
            DAL.Entities.LeaveStatus.Cancelled => "İptal",
            _ => "Bilinmiyor"
        };
    }
}
