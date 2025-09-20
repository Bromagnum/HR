using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaxDaysPerYear = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RequiresDocument = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CanCarryOver = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MaxCarryOverDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false, defaultValue: "#007bff"),
                    NotificationDays = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Değerlendirme dönemi adı"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Değerlendirme dönemi açıklaması"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Dönem başlangıç tarihi"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Dönem bitiş tarihi"),
                    ReviewStartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Değerlendirme başlangıç tarihi"),
                    ReviewEndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Değerlendirme bitiş tarihi"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "Değerlendirme dönemi türü"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    MaxLevel = table.Column<int>(type: "int", nullable: false),
                    LevelDescriptions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVerifiable = table.Column<bool>(type: "bit", nullable: false),
                    VerificationMethod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequiresCertification = table.Column<bool>(type: "bit", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RelatedSkills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    MinSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequiredExperience = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    Requirements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Responsibilities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EmploymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TcKimlikNo = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    EducationLevel = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BloodType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MilitaryStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DriverLicenseClass = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SskNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SskStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Persons_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOngoing = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    GPA = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    DetailedDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    MainResponsibilities = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SecondaryResponsibilities = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RequiredSkills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PreferredSkills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MinRequiredExperience = table.Column<int>(type: "int", nullable: false),
                    PreferredExperience = table.Column<int>(type: "int", nullable: true),
                    MinEducationLevel = table.Column<int>(type: "int", nullable: false),
                    PreferredEducationLevel = table.Column<int>(type: "int", nullable: true),
                    RequiredCertifications = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PreferredCertifications = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TechnicalSkills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SoftSkills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Languages = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TravelRequirement = table.Column<int>(type: "int", nullable: true),
                    RemoteWorkAllowed = table.Column<bool>(type: "bit", nullable: false),
                    PhysicalRequirements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WorkingConditions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CareerPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PerformanceMetrics = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PreviousVersionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDefinitions_JobDefinitions_PreviousVersionId",
                        column: x => x.PreviousVersionId,
                        principalTable: "JobDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobDefinitions_Persons_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobDefinitions_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobPostings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "İş ilanı başlığı"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false, comment: "İş tanımı"),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "İlan durumu"),
                    EmploymentType = table.Column<int>(type: "int", nullable: false, comment: "Çalışma türü"),
                    Requirements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Gereksinimler"),
                    Responsibilities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Sorumluluklar"),
                    Benefits = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Yan haklar"),
                    MinExperience = table.Column<int>(type: "int", nullable: true),
                    MaxExperience = table.Column<int>(type: "int", nullable: true),
                    MinEducation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Minimum eğitim seviyesi"),
                    MinSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Minimum maaş"),
                    MaxSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Maksimum maaş"),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Çalışma yeri"),
                    IsRemoteWork = table.Column<bool>(type: "bit", nullable: false),
                    OpenPositions = table.Column<int>(type: "int", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Yayınlanma tarihi"),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "İletişim bilgileri"),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "SEO dostu URL"),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Meta açıklama"),
                    Tags = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Etiketler"),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    ApplicationCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPostings", x => x.Id);
                    table.CheckConstraint("CK_JobPostings_ApplicationCount", "[ApplicationCount] >= 0");
                    table.CheckConstraint("CK_JobPostings_MaxExperience", "[MaxExperience] >= 0 AND [MaxExperience] <= 50");
                    table.CheckConstraint("CK_JobPostings_MaxSalary", "[MaxSalary] >= 0");
                    table.CheckConstraint("CK_JobPostings_MinExperience", "[MinExperience] >= 0 AND [MinExperience] <= 50");
                    table.CheckConstraint("CK_JobPostings_MinSalary", "[MinSalary] >= 0");
                    table.CheckConstraint("CK_JobPostings_OpenPositions", "[OpenPositions] >= 1 AND [OpenPositions] <= 100");
                    table.CheckConstraint("CK_JobPostings_ViewCount", "[ViewCount] >= 0");
                    table.ForeignKey(
                        name: "FK_JobPostings_CreatedBy",
                        column: x => x.CreatedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobPostings_Departments",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobPostings_Positions",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobPostings_UpdatedBy",
                        column: x => x.UpdatedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "İş ilanları tablosu");

            migrationBuilder.CreateTable(
                name: "LeaveBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false, defaultValue: 2025),
                    AllocatedDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    UsedDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    PendingDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    CarriedOverDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    AvailableDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    RemainingDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    MonthlyAccrual = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    LastAccrualDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AccruedToDate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    ManualAdjustment = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    AdjustmentReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveBalances", x => x.Id);
                    table.CheckConstraint("CK_LeaveBalance_AllocatedDays", "AllocatedDays >= 0");
                    table.CheckConstraint("CK_LeaveBalance_CarriedOverDays", "CarriedOverDays >= 0");
                    table.CheckConstraint("CK_LeaveBalance_PendingDays", "PendingDays >= 0");
                    table.CheckConstraint("CK_LeaveBalance_UsedDays", "UsedDays >= 0");
                    table.ForeignKey(
                        name: "FK_LeaveBalances_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDays = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EmergencyContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmergencyPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HandoverNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    HandoverToPersonId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.Id);
                    table.CheckConstraint("CK_Leave_DateRange", "EndDate >= StartDate");
                    table.CheckConstraint("CK_Leave_TotalDays", "TotalDays > 0");
                    table.ForeignKey(
                        name: "FK_Leaves_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leaves_Persons_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leaves_Persons_HandoverToPersonId",
                        column: x => x.HandoverToPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leaves_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParentOrganizationId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManagerPersonId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Organizations_ParentOrganizationId",
                        column: x => x.ParentOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Organizations_Persons_ManagerPersonId",
                        column: x => x.ManagerPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false, comment: "Bordronun ait olduğu personel ID"),
                    Year = table.Column<int>(type: "int", nullable: false, comment: "Bordro yılı"),
                    Month = table.Column<int>(type: "int", nullable: false, comment: "Bordro ayı (1-12)"),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Temel maaş (brüt)"),
                    Allowances = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Ek ödemeler (prim, yemek yardımı vs.)"),
                    Bonuses = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "İkramiyeler ve primler"),
                    Deductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Toplam kesintiler (vergi, SGK vs.)"),
                    NetSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Net maaş"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Bordro açıklaması"),
                    PreparedById = table.Column<int>(type: "int", nullable: true, comment: "Bordroyu hazırlayan kişi ID"),
                    PreparedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()", comment: "Bordro hazırlanma tarihi"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Maaş ödeme tarihi"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                    table.CheckConstraint("CK_Payrolls_Allowances", "[Allowances] >= 0");
                    table.CheckConstraint("CK_Payrolls_BasicSalary", "[BasicSalary] >= 0");
                    table.CheckConstraint("CK_Payrolls_Bonuses", "[Bonuses] >= 0");
                    table.CheckConstraint("CK_Payrolls_Deductions", "[Deductions] >= 0");
                    table.CheckConstraint("CK_Payrolls_Month", "[Month] >= 1 AND [Month] <= 12");
                    table.CheckConstraint("CK_Payrolls_Year", "[Year] >= 2020 AND [Year] <= 2030");
                    table.ForeignKey(
                        name: "FK_Payrolls_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payrolls_Persons_PreparedById",
                        column: x => x.PreparedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ReviewPeriodId = table.Column<int>(type: "int", nullable: false),
                    ReviewerId = table.Column<int>(type: "int", nullable: false),
                    OverallScore = table.Column<int>(type: "int", nullable: false, comment: "Genel değerlendirme skoru (1-5)"),
                    JobQualityScore = table.Column<int>(type: "int", nullable: false, comment: "İş kalitesi skoru (1-5)"),
                    ProductivityScore = table.Column<int>(type: "int", nullable: false, comment: "Üretkenlik skoru (1-5)"),
                    TeamworkScore = table.Column<int>(type: "int", nullable: false, comment: "Ekip çalışması skoru (1-5)"),
                    CommunicationScore = table.Column<int>(type: "int", nullable: false, comment: "İletişim skoru (1-5)"),
                    LeadershipScore = table.Column<int>(type: "int", nullable: false, comment: "Liderlik skoru (1-5)"),
                    InitiativeScore = table.Column<int>(type: "int", nullable: false, comment: "İnisiyatif skoru (1-5)"),
                    ProblemSolvingScore = table.Column<int>(type: "int", nullable: false, comment: "Problem çözme skoru (1-5)"),
                    AdaptabilityScore = table.Column<int>(type: "int", nullable: false, comment: "Uyum skoru (1-5)"),
                    Strengths = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Güçlü yönler"),
                    AreasForImprovement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Gelişim alanları"),
                    Achievements = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Başarılar"),
                    Goals = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Hedefler"),
                    ReviewerComments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Değerlendiren yorumları"),
                    EmployeeComments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Çalışan yorumları"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Değerlendirme durumu"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    IsSelfAssessmentCompleted = table.Column<bool>(type: "bit", nullable: false),
                    SelfAssessmentCompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SelfOverallScore = table.Column<int>(type: "int", nullable: true),
                    SelfAssessmentComments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Öz değerlendirme yorumları"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_ApprovedBy",
                        column: x => x.ApprovedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Person",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_ReviewPeriod",
                        column: x => x.ReviewPeriodId,
                        principalTable: "ReviewPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Reviewer",
                        column: x => x.ReviewerId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    SkillTemplateId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    ExperienceMonths = table.Column<int>(type: "int", nullable: true),
                    IsCertified = table.Column<bool>(type: "bit", nullable: false),
                    CertificationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CertificationAuthority = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CertificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CertificationExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProjectExamples = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LastUsed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEndorsed = table.Column<bool>(type: "bit", nullable: false),
                    EndorsedById = table.Column<int>(type: "int", nullable: true),
                    EndorsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndorsementNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSelfAssessed = table.Column<bool>(type: "bit", nullable: false),
                    AssessedById = table.Column<int>(type: "int", nullable: true),
                    AssessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonSkills_Persons_AssessedById",
                        column: x => x.AssessedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonSkills_Persons_EndorsedById",
                        column: x => x.EndorsedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonSkills_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonSkills_SkillTemplates_SkillTemplateId",
                        column: x => x.SkillTemplateId,
                        principalTable: "SkillTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IssuingAuthority = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CredentialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasExpiration = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Level = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Qualifications_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakEndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakDurationMinutes = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    TotalHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    RegularHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    OvertimeHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    WorkType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Office"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TasksCompleted = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsLateArrival = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsEarlyDeparture = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOvertime = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsWeekend = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CheckInIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CheckOutIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLogs", x => x.Id);
                    table.CheckConstraint("CK_WorkLog_BreakDuration", "[BreakDurationMinutes] >= 0 AND [BreakDurationMinutes] <= 480");
                    table.CheckConstraint("CK_WorkLog_OvertimeHours", "[OvertimeHours] >= 0 AND [OvertimeHours] <= 24");
                    table.CheckConstraint("CK_WorkLog_RegularHours", "[RegularHours] >= 0 AND [RegularHours] <= 24");
                    table.CheckConstraint("CK_WorkLog_TotalHours", "[TotalHours] >= 0 AND [TotalHours] <= 24");
                    table.ForeignKey(
                        name: "FK_WorkLogs_Persons_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLogs_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobDefinitionQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDefinitionId = table.Column<int>(type: "int", nullable: false),
                    QualificationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false),
                    MinScore = table.Column<int>(type: "int", nullable: true),
                    MinExperience = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDefinitionQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDefinitionQualifications_JobDefinitions_JobDefinitionId",
                        column: x => x.JobDefinitionId,
                        principalTable: "JobDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobRequiredSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDefinitionId = table.Column<int>(type: "int", nullable: false),
                    SkillTemplateId = table.Column<int>(type: "int", nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false),
                    MinLevel = table.Column<int>(type: "int", nullable: false),
                    PreferredLevel = table.Column<int>(type: "int", nullable: true),
                    MinExperienceYears = table.Column<int>(type: "int", nullable: true),
                    PreferredExperienceYears = table.Column<int>(type: "int", nullable: true),
                    RequiresCertification = table.Column<bool>(type: "bit", nullable: false),
                    SpecificRequirements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    AssessmentCriteria = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    JobDefinitionId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRequiredSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRequiredSkills_JobDefinitions_JobDefinitionId",
                        column: x => x.JobDefinitionId,
                        principalTable: "JobDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobRequiredSkills_JobDefinitions_JobDefinitionId1",
                        column: x => x.JobDefinitionId1,
                        principalTable: "JobDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobRequiredSkills_SkillTemplates_SkillTemplateId",
                        column: x => x.SkillTemplateId,
                        principalTable: "SkillTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualificationMatchingResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDefinitionId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    OverallMatchPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    RequiredSkillsMatch = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    PreferredSkillsMatch = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ExperienceMatch = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    EducationMatch = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CertificationMatch = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MatchingDetails = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MissingRequirements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedById = table.Column<int>(type: "int", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationMatchingResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualificationMatchingResults_JobDefinitions_JobDefinitionId",
                        column: x => x.JobDefinitionId,
                        principalTable: "JobDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualificationMatchingResults_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QualificationMatchingResults_Persons_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Başvuru sahibinin adı"),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Başvuru sahibinin soyadı"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "E-posta adresi"),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Telefon numarası"),
                    NationalId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true, comment: "TC Kimlik numarası"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Adres bilgisi"),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Başvuru durumu"),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Başvuru tarihi"),
                    CoverLetter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Ön yazı"),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    CurrentCompany = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Mevcut şirket"),
                    CurrentPosition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Mevcut pozisyon"),
                    ExpectedSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Beklenen maaş"),
                    EducationLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Eğitim seviyesi"),
                    University = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Üniversite"),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Bölüm"),
                    GraduationYear = table.Column<int>(type: "int", nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Yetenekler"),
                    Languages = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Diller"),
                    IsAvailableImmediately = table.Column<bool>(type: "bit", nullable: false),
                    AvailableStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedById = table.Column<int>(type: "int", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "İnceleme notları"),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Mülakat notları"),
                    JobPostingId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.CheckConstraint("CK_JobApplications_Age", "[Age] >= 18 AND [Age] <= 70");
                    table.CheckConstraint("CK_JobApplications_ExpectedSalary", "[ExpectedSalary] >= 0");
                    table.CheckConstraint("CK_JobApplications_ExperienceYears", "[ExperienceYears] >= 0 AND [ExperienceYears] <= 50");
                    table.CheckConstraint("CK_JobApplications_GraduationYear", "[GraduationYear] >= 1950 AND [GraduationYear] <= 2030");
                    table.CheckConstraint("CK_JobApplications_Rating", "[Rating] >= 1 AND [Rating] <= 10");
                    table.ForeignKey(
                        name: "FK_JobApplications_JobPostings",
                        column: x => x.JobPostingId,
                        principalTable: "JobPostings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobApplications_Positions",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobApplications_ReviewedBy",
                        column: x => x.ReviewedById,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "İş başvuruları tablosu");

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    MinStockLevel = table.Column<int>(type: "int", nullable: false),
                    MaxStockLevel = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    IsConsumable = table.Column<bool>(type: "bit", nullable: false),
                    LastPurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformanceReviewId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Hedef başlığı"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Hedef açıklaması"),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Hedef durumu"),
                    ProgressPercentage = table.Column<int>(type: "int", nullable: false, comment: "İlerleme yüzdesi (0-100)"),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Hedef notları"),
                    Priority = table.Column<int>(type: "int", nullable: false, comment: "Hedef önceliği"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceGoals_PerformanceReview",
                        column: x => x.PerformanceReviewId,
                        principalTable: "PerformanceReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonSkillId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AssessedLevel = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImprovementAreas = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AssessmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssessorId = table.Column<int>(type: "int", nullable: false),
                    AssessmentMethod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonSkillId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillAssessments_PersonSkills_PersonSkillId",
                        column: x => x.PersonSkillId,
                        principalTable: "PersonSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillAssessments_PersonSkills_PersonSkillId1",
                        column: x => x.PersonSkillId1,
                        principalTable: "PersonSkills",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SkillAssessments_Persons_AssessorId",
                        column: x => x.AssessorId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobApplicationId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedById = table.Column<int>(type: "int", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedById = table.Column<int>(type: "int", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerificationNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    LastDownloadedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastDownloadedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_AspNetUsers_LastDownloadedById",
                        column: x => x.LastDownloadedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_AspNetUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_AspNetUsers_VerifiedById",
                        column: x => x.VerifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationDocuments_JobApplications",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "ParentDepartmentId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 20, 16, 57, 10, 580, DateTimeKind.Local).AddTicks(4575), "İnsan kaynakları departmanı", true, "İnsan Kaynakları", null, null },
                    { 2, new DateTime(2025, 9, 20, 16, 57, 10, 580, DateTimeKind.Local).AddTicks(4720), "Bilgi işlem departmanı", true, "Bilgi İşlem", null, null },
                    { 3, new DateTime(2025, 9, 20, 16, 57, 10, 580, DateTimeKind.Local).AddTicks(4722), "Muhasebe departmanı", true, "Muhasebe", null, null }
                });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CanCarryOver", "Color", "CreatedAt", "Description", "IsActive", "IsPaid", "MaxCarryOverDays", "MaxDaysPerYear", "Name", "NotificationDays", "RequiresApproval", "UpdatedAt" },
                values: new object[] { 1, true, "#28a745", new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(3953), "Yıllık ücretli izin", true, true, 5, 21, "Yıllık İzin", 3, true, null });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "IsActive", "IsPaid", "Name", "NotificationDays", "RequiresApproval", "RequiresDocument", "UpdatedAt" },
                values: new object[] { 2, "#dc3545", new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(3959), "Sağlık raporu ile alınan izin", true, true, "Hastalık İzni", 1, true, true, null });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "IsActive", "IsPaid", "MaxDaysPerYear", "Name", "NotificationDays", "RequiresApproval", "RequiresDocument", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, "#ff69b4", new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(3977), "Annelik ve babalık izni", true, true, 128, "Doğum İzni", 30, true, true, null },
                    { 4, "#ffc107", new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(3981), "Evlilik için verilen izin", true, true, 3, "Evlilik İzni", 7, true, true, null }
                });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "IsActive", "Name", "NotificationDays", "RequiresApproval", "UpdatedAt" },
                values: new object[] { 5, "#6c757d", new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(3984), "Ücretsiz mazeret izni", true, "Mazeret İzni", 2, true, null });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "IsActive", "IsPaid", "MaxDaysPerYear", "Name", "NotificationDays", "RequiresApproval", "RequiresDocument", "UpdatedAt" },
                values: new object[] { 6, "#000000", new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(3988), "Yakın akraba ölümü izni", true, true, 7, "Ölüm İzni", 1, true, true, null });

            migrationBuilder.InsertData(
                table: "SkillTemplates",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "IsActive", "IsVerifiable", "Keywords", "LastUsedAt", "LevelDescriptions", "MaxLevel", "Name", "RelatedSkills", "RequiresCertification", "Type", "UpdatedAt", "UsageCount", "VerificationMethod" },
                values: new object[,]
                {
                    { 1, "Programlama", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1128), ".NET Framework ve .NET Core kullanarak C# ile uygulama geliştirme becerisi", true, true, "C#, .NET, OOP, SOLID", null, "1:Temel | 2:Başlangıç | 3:Orta | 4:İleri | 5:Uzman", 5, "C# Programlama", "ASP.NET, Entity Framework", false, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1133), 0, "Proje bazlı değerlendirme" },
                    { 2, "Web Geliştirme", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1145), "ASP.NET Core kullanarak web uygulamaları ve API geliştirme", true, true, "ASP.NET Core, MVC, Web API, Razor", null, "1:Temel | 2:Başlangıç | 3:Orta | 4:İleri | 5:Uzman", 5, "ASP.NET Core", "C#, JavaScript, HTML, CSS", false, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1145), 0, "Proje ve kod incelemesi" },
                    { 3, "Veritabanı", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1150), "Microsoft SQL Server ile veritabanı tasarımı ve yönetimi", true, true, "SQL, T-SQL, Stored Procedures, Indexing", null, "1:Temel | 2:Başlangıç | 3:Orta | 4:İleri | 5:Uzman", 5, "SQL Server", "Entity Framework, Database Design", true, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1151), 0, "SQL sorguları ve performans testleri" },
                    { 4, "Frontend Geliştirme", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1154), "JavaScript ve ES6+ özellikleri ile frontend geliştirme", true, true, "JavaScript, ES6, DOM, Async/Await", null, "1:Temel | 2:Başlangıç | 3:Orta | 4:İleri | 5:Uzman", 5, "JavaScript", "HTML, CSS, React, Vue.js", false, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1155), 0, "Kod incelemesi ve proje değerlendirmesi" },
                    { 5, "Yönetim", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1158), "Proje planlama, takip ve yönetim becerileri", true, true, "PMP, Agile, Scrum, Kanban", null, "1:Temel | 2:Başlangıç | 3:Orta | 4:İleri | 5:Uzman", 5, "Proje Yönetimi", "Liderlik, İletişim, Risk Yönetimi", true, 2, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1159), 0, "Proje başarı oranları ve takım geri bildirimleri" },
                    { 6, "Dil", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1162), "İngilizce dil yeterliliği - konuşma, okuma, yazma", true, true, "English, TOEFL, IELTS, Business English", null, "1:Başlangıç | 2:Temel | 3:Orta | 4:İleri | 5:İleri Düzey", 5, "İngilizce", "İletişim, Sunum", true, 3, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1163), 0, "TOEFL, IELTS veya kurumsal dil sınavı" },
                    { 7, "Kişisel Gelişim", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1166), "Etkili iletişim kurma ve sürdürme becerileri", true, true, "Communication, Interpersonal, Presentation", null, "1:Temel | 2:Gelişen | 3:Yeterli | 4:İyi | 5:Mükemmel", 5, "İletişim", "Liderlik, Takım Çalışması", false, 2, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1167), 0, "360 derece değerlendirme" },
                    { 8, "Cloud Computing", new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1182), "Microsoft Azure platformu ile cloud çözümleri geliştirme", true, true, "Azure, Cloud, DevOps, Containers", null, "1:Fundamentals | 2:Associate | 3:Expert | 4:Architect | 5:Master", 5, "Azure Cloud", "C#, PowerShell, Kubernetes", true, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(1183), 0, "Microsoft Azure sertifikaları" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Description", "EmploymentType", "IsActive", "Level", "MaxSalary", "MinSalary", "Name", "RequiredExperience", "Requirements", "Responsibilities", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(2865), 1, "İnsan kaynakları süreçlerini yönetir, personel işlemleri ile ilgilenir", "Tam Zamanlı", true, "Mid-Level", 18000m, 12000m, "İnsan Kaynakları Uzmanı", 2, "Lisans mezunu, İnsan Kaynakları veya İşletme bölümü tercih edilir", "Personel işlemleri, bordro hazırlama, izin takibi, performans değerlendirme", null });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Description", "EmploymentType", "IsActive", "IsAvailable", "Level", "MaxSalary", "MinSalary", "Name", "RequiredExperience", "Requirements", "Responsibilities", "UpdatedAt" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(2874), 2, "Web ve mobil uygulamalar geliştirir, sistem bakımı yapar", "Tam Zamanlı", true, true, "Senior", 25000m, 15000m, "Yazılım Geliştirici", 3, "Bilgisayar Mühendisliği mezunu, C#, .NET, SQL bilgisi", "Yazılım geliştirme, kod review, sistem analizi, dokümantasyon", null },
                    { 3, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(2879), 3, "Mali işleri yönetir, finansal raporlama yapar", "Tam Zamanlı", true, true, "Junior", 15000m, 10000m, "Muhasebe Uzmanı", 1, "İşletme veya İktisat mezunu, LUCA, Logo programları bilgisi", "Muhasebe kayıtları, mali raporlama, bütçe hazırlama, vergi işlemleri", null },
                    { 4, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(2884), 2, "Departmanlarda öğrenme ve gelişim süreci", "Stajyer", true, true, "Stajyer", 7000m, 5000m, "Stajyer", 0, "Üniversite 3. veya 4. sınıf öğrencisi", "Mentorluk eşliğinde proje desteği, öğrenme aktiviteleri", null }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Address", "BirthDate", "BirthPlace", "BloodType", "CreatedAt", "DepartmentId", "DriverLicenseClass", "EducationLevel", "Email", "EmployeeNumber", "ExperienceYears", "FatherName", "FirstName", "HireDate", "IsActive", "LastName", "MaritalStatus", "MilitaryStatus", "MotherName", "Phone", "PositionId", "Religion", "Salary", "SskNumber", "SskStartDate", "TcKimlikNo", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5548), 1, null, null, "ahmet.yilmaz@company.com", "EMP001", null, null, "Ahmet", new DateTime(2023, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5117), true, "Yılmaz", null, null, null, "0555 123 45 67", 1, null, 15000m, null, null, "12345678901", null },
                    { 2, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5562), 1, null, null, "fatma.kaya@company.com", "EMP002", null, null, "Fatma", new DateTime(2024, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5557), true, "Kaya", null, null, null, "0555 123 45 68", 1, null, 12000m, null, null, "12345678902", null },
                    { 3, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5595), 2, null, null, "mehmet.demir@company.com", "EMP003", null, null, "Mehmet", new DateTime(2025, 1, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5566), true, "Demir", null, null, null, "0555 123 45 69", 2, null, 18000m, null, null, "12345678903", null },
                    { 4, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5601), 1, null, null, "ayse.sahin@company.com", "EMP004", null, null, "Ayşe", new DateTime(2025, 3, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5600), true, "Şahin", null, null, null, "0555 123 45 70", 1, null, 13000m, null, null, "12345678904", null },
                    { 5, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5667), 3, null, null, "can.ozkan@company.com", "EMP005", null, null, "Can", new DateTime(2022, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5665), true, "Özkan", null, null, null, "0555 123 45 71", 3, null, 22000m, null, null, "12345678905", null },
                    { 6, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5673), 2, null, null, "elif.yildiz@company.com", "EMP006", null, null, "Elif", new DateTime(2024, 11, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5671), true, "Yıldız", null, null, null, "0555 123 45 72", 1, null, 14000m, null, null, "12345678906", null },
                    { 7, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5691), 1, null, null, "burak.arslan@company.com", "EMP007", null, null, "Burak", new DateTime(2024, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5677), true, "Arslan", null, null, null, "0555 123 45 73", 2, null, 16000m, null, null, "12345678907", null },
                    { 8, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5697), 3, null, null, "zeynep.kurt@company.com", "EMP008", null, null, "Zeynep", new DateTime(2025, 5, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5696), true, "Kurt", null, null, null, "0555 123 45 74", 1, null, 11000m, null, null, "12345678908", null },
                    { 9, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5703), 2, null, null, "emre.celik@company.com", "EMP009", null, null, "Emre", new DateTime(2023, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5701), true, "Çelik", null, null, null, "0555 123 45 75", 4, null, 25000m, null, null, "12345678909", null },
                    { 10, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5708), 1, null, null, "seda.polat@company.com", "EMP010", null, null, "Seda", new DateTime(2025, 2, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5707), true, "Polat", null, null, null, "0555 123 45 76", 1, null, 13500m, null, null, "12345678910", null },
                    { 11, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5713), 3, null, null, "kerem.aydin@company.com", "EMP011", null, null, "Kerem", new DateTime(2024, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5711), true, "Aydın", null, null, null, "0555 123 45 77", 2, null, 17000m, null, null, "12345678911", null },
                    { 12, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5718), 2, null, null, "gizem.turan@company.com", "EMP012", null, null, "Gizem", new DateTime(2025, 4, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5717), true, "Turan", null, null, null, "0555 123 45 78", 1, null, 12500m, null, null, "12345678912", null },
                    { 13, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5723), 1, null, null, "cem.oz@company.com", "EMP013", null, null, "Cem", new DateTime(2021, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5722), true, "Öz", null, null, null, "0555 123 45 79", 3, null, 28000m, null, null, "12345678913", null },
                    { 14, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5741), 3, null, null, "deniz.koc@company.com", "EMP014", null, null, "Deniz", new DateTime(2024, 12, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5739), true, "Koç", null, null, null, "0555 123 45 80", 1, null, 14500m, null, null, "12345678914", null },
                    { 15, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5747), 2, null, null, "murat.aktas@company.com", "EMP015", null, null, "Murat", new DateTime(2023, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5745), true, "Aktaş", null, null, null, "0555 123 45 81", 2, null, 19000m, null, null, "12345678915", null },
                    { 16, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5754), 1, null, null, "pinar.gunes@company.com", "EMP016", null, null, "Pınar", new DateTime(2025, 6, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5751), true, "Güneş", null, null, null, "0555 123 45 82", 1, null, 11500m, null, null, "12345678916", null },
                    { 17, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5761), 3, null, null, "okan.bulut@company.com", "EMP017", null, null, "Okan", new DateTime(2022, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5759), true, "Bulut", null, null, null, "0555 123 45 83", 4, null, 26000m, null, null, "12345678917", null },
                    { 18, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5766), 2, null, null, "nihan.erdogan@company.com", "EMP018", null, null, "Nihan", new DateTime(2025, 3, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5764), true, "Erdoğan", null, null, null, "0555 123 45 84", 1, null, 13200m, null, null, "12345678918", null },
                    { 19, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5771), 1, null, null, "tolga.yavuz@company.com", "EMP019", null, null, "Tolga", new DateTime(2024, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5769), true, "Yavuz", null, null, null, "0555 123 45 85", 2, null, 17500m, null, null, "12345678919", null },
                    { 20, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5788), 3, null, null, "esra.tan@company.com", "EMP020", null, null, "Esra", new DateTime(2023, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5786), true, "Tan", null, null, null, "0555 123 45 86", 3, null, 23000m, null, null, "12345678920", null },
                    { 21, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5866), 1, null, null, "serkan.cakir@company.com", "EMP021", null, null, "Serkan", new DateTime(2025, 1, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(5864), true, "Çakır", null, null, null, "0555 123 45 87", 1, null, 14200m, null, null, "12345678921", null }
                });

            migrationBuilder.InsertData(
                table: "Educations",
                columns: new[] { "Id", "CreatedAt", "Degree", "Description", "EndDate", "FieldOfStudy", "GPA", "IsActive", "Location", "PersonId", "SchoolName", "StartDate", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(7707), "Lisans", null, new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "İnsan Kaynakları Yönetimi", 3.45m, true, "İstanbul", 1, "İstanbul Üniversitesi", new DateTime(2018, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "Educations",
                columns: new[] { "Id", "CreatedAt", "Degree", "Description", "EndDate", "FieldOfStudy", "GPA", "IsActive", "IsOngoing", "Location", "PersonId", "SchoolName", "StartDate", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(7715), "Yüksek Lisans", null, null, "İşletme", null, true, true, "Eskişehir", 1, "Anadolu Üniversitesi", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 1, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6113), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6003), 1, 1.75m, 1, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 2, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6133), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6132), 2, 1, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 3, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6140), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6139), 4, 1, 3.0m, null, 2025 },
                    { 4, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6147), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6146), 6, 1, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 5, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6261), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6259), 1, 1.75m, 2, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 6, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6273), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6273), 2, 2, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 7, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6279), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6278), 4, 2, 3.0m, null, 2025 },
                    { 8, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6285), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6284), 6, 2, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 9, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6289), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6289), 1, 1.75m, 3, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 10, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6293), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6293), 2, 3, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 11, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6296), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6296), 4, 3, 3.0m, null, 2025 },
                    { 12, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6300), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6299), 6, 3, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 13, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6303), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6302), 1, 1.75m, 4, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 14, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6306), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6306), 2, 4, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 15, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6310), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6310), 4, 4, 3.0m, null, 2025 },
                    { 16, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6313), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6313), 6, 4, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 17, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6317), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6316), 1, 1.75m, 5, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 18, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6321), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6321), 2, 5, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 19, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6365), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6364), 4, 5, 3.0m, null, 2025 },
                    { 20, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6368), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6367), 6, 5, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 21, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6371), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6371), 1, 1.75m, 6, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 22, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6374), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6374), 2, 6, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 23, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6378), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6378), 4, 6, 3.0m, null, 2025 },
                    { 24, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6382), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6381), 6, 6, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 25, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6385), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6385), 1, 1.75m, 7, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 26, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6389), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6388), 2, 7, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 27, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6392), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6391), 4, 7, 3.0m, null, 2025 },
                    { 28, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6395), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6394), 6, 7, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 29, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6398), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6398), 1, 1.75m, 8, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 30, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6401), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6401), 2, 8, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 31, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6404), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6404), 4, 8, 3.0m, null, 2025 },
                    { 32, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6408), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6407), 6, 8, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 33, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6411), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6410), 1, 1.75m, 9, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 34, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6415), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6415), 2, 9, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 35, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6418), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6418), 4, 9, 3.0m, null, 2025 },
                    { 36, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6421), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6421), 6, 9, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 37, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6424), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6424), 1, 1.75m, 10, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 38, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6427), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6427), 2, 10, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 39, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6431), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6430), 4, 10, 3.0m, null, 2025 },
                    { 40, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6434), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6433), 6, 10, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 41, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6437), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6436), 1, 1.75m, 11, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 42, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6440), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6439), 2, 11, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 43, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6443), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6443), 4, 11, 3.0m, null, 2025 },
                    { 44, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6446), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6446), 6, 11, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 45, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6449), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6449), 1, 1.75m, 12, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 46, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6452), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6452), 2, 12, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 47, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6456), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6455), 4, 12, 3.0m, null, 2025 },
                    { 48, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6459), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6458), 6, 12, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 49, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6484), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6483), 1, 1.75m, 13, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 50, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6487), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6486), 2, 13, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 51, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6490), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6490), 4, 13, 3.0m, null, 2025 },
                    { 52, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6493), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6493), 6, 13, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 53, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6497), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6496), 1, 1.75m, 14, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 54, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6500), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6500), 2, 14, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 55, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6503), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6503), 4, 14, 3.0m, null, 2025 },
                    { 56, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6507), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6507), 6, 14, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 57, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6510), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6510), 1, 1.75m, 15, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 58, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6514), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6513), 2, 15, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 59, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6517), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6516), 4, 15, 3.0m, null, 2025 },
                    { 60, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6520), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6520), 6, 15, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 61, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6524), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6523), 1, 1.75m, 16, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 62, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6527), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6527), 2, 16, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 63, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6531), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6530), 4, 16, 3.0m, null, 2025 },
                    { 64, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6534), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6533), 6, 16, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 65, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6537), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6537), 1, 1.75m, 17, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 66, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6543), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6542), 2, 17, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 67, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6546), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6546), 4, 17, 3.0m, null, 2025 },
                    { 68, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6549), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6549), 6, 17, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 69, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6553), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6552), 1, 1.75m, 18, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 70, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6556), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6556), 2, 18, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 71, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6559), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6559), 4, 18, 3.0m, null, 2025 },
                    { 72, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6563), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6562), 6, 18, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 73, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6566), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6566), 1, 1.75m, 19, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 74, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6570), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6569), 2, 19, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 75, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6573), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6573), 4, 19, 3.0m, null, 2025 },
                    { 76, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6631), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6630), 6, 19, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 77, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6634), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6634), 1, 1.75m, 20, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 78, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6638), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6637), 2, 20, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 79, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6641), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6641), 4, 20, 3.0m, null, 2025 },
                    { 80, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6645), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6644), 6, 20, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "MonthlyAccrual", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[] { 81, 21.0m, null, null, 21.0m, 21.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6648), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6647), 1, 1.75m, 21, 21.0m, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AdjustmentDate", "AdjustmentReason", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "UpdatedAt", "Year" },
                values: new object[] { 82, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6651), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6651), 2, 21, null, 2025 });

            migrationBuilder.InsertData(
                table: "LeaveBalances",
                columns: new[] { "Id", "AccruedToDate", "AdjustmentDate", "AdjustmentReason", "AllocatedDays", "AvailableDays", "CreatedAt", "IsActive", "LastAccrualDate", "LeaveTypeId", "PersonId", "RemainingDays", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 83, 3.0m, null, null, 3.0m, 3.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6654), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6654), 4, 21, 3.0m, null, 2025 },
                    { 84, 7.0m, null, null, 7.0m, 7.0m, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6658), true, new DateTime(2025, 9, 20, 16, 57, 10, 582, DateTimeKind.Local).AddTicks(6657), 6, 21, 7.0m, null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "CreatedAt", "DocumentPath", "EmergencyContact", "EmergencyPhone", "EndDate", "HandoverNotes", "HandoverToPersonId", "IsActive", "IsUrgent", "LeaveTypeId", "Notes", "PersonId", "Reason", "RejectionReason", "RequestDate", "StartDate", "Status", "TotalDays", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Onaylandı", new DateTime(2025, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, false, 1, null, 1, "Aile ziyareti", null, new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, null },
                    { 2, "Sağlık raporu onaylandı", new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "/documents/medical_report_2.pdf", null, null, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, true, 2, null, 2, "Grip nedeniyle hastalık", null, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, null }
                });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "CreatedAt", "DocumentPath", "EmergencyContact", "EmergencyPhone", "EndDate", "HandoverNotes", "HandoverToPersonId", "IsActive", "IsUrgent", "LeaveTypeId", "Notes", "PersonId", "Reason", "RejectionReason", "RequestDate", "StartDate", "TotalDays", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, null, null, null, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Projeler tamamlandı, acil durumlar için telefon açık", 7, true, false, 1, null, 5, "Yılbaşı tatili", null, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, null },
                    { 4, null, null, null, new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, false, 1, null, 8, "Kişisel işler", null, new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null }
                });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "CreatedAt", "DocumentPath", "EmergencyContact", "EmergencyPhone", "EndDate", "HandoverNotes", "HandoverToPersonId", "IsActive", "IsUrgent", "LeaveTypeId", "Notes", "PersonId", "Reason", "RejectionReason", "RequestDate", "StartDate", "Status", "TotalDays", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, "İyi tatiller", new DateTime(2025, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Eşi: Ayşe Turan", "0555 987 65 43", new DateTime(2025, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, false, 1, null, 12, "Doktor kontrolü ve dinlenme", null, new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, null },
                    { 6, "Tebrikler! Mutluluklar dileriz.", new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "/documents/marriage_certificate_15.pdf", null, null, new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, false, 4, null, 15, "Evlilik", null, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 3, null },
                    { 7, null, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, false, 1, null, 19, "Tatil planı", "Yılsonu yoğunluğu nedeniyle bu tarihlerde izin verilemez. Ocak ayında tekrar başvurun.", new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6, null }
                });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "CreatedAt", "DocumentPath", "EmergencyContact", "EmergencyPhone", "EndDate", "HandoverNotes", "HandoverToPersonId", "IsActive", "IsUrgent", "LeaveTypeId", "Notes", "PersonId", "Reason", "RejectionReason", "RequestDate", "StartDate", "TotalDays", "UpdatedAt" },
                values: new object[] { 8, null, null, null, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, false, 1, null, 2, "Aile ziyareti", null, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "CreatedAt", "DocumentPath", "EmergencyContact", "EmergencyPhone", "EndDate", "HandoverNotes", "HandoverToPersonId", "IsActive", "IsUrgent", "LeaveTypeId", "Notes", "PersonId", "Reason", "RejectionReason", "RequestDate", "StartDate", "Status", "TotalDays", "UpdatedAt" },
                values: new object[] { 9, "Sağlık raporu onaylandı", new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "/documents/medical_4.pdf", null, null, new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, true, 2, null, 4, "Doktor kontrolü", null, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, null });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "CreatedAt", "DocumentPath", "EmergencyContact", "EmergencyPhone", "EndDate", "HandoverNotes", "HandoverToPersonId", "IsActive", "IsUrgent", "LeaveTypeId", "Notes", "PersonId", "Reason", "RejectionReason", "RequestDate", "StartDate", "TotalDays", "UpdatedAt" },
                values: new object[] { 10, null, null, null, new DateTime(2025, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tüm işler tamamlandı", null, true, false, 1, null, 7, "Yılbaşı öncesi tatil", null, new DateTime(2025, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null });

            migrationBuilder.InsertData(
                table: "Leaves",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "CreatedAt", "DocumentPath", "EmergencyContact", "EmergencyPhone", "EndDate", "HandoverNotes", "HandoverToPersonId", "IsActive", "IsUrgent", "LeaveTypeId", "Notes", "PersonId", "Reason", "RejectionReason", "RequestDate", "StartDate", "Status", "TotalDays", "UpdatedAt" },
                values: new object[] { 11, "Onaylandı", new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2025, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, false, 1, null, 10, "Kişisel işler", null, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 3, null });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Address", "Code", "CreatedAt", "Description", "Email", "IsActive", "Manager", "ManagerPersonId", "Name", "ParentOrganizationId", "Phone", "UpdatedAt" },
                values: new object[] { 1, "İstanbul, Türkiye", "GM001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(6800), "Ana organizasyon birimi", "genel@ikys.com", true, "Genel Müdür", 1, "Genel Müdürlük", null, "0212 123 45 67", null });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 1, 3003.24m, 15000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4627.65m, "Kasım 2024 Maaşı", true, 11, 13375.59m, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 2, 2140.98m, 12000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3654.34m, "Kasım 2024 Maaşı", true, 11, 10486.64m, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 3, 3755.90m, 18000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5996.93m, "Kasım 2024 Maaşı", true, 11, 15758.97m, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 4, 2784.44m, 13000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4131.25m, "Kasım 2024 Maaşı", true, 11, 11653.19m, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 5, 3868.49m, 22000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6881.32m, "Kasım 2024 Maaşı", true, 11, 18987.17m, new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 6, 1946.51m, 14000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4399.20m, "Kasım 2024 Maaşı", true, 11, 11547.31m, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 7, 3553.93m, 16000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5452.81m, "Kasım 2024 Maaşı", true, 11, 14101.12m, new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 8, 1351.12m, 11000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3143.43m, "Kasım 2024 Maaşı", true, 11, 9207.69m, new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 9, 5560.68m, 25000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8465.26m, "Kasım 2024 Maaşı", true, 11, 22095.42m, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 10, 2790.09m, 13500m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4192.87m, "Kasım 2024 Maaşı", true, 11, 12097.22m, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 11, 3466.10m, 17000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5644.97m, "Kasım 2024 Maaşı", true, 11, 14821.13m, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 12, 1327.83m, 12500m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3670.04m, "Kasım 2024 Maaşı", true, 11, 10157.79m, new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 13, 6248.47m, 28000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8568.89m, "Kasım 2024 Maaşı", true, 11, 25679.58m, new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 14, 3201.28m, 14500m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5087.67m, "Kasım 2024 Maaşı", true, 11, 12613.61m, new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 15, 2324.99m, 19000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5359.33m, "Kasım 2024 Maaşı", true, 11, 15965.66m, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 16, 2498.33m, 11500m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3938.05m, "Kasım 2024 Maaşı", true, 11, 10060.28m, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 17, 3574.85m, 26000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8535.94m, "Kasım 2024 Maaşı", true, 11, 21038.91m, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 18, 2603.33m, 13200m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4064.34m, "Kasım 2024 Maaşı", true, 11, 11738.99m, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 19, 3336.97m, 17500m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5746.75m, "Kasım 2024 Maaşı", true, 11, 15090.22m, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 20, 3750.26m, 23000m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 7632.63m, "Kasım 2024 Maaşı", true, 11, 19117.63m, new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 21, 1885.29m, 14200m, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4031.98m, "Kasım 2024 Maaşı", true, 11, 12053.31m, new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 22, 2101.74m, 15000m, 8014.96m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 7181.08m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 17935.62m, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 23, 2035.37m, 12000m, 6072.20m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5828.99m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 14278.58m, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 24, 3186.44m, 18000m, 13496.37m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8686.23m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 25996.58m, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 25, 1325.15m, 13000m, 6519.98m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5277.75m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 15567.38m, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 26, 2313.98m, 22000m, 21048.66m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 13447.69m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 31914.95m, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 27, 2188.20m, 14000m, 11316.24m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6937.23m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 20567.21m, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 28, 1940.51m, 16000m, 11495.61m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8525.12m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 20911.00m, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 29, 1761.15m, 11000m, 7135.55m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5876.75m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 14019.95m, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 30, 5256.64m, 25000m, 12742.65m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 12645.40m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 30353.89m, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 31, 2023.99m, 13500m, 11547.22m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6987.57m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 20083.64m, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 32, 2852.99m, 17000m, 11332.62m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 9006.58m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 22179.03m, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 33, 1502.55m, 12500m, 7597.88m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6039.12m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 15561.31m, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 34, 6945.49m, 28000m, 24727.12m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 16923.28m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 42749.33m, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 35, 2079.61m, 14500m, 11252.44m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8335.46m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 19496.59m, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 36, 1969.02m, 19000m, 12781.37m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 10005.41m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 23744.98m, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 37, 1529.06m, 11500m, 7276.30m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5115.41m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 15189.95m, new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 38, 4910.12m, 26000m, 18488.78m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 12581.24m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 36817.66m, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 39, 1560.36m, 13200m, 7300.22m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5869.56m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 16191.02m, new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 40, 3721.77m, 17500m, 15975.99m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 9316.26m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 27881.50m, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 41, 5199.71m, 23000m, 21802.79m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 13095.84m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 36906.66m, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 },
                    { 42, 3220.46m, 14200m, 9084.07m, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 7348.84m, "Aralık 2024 Maaşı + Yılsonu İkramiyesi", true, 12, 19155.69m, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2024 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 43, 2906.99m, 15000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4648.93m, "Ocak 2025 Maaşı", true, 1, 13258.06m, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 },
                    { 44, 1529.48m, 12000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3402.38m, "Ocak 2025 Maaşı", true, 1, 10127.10m, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[] { 45, 4320.35m, 18000m, 5354.74m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8000.37m, "Ocak 2025 Maaşı", true, 1, 19674.72m, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 46, 3022.62m, 13000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4155.23m, "Ocak 2025 Maaşı", true, 1, 11867.39m, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 },
                    { 47, 4047.96m, 22000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 7236.40m, "Ocak 2025 Maaşı", true, 1, 18811.56m, new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[] { 48, 1656.05m, 14000m, 3074.90m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5049.35m, "Ocak 2025 Maaşı", true, 1, 13681.60m, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 49, 2667.79m, 16000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4991.96m, "Ocak 2025 Maaşı", true, 1, 13675.83m, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 },
                    { 50, 1865.02m, 11000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3565.55m, "Ocak 2025 Maaşı", true, 1, 9299.47m, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[] { 51, 5538.76m, 25000m, 3141.61m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8600.84m, "Ocak 2025 Maaşı", true, 1, 25079.53m, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 52, 1718.26m, 13500m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4520.05m, "Ocak 2025 Maaşı", true, 1, 10698.21m, new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 },
                    { 53, 2730.21m, 17000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5515.38m, "Ocak 2025 Maaşı", true, 1, 14214.83m, new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[] { 54, 1484.28m, 12500m, 3526.72m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5221.69m, "Ocak 2025 Maaşı", true, 1, 12289.31m, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 55, 3525.06m, 28000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8407.55m, "Ocak 2025 Maaşı", true, 1, 23117.51m, new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 },
                    { 56, 2789.65m, 14500m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5137.13m, "Ocak 2025 Maaşı", true, 1, 12152.52m, new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[] { 57, 4029.85m, 19000m, 2965.93m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6940.46m, "Ocak 2025 Maaşı", true, 1, 19055.32m, new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 58, 2537.40m, 11500m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3682.79m, "Ocak 2025 Maaşı", true, 1, 10354.61m, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 },
                    { 59, 5191.23m, 26000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8093.44m, "Ocak 2025 Maaşı", true, 1, 23097.79m, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[] { 60, 1772.45m, 13200m, 3520.75m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5215.11m, "Ocak 2025 Maaşı", true, 1, 13278.09m, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 61, 2853.84m, 17500m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5907.84m, "Ocak 2025 Maaşı", true, 1, 14446.00m, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 },
                    { 62, 4986.84m, 23000m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 8330.79m, "Ocak 2025 Maaşı", true, 1, 19656.05m, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "Allowances", "BasicSalary", "Bonuses", "CreatedAt", "Deductions", "Description", "IsActive", "Month", "NetSalary", "PaymentDate", "PersonId", "PreparedById", "PreparedDate", "UpdatedAt", "Year" },
                values: new object[] { 63, 2413.88m, 14200m, 3949.77m, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6137.42m, "Ocak 2025 Maaşı", true, 1, 14426.23m, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2025 });

            migrationBuilder.InsertData(
                table: "PersonSkills",
                columns: new[] { "Id", "AssessedAt", "AssessedById", "CertificationAuthority", "CertificationDate", "CertificationExpiry", "CertificationName", "CreatedAt", "Description", "EndorsedAt", "EndorsedById", "EndorsementNotes", "ExperienceMonths", "ExperienceYears", "IsActive", "IsCertified", "IsEndorsed", "IsSelfAssessed", "LastUsed", "Level", "PersonId", "ProjectExamples", "SkillTemplateId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2452), null, null, null, null, null, 5, true, false, false, true, null, 4, 1, null, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2453) },
                    { 2, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2458), null, null, null, null, null, 4, true, false, false, true, null, 4, 1, null, 2, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2458) },
                    { 3, null, null, "Microsoft", new DateTime(2025, 3, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2461), new DateTime(2027, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2595), null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2807), null, null, null, null, null, 3, true, true, false, false, null, 3, 1, null, 3, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2808) },
                    { 4, null, null, "ETS", new DateTime(2024, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2812), null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2814), null, null, null, null, null, 8, true, true, false, false, null, 3, 1, null, 6, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2814) },
                    { 5, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2817), null, null, null, null, null, 4, true, false, false, true, null, 3, 2, null, 5, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2818) },
                    { 6, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2820), null, null, null, null, null, 6, true, false, false, true, null, 4, 2, null, 7, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2821) },
                    { 7, null, null, "Cambridge", new DateTime(2025, 1, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2872), null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2874), null, null, null, null, null, 10, true, true, false, false, null, 4, 2, null, 6, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2875) },
                    { 8, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2878), null, null, null, null, null, 2, true, false, false, true, null, 3, 3, null, 7, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2878) },
                    { 9, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2896), null, null, null, null, null, 5, true, false, false, true, null, 2, 3, null, 6, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(2896) },
                    { 10, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3351), null, new DateTime(2025, 9, 5, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3104), 1, "Mükemmel C# becerileri, takım liderliği yapabilir", null, 8, true, false, true, false, null, 5, 4, null, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3352) },
                    { 11, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3359), null, new DateTime(2025, 9, 5, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3357), 1, null, null, 6, true, false, true, false, null, 5, 4, null, 2, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3360) },
                    { 12, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3363), null, null, null, null, null, 5, true, false, false, true, null, 4, 4, null, 4, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3363) },
                    { 13, null, null, "Microsoft", new DateTime(2025, 6, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3366), new DateTime(2028, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3367), null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3369), null, null, null, null, null, 2, true, true, false, false, null, 3, 4, null, 8, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3370) },
                    { 14, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3372), null, null, null, null, null, 1, true, false, false, true, null, 2, 5, null, 1, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3374) },
                    { 15, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3378), null, null, null, null, null, 1, true, false, false, true, null, 2, 5, null, 4, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3381) },
                    { 16, null, null, null, null, null, null, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3384), null, null, null, null, null, 4, true, false, false, true, null, 3, 5, null, 6, new DateTime(2025, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(3385) }
                });

            migrationBuilder.InsertData(
                table: "Qualifications",
                columns: new[] { "Id", "AttachmentPath", "Category", "CreatedAt", "CredentialNumber", "Description", "ExpirationDate", "HasExpiration", "IsActive", "IssueDate", "IssuingAuthority", "Level", "Location", "Name", "PersonId", "Score", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, "Teknik", new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(9450), "AZ-900-2023-001", "Azure bulut hizmetleri temel bilgileri ve sertifikasyonu", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft", "Başlangıç", "İstanbul", "Microsoft Azure Fundamentals", 1, 85, null },
                    { 2, null, "Dil", new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(9459), "IELTS-2023-789456", "İngilizce dil yeterlilik sınavı - Akademik modül", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "British Council", "B2", "Ankara", "IELTS Academic", 1, 75, null }
                });

            migrationBuilder.InsertData(
                table: "Qualifications",
                columns: new[] { "Id", "AttachmentPath", "Category", "CreatedAt", "CredentialNumber", "Description", "ExpirationDate", "IsActive", "IssueDate", "IssuingAuthority", "Level", "Location", "Name", "PersonId", "Score", "UpdatedAt" },
                values: new object[] { 3, null, "Güvenlik", new DateTime(2025, 9, 20, 16, 57, 10, 581, DateTimeKind.Local).AddTicks(9476), "ISGUY-2022-15478", "İş sağlığı ve güvenliği alanında uzman yardımcısı sertifikası", null, true, new DateTime(2022, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Çalışma ve Sosyal Güvenlik Bakanlığı", "Uzman", "Ankara", "İş Güvenliği Uzman Yardımcısı", 1, null, null });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 1, null, null, null, 60m, null, null, null, null, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 0, 0, 0), true, null, "İlk hafta", 1, 8.0m, new TimeSpan(0, 9, 0, 0, 0), "Approved", null, 8.0m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsLateArrival", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 2, null, null, null, 60m, null, null, null, null, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 15, 0, 0), true, true, null, "Geç başlangıç", 2, 8.0m, new TimeSpan(0, 9, 15, 0, 0), "Approved", null, 8.0m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 3, null, null, null, 90m, null, null, null, null, new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), true, true, null, "Proje mesaisi", 1.5m, 3, 8.0m, new TimeSpan(0, 8, 30, 0, 0), "Approved", null, 9.5m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsEarlyDeparture", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 4, null, null, null, 60m, null, null, null, null, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 30, 0, 0), true, true, null, "Evden çalışma", 4, 7.5m, new TimeSpan(0, 9, 0, 0, 0), "Pending", null, 7.5m, null, "Remote" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 5, null, null, null, 120m, null, null, null, null, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 0, 0, 0), true, true, null, "Yönetim toplantısı", 1.25m, 5, 8.0m, new TimeSpan(0, 8, 45, 0, 0), "Approved", null, 9.25m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsEarlyDeparture", "IsLateArrival", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 6, null, null, null, 60m, null, null, null, null, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 30, 0, 0), true, true, true, null, "Ay başı geç gelme", 6, 7.0m, new TimeSpan(0, 9, 30, 0, 0), "Pending", null, 7.0m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 7, null, null, null, 60m, null, null, null, null, new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 45, 0, 0), true, true, null, "Proje teslimi", 1.0m, 7, 8.0m, new TimeSpan(0, 8, 45, 0, 0), "Approved", null, 9.0m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 8, null, null, null, 60m, null, null, null, null, new DateTime(2025, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 0, 0, 0), true, null, "Uzaktan mesai", 8, 8.0m, new TimeSpan(0, 9, 0, 0, 0), "Completed", null, 8.0m, null, "Remote" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 9, null, null, null, 90m, null, null, null, null, new DateTime(2025, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), true, true, null, "Departman koordinasyonu", 1.5m, 9, 8.0m, new TimeSpan(0, 8, 30, 0, 0), "Approved", null, 9.5m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsEarlyDeparture", "IsLateArrival", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 10, null, null, null, 60m, null, null, null, null, new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 45, 0, 0), true, true, true, null, "Kısmi mesai", 10, 7.5m, new TimeSpan(0, 9, 15, 0, 0), "Pending", null, 7.5m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 11, null, null, null, 60m, null, null, null, null, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), true, true, null, "Kasım başlangıcı", 0.7m, 11, 8.0m, new TimeSpan(0, 8, 50, 0, 0), "Approved", null, 8.7m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 12, null, null, null, 60m, null, null, null, null, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 0, 0, 0), true, null, "Evden tam mesai", 12, 8.0m, new TimeSpan(0, 9, 0, 0, 0), "Approved", null, 8.0m, null, "Remote" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 13, null, null, null, 120m, null, null, null, null, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 0, 0, 0), true, true, null, "Stratejik planlama", 2.0m, 13, 8.0m, new TimeSpan(0, 8, 0, 0, 0), "Approved", null, 10.0m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsEarlyDeparture", "IsLateArrival", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 14, null, null, null, 60m, null, null, null, null, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 20, 0, 0), true, true, true, null, "Doktor raporu", 14, 7.0m, new TimeSpan(0, 9, 20, 0, 0), "Pending", null, 7.0m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[,]
                {
                    { 15, null, null, null, 90m, null, null, null, null, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 15, 0, 0), true, true, null, "Proje kapanışı", 1.5m, 15, 8.0m, new TimeSpan(0, 8, 45, 0, 0), "Approved", null, 9.5m, null, "Office" },
                    { 16, null, null, null, 60m, null, null, null, null, new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 15, 0, 0), true, true, null, "Aralık ayı normal mesai", 0.25m, 16, 8.0m, new TimeSpan(0, 9, 0, 0, 0), "Approved", null, 8.25m, null, "Office" }
                });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsEarlyDeparture", "IsLateArrival", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 17, null, null, null, 45m, null, null, null, null, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 45, 0, 0), true, true, true, null, "Evden çalışma - trafik sebebiyle geç", 17, 7.7m, new TimeSpan(0, 9, 20, 0, 0), "Completed", null, 7.7m, null, "Remote" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "Location", "Notes", "PersonId", "StartTime", "Status", "TasksCompleted", "UpdatedAt", "WorkType" },
                values: new object[] { 18, null, null, null, null, null, null, null, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, "Aktif çalışma günü - devam ediyor", 18, new TimeSpan(0, 8, 45, 0, 0), "Active", null, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "Location", "Notes", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 19, null, null, null, 60m, null, null, null, null, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 0, 0, 0), true, null, "Son hafta çalışması", 19, 8.0m, new TimeSpan(0, 9, 0, 0, 0), "Approved", null, 8.0m, null, "Office" });

            migrationBuilder.InsertData(
                table: "WorkLogs",
                columns: new[] { "Id", "ApprovalNotes", "ApprovedAt", "ApprovedById", "BreakDurationMinutes", "BreakEndTime", "BreakStartTime", "CheckInIP", "CheckOutIP", "CreatedAt", "Date", "EndTime", "IsActive", "IsOvertime", "Location", "Notes", "OvertimeHours", "PersonId", "RegularHours", "StartTime", "Status", "TasksCompleted", "TotalHours", "UpdatedAt", "WorkType" },
                values: new object[] { 20, null, null, null, 90m, null, null, null, null, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), true, true, null, "Yıl sonu yoğunluğu", 1.5m, 20, 8.0m, new TimeSpan(0, 8, 30, 0, 0), "Approved", null, 9.5m, null, "Office" });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Category", "Code", "CreatedAt", "Description", "ExpirationDate", "IsActive", "IsConsumable", "LastPurchaseDate", "Location", "MaxStockLevel", "MinStockLevel", "Name", "OrganizationId", "StockQuantity", "Supplier", "Unit", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, "Temizlik Malzemeleri", "TM001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8952), "Genel temizlik deterjanı", null, true, true, new DateTime(2025, 9, 8, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8948), "Depo-C-01", 50, 10, "Deterjan", 1, 25, "Temizlik A.Ş.", "Litre", 18.50m, null },
                    { 6, "Temizlik Malzemeleri", "TM002", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8957), "Beyaz kağıt havlu", null, true, true, new DateTime(2025, 9, 15, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8956), "Depo-C-02", 60, 15, "Kağıt Havlu", 1, 35, "Temizlik A.Ş.", "Paket", 12.00m, null }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Address", "Code", "CreatedAt", "Description", "Email", "IsActive", "Manager", "ManagerPersonId", "Name", "ParentOrganizationId", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 2, "İstanbul, Türkiye", "IK001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(6807), "İnsan kaynakları yönetimi", "ik@ikys.com", true, "İK Müdürü", 2, "İnsan Kaynakları Bölümü", 1, "0212 123 45 68", null },
                    { 3, "İstanbul, Türkiye", "BI001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(6811), "Teknoloji ve bilgi işlem", "bi@ikys.com", true, "BI Müdürü", 3, "Bilgi İşlem Bölümü", 1, "0212 123 45 69", null },
                    { 4, "İstanbul, Türkiye", "MH001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(6815), "Mali işler ve muhasebe", "muhasebe@ikys.com", true, "Muhasebe Müdürü", 5, "Muhasebe Bölümü", 1, "0212 123 45 70", null }
                });

            migrationBuilder.InsertData(
                table: "SkillAssessments",
                columns: new[] { "Id", "AssessedLevel", "AssessmentDate", "AssessmentMethod", "AssessorId", "CreatedAt", "Feedback", "ImprovementAreas", "IsActive", "IsValid", "PersonSkillId", "PersonSkillId1", "Recommendations", "Score", "Type", "UpdatedAt", "ValidUntil" },
                values: new object[,]
                {
                    { 1, 4, new DateTime(2025, 9, 10, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4299), null, 4, new DateTime(2025, 9, 10, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4700), "Çok iyi kod kalitesi, best practices'i takip ediyor. Devam etsin, liderlik becerileri geliştirebilir.", null, true, true, 1, null, null, 8, 3, new DateTime(2025, 9, 10, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4701), new DateTime(2026, 3, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4585) },
                    { 2, 5, new DateTime(2025, 9, 15, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4705), null, 1, new DateTime(2025, 9, 15, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4709), "Uzman seviyede, kompleks problemleri çözebiliyor. Takım mentorluk görevlerini üstlenebilir.", null, true, true, 10, null, null, 10, 4, new DateTime(2025, 9, 15, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4710), new DateTime(2026, 9, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4708) },
                    { 3, 4, new DateTime(2025, 8, 31, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4713), null, 2, new DateTime(2025, 8, 31, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4715), "Takım içi iletişimde çok başarılı. Sunum becerileri geliştirilebilir.", null, true, true, 6, null, null, 8, 1, new DateTime(2025, 8, 31, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4717), new DateTime(2025, 12, 20, 16, 57, 10, 584, DateTimeKind.Local).AddTicks(4714) }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Category", "Code", "CreatedAt", "Description", "ExpirationDate", "IsActive", "IsConsumable", "LastPurchaseDate", "Location", "MaxStockLevel", "MinStockLevel", "Name", "OrganizationId", "StockQuantity", "Supplier", "Unit", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Ofis Malzemeleri", "OF001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8908), "Beyaz A4 yazıcı kağıdı", null, true, true, new DateTime(2025, 9, 5, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8652), "Depo-A-01", 200, 20, "A4 Kağıt", 2, 150, "Kağıt A.Ş.", "Paket", 25.50m, null },
                    { 2, "Ofis Malzemeleri", "OF002", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8923), "HP LaserJet toner kartuşu", null, true, true, new DateTime(2025, 9, 12, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8918), "Depo-A-02", 25, 5, "Toner Kartuş", 3, 12, "Teknoloji Ltd.", "Adet", 350.00m, null },
                    { 3, "Bilgisayar Ekipmanları", "BT001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8932), "Mekanik klavye", null, true, false, new DateTime(2025, 8, 21, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8930), "Depo-B-01", 15, 3, "Klavye", 3, 8, "Teknoloji Ltd.", "Adet", 150.00m, null },
                    { 4, "Bilgisayar Ekipmanları", "BT002", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8942), "Optik mouse", null, true, false, new DateTime(2025, 8, 31, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8939), "Depo-B-02", 20, 5, "Mouse", 3, 15, "Teknoloji Ltd.", "Adet", 75.00m, null },
                    { 7, "Kırtasiye", "KR001", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8976), "Mavi tükenmez kalem", null, true, true, new DateTime(2025, 9, 10, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8975), "Depo-A-03", 150, 30, "Kalem", 2, 100, "Kırtasiye Ltd.", "Adet", 3.50m, null },
                    { 8, "Kırtasiye", "KR002", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8981), "Plastik klasör dosya", null, true, false, new DateTime(2025, 9, 2, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8980), "Depo-A-04", 80, 20, "Dosya", 2, 45, "Kırtasiye Ltd.", "Adet", 8.75m, null },
                    { 9, "Ofis Malzemeleri", "OF003", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8985), "Siyah mürekkep kartuşu", null, true, true, new DateTime(2025, 8, 6, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8984), "Depo-A-05", 20, 5, "Yazıcı Mürekkep", 3, 2, "Teknoloji Ltd.", "Adet", 85.00m, null },
                    { 10, "Ofis Malzemeleri", "OF004", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8990), "A3 fotokopi kağıdı", null, true, true, new DateTime(2025, 7, 22, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8989), "Depo-A-06", 15, 3, "Fotokopi Kağıdı", 2, 1, "Kağıt A.Ş.", "Paket", 35.00m, null },
                    { 11, "Kırtasiye", "KR003", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8994), "Standart zımba teli", null, true, true, new DateTime(2025, 9, 13, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8993), "Depo-A-07", 30, 10, "Zımba Teli", 2, 45, "Kırtasiye Ltd.", "Kutu", 4.50m, null },
                    { 12, "Kırtasiye", "KR004", new DateTime(2025, 9, 20, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(9009), "Fosforlu işaretleme kalemi", null, true, true, new DateTime(2025, 9, 17, 16, 57, 10, 583, DateTimeKind.Local).AddTicks(8998), "Depo-A-08", 60, 20, "Bantlı Kalem", 2, 95, "Kırtasiye Ltd.", "Adet", 6.25m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_DocumentType",
                table: "ApplicationDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_FileName",
                table: "ApplicationDocuments",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_JobApplicationId",
                table: "ApplicationDocuments",
                column: "JobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_LastDownloadedById",
                table: "ApplicationDocuments",
                column: "LastDownloadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_UploadedById",
                table: "ApplicationDocuments",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocuments_VerifiedById",
                table: "ApplicationDocuments",
                column: "VerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonId",
                table: "AspNetUsers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_PersonId",
                table: "Educations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_PersonId_StartDate",
                table: "Educations",
                columns: new[] { "PersonId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ApplicationDate",
                table: "JobApplications",
                column: "ApplicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_Email",
                table: "JobApplications",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_FullName",
                table: "JobApplications",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobPostingId",
                table: "JobApplications",
                column: "JobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_PositionId",
                table: "JobApplications",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ReviewedById",
                table: "JobApplications",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_Status",
                table: "JobApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitionQualifications_JobDefinitionId",
                table: "JobDefinitionQualifications",
                column: "JobDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitionQualifications_JobDefinitionId_Category",
                table: "JobDefinitionQualifications",
                columns: new[] { "JobDefinitionId", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitions_ApprovedById",
                table: "JobDefinitions",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitions_IsApproved",
                table: "JobDefinitions",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitions_PositionId",
                table: "JobDefinitions",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitions_PositionId_Version",
                table: "JobDefinitions",
                columns: new[] { "PositionId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitions_PreviousVersionId",
                table: "JobDefinitions",
                column: "PreviousVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_CreatedById",
                table: "JobPostings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_DepartmentId",
                table: "JobPostings",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_ExpiryDate",
                table: "JobPostings",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_PositionId",
                table: "JobPostings",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_PublishDate",
                table: "JobPostings",
                column: "PublishDate");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_Slug",
                table: "JobPostings",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_Status",
                table: "JobPostings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_Title",
                table: "JobPostings",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_UpdatedById",
                table: "JobPostings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequiredSkills_Importance",
                table: "JobRequiredSkills",
                column: "Importance");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequiredSkills_JobDefinitionId",
                table: "JobRequiredSkills",
                column: "JobDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequiredSkills_JobDefinitionId_SkillTemplateId",
                table: "JobRequiredSkills",
                columns: new[] { "JobDefinitionId", "SkillTemplateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobRequiredSkills_JobDefinitionId1",
                table: "JobRequiredSkills",
                column: "JobDefinitionId1");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequiredSkills_SkillTemplateId",
                table: "JobRequiredSkills",
                column: "SkillTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_LeaveTypeId",
                table: "LeaveBalances",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_PersonId",
                table: "LeaveBalances",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_PersonId_LeaveTypeId_Year",
                table: "LeaveBalances",
                columns: new[] { "PersonId", "LeaveTypeId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_Year",
                table: "LeaveBalances",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_ApprovedById",
                table: "Leaves",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_HandoverToPersonId",
                table: "Leaves",
                column: "HandoverToPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_PersonId",
                table: "Leaves",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_RequestDate",
                table: "Leaves",
                column: "RequestDate");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_StartDate_EndDate",
                table: "Leaves",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_Status",
                table: "Leaves",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_IsActive",
                table: "LeaveTypes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_Name",
                table: "LeaveTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Category",
                table: "Materials",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Code",
                table: "Materials",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Name",
                table: "Materials",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_OrganizationId",
                table: "Materials",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Code",
                table: "Organizations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ManagerPersonId",
                table: "Organizations",
                column: "ManagerPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                table: "Organizations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ParentOrganizationId",
                table: "Organizations",
                column: "ParentOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_PersonId",
                table: "Payrolls",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_PersonId_Year_Month_Unique",
                table: "Payrolls",
                columns: new[] { "PersonId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_PreparedById",
                table: "Payrolls",
                column: "PreparedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_Year_Month",
                table: "Payrolls",
                columns: new[] { "Year", "Month" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceGoals_PerformanceReviewId",
                table: "PerformanceGoals",
                column: "PerformanceReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceGoals_Status",
                table: "PerformanceGoals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceGoals_TargetDate",
                table: "PerformanceGoals",
                column: "TargetDate");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ApprovedById",
                table: "PerformanceReviews",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_Person_Period",
                table: "PerformanceReviews",
                columns: new[] { "PersonId", "ReviewPeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_PersonId",
                table: "PerformanceReviews",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewerId",
                table: "PerformanceReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewPeriodId",
                table: "PerformanceReviews",
                column: "ReviewPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_Status",
                table: "PerformanceReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_DepartmentId",
                table: "Persons",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PositionId",
                table: "Persons",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_TcKimlikNo",
                table: "Persons",
                column: "TcKimlikNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_AssessedById",
                table: "PersonSkills",
                column: "AssessedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_EndorsedById",
                table: "PersonSkills",
                column: "EndorsedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_IsCertified",
                table: "PersonSkills",
                column: "IsCertified");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_Level",
                table: "PersonSkills",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_PersonId",
                table: "PersonSkills",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_PersonId_SkillTemplateId",
                table: "PersonSkills",
                columns: new[] { "PersonId", "SkillTemplateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonSkills_SkillTemplateId",
                table: "PersonSkills",
                column: "SkillTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_DepartmentId",
                table: "Positions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_IsAvailable",
                table: "Positions",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Name",
                table: "Positions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMatchingResults_JobDefinitionId",
                table: "QualificationMatchingResults",
                column: "JobDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMatchingResults_JobDefinitionId_PersonId",
                table: "QualificationMatchingResults",
                columns: new[] { "JobDefinitionId", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMatchingResults_OverallMatchPercentage",
                table: "QualificationMatchingResults",
                column: "OverallMatchPercentage");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMatchingResults_PersonId",
                table: "QualificationMatchingResults",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMatchingResults_ReviewedById",
                table: "QualificationMatchingResults",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationMatchingResults_Status",
                table: "QualificationMatchingResults",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_Category",
                table: "Qualifications",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_ExpirationDate",
                table: "Qualifications",
                column: "ExpirationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_PersonId",
                table: "Qualifications",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_PersonId_Category",
                table: "Qualifications",
                columns: new[] { "PersonId", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPeriods_DateRange",
                table: "ReviewPeriods",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPeriods_IsActive",
                table: "ReviewPeriods",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPeriods_Name",
                table: "ReviewPeriods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_AssessmentDate",
                table: "SkillAssessments",
                column: "AssessmentDate");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_AssessorId",
                table: "SkillAssessments",
                column: "AssessorId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_PersonSkillId",
                table: "SkillAssessments",
                column: "PersonSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_PersonSkillId1",
                table: "SkillAssessments",
                column: "PersonSkillId1");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_Type",
                table: "SkillAssessments",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_SkillTemplates_Category",
                table: "SkillTemplates",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_SkillTemplates_Name",
                table: "SkillTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillTemplates_Type",
                table: "SkillTemplates",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_UserId",
                table: "UserLoginLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_ApprovedById",
                table: "WorkLogs",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_Date",
                table: "WorkLogs",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_PersonId",
                table: "WorkLogs",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_PersonId_Date",
                table: "WorkLogs",
                columns: new[] { "PersonId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_Status",
                table: "WorkLogs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_WorkType",
                table: "WorkLogs",
                column: "WorkType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationDocuments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "JobDefinitionQualifications");

            migrationBuilder.DropTable(
                name: "JobRequiredSkills");

            migrationBuilder.DropTable(
                name: "LeaveBalances");

            migrationBuilder.DropTable(
                name: "Leaves");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "PerformanceGoals");

            migrationBuilder.DropTable(
                name: "QualificationMatchingResults");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SkillAssessments");

            migrationBuilder.DropTable(
                name: "UserLoginLogs");

            migrationBuilder.DropTable(
                name: "WorkLogs");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "PerformanceReviews");

            migrationBuilder.DropTable(
                name: "JobDefinitions");

            migrationBuilder.DropTable(
                name: "PersonSkills");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "JobPostings");

            migrationBuilder.DropTable(
                name: "ReviewPeriods");

            migrationBuilder.DropTable(
                name: "SkillTemplates");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
