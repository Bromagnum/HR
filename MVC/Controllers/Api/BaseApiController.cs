using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Utilities;

namespace MVC.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Başarılı yanıt döndürür
    /// </summary>
    /// <typeparam name="T">Yanıt veri tipi</typeparam>
    /// <param name="data">Yanıt verisi</param>
    /// <param name="message">Başarı mesajı</param>
    /// <returns>HTTP 200 OK</returns>
    protected IActionResult Success<T>(T data, string message = "İşlem başarılı")
    {
        return Ok(new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Başarılı yanıt döndürür (veri olmadan)
    /// </summary>
    /// <param name="message">Başarı mesajı</param>
    /// <returns>HTTP 200 OK</returns>
    protected IActionResult Success(string message = "İşlem başarılı")
    {
        return Ok(new ApiResponse
        {
            Success = true,
            Message = message,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Hata yanıtı döndürür
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="statusCode">HTTP durum kodu</param>
    /// <returns>HTTP Hata Yanıtı</returns>
    protected IActionResult Error(string message, int statusCode = 400)
    {
        var response = new ApiResponse
        {
            Success = false,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        return statusCode switch
        {
            400 => BadRequest(response),
            401 => Unauthorized(response),
            403 => Forbid(),
            404 => NotFound(response),
            500 => StatusCode(500, response),
            _ => BadRequest(response)
        };
    }

    /// <summary>
    /// Result pattern'den API yanıtı oluşturur
    /// </summary>
    /// <typeparam name="T">Veri tipi</typeparam>
    /// <param name="result">İşlem sonucu</param>
    /// <returns>API Yanıtı</returns>
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Success(result.Data, result.Message);
        }

        return Error(result.Message);
    }

    /// <summary>
    /// Result pattern'den API yanıtı oluşturur (veri olmadan)
    /// </summary>
    /// <param name="result">İşlem sonucu</param>
    /// <returns>API Yanıtı</returns>
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Success(result.Message);
        }

        return Error(result.Message);
    }

    /// <summary>
    /// Sayfalama bilgisi ile başarılı yanıt döndürür
    /// </summary>
    /// <typeparam name="T">Veri tipi</typeparam>
    /// <param name="data">Veri listesi</param>
    /// <param name="pageNumber">Sayfa numarası</param>
    /// <param name="pageSize">Sayfa boyutu</param>
    /// <param name="totalCount">Toplam kayıt sayısı</param>
    /// <param name="message">Başarı mesajı</param>
    /// <returns>HTTP 200 OK</returns>
    protected IActionResult PagedSuccess<T>(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount, string message = "İşlem başarılı")
    {
        return Ok(new PagedApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Model validasyon hatalarını kontrol eder
    /// </summary>
    /// <returns>Validasyon hata yanıtı veya null</returns>
    protected IActionResult? ValidateModel()
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new ValidationErrorResponse
            {
                Success = false,
                Message = "Validasyon hatası",
                Errors = errors,
                Timestamp = DateTime.UtcNow
            });
        }

        return null;
    }

    /// <summary>
    /// Model validasyon hatalarını dictionary olarak döndürür
    /// </summary>
    /// <returns>Hata dictionary'si</returns>
    protected Dictionary<string, string[]> GetModelErrors()
    {
        return ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
            );
    }

    /// <summary>
    /// API yanıtı oluşturur
    /// </summary>
    /// <typeparam name="T">Veri tipi</typeparam>
    /// <param name="data">Veri</param>
    /// <param name="message">Mesaj</param>
    /// <param name="success">Başarı durumu</param>
    /// <returns>API yanıtı</returns>
    protected IActionResult ApiResponse<T>(T data, string message, bool success = true)
    {
        if (success)
        {
            return Success(data, message);
        }
        else
        {
            return Error(message);
        }
    }
}

/// <summary>
/// API yanıt modeli
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Veri içeren API yanıt modeli
/// </summary>
/// <typeparam name="T">Veri tipi</typeparam>
public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}

/// <summary>
/// Sayfalanmış API yanıt modeli
/// </summary>
/// <typeparam name="T">Veri tipi</typeparam>
public class PagedApiResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// Validasyon hata yanıt modeli
/// </summary>
public class ValidationErrorResponse : ApiResponse
{
    public Dictionary<string, string[]>? Errors { get; set; }
}
