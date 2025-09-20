using DAL.Context;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using DAL.Entities;
using BLL.Services;
using BLL.Services.Export;
using BLL.Mapping;
using MVC.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// API Services
builder.Services.AddControllers();

// Swagger/OpenAPI Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "IKYS API",
        Version = "v1",
        Description = "İnsan Kaynakları Yönetim Sistemi API Dokümantasyonu",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "IKYS Team",
            Email = "info@ikys.com"
        }
    });

    // JWT Bearer Token Configuration
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // XML Documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3001") // React/Angular frontend URLs
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Database Context - SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
    
    // Development ortamında detaylı EF loglama
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
        // Suppress pending model changes warning for development
        options.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
});

// Repository Pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IQualificationRepository, QualificationRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IWorkLogRepository, WorkLogRepository>();
builder.Services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();

// TMK Repositories
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();

// Job Definition and Skill Management Repositories
builder.Services.AddScoped<IJobDefinitionRepository, JobDefinitionRepository>();
builder.Services.AddScoped<IJobDefinitionQualificationRepository, JobDefinitionQualificationRepository>();
builder.Services.AddScoped<IQualificationMatchingResultRepository, QualificationMatchingResultRepository>();
builder.Services.AddScoped<ISkillTemplateRepository, SkillTemplateRepository>();
builder.Services.AddScoped<IPersonSkillRepository, PersonSkillRepository>();
builder.Services.AddScoped<IJobRequiredSkillRepository, JobRequiredSkillRepository>();
builder.Services.AddScoped<ISkillAssessmentRepository, SkillAssessmentRepository>();

// Performance Review Repositories
builder.Services.AddScoped<IPerformanceReviewRepository, PerformanceReviewRepository>();
builder.Services.AddScoped<IReviewPeriodRepository, ReviewPeriodRepository>();
builder.Services.AddScoped<IPerformanceGoalRepository, PerformanceGoalRepository>();

// Services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IQualificationService, QualificationService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IWorkLogService, WorkLogService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<ILeaveBalanceService, LeaveBalanceService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();

// TMK Services
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();

// CV Services
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IJobPostingService, JobPostingService>();

// Job Definition and Skill Management Services
builder.Services.AddScoped<IJobDefinitionService, JobDefinitionService>();
builder.Services.AddScoped<ISkillManagementService, SkillManagementService>();

// Performance Review Services
builder.Services.AddScoped<IPerformanceReviewService, PerformanceReviewService>();

// Export Services
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();
builder.Services.AddScoped<IPdfExportService, PdfExportService>();
builder.Services.AddScoped<IWordExportService, WordExportService>();

// Authentication Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

// Identity Configuration
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configure Identity Authentication (Cookie-based)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    
    // Development ortamında HTTP'ye izin ver, Production'da HTTPS zorunlu
    if (builder.Environment.IsDevelopment())
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    }
    else
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    }
    
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Name = "IKYS.Auth";
    
    // Development'ta daha detaylı loglama
    if (builder.Environment.IsDevelopment())
    {
        options.Events.OnRedirectToLogin = context =>
        {
            Console.WriteLine($"Redirecting to login from: {context.Request.Path}");
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    }
});

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAbove", policy => policy.RequireRole("Admin", "Manager"));
    options.AddPolicy("AllRoles", policy => policy.RequireRole("Admin", "Manager", "Employee"));
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile), typeof(MVC.Mapping.ViewModelMappingProfile), typeof(MVC.Mapping.MvcMappingProfile));

var app = builder.Build();

// Database initialization with Migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    
    try
    {
        // Apply pending migrations automatically
        context.Database.Migrate();
        
        // Seed Roles and Default Admin User
        await SeedRolesAndAdminAsync(roleManager, userManager);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database migration veya seed data yüklenirken hata oluştu");
        
        // Development ortamında hata durumunda database'i yeniden oluştur
        if (app.Environment.IsDevelopment())
        {
            logger.LogWarning("Development ortamında database yeniden oluşturuluyor...");
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            await SeedRolesAndAdminAsync(roleManager, userManager);
        }
        else
        {
            throw; // Production'da hatayı yukarı fırlat
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // Development ortamında detaylı error sayfaları
    app.UseDeveloperExceptionPage();
    
    // Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IKYS API V1");
        c.RoutePrefix = "api/docs"; // Swagger URL: /api/docs
        c.DocumentTitle = "IKYS API Documentation";
        c.DefaultModelsExpandDepth(-1); // Collapse models by default
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

// Development ortamında HTTPS yönlendirmesini devre dışı bırak
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

// CORS
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "AllowSpecificOrigins");

app.UseRouting();

// Authentication & Authorization (correct order)
app.UseAuthentication();
app.UseAuthorization();

// MVC Routes
app.MapControllerRoute(
    name: "auth",
    pattern: "Auth/{action=Login}",
    defaults: new { controller = "Auth" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// API Routes
app.MapControllers();

// Ensure seed data is applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

// Seed Roles and Default Admin User
static async Task SeedRolesAndAdminAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
{
    // Create Roles
    string[] roles = { "Admin", "Manager", "Employee" };
    
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var applicationRole = new ApplicationRole
            {
                Name = role,
                Description = role switch
                {
                    "Admin" => "Sistem yöneticisi - tüm yetkilere sahip",
                    "Manager" => "Departman yöneticisi - departman çalışanlarını yönetebilir",
                    "Employee" => "Çalışan - kendi bilgilerini görüntüleyebilir",
                    _ => ""
                },
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            
            await roleManager.CreateAsync(applicationRole);
        }
    }
    
    // Create Default Admin User
    var adminEmail = "admin@ikys.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Sistem",
            LastName = "Yöneticisi",
            PersonId = 1, // Link to first person in seed data
            DepartmentId = 1, // Link to HR department
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    
    // Create Default Manager User
    var managerEmail = "manager@ikys.com";
    var managerUser = await userManager.FindByEmailAsync(managerEmail);
    
    if (managerUser == null)
    {
        managerUser = new ApplicationUser
        {
            UserName = managerEmail,
            Email = managerEmail,
            FirstName = "Departman",
            LastName = "Yöneticisi",
            PersonId = 2,
            DepartmentId = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(managerUser, "Manager123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(managerUser, "Manager");
        }
    }
    
    // Create Default Employee User
    var employeeEmail = "employee@ikys.com";
    var employeeUser = await userManager.FindByEmailAsync(employeeEmail);
    
    if (employeeUser == null)
    {
        employeeUser = new ApplicationUser
        {
            UserName = employeeEmail,
            Email = employeeEmail,
            FirstName = "Çalışan",
            LastName = "Test",
            PersonId = 4, // Ayşe Şahin (Department 1)
            DepartmentId = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(employeeUser, "Employee123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(employeeUser, "Employee");
        }
    }
}