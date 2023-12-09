namespace NATechApi.Models;

public class ProxyNATech
{
	public string Ip { get; set; }

	public string Port { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public string PublicIp { get; set; }

	public ProxyNATech()
	{
	}

	public ProxyNATech(string ip, string port, string userName, string password, string publicIp)
	{
		Ip = ip;
		Port = port;
		UserName = userName;
		Password = password;
		PublicIp = publicIp;
	}
}
