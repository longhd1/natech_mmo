namespace NATechApi.Models;

public class RegisterRequest
{
	public string UserName { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public string PasswordConfim { get; set; }

	public string Phone { get; set; }

	public string HardKey { get; set; }

	public int SoftId { get; set; }

	public RegisterRequest()
	{
	}

	public RegisterRequest(string userName, string email, string password, string passwordConfim, string phone, string hardKey, int softId)
	{
		UserName = userName;
		Email = email;
		Password = password;
		PasswordConfim = passwordConfim;
		Phone = phone;
		HardKey = hardKey;
		SoftId = softId;
	}
}
