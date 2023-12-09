namespace NATechProxy.Class;

public class ProxyRotation
{
	public int Supplier { get; set; }

	public int TypeProxy { get; set; }

	public string Key { get; set; }

	public string Location { get; set; }

	public ProxyRotation()
	{
	}

	public ProxyRotation(int supplier, int typeProxy, string key, string location)
	{
		Supplier = supplier;
		TypeProxy = typeProxy;
		Key = key;
		Location = location;
	}
}
