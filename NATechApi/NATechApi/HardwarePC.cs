using System;
using System.Management;
using Microsoft.Win32;

namespace NATechApi;

public class HardwarePC
{
	public string GetCPUID()
	{
		string text = string.Empty;
		ManagementClass managementClass = new ManagementClass("win32_processor");
		ManagementObjectCollection instances = managementClass.GetInstances();
		foreach (ManagementObject item in instances)
		{
			if (text == "")
			{
				text = item.Properties["processorID"].Value.ToString();
				break;
			}
		}
		return text;
	}

	public static string identifier(string wmiClass, string wmiProperty)
	{
		string text = "";
		ManagementClass managementClass = new ManagementClass(wmiClass);
		ManagementObjectCollection instances = managementClass.GetInstances();
		foreach (ManagementObject item in instances)
		{
			if (!(text == ""))
			{
				continue;
			}
			try
			{
				if (item != null && item[wmiProperty] != null)
				{
					text = item[wmiProperty].ToString();
					break;
				}
			}
			catch
			{
			}
		}
		return text;
	}

	public static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
	{
		string text = "";
		ManagementClass managementClass = new ManagementClass(wmiClass);
		ManagementObjectCollection instances = managementClass.GetInstances();
		foreach (ManagementObject item in instances)
		{
			if (item[wmiMustBeTrue].ToString() == "True" && text == "")
			{
				try
				{
					text = item[wmiProperty].ToString();
				}
				catch
				{
					continue;
				}
				break;
			}
		}
		return text;
	}

	public static string cpuId()
	{
		string text = identifier("Win32_Processor", "UniqueId");
		if (text == "")
		{
			text = identifier("Win32_Processor", "ProcessorId");
			if (text == "")
			{
				text = identifier("Win32_Processor", "Name");
				if (text == "")
				{
					text = identifier("Win32_Processor", "Manufacturer");
				}
				text += identifier("Win32_Processor", "MaxClockSpeed");
			}
		}
		return text;
	}

	public static string biosId()
	{
		return identifier("Win32_BIOS", "Manufacturer") + identifier("Win32_BIOS", "SMBIOSBIOSVersion") + identifier("Win32_BIOS", "IdentificationCode") + identifier("Win32_BIOS", "SerialNumber") + identifier("Win32_BIOS", "ReleaseDate") + identifier("Win32_BIOS", "Version");
	}

	public static string biosSerialNumber()
	{
		return identifier("Win32_BIOS", "SerialNumber");
	}

	public static string diskId()
	{
		return identifier("Win32_DiskDrive", "Model") + identifier("Win32_DiskDrive", "Manufacturer") + identifier("Win32_DiskDrive", "Signature") + identifier("Win32_DiskDrive", "TotalHeads");
	}

	public static string baseId()
	{
		return identifier("Win32_BaseBoard", "Model") + identifier("Win32_BaseBoard", "Manufacturer") + identifier("Win32_BaseBoard", "Name") + identifier("Win32_BaseBoard", "SerialNumber");
	}

	public static string MainBoardSerialNumber()
	{
		return identifier("Win32_BaseBoard", "SerialNumber");
	}

	public static string videoId()
	{
		return identifier("Win32_VideoController", "DriverVersion") + identifier("Win32_VideoController", "Name");
	}

	public static string macId()
	{
		return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
	}

	public static string ComputerName()
	{
		return Environment.MachineName;
	}

	public static string ComputerName(int LengthLimit)
	{
		string text = ComputerName();
		if (text.Length > LengthLimit)
		{
			text = text.Substring(0, LengthLimit);
		}
		return text;
	}

	public static bool SetMachineName(string newName)
	{
		RegistryKey localMachine = Registry.LocalMachine;
		string subkey = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ActiveComputerName";
		RegistryKey registryKey = localMachine.CreateSubKey(subkey);
		registryKey.SetValue("ComputerName", newName);
		registryKey.Close();
		string subkey2 = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName";
		RegistryKey registryKey2 = localMachine.CreateSubKey(subkey2);
		registryKey2.SetValue("ComputerName", newName);
		registryKey2.Close();
		string subkey3 = "SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters\\";
		RegistryKey registryKey3 = localMachine.CreateSubKey(subkey3);
		registryKey3.SetValue("Hostname", newName);
		registryKey3.SetValue("NV Hostname", newName);
		registryKey3.Close();
		return true;
	}
}
