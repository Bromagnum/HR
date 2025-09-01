namespace BLL.Utilities;

public class Result
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public static Result SuccessResult(string message = "İşlem başarılı")
    {
        return new Result { Success = true, Message = message };
    }

    public static Result ErrorResult(string message)
    {
        return new Result { Success = false, Message = message };
    }

    public static Result ErrorResult(List<string> errors)
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

    public static Result<T> SuccessResult(T data, string message = "İşlem başarılı")
    {
        return new Result<T> { Success = true, Data = data, Message = message };
    }

    public static new Result<T> ErrorResult(string message)
    {
        return new Result<T> { Success = false, Message = message };
    }

    public static new Result<T> ErrorResult(List<string> errors)
    {
        return new Result<T> 
        { 
            Success = false, 
            Message = "Validasyon hataları oluştu", 
            Errors = errors 
        };
    }
}
