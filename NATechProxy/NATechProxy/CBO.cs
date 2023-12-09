using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NATechProxy;

public class CBO
{
	private static object CreateObject(Type objType, IDataReader dr)
	{
		object obj = Activator.CreateInstance(objType);
		if (obj is IHydratable)
		{
			if (obj is IHydratable hydratable)
			{
				hydratable.Fill(dr);
			}
		}
		else
		{
			HydrateObject(obj, dr);
		}
		return obj;
	}

	private static T CreateObject<T>(IDataReader dr)
	{
		T val = Activator.CreateInstance<T>();
		if (val is IHydratable)
		{
			if ((object)val is IHydratable hydratable)
			{
				hydratable.Fill(dr);
			}
		}
		else
		{
			HydrateObject(val, dr);
		}
		return val;
	}

	private static int[] GetOrdinals(ArrayList objProperties, IDataReader dr)
	{
		int[] array = new int[objProperties.Count + 1];
		Hashtable hashtable = new Hashtable();
		if (dr != null)
		{
			for (int i = 0; i <= dr.FieldCount - 1; i++)
			{
				hashtable[dr.GetName(i).ToUpperInvariant()] = "";
			}
			for (int i = 0; i <= objProperties.Count - 1; i++)
			{
				string text = ((PropertyInfo)objProperties[i]).Name.ToUpperInvariant();
				if (hashtable.ContainsKey(text))
				{
					array[i] = dr.GetOrdinal(text);
				}
				else
				{
					array[i] = -1;
				}
			}
		}
		return array;
	}

	private static void HydrateObject(object objObject, IDataReader dr)
	{
		Type type = null;
		ArrayList propertyInfo = GetPropertyInfo(objObject.GetType());
		int[] ordinals = GetOrdinals(propertyInfo, dr);
		for (int i = 0; i <= propertyInfo.Count - 1; i++)
		{
			PropertyInfo propertyInfo2 = (PropertyInfo)propertyInfo[i];
			type = propertyInfo2.PropertyType;
			if (!propertyInfo2.CanWrite || ordinals[i] == -1)
			{
				continue;
			}
			object value = dr.GetValue(ordinals[i]);
			Type type2 = value.GetType();
			if (value is DBNull)
			{
				propertyInfo2.SetValue(objObject, Null.SetNull(propertyInfo2), null);
				continue;
			}
			if (propertyInfo2.PropertyType.Equals(type2) || (propertyInfo2.PropertyType.GetGenericArguments().Length != 0 && propertyInfo2.PropertyType.GetGenericArguments()[0].Equals(type2)))
			{
				propertyInfo2.SetValue(objObject, value, null);
				continue;
			}
			try
			{
				if (type.BaseType.Equals(typeof(Enum)))
				{
					if (IsNumeric(value))
					{
						propertyInfo2.SetValue(objObject, Enum.ToObject(type, Convert.ToInt32(value)), null);
					}
					else
					{
						propertyInfo2.SetValue(objObject, Enum.ToObject(type, value), null);
					}
				}
				else if (type.FullName.Equals("System.Guid"))
				{
					propertyInfo2.SetValue(objObject, Convert.ChangeType(new Guid(value.ToString()), type), null);
				}
				else if (type.FullName.Equals("System.String") && type2 == typeof(decimal))
				{
					propertyInfo2.SetValue(objObject, Convert.ToDecimal(value).ToString("#,#.##;-#,#.##;0"), null);
				}
				else
				{
					propertyInfo2.SetValue(objObject, Convert.ChangeType(value, type), null);
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
			}
		}
	}

	private static bool IsNumeric(object Expression)
	{
		double result;
		return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);
	}

	public static ArrayList FillCollection(IDataReader dr, Type objType)
	{
		ArrayList arrayList = new ArrayList();
		while (dr.Read())
		{
			object value = CreateObject(objType, dr);
			arrayList.Add(value);
		}
		dr?.Close();
		return arrayList;
	}

	public static IList FillCollection(IDataReader dr, Type objType, ref IList objToFill)
	{
		while (dr.Read())
		{
			object value = CreateObject(objType, dr);
			objToFill.Add(value);
		}
		dr?.Close();
		return objToFill;
	}

	public static List<T> FillCollection<T>(IDataReader dr)
	{
		List<T> list = new List<T>();
		while (dr.Read())
		{
			T item = CreateObject<T>(dr);
			list.Add(item);
		}
		dr?.Close();
		return list;
	}

	public static IList<T> FillCollection<T>(IDataReader dr, ref IList<T> objToFill)
	{
		while (dr.Read())
		{
			T item = CreateObject<T>(dr);
			objToFill.Add(item);
		}
		dr?.Close();
		return objToFill;
	}

