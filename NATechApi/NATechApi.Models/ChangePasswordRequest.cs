namespace NATechApi.Models;

public class ChangePasswordRequest
{
	public string UserName { get; set; }

	public string CurrentPassword { get; set; }

	public string NewPassword { get; set; }

	public string NewPasswordConfim { get; set; }

	public int SoftId { get; set; }

	public ChangePasswordRequest()
	{
	}

	public ChangePasswordRequest(string userName, string currentPassword, string newPassword, string newPasswordConfim, int softId)
	{
		UserName = userName;
		CurrentPassword = currentPassword;
		NewPassword = newPassword;
		NewPasswordConfim = newPasswordConfim;
		SoftId = softId;
	}
}
