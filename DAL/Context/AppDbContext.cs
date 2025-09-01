using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DAL.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Person> Persons { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Qualification> Qualifications { get; set; }
    public DbSet<Position> Positions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Seed data
        SeedData(modelBuilder);
    }
    
    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department 
            { 
                Id = 1, 
                Name = "İnsan Kaynakları", 
                Description = "İnsan kaynakları departmanı",
                CreatedAt = DateTime.Now
            },
            new Department 
            { 
                Id = 2, 
                Name = "Bilgi İşlem", 
                Description = "Bilgi işlem departmanı",
                CreatedAt = DateTime.Now
            },
            new Department 
            { 
                Id = 3, 
                Name = "Muhasebe", 
                Description = "Muhasebe departmanı",
                CreatedAt = DateTime.Now
            }
        );

        // Seed Sample Positions
        modelBuilder.Entity<Position>().HasData(
            new Position
            {
                Id = 1,
                Name = "İnsan Kaynakları Uzmanı",
                Description = "İnsan kaynakları süreçlerini yönetir, personel işlemleri ile ilgilenir",
                DepartmentId = 1,
                MinSalary = 12000,
                MaxSalary = 18000,
                RequiredExperience = 2,
                Requirements = "Lisans mezunu, İnsan Kaynakları veya İşletme bölümü tercih edilir",
                Responsibilities = "Personel işlemleri, bordro hazırlama, izin takibi, performans değerlendirme",
                EmploymentType = "Tam Zamanlı",
                Level = "Mid-Level",
                IsAvailable = false,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Position
            {
                Id = 2,
                Name = "Yazılım Geliştirici",
                Description = "Web ve mobil uygulamalar geliştirir, sistem bakımı yapar",
                DepartmentId = 2,
                MinSalary = 15000,
                MaxSalary = 25000,
                RequiredExperience = 3,
                Requirements = "Bilgisayar Mühendisliği mezunu, C#, .NET, SQL bilgisi",
                Responsibilities = "Yazılım geliştirme, kod review, sistem analizi, dokümantasyon",
                EmploymentType = "Tam Zamanlı",
                Level = "Senior",
                IsAvailable = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Position
            {
                Id = 3,
                Name = "Muhasebe Uzmanı",
                Description = "Mali işleri yönetir, finansal raporlama yapar",
                DepartmentId = 3,
                MinSalary = 10000,
                MaxSalary = 15000,
                RequiredExperience = 1,
                Requirements = "İşletme veya İktisat mezunu, LUCA, Logo programları bilgisi",
                Responsibilities = "Muhasebe kayıtları, mali raporlama, bütçe hazırlama, vergi işlemleri",
                EmploymentType = "Tam Zamanlı",
                Level = "Junior",
                IsAvailable = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Position
            {
                Id = 4,
                Name = "Stajyer",
                Description = "Departmanlarda öğrenme ve gelişim süreci",
                DepartmentId = 2,
                MinSalary = 5000,
                MaxSalary = 7000,
                RequiredExperience = 0,
                Requirements = "Üniversite 3. veya 4. sınıf öğrencisi",
                Responsibilities = "Mentorluk eşliğinde proje desteği, öğrenme aktiviteleri",
                EmploymentType = "Stajyer",
                Level = "Stajyer",
                IsAvailable = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        );
        
        // Seed Sample Person
        modelBuilder.Entity<Person>().HasData(
            new Person
            {
                Id = 1,
                TcKimlikNo = "12345678901",
                FirstName = "Ahmet",
                LastName = "Yılmaz",
                Email = "ahmet.yilmaz@company.com",
                Phone = "0555 123 45 67",
                EmployeeNumber = "EMP001",
                PositionId = 1,
                DepartmentId = 1,
                HireDate = DateTime.Now.AddYears(-2),
                Salary = 15000,
                CreatedAt = DateTime.Now
            }
        );
        
        // Seed Sample Education
        modelBuilder.Entity<Education>().HasData(
            new Education
            {
                Id = 1,
                SchoolName = "İstanbul Üniversitesi",
                Degree = "Lisans",
                FieldOfStudy = "İnsan Kaynakları Yönetimi",
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2022, 6, 15),
                IsOngoing = false,
                GPA = 3.45m,
                Location = "İstanbul",
                PersonId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Education
            {
                Id = 2,
                SchoolName = "Anadolu Üniversitesi",
                Degree = "Yüksek Lisans",
                FieldOfStudy = "İşletme",
                StartDate = new DateTime(2022, 9, 1),
                EndDate = null,
                IsOngoing = true,
                Location = "Eskişehir",
                PersonId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        );

        // Seed Sample Qualifications
        modelBuilder.Entity<Qualification>().HasData(
            new Qualification
            {
                Id = 1,
                Name = "Microsoft Azure Fundamentals",
                Category = "Teknik",
                IssuingAuthority = "Microsoft",
                CredentialNumber = "AZ-900-2023-001",
                IssueDate = new DateTime(2023, 3, 15),
                ExpirationDate = new DateTime(2026, 3, 15),
                HasExpiration = true,
                Level = "Başlangıç",
                Score = 85,
                Description = "Azure bulut hizmetleri temel bilgileri ve sertifikasyonu",
                Location = "İstanbul",
                PersonId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Qualification
            {
                Id = 2,
                Name = "IELTS Academic",
                Category = "Dil",
                IssuingAuthority = "British Council",
                CredentialNumber = "IELTS-2023-789456",
                IssueDate = new DateTime(2023, 1, 20),
                ExpirationDate = new DateTime(2025, 1, 20),
                HasExpiration = true,
                Level = "B2",
                Score = 75,
                Description = "İngilizce dil yeterlilik sınavı - Akademik modül",
                Location = "Ankara",
                PersonId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Qualification
            {
                Id = 3,
                Name = "İş Güvenliği Uzman Yardımcısı",
                Category = "Güvenlik",
                IssuingAuthority = "Çalışma ve Sosyal Güvenlik Bakanlığı",
                CredentialNumber = "ISGUY-2022-15478",
                IssueDate = new DateTime(2022, 11, 10),
                ExpirationDate = null,
                HasExpiration = false,
                Level = "Uzman",
                Score = null,
                Description = "İş sağlığı ve güvenliği alanında uzman yardımcısı sertifikası",
                Location = "Ankara",
                PersonId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        );
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.Now;
                    break;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}
