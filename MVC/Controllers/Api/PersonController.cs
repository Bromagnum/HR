using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Services;
using BLL.DTOs;
using System.Security.Claims;

namespace MVC.Controllers.Api;

/// <summary>
/// Personel yönetimi için API endpoint'leri
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonController : BaseApiController
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// Tüm personelleri listele
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _personService.GetAllAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Personeller başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Aktif personelleri listele
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveEmployees()
    {
        try
        {
            var result = await _personService.GetActiveEmployeesAsync();
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Aktif personeller başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Personel detayını getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _personService.GetByIdAsync(id);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Personel detayı başarıyla getirildi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// TC Kimlik No ile personel getir
    /// </summary>
    [HttpGet("by-tc/{tcKimlikNo}")]
    public async Task<IActionResult> GetByTcKimlikNo(string tcKimlikNo)
    {
        try
        {
            var result = await _personService.GetByTcKimlikNoAsync(tcKimlikNo);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Personel başarıyla bulundu", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Departmana göre personelleri getir
    /// </summary>
    [HttpGet("by-department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        try
        {
            var result = await _personService.GetByDepartmentIdAsync(departmentId);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Departman personelleri başarıyla listelendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Yeni personel oluştur
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Create([FromBody] PersonCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            var result = await _personService.CreateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Personel başarıyla oluşturuldu", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Personel güncelle
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Update(int id, [FromBody] PersonUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(GetModelErrors(), "Geçersiz veri", false);

            if (id != dto.Id)
                return ApiResponse<object>(null, "ID uyuşmazlığı", false);

            var result = await _personService.UpdateAsync(dto);
            return result.IsSuccess 
                ? ApiResponse(result.Data, "Personel başarıyla güncellendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Personel sil
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _personService.DeleteAsync(id);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "Personel başarıyla silindi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }

    /// <summary>
    /// Personel aktiflik durumunu değiştir
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> ToggleStatus(int id, [FromBody] bool isActive)
    {
        try
        {
            var result = await _personService.SetActiveStatusAsync(id, isActive);
            return result.IsSuccess 
                ? ApiResponse<object>(null, "Personel durumu başarıyla güncellendi", true)
                : ApiResponse<object>(null, result.Message, false);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>(null, $"Bir hata oluştu: {ex.Message}", false);
        }
    }
}