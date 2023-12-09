namespace NATechApi.Models;

public class ErrorSV
{
	public int ErrorCode { get; set; }

	public string ErrorMessage { get; set; }

	public ErrorSV()
	{
	}

	public ErrorSV(int errorCode, string errorMessage)
	{
		ErrorCode = errorCode;
		ErrorMessage = errorMessage;
	}
}
