using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NATechDriver;

public static class Json
{
	public static Dictionary<string, object> DeserializeData(string data)
	{
		return DeserializeData(JsonConvert.DeserializeObject<Dictionary<string, object>>(data)) as Dictionary<string, object>;
	}

	private static IDictionary<string, object> DeserializeData(JObject data)
	{
		return DeserializeData(data.ToObject<Dictionary<string, object>>());
	}

	private static IDictionary<string, object> DeserializeData(IDictionary<string, object> data)
	{
		string[] array = data.Keys.ToArray();
		foreach (string key in array)
		{
			object obj = data[key];
			if (obj is JObject)
			{
				data[key] = DeserializeData(obj as JObject);
			}
			if (obj is JArray)
			{
				data[key] = DeserializeData(obj as JArray);
			}
		}
		return data;
	}

	private static IList<object> DeserializeData(JArray data)
	{
		List<object> list = data.ToObject<List<object>>();
		for (int i = 0; i < list.Count; i++)
		{
			object obj = list[i];
			if (obj is JObject)
			{
				list[i] = DeserializeData(obj as JObject);
			}
			if (obj is JArray)
			{
				list[i] = DeserializeData(obj as JArray);
			}
		}
		return list;
	}
}
