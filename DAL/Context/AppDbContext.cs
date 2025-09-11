using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DAL.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Person> Persons { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Qualification> Qualifications { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<WorkLog> WorkLogs { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }
    
    // Authentication Tables
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserLoginLog> UserLoginLogs { get; set; }
    
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
            // Existing person
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
            },
            // Additional 20 persons for testing scalability
            new Person { Id = 2, TcKimlikNo = "12345678902", FirstName = "Fatma", LastName = "Kaya", Email = "fatma.kaya@company.com", Phone = "0555 123 45 68", EmployeeNumber = "EMP002", PositionId = 1, DepartmentId = 1, HireDate = DateTime.Now.AddYears(-1), Salary = 12000, CreatedAt = DateTime.Now },
            new Person { Id = 3, TcKimlikNo = "12345678903", FirstName = "Mehmet", LastName = "Demir", Email = "mehmet.demir@company.com", Phone = "0555 123 45 69", EmployeeNumber = "EMP003", PositionId = 2, DepartmentId = 2, HireDate = DateTime.Now.AddMonths(-8), Salary = 18000, CreatedAt = DateTime.Now },
            new Person { Id = 4, TcKimlikNo = "12345678904", FirstName = "Ayşe", LastName = "Şahin", Email = "ayse.sahin@company.com", Phone = "0555 123 45 70", EmployeeNumber = "EMP004", PositionId = 1, DepartmentId = 1, HireDate = DateTime.Now.AddMonths(-6), Salary = 13000, CreatedAt = DateTime.Now },
            new Person { Id = 5, TcKimlikNo = "12345678905", FirstName = "Can", LastName = "Özkan", Email = "can.ozkan@company.com", Phone = "0555 123 45 71", EmployeeNumber = "EMP005", PositionId = 3, DepartmentId = 3, HireDate = DateTime.Now.AddYears(-3), Salary = 22000, CreatedAt = DateTime.Now },
            new Person { Id = 6, TcKimlikNo = "12345678906", FirstName = "Elif", LastName = "Yıldız", Email = "elif.yildiz@company.com", Phone = "0555 123 45 72", EmployeeNumber = "EMP006", PositionId = 1, DepartmentId = 2, HireDate = DateTime.Now.AddMonths(-10), Salary = 14000, CreatedAt = DateTime.Now },
            new Person { Id = 7, TcKimlikNo = "12345678907", FirstName = "Burak", LastName = "Arslan", Email = "burak.arslan@company.com", Phone = "0555 123 45 73", EmployeeNumber = "EMP007", PositionId = 2, DepartmentId = 1, HireDate = DateTime.Now.AddYears(-1), Salary = 16000, CreatedAt = DateTime.Now },
            new Person { Id = 8, TcKimlikNo = "12345678908", FirstName = "Zeynep", LastName = "Kurt", Email = "zeynep.kurt@company.com", Phone = "0555 123 45 74", EmployeeNumber = "EMP008", PositionId = 1, DepartmentId = 3, HireDate = DateTime.Now.AddMonths(-4), Salary = 11000, CreatedAt = DateTime.Now },
            new Person { Id = 9, TcKimlikNo = "12345678909", FirstName = "Emre", LastName = "Çelik", Email = "emre.celik@company.com", Phone = "0555 123 45 75", EmployeeNumber = "EMP009", PositionId = 4, DepartmentId = 2, HireDate = DateTime.Now.AddYears(-2), Salary = 25000, CreatedAt = DateTime.Now },
            new Person { Id = 10, TcKimlikNo = "12345678910", FirstName = "Seda", LastName = "Polat", Email = "seda.polat@company.com", Phone = "0555 123 45 76", EmployeeNumber = "EMP010", PositionId = 1, DepartmentId = 1, HireDate = DateTime.Now.AddMonths(-7), Salary = 13500, CreatedAt = DateTime.Now },
            new Person { Id = 11, TcKimlikNo = "12345678911", FirstName = "Kerem", LastName = "Aydın", Email = "kerem.aydin@company.com", Phone = "0555 123 45 77", EmployeeNumber = "EMP011", PositionId = 2, DepartmentId = 3, HireDate = DateTime.Now.AddYears(-1), Salary = 17000, CreatedAt = DateTime.Now },
            new Person { Id = 12, TcKimlikNo = "12345678912", FirstName = "Gizem", LastName = "Turan", Email = "gizem.turan@company.com", Phone = "0555 123 45 78", EmployeeNumber = "EMP012", PositionId = 1, DepartmentId = 2, HireDate = DateTime.Now.AddMonths(-5), Salary = 12500, CreatedAt = DateTime.Now },
            new Person { Id = 13, TcKimlikNo = "12345678913", FirstName = "Cem", LastName = "Öz", Email = "cem.oz@company.com", Phone = "0555 123 45 79", EmployeeNumber = "EMP013", PositionId = 3, DepartmentId = 1, HireDate = DateTime.Now.AddYears(-4), Salary = 28000, CreatedAt = DateTime.Now },
            new Person { Id = 14, TcKimlikNo = "12345678914", FirstName = "Deniz", LastName = "Koç", Email = "deniz.koc@company.com", Phone = "0555 123 45 80", EmployeeNumber = "EMP014", PositionId = 1, DepartmentId = 3, HireDate = DateTime.Now.AddMonths(-9), Salary = 14500, CreatedAt = DateTime.Now },
            new Person { Id = 15, TcKimlikNo = "12345678915", FirstName = "Murat", LastName = "Aktaş", Email = "murat.aktas@company.com", Phone = "0555 123 45 81", EmployeeNumber = "EMP015", PositionId = 2, DepartmentId = 2, HireDate = DateTime.Now.AddYears(-2), Salary = 19000, CreatedAt = DateTime.Now },
            new Person { Id = 16, TcKimlikNo = "12345678916", FirstName = "Pınar", LastName = "Güneş", Email = "pinar.gunes@company.com", Phone = "0555 123 45 82", EmployeeNumber = "EMP016", PositionId = 1, DepartmentId = 1, HireDate = DateTime.Now.AddMonths(-3), Salary = 11500, CreatedAt = DateTime.Now },
            new Person { Id = 17, TcKimlikNo = "12345678917", FirstName = "Okan", LastName = "Bulut", Email = "okan.bulut@company.com", Phone = "0555 123 45 83", EmployeeNumber = "EMP017", PositionId = 4, DepartmentId = 3, HireDate = DateTime.Now.AddYears(-3), Salary = 26000, CreatedAt = DateTime.Now },
            new Person { Id = 18, TcKimlikNo = "12345678918", FirstName = "Nihan", LastName = "Erdoğan", Email = "nihan.erdogan@company.com", Phone = "0555 123 45 84", EmployeeNumber = "EMP018", PositionId = 1, DepartmentId = 2, HireDate = DateTime.Now.AddMonths(-6), Salary = 13200, CreatedAt = DateTime.Now },
            new Person { Id = 19, TcKimlikNo = "12345678919", FirstName = "Tolga", LastName = "Yavuz", Email = "tolga.yavuz@company.com", Phone = "0555 123 45 85", EmployeeNumber = "EMP019", PositionId = 2, DepartmentId = 1, HireDate = DateTime.Now.AddYears(-1), Salary = 17500, CreatedAt = DateTime.Now },
            new Person { Id = 20, TcKimlikNo = "12345678920", FirstName = "Esra", LastName = "Tan", Email = "esra.tan@company.com", Phone = "0555 123 45 86", EmployeeNumber = "EMP020", PositionId = 3, DepartmentId = 3, HireDate = DateTime.Now.AddYears(-2), Salary = 23000, CreatedAt = DateTime.Now },
            new Person { Id = 21, TcKimlikNo = "12345678921", FirstName = "Serkan", LastName = "Çakır", Email = "serkan.cakir@company.com", Phone = "0555 123 45 87", EmployeeNumber = "EMP021", PositionId = 1, DepartmentId = 1, HireDate = DateTime.Now.AddMonths(-8), Salary = 14200, CreatedAt = DateTime.Now }
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
        
        // Seed Sample WorkLog (4-month comprehensive data for scalability testing)
        modelBuilder.Entity<WorkLog>().HasData(
            // === SEPTEMBER 2025 DATA ===
            new WorkLog { Id = 1, PersonId = 1, Date = new DateTime(2025, 9, 2), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(18, 0, 0), BreakDurationMinutes = 60, TotalHours = 8.0m, RegularHours = 8.0m, OvertimeHours = 0, Status = "Approved", WorkType = "Office", Notes = "İlk hafta", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 9, 2) },
            new WorkLog { Id = 2, PersonId = 2, Date = new DateTime(2025, 9, 2), StartTime = new TimeSpan(9, 15, 0), EndTime = new TimeSpan(18, 15, 0), BreakDurationMinutes = 60, TotalHours = 8.0m, RegularHours = 8.0m, OvertimeHours = 0, Status = "Approved", WorkType = "Office", Notes = "Geç başlangıç", IsLateArrival = true, IsEarlyDeparture = false, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 9, 2) },
            new WorkLog { Id = 3, PersonId = 3, Date = new DateTime(2025, 9, 3), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(19, 30, 0), BreakDurationMinutes = 90, TotalHours = 9.5m, RegularHours = 8.0m, OvertimeHours = 1.5m, Status = "Approved", WorkType = "Office", Notes = "Proje mesaisi", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 9, 3) },
            new WorkLog { Id = 4, PersonId = 4, Date = new DateTime(2025, 9, 4), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 30, 0), BreakDurationMinutes = 60, TotalHours = 7.5m, RegularHours = 7.5m, OvertimeHours = 0, Status = "Pending", WorkType = "Remote", Notes = "Evden çalışma", IsLateArrival = false, IsEarlyDeparture = true, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 9, 4) },
            new WorkLog { Id = 5, PersonId = 5, Date = new DateTime(2025, 9, 5), StartTime = new TimeSpan(8, 45, 0), EndTime = new TimeSpan(20, 0, 0), BreakDurationMinutes = 120, TotalHours = 9.25m, RegularHours = 8.0m, OvertimeHours = 1.25m, Status = "Approved", WorkType = "Office", Notes = "Yönetim toplantısı", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 9, 5) },

            // === OCTOBER 2025 DATA ===
            new WorkLog { Id = 6, PersonId = 6, Date = new DateTime(2025, 10, 1), StartTime = new TimeSpan(9, 30, 0), EndTime = new TimeSpan(17, 30, 0), BreakDurationMinutes = 60, TotalHours = 7.0m, RegularHours = 7.0m, OvertimeHours = 0, Status = "Pending", WorkType = "Office", Notes = "Ay başı geç gelme", IsLateArrival = true, IsEarlyDeparture = true, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 10, 1) },
            new WorkLog { Id = 7, PersonId = 7, Date = new DateTime(2025, 10, 2), StartTime = new TimeSpan(8, 45, 0), EndTime = new TimeSpan(18, 45, 0), BreakDurationMinutes = 60, TotalHours = 9.0m, RegularHours = 8.0m, OvertimeHours = 1.0m, Status = "Approved", WorkType = "Office", Notes = "Proje teslimi", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 10, 2) },
            new WorkLog { Id = 8, PersonId = 8, Date = new DateTime(2025, 10, 3), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(18, 0, 0), BreakDurationMinutes = 60, TotalHours = 8.0m, RegularHours = 8.0m, OvertimeHours = 0, Status = "Completed", WorkType = "Remote", Notes = "Uzaktan mesai", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 10, 3) },
            new WorkLog { Id = 9, PersonId = 9, Date = new DateTime(2025, 10, 4), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(19, 30, 0), BreakDurationMinutes = 90, TotalHours = 9.5m, RegularHours = 8.0m, OvertimeHours = 1.5m, Status = "Approved", WorkType = "Office", Notes = "Departman koordinasyonu", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 10, 4) },
            new WorkLog { Id = 10, PersonId = 10, Date = new DateTime(2025, 10, 7), StartTime = new TimeSpan(9, 15, 0), EndTime = new TimeSpan(17, 45, 0), BreakDurationMinutes = 60, TotalHours = 7.5m, RegularHours = 7.5m, OvertimeHours = 0, Status = "Pending", WorkType = "Office", Notes = "Kısmi mesai", IsLateArrival = true, IsEarlyDeparture = true, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 10, 7) },

            // === NOVEMBER 2025 DATA ===
            new WorkLog { Id = 11, PersonId = 11, Date = new DateTime(2025, 11, 1), StartTime = new TimeSpan(8, 50, 0), EndTime = new TimeSpan(18, 30, 0), BreakDurationMinutes = 60, TotalHours = 8.7m, RegularHours = 8.0m, OvertimeHours = 0.7m, Status = "Approved", WorkType = "Office", Notes = "Kasım başlangıcı", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 11, 1) },
            new WorkLog { Id = 12, PersonId = 12, Date = new DateTime(2025, 11, 5), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(18, 0, 0), BreakDurationMinutes = 60, TotalHours = 8.0m, RegularHours = 8.0m, OvertimeHours = 0, Status = "Approved", WorkType = "Remote", Notes = "Evden tam mesai", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 11, 5) },
            new WorkLog { Id = 13, PersonId = 13, Date = new DateTime(2025, 11, 10), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(20, 0, 0), BreakDurationMinutes = 120, TotalHours = 10.0m, RegularHours = 8.0m, OvertimeHours = 2.0m, Status = "Approved", WorkType = "Office", Notes = "Stratejik planlama", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 11, 10) },
            new WorkLog { Id = 14, PersonId = 14, Date = new DateTime(2025, 11, 15), StartTime = new TimeSpan(9, 20, 0), EndTime = new TimeSpan(17, 20, 0), BreakDurationMinutes = 60, TotalHours = 7.0m, RegularHours = 7.0m, OvertimeHours = 0, Status = "Pending", WorkType = "Office", Notes = "Doktor raporu", IsLateArrival = true, IsEarlyDeparture = true, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 11, 15) },
            new WorkLog { Id = 15, PersonId = 15, Date = new DateTime(2025, 11, 20), StartTime = new TimeSpan(8, 45, 0), EndTime = new TimeSpan(19, 15, 0), BreakDurationMinutes = 90, TotalHours = 9.5m, RegularHours = 8.0m, OvertimeHours = 1.5m, Status = "Approved", WorkType = "Office", Notes = "Proje kapanışı", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 11, 20) },

            // === DECEMBER 2025 DATA (Current Month) ===
            new WorkLog { Id = 16, PersonId = 16, Date = new DateTime(2025, 12, 2), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(18, 15, 0), BreakDurationMinutes = 60, TotalHours = 8.25m, RegularHours = 8.0m, OvertimeHours = 0.25m, Status = "Approved", WorkType = "Office", Notes = "Aralık ayı normal mesai", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 2) },
            new WorkLog { Id = 17, PersonId = 17, Date = new DateTime(2025, 12, 3), StartTime = new TimeSpan(9, 20, 0), EndTime = new TimeSpan(17, 45, 0), BreakDurationMinutes = 45, TotalHours = 7.7m, RegularHours = 7.7m, OvertimeHours = 0, Status = "Completed", WorkType = "Remote", Notes = "Evden çalışma - trafik sebebiyle geç", IsLateArrival = true, IsEarlyDeparture = true, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 3) },
            new WorkLog { Id = 18, PersonId = 18, Date = new DateTime(2025, 12, 4), StartTime = new TimeSpan(8, 45, 0), EndTime = null, BreakDurationMinutes = 0, TotalHours = 0, RegularHours = 0, OvertimeHours = 0, Status = "Active", WorkType = "Office", Notes = "Aktif çalışma günü - devam ediyor", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 4) },
            new WorkLog { Id = 19, PersonId = 19, Date = new DateTime(2025, 12, 1), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(18, 0, 0), BreakDurationMinutes = 60, TotalHours = 8.0m, RegularHours = 8.0m, OvertimeHours = 0, Status = "Approved", WorkType = "Office", Notes = "Son hafta çalışması", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = false, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 1) },
            new WorkLog { Id = 20, PersonId = 20, Date = new DateTime(2025, 12, 5), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(19, 30, 0), BreakDurationMinutes = 90, TotalHours = 9.5m, RegularHours = 8.0m, OvertimeHours = 1.5m, Status = "Approved", WorkType = "Office", Notes = "Yıl sonu yoğunluğu", IsLateArrival = false, IsEarlyDeparture = false, IsOvertime = true, IsWeekend = false, IsHoliday = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 5) }
        );

        // Seed Leave Types
        modelBuilder.Entity<LeaveType>().HasData(
            new LeaveType { Id = 1, Name = "Yıllık İzin", Description = "Yıllık ücretli izin", MaxDaysPerYear = 21, RequiresApproval = true, RequiresDocument = false, IsPaid = true, CanCarryOver = true, MaxCarryOverDays = 5, Color = "#28a745", NotificationDays = 3, IsActive = true, CreatedAt = DateTime.Now },
            new LeaveType { Id = 2, Name = "Hastalık İzni", Description = "Sağlık raporu ile alınan izin", MaxDaysPerYear = 0, RequiresApproval = true, RequiresDocument = true, IsPaid = true, CanCarryOver = false, MaxCarryOverDays = 0, Color = "#dc3545", NotificationDays = 1, IsActive = true, CreatedAt = DateTime.Now },
            new LeaveType { Id = 3, Name = "Doğum İzni", Description = "Annelik ve babalık izni", MaxDaysPerYear = 128, RequiresApproval = true, RequiresDocument = true, IsPaid = true, CanCarryOver = false, MaxCarryOverDays = 0, Color = "#ff69b4", NotificationDays = 30, IsActive = true, CreatedAt = DateTime.Now },
            new LeaveType { Id = 4, Name = "Evlilik İzni", Description = "Evlilik için verilen izin", MaxDaysPerYear = 3, RequiresApproval = true, RequiresDocument = true, IsPaid = true, CanCarryOver = false, MaxCarryOverDays = 0, Color = "#ffc107", NotificationDays = 7, IsActive = true, CreatedAt = DateTime.Now },
            new LeaveType { Id = 5, Name = "Mazeret İzni", Description = "Ücretsiz mazeret izni", MaxDaysPerYear = 0, RequiresApproval = true, RequiresDocument = false, IsPaid = false, CanCarryOver = false, MaxCarryOverDays = 0, Color = "#6c757d", NotificationDays = 2, IsActive = true, CreatedAt = DateTime.Now },
            new LeaveType { Id = 6, Name = "Ölüm İzni", Description = "Yakın akraba ölümü izni", MaxDaysPerYear = 7, RequiresApproval = true, RequiresDocument = true, IsPaid = true, CanCarryOver = false, MaxCarryOverDays = 0, Color = "#000000", NotificationDays = 1, IsActive = true, CreatedAt = DateTime.Now }
        );

        // Seed Leave Balances for 2025
        var leaveBalances = new List<LeaveBalance>();
        int balanceId = 1;
        for (int personId = 1; personId <= 21; personId++)
        {
            // Annual Leave (21 days for everyone)
            leaveBalances.Add(new LeaveBalance { Id = balanceId++, PersonId = personId, LeaveTypeId = 1, Year = 2025, AllocatedDays = 21.0m, UsedDays = 0.0m, PendingDays = 0.0m, CarriedOverDays = 0.0m, AvailableDays = 21.0m, RemainingDays = 21.0m, MonthlyAccrual = 1.75m, AccruedToDate = 21.0m, ManualAdjustment = 0.0m, LastAccrualDate = DateTime.Now, IsActive = true, CreatedAt = DateTime.Now });
            
            // Sick Leave (unlimited, tracked for reporting)
            leaveBalances.Add(new LeaveBalance { Id = balanceId++, PersonId = personId, LeaveTypeId = 2, Year = 2025, AllocatedDays = 0.0m, UsedDays = 0.0m, PendingDays = 0.0m, CarriedOverDays = 0.0m, AvailableDays = 0.0m, RemainingDays = 0.0m, MonthlyAccrual = 0.0m, AccruedToDate = 0.0m, ManualAdjustment = 0.0m, LastAccrualDate = DateTime.Now, IsActive = true, CreatedAt = DateTime.Now });
            
            // Marriage Leave (3 days)
            leaveBalances.Add(new LeaveBalance { Id = balanceId++, PersonId = personId, LeaveTypeId = 4, Year = 2025, AllocatedDays = 3.0m, UsedDays = 0.0m, PendingDays = 0.0m, CarriedOverDays = 0.0m, AvailableDays = 3.0m, RemainingDays = 3.0m, MonthlyAccrual = 0.0m, AccruedToDate = 3.0m, ManualAdjustment = 0.0m, LastAccrualDate = DateTime.Now, IsActive = true, CreatedAt = DateTime.Now });
            
            // Death Leave (7 days)
            leaveBalances.Add(new LeaveBalance { Id = balanceId++, PersonId = personId, LeaveTypeId = 6, Year = 2025, AllocatedDays = 7.0m, UsedDays = 0.0m, PendingDays = 0.0m, CarriedOverDays = 0.0m, AvailableDays = 7.0m, RemainingDays = 7.0m, MonthlyAccrual = 0.0m, AccruedToDate = 7.0m, ManualAdjustment = 0.0m, LastAccrualDate = DateTime.Now, IsActive = true, CreatedAt = DateTime.Now });
        }

        modelBuilder.Entity<LeaveBalance>().HasData(leaveBalances.ToArray());

        // Seed Sample Leave Requests  
        modelBuilder.Entity<Leave>().HasData(
            // Approved leaves
            new Leave { Id = 1, PersonId = 1, LeaveTypeId = 1, StartDate = new DateTime(2025, 11, 15), EndDate = new DateTime(2025, 11, 19), TotalDays = 5, Reason = "Aile ziyareti", Status = LeaveStatus.Approved, RequestDate = new DateTime(2025, 10, 20), ApprovedById = 3, ApprovedAt = new DateTime(2025, 10, 22), ApprovalNotes = "Onaylandı", IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 10, 20) },
            new Leave { Id = 2, PersonId = 2, LeaveTypeId = 2, StartDate = new DateTime(2025, 12, 1), EndDate = new DateTime(2025, 12, 3), TotalDays = 3, Reason = "Grip nedeniyle hastalık", Status = LeaveStatus.Approved, RequestDate = new DateTime(2025, 11, 30), ApprovedById = 1, ApprovedAt = new DateTime(2025, 11, 30), ApprovalNotes = "Sağlık raporu onaylandı", DocumentPath = "/documents/medical_report_2.pdf", IsUrgent = true, IsActive = true, CreatedAt = new DateTime(2025, 11, 30) },
            
            // Pending approvals
            new Leave { Id = 3, PersonId = 5, LeaveTypeId = 1, StartDate = new DateTime(2025, 12, 20), EndDate = new DateTime(2025, 12, 31), TotalDays = 10, Reason = "Yılbaşı tatili", Status = LeaveStatus.Pending, RequestDate = new DateTime(2025, 12, 1), HandoverNotes = "Projeler tamamlandı, acil durumlar için telefon açık", HandoverToPersonId = 7, IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 1) },
            new Leave { Id = 4, PersonId = 8, LeaveTypeId = 1, StartDate = new DateTime(2025, 12, 15), EndDate = new DateTime(2025, 12, 17), TotalDays = 3, Reason = "Kişisel işler", Status = LeaveStatus.Pending, RequestDate = new DateTime(2025, 12, 2), IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 2) },
            
            // In progress (currently on leave)
            new Leave { Id = 5, PersonId = 12, LeaveTypeId = 1, StartDate = new DateTime(2025, 12, 4), EndDate = new DateTime(2025, 12, 6), TotalDays = 3, Reason = "Doktor kontrolü ve dinlenme", Status = LeaveStatus.InProgress, RequestDate = new DateTime(2025, 11, 25), ApprovedById = 13, ApprovedAt = new DateTime(2025, 11, 26), ApprovalNotes = "İyi tatiller", EmergencyContact = "Eşi: Ayşe Turan", EmergencyPhone = "0555 987 65 43", IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 11, 25) },
            
            // Completed leaves
            new Leave { Id = 6, PersonId = 15, LeaveTypeId = 4, StartDate = new DateTime(2025, 10, 10), EndDate = new DateTime(2025, 10, 12), TotalDays = 3, Reason = "Evlilik", Status = LeaveStatus.Completed, RequestDate = new DateTime(2025, 9, 15), ApprovedById = 13, ApprovedAt = new DateTime(2025, 9, 16), ApprovalNotes = "Tebrikler! Mutluluklar dileriz.", DocumentPath = "/documents/marriage_certificate_15.pdf", IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 9, 15) },
            
            // Rejected leave
            new Leave { Id = 7, PersonId = 19, LeaveTypeId = 1, StartDate = new DateTime(2025, 12, 23), EndDate = new DateTime(2025, 12, 30), TotalDays = 6, Reason = "Tatil planı", Status = LeaveStatus.Rejected, RequestDate = new DateTime(2025, 12, 3), ApprovedById = 1, ApprovedAt = new DateTime(2025, 12, 4), RejectionReason = "Yılsonu yoğunluğu nedeniyle bu tarihlerde izin verilemez. Ocak ayında tekrar başvurun.", IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 3) },
            
            // Additional leaves for Department 1 (HR) employees for testing Manager filtering
            new Leave { Id = 8, PersonId = 2, LeaveTypeId = 1, StartDate = new DateTime(2025, 12, 16), EndDate = new DateTime(2025, 12, 18), TotalDays = 3, Reason = "Aile ziyareti", Status = LeaveStatus.Pending, RequestDate = new DateTime(2025, 12, 5), IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 5) },
            new Leave { Id = 9, PersonId = 4, LeaveTypeId = 2, StartDate = new DateTime(2025, 12, 10), EndDate = new DateTime(2025, 12, 12), TotalDays = 3, Reason = "Doktor kontrolü", Status = LeaveStatus.Approved, RequestDate = new DateTime(2025, 12, 3), ApprovedById = 1, ApprovedAt = new DateTime(2025, 12, 4), ApprovalNotes = "Sağlık raporu onaylandı", DocumentPath = "/documents/medical_4.pdf", IsUrgent = true, IsActive = true, CreatedAt = new DateTime(2025, 12, 3) },
            new Leave { Id = 10, PersonId = 7, LeaveTypeId = 1, StartDate = new DateTime(2025, 12, 20), EndDate = new DateTime(2025, 12, 24), TotalDays = 5, Reason = "Yılbaşı öncesi tatil", Status = LeaveStatus.Pending, RequestDate = new DateTime(2025, 12, 6), HandoverNotes = "Tüm işler tamamlandı", IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 12, 6) },
            new Leave { Id = 11, PersonId = 10, LeaveTypeId = 1, StartDate = new DateTime(2025, 11, 25), EndDate = new DateTime(2025, 11, 27), TotalDays = 3, Reason = "Kişisel işler", Status = LeaveStatus.Completed, RequestDate = new DateTime(2025, 11, 15), ApprovedById = 1, ApprovedAt = new DateTime(2025, 11, 16), ApprovalNotes = "Onaylandı", IsUrgent = false, IsActive = true, CreatedAt = new DateTime(2025, 11, 15) }
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
