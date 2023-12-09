namespace NATechApi.Models;

public class RegisterResponse
{
	public ErrorSV sErr { get; set; }

	public int UserId { get; set; }

	public string UserName { get; set; }

	public string Email { get; set; }

	public string Phone { get; set; }

	public int SoftId { get; set; }

	public RegisterResponse()
	{
		sErr = new ErrorSV();
	}

	public RegisterResponse(ErrorSV sErr)
	{
		this.sErr = sErr;
	}
}
