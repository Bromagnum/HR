using DAL.Context;
using DAL.Repositories;
using BLL.Services;
using BLL.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

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

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

