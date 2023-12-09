namespace NATechApi.Models;

public class GetTikTokProfileRequest
{
	public string path { get; set; }

	public string email { get; set; }

	public GetTikTokProfileRequest()
	{
	}

	public GetTikTokProfileRequest(string path, string email)
	{
		this.email = email;
		this.path = path;
	}
}
