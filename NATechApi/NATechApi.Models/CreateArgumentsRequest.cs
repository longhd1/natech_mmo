using System.Collections.Generic;

namespace NATechApi.Models;

public class CreateArgumentsRequest
{
	public List<string> lstAgrs { get; set; }

	public CreateArgumentsRequest(List<string> lstAgrs)
	{
		this.lstAgrs = lstAgrs;
	}
}