	public static IList<T> FillCollection<T>(IDataReader dr, ref IList<T> objToFill, ref int totalRecords)
	{
		try
		{
			while (dr.Read())
			{
				T item = CreateObject<T>(dr);
				objToFill.Add(item);
			}
			if (dr.NextResult())
			{
				totalRecords = GetTotalRecords(dr);
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			dr?.Close();
		}
		return objToFill;
	}

	private static int GetTotalRecords(IDataReader dr)
	{
		int result = 0;
		if (dr.Read())
		{
			try
			{
				result = Convert.ToInt32(dr[0]);
			}
			catch
			{
				result = -1;
			}
		}
		return result;
	}

	public static IDictionary<int, T> FillDictionary<T>(IDataReader dr) where T : IHydratable
	{
		Dictionary<int, T> dictionary = new Dictionary<int, T>();
		while (dr.Read())
		{
			T val = CreateObject<T>(dr);
			if (val != null)
			{
				dictionary.Add(val.KeyID, val);
			}
		}
		dr?.Close();
		return dictionary;
	}

	public static IDictionary<int, T> FillDictionary<T>(IDataReader dr, ref IDictionary<int, T> objToFill) where T : IHydratable
	{
		while (dr.Read())
		{
			T val = CreateObject<T>(dr);
			if (val != null)
			{
				objToFill.Add(val.KeyID, val);
			}
		}
		dr?.Close();
		return objToFill;
	}

	public static object FillObject(IDataReader dr, Type objType)
	{
		if (dr == null)
		{
			return Activator.CreateInstance(objType);
		}
		return FillObject(dr, objType, ManageDataReader: true);
	}

	public static object FillObject(IDataReader dr, Type objType, bool ManageDataReader)
	{
		bool flag;
		if (ManageDataReader)
		{
			flag = false;
			if (dr.Read())
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		object result = ((!flag) ? null : CreateObject(objType, dr));
		if (ManageDataReader)
		{
			dr?.Close();
		}
		return result;
	}

	public static T FillObject<T>(IDataReader dr)
	{
		return FillObject<T>(dr, ManageDataReader: true);
	}

	public static T FillObject<T>(IDataReader dr, bool ManageDataReader)
	{
		bool flag;
		if (ManageDataReader)
		{
			flag = false;
			if (dr.Read())
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		T result = ((!flag) ? default(T) : CreateObject<T>(dr));
		if (ManageDataReader)
		{
			dr?.Close();
		}
		return result;
	}

	public static ArrayList GetPropertyInfo(Type objType)
	{
		ArrayList arrayList = null;
		if (arrayList == null)
		{
			arrayList = new ArrayList();
			PropertyInfo[] properties = objType.GetProperties();
			foreach (PropertyInfo value in properties)
			{
				arrayList.Add(value);
			}
		}
		return arrayList;
	}

	public static object InitializeObject(object objObject, Type objType)
	{
		ArrayList propertyInfo = GetPropertyInfo(objType);
		for (int i = 0; i <= propertyInfo.Count - 1; i++)
		{
			PropertyInfo propertyInfo2 = (PropertyInfo)propertyInfo[i];
			if (propertyInfo2.CanWrite)
			{
				object value = Null.SetNull(propertyInfo2);
				propertyInfo2.SetValue(objObject, value, null);
			}
		}
		return objObject;
	}

	public static XmlDocument Serialize(object objObject)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(objObject.GetType());
		StringBuilder sb = new StringBuilder();
		TextWriter textWriter = new StringWriter(sb);
		xmlSerializer.Serialize(textWriter, objObject);
		StringReader reader = new StringReader(textWriter.ToString());
		DataSet dataSet = new DataSet();
		dataSet.ReadXml(reader);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(dataSet.GetXml());
		return xmlDocument;
	}

	public static object GetProperty(object o, string propertyname)
	{
		object obj = null;
		try
		{
			if (o == null || o.GetType() == null)
			{
				return obj;
			}
			PropertyInfo property = o.GetType().GetProperty(propertyname);
			if (property != null)
			{
				obj = property.GetValue(o, null);
			}
		}
		catch
		{
		}
		if (obj == null)
		{
			obj = Null.GetNull(obj, DBNull.Value);
		}
		return obj;
	}

	public static void SetProperty(object o, string propertyname, object value)
	{
		try
		{
			if (o != null)
			{
				PropertyInfo property = o.GetType().GetProperty(propertyname);
				if (property != null && property.CanWrite)
				{
					property.SetValue(o, value, null);
				}
			}
		}
		catch
		{
		}
	}
}
