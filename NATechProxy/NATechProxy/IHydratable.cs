using System.Data;

namespace NATechProxy;

public interface IHydratable
{
	int KeyID { get; set; }

	void Fill(IDataReader dr);
}
