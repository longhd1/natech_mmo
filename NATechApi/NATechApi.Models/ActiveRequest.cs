namespace NATechApi.Models;

public class ActiveRequest
{
	public string Email { get; set; }

	public string Code { get; set; }

	public int SoftId { get; set; }

	public ActiveRequest()
	{
	}

	public ActiveRequest(string email, string code, int softId)
	{
		Email = email;
		Code = code;
		SoftId = softId;
	}
}
