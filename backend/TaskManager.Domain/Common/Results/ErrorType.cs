namespace TaskManager.Domain.Common.Results
{
    public enum ErrorType
    {
        Failure,
        Validation,
        Conflict,
        NotFound,
        Unauthorized,
        Forbidden,
    }
}
