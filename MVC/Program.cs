using DAL.Context;
using DAL.Repositories;
using DAL.Entities;
using BLL.Services;
using BLL.Mapping;
using MVC.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure cookies for HTTPS-only operation
if (builder.Environment.IsDevelopment())
{
    // HTTPS-only settings for development
    builder.Services.AddAntiforgery(options =>
    {
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Force HTTPS
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "__RequestVerificationToken";
        options.SuppressXFrameOptionsHeader = false;
    });
    
    // Configure session cookies for HTTPS
    builder.Services.AddSession(options =>
    {
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Force HTTPS
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
    
    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.MinimumSameSitePolicy = SameSiteMode.Lax;
        options.Secure = CookieSecurePolicy.Always; // Force HTTPS
        options.CheckConsentNeeded = context => false;
        options.OnAppendCookie = cookieContext => 
        {
            cookieContext.CookieOptions.SameSite = SameSiteMode.Lax;
            cookieContext.CookieOptions.Secure = true; // Always secure
            cookieContext.CookieOptions.HttpOnly = true;
        };
        options.OnDeleteCookie = cookieContext => 
        {
            cookieContext.CookieOptions.SameSite = SameSiteMode.Lax;
            cookieContext.CookieOptions.Secure = true; // Always secure
            cookieContext.CookieOptions.HttpOnly = true;
        };
    });
}
else
{
    // Production configuration
    builder.Services.AddAntiforgery();
    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.MinimumSameSitePolicy = SameSiteMode.Strict;
        options.Secure = CookieSecurePolicy.Always;
        options.CheckConsentNeeded = context => true;
    });
}

// Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Export Services
builder.Services.AddScoped<BLL.Services.Export.IExcelExportService, BLL.Services.Export.ExcelExportService>();

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

// Configure Identity Authentication (Cookie-based for now)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthorization();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile), typeof(MVC.Mapping.ViewModelMappingProfile));

// Localization for Turkish culture
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "tr-TR" };
    options.SetDefaultCulture(supportedCultures[0])
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

var app = builder.Build();

        // Database initialization - ONLY for development
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            
            // DEVELOPMENT ONLY: Recreate database with seed data
            if (app.Environment.IsDevelopment())
            {
                // WARNING: This will delete and recreate the database!
                // Only use in development environment
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            else
            {
                // PRODUCTION: Only ensure database exists, don't delete
                context.Database.EnsureCreated();
            }
            
            // Seed Roles and Default Admin User
            await SeedRolesAndAdminAsync(roleManager, userManager);
        }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Force HTTPS redirection in all environments
app.UseHttpsRedirection();

app.UseRequestLocalization();

// Enable cookie policy and session
app.UseCookiePolicy();
app.UseSession();
app.UseRouting();

// Authentication & Authorization (correct order)
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


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

