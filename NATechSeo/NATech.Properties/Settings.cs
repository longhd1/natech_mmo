using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NATech.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string TinSoftKey
	{
		get
		{
			return (string)this["TinSoftKey"];
		}
		set
		{
			this["TinSoftKey"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string TinSoftTinh
	{
		get
		{
			return (string)this["TinSoftTinh"];
		}
		set
		{
			this["TinSoftTinh"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string TypeProxy
	{
		get
		{
			return (string)this["TypeProxy"];
		}
		set
		{
			this["TypeProxy"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string TinSoftLoaiKey
	{
		get
		{
			return (string)this["TinSoftLoaiKey"];
		}
		set
		{
			this["TinSoftLoaiKey"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string XProxyHost
	{
		get
		{
			return (string)this["XProxyHost"];
		}
		set
		{
			this["XProxyHost"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string OBCProxyHost
	{
		get
		{
			return (string)this["OBCProxyHost"];
		}
		set
		{
			this["OBCProxyHost"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string OBCv2ProxyHost
	{
		get
		{
			return (string)this["OBCv2ProxyHost"];
		}
		set
		{
			this["OBCv2ProxyHost"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string XProxyDeviceType
	{
		get
		{
			return (string)this["XProxyDeviceType"];
		}
		set
		{
			this["XProxyDeviceType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string XProxyTypeIP
	{
		get
		{
			return (string)this["XProxyTypeIP"];
		}
		set
		{
			this["XProxyTypeIP"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string OBCV3Host
	{
		get
		{
			return (string)this["OBCV3Host"];
		}
		set
		{
			this["OBCV3Host"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string OBCV3Proxy
	{
		get
		{
			return (string)this["OBCV3Proxy"];
		}
		set
		{
			this["OBCV3Proxy"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("65634bb89fbcb409fcfb9a1240d7b98e")]
	public string ProxyKey
	{
		get
		{
			return (string)this["ProxyKey"];
		}
		set
		{
			this["ProxyKey"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string ProxySupplier
	{
		get
		{
			return (string)this["ProxySupplier"];
		}
		set
		{
			this["ProxySupplier"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("https://api.na.com.vn")]
	public string NATechApiUrl
	{
		get
		{
			return (string)this["NATechApiUrl"];
		}
		set
		{
			this["NATechApiUrl"] = value;
		}
	}
}
