namespace NATechApi.Models;

public class ForgotPasswordRequireRequest
{
	public string UserName { get; set; }

	public int SoftId { get; set; }

	public ForgotPasswordRequireRequest()
	{
	}

	public ForgotPasswordRequireRequest(string userName, int softId)
	{
		UserName = userName;
		SoftId = softId;
	}
}
