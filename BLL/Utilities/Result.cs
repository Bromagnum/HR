namespace BLL.Utilities;

public class Result
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    // IsSuccess property for compatibility
    public bool IsSuccess => Success;


    // Static methods for non-generic Result
    public static Result Ok(string message = "İşlem başarılı")
    {
        return new Result { Success = true, Message = message };
    }

    // OkResult alias for compatibility
    public static Result OkResult(string message = "İşlem başarılı")
    {
        return Ok(message);
    }

    public static Result Fail(string message)
    {
        return new Result { Success = false, Message = message };
    }

    public static Result Fail(List<string> errors)
    {
        return new Result 
        { 
            Success = false, 
            Message = "Validasyon hataları oluştu", 
            Errors = errors 
        };
    }

}

public class Result<T> : Result
{
    public T? Data { get; set; }


    // Static methods used in services
    public static Result<T> Ok(T data, string message = "İşlem başarılı")
    {
        return new Result<T> { Success = true, Data = data, Message = message };
    }

    public static new Result<T> Fail(string message)
    {
        return new Result<T> { Success = false, Message = message };
    }

    public static new Result<T> Fail(List<string> errors)
    {
        return new Result<T> 
        { 
            Success = false, 
            Message = "Validasyon hataları oluştu", 
            Errors = errors 
        };
    }
}
