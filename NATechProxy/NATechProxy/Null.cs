using System;
using System.Reflection;

namespace NATechProxy;

public class Null
{
	public static short NullShort => -1;

	public static int NullInteger => -99999;

	public static byte NullByte => byte.MaxValue;

	public static float NullSingle => float.MinValue;

	public static double NullDouble => double.MinValue;

	public static decimal NullDecimal => decimal.MinValue;

	public static DateTime NullDate => DateTime.MinValue;

	public static string NullString => "";

	public static bool NullBoolean => false;

	public static Guid NullGuid => Guid.Empty;

	public static object SetNull(object objValue, object objField)
	{
		object obj = null;
		Type type = objField.GetType();
		Type type2 = objField.GetType();
		if (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
		{
			type2 = type.GetGenericArguments()[0];
		}
		if (Convert.IsDBNull(objValue))
		{
			if (type2 == typeof(short))
			{
				return NullShort;
			}
			if (type2 == typeof(byte))
			{
				return NullByte;
			}
			if (type2 == typeof(int))
			{
				return NullInteger;
			}
			if (type2 == typeof(float))
			{
				return NullSingle;
			}
			if (type2 == typeof(double))
			{
				return NullDouble;
			}
			if (type2 == typeof(decimal))
			{
				return NullDecimal;
			}
			if (type2 == typeof(DateTime))
			{
				return NullDate;
			}
			if (objField is string)
			{
				return NullString;
			}
			if (type2 == typeof(bool))
			{
				return NullBoolean;
			}
			if (type2 == typeof(Guid))
			{
				return NullGuid;
			}
			return null;
		}
		return objValue;
	}

	public static object SetNull(PropertyInfo objPropertyInfo)
	{
		object obj = null;
		switch (objPropertyInfo.PropertyType.ToString())
		{
		case "System.Int16":
			return NullShort;
		case "System.Int32":
		case "System.Int64":
			return NullInteger;
		case "system.Byte":
			return NullByte;
		case "System.Single":
			return NullSingle;
		case "System.Double":
			return NullDouble;
		case "System.Decimal":
			return NullDecimal;
		case "System.DateTime":
			return NullDate;
		case "System.String":
		case "System.Char":
			return NullString;
		case "System.Boolean":
			return NullBoolean;
		case "System.Guid":
			return NullGuid;
		default:
		{
			Type propertyType = objPropertyInfo.PropertyType;
			if (propertyType.BaseType.Equals(typeof(Enum)))
			{
				Array values = Enum.GetValues(propertyType);
				Array.Sort(values);
				return Enum.ToObject(propertyType, values.GetValue(0));
			}
			return null;
		}
		}
	}

	public static object GetNull(object objField, object objDBNull)
	{
		object obj = null;
		obj = objField;
		if (objField == null)
		{
			obj = objDBNull;
		}
		else if (objField is byte)
		{
			if (Convert.ToByte(objField) == NullByte)
			{
				obj = objDBNull;
			}
		}
		else if (objField is short)
		{
			if (Convert.ToInt16(objField) == NullShort)
			{
				obj = objDBNull;
			}
		}
		else if (objField is int)
		{
			if (Convert.ToInt32(objField) == NullInteger)
			{
				obj = objDBNull;
			}
		}
		else if (objField is long)
		{
			if (Convert.ToInt64(objField) == NullInteger)
			{
				obj = objDBNull;
			}
		}
		else if (objField is float)
		{
			if (Convert.ToSingle(objField) == NullSingle)
			{
				obj = objDBNull;
			}
		}
		else if (objField is double)
		{
			if (Convert.ToDouble(objField) == NullDouble)
			{
				obj = objDBNull;
			}
		}
		else if (objField is decimal)
		{
			if (Convert.ToDecimal(objField) == NullDecimal)
			{
				obj = objDBNull;
			}
		}
		else if (objField is DateTime)
		{
			if (Convert.ToDateTime(objField).Date == NullDate.Date)
			{
				obj = objDBNull;
			}
		}
		else if (objField is string)
		{
			if (objField == null)
			{
				obj = objDBNull;
			}
			else if (objField.ToString() == NullString)
			{
				obj = objDBNull;
			}
		}
		else if (objField is Guid guid && guid.Equals(NullGuid))
		{
			obj = objDBNull;
		}
		return obj;
	}

	public static bool IsNull(object objField)
	{
		bool flag = false;
		if (objField != null)
		{
			if (objField is int)
			{
				return objField.Equals(NullInteger);
			}
			if (objField is short)
			{
				return objField.Equals(NullShort);
			}
			if (objField is byte)
			{
				return objField.Equals(NullByte);
			}
			if (objField is float)
			{
				return objField.Equals(NullSingle);
			}
			if (objField is double)
			{
				return objField.Equals(NullDouble);
			}
			if (objField is decimal)
			{
				return objField.Equals(NullDecimal);
			}
			if (objField is DateTime dateTime)
			{
				return dateTime.Date.Equals(NullDate.Date);
			}
			if (objField is string)
			{
				return objField.Equals(NullString);
			}
			if (objField is bool)
			{
				return objField.Equals(NullBoolean);
			}
			if (objField is Guid)
			{
				return objField.Equals(NullGuid);
			}
			return false;
		}
		return true;
	}
}
