namespace NATechApi.Models;

public class ActiveResponse
{
	public bool IsActived { get; set; }

	public ErrorSV sErr { get; set; }

	public ActiveResponse()
	{
	}

	public ActiveResponse(bool isActived, ErrorSV sErr)
	{
		IsActived = isActived;
		this.sErr = sErr;
	}
}
