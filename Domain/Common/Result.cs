namespace Tickest.Domain.Common;

public class Result<T>
{
	public T Data { get; private set; }
	public string Error { get; private set; }
	public bool IsSuccess => string.IsNullOrEmpty(Error);

	public static Result<T> Success(T data) => new Result<T> { Data = data };
	public static Result<T> Failure(string error) => new Result<T> { Error = error };
}
