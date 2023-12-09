using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Excel;

namespace NC.Lib;

public class ImportData
{
	internal class CSVReader : IDisposable
	{
		private string strSource = string.Empty;

		public int ScanRows = 0;

		private string currentLine = "";

		public const string NEWLINE = "\r\n";

		public CSVReader(FileInfo csvFileInfo)
		{
			if (csvFileInfo == null)
			{
				strSource = Misc.ReadFileToString(csvFileInfo.FullName);
			}
		}

		public CSVReader(string csvData)
		{
			if (csvData == null)
			{
				throw new ArgumentNullException("Null string passed to CSVReader");
			}
			strSource = csvData;
		}

		public CSVReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("Null TextReader passed to CSVReader");
			}
			strSource = reader.ReadToEnd();
			reader.Close();
		}

		public List<object> ReadRow(string srow)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(srow);
			currentLine = stringBuilder.ToString();
			if (currentLine.EndsWith("\r\n"))
			{
				currentLine = currentLine.Remove(currentLine.IndexOf("\r\n"), "\r\n".Length);
			}
			List<object> list = new List<object>();
			while (currentLine != "")
			{
				list.Add(ReadNextObject());
			}
			return list;
		}

		private object ReadNextObject()
		{
			if (currentLine != null)
			{
				bool flag = false;
				if (currentLine.StartsWith("\""))
				{
					flag = true;
				}
				int num = 0;
				int length = currentLine.Length;
				bool flag2 = false;
				while (!flag2 && num <= length)
				{
					if ((flag || num != length) && (flag || !(currentLine.Substring(num, 1) == ",")) && (!flag || num != length - 1 || !currentLine.EndsWith("\"")) && (!flag || !(currentLine.Substring(num, 2) == "\",")))
					{
						num++;
					}
					else
					{
						flag2 = true;
					}
				}
				if (flag)
				{
					if (num > length || !currentLine.Substring(num, 1).StartsWith("\""))
					{
						throw new FormatException("Invalid CSV format: " + currentLine.Substring(0, num));
					}
					num++;
				}
				string text = currentLine.Substring(0, num).Replace("\"\"", "\"");
				currentLine = ((num >= length) ? "" : currentLine.Substring(num + 1));
				if (!flag)
				{
					StringConverter.ConvertString(text, out var convertedValue);
					return convertedValue;
				}
				if (text.StartsWith("\""))
				{
					text = text.Substring(1);
				}
				if (text.EndsWith("\""))
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			return null;
		}

		public DataTable CreateDataTable(bool headerRow)
		{
			List<List<object>> list = new List<List<object>>();
			foreach (string item2 in Misc.SplitToList(strSource, "\r\n"))
			{
				List<object> item = ReadRow(item2);
				list.Add(item);
			}
			List<Type> list2 = new List<Type>();
			List<string> list3 = new List<string>();
			if (headerRow)
			{
				foreach (object item3 in list[0])
				{
					list3.Add(item3.ToString());
				}
			}
			bool flag = false;
			foreach (List<object> item4 in list)
			{
				if (flag || !headerRow)
				{
					for (int i = 0; i < item4.Count; i++)
					{
						if (list2.Count < i + 1)
						{
							list2.Add(item4[i].GetType());
						}
						else
						{
							list2[i] = StringConverter.FindCommonType(list2[i], item4[i].GetType());
						}
					}
				}
				else
				{
					flag = true;
				}
			}
			DataTable dataTable = new DataTable();
			for (int j = 0; j < list2.Count; j++)
			{
				dataTable.Columns.Add();
				dataTable.Columns[j].DataType = list2[j];
				if (j < list3.Count)
				{
					dataTable.Columns[j].ColumnName = list3[j];
				}
			}
			bool flag2 = false;
			foreach (List<object> item5 in list)
			{
				if (flag2 || !headerRow)
				{
					DataRow dataRow = dataTable.NewRow();
					for (int k = 0; k < item5.Count; k++)
					{
						dataRow[k] = item5[k];
					}
					dataTable.Rows.Add(dataRow);
				}
				else
				{
					flag2 = true;
				}
			}
			return dataTable;
		}

		public static DataTable ReadCSVFile(string filename, bool headerRow, int scanRows)
		{
			using CSVReader cSVReader = new CSVReader(new FileInfo(filename));
			cSVReader.ScanRows = scanRows;
			return cSVReader.CreateDataTable(headerRow);
		}

		public static DataTable ReadCSVFile(string filename, bool headerRow)
		{
			using CSVReader cSVReader = new CSVReader(Misc.ReadFileToString(filename));
			return cSVReader.CreateDataTable(headerRow);
		}

		public void Dispose()
		{
		}
	}

	internal static class StringConverter
	{
		private static Dictionary<Type, Dictionary<Type, Type>> typeMap = null;

		private static object locker = new object();

		public static Type ConvertString(string value, out object convertedValue)
		{
			if (!byte.TryParse(value, out var result))
			{
				if (!short.TryParse(value, out var result2))
				{
					if (!int.TryParse(value, out var result3))
					{
						if (!long.TryParse(value, out var result4))
						{
							if (!ulong.TryParse(value, out var result5))
							{
								if (!float.TryParse(value, out var result6))
								{
									if (!double.TryParse(value, out var result7))
									{
										if (!decimal.TryParse(value, out var result8))
										{
											if (bool.TryParse(value, out var result9))
											{
												convertedValue = (result9 ? true : false);
												return typeof(bool);
											}
											if (!char.TryParse(value, out var result10))
											{
												convertedValue = value;
												return typeof(string);
											}
											convertedValue = result10;
											return typeof(char);
										}
										convertedValue = result8;
										return typeof(decimal);
									}
									convertedValue = result7;
									return typeof(double);
								}
								convertedValue = result6;
								return typeof(float);
							}
							convertedValue = result5;
							return typeof(ulong);
						}
						convertedValue = result4;
						return typeof(long);
					}
					convertedValue = result3;
					return typeof(int);
				}
				convertedValue = result2;
				return typeof(short);
			}
			convertedValue = result;
			return typeof(byte);
		}

		public static Type FindCommonType(Type typeA, Type typeB)
		{
			BuildTypeMap();
			if (!typeMap.ContainsKey(typeA))
			{
				return typeof(string);
			}
			if (typeMap[typeA].ContainsKey(typeB))
			{
				return typeMap[typeA][typeB];
			}
			return typeof(string);
		}

		private static void BuildTypeMap()
		{
			lock (locker)
			{
				if (typeMap == null)
				{
					Dictionary<Type, Dictionary<Type, Type>> dictionary = new Dictionary<Type, Dictionary<Type, Type>>();
					dictionary.Add(typeof(byte), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(byte)
						},
						{
							typeof(short),
							typeof(short)
						},
						{
							typeof(int),
							typeof(int)
						},
						{
							typeof(long),
							typeof(long)
						},
						{
							typeof(ulong),
							typeof(ulong)
						},
						{
							typeof(float),
							typeof(float)
						},
						{
							typeof(double),
							typeof(double)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(short), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(short)
						},
						{
							typeof(short),
							typeof(short)
						},
						{
							typeof(int),
							typeof(int)
						},
						{
							typeof(long),
							typeof(long)
						},
						{
							typeof(ulong),
							typeof(ulong)
						},
						{
							typeof(float),
							typeof(float)
						},
						{
							typeof(double),
							typeof(double)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(int), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(int)
						},
						{
							typeof(short),
							typeof(int)
						},
						{
							typeof(int),
							typeof(int)
						},
						{
							typeof(long),
							typeof(long)
						},
						{
							typeof(ulong),
							typeof(ulong)
						},
						{
							typeof(float),
							typeof(float)
						},
						{
							typeof(double),
							typeof(double)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(long), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(long)
						},
						{
							typeof(short),
							typeof(long)
						},
						{
							typeof(int),
							typeof(long)
						},
						{
							typeof(long),
							typeof(long)
						},
						{
							typeof(ulong),
							typeof(ulong)
						},
						{
							typeof(float),
							typeof(float)
						},
						{
							typeof(double),
							typeof(double)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(ulong), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(ulong)
						},
						{
							typeof(short),
							typeof(ulong)
						},
						{
							typeof(int),
							typeof(ulong)
						},
						{
							typeof(long),
							typeof(ulong)
						},
						{
							typeof(ulong),
							typeof(ulong)
						},
						{
							typeof(float),
							typeof(float)
						},
						{
							typeof(double),
							typeof(double)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(float), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(float)
						},
						{
							typeof(short),
							typeof(float)
						},
						{
							typeof(int),
							typeof(float)
						},
						{
							typeof(long),
							typeof(float)
						},
						{
							typeof(ulong),
							typeof(float)
						},
						{
							typeof(float),
							typeof(float)
						},
						{
							typeof(double),
							typeof(double)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(double), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(double)
						},
						{
							typeof(short),
							typeof(double)
						},
						{
							typeof(int),
							typeof(double)
						},
						{
							typeof(long),
							typeof(double)
						},
						{
							typeof(ulong),
							typeof(double)
						},
						{
							typeof(float),
							typeof(double)
						},
						{
							typeof(double),
							typeof(double)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(decimal), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(decimal)
						},
						{
							typeof(short),
							typeof(decimal)
						},
						{
							typeof(int),
							typeof(decimal)
						},
						{
							typeof(long),
							typeof(decimal)
						},
						{
							typeof(ulong),
							typeof(decimal)
						},
						{
							typeof(float),
							typeof(decimal)
						},
						{
							typeof(double),
							typeof(decimal)
						},
						{
							typeof(decimal),
							typeof(decimal)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(bool), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(string)
						},
						{
							typeof(short),
							typeof(string)
						},
						{
							typeof(int),
							typeof(string)
						},
						{
							typeof(long),
							typeof(string)
						},
						{
							typeof(ulong),
							typeof(string)
						},
						{
							typeof(float),
							typeof(string)
						},
						{
							typeof(double),
							typeof(string)
						},
						{
							typeof(decimal),
							typeof(string)
						},
						{
							typeof(bool),
							typeof(bool)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(char), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(string)
						},
						{
							typeof(short),
							typeof(string)
						},
						{
							typeof(int),
							typeof(string)
						},
						{
							typeof(long),
							typeof(string)
						},
						{
							typeof(ulong),
							typeof(string)
						},
						{
							typeof(float),
							typeof(string)
						},
						{
							typeof(double),
							typeof(string)
						},
						{
							typeof(decimal),
							typeof(string)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(char)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					dictionary.Add(typeof(string), new Dictionary<Type, Type>
					{
						{
							typeof(byte),
							typeof(string)
						},
						{
							typeof(short),
							typeof(string)
						},
						{
							typeof(int),
							typeof(string)
						},
						{
							typeof(long),
							typeof(string)
						},
						{
							typeof(ulong),
							typeof(string)
						},
						{
							typeof(float),
							typeof(string)
						},
						{
							typeof(double),
							typeof(string)
						},
						{
							typeof(decimal),
							typeof(string)
						},
						{
							typeof(bool),
							typeof(string)
						},
						{
							typeof(char),
							typeof(string)
						},
						{
							typeof(string),
							typeof(string)
						}
					});
					typeMap = dictionary;
				}
			}
		}
	}

	public DataSet DataReader(string filePath, bool isFirstRowAsDatatableColumn)
	{
		string text = Path.GetExtension(filePath).ToLower();
		DataSet dataSet = null;
		string text2 = text;
		string text3 = text2;
		string text4 = text3;
		if (text4 != null)
		{
			switch (text2)
			{
			case ".txt":
				dataSet = CSVDataReader(filePath, isFirstRowAsDatatableColumn);
				break;
			case ".csv":
				dataSet = CSVDataReader(filePath, isFirstRowAsDatatableColumn);
				break;
			case ".xlsx":
				dataSet = ExcelDataReader(filePath, isFirstRowAsDatatableColumn);
				break;
			case ".xls":
				dataSet = ExcelDataReader(filePath, isFirstRowAsDatatableColumn);
				break;
			}
		}
		if (dataSet != null)
		{
			foreach (DataTable table in dataSet.Tables)
			{
				for (int num = table.Rows.Count - 1; num > -1; num--)
				{
					DataRow dataRow = table.Rows[num];
					bool flag = true;
					foreach (DataColumn column in table.Columns)
					{
						if (!Misc.IsNullOrDbNull(dataRow[column.ColumnName]) && (column.DataType != typeof(string) || dataRow[column.ColumnName].ToString().Length != 0))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						dataRow.Delete();
					}
				}
				table.Columns.Add(new DataColumn("Status", typeof(string)));
				table.Columns.Add(new DataColumn("isSkip", typeof(bool)));
			}
			dataSet.AcceptChanges();
		}
		return dataSet;
	}

	private DataSet ExcelDataReader(string filePath, bool isFirstRowAsDatatableColumn)
	{
		try
		{
			FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
			IExcelDataReader excelDataReader = (Path.GetExtension(filePath).ToLower().Equals(".xlsx") ? ExcelReaderFactory.CreateOpenXmlReader(fileStream) : ExcelReaderFactory.CreateBinaryReader(fileStream));
			excelDataReader.IsFirstRowAsColumnNames = isFirstRowAsDatatableColumn;
			DataSet result = excelDataReader.AsDataSet();
			excelDataReader.Close();
			return result;
		}
		catch (Exception ex)
		{
			if (ex.Message.StartsWith("Error reading stream from FAT area."))
			{
				throw new Exception("ERP:Có sự cố khi đọc file. Xử lý bằng cách dùng chương trình Excel mở file này ra sửa chữa và lưu lại.");
			}
			throw ex;
		}
	}

	private DataSet CSVDataReader(string filePath, bool isFirstRowAsDatatableColumn)
	{
		CSVReader cSVReader = new CSVReader(Misc.ReadFileToString(filePath));
		return new DataSet
		{
			Tables = { cSVReader.CreateDataTable(isFirstRowAsDatatableColumn) }
		};
	}
}
