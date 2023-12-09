namespace NATechProxy.Class;

public class EmailItem
{
	public string Email { get; set; }

	public string Password { get; set; }

	public string RecoveryEmail { get; set; }

	public EmailItem()
	{
	}

	public EmailItem(string email, string password, string recoveryEmail)
	{
		Email = email;
		Password = password;
		RecoveryEmail = recoveryEmail;
	}
}
