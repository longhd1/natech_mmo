namespace NATechApi.Models;

public class ForgotPasswordResponse
{
	public bool ResetOk { get; set; }

	public ErrorSV sErr { get; set; }
}
