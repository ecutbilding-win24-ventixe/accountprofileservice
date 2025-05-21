namespace Business.Models;

public class AccountProfileResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class AccountProfileResult : ServiceResult { }