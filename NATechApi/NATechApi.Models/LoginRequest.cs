namespace NATechApi.Models;

public class LoginRequest
{
	public string UserName { get; set; }

	public string Password { get; set; }

	public bool RememberMe { get; set; }

	public int SoftId { get; set; }

	public string HardKey { get; set; }

	public LoginRequest()
	{
	}

	public LoginRequest(string userName, string password, bool rememberMe, int softId, string hardKey)
	{
		UserName = userName;
		Password = password;
		RememberMe = rememberMe;
		SoftId = softId;
		HardKey = hardKey;
	}
}
