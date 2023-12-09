using System;

namespace NATechApi.Models;

public class LoginResponse
{
	public string Token { get; set; }

	public ErrorSV sErr { get; set; }

	public int UserId { get; set; }

	public string UserName { get; set; }

	public string Email { get; set; }

	public string Phone { get; set; }

	public int SoftId { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public string SoftVersion { get; set; }

	public DateTime CurrentTime { get; set; }

	public bool Licensed { get; set; }

	public bool MultipleDevice { get; set; }

	public int? NumberOfDevice { get; set; }

	public DateTime? LastLogin { get; set; }

	public LoginResponse()
	{
		sErr = new ErrorSV();
	}

	public LoginResponse(string Token, ErrorSV sErr)
	{
		this.Token = Token;
		this.sErr = sErr;
	}
}
