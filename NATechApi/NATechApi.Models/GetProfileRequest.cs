namespace NATechApi.Models;

public class GetProfileRequest
{
	public string ProfileName { get; set; }

	public string UserAgent { get; set; }

	public ProxyNATech Proxy { get; set; }

	public string BrowserLanguage { get; set; }

	public GetProfileRequest()
	{
	}

	public GetProfileRequest(string profileName, string userAgent, ProxyNATech proxy, string browserLanguage)
	{
		ProfileName = profileName;
		UserAgent = userAgent;
		Proxy = proxy;
		BrowserLanguage = browserLanguage;
	}
}
