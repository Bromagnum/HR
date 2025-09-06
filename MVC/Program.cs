using DAL.Context;
using DAL.Repositories;
using BLL.Services;
using BLL.Mapping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Simple session configuration
builder.Services.AddSession();

// Cookie policy disabled for development to eliminate warnings

// Simple AntiForgery configuration for development
builder.Services.AddAntiforgery();

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

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseRequestLocalization();
// app.UseCookiePolicy(); // Disabled for development to avoid cookie warnings
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
