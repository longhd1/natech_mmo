namespace NATechApi.Models;

public class ForgotPasswordSubmitRequest
{
	public string UserName { get; set; }

	public string SubmitCode { get; set; }

	public int SoftId { get; set; }

	public ForgotPasswordSubmitRequest()
	{
	}

	public ForgotPasswordSubmitRequest(string userName, string submitCode, int softId)
	{
		UserName = userName;
		SubmitCode = submitCode;
		SoftId = softId;
	}
}
