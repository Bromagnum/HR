using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Services;
using BLL.DTOs;
using DAL.Entities;

namespace MVC.Controllers.Api;

/// <summary>
/// Performance Review API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PerformanceReviewController : BaseApiController
{
    private readonly IPerformanceReviewService _performanceReviewService;

    public PerformanceReviewController(IPerformanceReviewService performanceReviewService)
    {
        _performanceReviewService = performanceReviewService;
    }

    /// <summary>
    /// Tüm performans değerlendirmelerini listele
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _performanceReviewService.GetAllAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Performans değerlendirmeleri başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Performans değerlendirmesi detayını getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _performanceReviewService.GetByIdAsync(id);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Performans değerlendirmesi başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Personele ait performans değerlendirmelerini getir
    /// </summary>
    [HttpGet("by-person/{personId}")]
    public async Task<IActionResult> GetByPerson(int personId)
    {
        try
        {
            var result = await _performanceReviewService.GetByPersonIdAsync(personId);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Personel değerlendirmeleri başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Bekleyen performans değerlendirmelerini getir
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetPending()
    {
        try
        {
            var result = await _performanceReviewService.GetPendingReviewsAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Bekleyen değerlendirmeler başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Tamamlanan performans değerlendirmelerini getir
    /// </summary>
    [HttpGet("completed")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetCompleted()
    {
        try
        {
            var result = await _performanceReviewService.GetCompletedReviewsAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Tamamlanan değerlendirmeler başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Yeni performans değerlendirmesi oluştur
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] PerformanceReviewCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            var result = await _performanceReviewService.CreateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Performans değerlendirmesi başarıyla oluşturuldu", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Performans değerlendirmesi güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PerformanceReviewUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            if (id != dto.Id)
                return ApiResponse<object>(null, "ID uyuşmazlığı", false);

            var result = await _performanceReviewService.UpdateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Performans değerlendirmesi başarıyla güncellendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Performans değerlendirmesini onayla
    /// </summary>
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Approve(int id, [FromBody] int approverId)
    {
        try
        {
            var result = await _performanceReviewService.ApproveReviewAsync(id, approverId);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "Performans değerlendirmesi başarıyla onaylandı", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Öz değerlendirme tamamla
    /// </summary>
    [HttpPost("self-assessment")]
    public async Task<IActionResult> CompleteSelfAssessment([FromBody] SelfAssessmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            var result = await _performanceReviewService.CompleteSelfAssessmentAsync(dto);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "Öz değerlendirme başarıyla tamamlandı", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Performans analitiği getir
    /// </summary>
    [HttpGet("analytics")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAnalytics([FromQuery] int? departmentId = null, [FromQuery] int? year = null)
    {
        try
        {
            var result = await _performanceReviewService.GetPerformanceAnalyticsAsync(departmentId, year);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Performans analitiği başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Aktif değerlendirme dönemlerini getir
    /// </summary>
    [HttpGet("periods")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetActivePeriods()
    {
        try
        {
            var result = await _performanceReviewService.GetActivePeriodsAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Aktif dönemler başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Yeni değerlendirme dönemi oluştur
    /// </summary>
    [HttpPost("periods")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePeriod([FromBody] ReviewPeriodCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            var result = await _performanceReviewService.CreatePeriodAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Değerlendirme dönemi başarıyla oluşturuldu", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }
}
