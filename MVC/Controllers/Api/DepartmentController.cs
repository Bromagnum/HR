using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Services;
using BLL.DTOs;
using System.Security.Claims;

namespace MVC.Controllers.Api;

/// <summary>
/// Departman yönetimi için API endpoint'leri
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController : BaseApiController
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    /// <summary>
    /// Tüm departmanları listele
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _departmentService.GetAllAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Departmanlar başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Departman detayını getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _departmentService.GetByIdAsync(id);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Departman detayı başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Ana departmanları getir
    /// </summary>
    [HttpGet("root")]
    public async Task<IActionResult> GetRootDepartments()
    {
        try
        {
            var result = await _departmentService.GetRootDepartmentsAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Ana departmanlar başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Alt departmanları getir
    /// </summary>
    [HttpGet("{parentId}/sub-departments")]
    public async Task<IActionResult> GetSubDepartments(int parentId)
    {
        try
        {
            var result = await _departmentService.GetSubDepartmentsAsync(parentId);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Alt departmanlar başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Departman ara
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? searchTerm, [FromQuery] bool? isActive)
    {
        try
        {
            var result = await _departmentService.GetFilteredAsync(searchTerm, isActive);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Arama sonuçları başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Yeni departman oluştur
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            var result = await _departmentService.CreateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Departman başarıyla oluşturuldu", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Departman güncelle
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            if (id != dto.Id)
                return ApiResponse<object>(null, "ID uyuşmazlığı", false);

            var result = await _departmentService.UpdateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Departman başarıyla güncellendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Departman sil
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _departmentService.DeleteAsync(id);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "Departman başarıyla silindi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Departman aktiflik durumunu değiştir
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> ToggleStatus(int id, [FromBody] bool isActive)
    {
        try
        {
            var result = await _departmentService.SetActiveStatusAsync(id, isActive);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "Departman durumu başarıyla güncellendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }
}