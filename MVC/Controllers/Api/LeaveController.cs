using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Services;
using BLL.DTOs;
using System.Security.Claims;

namespace MVC.Controllers.Api;

/// <summary>
/// İzin yönetimi için API endpoint'leri
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeaveController : BaseApiController
{
    private readonly ILeaveService _leaveService;

    public LeaveController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    /// <summary>
    /// Tüm izinleri listele
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _leaveService.GetAllAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "İzinler başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Filtrelenmiş izinleri getir
    /// </summary>
    [HttpPost("filter")]
    public async Task<IActionResult> GetFiltered([FromBody] LeaveFilterDto filter)
    {
        try
        {
            var result = await _leaveService.GetFilteredAsync(filter);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Filtrelenmiş izinler başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// İzin detayını getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _leaveService.GetByIdAsync(id);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "İzin detayı başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Personele ait izinleri getir
    /// </summary>
    [HttpGet("by-person/{personId}")]
    public async Task<IActionResult> GetByPerson(int personId, [FromQuery] int? year = null)
    {
        try
        {
            var result = await _leaveService.GetLeavesByPersonAsync(personId, year);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Personel izinleri başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Tarih aralığına göre izinleri getir
    /// </summary>
    [HttpGet("by-date-range")]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var result = await _leaveService.GetLeavesByDateRangeAsync(startDate, endDate);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Tarih aralığı izinleri başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Onay bekleyen izinleri getir
    /// </summary>
    [HttpGet("pending-approvals")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetPendingApprovals()
    {
        try
        {
            var result = await _leaveService.GetPendingApprovalsAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Onay bekleyen izinler başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Yaklaşan izinleri getir
    /// </summary>
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming([FromQuery] int days = 30)
    {
        try
        {
            var result = await _leaveService.GetUpcomingLeavesAsync(days);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Yaklaşan izinler başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Takım izinleri getir
    /// </summary>
    [HttpGet("team/{departmentId}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetTeamLeaves(int departmentId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var result = await _leaveService.GetTeamLeavesAsync(departmentId, startDate, endDate);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Takım izinleri başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Yeni izin oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeaveCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            var result = await _leaveService.CreateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "İzin başarıyla oluşturuldu", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// İzin güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LeaveUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            if (id != dto.Id)
                return ApiResponse<object>(null, "ID uyuşmazlığı", false);

            var result = await _leaveService.UpdateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "İzin başarıyla güncellendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// İzin onayla
    /// </summary>
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Approve(int id, [FromBody] LeaveApprovalDto approval)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            approval.Id = id;
            var result = await _leaveService.ApproveLeaveAsync(approval);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "İzin başarıyla onaylandı", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// İzin reddet
    /// </summary>
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Reject(int id, [FromBody] LeaveApprovalDto rejection)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            rejection.Id = id;
            var result = await _leaveService.RejectLeaveAsync(rejection);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "İzin başarıyla reddedildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// İzin iptal et
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return ApiResponse<object>(null, "Kullanıcı kimliği bulunamadı", false);

            var result = await _leaveService.CancelAsync(id, userId);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "İzin başarıyla iptal edildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// İzin çakışması kontrol et
    /// </summary>
    [HttpPost("check-conflicts")]
    public async Task<IActionResult> CheckConflicts([FromBody] LeaveConflictCheckDto request)
    {
        try
        {
            var result = await _leaveService.CheckConflictsAsync(
                request.PersonId, 
                request.StartDate, 
                request.EndDate, 
                request.ExcludeLeaveId);
            
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Çakışma kontrolü başarıyla tamamlandı", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Takvim verilerini getir
    /// </summary>
    [HttpGet("calendar")]
    public async Task<IActionResult> GetCalendarData([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? departmentId = null)
    {
        try
        {
            var result = await _leaveService.GetCalendarDataAsync(startDate, endDate, departmentId);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Takvim verileri başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// İzin istatistiklerini getir
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics([FromQuery] int? personId = null, [FromQuery] int? departmentId = null, [FromQuery] int? year = null)
    {
        try
        {
            var result = await _leaveService.GetLeaveStatisticsAsync(personId, departmentId, year);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "İstatistikler başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Çalışma günü sayısını hesapla
    /// </summary>
    [HttpGet("calculate-working-days")]
    public async Task<IActionResult> CalculateWorkingDays([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var result = await _leaveService.CalculateWorkingDaysAsync(startDate, endDate);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Çalışma günleri başarıyla hesaplandı", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }
}