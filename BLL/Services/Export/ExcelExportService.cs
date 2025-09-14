using BLL.DTOs;
using System.Text;
using System.Reflection;

namespace BLL.Services.Export;

public class ExcelExportService : IExcelExportService
{
    public async Task<byte[]> ExportDepartmentListAsync(IEnumerable<DepartmentListDto> departments, bool includeHierarchy = true)
    {
        return await Task.Run(() =>
        {
            var html = new StringBuilder();
            
            // HTML başlangıcı
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8'>");
            html.AppendLine("<title>Departman Listesi</title>");
            html.AppendLine("<style>");
            html.AppendLine("table { border-collapse: collapse; width: 100%; }");
            html.AppendLine("th, td { border: 1px solid black; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #4CAF50; color: white; font-weight: bold; }");
            html.AppendLine(".inactive { color: #999; }");
            html.AppendLine(".summary { margin-top: 20px; background-color: #f9f9f9; padding: 10px; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            // Başlık
            html.AppendLine("<h1>Departman Listesi</h1>");
            html.AppendLine($"<p>Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");
            
            // Tablo başlangıcı
            html.AppendLine("<table>");
            html.AppendLine("<tr>");
            html.AppendLine("<th>ID</th>");
            html.AppendLine("<th>Departman Adı</th>");
            html.AppendLine("<th>Açıklama</th>");
            html.AppendLine("<th>Üst Departman</th>");
            html.AppendLine("<th>Personel Sayısı</th>");
            html.AppendLine("<th>Durum</th>");
            html.AppendLine("<th>Oluşturulma Tarihi</th>");
            html.AppendLine("</tr>");
            
            // Hiyerarşik sıralama
            var sortedDepartments = includeHierarchy 
                ? BuildHierarchicalList(departments.ToList())
                : departments.OrderBy(d => d.Name);
            
            foreach (var dept in sortedDepartments)
            {
                string indentation = includeHierarchy ? GetIndentation(dept, departments) : "";
                string rowClass = dept.IsActive ? "" : " class='inactive'";
                
                html.AppendLine($"<tr{rowClass}>");
                html.AppendLine($"<td>{dept.Id}</td>");
                html.AppendLine($"<td>{indentation}{dept.Name}</td>");
                html.AppendLine($"<td>{dept.Description ?? ""}</td>");
                html.AppendLine($"<td>{dept.ParentDepartmentName ?? "Ana Departman"}</td>");
                html.AppendLine($"<td>{dept.EmployeeCount}</td>");
                html.AppendLine($"<td>{(dept.IsActive ? "Aktif" : "Pasif")}</td>");
                html.AppendLine($"<td>{DateTime.Now:dd.MM.yyyy}</td>");
                html.AppendLine("</tr>");
            }
            
            html.AppendLine("</table>");
            
            // Özet bilgiler
            AddSummaryInfo(html, departments);
            
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            return Encoding.UTF8.GetBytes(html.ToString());
        });
    }

    public async Task<byte[]> ExportOrganizationChartAsync(IEnumerable<DepartmentListDto> departments)
    {
        return await Task.Run(() =>
        {
            var html = new StringBuilder();
            
            // HTML başlangıcı
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8'>");
            html.AppendLine("<title>Organizasyon Şeması</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; }");
            html.AppendLine(".org-chart { margin: 20px; }");
            html.AppendLine(".level-0 { margin-left: 0px; font-weight: bold; color: #2E7D32; font-size: 16px; }");
            html.AppendLine(".level-1 { margin-left: 20px; font-weight: bold; color: #1976D2; }");
            html.AppendLine(".level-2 { margin-left: 40px; color: #F57C00; }");
            html.AppendLine(".level-3 { margin-left: 60px; color: #7B1FA2; }");
            html.AppendLine(".dept-info { margin: 5px 0; padding: 5px; border-left: 3px solid #ddd; }");
            html.AppendLine(".summary { margin-top: 30px; background-color: #f0f0f0; padding: 15px; border-radius: 5px; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            // Başlık
            html.AppendLine("<h1>ORGANIZASYON ŞEMASI</h1>");
            html.AppendLine($"<p>Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");
            
            html.AppendLine("<div class='org-chart'>");
            
            // Ana departmanları bul
            var rootDepartments = departments.Where(d => string.IsNullOrEmpty(d.ParentDepartmentName) && d.IsActive)
                                           .OrderBy(d => d.Name);
            
            foreach (var rootDept in rootDepartments)
            {
                AddDepartmentToChart(html, rootDept, departments, 0);
            }
            
            html.AppendLine("</div>");
            
            // Özet istatistikler
            AddOrganizationSummary(html, departments);
            
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            return Encoding.UTF8.GetBytes(html.ToString());
        });
    }

    public async Task<byte[]> ExportDepartmentDetailAsync(int departmentId, DepartmentDetailDto department)
    {
        return await Task.Run(() =>
        {
            var html = new StringBuilder();
            
            // HTML başlangıcı
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8'>");
            html.AppendLine("<title>Departman Detayı</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine("table { border-collapse: collapse; width: 100%; margin: 10px 0; }");
            html.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; font-weight: bold; }");
            html.AppendLine(".section-header { background-color: #4CAF50; color: white; font-weight: bold; }");
            html.AppendLine(".sub-item { padding-left: 20px; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            // Başlık
            html.AppendLine("<h1>DEPARTMAN DETAY RAPORU</h1>");
            html.AppendLine($"<h2>{department.Name}</h2>");
            html.AppendLine($"<p>Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");
            
            // Temel bilgiler
            html.AppendLine("<table>");
            html.AppendLine("<tr><th>Özellik</th><th>Değer</th></tr>");
            html.AppendLine($"<tr><td>Departman Adı</td><td>{department.Name}</td></tr>");
            html.AppendLine($"<tr><td>Açıklama</td><td>{department.Description ?? "Belirtilmemiş"}</td></tr>");
            html.AppendLine($"<tr><td>Üst Departman</td><td>{department.ParentDepartmentName ?? "Ana Departman"}</td></tr>");
            html.AppendLine($"<tr><td>Durum</td><td>{(department.IsActive ? "Aktif" : "Pasif")}</td></tr>");
            html.AppendLine($"<tr><td>Oluşturulma Tarihi</td><td>{department.CreatedAt:dd.MM.yyyy HH:mm}</td></tr>");
            html.AppendLine($"<tr><td>Son Güncelleme</td><td>{department.UpdatedAt?.ToString("dd.MM.yyyy HH:mm") ?? "Güncellenmemiş"}</td></tr>");
            html.AppendLine("</table>");
            
            // Alt departmanlar
            if (department.SubDepartments.Any())
            {
                html.AppendLine("<h3>Alt Departmanlar</h3>");
                html.AppendLine("<table>");
                html.AppendLine("<tr><th>Departman Adı</th><th>Personel Sayısı</th><th>Durum</th></tr>");
                
                foreach (var subDept in department.SubDepartments)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{subDept.Name}</td>");
                    html.AppendLine($"<td>{subDept.EmployeeCount}</td>");
                    html.AppendLine($"<td>{(subDept.IsActive ? "Aktif" : "Pasif")}</td>");
                    html.AppendLine("</tr>");
                }
                html.AppendLine("</table>");
            }
            
            // Personeller
            if (department.Employees.Any())
            {
                html.AppendLine("<h3>Personeller</h3>");
                html.AppendLine("<table>");
                html.AppendLine("<tr><th>Ad Soyad</th><th>Pozisyon</th><th>Personel No</th></tr>");
                
                foreach (var employee in department.Employees)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{employee.FullName}</td>");
                    html.AppendLine($"<td>{employee.Position ?? "Belirtilmemiş"}</td>");
                    html.AppendLine($"<td>{employee.EmployeeNumber ?? "Belirtilmemiş"}</td>");
                    html.AppendLine("</tr>");
                }
                html.AppendLine("</table>");
            }
            
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            return Encoding.UTF8.GetBytes(html.ToString());
        });
    }

    public async Task<byte[]> ExportSearchResultsAsync(DepartmentSearchResultDto searchResult)
    {
        return await Task.Run(() =>
        {
            var html = new StringBuilder();
            
            // HTML başlangıcı
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8'>");
            html.AppendLine("<title>Departman Arama Sonuçları</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine("table { border-collapse: collapse; width: 100%; margin: 10px 0; }");
            html.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #4CAF50; color: white; font-weight: bold; }");
            html.AppendLine(".criteria { background-color: #f0f0f0; padding: 15px; margin: 10px 0; border-radius: 5px; }");
            html.AppendLine(".highlight { background-color: yellow; font-weight: bold; }");
            html.AppendLine(".inactive { color: #999; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            // Başlık
            html.AppendLine("<h1>DEPARTMAN ARAMA SONUÇLARI</h1>");
            html.AppendLine($"<p>Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}</p>");
            
            // Arama kriterleri
            html.AppendLine("<div class='criteria'>");
            html.AppendLine("<h3>Arama Kriterleri</h3>");
            
            if (!string.IsNullOrEmpty(searchResult.Filter.SearchTerm))
                html.AppendLine($"<p><strong>Arama Terimi:</strong> {searchResult.Filter.SearchTerm}</p>");
            
            if (searchResult.Filter.IsActive.HasValue)
                html.AppendLine($"<p><strong>Durum Filtresi:</strong> {(searchResult.Filter.IsActive.Value ? "Aktif" : "Pasif")}</p>");
            
            if (searchResult.Filter.HasParent.HasValue)
                html.AppendLine($"<p><strong>Seviye Filtresi:</strong> {(searchResult.Filter.HasParent.Value ? "Alt Departmanlar" : "Ana Departmanlar")}</p>");
            
            html.AppendLine($"<p><strong>Sıralama:</strong> {searchResult.Filter.SortBy} ({(searchResult.Filter.SortDescending ? "Azalan" : "Artan")})</p>");
            html.AppendLine($"<p><strong>Toplam Sonuç:</strong> {searchResult.TotalCount}</p>");
            html.AppendLine("</div>");
            
            // Sonuçlar tablosu
            html.AppendLine("<h3>Sonuçlar</h3>");
            html.AppendLine("<table>");
            html.AppendLine("<tr>");
            html.AppendLine("<th>ID</th>");
            html.AppendLine("<th>Departman Adı</th>");
            html.AppendLine("<th>Açıklama</th>");
            html.AppendLine("<th>Üst Departman</th>");
            html.AppendLine("<th>Personel Sayısı</th>");
            html.AppendLine("<th>Durum</th>");
            html.AppendLine("</tr>");
            
            foreach (var dept in searchResult.Departments)
            {
                string rowClass = dept.IsActive ? "" : " class='inactive'";
                
                html.AppendLine($"<tr{rowClass}>");
                html.AppendLine($"<td>{dept.Id}</td>");
                
                // Arama terimini vurgula
                string deptName = dept.Name;
                string description = dept.Description ?? "";
                
                if (!string.IsNullOrEmpty(searchResult.Filter.SearchTerm))
                {
                    var searchTerm = searchResult.Filter.SearchTerm;
                    deptName = deptName.Replace(searchTerm, $"<span class='highlight'>{searchTerm}</span>", StringComparison.OrdinalIgnoreCase);
                    description = description.Replace(searchTerm, $"<span class='highlight'>{searchTerm}</span>", StringComparison.OrdinalIgnoreCase);
                }
                
                html.AppendLine($"<td>{deptName}</td>");
                html.AppendLine($"<td>{description}</td>");
                html.AppendLine($"<td>{dept.ParentDepartmentName ?? "Ana Departman"}</td>");
                html.AppendLine($"<td>{dept.EmployeeCount}</td>");
                html.AppendLine($"<td>{(dept.IsActive ? "Aktif" : "Pasif")}</td>");
                html.AppendLine("</tr>");
            }
            
            html.AppendLine("</table>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            return Encoding.UTF8.GetBytes(html.ToString());
        });
    }

    private IEnumerable<DepartmentListDto> BuildHierarchicalList(List<DepartmentListDto> departments)
    {
        var result = new List<DepartmentListDto>();
        var rootDepartments = departments.Where(d => string.IsNullOrEmpty(d.ParentDepartmentName)).OrderBy(d => d.Name);
        
        foreach (var rootDept in rootDepartments)
        {
            AddDepartmentWithChildren(rootDept, departments, result);
        }
        
        return result;
    }

    private void AddDepartmentWithChildren(DepartmentListDto department, List<DepartmentListDto> allDepartments, List<DepartmentListDto> result)
    {
        result.Add(department);
        var children = allDepartments.Where(d => d.ParentDepartmentName == department.Name).OrderBy(d => d.Name);
        foreach (var child in children)
        {
            AddDepartmentWithChildren(child, allDepartments, result);
        }
    }

    private string GetIndentation(DepartmentListDto department, IEnumerable<DepartmentListDto> allDepartments)
    {
        int level = 0;
        var current = department;
        
        while (!string.IsNullOrEmpty(current.ParentDepartmentName))
        {
            level++;
            current = allDepartments.FirstOrDefault(d => d.Name == current.ParentDepartmentName);
            if (current == null) break;
        }
        
        return new string('&', level * 4) + "nbsp;" + (level > 0 ? "└─ " : "");
    }

    private void AddDepartmentToChart(StringBuilder html, DepartmentListDto department, 
                                    IEnumerable<DepartmentListDto> allDepartments, int level)
    {
        html.AppendLine($"<div class='dept-info level-{level}'>");
        html.AppendLine($"<strong>{department.Name}</strong>");
        
        if (!string.IsNullOrEmpty(department.Description))
            html.AppendLine($" - {department.Description}");
        
        html.AppendLine($" ({department.EmployeeCount} personel, {(department.IsActive ? "Aktif" : "Pasif")})");
        html.AppendLine("</div>");
        
        // Alt departmanları ekle
        var children = allDepartments.Where(d => d.ParentDepartmentName == department.Name && d.IsActive)
                                    .OrderBy(d => d.Name);
        
        foreach (var child in children)
        {
            AddDepartmentToChart(html, child, allDepartments, level + 1);
        }
    }

    private void AddSummaryInfo(StringBuilder html, IEnumerable<DepartmentListDto> departments)
    {
        var totalDepts = departments.Count();
        var activeDepts = departments.Count(d => d.IsActive);
        var totalEmployees = departments.Sum(d => d.EmployeeCount);
        var rootDepts = departments.Count(d => string.IsNullOrEmpty(d.ParentDepartmentName));
        
        html.AppendLine("<div class='summary'>");
        html.AppendLine("<h3>Özet Bilgiler</h3>");
        html.AppendLine($"<p><strong>Toplam Departman:</strong> {totalDepts}</p>");
        html.AppendLine($"<p><strong>Aktif Departman:</strong> {activeDepts}</p>");
        html.AppendLine($"<p><strong>Pasif Departman:</strong> {totalDepts - activeDepts}</p>");
        html.AppendLine($"<p><strong>Ana Departman:</strong> {rootDepts}</p>");
        html.AppendLine($"<p><strong>Alt Departman:</strong> {totalDepts - rootDepts}</p>");
        html.AppendLine($"<p><strong>Toplam Personel:</strong> {totalEmployees}</p>");
        html.AppendLine("</div>");
    }

    private void AddOrganizationSummary(StringBuilder html, IEnumerable<DepartmentListDto> departments)
    {
        html.AppendLine("<div class='summary'>");
        html.AppendLine("<h3>Organizasyon İstatistikleri</h3>");
        html.AppendLine($"<p><strong>Toplam Departman Sayısı:</strong> {departments.Count()}</p>");
        html.AppendLine($"<p><strong>Ana Departman Sayısı:</strong> {departments.Count(d => string.IsNullOrEmpty(d.ParentDepartmentName))}</p>");
        html.AppendLine($"<p><strong>Alt Departman Sayısı:</strong> {departments.Count(d => !string.IsNullOrEmpty(d.ParentDepartmentName))}</p>");
        html.AppendLine($"<p><strong>Aktif Departman:</strong> {departments.Count(d => d.IsActive)}</p>");
        html.AppendLine($"<p><strong>Pasif Departman:</strong> {departments.Count(d => !d.IsActive)}</p>");
        html.AppendLine($"<p><strong>Toplam Personel:</strong> {departments.Sum(d => d.EmployeeCount)}</p>");
        
        if (departments.Any())
        {
            html.AppendLine($"<p><strong>Ortalama Departman Başına Personel:</strong> {Math.Round(departments.Average(d => d.EmployeeCount), 1)}</p>");
        }
        
        html.AppendLine("</div>");
    }

    public async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, string fileName) where T : class
    {
        return await Task.Run(() =>
        {
            var csv = new StringBuilder();
            
            // BOM for UTF-8 to ensure Turkish characters display correctly in Excel
            csv.Append('\uFEFF');
            
            // Başlık bilgileri
            csv.AppendLine($"{fileName}");
            csv.AppendLine($"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}");
            csv.AppendLine($"Toplam Kayıt: {data.Count()}");
            csv.AppendLine(); // Boş satır
            
            if (data.Any())
            {
                // Header'ları property'lerden oluştur
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanRead && p.PropertyType.IsPublic)
                    .ToList();
                
                // CSV Header
                var headers = new List<string>();
                foreach (var prop in properties)
                {
                    // Display attribute varsa onu kullan, yoksa property adını kullan
                    var displayName = prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                        .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                        .FirstOrDefault()?.Name ?? prop.Name;
                    headers.Add($"\"{displayName}\"");
                }
                csv.AppendLine(string.Join(",", headers));
                
                // Veri satırları
                foreach (var item in data)
                {
                    var values = new List<string>();
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(item)?.ToString() ?? "";
                        
                        // Boolean değerleri Türkçe'ye çevir
                        if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                        {
                            if (bool.TryParse(value, out bool boolValue))
                                value = boolValue ? "Evet" : "Hayır";
                        }
                        
                        // DateTime değerleri formatla
                        if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                        {
                            if (DateTime.TryParse(value, out DateTime dateValue))
                                value = dateValue.ToString("dd.MM.yyyy HH:mm");
                        }
                        
                        // CSV için değeri quote'la ve escape et
                        value = value.Replace("\"", "\"\""); // Double quotes'u escape et
                        values.Add($"\"{value}\"");
                    }
                    csv.AppendLine(string.Join(",", values));
                }
            }
            else
            {
                csv.AppendLine("Gösterilecek veri bulunamadı.");
            }
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        });
    }
}