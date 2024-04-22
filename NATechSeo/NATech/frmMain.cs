using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using Microsoft.Win32;
using NC.Lib;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using NATech.Properties;
using NATechApi;
using NATechApi.Models;
using NATechDriver;
using NATechProxy;
using NATechProxy.Class;
using NATechProxy.Class.TMProxy;

namespace NATech;

public class frmMain : Form
{
	private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

	private bllMain bllMainBLL = new bllMain();

	private bllProxy bllProxyBll = new bllProxy();

	private bllXProxy bllXProxyBll = new bllXProxy();

	private bllOBCProxy bllOBCProxyBll = new bllOBCProxy();

	private bllOBCProxyV2 bllOBCProxyV2Bll = new bllOBCProxyV2();

	private bllMultiProxy bllMultiProxyBll = new bllMultiProxy();

	private bllTMProxy bllTMProxyBll = new bllTMProxy();

	private const string Version = "5.5.2";

	private List<string> lstDialUp = new List<string>();

	private int maxClick = 0;

	private string pathFF = string.Empty;

	private int timeRelaxPerRound = 0;

	private int timeViewAdsFrom = 0;

	private int timeViewAdsTo = 0;

	private int TimeViewSearch = 0;

	private int OtherSiteViewTime = 5;

	private int Timeout = 180;

	private int SubLinkView = 10;

	private int TimeViewSearchTo = 0;

	private int SubLinkViewTo = 0;

	private int OtherSiteViewTimeTo = 0;

	private int indexCurrent;

	private int clickSuscess = 0;

	private bool viewOtherSite = false;

	private List<string> lstOtherSiteUrl;

	private bool TatDcom = false;

	private bool BatDcom = false;

	private bool KhoiDongDcom = false;

	private bool NotViewImage = false;

	private List<string> lstEmail = null;

	private int EmailDelay = 1;

	private int EmailIndex = 0;

	private string DesignBy = "NATECH (fb.com/na.com.vn - Tele @longhoang02363 - Zalo: 09222.33.666)";

	private int SoLuong = 1;

	private DataTable dtKeywordBingding;

	private DataTable dtKeywordRuning;

	private DataTable dtMultiProxyBinding;

	private string KeywordFileName = "Keyword.xml";

	private string TinsoftFileName = "Tinsoft.xml";

	private string TMProxyFileName = "TMProxy.xml";

	private int TypeIp = 0;

	private List<ProxyItems> lstProxy = null;

	private int proxyIndex = 0;

	private CultureInfo culture;

	private int TypeProxy = 1;

	private Dictionary<string, TinsoftProxy.TinsoftData> TinsoftData = new Dictionary<string, TinsoftProxy.TinsoftData>();

	private string XProxyHost = string.Empty;

	private string OBCProxyHost = string.Empty;

	private string OBCv2Host = string.Empty;

	private DataTable dtXProxyBinding;

	private DataTable dtXProxy;

	private DataTable dtOBCv2ProxyBinding;

	private DataTable dtOBCv2Proxy;

	private DataTable dtOBCProxyBinding;

	private DataTable dtOBCProxy;

	private DataTable dtMultiXProxy;

	private DataTable dtMultiOBCProxy;

	private DataTable dtMultiXProxyBinding;

	private DataTable dtMultiOBCProxyBinding;

	private DataTable dtMachProxy;

	private DataTable dtTinsoftType;

	private DataTable dtTinsoftTinhTP;

	private DataTable dtTinsoft;

	private DataTable dtOption;

	private DataTable dtTMProxy;

	private DataTable dtTMProxyType;

	private DataTable dtTMProxyTinhTP;

	private DataTable dtDisplayMode;

	private DataTable dtBrowserLanguage;

	private DataTable dtDeviceType;

	private string SupplierName = string.Empty;

	private Random r = new Random();

	private bool isViewPage = false;

	private string url = "https://www.google.com/";

	private string urlvideo = "https://www.google.com/webhp?tbm=vid";

	private string url_login = "https://accounts.google.com/ServiceLogin/identifier?hl=vi&flowName=GlifWebSignIn&flowEntry=AddSession";

	private string url_youtube = "https://www.youtube.com";

	private string sTinhTrang = "";

	private bool isRunFinish = false;

	private string sHistory = "";

	private int SoTrang = 1;

	private bool UseHistory = true;

	private List<string> lstAgent = null;

	private bool IsLocation = false;

	private string cultureName = "vi-VN";

	private DataTable dtHistory;

	private DataTable dtIp;

	private string HistoryFileName = "History.xml";

	private string IpFile = "Ip.xml";

	private int GMailType = 0;

	private List<string> lstProfile = new List<string>();

	private bool IsChangeMacAddress = false;

	private int MacAddressInterval = 10;

	private bool ChangeMacAddressFlag = false;

	private Dictionary<string, bool> dicBrowserStop = null;

	private bool SaveReport = true;

	private Dictionary<string, ProxyCommand> dicProxySupplier = new Dictionary<string, ProxyCommand>();

	private Dictionary<string, bllTMProxy.TMProxyData> TMProxyData;

	private int DcomTypeReset = 1;

	private int DcomDelay = 3;

	private int ResetDcomInterval = 3;

	private bool DangChoResetDcom = false;

	private bool ViewYoutube = false;

	private int LoadProfilePercent = 1;

	private int InternalCount = 1;

	private int SpeedKeyboard = 50;

	private int DCOMProxyDelay = 2;

	private bool CallPhoneZalo = false;

	private DataTable dtOldProfile = null;

	private bllOrbita bllOrbitaBLL = new bllOrbita();

	private int SoftId = 2064;

	private Dictionary<int, DriverRec> dicDriverRec = new Dictionary<int, DriverRec>();

	private int DisplayMode = 1;

	private int tempCountSecond = 0;

	private int ClearChromeTime = 60;

	private bool ClearChrome = false;

	private bool isClearChrome = false;

	private int TimerCount = 0;

	private string BrowserLanguage = "vi-VN,vi;q=0.9";

	private const int NumberOfRetries = 3;

	private const int DelayOnRetry = 1000;

	private DateTime TimeGetProxy = DateTime.Now.AddDays(-1.0);

	private IContainer components = null;

	private System.Windows.Forms.Timer timer1;

	private BarManager barManager1;

	private Bar bar3;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private BarEditItem bsiStatus;

	private RepositoryItemProgressBar bsieStatus;

	private Bar barMain;

	private BarSubItem bsiHelp;

	private BarButtonItem bbiRegister;

	private BarButtonItem bbiAbout;

	private BarSubItem bsiAccount;

	private BarButtonItem bbiVideoDemo;

	private BarButtonItem bbiUserAgent;

	private BarButtonItem bbiUpdate;

	private BarButtonItem bbiClose;

	private BarButtonItem bbiNotification;

	private BarButtonItem bbiGUI;

	private RepositoryItemRadioGroup repositoryItemRadioGroup1;

	private BarEditItem cbeLang;

	private RepositoryItemComboBox cbeeLang;

	private BarStaticItem bsiFanpage;

	private BarStaticItem barStaticItem1;

	private System.Windows.Forms.Timer tmSaveHistory;

	private BarButtonItem bbiBuy;

	private BarButtonItem bbiPrice;

	private BarButtonItem bbiFirefoxProfile;

	private BarButtonItem bbiTest;

	private System.Windows.Forms.Timer tmChangeMAC;

	private System.Windows.Forms.Timer tmAutoStart;

	private BarButtonItem bbiDcomSetting;

	private BarButtonItem bbiDisableIPv6;

	private BarButtonItem bbiRegisterTinsoft;

	private System.Windows.Forms.Timer tmResetDcom;

	private BarStaticItem bsiUserInfo;

	private BarButtonItem bbiChangePass;

	private BarSubItem bsiGUI;

	private BarButtonItem bbiLogin;

	private BarButtonItem bbiLogout;

	private BarButtonItem bbiResgisterAccount;

	private BarButtonItem bbiAcctiveAccount;

	private BarButtonItem bbiCreateProfile;

	private BarButtonItem bbiCreateProfileMain;

	private ImageCollection imageCollection1;

	private BarButtonItem bbiUpdateVersion;

	private BarButtonItem bbiBuyVPS;
    private LayoutControl lcMain;
    private MemoEdit mmeHistory;
    private GridLookUpEdit lueDeviceType;
    private GridView luevDeviceType;
    private GridColumn luevDeviceType_NAME;
    private CheckedComboBoxEdit ccbBrowserLanguage;
    private CheckEdit ceiClearChrome;
    private CheckEdit ceiCallPhoneZalo;
    private SpinEdit speClearChromeTime;
    private CheckEdit ceiViewFilm;
    private GridLookUpEdit lueDisplayMode;
    private GridView gridLookUpEdit1View;
    private GridColumn luevDisplayMode_NAME;
    private SimpleButton btnTinsoftImportExcel;
    private SimpleButton btnTinsoftExportExcel;
    private CheckEdit ceiStarupWindow;
    private SpinEdit speDCOMProxyDelay;
    private SpinEdit speSpeedKeyboard;
    private SpinEdit speInternalCount;
    private SimpleButton btnSyncUserAgent;
    private SpinEdit speLoadProfilePercent;
    private CheckEdit ceiViewYoutube;
    private GridControl grdTMProxy;
    private GridView grdvTMProxy;
    private GridColumn grdvTMProxy_Type;
    private RepositoryItemGridLookUpEdit grdvlueTMProxy_Type;
    private GridView gridView3;
    private GridColumn gridColumn11;
    private GridColumn grdvTMProxy_Key;
    private GridColumn grdvTMProxy_TinhTP;
    private RepositoryItemCheckedComboBoxEdit grdvlueTMProxy_TinhTP;
    private GridColumn grdvTMProxy_Xoa;
    private RepositoryItemButtonEdit grdvbeiTMProxy_Xoa;
    private SimpleButton btnTMProxyHome;
    private SpinEdit speDcomDelay;
    private SpinEdit speResetDcomInterval;
    private RadioGroup radDcomTypeReset;
    private SimpleButton btnSetupDCOM;
    private ComboBoxEdit cbeProxySupplier;
    private SimpleButton btnCreateProfile;
    private SimpleButton btnFirefoxProfile;
    private GridControl grdTinsoft;
    private GridView grdvTinsoft;
    private GridColumn grdvTinsoft_Type;
    private RepositoryItemGridLookUpEdit grdvlueTinsoft_Type;
    private GridView grdvluevTinsoft_Type;
    private GridColumn gridColumn9;
    private GridColumn grdvTinsoft_Key;
    private GridColumn grdvTinsoft_TinhTP;
    private RepositoryItemCheckedComboBoxEdit grdvccbTinsoft_TinhTP;
    private GridColumn grdvTinsoft_Select;
    private RepositoryItemCheckEdit grdvlueTinsoft_Select;
    private GridColumn grdvTinsoft_Delete;
    private RepositoryItemButtonEdit grdvbeiTinsoft_Delete;
    private RepositoryItemComboBox grdvcbeTinsoft_Type;
    private LabelControl lbeFakeIpNoticMacAddress;
    private MemoEdit memOBCV3Proxy;
    private TextEdit txtOBCV3Host;
    private CheckEdit ceiSaveReport;
    private GridControl grdOBCV2;
    private GridView grdvOBCV2;
    private GridColumn gridColumn1;
    private GridColumn gridColumn2;
    private GridColumn gridColumn3;
    private GridColumn gridColumn4;
    private GridColumn gridColumn5;
    private GridColumn gridColumn6;
    private GridColumn gridColumn7;
    private GridColumn gridColumn8;
    private SimpleButton btnOBCV2HomePage;
    private TextEdit txtOBCV2Host;
    private SimpleButton btnConnectOBCV2;
    private CheckEdit ceiAutoStart;
    private SpinEdit speSubLinkViewTo;
    private SpinEdit speOtherSiteViewTimeTo;
    private SpinEdit speTimeViewSearchTo;
    private SimpleButton btnGetAgent;
    private SimpleButton btnRegisterTinsoft;
    private SimpleButton btnDisableIPv6;
    private SimpleButton btnTinSoftHomepage;
    private SimpleButton btnXProxyHomepage;
    private SimpleButton btnOBCHomePage;
    private SimpleButton btnMultiProxyConnect;
    private GridControl grdMultiOBC;
    private GridView grdvMultiOBC;
    private GridColumn grdvMultiOBC_name;
    private GridColumn grdvMultiOBC_idKey;
    private GridColumn grdvMultiOBC_proxyAddress;
    private GridColumn grdvMultiOBC_proxyStatus;
    private GridColumn grdvMultiOBC_port;
    private GridColumn grdvMultiOBC_sockPort;
    private GridColumn grdvMultiOBC_socksEnable;
    private GridColumn grdvMultiOBC_ServiceUrl;
    private GridColumn grdvMultiOBC_IsRun;
    private RepositoryItemCheckEdit grdvlueMultiOBC_IsRun;
    private GridControl grdMultiXProxy;
    private GridView grdvMultiXProxy;
    private GridColumn grdvMultiXProxy_stt;
    private GridColumn grdvMultiXProxy_public_ip;
    private GridColumn grdvMultiXProxy_system;
    private GridColumn grdvMultiXProxy_proxy_port;
    private GridColumn grdvMultiXProxy_sock_port;
    private GridColumn grdvMultiXProxy_proxy_full;
    private GridColumn grdvMultiXProxy_imei;
    private GridColumn grdvMultiXProxy_ServiceUrl;
    private GridColumn grdvMultiXProxy_IsRun;
    private RepositoryItemCheckEdit grdvlueMultiXProxy_IsRun;
    private RadioGroup radMultiProxyType;
    private GridControl grdMultiProxy;
    private GridView grdvMultiProxy;
    private GridColumn grdvMultiProxy_Type;
    private RepositoryItemLookUpEdit grdvlueMultiProxy_Type;
    private GridColumn grdvMultiProxy_ServiceUrl;
    private RepositoryItemComboBox repositoryItemComboBox1;
    private SimpleButton btnClearAllKeyword;
    private SimpleButton btnClearSelectionKeyword;
    private GridControl grdOBC;
    private GridView grdvOBC;
    private GridColumn grdvOBC_name;
    private GridColumn grdvOBC_idKey;
    private GridColumn grdvOBC_proxyAddress;
    private GridColumn grdvOBC_proxyStatus;
    private GridColumn grdvOBC_port;
    private GridColumn grdvOBC_sockPort;
    private GridColumn grdvOBC_socksEnable;
    private GridColumn grdvOBC_publicIp;
    private GridColumn grdvOBC_IsRun;
    private SimpleButton btnConnectOBC;
    private TextEdit txtOBCHost;
    private SimpleButton btnImportKeyword;
    private SimpleButton btnExportKeyword;
    private GridControl grdKeyword;
    private GridView grdvKeyword;
    private GridColumn grdvKeyword_Key;
    private GridColumn grdvKeyword_Domain;
    private GridColumn grdvKeyword_SubLink;
    private RepositoryItemMemoExEdit luevSubLink;
    private GridColumn grdvKeyword_Delete;
    private RepositoryItemButtonEdit grdvbeiKeyword_Delete;
    private GridColumn grdvKeyword_Type;
    private RepositoryItemGridLookUpEdit grdvlueKeyword_Type;
    private GridView grdvluevKeyword_Type;
    private GridColumn grdvluevKeyword_Type_NAME;
    private SimpleButton btnExcelIp;
    private SimpleButton btnExcelDomain;
    private MemoEdit memReChuot;
    private GridControl grdXProxyList;
    private GridView grdvXProxyList;
    private GridColumn grdvXProxyList_stt;
    private GridColumn grdvXProxyList_public_ip;
    private GridColumn grdvXProxyList_system;
    private GridColumn grdvXProxyList_proxy_port;
    private GridColumn grdvXProxyList_sock_port;
    private GridColumn grdvXProxyList_proxy_full;
    private GridColumn grdvXProxyList_imei;
    private GridColumn grdvXProxyList_IsRun;
    private RepositoryItemCheckEdit ceiIsRun;
    private SimpleButton btnConnectxProxy;
    private TextEdit txtXProxyHost;
    private LabelControl lbeNoticeFreeProxy;
    private SimpleButton btnChangeMAC;
    private SpinEdit speMACAddressInterval;
    private CheckEdit ceiChangeMACAddress;
    private SpinEdit speSubLinkView;
    private GridControl grdIp;
    private GridView grdvIp;
    private GridColumn grdvIp_Ngay;
    private GridColumn grdvIp_Ip;
    private GridColumn grdvIp_Click;
    private SpinEdit speSoLuong;
    private MemoEdit memProfile;
    private RadioGroup radGMail;
    private LabelControl lbeNoticeTinsoft;
    private SimpleButton btnDeleteHistoryXml;
    private SimpleButton btnLoadHistory;
    private GridControl grdHistory;
    private GridView grdvHistory;
    private GridColumn grdvHistory_Ngay;
    private GridColumn grdvHistory_Domain;
    private GridColumn grdvHistory_Click;
    private MemoEdit memProxyNote;
    private MemoEdit memProxy;
    private LabelControl lbeUserAgentNotice;
    private RadioGroup raiTypeProxy;
    private MemoEdit txtDialUp;
    private SimpleButton btnHomepage;
    private RadioGroup radTypeIp;
    private ComboBoxEdit cbeGoogleSite;
    private SpinEdit speTimeout;
    private SpinEdit speEmailDelay;
    private SimpleButton btnDeleteHistory;
    private MemoEdit memEmail;
    private SimpleButton btnSave;
    private CheckEdit ceiNotViewImage;
    private CheckEdit ceiUseHistory;
    private SpinEdit speTimeViewTo;
    private SpinEdit speTimeViewSearch;
    private MemoEdit txtOtherSiteUrl;
    private SpinEdit speOtherSiteViewTime;
    private CheckEdit ceiViewOtherSite;
    private MemoEdit txtAgent;
    private SpinEdit speSoTrang;
    private SpinEdit speSumClick;
    private SpinEdit speTimeViewFrom;
    private SimpleButton btnStop;
    private SimpleButton btnRun;
    private LayoutControlGroup layoutControlGroup1;
    private LayoutControlGroup layoutControlGroup2;
    private EmptySpaceItem emptySpaceItem4;
    private LayoutControlItem layoutControlItem6;
    private LayoutControlItem layoutControlItem9;
    private LayoutControlItem layoutControlItem14;
    private LayoutControlItem layoutControlItem19;
    private LayoutControlItem layoutControlItem30;
    private LayoutControlItem layoutControlItem1;
    private TabbedControlGroup tabMain;
    private LayoutControlGroup lcgMain;
    private LayoutControlGroup layoutControlGroup8;
    private LayoutControlGroup lgTuKhoa;
    private LayoutControlItem layoutControlItem20;
    private LayoutControlItem layoutControlItem5;
    private LayoutControlItem layoutControlItem24;
    private LayoutControlItem layoutControlItem16;
    private LayoutControlItem layoutControlItem28;
    private EmptySpaceItem emptySpaceItem14;
    private LayoutControlGroup lcgHistory;
    private LayoutControlItem lciHistory;
    private SplitterItem splitterItem3;
    private LayoutControlGroup lcgTimeSetup;
    private LayoutControlGroup lgTraffic;
    private LayoutControlItem lciSuDung;
    private LayoutControlItem lciOtherSiteListUrl;
    private LayoutControlItem lciOtherSiteViewTime;
    private LayoutControlItem lciOtherSiteViewTimeTo;
    private LayoutControlGroup lcgSettings;
    private LayoutControlItem lciGoogleSite;
    private LayoutControlItem lciSoLanClick;
    private LayoutControlItem lciSoLuong;
    private LayoutControlItem lciDuyet;
    private LayoutControlGroup lcgOtherConfig;
    private LayoutControlItem lciUseHistory;
    private LayoutControlItem lciAutoStart;
    private LayoutControlItem lciTimeout;
    private LayoutControlItem lciLoadProfilePercent;
    private LayoutControlItem lciStarupWindow;
    private LayoutControlItem lciDisplayMode;
    private LayoutControlItem lciClearChrome;
    private LayoutControlItem layoutControlItem40;
    private LayoutControlItem lciBrowserLanguage;
    private LayoutControlGroup lcgTimeGoogle;
    private LayoutControlItem lciSpeedKeyboard;
    private LayoutControlItem lciThoiGianTK;
    private LayoutControlItem lciTimeViewSearchTo;
    private LayoutControlGroup lcgViewAds;
    private LayoutControlItem lciTimeViewFrom;
    private LayoutControlItem lciTimeViewTo;
    private LayoutControlGroup lcgTimeInternalExternal;
    private LayoutControlItem layoutControlItem63;
    private LayoutControlItem lciSubLinkTime;
    private LayoutControlItem lciSubLinkViewTo;
    private LayoutControlItem layoutControlItem39;
    private LayoutControlItem lciCallPhoneZalo;
    private LayoutControlItem lciViewYoutube;
    private LayoutControlItem lciNotViewImage;
    private EmptySpaceItem emptySpaceItem16;
    private LayoutControlGroup lcgLoginGmail;
    private LayoutControlItem lciGMail;
    private TabbedControlGroup tagGMail;
    private LayoutControlGroup lcgMail;
    private LayoutControlItem lciEmailDelay;
    private LayoutControlItem lciListEmail;
    private EmptySpaceItem emptySpaceItem8;
    private LayoutControlGroup lcgProfile;
    private LayoutControlItem lciListProfile;
    private EmptySpaceItem emptySpaceItem11;
    private LayoutControlItem layoutControlItem53;
    private LayoutControlItem layoutControlItem60;
    private LayoutControlGroup lcgUserAgent;
    private LayoutControlItem layoutControlItem13;
    private LayoutControlItem lbeNoticeAgent;
    private LayoutControlItem layoutControlItem38;
    private LayoutControlItem layoutControlItem62;
    private LayoutControlItem lciDeviceType;
    private LayoutControlGroup lcgFakeIP;
    private LayoutControlGroup lcgDcom;
    private TabbedControlGroup tabIP;
    private LayoutControlGroup lcgProxy;
    private TabbedControlGroup tabProxyMain;
    private LayoutControlGroup lcgOBCv2Proxy;
    private LayoutControlItem layoutControlItem49;
    private LayoutControlGroup layoutControlGroup9;
    private LayoutControlItem layoutControlItem50;
    private LayoutControlItem layoutControlItem45;
    private LayoutControlItem layoutControlItem54;
    private LayoutControlItem layoutControlItem65;
    private LayoutControlGroup lcgProxyFree;
    private LayoutControlGroup lcgFreeProxySub;
    private LayoutControlItem lcgAddonGetProxy;
    private SplitterItem splitterItem4;
    private LayoutControlItem layoutControlItem25;
    private LayoutControlGroup lcgFreeProxyConfig;
    private LayoutControlItem layoutControlItem2;
    private LayoutControlGroup lcgTinSoftProxy;
    private EmptySpaceItem emptySpaceItem3;
    private LayoutControlItem layoutControlItem11;
    private LayoutControlItem layoutControlItem37;
    private LayoutControlItem layoutControlItem52;
    private LayoutControlItem layoutControlItem47;
    private LayoutControlItem layoutControlItem55;
    private LayoutControlItem layoutControlItem3;
    private LayoutControlItem layoutControlItem7;
    private LayoutControlGroup lcgXProxy;
    private LayoutControlItem lciHostXProxy;
    private LayoutControlItem layoutControlItem26;
    private EmptySpaceItem emptySpaceItem7;
    private LayoutControlItem layoutControlItem27;
    private LayoutControlItem layoutControlItem36;
    private LayoutControlGroup lcgOBCProxy;
    private LayoutControlItem layoutControlItem18;
    private LayoutControlItem layoutControlItem21;
    private EmptySpaceItem emptySpaceItem10;
    private LayoutControlItem layoutControlItem22;
    private LayoutControlItem layoutControlItem35;
    private LayoutControlGroup lcgOBCv2Proxy_Old;
    private LayoutControlItem layoutControlItem43;
    private LayoutControlItem layoutControlItem44;
    private EmptySpaceItem emptySpaceItem13;
    private LayoutControlItem layoutControlItem46;
    private LayoutControlGroup lcgMultiProxy;
    private LayoutControlItem layoutControlItem29;
    private LayoutControlItem layoutControlItem31;
    private LayoutControlItem layoutControlItem32;
    private LayoutControlItem layoutControlItem33;
    private LayoutControlItem layoutControlItem34;
    private EmptySpaceItem emptySpaceItem9;
    private SplitterItem splitterItem1;
    private SplitterItem splitterItem2;
    private LayoutControlGroup lcgTMProxy;
    private LayoutControlItem layoutControlItem57;
    private LayoutControlItem layoutControlItem58;
    private EmptySpaceItem emptySpaceItem15;
    private LayoutControlItem lciProxySupplier;
    private LayoutControlGroup lcgListDcom;
    private LayoutControlGroup layoutControlGroup4;
    private LayoutControlItem lciDialUp;
    private EmptySpaceItem emptySpaceItem6;
    private LayoutControlItem layoutControlItem56;
    private LayoutControlItem lciResetDcomInterval;
    private LayoutControlItem lciDcomDelay;
    private LayoutControlItem layoutControlItem48;
    private LayoutControlItem lciChangeIp;
    private LayoutControlGroup lcgChangeMac;
    private LayoutControlItem lciChangeMac;
    private LayoutControlItem lciChangeMacTime;
    private LayoutControlItem layoutControlItem23;
    private EmptySpaceItem emptySpaceItem12;
    private LayoutControlItem layoutControlItem51;
    private LayoutControlGroup lcgReport;
    private LayoutControlItem layoutControlItem8;
    private EmptySpaceItem emptySpaceItem2;
    private LayoutControlItem layoutControlItem10;
    private LayoutControlGroup lciReportIp;
    private LayoutControlItem layoutControlItem17;
    private LayoutControlItem layoutControlItem15;
    private EmptySpaceItem emptySpaceItem5;
    private LayoutControlGroup lcgReportDomain;
    private LayoutControlItem layoutControlItem4;
    private LayoutControlItem layoutControlItem12;
    private EmptySpaceItem emptySpaceItem1;
    private SplitterItem splitterItem7;
    private LayoutControlItem lciSaveReport;
    private GridView gridView1;
    private GridView gridView2;
    private GridView gridView4;
    private GridView gridView5;
    private GridView gridView6;
    private GridView gridView7;
    private GridView gridView8;
    private GridView gridView9;
    private GridView gridView10;
    private GridView gridView11;
    private System.Windows.Forms.Timer tmCanhBao;

	[DllImport("User32.dll")]
	public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int SendMessage(IntPtr hwndControl, uint Msg, int wParam, StringBuilder strBuffer);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int SendMessage(IntPtr hwndControl, uint Msg, int wParam, int lParam);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

	public frmMain()
	{
		InitializeComponent();
		culture = CultureInfo.CurrentCulture;
	}

	private void DelApp()
	{
		try
		{
			int num = 0;
			while (!DongHetTrinhDuyet() && num < 200)
			{
				num++;
				Sleep(1000);
			}
			Process[] processes = Process.GetProcesses();
			Process[] array = processes;
			foreach (Process process in array)
			{
				if (process.ProcessName.ToLower().Equals("orbita", StringComparison.OrdinalIgnoreCase))
				{
					process.Kill();
				}
				if (process.ProcessName.ToLower().Equals("chrome", StringComparison.OrdinalIgnoreCase) && process.MainModule.FileName.ToLower().Contains("orbita-browser"))
				{
					process.Kill();
				}
				if (process.ProcessName.ToLower().Equals("chromedriver", StringComparison.OrdinalIgnoreCase))
				{
					process.Kill();
				}
				if (process.ProcessName.Equals("conhost", StringComparison.OrdinalIgnoreCase))
				{
					process.Kill();
				}
			}
		}
		catch (Exception ex)
		{
			isClearChrome = false;
			TimerCount = 0;
			LogHistory("Error: Xóa cache trình duyệt lỗi." + ex.Message);
		}
		finally
		{
			isClearChrome = false;
			TimerCount = 0;
			LogHistory("====> Xóa cache trình duyệt");
		}
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		try
		{
			tmCanhBao.Start();
			lcgChangeMac.Text = "Đổi MAC Address";
			if (e != null)
			{
				ReadConfig();
			}
			ApiClientNATech.ApiUrl = (string.IsNullOrEmpty(Settings.Default.NATechApiUrl) ? "https://api.na.com.vn" : Settings.Default.NATechApiUrl);
			ApiClientNATech.SoftId = SoftId;
			ApiClientNATech.LinkDownloadSoft = "https://na.com.vn/NATechSEOUpdate.zip";
			ApiClientNATech.SoftwareZipFileName = "NATechSEO.zip";
			LoginApi(isAutoLogin: true);
			ReadHistory();
			if (ceiAutoStart.Checked)
			{
				tmAutoStart.Interval = 5000;
				tmAutoStart.Start();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			Application.Exit();
		}
	}

	private void LoginApi(bool isAutoLogin)
	{
		frmLogin frmLogin = new frmLogin(isAutoLogin);
		frmLogin.LinkGUINewVersion = "https://na.com.vn/huong-dan-cap-nhat-phan-mem-soft-viet-seo-phien-ban-5-0-0-ban-dung-profile-gologin.html";
		frmLogin.ShowDialog(this);
		if (ApiClientNATech.loginResponse != null && !string.IsNullOrEmpty(ApiClientNATech.loginResponse.Token) && ApiClientNATech.loginResponse.DateTo.HasValue)
		{
            ApiClientNATech.loginResponse.Token = "expired";
            ApiClientNATech.loginResponse.UserId = 16819;

            ApiClientNATech.loginResponse.UserName = "danhlongit@gmail.com";
            ApiClientNATech.loginResponse.Email = "danhlongit@gmail.com";
            ApiClientNATech.loginResponse.Phone = "0977377083";
            ApiClientNATech.loginResponse.SoftId = 123;
            ApiClientNATech.loginResponse.DateFrom = DateTime.Now.AddYears(10);
            ApiClientNATech.loginResponse.DateTo = DateTime.Now;
            ApiClientNATech.loginResponse.SoftVersion = "5.5.2";
            ApiClientNATech.loginResponse.CurrentTime = DateTime.Now;
            ApiClientNATech.loginResponse.Licensed = true;
            if (ApiClientNATech.loginResponse.DateTo < ApiClientNATech.loginResponse.CurrentTime)
			{
				btnRun.Enabled = false;
				btnStop.Enabled = false;
				bbiCreateProfileMain.Enabled = false;
				bbiCreateProfile.Enabled = false;
				lcgChangeMac.Enabled = false;
				lcgChangeMac.Text = "Đổi MAC Address (Tính năng này chỉ sử dụng khi đã đăng ký bản quyền)";
				bsiUserInfo.Caption = ApiClientNATech.loginResponse.UserName + "(" + ApiClientNATech.loginResponse.Email + ")";
				Text = "NATECH SEO 5.5.2. " + GetLanguage("DesignBy") + " " + DesignBy + ". " + GetLanguage("LimitDate") + ": " + GetLanguage("Expired") + ".";
				bbiBuy_ItemClick(null, null);
			}
			else
			{
				btnRun.Enabled = true;
				btnStop.Enabled = true;
				bbiCreateProfileMain.Enabled = true;
				bbiCreateProfile.Enabled = true;
				lcgChangeMac.Enabled = ApiClientNATech.loginResponse.Licensed;
				if (!ApiClientNATech.loginResponse.Licensed)
				{
					lcgChangeMac.Text = "Đổi MAC Address (Tính năng này chỉ sử dụng khi đã đăng ký bản quyền)";
					lciSoLanClick.AppearanceItemCaption.ForeColor = Color.Red;
				}
				else
				{
					lciSoLanClick.AppearanceItemCaption.ForeColor = Color.Black;
					lcgChangeMac.Text = "Đổi MAC Address";
				}
				bsiUserInfo.Caption = ApiClientNATech.loginResponse.UserName + "(" + ApiClientNATech.loginResponse.Email + ")";
				string text = ((NATechProxy.Misc.OnlyDate(ApiClientNATech.loginResponse.DateTo.Value) == new DateTime(2100, 1, 1)) ? GetLanguage("CopyrightPermanently") : ApiClientNATech.loginResponse.DateTo.Value.ToString("dd/MM/yyyy"));
				Text = "NATECH SEO 5.5.2. " + GetLanguage("DesignBy") + " " + DesignBy + ". " + GetLanguage("LimitDate") + ": " + text + ".";
			}
			bbiLogin.Visibility = BarItemVisibility.Never;
			bbiChangePass.Visibility = BarItemVisibility.Always;
			bbiResgisterAccount.Visibility = BarItemVisibility.Never;
			bbiAcctiveAccount.Visibility = BarItemVisibility.Never;
			bbiLogout.Visibility = BarItemVisibility.Always;
			bbiCreateProfile.Visibility = BarItemVisibility.Always;
			if (string.IsNullOrEmpty(txtAgent.Text))
			{
				btnSyncUserAgent_Click(null, null);
			}
			if (!string.IsNullOrEmpty(ApiClientNATech.loginResponse.SoftVersion) && "5.5.2" != ApiClientNATech.loginResponse.SoftVersion)
			{
				frmUpdate frmUpdate = new frmUpdate();
				frmUpdate.CurrentVersion = "5.5.2";
				frmUpdate.ShowDialog(this);
				Application.Exit();
			}
		}
		else
		{
			bbiLogin.Visibility = BarItemVisibility.Always;
			bbiChangePass.Visibility = BarItemVisibility.Never;
			bbiResgisterAccount.Visibility = BarItemVisibility.Always;
			bbiAcctiveAccount.Visibility = BarItemVisibility.Always;
			bbiLogout.Visibility = BarItemVisibility.Never;
			bbiCreateProfile.Visibility = BarItemVisibility.Never;
			btnRun.Enabled = false;
			btnStop.Enabled = false;
			lcgChangeMac.Enabled = false;
			lcgChangeMac.Text = "Đổi MAC Address (Tính năng này chỉ sử dụng khi đã đăng ký bản quyền)";
			Text = "NATECH SEO 5.5.2. " + GetLanguage("DesignBy") + " " + DesignBy + ". " + GetLanguage("NoLogin") + ".";
		}
	}

	private void ReadConfig()
	{
		dicProxySupplier = bllProxyBll.GetProxySupplier();
		dtBrowserLanguage = bllProxyBll.GetBrowserLanguage();
		dtDeviceType = bllProxyBll.GetDeviceType();
		ccbBrowserLanguage.Properties.DataSource = dtBrowserLanguage;
		lueDeviceType.Properties.DataSource = dtDeviceType;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\NATechSEO");
		if (registryKey != null)
		{
			string text = NATechProxy.Misc.ToString(registryKey.GetValue("TrinhDuyet"));
			text = NATechProxy.Misc.ToString(registryKey.GetValue("DcomDiaup"));
			if (text.Length > 0)
			{
				txtDialUp.Text = text;
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("Language"));
			if (text.Length > 0)
			{
				cbeLang.EditValue = ((text == "en-US") ? "English" : "Tiếng Việt");
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("SoTrang"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speSoTrang.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("TimeViewSearch"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speTimeViewSearch.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("NumberOfClick"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speSumClick.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ViewPageFrom"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speTimeViewFrom.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ViewPageTo"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speTimeViewTo.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("NotViewImage"));
			ceiNotViewImage.Checked = text == "1";
			text = NATechProxy.Misc.ToString(registryKey.GetValue("AgentFile.txt"));
			if (!string.IsNullOrEmpty(text))
			{
				txtAgent.Text = text;
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("IsOtherSite"));
			if (text.Length > 0)
			{
				ceiViewOtherSite.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("OtherSiteUrl"));
			if (text.Length > 0)
			{
				txtOtherSiteUrl.Text = text;
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("OtherSiteViewTime"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speOtherSiteViewTime.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("Email"));
			if (text.Length > 0)
			{
				memEmail.Text = text;
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("EmailDelay"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speEmailDelay.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("Timeout"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speTimeout.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("GoogleUrl"));
			if (text.Length > 0)
			{
				cbeGoogleSite.EditValue = text;
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("TypeIp"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				radTypeIp.EditValue = Convert.ToInt32(text);
				radTypeIp_SelectedIndexChanged(null, null);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("Proxy"));
			memProxy.Text = text;
			text = NATechProxy.Misc.ToString(registryKey.GetValue("TypeProxy"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				raiTypeProxy.EditValue = Convert.ToInt32(text);
				raiTypeProxy_SelectedIndexChanged(null, null);
				if (NATechProxy.Misc.ObjInt(text) == 2)
				{
					btnLoadTinhTP_Click(null, null);
				}
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("DcomTypeReset"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				radDcomTypeReset.EditValue = Convert.ToInt32(text);
				radDcomTypeReset_SelectedIndexChanged(null, null);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ResetDcomInterval"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speResetDcomInterval.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("DcomDelay"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speDcomDelay.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("GMailType"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				radGMail.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ProfileConfig"));
			memProfile.Text = text;
			text = NATechProxy.Misc.ToString(registryKey.GetValue("SoLuong"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speSoLuong.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("SubLinkView"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speSubLinkView.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ChangeMACAddress"));
			if (text.Length > 0)
			{
				ceiChangeMACAddress.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("MACAddressInterval"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speMACAddressInterval.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("TimeViewSearchTo"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speTimeViewSearchTo.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("SubLinkViewTo"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speSubLinkViewTo.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("OtherSiteViewTimeTo"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speOtherSiteViewTimeTo.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("AutoStart"));
			if (text.Length > 0)
			{
				ceiAutoStart.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("SaveReport"));
			if (text.Length > 0)
			{
				ceiSaveReport.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ViewYoutube"));
			if (text.Length > 0)
			{
				ceiViewYoutube.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("LoadProfilePercent"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speLoadProfilePercent.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("InternalCount"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speInternalCount.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("SpeedKeyboard"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speSpeedKeyboard.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("DCOMProxyDelay"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speDCOMProxyDelay.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("StarupWindow"));
			if (text.Length > 0)
			{
				ceiStarupWindow.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("DisplayMode"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				lueDisplayMode.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ViewFilm"));
			if (text.Length > 0)
			{
				ceiViewFilm.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ClearChrome"));
			if (text.Length > 0)
			{
				ceiClearChrome.Checked = text == "1";
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("ClearChromeTime"));
			if (text.Length > 0 && NATechProxy.Misc.IsDigit(text))
			{
				speClearChromeTime.EditValue = Convert.ToInt32(text);
			}
			text = NATechProxy.Misc.ToString(registryKey.GetValue("LanguageBrowser"));
			if (!string.IsNullOrEmpty(text))
			{
				ccbBrowserLanguage.EditValue = text;
			}
			registryKey.Close();
		}
		if (!string.IsNullOrEmpty(Settings.Default.TypeProxy) && NATechProxy.Misc.IsDigit(Settings.Default.TypeProxy))
		{
			raiTypeProxy.EditValue = Convert.ToInt32(Settings.Default.TypeProxy);
			raiTypeProxy_SelectedIndexChanged(null, null);
			if (NATechProxy.Misc.ObjInt(raiTypeProxy.EditValue) == 2)
			{
				btnLoadTinhTP_Click(null, null);
			}
		}
		if (!string.IsNullOrEmpty(Settings.Default.XProxyHost))
		{
			txtXProxyHost.Text = Settings.Default.XProxyHost;
		}
		if (!string.IsNullOrEmpty(Settings.Default.OBCProxyHost))
		{
			txtOBCHost.Text = Settings.Default.OBCProxyHost;
		}
		if (!string.IsNullOrEmpty(Settings.Default.OBCv2ProxyHost))
		{
			txtOBCV2Host.Text = Settings.Default.OBCv2ProxyHost;
		}
		if (!string.IsNullOrEmpty(Settings.Default.OBCV3Host))
		{
			txtOBCV3Host.Text = Settings.Default.OBCV3Host;
		}
		if (!string.IsNullOrEmpty(Settings.Default.OBCV3Proxy))
		{
			memOBCV3Proxy.Text = Settings.Default.OBCV3Proxy;
		}
		if (!string.IsNullOrEmpty(Settings.Default.ProxySupplier))
		{
			cbeProxySupplier.EditValue = Settings.Default.ProxySupplier;
		}
		InitKeyword(ref dtKeywordBingding);
		grdKeyword.DataSource = dtKeywordBingding;
		bllMultiProxyBll.InitMultiProxyType(ref dtMultiProxyBinding);
		grdvlueMultiProxy_Type.DataSource = bllMultiProxyBll.GetProxyType();
		grdMultiProxy.DataSource = dtMultiProxyBinding;
		dtTinsoftType = new TinsoftProxy().GetProxyType();
		grdvlueTinsoft_Type.DataSource = dtTinsoftType;
		dtTinsoftTinhTP = new TinsoftProxy().getLocations();
		grdvccbTinsoft_TinhTP.DataSource = dtTinsoftTinhTP;
		new TinsoftProxy().InitData(ref dtTinsoft, TinsoftFileName);
		grdTinsoft.DataSource = dtTinsoft;
		dtOption = new bllConst().GetOption();
		grdvlueKeyword_Type.DataSource = dtOption;
		dtDisplayMode = new bllConst().GetDisplayMode();
		lueDisplayMode.Properties.DataSource = dtDisplayMode;
		dtTMProxyType = bllTMProxyBll.GetProxyType();
		grdvlueTMProxy_Type.DataSource = dtTMProxyType;
		dtTMProxyTinhTP = bllTMProxyBll.getLocations(out var _);
		grdvlueTMProxy_TinhTP.DataSource = dtTMProxyTinhTP;
		bllTMProxyBll.InitData(ref dtTMProxy, TMProxyFileName);
		grdTMProxy.DataSource = dtTMProxy;
	}

	private void InitKeyword(ref DataTable dtKeyword)
	{
		if (dtKeyword == null)
		{
			dtKeyword = new DataTable("Keyword");
		}
		if (File.Exists(KeywordFileName))
		{
			dtKeyword.ReadXml(KeywordFileName);
		}
		if (!dtKeyword.Columns.Contains("Key"))
		{
			dtKeyword.Columns.Add("Key", typeof(string));
		}
		if (!dtKeyword.Columns.Contains("Domain"))
		{
			dtKeyword.Columns.Add("Domain", typeof(string));
		}
		if (!dtKeyword.Columns.Contains("SubLink"))
		{
			dtKeyword.Columns.Add("SubLink", typeof(string));
		}
		if (!dtKeyword.Columns.Contains("Type"))
		{
			dtKeyword.Columns.Add("Type", typeof(int));
		}
		dtKeyword.Columns["Type"].DefaultValue = 1;
		foreach (DataRow row in dtKeyword.Rows)
		{
			if (NATechProxy.Misc.IsNullOrDbNull(row["Type"]))
			{
				if (NATechProxy.Misc.IsNullOrDbNull(row["Key"]))
				{
					row["Type"] = 3;
				}
				else
				{
					row["Type"] = 1;
				}
			}
		}
	}

	public void ChangeMACAddress()
	{
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class", writable: true);
		if (registryKey != null)
		{
			string[] subKeyNames = registryKey.GetSubKeyNames();
			string[] array = subKeyNames;
			foreach (string text in array)
			{
				if (!text.ToLower().StartsWith("{4d36e"))
				{
					continue;
				}
				RegistryKey registryKey2 = registryKey.OpenSubKey(text, writable: true);
				if (registryKey2 == null)
				{
					continue;
				}
				string[] subKeyNames2 = registryKey2.GetSubKeyNames();
				string[] array2 = subKeyNames2;
				foreach (string text2 in array2)
				{
					if (!(text2.ToLower() == "properties"))
					{
						if (registryKey2.OpenSubKey(text2, writable: true).GetValueNames().Contains("NetworkAddress"))
						{
							string arg = NATechProxy.Misc.ToString(registryKey2.OpenSubKey(text2, writable: true).GetValue("NetworkAddress"));
							string randomMacAddress = bllProxyBll.GetRandomMacAddress();
							LogHistory($"Change MAC Address: [{arg}] -> [{randomMacAddress}]");
							registryKey2.OpenSubKey(text2, writable: true).SetValue("NetworkAddress", randomMacAddress);
						}
						if (registryKey2.OpenSubKey(text2, writable: true).GetValueNames().Contains("OriginalNetworkAddress"))
						{
							string arg2 = NATechProxy.Misc.ToString(registryKey2.OpenSubKey(text2, writable: true).GetValue("OriginalNetworkAddress"));
							string randomMacAddress2 = bllProxyBll.GetRandomMacAddress();
							LogHistory($"Change MAC Address: [{arg2}] -> [{randomMacAddress2}]");
							registryKey2.OpenSubKey(text2, writable: true).SetValue("OriginalNetworkAddress", randomMacAddress2);
						}
						if (registryKey2.OpenSubKey(text2, writable: true).GetValueNames().Contains("Locally Administered Address"))
						{
							string arg3 = NATechProxy.Misc.ToString(registryKey2.OpenSubKey(text2, writable: true).GetValue("Locally Administered Address"));
							string randomMacAddress3 = bllProxyBll.GetRandomMacAddress();
							LogHistory($"Change MAC Address: [{arg3}] -> [{randomMacAddress3}]");
							registryKey2.OpenSubKey(text2, writable: true).SetValue("Locally Administered Address", randomMacAddress3);
						}
					}
				}
			}
		}
		registryKey.Close();
		bllProxyBll.ResetAdapter();
		Sleep(2000);
	}

	private string GetLanguage(string key)
	{
		culture = CultureInfo.CreateSpecificCulture(cultureName);
		ResourceManager resourceManager = new ResourceManager("NATech.Language.MyLang", typeof(frmMain).Assembly);
		return resourceManager.GetString(key, culture);
	}

	private void SetLanguage(string cultureName)
	{
		culture = CultureInfo.CreateSpecificCulture(cultureName);
		ResourceManager resourceManager = new ResourceManager("NATech.Language.MyLang", typeof(frmMain).Assembly);
		bsiHelp.Caption = resourceManager.GetString(bsiHelp.Name, culture);
		bbiRegister.Caption = resourceManager.GetString(bbiRegister.Name, culture);
		bbiAbout.Caption = resourceManager.GetString(bbiAbout.Name, culture);
		bsiAccount.Caption = resourceManager.GetString(bsiAccount.Name, culture);
		bbiGUI.Caption = resourceManager.GetString(bbiGUI.Name, culture);
		bsiGUI.Caption = resourceManager.GetString(bsiGUI.Name, culture);
		bbiVideoDemo.Caption = resourceManager.GetString(bbiVideoDemo.Name, culture);
		bbiUserAgent.Caption = resourceManager.GetString(bbiUserAgent.Name, culture);
		bbiUpdate.Caption = resourceManager.GetString(bbiUpdate.Name, culture);
		bbiNotification.Caption = resourceManager.GetString(bbiNotification.Name, culture);
		bbiClose.Caption = resourceManager.GetString(bbiClose.Name, culture);
		cbeLang.Caption = resourceManager.GetString(cbeLang.Name, culture);
		btnSave.Text = resourceManager.GetString(btnSave.Name, culture);
		btnStop.Text = resourceManager.GetString(btnStop.Name, culture);
		btnRun.Text = resourceManager.GetString(btnRun.Name, culture);
		btnDeleteHistory.Text = resourceManager.GetString(btnDeleteHistory.Name, culture);
		btnHomepage.Text = resourceManager.GetString(btnHomepage.Name, culture);
		bsiStatus.Caption = resourceManager.GetString(bsiStatus.Name);
		memProxyNote.Text = resourceManager.GetString(memProxyNote.Name);
		lbeNoticeAgent.Text = resourceManager.GetString(lbeNoticeAgent.Name);
		btnChangeMAC.Text = resourceManager.GetString(btnChangeMAC.Name, culture);
		btnConnectxProxy.Text = resourceManager.GetString(btnConnectxProxy.Name, culture);
		btnLoadHistory.Text = resourceManager.GetString(btnLoadHistory.Name, culture);
		btnDeleteHistoryXml.Text = resourceManager.GetString(btnDeleteHistoryXml.Name, culture);
		bbiCreateProfileMain.Caption = resourceManager.GetString(bbiCreateProfileMain.Name, culture);
		lbeUserAgentNotice.Text = resourceManager.GetString(lbeUserAgentNotice.Name, culture);
		bbiFirefoxProfile.Caption = resourceManager.GetString(bbiFirefoxProfile.Name, culture);
		bbiBuy.Caption = resourceManager.GetString(bbiBuy.Name, culture);
		bbiDcomSetting.Caption = resourceManager.GetString(bbiDcomSetting.Name, culture);
		bbiDisableIPv6.Caption = resourceManager.GetString(bbiDisableIPv6.Name, culture);
		lcgTimeSetup.Text = resourceManager.GetString(lcgTimeSetup.Name, culture);
		lbeFakeIpNoticMacAddress.Text = resourceManager.GetString(lbeFakeIpNoticMacAddress.Name, culture);
		radTypeIp.Properties.Items[1].Description = resourceManager.GetString("TypeIpDCOM");
		radTypeIp.Properties.Items[2].Description = resourceManager.GetString("TypeIpProxy");
		bsiFanpage.Caption = resourceManager.GetString(bsiFanpage.Name, culture);
		btnExcelDomain.Text = resourceManager.GetString(btnExcelDomain.Name, culture);
		btnExcelIp.Text = resourceManager.GetString(btnExcelDomain.Name, culture);
		memReChuot.Text = resourceManager.GetString(memReChuot.Name, culture);
		bbiRegisterTinsoft.Caption = resourceManager.GetString(bbiRegisterTinsoft.Name, culture);
		grdvbeiKeyword_Delete.Buttons[0].Caption = resourceManager.GetString("grdvbeiKeyword_Delete_Xoa", culture);
		grdvbeiKeyword_Delete.Buttons[1].Caption = resourceManager.GetString("grdvbeiKeyword_Delete_ChiTiet", culture);
		lbeNoticeFreeProxy.Text = resourceManager.GetString(lbeNoticeFreeProxy.Name, culture);
		foreach (object item in lcMain.Items)
		{
			if (item is LayoutControlItem)
			{
				LayoutControlItem layoutControlItem = (LayoutControlItem)item;
				layoutControlItem.Text = resourceManager.GetString(layoutControlItem.Name, culture);
			}
			else if (item is LayoutControlGroup)
			{
				LayoutControlGroup layoutControlGroup = (LayoutControlGroup)item;
				layoutControlGroup.Text = resourceManager.GetString(layoutControlGroup.Name, culture);
			}
		}
		foreach (object item2 in lcgTimeSetup.Items)
		{
			if (item2 is LayoutControlItem)
			{
				LayoutControlItem layoutControlItem2 = (LayoutControlItem)item2;
				layoutControlItem2.Text = resourceManager.GetString(layoutControlItem2.Name, culture);
			}
			else if (item2 is LayoutControlGroup)
			{
				LayoutControlGroup layoutControlGroup2 = (LayoutControlGroup)item2;
				layoutControlGroup2.Text = resourceManager.GetString(layoutControlGroup2.Name, culture);
			}
		}
		foreach (object item3 in lcgFakeIP.Items)
		{
			if (item3 is LayoutControlItem)
			{
				LayoutControlItem layoutControlItem3 = (LayoutControlItem)item3;
				layoutControlItem3.Text = resourceManager.GetString(layoutControlItem3.Name, culture);
			}
			else if (item3 is LayoutControlGroup)
			{
				LayoutControlGroup layoutControlGroup3 = (LayoutControlGroup)item3;
				layoutControlGroup3.Text = resourceManager.GetString(layoutControlGroup3.Name, culture);
			}
		}
		foreach (BarItemLink itemLink in bsiAccount.ItemLinks)
		{
			itemLink.Item.Caption = resourceManager.GetString(itemLink.Item.Name, culture);
		}
		foreach (GridColumn column in grdvHistory.Columns)
		{
			column.Caption = resourceManager.GetString(column.Name, culture);
		}
		foreach (GridColumn column2 in grdvIp.Columns)
		{
			column2.Caption = resourceManager.GetString(column2.Name, culture);
		}
	}

	private void frmRegisterRightFRM_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	public string GetFromUrl(string url)
	{
		string result = "";
		for (int i = 1; i <= 3; i++)
		{
			try
			{
				Encoding encoding = Encoding.GetEncoding("utf-8");
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				HttpWebResponse httpWebResponse = null;
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, encoding, detectEncodingFromByteOrderMarks: true);
				result = streamReader.ReadToEnd().Trim();
				streamReader.Close();
				responseStream.Close();
				return result;
			}
			catch (Exception)
			{
				if (i == 3)
				{
					throw;
				}
				Sleep(1000);
			}
		}
		return result;
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			TimerCount++;
			if (ClearChrome && ((TimerCount >= ClearChromeTime * 60) & !isClearChrome))
			{
				isClearChrome = true;
				Thread thread = new Thread((ThreadStart)delegate
				{
					DelApp();
				});
				thread.IsBackground = true;
				thread.Start();
			}
			bsiStatus.Caption = sTinhTrang;
			if (indexCurrent > 0)
			{
				SetTextProcess();
			}
			if (isRunFinish)
			{
				timer1.Stop();
				tmSaveHistory.Stop();
				btnRun.Enabled = true;
				btnStop.Enabled = true;
				btnLoadHistory.Enabled = true;
				bsiStatus.EditValue = 0;
				if (string.IsNullOrEmpty(sTinhTrang))
				{
					sTinhTrang = "Đã chạy xong";
				}
				DeleteProfileTemp();
				MessageBox.Show(this, sTinhTrang, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			if (isViewPage)
			{
				tempCountSecond++;
				if (tempCountSecond > timeViewAdsFrom)
				{
					tempCountSecond = 0;
					isViewPage = false;
				}
			}
			if (TatDcom)
			{
				TatDcom = false;
				if (bllProxyBll.IsConnect(lstDialUp))
				{
					LogHistory(GetLanguage("ShutdownDcom"));
					string text = bllProxyBll.RunCMD("Rasdial /disconnect", 3000);
					if (text.Contains("No connections"))
					{
						sTinhTrang = GetLanguage("NotFoundDcom") + text;
						LogHistory(sTinhTrang);
						btnStop_Click(null, null);
					}
					else if (!text.Contains("Command completed"))
					{
						sTinhTrang = GetLanguage("ShutdownDcomErr") + text;
						LogHistory(sTinhTrang);
					}
					else
					{
						LogHistory(GetLanguage("ShutdownDcomOk"));
						LogHistory(GetLanguage("RestartDcom"));
						BatDcom = true;
					}
				}
				else
				{
					LogHistory(GetLanguage("ShutdownDcomOk"));
					LogHistory(GetLanguage("RestartDcom"));
					BatDcom = true;
				}
			}
			if (!BatDcom)
			{
				return;
			}
			tempCountSecond++;
			if (tempCountSecond <= 1)
			{
				return;
			}
			BatDcom = false;
			tempCountSecond = 0;
			if (!bllProxyBll.IsConnect(lstDialUp))
			{
				LogHistory(GetLanguage("RestartingDcom"));
				string randomString = GetRandomString(lstDialUp);
				string text2 = bllProxyBll.RunCMD("Rasdial " + randomString, DcomDelay * 1000);
				if (text2.Contains("connected"))
				{
					LogHistory(string.Format("{1} [{0}] {2}", randomString, GetLanguage("OpenedDcom"), GetLanguage("SussessFull")));
					KhoiDongDcom = true;
				}
				else
				{
					sTinhTrang = string.Format("{1} {0}. {2}", text2, GetLanguage("OpenDcomErr"), GetLanguage("EndPrograme"));
					LogHistory(sTinhTrang);
					btnStop_Click(null, null);
				}
			}
			else
			{
				LogHistory("Connected!.");
				KhoiDongDcom = true;
			}
		}
		catch (Exception ex)
		{
			btnRun.Enabled = true;
			btnLoadHistory.Enabled = true;
			timer1.Stop();
			tmSaveHistory.Stop();
			MessageBox.Show(ex.ToString());
		}
	}

	private void ResetDcom()
	{
		if (TypeIp == 1)
		{
			while (!DongHetTrinhDuyet())
			{
				Sleep(300);
			}
			if (!TatDcom)
			{
				TatDcom = true;
			}
		}
		else
		{
			Sleep(timeRelaxPerRound * 1000);
			KhoiDongDcom = true;
		}
	}

	private void SetTextProcess()
	{
		bsiStatus.EditValue = indexCurrent;
		bsiStatus.Caption = string.Format("{3} {0}/{1}. {4}: {2}.", indexCurrent, maxClick, clickSuscess, GetLanguage("Progress"), GetLanguage("ClickSussess"));
		mmeHistory.Text = sHistory;
		if (!string.IsNullOrEmpty(sHistory) && sHistory.Length > 500000)
		{
			sHistory = string.Empty;
		}
	}

	private void frmSendMail_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void btnRun_Click(object sender, EventArgs e)
	{
		try
		{
			DeleteProfileTemp();
			if (NATechProxy.Misc.ObjInt(radTypeIp.EditValue) == 1 && string.IsNullOrEmpty(txtDialUp.Text))
			{
				MessageBox.Show(GetLanguage("ChuaNhapDialUp"), GetLanguage("Notice"));
				txtDialUp.Focus();
				return;
			}
			TimerCount = 0;
			tempCountSecond = 0;
			btnSaveConfig_Click(null, null);
			if (dtKeywordBingding.Rows.Count == 0)
			{
				MessageBox.Show(GetLanguage("ChuaNhapKey"), GetLanguage("Notice"));
				return;
			}
			ReadParam();
			indexCurrent = -1;
			clickSuscess = 0;
			bsiStatus.Caption = GetLanguage("bsiStatus");
			bsieStatus.Maximum = maxClick;
			btnRun.Enabled = false;
			btnStop.Enabled = true;
			btnLoadHistory.Enabled = false;
			dicBrowserStop = new Dictionary<string, bool>();
			dicDriverRec = new DriverRec().SplitScreen(DisplayMode, SoLuong, 1, TypeIp, TypeProxy);
			switch (TypeIp)
			{
			case 0:
			{
				int i;
				for (i = 0; i < SoLuong; i++)
				{
					Thread thread10 = new Thread((ThreadStart)delegate
					{
						Run(i);
					});
					thread10.IsBackground = true;
					thread10.Start();
					Sleep(500);
				}
				break;
			}
			case 1:
			{
				int j;
				for (j = 0; j < SoLuong; j++)
				{
					Thread thread9 = new Thread((ThreadStart)delegate
					{
						Run(j);
					});
					thread9.IsBackground = true;
					thread9.Start();
					Sleep(500);
				}
				break;
			}
			case 2:
				switch (TypeProxy)
				{
				case 1:
				{
					int i2;
					for (i2 = 0; i2 < SoLuong; i2++)
					{
						Thread thread3 = new Thread((ThreadStart)delegate
						{
							Run(i2);
						});
						thread3.IsBackground = true;
						thread3.Start();
						Sleep(500);
					}
					break;
				}
				case 2:
				{
					int soProxy = NATechProxy.Misc.ObjInt(dtTinsoft.Compute("count(Key)", "Select=1"));
					dicDriverRec = new DriverRec().SplitScreen(DisplayMode, SoLuong, soProxy, TypeIp, TypeProxy);
					if (TinsoftData == null)
					{
						TinsoftData = new Dictionary<string, TinsoftProxy.TinsoftData>();
					}
					for (int num4 = 0; num4 < dtTinsoft.Rows.Count; num4++)
					{
						if (!NATechProxy.Misc.ObjBol(dtTinsoft.Rows[num4]["Select"]))
						{
							continue;
						}
						string imei = NATechProxy.Misc.ToString(dtTinsoft.Rows[num4]["Key"]);
						if (!TinsoftData.ContainsKey(imei))
						{
							TinsoftProxy.TinsoftData tinsoftData = new TinsoftProxy.TinsoftData();
							tinsoftData.DangLay = false;
							tinsoftData.LastChange = DateTime.Now.AddDays(-1.0);
							tinsoftData.ThoiGianCho = new TinsoftProxy().LayThoiGianCho(NATechProxy.Misc.ObjInt(dtTinsoft.Rows[num4]["Type"]));
							TinsoftData.Add(imei, tinsoftData);
						}
						List<int> lstTinhTP = new bllProxy().LayTinhTP(NATechProxy.Misc.ToString(dtTinsoft.Rows[num4]["TinhTP"]));
						int j2;
						for (j2 = 0; j2 < SoLuong; j2++)
						{
							Thread thread8 = new Thread((ThreadStart)delegate
							{
								Run(null, string.Empty, imei, lstTinhTP, j2);
							});
							thread8.IsBackground = true;
							thread8.Start();
							Sleep(1000);
						}
					}
					break;
				}
				case 3:
				{
					string empty = string.Empty;
					if (dtXProxyBinding == null || dtXProxyBinding.Rows.Count == 0)
					{
						btnConnectxProxy_Click(null, null);
					}
					grdvXProxyList.CloseEditor();
					BindingContext[dtXProxyBinding].EndCurrentEdit();
					dtXProxy = dtXProxyBinding.Copy();
					int num2 = NATechProxy.Misc.ObjInt(dtXProxy.Compute("count(imei)", "IsRun=1"));
					speSoLuong.EditValue = num2;
					dicDriverRec = new DriverRec().SplitScreen(DisplayMode, 1, num2, TypeIp, TypeProxy);
					for (int n = 0; n < dtXProxy.Rows.Count; n++)
					{
						if (NATechProxy.Misc.ObjBol(dtXProxy.Rows[n]["IsRun"]))
						{
							string imei5 = NATechProxy.Misc.ToString(dtXProxy.Rows[n]["imei"]);
							string ServiceUrl4 = NATechProxy.Misc.ToString(dtXProxy.Rows[n]["ServiceUrl"]);
							Thread thread4 = new Thread((ThreadStart)delegate
							{
								Run(null, ServiceUrl4, imei5, null, n);
							});
							thread4.IsBackground = true;
							thread4.Start();
							Sleep(5000);
						}
					}
					break;
				}
				case 4:
				{
					string empty3 = string.Empty;
					if (dtOBCProxyBinding == null || dtOBCProxyBinding.Rows.Count == 0)
					{
						btnConnectOBC_Click(null, null);
					}
					grdvOBC.CloseEditor();
					BindingContext[dtOBCProxyBinding].EndCurrentEdit();
					dtOBCProxy = dtOBCProxyBinding.Copy();
					int num3 = NATechProxy.Misc.ObjInt(dtOBCProxy.Compute("count(idKey)", "IsRun=1"));
					speSoLuong.EditValue = num3;
					dicDriverRec = new DriverRec().SplitScreen(DisplayMode, 1, num3, TypeIp, TypeProxy);
					for (int k = 0; k < dtOBCProxy.Rows.Count; k++)
					{
						if (NATechProxy.Misc.ObjBol(dtOBCProxy.Rows[k]["IsRun"]))
						{
							string imei2 = NATechProxy.Misc.ToString(dtOBCProxy.Rows[k]["idKey"]);
							string ServiceUrl = NATechProxy.Misc.ToString(dtOBCProxy.Rows[k]["ServiceUrl"]);
							Thread thread7 = new Thread((ThreadStart)delegate
							{
								Run(null, ServiceUrl, imei2, null, k);
							});
							thread7.IsBackground = true;
							thread7.Start();
							Sleep(5000);
						}
					}
					break;
				}
				case 5:
				{
					List<string> listFromString = GetListFromString(memOBCV3Proxy.Text);
					bllProxyBll.InitCircuitProxyTable(ref dtMachProxy);
					dtMachProxy.Rows.Clear();
					foreach (string item in listFromString)
					{
						List<string> list = NATechProxy.Misc.SplitToList(item, ":");
						if (list.Count > 1)
						{
							DataRow dataRow = dtMachProxy.NewRow();
							dataRow["ip"] = list[0];
							dataRow["port"] = list[1];
							dataRow["fullProxy"] = $"{list[0]}:{list[1]}";
							dataRow["ServiceUrl"] = txtOBCV3Host.Text;
							if (list.Count > 3)
							{
								dataRow["user"] = list[2];
								dataRow["pass"] = list[3];
							}
							dtMachProxy.Rows.Add(dataRow);
						}
					}
					dicDriverRec = new DriverRec().SplitScreen(DisplayMode, 1, dtMachProxy.Rows.Count, TypeIp, TypeProxy);
					for (int i3 = 0; i3 < dtMachProxy.Rows.Count; i3++)
					{
						string imei6 = NATechProxy.Misc.ToString(dtMachProxy.Rows[i3]["fullProxy"]);
						string ServiceUrl5 = NATechProxy.Misc.ToString(dtMachProxy.Rows[i3]["ServiceUrl"]);
						Thread thread2 = new Thread((ThreadStart)delegate
						{
							Run(null, ServiceUrl5, imei6, null, i3);
						});
						thread2.IsBackground = true;
						thread2.Start();
						Sleep(5000);
					}
					break;
				}
				case 6:
				{
					string empty2 = string.Empty;
					if (dtMultiOBCProxyBinding == null && dtMultiXProxyBinding == null)
					{
						btnMultiProxyConnect_Click(null, null);
					}
					SoLuong = 0;
					if (dtMultiXProxyBinding != null)
					{
						grdvMultiXProxy.CloseEditor();
						BindingContext[dtMultiXProxyBinding].EndCurrentEdit();
						dtMultiXProxy = dtMultiXProxyBinding.Copy();
						SoLuong = NATechProxy.Misc.ObjInt(dtMultiXProxy.Compute("count(imei)", "IsRun=1"));
						dicDriverRec = new DriverRec().SplitScreen(DisplayMode, 1, SoLuong, TypeIp, TypeProxy);
						for (int m = 0; m < dtMultiXProxy.Rows.Count; m++)
						{
							if (NATechProxy.Misc.ObjBol(dtMultiXProxy.Rows[m]["IsRun"]))
							{
								string imei4 = NATechProxy.Misc.ToString(dtMultiXProxy.Rows[m]["imei"]);
								string ServiceUrl3 = NATechProxy.Misc.ToString(dtMultiXProxy.Rows[m]["ServiceUrl"]);
								Thread thread5 = new Thread((ThreadStart)delegate
								{
									Run(3, ServiceUrl3, imei4, null, m);
								});
								thread5.IsBackground = true;
								thread5.Start();
								Sleep(5000);
							}
						}
					}
					if (dtMultiOBCProxyBinding != null)
					{
						grdvMultiOBC.CloseEditor();
						BindingContext[dtMultiOBCProxyBinding].EndCurrentEdit();
						dtMultiOBCProxy = dtMultiOBCProxyBinding.Copy();
						SoLuong += NATechProxy.Misc.ObjInt(dtMultiOBCProxy.Compute("count(idKey)", "IsRun=1"));
						dicDriverRec = new DriverRec().SplitScreen(DisplayMode, 1, SoLuong, TypeIp, TypeProxy);
						for (int l = 0; l < dtMultiOBCProxy.Rows.Count; l++)
						{
							if (NATechProxy.Misc.ObjBol(dtMultiOBCProxy.Rows[l]["IsRun"]))
							{
								string imei3 = NATechProxy.Misc.ToString(dtMultiOBCProxy.Rows[l]["idKey"]);
								string ServiceUrl2 = NATechProxy.Misc.ToString(dtMultiOBCProxy.Rows[l]["ServiceUrl"]);
								Thread thread6 = new Thread((ThreadStart)delegate
								{
									Run(4, ServiceUrl2, imei3, null, l);
								});
								thread6.IsBackground = true;
								thread6.Start();
								Sleep(5000);
							}
						}
					}
					speSoLuong.EditValue = SoLuong;
					break;
				}
				case 7:
				{
					if (TMProxyData == null)
					{
						TMProxyData = new Dictionary<string, bllTMProxy.TMProxyData>();
					}
					TMProxyData.Clear();
					dicDriverRec = new DriverRec().SplitScreen(DisplayMode, SoLuong, dtTMProxy.Rows.Count, TypeIp, TypeProxy);
					for (int num = 0; num < dtTMProxy.Rows.Count; num++)
					{
						string imei7 = NATechProxy.Misc.ToString(dtTMProxy.Rows[num]["Key"]);
						if (!TMProxyData.ContainsKey(imei7))
						{
							bllTMProxy.TMProxyData tMProxyData = new bllTMProxy.TMProxyData();
							tMProxyData.DangLay = false;
							tMProxyData.LastChange = DateTime.Now.AddDays(-1.0);
							tMProxyData.ThoiGianCho = bllTMProxyBll.LayThoiGianCho(NATechProxy.Misc.ObjInt(dtTMProxy.Rows[num]["Type"]));
							TMProxyData.Add(imei7, tMProxyData);
						}
						List<int> lstTinhTP2 = new bllProxy().LayTinhTP(NATechProxy.Misc.ToString(dtTMProxy.Rows[num]["TinhTP"]));
						int j3;
						for (j3 = 0; j3 < SoLuong; j3++)
						{
							Thread thread = new Thread((ThreadStart)delegate
							{
								Run(null, string.Empty, imei7, lstTinhTP2, j3);
							});
							thread.IsBackground = true;
							thread.Start();
							Sleep(1000);
						}
					}
					break;
				}
				}
				break;
			}
			timer1.Start();
			if (IsChangeMacAddress)
			{
				tmChangeMAC.Interval = MacAddressInterval * 60 * 1000;
				tmChangeMAC.Start();
			}
			if (DcomTypeReset == 2)
			{
				tmResetDcom.Interval = ResetDcomInterval * 60 * 1000;
				tmResetDcom.Start();
			}
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
			MessageBox.Show(this, ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private List<string> GetListFromString(string text)
	{
		List<string> list = NATechProxy.Misc.SplitToList(text, "\r\n");
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (string.IsNullOrEmpty(list[num]))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	private List<string> GetListDomain(string[] domains, List<string> lstDomainException)
	{
		List<string> list = new List<string>();
		foreach (string text in domains)
		{
			if (string.IsNullOrEmpty(text) || text.Length < 4 || list.Contains(text.ToLower()))
			{
				continue;
			}
			bool flag = false;
			foreach (string item in lstDomainException)
			{
				if (item.Contains(text.ToLower()))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(text.ToLower());
			}
		}
		return list;
	}

	private List<int> GetTinhTP(string v)
	{
		List<string> list = NATechProxy.Misc.SplitToList(v.Replace(" ", ""), ",");
		List<int> list2 = new List<int>();
		foreach (string item in list)
		{
			list2.Add(NATechProxy.Misc.ObjInt(item));
		}
		return list2;
	}

	private void btnStop_Click(object sender, EventArgs e)
	{
		try
		{
			if (sender != null)
			{
				sTinhTrang = "Kết thúc chương trình.";
			}
			isRunFinish = true;
			SaveHistoryToXml();
			tmChangeMAC.Stop();
			Sleep(3000);
			btnRun.Enabled = true;
			btnLoadHistory.Enabled = true;
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void DeleteProfileTemp()
	{
		LogHistory("Đang xóa các Profile ẩn danh cũ...");
		Sleep(1100);
		string[] directories = Directory.GetDirectories(Path.GetTempPath(), "NATech_*");
		string[] array = directories;
		foreach (string path in array)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (directoryInfo != null)
			{
				try
				{
					Directory.Delete(directoryInfo.FullName, recursive: true);
				}
				catch (Exception ex)
				{
					string message = ex.Message;
				}
			}
		}
	}

	private void Run(int LuongThu)
	{
		Run(null, string.Empty, null, null, LuongThu);
	}

	private void Run(int? MultiType, string ServiceUrl, string imei, List<int> lstTinhTP, int LuongThu)
	{
		Run(MultiType, ServiceUrl, imei, lstTinhTP, LuongThu, string.Empty);
	}

	private void Run(int? MultiType, string ServiceUrl, string imei, List<int> lstTinhTP, int LuongThu, string IDBrowser)
	{
		if (string.IsNullOrEmpty(IDBrowser))
		{
			IDBrowser = "NATech_" + NATechProxy.Misc.GenID(6);
		}
		ChromeDriver driver = null;
		try
		{
			LogHistory(GetLanguage("btnRun"));
			if (!dicBrowserStop.ContainsKey(IDBrowser))
			{
				dicBrowserStop.Add(IDBrowser, value: false);
			}
			for (int i = 0; i < maxClick; i++)
			{
				while (DangChoResetDcom)
				{
					Sleep(300);
				}
				if (ChangeMacAddressFlag)
				{
					LogHistory($"Lệnh đổi MAC Address đang thực thi.");
				}
				if (ClearChrome)
				{
					while (isClearChrome)
					{
						Sleep(800);
						if (!dicBrowserStop[IDBrowser])
						{
							dicBrowserStop[IDBrowser] = true;
						}
					}
				}
				DongTrinhDuyet(ref driver, IDBrowser);
				InitDriverChrome(ref driver, MultiType, ServiceUrl, imei, lstTinhTP, ref IDBrowser, LuongThu);
				if (!dicBrowserStop.ContainsKey(IDBrowser))
				{
					dicBrowserStop.Add(IDBrowser, value: false);
				}
				dicBrowserStop[IDBrowser] = false;
				if (isRunFinish)
				{
					DongTrinhDuyet(ref driver, IDBrowser);
					break;
				}
				try
				{
					int TrangThu = 1;
					indexCurrent = i + 1;
					int Lan = 0;
					if (TypeIp == 1 || TypeIp == 0)
					{
						string iP = GetIP(ref Lan);
						LogHistory(string.Format("{2} {0} Ip [{1}]", indexCurrent, iP, GetLanguage("Step")));
						AddIp(iP);
					}
					string domain = string.Empty;
					int type = 1;
					List<string> lstSubLink = new List<string>();
					string keyword = GetKeyword(ref domain, ref lstSubLink, ref type);
					bool flag = false;
					switch (type)
					{
					case 1:
					{
						try
						{
							driver.Navigate().GoToUrl(url);
						}
						catch (Exception ex3)
						{
							if (NATechProxy.Misc.ToString(ex3.Message).Contains("Reached error page"))
							{
								LogHistory(GetLanguage("NoConnect"));
							}
							else
							{
								LogHistory(ex3.Message);
							}
							DongTrinhDuyet(ref driver, IDBrowser);
							if (isRunFinish)
							{
								return;
							}
							goto end_IL_0161;
						}
						IWebElement webElement = driver.FindElement(By.Name("q"));
						if (webElement == null)
						{
							break;
						}
						webElement.Click();
						SendKeys(webElement, keyword);
						webElement.Submit();
						LogHistory(string.Format("{1} [{0}]", keyword, GetLanguage("DangTim")));
						isViewPage = true;
						LogHistory(GetLanguage("DangCuon"));
						DuyetTrangTimKiem(driver, TimeViewSearch, TimeViewSearchTo);
						if (IsLocation)
						{
							try
							{
								IWebElement webElement2 = driver.FindElement(By.TagName("update-location"));
								try
								{
									webElement2.Click();
								}
								catch (Exception)
								{
									Actions actions = new Actions(driver);
									actions.MoveToElement(webElement2);
									actions.Click();
								}
								LogHistory(GetLanguage("ChoViTri"));
								LogHistory(GetLanguage("DaCapNhatViTri"));
							}
							catch (Exception ex5)
							{
								LogHistory(string.Format("{1}. {0}", ex5.Message, GetLanguage("KoTimThayViTri")));
							}
						}
						try
						{
							Clicks(ref TrangThu, driver, domain, lstSubLink, type);
						}
						catch (Exception ex6)
						{
							LogHistory("Error: " + ex6.ToString());
						}
						break;
					}
					case 2:
					{
						try
						{
							driver.Navigate().GoToUrl(urlvideo);
						}
						catch (Exception ex14)
						{
							if (NATechProxy.Misc.ToString(ex14.Message).Contains("Reached error page"))
							{
								LogHistory(GetLanguage("NoConnect"));
							}
							else
							{
								LogHistory(ex14.Message);
							}
							DongTrinhDuyet(ref driver, IDBrowser);
							if (isRunFinish)
							{
								return;
							}
							goto end_IL_0161;
						}
						IWebElement webElement6 = driver.FindElement(By.Name("q"));
						if (webElement6 == null)
						{
							break;
						}
						webElement6.Click();
						SendKeys(webElement6, keyword);
						webElement6.Submit();
						LogHistory(string.Format("{1} [{0}]", keyword, GetLanguage("DangTim")));
						isViewPage = true;
						LogHistory(GetLanguage("DangCuon"));
						DuyetTrangTimKiem(driver, TimeViewSearch, TimeViewSearchTo);
						if (IsLocation)
						{
							try
							{
								IWebElement webElement7 = driver.FindElement(By.TagName("update-location"));
								try
								{
									webElement7.Click();
								}
								catch (Exception)
								{
									Actions actions2 = new Actions(driver);
									actions2.MoveToElement(webElement7);
									actions2.Click();
								}
								LogHistory(GetLanguage("ChoViTri"));
								LogHistory(GetLanguage("DaCapNhatViTri"));
							}
							catch (Exception ex16)
							{
								LogHistory(string.Format("{1}. {0}", ex16.Message, GetLanguage("KoTimThayViTri")));
							}
						}
						try
						{
							Clicks(ref TrangThu, driver, domain, lstSubLink, type);
						}
						catch (Exception ex17)
						{
							LogHistory("Error: " + ex17.ToString());
						}
						break;
					}
					case 3:
					case 6:
						DirectTraffic(driver, domain, lstSubLink, type);
						break;
					case 4:
					case 5:
					case 7:
					{
						try
						{
							driver.Navigate().GoToUrl(url_youtube);
						}
						catch (Exception ex7)
						{
							if (NATechProxy.Misc.ToString(ex7.Message).Contains("Reached error page"))
							{
								LogHistory(GetLanguage("NoConnect"));
							}
							else
							{
								LogHistory(ex7.Message);
							}
							DongTrinhDuyet(ref driver, IDBrowser);
							if (isRunFinish)
							{
								return;
							}
							goto end_IL_0161;
						}
						IWebElement webElement3 = null;
						try
						{
							webElement3 = driver.FindElement(By.Id("search-input"));
						}
						catch (Exception)
						{
						}
						if (webElement3 != null)
						{
							webElement3.Click();
							IWebElement webElement4 = webElement3.FindElement(By.Id("search"));
							SendKeys(webElement4, keyword);
							webElement4.Submit();
							try
							{
								switch (type)
								{
								case 5:
									driver.Navigate().GoToUrl("https://www.youtube.com/results?search_query=" + keyword + "&sp=EgJAAQ%253D%253D");
									break;
								case 7:
									driver.Navigate().GoToUrl("https://www.youtube.com/results?search_query=" + keyword + "&sp=CAI%253D");
									break;
								}
							}
							catch (Exception ex9)
							{
								string message = ex9.Message;
							}
							LogHistory(string.Format("{1} [{0}]", keyword, GetLanguage("DangTim")));
							isViewPage = true;
							LogHistory(GetLanguage("DangCuon"));
							DuyetTrangTimKiem(driver, TimeViewSearch, TimeViewSearchTo);
							try
							{
								ClicksYoutube(ref TrangThu, driver, domain, lstSubLink, type);
							}
							catch (Exception ex10)
							{
								LogHistory("Error: " + ex10.ToString());
							}
							break;
						}
						try
						{
							webElement3 = driver.FindElement(By.XPath("//*[@id=\"header-bar\"]/header/div/button"));
							webElement3.Click();
							Sleep(200);
							IWebElement webElement5 = driver.FindElement(By.Name("search"));
							SendKeys(webElement5, keyword);
							webElement5.Submit();
							try
							{
								switch (type)
								{
								case 5:
									driver.Navigate().GoToUrl("https://www.youtube.com/results?search_query=" + keyword + "&sp=EgJAAQ%253D%253D");
									break;
								case 7:
									driver.Navigate().GoToUrl("https://www.youtube.com/results?search_query=" + keyword + "&sp=CAI%253D");
									break;
								}
							}
							catch (Exception ex11)
							{
								string message2 = ex11.Message;
							}
							LogHistory(string.Format("{1} [{0}]", keyword, GetLanguage("DangTim")));
							isViewPage = true;
							LogHistory(GetLanguage("DangCuon"));
							DuyetTrangTimKiem(driver, TimeViewSearch, TimeViewSearchTo);
							try
							{
								ClicksYoutube(ref TrangThu, driver, domain, lstSubLink, type);
							}
							catch (Exception ex12)
							{
								LogHistory("Error: " + ex12.ToString());
							}
						}
						catch (Exception ex13)
						{
							string message3 = ex13.Message;
						}
						if (webElement3 == null)
						{
						}
						break;
					}
					case 8:
						try
						{
							driver.Navigate().GoToUrl(keyword);
						}
						catch (Exception ex)
						{
							if (NATechProxy.Misc.ToString(ex.Message).Contains("Reached error page"))
							{
								LogHistory(GetLanguage("NoConnect"));
							}
							else
							{
								LogHistory(ex.Message);
							}
							DongTrinhDuyet(ref driver, IDBrowser);
							if (isRunFinish)
							{
								return;
							}
							goto end_IL_0161;
						}
						isViewPage = true;
						LogHistory(GetLanguage("DangCuon"));
						DuyetTrangTimKiem(driver, TimeViewSearch, TimeViewSearchTo);
						try
						{
							ClicksYoutube(ref TrangThu, driver, domain, lstSubLink, type);
						}
						catch (Exception ex2)
						{
							LogHistory("Error: " + ex2.ToString());
						}
						break;
					}
					DongTrinhDuyet(ref driver, IDBrowser);
					if (isRunFinish)
					{
						return;
					}
					dicBrowserStop[IDBrowser] = true;
					if (TypeIp != 1 || DcomTypeReset != 2)
					{
						KhoiDongDcom = false;
						ResetDcom();
						while (!KhoiDongDcom)
						{
							Sleep(1000);
						}
					}
					end_IL_0161:;
				}
				catch (Exception ex18)
				{
					LogHistory(string.Format("{0}. {1}.", ex18.ToString(), GetLanguage("ErrorClick")));
					DongTrinhDuyet(ref driver, IDBrowser);
					if (isRunFinish)
					{
						return;
					}
					dicBrowserStop[IDBrowser] = true;
				}
			}
			DongTrinhDuyet(ref driver, IDBrowser);
			tmChangeMAC.Stop();
		}
		catch (Exception ex19)
		{
			if (!NATechProxy.Misc.ToString(ex19.Message).ToLower().Contains("aborted"))
			{
				LogHistory("Người dùng bấm Kết thúc.");
			}
			DongTrinhDuyet(ref driver, IDBrowser);
			Run(MultiType, ServiceUrl, imei, lstTinhTP, LuongThu, IDBrowser);
		}
	}

	private void SendKeys(IWebElement element, string keySearch)
	{
		if (element != null && !string.IsNullOrEmpty(keySearch))
		{
			for (int i = 0; i < keySearch.Length; i++)
			{
				element.SendKeys(NATechProxy.Misc.ToString(keySearch[i]));
				Sleep(SpeedKeyboard);
			}
		}
	}

	private void DuyetTrangTimKiem(ChromeDriver driver, int timeViewSearch, int timeViewSearchTo)
	{
		int num = r.Next(timeViewSearch, timeViewSearchTo);
		int i = 0;
		int num2 = 0;
		for (; i < num; i++)
		{
			if (isRunFinish)
			{
				break;
			}
			num2 += r.Next(40, 500);
			driver.ExecuteScript($"window.scrollTo(0, {num2});");
			int interval = r.Next(500, 1500);
			Sleep(interval);
		}
		driver.ExecuteScript($"window.scrollTo(0, 0);");
	}

	private void DuyetTrangTimKiem(ref int t, ChromeDriver driver, int timeViewSearch, int timeViewSearchTo)
	{
		int num = r.Next(timeViewSearch, timeViewSearchTo);
		for (int i = 0; i < num; i++)
		{
			if (isRunFinish)
			{
				break;
			}
			t += r.Next(40, 500);
			driver.ExecuteScript($"window.scrollTo(0, {t});");
			int interval = r.Next(500, 1500);
			Sleep(interval);
		}
		driver.ExecuteScript($"window.scrollTo(0, 0);");
	}

	private bool CheckLicenseLocal()
	{
		return ApiClientNATech.loginResponse != null && !string.IsNullOrEmpty(ApiClientNATech.loginResponse.Token);
	}

	private string GetEmail(List<string> lstEmail, ref int emailIndex)
	{
		if (lstEmail == null || lstEmail.Count == 0)
		{
			return string.Empty;
		}
		if (lstEmail.Count <= emailIndex)
		{
			emailIndex = 0;
		}
		string result = lstEmail[emailIndex];
		emailIndex++;
		return result;
	}

	private string GetRandomString(List<string> lstKeyword)
	{
		int index = r.Next(0, lstKeyword.Count);
		return string.IsNullOrEmpty(lstKeyword[index]) ? GetRandomString(lstKeyword) : lstKeyword[index];
	}

	private string GetKeyword(ref string domain, ref List<string> lstSubLink, ref int type)
	{
		int index = r.Next(0, dtKeywordRuning.Rows.Count);
		string result = NATechProxy.Misc.ToString(dtKeywordRuning.Rows[index]["Key"]);
		domain = NATechProxy.Misc.ToString(dtKeywordRuning.Rows[index]["Domain"]);
		type = NATechProxy.Misc.ObjInt(dtKeywordRuning.Rows[index]["Type"]);
		if (type == 0)
		{
			type = 1;
		}
		string text = NATechProxy.Misc.ToString(dtKeywordRuning.Rows[index]["SubLink"]);
		if (!string.IsNullOrEmpty(text))
		{
			lstSubLink = GetListFromString(text);
		}
		return result;
	}

	private int GetRandomValue(List<int> lstKeyword)
	{
		if (lstKeyword == null || lstKeyword.Count == 0)
		{
			return 0;
		}
		int index = r.Next(0, lstKeyword.Count);
		return lstKeyword[index];
	}

	private void ReadParam()
	{
		url = NATechProxy.Misc.ToString(cbeGoogleSite.EditValue);
		isRunFinish = false;
		lstAgent = NATechProxy.Misc.SplitToList(txtAgent.Text, "\r\n");
		sTinhTrang = "";
		TatDcom = (BatDcom = (KhoiDongDcom = false));
		UseHistory = ceiUseHistory.Checked;
		lstDialUp = NATechProxy.Misc.SplitToList(txtDialUp.Text, "\r\n");
		viewOtherSite = ceiViewOtherSite.Checked;
		lstOtherSiteUrl = NATechProxy.Misc.SplitToList(txtOtherSiteUrl.Text, "\r\n");
		NotViewImage = ceiNotViewImage.Checked;
		lstEmail = NATechProxy.Misc.SplitToList(memEmail.Text, "\r\n");
		EmailDelay = NATechProxy.Misc.ObjInt(speEmailDelay.EditValue);
		EmailIndex = 0;
		lstProxy = bllProxyBll.GetFreeProxy(memProxy.Text);
		maxClick = NATechProxy.Misc.ObjInt(speSumClick.EditValue);
		timeRelaxPerRound = 1;
		timeViewAdsFrom = NATechProxy.Misc.ObjInt(speTimeViewFrom.EditValue);
		timeViewAdsTo = NATechProxy.Misc.ObjInt(speTimeViewTo.EditValue);
		TimeViewSearch = NATechProxy.Misc.ObjInt(speTimeViewSearch.EditValue);
		TimeViewSearchTo = NATechProxy.Misc.ObjInt(speTimeViewSearchTo.EditValue);
		SubLinkView = NATechProxy.Misc.ObjInt(speSubLinkView.Value);
		SubLinkViewTo = NATechProxy.Misc.ObjInt(speSubLinkViewTo.EditValue);
		OtherSiteViewTime = NATechProxy.Misc.ObjInt(speOtherSiteViewTime.EditValue);
		OtherSiteViewTimeTo = NATechProxy.Misc.ObjInt(speOtherSiteViewTimeTo.EditValue);
		Timeout = NATechProxy.Misc.ObjInt(speTimeout.EditValue);
		TypeIp = NATechProxy.Misc.ObjInt(radTypeIp.EditValue);
		TypeProxy = NATechProxy.Misc.ObjInt(raiTypeProxy.EditValue);
		GMailType = NATechProxy.Misc.ObjInt(radGMail.EditValue);
		lstProfile = GetListFromString(memProfile.Text);
		SoLuong = NATechProxy.Misc.ObjInt(speSoLuong.EditValue);
		SoTrang = NATechProxy.Misc.ObjInt(speSoTrang.Value);
		IsChangeMacAddress = ceiChangeMACAddress.Checked;
		MacAddressInterval = NATechProxy.Misc.ObjInt(speMACAddressInterval.Value);
		XProxyHost = txtXProxyHost.Text.Trim().TrimEnd('/');
		OBCProxyHost = txtOBCHost.Text.Trim().TrimEnd('/');
		OBCv2Host = txtOBCV2Host.Text.Trim().TrimEnd('/');
		dtKeywordRuning = dtKeywordBingding.Copy();
		SaveReport = ceiSaveReport.Checked;
		DcomTypeReset = NATechProxy.Misc.ObjInt(radDcomTypeReset.EditValue);
		ResetDcomInterval = NATechProxy.Misc.ObjInt(speResetDcomInterval.EditValue);
		DcomDelay = NATechProxy.Misc.ObjInt(speDcomDelay.EditValue);
		SupplierName = NATechProxy.Misc.ToString(cbeProxySupplier.EditValue).Trim();
		ViewYoutube = ceiViewYoutube.Checked;
		LoadProfilePercent = NATechProxy.Misc.ObjInt(speLoadProfilePercent.EditValue);
		InternalCount = NATechProxy.Misc.ObjInt(speInternalCount.EditValue);
		SpeedKeyboard = NATechProxy.Misc.ObjInt(speSpeedKeyboard.EditValue);
		DCOMProxyDelay = NATechProxy.Misc.ObjInt(speDCOMProxyDelay.EditValue);
		DisplayMode = NATechProxy.Misc.ObjInt(lueDisplayMode.EditValue);
		ClearChrome = ceiClearChrome.Checked;
		ClearChromeTime = NATechProxy.Misc.ObjInt(speClearChromeTime.EditValue);
		CallPhoneZalo = ceiCallPhoneZalo.Checked;
		BrowserLanguage = bllProxyBll.GetValueBrowserLanguage(dtBrowserLanguage, NATechProxy.Misc.ToString(ccbBrowserLanguage.EditValue));
		btnOBCV2HomePage.Text = $"Trang chủ {dicProxySupplier[SupplierName].Name}";
		btnOBCV2HomePage.Tag = dicProxySupplier[SupplierName].HomePage;
		dtOldProfile = new DataTable();
		if (File.Exists(Directory.GetCurrentDirectory() + "\\Settings\\Profile\\Manual\\DataSource.xml"))
		{
			dtOldProfile.ReadXml(Directory.GetCurrentDirectory() + "\\Settings\\Profile\\Manual\\DataSource.xml");
		}
		if (maxClick <= 0)
		{
			speSumClick.Focus();
			MessageBox.Show("Số lần lặp lại ít nhất 1 lần.");
		}
	}

	private void Sleep(int Interval)
	{
		Thread.Sleep(Interval);
	}

	private bool IsScroll(int type)
	{
		return type != 2 && type != 4 && type != 5 && type != 6 && type != 7;
	}

	private void Clicks(ref int PageIndex, ChromeDriver driver, string domain_run, List<string> lstSubLink, int type)
	{
		bool flag = false;
		try
		{
			int timeViewAds = r.Next(timeViewAdsFrom, timeViewAdsTo);
			IWebElement webElement = null;
			try
			{
				webElement = driver.FindElement(By.Id("rso"));
				List<IWebElement> list = webElement.FindElements(By.XPath(".//a[contains(@href, \"" + domain_run + "\")]")).ToList();
				if (list.Count > 0)
				{
					IWebElement webElement2 = list[0];
					flag = true;
					clickSuscess++;
					try
					{
						LogHistory(string.Format("{1} [{0}].", domain_run, GetLanguage("DaTimThayQC")));
						try
						{
							Actions actions = new Actions(driver);
							actions.MoveToElement(webElement2);
							actions.Perform();
						}
						catch (Exception ex)
						{
							string message = ex.Message;
						}
						try
						{
							webElement2.SendKeys(OpenQA.Selenium.Keys.Return);
						}
						catch (Exception)
						{
							webElement2.Click();
						}
						if ((type == 2 || type == 3 || type == 4) && ViewYoutube)
						{
							Sleep(3000);
							Actions actions2 = new Actions(driver);
							actions2.SendKeys(OpenQA.Selenium.Keys.Space).Build().Perform();
						}
						LogHistory(string.Format("{1} [{0}]", domain_run, GetLanguage("DaClick")));
						AddHistory(domain_run);
					}
					catch (Exception ex3)
					{
						if (NATechProxy.Misc.ToString(ex3.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory(string.Format("{1} [{0}]", domain_run, GetLanguage("DaClick")));
						}
						else
						{
							NgoaiLe(driver);
						}
					}
					try
					{
						DuyetTrang(driver, timeViewAds, IsScroll(type));
					}
					catch (Exception ex4)
					{
						if (NATechProxy.Misc.ToString(ex4.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
							return;
						}
						if (NATechProxy.Misc.ToString(ex4.Message).ToLower().Contains("thread was being aborted"))
						{
							LogHistory(GetLanguage("End"));
						}
						else
						{
							LogHistory($"Error: {ex4.Message}");
						}
					}
					bool coClick = false;
					ClickSubLink(driver, ref coClick, lstSubLink);
					try
					{
						ClickAnyLink(driver);
					}
					catch (Exception ex5)
					{
						if (NATechProxy.Misc.ToString(ex5.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
							return;
						}
						LogHistory($"Error: {ex5.Message}");
					}
					try
					{
						ClickPhoneOrZalo(driver);
					}
					catch (Exception ex6)
					{
						if (NATechProxy.Misc.ToString(ex6.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
							return;
						}
						LogHistory($"Error: {ex6.Message}");
					}
					try
					{
						ViewOtherSite(driver);
					}
					catch (Exception ex7)
					{
						if (NATechProxy.Misc.ToString(ex7.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
							return;
						}
						LogHistory($"Error: {ex7.Message}");
					}
				}
			}
			catch (Exception ex8)
			{
				string message2 = ex8.Message;
			}
			if (webElement == null)
			{
				if (!flag)
				{
					try
					{
						IWebElement webElement3 = null;
						try
						{
							webElement3 = driver.FindElement(By.Id("main"));
						}
						catch (Exception)
						{
						}
						if (webElement3 == null)
						{
							try
							{
								webElement3 = driver.FindElement(By.Id("search"));
							}
							catch (Exception)
							{
								LogHistory("Không tìm thấy div [main] or [search] giao diện Android");
							}
						}
						if (webElement3 != null)
						{
							IList<IWebElement> list2 = webElement3.FindElements(By.XPath("./div[not(@class='uEierd')]"));
							if (list2 != null)
							{
								foreach (IWebElement item in list2)
								{
									string attribute = item.GetAttribute("outerHTML");
									if (string.IsNullOrEmpty(attribute) || attribute.Contains("www.google.com/aclk?") || attribute.Contains("www.google.com.vn/aclk?") || !attribute.Contains(domain_run))
									{
										continue;
									}
									IWebElement webElement4 = item.FindElement(By.TagName("a"));
									if (webElement4 == null)
									{
										continue;
									}
									flag = true;
									clickSuscess++;
									try
									{
										LogHistory(string.Format("{1} [{0}].", domain_run, GetLanguage("DaTimThayQC")));
										try
										{
											Actions actions3 = new Actions(driver);
											actions3.MoveToElement(webElement4);
											actions3.Perform();
										}
										catch (Exception ex11)
										{
											string message3 = ex11.Message;
										}
										try
										{
											webElement4.SendKeys(OpenQA.Selenium.Keys.Return);
										}
										catch (Exception)
										{
											webElement4.Click();
										}
										if ((type == 2 || type == 3 || type == 4) && ViewYoutube)
										{
											Sleep(3000);
											Actions actions4 = new Actions(driver);
											actions4.SendKeys(OpenQA.Selenium.Keys.Space).Build().Perform();
										}
										LogHistory(string.Format("{1} [{0}]", domain_run, GetLanguage("DaClick")));
										AddHistory(domain_run);
									}
									catch (Exception)
									{
										NgoaiLe(driver);
									}
									DuyetTrang(driver, timeViewAds, IsScroll(type));
									bool coClick2 = false;
									try
									{
										ClickSubLink(driver, ref coClick2, lstSubLink);
									}
									catch (Exception ex14)
									{
										if (NATechProxy.Misc.ToString(ex14.Message).Contains("The HTTP request to the remote WebDriver"))
										{
											LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
											return;
										}
										LogHistory($"Error: {ex14.Message}");
									}
									try
									{
										ClickAnyLink(driver);
									}
									catch (Exception ex15)
									{
										if (NATechProxy.Misc.ToString(ex15.Message).Contains("The HTTP request to the remote WebDriver"))
										{
											LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
											return;
										}
										LogHistory($"Error: {ex15.Message}");
									}
									try
									{
										ClickPhoneOrZalo(driver);
									}
									catch (Exception ex16)
									{
										if (NATechProxy.Misc.ToString(ex16.Message).Contains("The HTTP request to the remote WebDriver"))
										{
											LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
											return;
										}
										LogHistory($"Error: {ex16.Message}");
									}
									try
									{
										ViewOtherSite(driver);
									}
									catch (Exception ex17)
									{
										if (NATechProxy.Misc.ToString(ex17.Message).Contains("The HTTP request to the remote WebDriver"))
										{
											LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
											return;
										}
										LogHistory($"Error: {ex17.Message}");
									}
									break;
								}
							}
						}
					}
					catch (Exception ex18)
					{
						if (!NATechProxy.Misc.ToString(ex18.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory($"Error: {ex18.Message}");
						}
					}
				}
				if (!flag)
				{
					LogHistory(string.Format("{2} [{0}] {3} {1}", domain_run, PageIndex, GetLanguage("NotFoundAds"), GetLanguage("AtPage")));
				}
			}
			if (!flag && PageIndex < SoTrang)
			{
				PageIndex++;
				ClickNextPage(driver, ref PageIndex, domain_run, lstSubLink, type);
			}
			if (!flag)
			{
				LogHistory(string.Format("{1} [{0}].", domain_run, GetLanguage("NotFoundAds")));
			}
		}
		catch (Exception ex19)
		{
			LogHistory(string.Format("{2} {0} {1}.", PageIndex, GetLanguage("NoLinkAds"), GetLanguage("Page")));
			if (!flag && PageIndex < SoTrang)
			{
				PageIndex++;
				ClickNextPage(driver, ref PageIndex, domain_run, lstSubLink, type);
			}
			LogHistory($"Error: {ex19.Message}");
		}
	}

	private void ClicksYoutube(ref int TrangThu, ChromeDriver driver, string domain_run, List<string> lstSubLink, int type)
	{
		bool flag = false;
		try
		{
			int timeViewAds = r.Next(timeViewAdsFrom, timeViewAdsTo);
			List<IWebElement> list = driver.FindElements(By.XPath(".//a[contains(@href, \"" + domain_run + "\")]")).ToList();
			if (list.Count > 0)
			{
				IWebElement webElement = list[0];
				flag = true;
				clickSuscess++;
				try
				{
					LogHistory("Đã tìm thấy video Youtube [" + domain_run + "].");
					try
					{
						Actions actions = new Actions(driver);
						actions.MoveToElement(webElement);
						actions.Perform();
					}
					catch (Exception ex)
					{
						string message = ex.Message;
					}
					try
					{
						webElement.SendKeys(OpenQA.Selenium.Keys.Return);
					}
					catch (Exception)
					{
						webElement.Click();
					}
					if ((type == 2 || type == 3 || type == 4 || type == 8) && ViewYoutube)
					{
						Sleep(3000);
						Actions actions2 = new Actions(driver);
						actions2.SendKeys(OpenQA.Selenium.Keys.Space).Build().Perform();
					}
					LogHistory(string.Format("{1} [{0}]", domain_run, GetLanguage("DaClick")));
					AddHistory(domain_run);
					try
					{
						DuyetTrang(driver, timeViewAds, IsScroll(type));
					}
					catch (Exception ex3)
					{
						if (NATechProxy.Misc.ToString(ex3.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
							return;
						}
						if (NATechProxy.Misc.ToString(ex3.Message).ToLower().Contains("thread was being aborted"))
						{
							LogHistory(GetLanguage("End"));
						}
						else
						{
							LogHistory($"Error: {ex3.Message}");
						}
					}
					bool coClick = false;
					ClickSubLink(driver, ref coClick, lstSubLink);
					try
					{
						ViewOtherSite(driver);
						return;
					}
					catch (Exception ex4)
					{
						if (NATechProxy.Misc.ToString(ex4.Message).Contains("The HTTP request to the remote WebDriver"))
						{
							LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
						}
						else
						{
							LogHistory($"Error: {ex4.Message}");
						}
						return;
					}
				}
				catch (Exception ex5)
				{
					if (NATechProxy.Misc.ToString(ex5.Message).Contains("The HTTP request to the remote WebDriver"))
					{
						LogHistory(string.Format("{1} [{0}]", domain_run, GetLanguage("DaClick")));
					}
					else
					{
						NgoaiLe(driver);
					}
					return;
				}
			}
			if (!flag && TrangThu < SoTrang)
			{
				TrangThu++;
				ClickNextPageYoutube(driver, ref TrangThu, domain_run, lstSubLink, type);
			}
			if (!flag)
			{
				LogHistory(string.Format("{1} [{0}].", domain_run, GetLanguage("NotFoundAds")));
			}
		}
		catch (Exception ex6)
		{
			LogHistory(string.Format("{2} {0} {1}.", TrangThu, GetLanguage("NoLinkAds"), GetLanguage("Page")));
			if (!flag && TrangThu < SoTrang)
			{
				TrangThu++;
				ClickNextPageYoutube(driver, ref TrangThu, domain_run, lstSubLink, type);
			}
			LogHistory($"Error: {ex6.Message}");
		}
	}

	private void DirectTraffic(ChromeDriver driver, string domain_run, List<string> lstSubLink, int type)
	{
		try
		{
			int timeViewAds = r.Next(timeViewAdsFrom, timeViewAdsTo);
			try
			{
				driver.Navigate().GoToUrl(domain_run);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
			}
			LogHistory(string.Format("{1} [{0}]", domain_run, GetLanguage("DaDuyet")));
			AddHistory(domain_run);
			try
			{
				DuyetTrang(driver, timeViewAds, IsScroll(type));
			}
			catch (Exception ex2)
			{
				if (NATechProxy.Misc.ToString(ex2.Message).Contains("The HTTP request to the remote WebDriver"))
				{
					LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
					return;
				}
				if (NATechProxy.Misc.ToString(ex2.Message).ToLower().Contains("thread was being aborted"))
				{
					LogHistory(GetLanguage("End"));
				}
				else
				{
					LogHistory($"Error: {ex2.Message}");
				}
			}
			bool coClick = false;
			try
			{
				ClickSubLink(driver, ref coClick, lstSubLink);
			}
			catch (Exception ex3)
			{
				if (NATechProxy.Misc.ToString(ex3.Message).Contains("The HTTP request to the remote WebDriver"))
				{
					LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
					return;
				}
				LogHistory($"Error: {ex3.Message}");
			}
			try
			{
				ClickAnyLink(driver);
			}
			catch (Exception ex4)
			{
				if (NATechProxy.Misc.ToString(ex4.Message).Contains("The HTTP request to the remote WebDriver"))
				{
					LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
					return;
				}
				LogHistory($"Error: {ex4.Message}");
			}
			try
			{
				ClickPhoneOrZalo(driver);
			}
			catch (Exception ex5)
			{
				if (NATechProxy.Misc.ToString(ex5.Message).Contains("The HTTP request to the remote WebDriver"))
				{
					LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
					return;
				}
				LogHistory($"Error: {ex5.Message}");
			}
			try
			{
				ViewOtherSite(driver);
			}
			catch (Exception ex6)
			{
				if (NATechProxy.Misc.ToString(ex6.Message).Contains("The HTTP request to the remote WebDriver"))
				{
					LogHistory(string.Format("{1} [{0}s]. {2}", Timeout, GetLanguage("TaiTrangQua"), GetLanguage("NextStep")));
				}
				else
				{
					LogHistory($"Error: {ex6.Message}");
				}
			}
		}
		catch (Exception ex7)
		{
			LogHistory($"Error: {ex7.Message}");
		}
	}

	private void ClickNextPage(ChromeDriver driver, ref int TrangThu, string domain_run, List<string> lstSubLink, int type)
	{
		try
		{
			IWebElement webElement = null;
			try
			{
				webElement = driver.FindElement(By.Id("pnnext"));
			}
			catch (Exception)
			{
			}
			if (webElement == null)
			{
				IWebElement webElement2 = FindElementByTagName(driver, "footer");
				if (webElement2 != null)
				{
					IList<IWebElement> list = webElement2.FindElements(By.TagName("a"));
					foreach (IWebElement item in list)
					{
						if (item.GetAttribute("href") != null && item.GetAttribute("href").Contains("/search?q="))
						{
							webElement = item;
						}
					}
				}
			}
			if (webElement == null)
			{
				List<IWebElement> list2 = driver.FindElements(By.XPath("//a[contains(@href,'search?q=')]")).ToList();
				if (list2 != null && list2.Count > 0)
				{
					IWebElement webElement3 = list2.Find((IWebElement c) => c.Text == "Xem thêm" || c.Text == "See more" || c.GetAttribute("aria-label") == "Trang tiếp theo" || c.GetAttribute("aria-label") == "Next page");
					if (webElement3 != null)
					{
						webElement = webElement3;
					}
				}
			}
			if (webElement == null)
			{
				try
				{
					IWebElement webElement4 = driver.FindElement(By.XPath("/html/body/div[3]/div/div[4]/div/div[4]/div/div[2]/div[4]/a[1]"));
					if (webElement4 != null)
					{
						webElement = webElement4;
					}
				}
				catch (Exception)
				{
				}
			}
			if (webElement != null)
			{
				LogHistory(string.Format("{1} {0}.", TrangThu, GetLanguage("NextPage")));
				webElement.Click();
				DuyetTrangTimKiem(driver, TimeViewSearch, TimeViewSearchTo);
				Clicks(ref TrangThu, driver, domain_run, lstSubLink, type);
			}
		}
		catch (Exception ex3)
		{
			string message = ex3.Message;
		}
	}

	private void ClickNextPageYoutube(ChromeDriver driver, ref int TrangThu, string domain_run, List<string> lstSubLink, int type)
	{
		LogHistory(string.Format("{1} {0}.", TrangThu, GetLanguage("NextPage")));
		int t = TrangThu * 1000;
		driver.ExecuteScript($"window.scrollTo(0, {t});");
		for (int i = 0; i < 5; i++)
		{
			t += r.Next(5, 100);
			driver.ExecuteScript($"window.scrollTo(0, {t});");
			int interval = r.Next(500, 1200);
			Sleep(interval);
		}
		DuyetTrangTimKiem(ref t, driver, TimeViewSearch, TimeViewSearchTo);
		ClicksYoutube(ref TrangThu, driver, domain_run, lstSubLink, type);
	}

	private void AddHistory(string domain_run)
	{
		try
		{
			if (SaveReport)
			{
				DateTime dateTime = NATechProxy.Misc.OnlyDate(DateTime.Now);
				DataRow[] array = dtHistory.Select($"Domain='{domain_run}' and Ngay='{dateTime}'");
				if (array != null && array.Length != 0)
				{
					array[0]["Click"] = NATechProxy.Misc.ObjInt(array[0]["Click"]) + 1;
					return;
				}
				DataRow dataRow = dtHistory.NewRow();
				dataRow["Ngay"] = dateTime;
				dataRow["Click"] = 1;
				dataRow["Domain"] = domain_run;
				dtHistory.Rows.Add(dataRow);
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private void AddIp(string FullIp)
	{
		try
		{
			if (SaveReport)
			{
				DateTime dateTime = NATechProxy.Misc.OnlyDate(DateTime.Now);
				DataRow[] array = dtIp.Select($"IP='{FullIp}' and Ngay='{dateTime}'");
				if (array != null && array.Length != 0)
				{
					array[0]["Click"] = NATechProxy.Misc.ObjInt(array[0]["Click"]) + 1;
					return;
				}
				DataRow dataRow = dtIp.NewRow();
				dataRow["Ngay"] = dateTime;
				dataRow["Click"] = 1;
				dataRow["IP"] = FullIp;
				dtIp.Rows.Add(dataRow);
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private void ViewOtherSite(ChromeDriver driver)
	{
	}

	private void NgoaiLe(ChromeDriver driver)
	{
		try
		{
			IWebElement webElement = driver.FindElement(By.Id("advancedButton"));
			IWebElement webElement2 = driver.FindElement(By.Id("exceptionDialogButton"));
			if (webElement != null && webElement2 != null)
			{
				webElement.Click();
				Sleep(500);
				webElement2.Click();
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private void DismissPopup(ChromeDriver driver)
	{
		if (driver == null)
		{
			return;
		}
		try
		{
			driver.SwitchTo().Alert()?.Dismiss();
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private void DuyetTrang(ChromeDriver driver, int timeViewAds, bool isScroll)
	{
		LogHistory(string.Format("{1} {0}s ", timeViewAds, GetLanguage("DuyetWeb")));
		isViewPage = true;
		int num = 0;
		int i = 0;
		if (!DuyetLink())
		{
			return;
		}
		Actions actions = new Actions(driver);
		DismissPopup(driver);
		if (!isScroll)
		{
			int num2 = r.Next(30, 200);
			driver.ExecuteScript($"window.scrollTo(0, {num2});");
			Sleep(1000);
			driver.ExecuteScript($"window.scrollTo(0, {-num2});");
		}
		if (ViewYoutube)
		{
			actions.SendKeys(OpenQA.Selenium.Keys.Space).Build().Perform();
		}
		for (; i < timeViewAds; i++)
		{
			if (isRunFinish)
			{
				break;
			}
			if (isScroll)
			{
				num += r.Next(10, 100);
				driver.ExecuteScript($"window.scrollTo(0, {num});");
			}
			else if (r.Next(1, 20) == 10)
			{
				int num3 = r.Next(50, 200);
				driver.ExecuteScript($"window.scrollTo(0, {num3});");
				Sleep(1000);
				int num4 = num3 + r.Next(50, 200);
				driver.ExecuteScript($"window.scrollTo(0, {num4});");
				Sleep(1000);
				driver.ExecuteScript($"window.scrollTo(0, {-num3});");
				Sleep(1000);
				driver.ExecuteScript($"window.scrollTo(0, {-num4});");
			}
			int interval = r.Next(500, 1200);
			Sleep(interval);
			DismissPopup(driver);
		}
	}

	private void ClickAnyLink(ChromeDriver driver)
	{
		try
		{
			Uri uri = new Uri(driver.Url);
			for (int i = 0; i < InternalCount; i++)
			{
				List<IWebElement> list = driver.FindElements(By.TagName("li")).ToList();
				List<IWebElement> list2 = driver.FindElements(By.XPath($".//a[contains(@href,'{uri.Host}')]")).ToList();
				list.AddRange(list2.ToArray());
				if (list.Count <= 0)
				{
					continue;
				}
				int index = r.Next(0, list.Count);
				IWebElement webElement = list[index];
				try
				{
					Actions actions = new Actions(driver);
					actions.MoveToElement(webElement);
					actions.Perform();
				}
				catch (Exception ex)
				{
					string message = ex.Message;
				}
				try
				{
					webElement.Click();
				}
				catch (Exception)
				{
					webElement.SendKeys(OpenQA.Selenium.Keys.Return);
				}
				try
				{
					IReadOnlyCollection<string> windowHandles = driver.WindowHandles;
					driver.SwitchTo().Window(windowHandles.Last());
				}
				catch (Exception)
				{
				}
				int j = 0;
				int num = 0;
				int num2 = r.Next(SubLinkView, SubLinkViewTo);
				LogHistory(string.Format("{0}. Thời gian duyệt {1}s", GetLanguage("ClickAny"), num2));
				for (; j < num2; j++)
				{
					if (isRunFinish)
					{
						break;
					}
					num += r.Next(5, 100);
					driver.ExecuteScript($"window.scrollTo(0, {num});");
					int interval = r.Next(500, 1200);
					Sleep(interval);
				}
			}
		}
		catch (Exception ex4)
		{
			string message2 = ex4.Message;
		}
	}

	private void ClickSubLink(ChromeDriver driver, ref bool coClick, List<string> lstSubLink)
	{
		if (!DuyetLink() || lstSubLink == null || lstSubLink.Count == 0)
		{
			return;
		}
		foreach (string item in lstSubLink)
		{
			try
			{
				bool flag = item.Contains("youtu.be") || item.Contains("youtube.com");
				IWebElement webElement = null;
				try
				{
					webElement = driver.FindElement(By.XPath(".//a[contains(@href,'" + item + "') or contains(text(),'" + item + "')]"));
				}
				catch (Exception)
				{
					webElement = driver.FindElement(By.XPath(".//a[contains(text(),'" + item + "')]"));
				}
				if (webElement == null)
				{
					continue;
				}
				try
				{
					Actions actions = new Actions(driver);
					actions.MoveToElement(webElement);
					actions.Perform();
				}
				catch (Exception ex2)
				{
					string message = ex2.Message;
				}
				try
				{
					webElement.Click();
				}
				catch (Exception ex3)
				{
					string message2 = ex3.Message;
					webElement.SendKeys(OpenQA.Selenium.Keys.Return);
				}
				try
				{
					IReadOnlyCollection<string> windowHandles = driver.WindowHandles;
					driver.SwitchTo().Window(windowHandles.Last());
				}
				catch (Exception)
				{
				}
				int num = r.Next(SubLinkView, SubLinkViewTo);
				LogHistory($"Click External Link {item} - Time {num}s");
				int i = 0;
				int num2 = 0;
				for (; i < num; i++)
				{
					if (isRunFinish)
					{
						break;
					}
					num2 += r.Next(5, 100);
					if (!flag)
					{
						driver.ExecuteScript($"window.scrollTo(0, {num2});");
					}
					int interval = r.Next(700, 1200);
					Sleep(interval);
				}
				coClick = true;
			}
			catch (Exception ex5)
			{
				string message3 = ex5.Message;
			}
		}
	}

	private void ClickPhoneOrZalo(ChromeDriver driver)
	{
		try
		{
			if (!CallPhoneZalo)
			{
				return;
			}
			List<IWebElement> list = driver.FindElements(By.XPath(".//a[starts-with(@href,'www.google.com/aclk?') or contains(@href,'zalo.me')]")).ToList();
			if (!DuyetLink() || list.Count <= 0)
			{
				return;
			}
			IWebElement webElement = null;
			int index = r.Next(0, list.Count);
			webElement = list[index];
			try
			{
				webElement.Click();
			}
			catch (Exception)
			{
				webElement.SendKeys(OpenQA.Selenium.Keys.Return);
			}
			LogHistory(string.Format("{0}. Thời gian duyệt {1}s", GetLanguage("ClickPhone"), SubLinkView));
			int i = 0;
			int num = 0;
			for (; i < SubLinkView; i++)
			{
				if (isRunFinish)
				{
					break;
				}
				num += r.Next(-30, 200);
				driver.ExecuteScript($"window.scrollTo(0, {num});");
				int interval = r.Next(700, 1200);
				Sleep(interval);
			}
		}
		catch (Exception ex2)
		{
			string message = ex2.Message;
		}
	}

	private int ScrollDown(ChromeDriver driver, int solan)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < solan; i++)
		{
			num3 += r.Next(200, 700);
			driver.ExecuteScript($"window.scrollTo(0, {num3});");
			num2 = r.Next(2, 7);
			Thread.Sleep(num2 * 1000);
			num += num2 * 1000;
		}
		return num;
	}

	private void InitDriverFireFox(ref FirefoxDriver driver)
	{
		InitDriverFireFox(ref driver, null, string.Empty, null, null);
	}

	private void InitDriverFireFox(ref FirefoxDriver driver, int? MultiType, string ServiceUrl, string keyProxy, List<int> lstTinhTP)
	{
		ProxyItems proxyItems = new ProxyItems();
		FirefoxDriverService firefoxDriverService = FirefoxDriverService.CreateDefaultService(Path.GetDirectoryName(Application.ExecutablePath).TrimEnd('\\') + "\\Settings");
		firefoxDriverService.FirefoxBinaryPath = pathFF;
		firefoxDriverService.HideCommandPromptWindow = true;
		FirefoxOptions firefoxOptions = new FirefoxOptions();
		FirefoxProfileManager firefoxProfileManager = new FirefoxProfileManager();
		FirefoxProfile firefoxProfile = null;
		if (GMailType == 2 && lstProfile.Count > 0)
		{
			firefoxProfile = firefoxProfileManager.GetProfile(GetRandomString(lstProfile));
		}
		if (firefoxProfile != null)
		{
			firefoxOptions.Profile = firefoxProfile;
		}
		else
		{
			firefoxOptions.Profile = new FirefoxProfile();
		}
		if (TypeProxy == 5 && MultiType.HasValue && !string.IsNullOrEmpty(ServiceUrl))
		{
			TypeProxy = MultiType.Value;
		}
		if (TypeIp == 2)
		{
			if (TypeProxy == 1)
			{
				if (lstProxy != null && lstProxy.Count > 0)
				{
					if (proxyIndex >= lstProxy.Count)
					{
						proxyIndex = 0;
					}
					proxyItems = lstProxy[proxyIndex];
					proxyItems.proxyPort = bllProxy.ProxyPort.HttpIPv4;
				}
				proxyIndex++;
			}
			else if (TypeProxy == 2)
			{
				TinsoftProxy tinsoftProxy = new TinsoftProxy();
				tinsoftProxy.api_key = keyProxy;
				if (lstTinhTP != null && lstTinhTP.Count > 0)
				{
					tinsoftProxy.location = GetRandomValue(lstTinhTP);
				}
				else
				{
					tinsoftProxy.location = 0;
				}
				while (TinsoftData[keyProxy].DangLay)
				{
					Sleep(100);
				}
				int num = NATechProxy.Misc.ObjInt((DateTime.Now - TinsoftData[keyProxy].LastChange).TotalSeconds);
				if (num > TinsoftData[keyProxy].ThoiGianCho)
				{
					TinsoftData[keyProxy].DangLay = true;
					bool flag = false;
					try
					{
						flag = tinsoftProxy.changeProxy();
					}
					catch (Exception ex)
					{
						LogHistory("Lỗi changeProxy Tinsoft. Chi tiết lỗi " + ex.Message);
					}
					TinsoftData[keyProxy].DangLay = false;
					if (flag)
					{
						TinsoftData[keyProxy].LastChange = DateTime.Now;
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = tinsoftProxy.proxy;
						proxyItems.ip = tinsoftProxy.ip;
						proxyItems.port = NATechProxy.Misc.ToString(tinsoftProxy.port);
						proxyItems.proxyPort = bllProxy.ProxyPort.HttpIPv4;
						proxyItems.TypeProxy = "TinSoftProxy";
						TinsoftData[keyProxy].LastProxy = proxyItems;
					}
					else
					{
						string errorCode = tinsoftProxy.errorCode;
						LogHistory(errorCode);
						if (!string.IsNullOrEmpty(errorCode))
						{
							if (errorCode.StartsWith("wait") && errorCode.EndsWith("for next change!"))
							{
								string value = Regex.Match(errorCode, "\\d+").Value;
								if (NATechProxy.Misc.IsDigit(value))
								{
									LogHistory($"Đợi {value}s sau để lấy proxy mới.");
									Sleep(1000 * NATechProxy.Misc.ObjInt(value));
								}
							}
							else
							{
								int num2 = TinsoftData[keyProxy].ThoiGianCho - num;
								if (num2 < 0)
								{
									num2 = TinsoftData[keyProxy].ThoiGianCho;
								}
								LogHistory(string.Format("Lỗi {1}. Chờ {0}s để chạy lần kế tiếp", num2, errorCode));
								Sleep(1000 * num2);
							}
						}
						else
						{
							int num3 = TinsoftData[keyProxy].ThoiGianCho - num;
							if (num3 < 0)
							{
								num3 = TinsoftData[keyProxy].ThoiGianCho;
							}
							LogHistory($"Lỗi đổi IP của TinSoft. Chờ {num3}s để chạy lần kế tiếp");
							Sleep(1000 * num3);
						}
					}
				}
				else
				{
					proxyItems = TinsoftData[keyProxy].LastProxy;
				}
			}
			else if (TypeProxy == 3)
			{
				if (!string.IsNullOrEmpty(keyProxy))
				{
					string err = string.Empty;
					DataTable dtDataXProxy = null;
					bllXProxyBll.RefreshXProxyList(ref err, ServiceUrl, ref dtDataXProxy, IsClearAll: false);
					DataRow[] array = dtDataXProxy.Select($"imei='{keyProxy}'");
					if (array != null && array.Length != 0)
					{
						int num4 = 0;
						while (!bllXProxyBll.ResetXProxy(ServiceUrl, NATechProxy.Misc.ToString(array[0]["proxy_ip"]), NATechProxy.Misc.ToString(array[0]["sock_port"])) && num4 < 30)
						{
							Sleep(300);
							num4++;
						}
						string msg = string.Empty;
						num4 = 0;
						while (!bllXProxyBll.CheckStatusXProxy(ServiceUrl, NATechProxy.Misc.ToString(array[0]["proxy_ip"]), NATechProxy.Misc.ToString(array[0]["sock_port"]), ref msg) && num4 < 30 && !(msg == "MODEM_NOT_FOUND") && !(msg == "COLLISION_IP") && !(msg == "MODEM_DISCONNECTED"))
						{
							Sleep(300);
							num4++;
						}
						bool flag2 = false;
						num4 = 0;
						do
						{
							bllXProxyBll.RefreshXProxyList(ref err, ServiceUrl, ref dtDataXProxy, IsClearAll: false);
							DataRow[] array2 = dtDataXProxy.Select($"imei='{keyProxy}'");
							flag2 = array2 != null && array2.Length != 0 && NATechProxy.Misc.ToString(array2[0]["public_ip"]) != "CONNECT_INTERNET_ERROR";
							num4++;
							if (!flag2 && num4 < 20)
							{
								Sleep(300);
							}
						}
						while (!flag2 && num4 < 20);
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = NATechProxy.Misc.ToString(array[0]["proxy_full"]);
						proxyItems.TypeProxy = "XProxy";
						proxyItems.ip = NATechProxy.Misc.ToString(array[0]["proxy_ip"]);
						proxyItems.port = NATechProxy.Misc.ToString(array[0]["proxy_port"]);
						proxyItems.proxyPort = bllProxyBll.GetProxyPort(proxyItems.port);
						proxyItems.public_ip = NATechProxy.Misc.ToString(array[0]["public_ip"]);
					}
				}
			}
			else if (TypeProxy == 4)
			{
				if (!string.IsNullOrEmpty(keyProxy))
				{
					string err2 = string.Empty;
					DataTable dtDataOBCProxy = null;
					bllOBCProxyBll.RefreshOBCProxyList(ref err2, ServiceUrl, ref dtDataOBCProxy, IsClearAll: false);
					DataRow[] array3 = dtDataOBCProxy.Select($"idKey='{keyProxy}'");
					if (array3 != null && array3.Length != 0)
					{
						int num5 = 0;
						while (!bllOBCProxyBll.ResetOBCProxy(ServiceUrl, NATechProxy.Misc.ToString(array3[0]["port"])) && num5 < 30)
						{
							Sleep(300);
							num5++;
						}
						num5 = 0;
						while (bllOBCProxyBll.GetStatusOBCProxy(ServiceUrl, NATechProxy.Misc.ToString(array3[0]["idKey"])).ToLower() != "running" && num5 < 30)
						{
							Sleep(300);
							num5++;
						}
						bool flag3 = false;
						num5 = 0;
						do
						{
							bllOBCProxyBll.RefreshOBCProxyList(ref err2, ServiceUrl, ref dtDataOBCProxy, IsClearAll: false);
							DataRow[] array4 = dtDataOBCProxy.Select($"idKey='{keyProxy}'");
							flag3 = array4 != null && array4.Length != 0 && NATechProxy.Misc.ToString(array4[0]["proxyStatus"]).ToLower() == "running";
							num5++;
							if (!flag3 && num5 < 20)
							{
								Sleep(300);
							}
						}
						while (!flag3 && num5 < 20);
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = NATechProxy.Misc.ToString(array3[0]["proxy_full"]);
						proxyItems.TypeProxy = "OBCProxy";
						proxyItems.ip = NATechProxy.Misc.ToString(array3[0]["proxy_ip"]);
						proxyItems.port = NATechProxy.Misc.ToString(array3[0]["port"]);
						proxyItems.proxyPort = bllProxyBll.GetProxyPort(NATechProxy.Misc.ToString(array3[0]["port"]));
						proxyItems.public_ip = NATechProxy.Misc.ToString(array3[0]["publicIp"]);
					}
				}
			}
			else if (TypeProxy == 5)
			{
				if (!string.IsNullOrEmpty(keyProxy))
				{
					string cmd = string.Empty;
					string cmd2 = string.Empty;
					if (dicProxySupplier != null && dicProxySupplier.ContainsKey(SupplierName))
					{
						cmd = dicProxySupplier[SupplierName].Reset;
						cmd2 = dicProxySupplier[SupplierName].CheckStatusCmd;
					}
					string empty = string.Empty;
					DataRow[] array5 = dtMachProxy.Select($"fullProxy='{keyProxy}'");
					if (array5 != null && array5.Length != 0)
					{
						int num6 = 0;
						while (!bllProxyBll.ResetCircuitProxy(ServiceUrl, NATechProxy.Misc.ToString(array5[0]["ip"]), NATechProxy.Misc.ToString(array5[0]["port"]), cmd) && num6 < 30)
						{
							Sleep(300);
							num6++;
						}
						string msg2 = string.Empty;
						num6 = 0;
						while (!bllProxyBll.CheckCircuitProxy(ServiceUrl, NATechProxy.Misc.ToString(array5[0]["ip"]), NATechProxy.Misc.ToString(array5[0]["port"]), ref msg2, cmd2) && num6 < 30 && !(msg2 == "MODEM_NOT_FOUND") && !(msg2 == "COLLISION_IP") && !(msg2 == "MODEM_DISCONNECTED"))
						{
							Sleep(300);
							num6++;
						}
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = string.Format("{0}", array5[0]["fullProxy"]);
						proxyItems.TypeProxy = "Mạch Proxy";
						proxyItems.ip = NATechProxy.Misc.ToString(array5[0]["ip"]);
						proxyItems.port = NATechProxy.Misc.ToString(array5[0]["port"]);
						proxyItems.proxyPort = bllProxyBll.GetProxyPort(NATechProxy.Misc.ToString(array5[0]["port"]));
						proxyItems.public_ip = string.Empty;
					}
				}
			}
			else if (TypeProxy == 7)
			{
				bllTMProxy bllTMProxy = new bllTMProxy();
				bllTMProxy.api_key = keyProxy;
				if (lstTinhTP != null && lstTinhTP.Count > 0)
				{
					bllTMProxy.location = GetRandomValue(lstTinhTP);
				}
				else
				{
					bllTMProxy.location = 1;
				}
				while (TMProxyData[keyProxy].DangLay)
				{
					Sleep(100);
				}
				int num7 = NATechProxy.Misc.ObjInt((DateTime.Now - TMProxyData[keyProxy].LastChange).TotalSeconds);
				if (num7 > TMProxyData[keyProxy].ThoiGianCho)
				{
					TMProxyData[keyProxy].DangLay = true;
					bool flag4 = false;
					try
					{
						flag4 = bllTMProxy.changeProxy();
					}
					catch (Exception ex2)
					{
						LogHistory("Lỗi changeProxy TMProxy. Chi tiết lỗi " + ex2.Message);
					}
					TMProxyData[keyProxy].DangLay = false;
					if (flag4)
					{
						TMProxyData[keyProxy].LastChange = DateTime.Now;
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = bllTMProxy.sock5;
						proxyItems.ip = bllTMProxy.ip;
						proxyItems.port = NATechProxy.Misc.ToString(bllTMProxy.socks5_port);
						proxyItems.proxyPort = bllProxy.ProxyPort.SocksIPv4;
						proxyItems.TypeProxy = "TMProxy (Socks5)";
						TMProxyData[keyProxy].LastProxy = proxyItems;
					}
					else
					{
						string errorCode2 = bllTMProxy.errorCode;
						LogHistory(errorCode2);
						if (!string.IsNullOrEmpty(errorCode2))
						{
							if (errorCode2.StartsWith("retry after") && errorCode2.EndsWith("second(s)"))
							{
								string value2 = Regex.Match(errorCode2, "\\d+").Value;
								if (NATechProxy.Misc.IsDigit(value2))
								{
									LogHistory($"Đợi {value2}s sau để lấy proxy mới.");
									Sleep(1000 * NATechProxy.Misc.ObjInt(value2));
								}
							}
							else if (errorCode2.ToLower().Contains("hết hạn"))
							{
								isRunFinish = true;
							}
							else
							{
								int num8 = TMProxyData[keyProxy].ThoiGianCho - num7;
								if (num8 < 0)
								{
									num8 = TMProxyData[keyProxy].ThoiGianCho;
								}
								LogHistory(string.Format("Lỗi {1}. Chờ {0}s để chạy lần kế tiếp", num8, errorCode2));
								Sleep(1000 * num8);
							}
						}
						else
						{
							int num9 = TMProxyData[keyProxy].ThoiGianCho - num7;
							if (num9 < 0)
							{
								num9 = TMProxyData[keyProxy].ThoiGianCho;
							}
							LogHistory(string.Format("Lỗi {1}. Chờ {0}s để chạy lần kế tiếp", num9, errorCode2));
							Sleep(1000 * num9);
						}
					}
				}
				else
				{
					proxyItems = TMProxyData[keyProxy].LastProxy;
				}
			}
			if (proxyItems != null && !string.IsNullOrEmpty(proxyItems.ip))
			{
				firefoxOptions.SetPreference("network.proxy.type", 1);
				switch (proxyItems.proxyPort)
				{
				default:
					firefoxOptions.SetPreference("network.proxy.http", proxyItems.ip);
					firefoxOptions.SetPreference("network.proxy.http_port", Convert.ToInt32(proxyItems.port));
					firefoxOptions.SetPreference("network.proxy.ssl", proxyItems.ip);
					firefoxOptions.SetPreference("network.proxy.ssl_port", Convert.ToInt32(proxyItems.port));
					break;
				case bllProxy.ProxyPort.SocksIPv4:
				case bllProxy.ProxyPort.SocksIPv6:
					firefoxOptions.SetPreference("network.proxy.socks", proxyItems.ip);
					firefoxOptions.SetPreference("network.proxy.socks_port", Convert.ToInt32(proxyItems.port));
					firefoxOptions.SetPreference("network.proxy.socks_version", 5);
					break;
				}
				LogHistory($"{proxyItems.TypeProxy}: [{proxyItems.ip}:{proxyItems.port}]");
				AddIp(proxyItems.fullProxy);
			}
		}
		firefoxOptions.Profile.SetPreference("browser.cache.disk.enable", value: false);
		firefoxOptions.Profile.SetPreference("browser.cache.memory.enable", value: false);
		firefoxOptions.Profile.SetPreference("browser.cache.offline.enable", value: false);
		firefoxOptions.Profile.SetPreference("network.http.use-cache", value: false);
		firefoxOptions.Profile.SetPreference("dom.webdriver.enabled", value: false);
		firefoxOptions.Profile.SetPreference("webdriver_enable_native_events", value: false);
		firefoxOptions.Profile.SetPreference("webdriver_assume_untrusted_issuer", value: false);
		firefoxOptions.Profile.SetPreference("media.peerconnection.enabled", value: false);
		firefoxOptions.Profile.SetPreference("media.navigator.permission.disabled", value: true);
		if (NotViewImage)
		{
			firefoxOptions.Profile.SetPreference("permissions.default.image", 2);
			firefoxOptions.Profile.SetPreference("dom.ipc.plugins.enabled.libflashplayer.so", value: false);
		}
		firefoxOptions.Profile.SetPreference("geo.enabled", value: true);
		firefoxOptions.Profile.SetPreference("geo.provider.use_corelocation", value: true);
		firefoxOptions.Profile.SetPreference("geo.prompt.testing", value: true);
		firefoxOptions.Profile.SetPreference("geo.prompt.testing.allow", value: true);
		firefoxOptions.Profile.SetPreference("intl.accept_languages", (cultureName == "vi-VN") ? "vi" : "en");
		string text = string.Empty;
		if (lstAgent.Count > 0)
		{
			while (string.IsNullOrEmpty(text))
			{
				text = lstAgent[new Random().Next(0, lstAgent.Count)];
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			firefoxOptions.Profile.SetPreference("general.useragent.override", text);
			LogHistory(string.Format("{1}: {0}", text, GetLanguage("ChangeAgent")));
		}
		driver = new FirefoxDriver(firefoxDriverService, firefoxOptions, TimeSpan.FromSeconds(Timeout));
		if (DisplayMode == 3)
		{
			driver.Manage().Window.Minimize();
		}
	}

	private void InitDriverChrome(ref ChromeDriver driver, int? MultiType, string ServiceUrl, string keyProxy, List<int> lstTinhTP, ref string IDBrowser, int LuongThu)
	{
		ProxyItems proxyItems = new ProxyItems();
		ChromeOptions chromeOptions = new ChromeOptions();
		DriverRec driverRec = null;
		if (dicDriverRec != null && dicDriverRec.Count > 0 && dicDriverRec.ContainsKey(LuongThu))
		{
			driverRec = dicDriverRec[LuongThu];
		}
		if (TypeProxy == 5 && MultiType.HasValue && !string.IsNullOrEmpty(ServiceUrl))
		{
			TypeProxy = MultiType.Value;
		}
		if (TypeIp == 2)
		{
			if (TypeProxy == 1)
			{
				if (lstProxy != null && lstProxy.Count > 0)
				{
					if (proxyIndex >= lstProxy.Count)
					{
						proxyIndex = 0;
					}
					proxyItems = lstProxy[proxyIndex];
					proxyItems.proxyPort = bllProxy.ProxyPort.HttpIPv4;
				}
				proxyIndex++;
			}
			else if (TypeProxy == 2)
			{
				TinsoftProxy tinsoftProxy = new TinsoftProxy();
				tinsoftProxy.api_key = keyProxy;
				if (lstTinhTP != null && lstTinhTP.Count > 0)
				{
					tinsoftProxy.location = GetRandomValue(lstTinhTP);
				}
				else
				{
					tinsoftProxy.location = 0;
				}
				while (TinsoftData[keyProxy].DangLay)
				{
					Sleep(100);
				}
				int num = NATechProxy.Misc.ObjInt((DateTime.Now - TinsoftData[keyProxy].LastChange).TotalSeconds);
				if (num > TinsoftData[keyProxy].ThoiGianCho)
				{
					TinsoftData[keyProxy].DangLay = true;
					bool flag = false;
					try
					{
						flag = tinsoftProxy.changeProxy();
					}
					catch (Exception ex)
					{
						LogHistory("Lỗi changeProxy Tinsoft. Chi tiết lỗi " + ex.Message);
					}
					TinsoftData[keyProxy].DangLay = false;
					if (flag)
					{
						TinsoftData[keyProxy].LastChange = DateTime.Now;
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = tinsoftProxy.proxy;
						proxyItems.ip = tinsoftProxy.ip;
						proxyItems.port = NATechProxy.Misc.ToString(tinsoftProxy.port);
						proxyItems.proxyPort = bllProxy.ProxyPort.HttpIPv4;
						proxyItems.TypeProxy = "TinSoftProxy";
						TinsoftData[keyProxy].LastProxy = proxyItems;
					}
					else
					{
						string errorCode = tinsoftProxy.errorCode;
						LogHistory(errorCode);
						if (!string.IsNullOrEmpty(errorCode))
						{
							if (errorCode.StartsWith("wait") && errorCode.EndsWith("for next change!"))
							{
								string value = Regex.Match(errorCode, "\\d+").Value;
								if (NATechProxy.Misc.IsDigit(value))
								{
									LogHistory($"Đợi {value}s sau để lấy proxy mới.");
									Sleep(1000 * NATechProxy.Misc.ObjInt(value));
								}
							}
							else if (errorCode.ToLower().Contains("hết hạn") || errorCode.ToLower().Contains("expired"))
							{
								isRunFinish = true;
							}
							else if (errorCode.ToLower().Contains("proxy not found"))
							{
								LogHistory($"Tỉnh/TP [{tinsoftProxy.location}] đã hết Proxy. Bạn vui lòng chọn Tỉnh/TP khác hoặc đợi phía TinsoftProxy họ bổ sung nhé!");
							}
							else
							{
								int num2 = TinsoftData[keyProxy].ThoiGianCho - num;
								if (num2 < 0)
								{
									num2 = TinsoftData[keyProxy].ThoiGianCho;
								}
								LogHistory(string.Format("Lỗi {1}. Chờ {0}s để chạy lần kế tiếp", num2, errorCode));
								Sleep(1000 * num2);
							}
						}
						else
						{
							int num3 = TinsoftData[keyProxy].ThoiGianCho - num;
							if (num3 < 0)
							{
								num3 = TinsoftData[keyProxy].ThoiGianCho;
							}
							LogHistory($"Lỗi đổi IP của TinSoft. Chờ {num3}s để chạy lần kế tiếp");
							Sleep(1000 * num3);
						}
					}
				}
				else
				{
					proxyItems = TinsoftData[keyProxy].LastProxy;
					LogHistory(string.Format("Lastchange: {0} - SoGiayCho: {1} - ThoiGianCho: {2}", TinsoftData[keyProxy].LastChange.ToString("dd/MM/yyyy HH:mm"), num, TinsoftData[keyProxy].ThoiGianCho));
				}
			}
			else if (TypeProxy == 3)
			{
				if (!string.IsNullOrEmpty(keyProxy))
				{
					string err = string.Empty;
					DataTable dtDataXProxy = null;
					bllXProxyBll.RefreshXProxyList(ref err, ServiceUrl, ref dtDataXProxy, IsClearAll: false);
					DataRow[] array = dtDataXProxy.Select($"imei='{keyProxy}'");
					if (array != null && array.Length != 0)
					{
						int num4 = 0;
						while (!bllXProxyBll.ResetXProxy(ServiceUrl, NATechProxy.Misc.ToString(array[0]["proxy_ip"]), NATechProxy.Misc.ToString(array[0]["sock_port"])) && num4 < 30)
						{
							Sleep(300);
							num4++;
						}
						string msg = string.Empty;
						num4 = 0;
						while (!bllXProxyBll.CheckStatusXProxy(ServiceUrl, NATechProxy.Misc.ToString(array[0]["proxy_ip"]), NATechProxy.Misc.ToString(array[0]["sock_port"]), ref msg) && num4 < 30 && !(msg == "MODEM_NOT_FOUND") && !(msg == "COLLISION_IP") && !(msg == "MODEM_DISCONNECTED"))
						{
							Sleep(300);
							num4++;
						}
						bool flag2 = false;
						num4 = 0;
						do
						{
							bllXProxyBll.RefreshXProxyList(ref err, ServiceUrl, ref dtDataXProxy, IsClearAll: false);
							DataRow[] array2 = dtDataXProxy.Select($"imei='{keyProxy}'");
							flag2 = array2 != null && array2.Length != 0 && NATechProxy.Misc.ToString(array2[0]["public_ip"]) != "CONNECT_INTERNET_ERROR";
							num4++;
							if (!flag2 && num4 < 20)
							{
								Sleep(300);
							}
						}
						while (!flag2 && num4 < 20);
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = NATechProxy.Misc.ToString(array[0]["proxy_full"]);
						proxyItems.TypeProxy = "XProxy";
						proxyItems.ip = NATechProxy.Misc.ToString(array[0]["proxy_ip"]);
						proxyItems.port = NATechProxy.Misc.ToString(array[0]["proxy_port"]);
						proxyItems.proxyPort = bllProxyBll.GetProxyPort(proxyItems.port);
						proxyItems.public_ip = NATechProxy.Misc.ToString(array[0]["public_ip"]);
					}
				}
			}
			else if (TypeProxy == 4)
			{
				if (!string.IsNullOrEmpty(keyProxy))
				{
					string err2 = string.Empty;
					DataTable dtDataOBCProxy = null;
					bllOBCProxyBll.RefreshOBCProxyList(ref err2, ServiceUrl, ref dtDataOBCProxy, IsClearAll: false);
					DataRow[] array3 = dtDataOBCProxy.Select($"idKey='{keyProxy}'");
					if (array3 != null && array3.Length != 0)
					{
						int num5 = 0;
						while (!bllOBCProxyBll.ResetOBCProxy(ServiceUrl, NATechProxy.Misc.ToString(array3[0]["port"])) && num5 < 30)
						{
							Sleep(300);
							num5++;
						}
						num5 = 0;
						while (bllOBCProxyBll.GetStatusOBCProxy(ServiceUrl, NATechProxy.Misc.ToString(array3[0]["idKey"])).ToLower() != "running" && num5 < 30)
						{
							Sleep(300);
							num5++;
						}
						bool flag3 = false;
						num5 = 0;
						do
						{
							bllOBCProxyBll.RefreshOBCProxyList(ref err2, ServiceUrl, ref dtDataOBCProxy, IsClearAll: false);
							DataRow[] array4 = dtDataOBCProxy.Select($"idKey='{keyProxy}'");
							flag3 = array4 != null && array4.Length != 0 && NATechProxy.Misc.ToString(array4[0]["proxyStatus"]).ToLower() == "running";
							num5++;
							if (!flag3 && num5 < 20)
							{
								Sleep(300);
							}
						}
						while (!flag3 && num5 < 20);
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = NATechProxy.Misc.ToString(array3[0]["proxy_full"]);
						proxyItems.TypeProxy = "OBCProxy";
						proxyItems.ip = NATechProxy.Misc.ToString(array3[0]["proxy_ip"]);
						proxyItems.port = NATechProxy.Misc.ToString(array3[0]["port"]);
						proxyItems.proxyPort = bllProxyBll.GetProxyPort(NATechProxy.Misc.ToString(array3[0]["port"]));
						proxyItems.public_ip = NATechProxy.Misc.ToString(array3[0]["publicIp"]);
					}
				}
			}
			else if (TypeProxy == 5)
			{
				if (!string.IsNullOrEmpty(keyProxy))
				{
					string cmd = string.Empty;
					string cmd2 = string.Empty;
					if (dicProxySupplier != null && dicProxySupplier.ContainsKey(SupplierName))
					{
						cmd = dicProxySupplier[SupplierName].Reset;
						cmd2 = dicProxySupplier[SupplierName].CheckStatusCmd;
					}
					string empty = string.Empty;
					DataRow[] array5 = dtMachProxy.Select($"fullProxy='{keyProxy}'");
					if (array5 != null && array5.Length != 0)
					{
						int num6 = 0;
						while (!bllProxyBll.ResetCircuitProxy(ServiceUrl, NATechProxy.Misc.ToString(array5[0]["ip"]), NATechProxy.Misc.ToString(array5[0]["port"]), cmd) && num6 < 30)
						{
							Sleep(300);
							num6++;
						}
						if (DCOMProxyDelay > 0)
						{
							Sleep(DCOMProxyDelay * 1000);
						}
						string msg2 = string.Empty;
						num6 = 0;
						while (!bllProxyBll.CheckCircuitProxy(ServiceUrl, NATechProxy.Misc.ToString(array5[0]["ip"]), NATechProxy.Misc.ToString(array5[0]["port"]), ref msg2, cmd2) && num6 < 30 && !(msg2 == "MODEM_NOT_FOUND") && !(msg2 == "COLLISION_IP") && !(msg2 == "MODEM_DISCONNECTED"))
						{
							Sleep(300);
							num6++;
						}
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = string.Format("{0}", array5[0]["fullProxy"]);
						proxyItems.TypeProxy = "Mạch Proxy";
						proxyItems.ip = NATechProxy.Misc.ToString(array5[0]["ip"]);
						proxyItems.port = NATechProxy.Misc.ToString(array5[0]["port"]);
						proxyItems.userName = NATechProxy.Misc.ToString(array5[0]["user"]);
						proxyItems.passWord = NATechProxy.Misc.ToString(array5[0]["pass"]);
						proxyItems.proxyPort = bllProxyBll.GetProxyPort(NATechProxy.Misc.ToString(array5[0]["port"]));
						proxyItems.public_ip = string.Empty;
					}
				}
			}
			else if (TypeProxy == 7)
			{
				bllTMProxy bllTMProxy = new bllTMProxy();
				bllTMProxy.api_key = keyProxy;
				if (lstTinhTP != null && lstTinhTP.Count > 0)
				{
					bllTMProxy.location = GetRandomValue(lstTinhTP);
				}
				else
				{
					bllTMProxy.location = 1;
				}
				while (TMProxyData[keyProxy].DangLay)
				{
					Sleep(100);
				}
				int num7 = NATechProxy.Misc.ObjInt((DateTime.Now - TMProxyData[keyProxy].LastChange).TotalSeconds);
				if (num7 > TMProxyData[keyProxy].ThoiGianCho)
				{
					TMProxyData[keyProxy].DangLay = true;
					bool flag4 = false;
					try
					{
						flag4 = bllTMProxy.changeProxy();
					}
					catch (Exception ex2)
					{
						LogHistory("Lỗi changeProxy TMProxy. Chi tiết lỗi " + ex2.Message);
					}
					TMProxyData[keyProxy].DangLay = false;
					if (flag4)
					{
						TMProxyData[keyProxy].LastChange = DateTime.Now;
						proxyItems = new ProxyItems();
						proxyItems.fullProxy = bllTMProxy.sock5;
						proxyItems.ip = bllTMProxy.ip;
						proxyItems.port = NATechProxy.Misc.ToString(bllTMProxy.socks5_port);
						proxyItems.proxyPort = bllProxy.ProxyPort.SocksIPv4;
						proxyItems.TypeProxy = "TMProxy (Socks5)";
						TMProxyData[keyProxy].LastProxy = proxyItems;
					}
					else
					{
						string errorCode2 = bllTMProxy.errorCode;
						LogHistory(errorCode2);
						if (!string.IsNullOrEmpty(errorCode2))
						{
							if (errorCode2.StartsWith("retry after") && errorCode2.EndsWith("second(s)"))
							{
								string value2 = Regex.Match(errorCode2, "\\d+").Value;
								if (NATechProxy.Misc.IsDigit(value2))
								{
									LogHistory($"Đợi {value2}s sau để lấy proxy mới.");
									Sleep(1000 * NATechProxy.Misc.ObjInt(value2));
								}
							}
							else if (errorCode2.ToLower().Contains("hết hạn"))
							{
								isRunFinish = true;
							}
							else
							{
								int num8 = TMProxyData[keyProxy].ThoiGianCho - num7;
								if (num8 < 0)
								{
									num8 = TMProxyData[keyProxy].ThoiGianCho;
								}
								LogHistory(string.Format("Lỗi {1}. Chờ {0}s để chạy lần kế tiếp", num8, errorCode2));
								Sleep(1000 * num8);
							}
						}
						else
						{
							int num9 = TMProxyData[keyProxy].ThoiGianCho - num7;
							if (num9 < 0)
							{
								num9 = TMProxyData[keyProxy].ThoiGianCho;
							}
							LogHistory(string.Format("Lỗi {1}. Chờ {0}s để chạy lần kế tiếp", num9, errorCode2));
							Sleep(1000 * num9);
						}
					}
				}
				else
				{
					proxyItems = TMProxyData[keyProxy].LastProxy;
				}
			}
		}
		chromeOptions.AddArgument("--lang=en");
		chromeOptions.AddArgument("--mute-audio");
		chromeOptions.AddArgument("--disable-dev-shm-usage");
		if (driverRec != null)
		{
			chromeOptions.AddArgument($"--window-size={driverRec.Width},{driverRec.Height}");
		}
		chromeOptions.AddArguments("--disable-popup-blocking", "--disable-extensions");
		if (DisplayMode == 3)
		{
			chromeOptions.AddArgument("--headless");
			chromeOptions.AddArgument("--no-sandbox");
		}
		if (NotViewImage)
		{
			chromeOptions.AddArgument("--blink-settings=imagesEnabled=false");
		}
		string browserExecutablePath = Directory.GetCurrentDirectory() + "\\browser\\orbita-browser\\chrome.exe";
		string text = string.Empty;
		if (lstAgent.Count > 0)
		{
			while (string.IsNullOrEmpty(text))
			{
				text = lstAgent[new Random().Next(0, lstAgent.Count)];
			}
		}
		string userDataDir = string.Empty;
		if (string.IsNullOrEmpty(IDBrowser))
		{
			IDBrowser = "NATech_" + NATechProxy.Misc.GenID(6);
		}
		if (!string.IsNullOrEmpty(IDBrowser))
		{
			userDataDir = GetTempProfile(IDBrowser);
		}
		if (r.Next(1, 101) <= LoadProfilePercent && dtOldProfile != null && dtOldProfile.Rows.Count > 0)
		{
			int index = r.Next(dtOldProfile.Rows.Count);
			string text2 = NATechProxy.Misc.ToString(dtOldProfile.Rows[index]["ProfilePath"]);
			string text3 = NATechProxy.Misc.ToString(dtOldProfile.Rows[index]["Proxy"]);
			if (Directory.Exists(text2))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text2);
				userDataDir = text2;
				if (proxyItems != null && !string.IsNullOrEmpty(text3))
				{
					List<string> list = NATechProxy.Misc.SplitToList(text3, ":");
					if (list.Count >= 2)
					{
						proxyItems.ip = list[0];
						proxyItems.port = list[1];
						if (list.Count >= 4)
						{
							proxyItems.userName = list[2];
							proxyItems.passWord = list[3];
						}
					}
				}
				LogHistory("Mở profile có sẵn:[" + directoryInfo.Name + "]");
			}
			else
			{
				LogHistory("Profile [" + text2 + "] không tồn tại hoặc đã bị xóa.");
			}
		}
		else
		{
			ProxyNATech proxy = null;
			if (proxyItems != null)
			{
				proxy = new ProxyNATech(proxyItems.ip, proxyItems.port, proxyItems.userName, proxyItems.passWord, proxyItems.ip);
			}
			string sErr = string.Empty;
			string ProfileName = IDBrowser;
			string profileDefaultPath = Directory.GetCurrentDirectory() + "\\Settings\\Profile\\Default";
			global::NATechDriver.NATechDriver.CreateProfile(profileDefaultPath, Path.GetTempPath(), ref ProfileName, text, proxy, BrowserLanguage, ref sErr);
			userDataDir = Path.Combine(Path.GetTempPath(), ProfileName);
			LogHistory("Chạy với profile ẩn danh [" + ProfileName + "]");
			IDBrowser = ProfileName;
		}
		if (proxyItems != null && !string.IsNullOrEmpty(proxyItems.ip) && !string.IsNullOrEmpty(proxyItems.port))
		{
			switch (proxyItems.proxyPort)
			{
			default:
				proxyItems.proxyType = bllProxy.TypeProxy.Http;
				break;
			case bllProxy.ProxyPort.SocksIPv4:
			case bllProxy.ProxyPort.SocksIPv6:
				proxyItems.proxyType = bllProxy.TypeProxy.Socks5;
				break;
			}
			if (proxyItems.proxyType == bllProxy.TypeProxy.Socks5)
			{
				chromeOptions.AddArguments("--proxy-server=socks5://" + proxyItems.ip + ":" + proxyItems.port);
			}
			else
			{
				chromeOptions.AddArguments("--proxy-server=" + proxyItems.ip + ":" + proxyItems.port);
			}
			LogHistory($"{proxyItems.TypeProxy}: [{proxyItems.ip}:{proxyItems.port}]");
			AddIp(proxyItems.fullProxy);
		}
		string driverExecutablePath = Directory.GetCurrentDirectory() + "\\chromedriver.exe";
		driver = global::NATechDriver.NATechDriver.Create(chromeOptions, userDataDir, driverExecutablePath, browserExecutablePath);
		if (DisplayMode != 3)
		{
			if (driverRec != null)
			{
				driver.Manage().Window.Size = new Size(driverRec.Width, driverRec.Height);
				driver.Manage().Window.Position = new Point(driverRec.X, driverRec.Y);
			}
			else
			{
				driver.Manage().Window.Maximize();
			}
		}
		driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Timeout);
		driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.0);
	}

	private string GetTempProfile(string IDBrowser)
	{
		return Path.Combine(Path.GetTempPath(), IDBrowser);
	}

	private void DongTrinhDuyet(ref FirefoxDriver driver)
	{
		try
		{
			if (driver != null)
			{
				driver.Close();
				driver.Quit();
				driver.Dispose();
				driver = null;
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	private int GetProcessIdFromWindowHandle(string windowHandle)
	{
		GetWindowThreadProcessId(new IntPtr(long.Parse(windowHandle)), out var lpdwProcessId);
		return lpdwProcessId;
	}

	private void DongTrinhDuyet(ref ChromeDriver driver, string IDBrowser)
	{
		try
		{
			if (driver != null)
			{
				try
				{
					driver.Close();
					driver.Quit();
				}
				catch (Exception)
				{
					try
					{
						driver.Close();
						driver.Quit();
					}
					catch (Exception ex)
					{
						LogHistory("Lỗi đóng trình duyệt: " + ex.Message);
					}
				}
				finally
				{
					driver = null;
				}
			}
			if (string.IsNullOrEmpty(IDBrowser))
			{
				return;
			}
			string tempProfile = GetTempProfile(IDBrowser);
			if (Directory.Exists(tempProfile))
			{
				Sleep(1000);
				try
				{
					Directory.Delete(tempProfile, recursive: true);
					return;
				}
				catch (Exception ex3)
				{
					string message = ex3.Message;
					return;
				}
			}
		}
		catch (Exception ex4)
		{
			LogHistory("Lỗi đóng trình duyệt: " + ex4.Message);
		}
	}

	private void CloseBrowser(ref ChromeDriver driver)
	{
		try
		{
			if (driver == null)
			{
				return;
			}
			try
			{
				driver.Close();
			}
			catch (Exception)
			{
			}
			finally
			{
				driver.Close();
			}
		}
		catch (Exception ex2)
		{
			LogHistory("Lỗi đóng trình duyệt: " + ex2.Message);
		}
	}

	private void QuitDriver(ref ChromeDriver driver)
	{
		try
		{
			if (driver == null)
			{
				return;
			}
			try
			{
				driver.Quit();
			}
			catch (Exception)
			{
			}
			finally
			{
				driver.Quit();
			}
		}
		catch (Exception ex2)
		{
			LogHistory("Lỗi đóng Driver: " + ex2.Message);
		}
	}

	private void KillProcess(ref ChromeDriver driver)
	{
		try
		{
			if (driver != null)
			{
				string currentWindowHandle = driver.CurrentWindowHandle;
				int processIdFromWindowHandle = GetProcessIdFromWindowHandle(currentWindowHandle.Replace("CDWindow-", ""));
				Process processById = Process.GetProcessById(processIdFromWindowHandle);
				processById.Kill();
				driver = null;
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private string GetAttributeFromList(IWebElement webElement, params string[] attributeName)
	{
		List<string> list = new List<string>();
		foreach (string attributeName2 in attributeName)
		{
			string attribute = webElement.GetAttribute(attributeName2);
			if (!string.IsNullOrEmpty(attribute))
			{
				list.Add(attribute);
			}
		}
		if (list.Count > 0)
		{
			return string.Join("|", list.ToArray());
		}
		return string.Empty;
	}

	private string GetIP(ref int Lan)
	{
		try
		{
			Lan++;
			string requestUriString = "http://checkip.dyndns.org";
			WebRequest webRequest = WebRequest.Create(requestUriString);
			WebResponse response = webRequest.GetResponse();
			StreamReader streamReader = new StreamReader(response.GetResponseStream());
			string text = streamReader.ReadToEnd().Trim();
			string[] array = text.Split(':');
			string text2 = array[1].Substring(1);
			string[] array2 = text2.Split('<');
			return array2[0];
		}
		catch (Exception ex)
		{
			if (Lan >= 30)
			{
				return string.Format("{1}. {0}", ex.Message, GetLanguage("GetIpErr"));
			}
			Sleep(1000);
			return GetIP(ref Lan);
		}
	}

	private void LogHistory(string s)
	{
		if (UseHistory)
		{
			sHistory = string.Format("{0}: {1}{2}{3}", DateTime.Now.ToString("HH:mm:ss"), s, Environment.NewLine, sHistory);
		}
	}

	private void btnSaveConfig_Click(object sender, EventArgs e)
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\NATechSEO");
			registryKey.SetValue("Language", cultureName);
			registryKey.SetValue("DcomDiaup", txtDialUp.Text);
			registryKey.SetValue("SoTrang", NATechProxy.Misc.ObjInt(speSoTrang.EditValue).ToString());
			registryKey.SetValue("TimeViewSearch", NATechProxy.Misc.ObjInt(speTimeViewSearch.EditValue).ToString());
			registryKey.SetValue("NumberOfClick", NATechProxy.Misc.ObjInt(speSumClick.EditValue).ToString());
			registryKey.SetValue("ViewPageFrom", NATechProxy.Misc.ObjInt(speTimeViewFrom.EditValue).ToString());
			registryKey.SetValue("ViewPageTo", NATechProxy.Misc.ObjInt(speTimeViewTo.EditValue).ToString());
			registryKey.SetValue("NotViewImage", ceiNotViewImage.Checked ? "1" : "0");
			registryKey.SetValue("AgentFile.txt", txtAgent.Text);
			registryKey.SetValue("IsOtherSite", ceiViewOtherSite.Checked ? "1" : "0");
			registryKey.SetValue("OtherSiteUrl", txtOtherSiteUrl.Text);
			registryKey.SetValue("OtherSiteViewTime", NATechProxy.Misc.ToString(NATechProxy.Misc.ObjInt(speOtherSiteViewTime.EditValue)));
			registryKey.SetValue("Email", memEmail.Text);
			registryKey.SetValue("EmailDelay", NATechProxy.Misc.ObjInt(speEmailDelay.EditValue).ToString());
			registryKey.SetValue("Timeout", NATechProxy.Misc.ObjInt(speTimeout.EditValue).ToString());
			registryKey.SetValue("GoogleUrl", NATechProxy.Misc.ToString(cbeGoogleSite.EditValue));
			registryKey.SetValue("TypeIp", NATechProxy.Misc.ToString(radTypeIp.EditValue));
			registryKey.SetValue("Proxy", NATechProxy.Misc.ToString(memProxy.Text));
			registryKey.SetValue("TypeProxy", NATechProxy.Misc.ToString(raiTypeProxy.EditValue));
			registryKey.SetValue("ProfileConfig", NATechProxy.Misc.ToString(memProfile.Text.Trim()));
			registryKey.SetValue("GMailType", NATechProxy.Misc.ToString(radGMail.EditValue));
			registryKey.SetValue("SoLuong", NATechProxy.Misc.ObjInt(speSoLuong.EditValue).ToString());
			registryKey.SetValue("SubLinkView", NATechProxy.Misc.ObjInt(speSubLinkView.EditValue).ToString());
			registryKey.SetValue("ChangeMACAddress", ceiChangeMACAddress.Checked ? "1" : "0");
			registryKey.SetValue("MACAddressInterval", NATechProxy.Misc.ObjInt(speMACAddressInterval.EditValue).ToString());
			registryKey.SetValue("SubLinkViewTo", NATechProxy.Misc.ObjInt(speSubLinkViewTo.EditValue).ToString());
			registryKey.SetValue("TimeViewSearchTo", NATechProxy.Misc.ObjInt(speTimeViewSearchTo.EditValue).ToString());
			registryKey.SetValue("OtherSiteViewTimeTo", NATechProxy.Misc.ObjInt(speOtherSiteViewTimeTo.EditValue).ToString());
			registryKey.SetValue("AutoStart", ceiAutoStart.Checked ? "1" : "0");
			registryKey.SetValue("SaveReport", ceiSaveReport.Checked ? "1" : "0");
			registryKey.SetValue("DcomTypeReset", NATechProxy.Misc.ToString(radDcomTypeReset.EditValue));
			registryKey.SetValue("ResetDcomInterval", NATechProxy.Misc.ObjInt(speResetDcomInterval.EditValue).ToString());
			registryKey.SetValue("DcomDelay", NATechProxy.Misc.ObjInt(speDcomDelay.EditValue).ToString());
			registryKey.SetValue("ViewYoutube", ceiViewYoutube.Checked ? "1" : "0");
			registryKey.SetValue("LoadProfilePercent", NATechProxy.Misc.ObjInt(speLoadProfilePercent.EditValue).ToString());
			registryKey.SetValue("InternalCount", NATechProxy.Misc.ObjInt(speInternalCount.EditValue).ToString());
			registryKey.SetValue("SpeedKeyboard", NATechProxy.Misc.ObjInt(speSpeedKeyboard.EditValue).ToString());
			registryKey.SetValue("DCOMProxyDelay", NATechProxy.Misc.ObjInt(speDCOMProxyDelay.EditValue).ToString());
			registryKey.SetValue("StarupWindow", ceiStarupWindow.Checked ? "1" : "0");
			registryKey.SetValue("DisplayMode", NATechProxy.Misc.ObjInt(lueDisplayMode.EditValue));
			registryKey.SetValue("ViewFilm", ceiViewFilm.Checked ? "1" : "0");
			registryKey.SetValue("ClearChrome", ceiClearChrome.Checked ? "1" : "0");
			registryKey.SetValue("ClearChromeTime", NATechProxy.Misc.ObjInt(speClearChromeTime.EditValue).ToString());
			registryKey.SetValue("ClickPhoneZalo", ceiCallPhoneZalo.Checked ? "1" : "0");
			registryKey.SetValue("LanguageBrowser", NATechProxy.Misc.ToString(ccbBrowserLanguage.EditValue));
			registryKey.Close();
			Settings.Default.TypeProxy = NATechProxy.Misc.ToString(raiTypeProxy.EditValue);
			Settings.Default.XProxyHost = NATechProxy.Misc.ToString(txtXProxyHost.Text);
			Settings.Default.OBCProxyHost = NATechProxy.Misc.ToString(txtOBCHost.Text);
			Settings.Default.OBCv2ProxyHost = NATechProxy.Misc.ToString(txtOBCV2Host.Text);
			Settings.Default.OBCV3Host = NATechProxy.Misc.ToString(txtOBCV3Host.Text);
			Settings.Default.OBCV3Proxy = NATechProxy.Misc.ToString(memOBCV3Proxy.Text);
			Settings.Default.ProxySupplier = NATechProxy.Misc.ToString(cbeProxySupplier.EditValue);
			Settings.Default.Save();
			grdvKeyword.CloseEditor();
			BindingContext[dtKeywordBingding].EndCurrentEdit();
			CheckValidKeyword(dtKeywordBingding);
			dtKeywordBingding.WriteXml(KeywordFileName, XmlWriteMode.WriteSchema);
			grdvMultiProxy.CloseEditor();
			BindingContext[dtMultiProxyBinding].EndCurrentEdit();
			dtMultiProxyBinding.WriteXml("MultiProxy.xml", XmlWriteMode.WriteSchema);
			grdvTinsoft.CloseEditor();
			BindingContext[dtTinsoft].EndCurrentEdit();
			dtTinsoft.WriteXml(TinsoftFileName, XmlWriteMode.WriteSchema);
			grdvTMProxy.CloseEditor();
			BindingContext[dtTMProxy].EndCurrentEdit();
			dtTMProxy.WriteXml(TMProxyFileName, XmlWriteMode.WriteSchema);
			NATechProxy.Misc.RegisterInStartup(ceiStarupWindow.Checked, "NATechSeo", Application.ExecutablePath);
			if (e != null)
			{
				MessageBox.Show(GetLanguage("SaveSussessfull"), GetLanguage("Notice"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CheckValidKeyword(DataTable dtKeywordBinding)
	{
		if (ApiClientNATech.loginResponse.UserName != "admin")
		{
			for (int num = dtKeywordBinding.Rows.Count - 1; num >= 0; num--)
			{
				if (dtKeywordBinding.Rows[num].RowState != DataRowState.Deleted && NATechProxy.Misc.ToString(dtKeywordBinding.Rows[num]["Domain"]).ToLower().Contains("NATech"))
				{
					dtKeywordBinding.Rows[num].Delete();
				}
			}
		}
		dtKeywordBinding.AcceptChanges();
	}

	private bool CheckFirefox(out string pIP, out string pathFF)
	{
		pathFF = "";
		pIP = string.Empty;
		try
		{
			pIP = Path.GetDirectoryName(Application.ExecutablePath).TrimEnd('\\') + "\\Settings\\config.txt";
			if (File.Exists(pIP))
			{
				string[] array = File.ReadAllLines(pIP);
				if (array.Length != 0)
				{
					pathFF = array[0];
				}
			}
		}
		catch (Exception)
		{
			return false;
		}
		if (string.IsNullOrEmpty(pathFF))
		{
			MessageBox.Show(GetLanguage("FirefoxErr"));
			return false;
		}
		if (!File.Exists(pathFF))
		{
			pathFF = pathFF.Replace("Program Files", "Program Files (x86)");
		}
		if (!File.Exists(pathFF))
		{
			return false;
		}
		return true;
	}

	private void btnPause_Click(object sender, EventArgs e)
	{
	}

	private void bbiRegister_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void bbiAbout_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			frmAbout frmAbout2 = new frmAbout();
			frmAbout2.ShowDialog(this);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void cedUseDcom_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void ceiViewOtherSite_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void bbiGUI_ItemClick(object sender, ItemClickEventArgs e)
	{
		Process.Start("https://www.youtube.com/watch?v=WSe415S7dzA");
	}

	private void txtAgent_EditValueChanged(object sender, EventArgs e)
	{
	}

	private void bbiVideoDemo_ItemClick(object sender, ItemClickEventArgs e)
	{
		Process.Start("https://www.facebook.com/na.com.vn");
	}

	private void bbiUserAgent_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://na.com.vn/huong-dan-cai-dat-addons-random-user-agent-phan-mem-seo-traffic-soft-viet.html");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bbiUpdate_ItemClick(object sender, ItemClickEventArgs e)
	{
		Process.Start("https://na.com.vn/huong-dan-cap-nhat-phan-mem-soft-viet-seo-phien-ban-5-0-0-ban-dung-profile-gologin.html");
	}

	private void btnDeleteHistory_Click(object sender, EventArgs e)
	{
		try
		{
			sTinhTrang = string.Empty;
			mmeHistory.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bbiClose_ItemClick(object sender, ItemClickEventArgs e)
	{
		Close();
	}

	private void btnProxyTemplate_Click(object sender, EventArgs e)
	{
		try
		{
			memProxy.Text = "103.35.64.12:3128\r\n113.160.218.14:50670\r\n118.70.144.77:3128\r\n118.69.140.108:53281\r\n203.162.21.216:8080\r\n210.245.8.19:3128";
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void btnReadProxy_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text file (.txt)|*.txt";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Title = "Load proxy from file (vd: 117.6.161.118:8080)";
			openFileDialog.Multiselect = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string text = NATechProxy.Misc.ReadFileToString(openFileDialog.FileName);
				if (!string.IsNullOrEmpty(text))
				{
					memProxy.Text = text;
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private bool DuyetLink()
	{
		return true;
	}

	private void radTypeIp_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			tabIP.SelectedTabPageIndex = ((NATechProxy.Misc.ObjInt(radTypeIp.EditValue) != 1) ? 1 : 0);
			lcgListDcom.Enabled = NATechProxy.Misc.ObjInt(radTypeIp.EditValue) == 1;
			lcgProxy.Enabled = NATechProxy.Misc.ObjInt(radTypeIp.EditValue) == 2;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void bbiNotification_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://docs.google.com/forms/d/e/1FAIpQLScfa8zWg8Dg_KE5QWKYoyirRzM3rSlpGOhV8x2terWuXc_baw/viewform");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void btnHomepage_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("https://www.facebook.com/na.com.vn");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void bbiGUI_ItemClick_1(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://na.com.vn/phan-mem-seo-traffic");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void cbeLang_EditValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (NATechProxy.Misc.ToString(cbeLang.EditValue).Equals("English"))
			{
				cultureName = "en-US";
			}
			else
			{
				cultureName = "vi-VN";
			}
			SetLanguage(cultureName);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void raiTypeProxy_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			speSoLuong.Enabled = true;
			switch (NATechProxy.Misc.ObjInt(raiTypeProxy.EditValue))
			{
			case 1:
				tabProxyMain.SelectedTabPage = lcgProxyFree;
				lcgProxyFree.Enabled = true;
				lcgTinSoftProxy.Enabled = false;
				lcgXProxy.Enabled = false;
				lcgOBCProxy.Enabled = false;
				lcgMultiProxy.Enabled = false;
				lcgOBCv2Proxy.Enabled = false;
				speSoLuong.Enabled = true;
				lcgTMProxy.Enabled = false;
				break;
			case 2:
				tabProxyMain.SelectedTabPage = lcgTinSoftProxy;
				lcgProxyFree.Enabled = false;
				lcgTinSoftProxy.Enabled = true;
				lcgXProxy.Enabled = false;
				lcgOBCProxy.Enabled = false;
				lcgMultiProxy.Enabled = false;
				lcgOBCv2Proxy.Enabled = false;
				speSoLuong.Enabled = true;
				lcgTMProxy.Enabled = false;
				break;
			case 3:
				tabProxyMain.SelectedTabPage = lcgXProxy;
				lcgProxyFree.Enabled = false;
				lcgTinSoftProxy.Enabled = false;
				lcgXProxy.Enabled = true;
				lcgOBCProxy.Enabled = false;
				lcgMultiProxy.Enabled = false;
				speSoLuong.Enabled = false;
				lcgOBCv2Proxy.Enabled = false;
				lcgTMProxy.Enabled = false;
				break;
			case 4:
				tabProxyMain.SelectedTabPage = lcgOBCProxy;
				lcgProxyFree.Enabled = false;
				lcgTinSoftProxy.Enabled = false;
				lcgXProxy.Enabled = false;
				lcgOBCProxy.Enabled = true;
				lcgMultiProxy.Enabled = false;
				speSoLuong.Enabled = false;
				lcgOBCv2Proxy.Enabled = false;
				lcgTMProxy.Enabled = false;
				break;
			case 5:
				tabProxyMain.SelectedTabPage = lcgOBCv2Proxy;
				lcgProxyFree.Enabled = false;
				lcgTinSoftProxy.Enabled = false;
				lcgXProxy.Enabled = false;
				lcgOBCProxy.Enabled = false;
				lcgMultiProxy.Enabled = false;
				speSoLuong.Enabled = true;
				lcgOBCv2Proxy.Enabled = true;
				lcgTMProxy.Enabled = false;
				break;
			case 6:
				tabProxyMain.SelectedTabPage = lcgMultiProxy;
				lcgProxyFree.Enabled = false;
				lcgTinSoftProxy.Enabled = false;
				lcgXProxy.Enabled = false;
				lcgOBCProxy.Enabled = false;
				lcgMultiProxy.Enabled = true;
				speSoLuong.Enabled = false;
				lcgOBCv2Proxy.Enabled = false;
				lcgTMProxy.Enabled = false;
				break;
			case 7:
				tabProxyMain.SelectedTabPage = lcgTMProxy;
				lcgProxyFree.Enabled = false;
				lcgTinSoftProxy.Enabled = false;
				lcgXProxy.Enabled = false;
				lcgOBCProxy.Enabled = false;
				speSoLuong.Enabled = true;
				lcgOBCv2Proxy.Enabled = false;
				lcgTMProxy.Enabled = true;
				break;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void btnLoadTinhTP_Click(object sender, EventArgs e)
	{
		try
		{
			if (NATechProxy.Misc.ObjInt(raiTypeProxy.EditValue) == 2)
			{
				DataTable locations = new TinsoftProxy("a").getLocations();
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void bsiFanpage_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://www.facebook.com/na.com.vn");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void tmSaveHistory_Tick(object sender, EventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void ReadHistory()
	{
		string text = $"{Environment.CurrentDirectory}\\{HistoryFileName}";
		if (dtHistory == null)
		{
			dtHistory = new DataTable("History");
		}
		if (File.Exists(text))
		{
			FileInfo fileInfo = new FileInfo(text);
			if (fileInfo != null && fileInfo.Length > 500000000)
			{
				try
				{
					File.Delete(text);
				}
				catch
				{
				}
				ReadHistory();
				return;
			}
			try
			{
				dtHistory.ReadXml(text);
			}
			catch
			{
				try
				{
					File.Delete(text);
				}
				catch
				{
				}
				ReadHistory();
				return;
			}
		}
		dtHistory.TableName = "History";
		if (!dtHistory.Columns.Contains("Ngay"))
		{
			dtHistory.Columns.Add("Ngay", typeof(DateTime));
		}
		if (!dtHistory.Columns.Contains("Domain"))
		{
			dtHistory.Columns.Add("Domain", typeof(string));
		}
		if (!dtHistory.Columns.Contains("Click"))
		{
			dtHistory.Columns.Add("Click", typeof(int));
		}
		string text2 = $"{Environment.CurrentDirectory}\\{IpFile}";
		if (dtIp == null)
		{
			dtIp = new DataTable("Ip");
		}
		if (File.Exists(text2))
		{
			FileInfo fileInfo2 = new FileInfo(text2);
			if (fileInfo2 != null && fileInfo2.Length > 500000000)
			{
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
				ReadHistory();
				return;
			}
			try
			{
				dtIp.ReadXml(text2);
			}
			catch
			{
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
				ReadHistory();
				return;
			}
		}
		dtIp.TableName = "Ip";
		if (!dtIp.Columns.Contains("Ngay"))
		{
			dtIp.Columns.Add("Ngay", typeof(DateTime));
		}
		if (!dtIp.Columns.Contains("IP"))
		{
			dtIp.Columns.Add("IP", typeof(string));
		}
		if (!dtIp.Columns.Contains("Click"))
		{
			dtIp.Columns.Add("Click", typeof(int));
		}
	}

	private void SaveHistoryToXml()
	{
		string fileName = $"{Environment.CurrentDirectory}\\{HistoryFileName}";
		dtHistory.WriteXml(fileName, XmlWriteMode.WriteSchema);
		string text = $"{Environment.CurrentDirectory}\\{IpFile}";
		dtIp.WriteXml(IpFile, XmlWriteMode.WriteSchema);
	}

	private void btnLoadHistory_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dataSource = dtHistory.Copy();
			grdHistory.DataSource = dataSource;
			DataTable dataSource2 = dtIp.Copy();
			grdIp.DataSource = dataSource2;
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnDeleteHistoryXml_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Bạn có chắc muốn xóa toàn bộ lịch sử báo cáo?", "Hỏi ý kiến", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
			{
				return;
			}
			if (!btnLoadHistory.Enabled)
			{
				MessageBox.Show("Phải dừng [Kết thúc] trước khi xóa lịch sử báo cáo");
				btnStop.Focus();
				return;
			}
			if (dtHistory == null)
			{
				ReadHistory();
			}
			dtHistory.Rows.Clear();
			SaveHistoryToXml();
			btnLoadHistory_Click(null, null);
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void bbiFirefoxProfile_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://na.com.vn/huong-dan-tao-firefox-profile-va-ap-dung-dang-nhap-gmail-cho-tool-soft-viet-seo-traffic.html");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void bbiTest_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void speTimeViewTo_EditValueChanged(object sender, EventArgs e)
	{
	}

	private void tmChangeMAC_Tick(object sender, EventArgs e)
	{
		try
		{
			tmChangeMAC.Stop();
			ChangeMacAddressFlag = true;
			ChangeMACAddress();
			ChangeMacAddressFlag = false;
			tmChangeMAC.Start();
		}
		catch (Exception ex)
		{
			ChangeMacAddressFlag = false;
			LogHistory(ex.Message);
		}
	}

	private bool DongHetTrinhDuyet()
	{
		if (dicBrowserStop == null)
		{
			return true;
		}
		foreach (KeyValuePair<string, bool> item in dicBrowserStop)
		{
			if (!item.Value)
			{
				return false;
			}
		}
		return true;
	}

	private void btnChangeMAC_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Bạn có chắc muốn thay đổi MAC Address?", "Hỏi ý kiến", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				ChangeMACAddress();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void radGMail_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			int num = NATechProxy.Misc.ObjInt(radGMail.EditValue);
			lcgMail.Enabled = num == 1;
			lcgProfile.Enabled = num == 2;
			switch (num)
			{
			case 1:
				tagGMail.SelectedTabPageIndex = 0;
				break;
			case 2:
				tagGMail.SelectedTabPageIndex = 1;
				break;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void bbiBuy_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			frmThanhToan frmThanhToan2 = new frmThanhToan();
			frmThanhToan2.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnConnectxProxy_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(txtXProxyHost.Text))
			{
				MessageBox.Show("Chưa nhập Service XProxy", "Thông báo lỗi XProxy");
				return;
			}
			XProxyHost = txtXProxyHost.Text;
			string err = string.Empty;
			bllXProxyBll.RefreshXProxyList(ref err, XProxyHost, ref dtXProxy, IsClearAll: true);
			if (!string.IsNullOrEmpty(err))
			{
				MessageBox.Show(err);
			}
			dtXProxyBinding = dtXProxy.Copy();
			grdXProxyList.DataSource = dtXProxyBinding;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnXProxyHomepage_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("https://www.facebook.com/xproxyvn");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnOBCHomePage_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("http://obc.vn");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnConnectOBC_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(txtOBCHost.Text))
			{
				MessageBox.Show("Chưa nhập Service OBC Proxy", "Thông báo lỗi OBC Proxy");
				return;
			}
			if (!txtOBCHost.Text.StartsWith("http") || NATechProxy.Misc.SplitToList(txtOBCHost.Text, ":").Count < 3)
			{
				MessageBox.Show("Service OBC Proxy không hợp lệ. Phải có dạng http://ip:port", "Thông báo lỗi OBC Proxy");
				return;
			}
			OBCProxyHost = txtOBCHost.Text.Trim().TrimEnd('/');
			string err = string.Empty;
			bllOBCProxyBll.RefreshOBCProxyList(ref err, OBCProxyHost, ref dtOBCProxy, IsClearAll: true);
			if (!string.IsNullOrEmpty(err))
			{
				MessageBox.Show(err);
			}
			dtOBCProxyBinding = dtOBCProxy.Copy();
			grdOBC.DataSource = dtOBCProxyBinding;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdTinsoft_Click(object sender, EventArgs e)
	{
	}

	private void btnMultiProxyConnect_Click(object sender, EventArgs e)
	{
		try
		{
			string err = string.Empty;
			grdvMultiProxy.CloseEditor();
			BindingContext[dtMultiProxyBinding].EndCurrentEdit();
			if (dtMultiProxyBinding.Rows.Count == 0)
			{
				MessageBox.Show("Chưa nhập Service Proxy", "Thông báo lỗi thiết lập");
				return;
			}
			if (dtMultiOBCProxy != null)
			{
				dtMultiOBCProxy.Rows.Clear();
			}
			foreach (DataRow row in dtMultiProxyBinding.Rows)
			{
				int num = NATechProxy.Misc.ObjInt(row["Type"]);
				string text = NATechProxy.Misc.ToString(row["ServiceUrl"]).TrimEnd('/');
				switch (num)
				{
				case 3:
				{
					DataTable dtDataXProxy = null;
					bllXProxyBll.RefreshXProxyList(ref err, text, ref dtDataXProxy, IsClearAll: true);
					if (!string.IsNullOrEmpty(err))
					{
						MessageBox.Show(err);
						break;
					}
					if (dtMultiXProxy == null)
					{
						dtMultiXProxy = dtDataXProxy.Copy();
						break;
					}
					foreach (DataRow row2 in dtDataXProxy.Rows)
					{
						DataRow dataRow5 = dtMultiXProxy.NewRow();
						dataRow5.ItemArray = row2.ItemArray;
						dtMultiXProxy.Rows.Add(dataRow5);
					}
					break;
				}
				case 4:
				{
					DataTable dtDataOBCProxy = null;
					bllOBCProxyBll.RefreshOBCProxyList(ref err, text, ref dtDataOBCProxy, IsClearAll: true);
					if (!string.IsNullOrEmpty(err))
					{
						MessageBox.Show(err);
						break;
					}
					if (dtMultiOBCProxy == null)
					{
						dtMultiOBCProxy = dtDataOBCProxy.Copy();
						break;
					}
					foreach (DataRow row3 in dtDataOBCProxy.Rows)
					{
						DataRow dataRow3 = dtMultiOBCProxy.NewRow();
						dataRow3.ItemArray = row3.ItemArray;
						dtMultiOBCProxy.Rows.Add(dataRow3);
					}
					break;
				}
				}
			}
			if (dtMultiXProxy != null)
			{
				dtMultiXProxyBinding = dtMultiXProxy.Copy();
				grdMultiXProxy.DataSource = dtMultiXProxyBinding;
			}
			if (dtMultiOBCProxy != null)
			{
				dtMultiOBCProxyBinding = dtMultiOBCProxy.Copy();
				grdMultiOBC.DataSource = dtMultiOBCProxyBinding;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFirefoxProfile_Click(object sender, EventArgs e)
	{
		bbiFirefoxProfile_ItemClick(null, null);
	}

	private void bbiDcomSetting_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://na.com.vn/huong-dan-cau-hinh-dcom-3g-4g-de-chay-tu-dong-reset-fake-ip-soft-viet.html");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void cbeProxySupplier_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			string key = NATechProxy.Misc.ToString(cbeProxySupplier.EditValue).Trim();
			if (dicProxySupplier != null && dicProxySupplier.ContainsKey(key))
			{
				btnOBCV2HomePage.Text = $"Trang chủ {dicProxySupplier[key].Name}";
				btnOBCV2HomePage.Tag = dicProxySupplier[key].HomePage;
			}
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnOBCV2HomePage_Click(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(NATechProxy.Misc.ToString(btnOBCV2HomePage.Tag)))
			{
				Process.Start(NATechProxy.Misc.ToString(btnOBCV2HomePage.Tag));
			}
			else
			{
				Process.Start("http://obc.vn");
			}
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnDisableIPv6_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("https://na.com.vn/huong-dan-cac-buoc-tat-ipv6-tren-windows-10.html");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void bbiDisableIPv6_ItemClick(object sender, ItemClickEventArgs e)
	{
		btnDisableIPv6_Click(null, null);
	}

	private void btnSetupDCOM_Click(object sender, EventArgs e)
	{
		bbiDcomSetting_ItemClick(null, null);
	}

	private void cbeProxySupplier_EditValueChanged(object sender, EventArgs e)
	{
		cbeProxySupplier_SelectedIndexChanged(null, null);
	}

	private void bbiRegisterTinsoft_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://na.com.vn/huong-dan-dang-ky-va-su-dung-proxy-cua-tinsoftproxy.html");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnRegisterTinsoft_Click(object sender, EventArgs e)
	{
		bbiRegisterTinsoft_ItemClick(null, null);
	}

	private void grdvbeiTinsoft_Delete_ButtonClick(object sender, ButtonPressedEventArgs e)
	{
		try
		{
			if (grdvTinsoft.FocusedRowHandle >= 0 && MessageBox.Show("Bạn có chắn muốn các dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				grdvTinsoft.DeleteRow(grdvTinsoft.FocusedRowHandle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTMProxyHome_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("https://tmproxy.com");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void grdvbeiTMProxy_Xoa_ButtonClick(object sender, ButtonPressedEventArgs e)
	{
		try
		{
			if (grdvTMProxy.FocusedRowHandle >= 0 && MessageBox.Show("Bạn có chắn muốn các dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				grdvTMProxy.DeleteRow(grdvTMProxy.FocusedRowHandle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdvbeiKeyword_Delete_ButtonClick(object sender, ButtonPressedEventArgs e)
	{
		try
		{
			if (grdvKeyword.FocusedRowHandle < 0)
			{
				return;
			}
			switch (e.Button.Index)
			{
			case 0:
				if (MessageBox.Show("Bạn có chắn muốn các dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
				{
					grdvKeyword.DeleteRow(grdvKeyword.FocusedRowHandle);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bbiLogin_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			LoginApi(isAutoLogin: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bbiChangePass_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			frmChangePassword frmChangePassword = new frmChangePassword();
			frmChangePassword.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bbiLogout_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			ApiClientNATech.loginResponse = null;
			LoginApi(isAutoLogin: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bbiResgisterAccount_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			frmRegister frmRegister = new frmRegister();
			frmRegister.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bbiAcctiveAccount_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			frmActiveAccount frmActiveAccount = new frmActiveAccount();
			frmActiveAccount.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCreateProfile_Click(object sender, EventArgs e)
	{
		try
		{
			ReadParam();
			frmCreateProfile frmCreateProfile = new frmCreateProfile();
			frmCreateProfile.FolderPath = Directory.GetCurrentDirectory() + "\\Settings\\Profile\\Manual";
			frmCreateProfile.BrowserPath = Directory.GetCurrentDirectory() + "\\browser\\orbita-browser\\chrome.exe";
			frmCreateProfile.ProfileDefaultPath = Directory.GetCurrentDirectory() + "\\Settings\\Profile\\Default";
			frmCreateProfile.lstUserAgent = lstAgent;
			frmCreateProfile.BrowserLanguage = BrowserLanguage;
			frmCreateProfile.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bbiCreateProfile_ItemClick(object sender, ItemClickEventArgs e)
	{
		btnCreateProfile_Click(null, null);
	}

	private void btnSyncUserAgent_Click(object sender, EventArgs e)
	{
		try
		{
			bool flag = true;
			if (e != null)
			{
				flag = MessageBox.Show(this, "Bạn có chắc chắn đồng bộ User Agent từ máy chủ của NATECH?", "Hỏi ý kiến", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes;
			}
			if (flag)
			{
				string sErr = string.Empty;
				string api = new ApiClientNATech().GetApi("Admin/GetUserAgent", null, ApiClientNATech.loginResponse.Token, out sErr);
				if (!string.IsNullOrEmpty(sErr))
				{
					MessageBox.Show(this, sErr, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				string text = new Sec().Decrypt(JsonConvert.DeserializeObject<string>(api));
				txtAgent.Text = (string.IsNullOrEmpty(txtAgent.Text) ? text : (txtAgent.Text + "\r\n" + text));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdvKeyword_ValidateRow(object sender, ValidateRowEventArgs e)
	{
		try
		{
			if (!e.Valid)
			{
				return;
			}
			ColumnView columnView = sender as ColumnView;
			List<string> list = new List<string>();
			DataView dataView = (DataView)(((GridView)grdKeyword.DefaultView).DataSource as IBindingList);
			for (int i = 0; i < dataView.Count; i++)
			{
				int num = NATechProxy.Misc.ObjInt(dataView[i]["Type"]);
				string nameType = GetNameType(num);
				string value = NATechProxy.Misc.ToString(dataView[i]["Key"]).ToLower();
				string text = NATechProxy.Misc.ToString(dataView[i]["Domain"]).ToLower();
				if (string.IsNullOrEmpty(value) && IsKeywordRequired(num))
				{
					e.Valid = false;
					columnView.SetColumnError(columnView.Columns["Key"], "[Từ khóa] bắt buộc nhập");
					break;
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (IsDomainNotHttp(num) && text.StartsWith("http"))
					{
						e.Valid = false;
						if (new List<int> { 4, 5, 7 }.Contains(num))
						{
							columnView.SetColumnError(columnView.Columns["Domain"], "Loại [" + nameType + "] chỉ cần điền Video ID.");
						}
						else
						{
							columnView.SetColumnError(columnView.Columns["Domain"], "Loại [" + nameType + "] không cần nhập http hoặc https ở đầu. Chỉ để domain trở về sau (vd: na.com.vn/phan-mem)");
						}
						break;
					}
					if (IsDomainRequiredHttp(num) && !text.StartsWith("http"))
					{
						e.Valid = false;
						columnView.SetColumnError(columnView.Columns["Domain"], "Phải điền link đầy đủ (Có http://....).");
						break;
					}
					continue;
				}
				e.Valid = false;
				columnView.SetColumnError(columnView.Columns["Domain"], "[Domain/Link/YoutubeVideoID] bắt buộc nhập");
				break;
			}
			if (e.Valid)
			{
				columnView.SetColumnError(columnView.Columns["Key"], string.Empty);
				columnView.SetColumnError(columnView.Columns["Domain"], string.Empty);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool IsDomainRequiredHttp(int loai)
	{
		List<int> list = new List<int> { 3, 6 };
		return list.Contains(loai);
	}

	private bool IsDomainNotHttp(int loai)
	{
		List<int> list = new List<int>();
		return list.Contains(loai);
	}

	private bool IsKeywordRequired(int loai)
	{
		List<int> list = new List<int> { 1, 2, 4, 5, 7 };
		return list.Contains(loai);
	}

	private void grdvKeyword_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
	{
		e.ExceptionMode = ExceptionMode.NoAction;
	}

	private void grdvKeyword_InvalidValueException(object sender, InvalidValueExceptionEventArgs e)
	{
		e.ExceptionMode = ExceptionMode.NoAction;
	}

	private void radDcomTypeReset_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			lciResetDcomInterval.Visibility = ((NATechProxy.Misc.ObjInt(radDcomTypeReset.EditValue) != 2) ? LayoutVisibility.Never : LayoutVisibility.Always);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tmResetDcom_Tick(object sender, EventArgs e)
	{
		try
		{
			LogHistory("Reset DCOM theo chu kỳ");
			tmResetDcom.Stop();
			DangChoResetDcom = true;
			while (!DongHetTrinhDuyet())
			{
				Sleep(300);
			}
			ResetDcom();
			DangChoResetDcom = false;
			tmResetDcom.Start();
		}
		catch (Exception ex)
		{
			DangChoResetDcom = false;
			LogHistory(ex.Message);
		}
	}

	private void grdTinsoft_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void btnTinsoftExportExcel_Click(object sender, EventArgs e)
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = "Tinsoft.xlsx";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				grdTinsoft.ExportToXlsx(saveFileDialog.FileName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lueDisplayMode_EditValueChanged(object sender, EventArgs e)
	{
	}

	private void lueDeviceType_EditValueChanged(object sender, EventArgs e)
	{
		try
		{
			int num = NATechProxy.Misc.ObjInt(lueDeviceType.EditValue);
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\NATechSEO");
			if (registryKey != null)
			{
				string text = NATechProxy.Misc.ToString(registryKey.GetValue("AgentFile.txt"));
				if (!string.IsNullOrEmpty(text))
				{
					txtAgent.Text = text;
				}
			}
			registryKey.Close();
			switch (num)
			{
			case 0:
				break;
			case 1:
			{
				List<string> list3 = NATechProxy.Misc.SplitToList(txtAgent.Text, "\r\n");
				List<string> list4 = list3.FindAll((string c) => c.Contains("iPhone") || c.Contains("Android")).ToList();
				txtAgent.Text = string.Join("\r\n", list4.ToArray());
				break;
			}
			case 2:
			{
				List<string> list = NATechProxy.Misc.SplitToList(txtAgent.Text, "\r\n");
				List<string> list2 = list.FindAll((string c) => !c.Contains("iPhone") && !c.Contains("Android")).ToList();
				txtAgent.Text = string.Join("\r\n", list2.ToArray());
				break;
			}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bbiUpdateVersion_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			frmUpdate frmUpdate = new frmUpdate();
			frmUpdate.CurrentVersion = "5.5.2";
			frmUpdate.ShowDialog(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void tmCanhBao_Tick(object sender, EventArgs e)
	{
		try
		{
			string text = NATechProxy.Misc.ToString(tmCanhBao.Tag);
			if (text == "RED")
			{
				tmCanhBao.Tag = "Green";
				bsiFanpage.ItemAppearance.Normal.ForeColor = Color.Green;
			}
			else
			{
				tmCanhBao.Tag = "RED";
				bsiFanpage.ItemAppearance.Normal.ForeColor = Color.Red;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void bbiBuyVPS_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Process.Start("https://shopvps247.com/register?rel_code=N8SVJZ");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnTinsoftImportExcel_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = MText.Show("Chọn file Excel hoặc CSV");
			openFileDialog.Filter = "Excel Files & CSV file |*.xlsx;*.xls;*.csv";
			openFileDialog.AddExtension = false;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			ImportData importData = new ImportData();
			DataSet dataSet = importData.DataReader(openFileDialog.FileName, isFirstRowAsDatatableColumn: true);
			if (dataSet != null && dataSet.Tables.Count == 0)
			{
				throw new Exception("Bạn hãy mở lại file Excel rồi chỉnh gì đó và lưu lại.\r\nSau đó thử thực hiện đọc lại lần nữa.");
			}
			if (dataSet.Tables[0].Columns.Count < 3)
			{
				MessageBox.Show("Cấu trúc file excel chưa đủ 3 cột");
				return;
			}
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				DataRow dataRow2 = dtTinsoft.NewRow();
				switch (NATechProxy.Misc.ToString(row[0]))
				{
				case "Key thường":
					dataRow2["Type"] = 1;
					break;
				case "Key VIP":
					dataRow2["Type"] = 2;
					break;
				case "Key dùng nhanh":
					dataRow2["Type"] = 3;
					break;
				default:
					dataRow2["Type"] = 1;
					break;
				}
				dataRow2["Key"] = row[1];
				dataRow2["TinhTP"] = row[2];
				dataRow2["Select"] = NATechProxy.Misc.ObjBol(row[3]);
				dtTinsoft.Rows.Add(dataRow2);
			}
			grdTinsoft.RefreshDataSource();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdvTinsoft_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Delete && grdvTinsoft.FocusedRowHandle > -1 && MessageBox.Show("Bạn có chắn muốn các dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				grdvTinsoft.DeleteRow(grdvTinsoft.FocusedRowHandle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void speTimeViewSearch_EditValueChanged(object sender, EventArgs e)
	{
		try
		{
			speTimeViewSearchTo.Properties.MinValue = NATechProxy.Misc.ObjInt(speTimeViewSearch.EditValue);
			speTimeViewSearchTo.Properties.MaxValue = 1000m;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void speTimeViewFrom_EditValueChanged(object sender, EventArgs e)
	{
		try
		{
			speTimeViewTo.Properties.MinValue = NATechProxy.Misc.ObjInt(speTimeViewFrom.EditValue);
			speTimeViewTo.Properties.MaxValue = 9999m;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void speOtherSiteViewTime_EditValueChanged(object sender, EventArgs e)
	{
		try
		{
			speOtherSiteViewTimeTo.Properties.MinValue = NATechProxy.Misc.ObjInt(speOtherSiteViewTime.EditValue);
			speOtherSiteViewTimeTo.Properties.MaxValue = 10000m;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void speSubLinkView_EditValueChanged(object sender, EventArgs e)
	{
		try
		{
			speSubLinkViewTo.Properties.MinValue = NATechProxy.Misc.ObjInt(speSubLinkView.EditValue);
			speSubLinkViewTo.Properties.MaxValue = 10000m;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tmAutoStart_Tick(object sender, EventArgs e)
	{
		try
		{
			if (btnRun.Enabled)
			{
				tmAutoStart.Stop();
				btnRun_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnConnectOBCV2_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(txtOBCV2Host.Text))
			{
				MessageBox.Show("Chưa nhập Service OBC v2", "Thông báo lỗi");
				return;
			}
			if (!txtOBCV2Host.Text.StartsWith("http") || NATechProxy.Misc.SplitToList(txtOBCV2Host.Text, ":").Count < 3)
			{
				MessageBox.Show("Service OBC v2 không hợp lệ. Phải có dạng http://ip:port", "Thông báo lỗi");
				return;
			}
			OBCv2Host = txtOBCV2Host.Text;
			string err = string.Empty;
			bllOBCProxyV2Bll.RefreshOBCv2ProxyList(ref err, OBCv2Host, ref dtOBCv2Proxy, IsClearAll: true);
			if (!string.IsNullOrEmpty(err))
			{
				MessageBox.Show(err);
			}
			dtOBCv2ProxyBinding = dtOBCv2Proxy.Copy();
			grdOBCV2.DataSource = dtOBCv2ProxyBinding;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnExcelDomain_Click(object sender, EventArgs e)
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = string.Format("ReportDomain_{0}", DateTime.Now.ToString("ddMMyyyyHHmm"));
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				grdHistory.ExportToXlsx(saveFileDialog.FileName);
				if (MessageBox.Show("Bạn có muốn mở file không?", "Hỏi ý kiến", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
				{
					Process.Start(saveFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdvKeyword_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Delete && MessageBox.Show("Bạn có chắn muốn các dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				btnClearSelectionKeyword_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdvMultiProxy_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Delete && MessageBox.Show("Bạn có chắn muốn các dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				grdvMultiProxy.DeleteRow(grdvMultiProxy.FocusedRowHandle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTinSoftHomepage_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("https://tinsoftproxy.com?r=greatman1612");
		}
		catch (Exception ex)
		{
			LogHistory(ex.Message);
		}
	}

	private void btnGetAgent_Click(object sender, EventArgs e)
	{
		bbiUserAgent_ItemClick(null, null);
	}

	private void btnExportKeyword_Click(object sender, EventArgs e)
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = "Keyword.xlsx";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				grdKeyword.ExportToXlsx(saveFileDialog.FileName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnClearSelectionKeyword_Click(object sender, EventArgs e)
	{
		try
		{
			int[] selectedRows = grdvKeyword.GetSelectedRows();
			if (e == null)
			{
				int[] array = selectedRows;
				foreach (int rowHandle in array)
				{
					grdvKeyword.DeleteRow(rowHandle);
				}
			}
			else if (MessageBox.Show("Bạn có chắn muốn xóa các dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				int[] array2 = selectedRows;
				foreach (int rowHandle2 in array2)
				{
					grdvKeyword.DeleteRow(rowHandle2);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnClearAllKeyword_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Bạn có chắn muốn tất cả từ khóa?", "Xác nhận", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				dtKeywordBingding.Rows.Clear();
				grdKeyword.RefreshDataSource();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnImportKeyword_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = MText.Show("Chọn file Excel hoặc CSV");
			openFileDialog.Filter = "Excel Files & CSV file |*.xlsx;*.xls;*.csv";
			openFileDialog.AddExtension = false;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			ImportData importData = new ImportData();
			DataSet dataSet = importData.DataReader(openFileDialog.FileName, isFirstRowAsDatatableColumn: true);
			if (dataSet != null && dataSet.Tables.Count == 0)
			{
				throw new Exception("Không tìm thấy Sheet nào, đề nghị mở file Excel ra và lưu lại.\r\nSau đó thử thực hiện đọc lại lần nữa.");
			}
			if (dataSet.Tables[0].Columns.Count < 3)
			{
				MessageBox.Show("Cấu trúc file excel chưa đủ 3 cột");
				return;
			}
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				DataRow dataRow2 = dtKeywordBingding.NewRow();
				dataRow2["Key"] = row[0];
				dataRow2["Domain"] = row[1];
				string text = NATechProxy.Misc.ToString(row[2]);
				if (!text.Contains("\r\n"))
				{
					if (text.Contains("\r"))
					{
						text = text.Replace("\r", "\r\n");
					}
					else if (text.Contains("\n"))
					{
						text = text.Replace("\n", "\r\n");
					}
				}
				dataRow2["SubLink"] = text;
				if (dataSet.Tables[0].Columns.Count > 3)
				{
					dataRow2["Type"] = GetType(NATechProxy.Misc.ToString(row[3]));
				}
				dtKeywordBingding.Rows.Add(dataRow2);
			}
			grdKeyword.RefreshDataSource();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private int GetType(string value)
	{
		if (NATechProxy.Misc.IsDigit(value))
		{
			return NATechProxy.Misc.ObjInt(value);
		}
		if (dtOption != null)
		{
			DataRow[] array = dtOption.Select($"NAME='{value}'");
			if (array != null && array.Length != 0)
			{
				return NATechProxy.Misc.ObjInt(array[0]["ID"]);
			}
		}
		return 1;
	}

	private string GetNameType(int value)
	{
		if (dtOption != null)
		{
			DataRow[] array = dtOption.Select($"ID={value}");
			if (array != null && array.Length != 0)
			{
				return NATechProxy.Misc.ToString(array[0]["NAME"]);
			}
		}
		return "SEO Google";
	}

	private void btnExcelIp_Click(object sender, EventArgs e)
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = string.Format("ReportIp_{0}", DateTime.Now.ToString("ddMMyyyyHHmm"));
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				grdIp.ExportToXlsx(saveFileDialog.FileName);
				if (MessageBox.Show("Bạn có muốn mở file không?", "Hỏi ý kiến", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
				{
					Process.Start(saveFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public IWebElement FindElementByXPath(ChromeDriver driver, string XPath)
	{
		try
		{
			if (driver == null || string.IsNullOrEmpty(XPath))
			{
				return null;
			}
			return driver.FindElement(By.XPath(XPath));
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return null;
		}
	}

	public IWebElement FindElementByTagName(ChromeDriver driver, string TagName)
	{
		try
		{
			if (driver == null || string.IsNullOrEmpty(TagName))
			{
				return null;
			}
			return driver.FindElement(By.TagName(TagName));
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return null;
		}
	}

	public IWebElement FindElementById(ChromeDriver driver, string Id)
	{
		try
		{
			if (driver == null || string.IsNullOrEmpty(Id))
			{
				return null;
			}
			return driver.FindElement(By.Id(Id));
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return null;
		}
	}

	public void ChiaManHinh(int SoLuong, ref ChromeDriver driver, int luongThu)
	{
		luongThu++;
		if (SoLuong <= 1)
		{
			return;
		}
		if (SoLuong < 5)
		{
			Screen primaryScreen = Screen.PrimaryScreen;
			int num = 0;
			int num2 = 0;
			int x = 0;
			int y = 0;
			if (SoLuong <= 2)
			{
				num = primaryScreen.Bounds.Width / 2;
				num2 = primaryScreen.Bounds.Height;
				if (luongThu == 2)
				{
					x = num;
					y = 0;
				}
			}
			else if (SoLuong <= 4)
			{
				num = primaryScreen.Bounds.Width / 2;
				num2 = primaryScreen.Bounds.Height / 2;
				switch (luongThu)
				{
				case 2:
					x = num;
					y = 0;
					break;
				case 3:
					x = 0;
					y = num2;
					break;
				case 4:
					x = num;
					y = num2;
					break;
				}
			}
			driver.Manage().Window.Position = new Point(x, y);
			driver.Manage().Window.Size = new Size(num, num2);
			return;
		}
		Screen primaryScreen2 = Screen.PrimaryScreen;
		if (luongThu == 5)
		{
		}
		int num3 = 5;
		int num4 = 3;
		int num5 = primaryScreen2.Bounds.Width / num3;
		int num6 = primaryScreen2.Bounds.Height / num4;
		if (num5 > 0 && num6 > 0)
		{
			int num7 = luongThu;
			int num8 = num5;
			int num9 = num6;
			int num10 = num3;
			int num11 = num4;
			int num12 = num7 % num10;
			int num13 = num7 % num11;
			int x2 = Math.Max(0, (num12 == 0) ? ((num10 - 1) * num8) : ((num12 - 1) * num8));
			int num14 = num7 / num10;
			if (num7 % num10 == 0 && num14 > 0)
			{
				num14--;
			}
			int y2 = Math.Min(num14 * num9, primaryScreen2.Bounds.Height - num6);
			driver.Manage().Window.Position = new Point(x2, y2);
			driver.Manage().Window.Size = new Size(num5, num6);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip5 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem5 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip6 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem6 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem6 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip7 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem7 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem7 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            DevExpress.Utils.SuperToolTip superToolTip8 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem8 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem8 = new DevExpress.Utils.ToolTipItem();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SuperToolTip superToolTip9 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem9 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem9 = new DevExpress.Utils.ToolTipItem();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SuperToolTip superToolTip10 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem10 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem10 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip11 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem11 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem11 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip12 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem12 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem12 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip13 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem13 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem13 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip14 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem14 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem14 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip15 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem15 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem15 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip16 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem16 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem16 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip17 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem17 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem17 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip18 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem18 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem18 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip19 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem19 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem19 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip20 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem20 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem20 = new DevExpress.Utils.ToolTipItem();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SuperToolTip superToolTip21 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem21 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem21 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip22 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem22 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem22 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip23 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem23 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem23 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip24 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem24 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem24 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip25 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem25 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem25 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip26 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem26 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem26 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip27 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem27 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem27 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip28 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem28 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem28 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip29 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem29 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem29 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip30 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem30 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem30 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip31 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem31 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem31 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip32 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem32 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem32 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip33 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem33 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem33 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip34 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem34 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem34 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip35 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem35 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem35 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip36 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem36 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem36 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip37 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem37 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem37 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip38 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem38 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem38 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip39 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem39 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem39 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip40 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem40 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem40 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip41 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem41 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem41 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip42 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem42 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem42 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip43 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem43 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem43 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip44 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem44 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem44 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip45 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem45 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem45 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip46 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem46 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem46 = new DevExpress.Utils.ToolTipItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.bsiStatus = new DevExpress.XtraBars.BarEditItem();
            this.bsieStatus = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.barMain = new DevExpress.XtraBars.Bar();
            this.bsiAccount = new DevExpress.XtraBars.BarSubItem();
            this.bsiUserInfo = new DevExpress.XtraBars.BarStaticItem();
            this.bbiLogin = new DevExpress.XtraBars.BarButtonItem();
            this.bbiChangePass = new DevExpress.XtraBars.BarButtonItem();
            this.bbiResgisterAccount = new DevExpress.XtraBars.BarButtonItem();
            this.bbiAcctiveAccount = new DevExpress.XtraBars.BarButtonItem();
            this.bbiLogout = new DevExpress.XtraBars.BarButtonItem();
            this.bbiCreateProfileMain = new DevExpress.XtraBars.BarButtonItem();
            this.bsiGUI = new DevExpress.XtraBars.BarSubItem();
            this.bbiUpdateVersion = new DevExpress.XtraBars.BarButtonItem();
            this.bbiUserAgent = new DevExpress.XtraBars.BarButtonItem();
            this.bbiVideoDemo = new DevExpress.XtraBars.BarButtonItem();
            this.bbiUpdate = new DevExpress.XtraBars.BarButtonItem();
            this.bbiGUI = new DevExpress.XtraBars.BarButtonItem();
            this.bbiFirefoxProfile = new DevExpress.XtraBars.BarButtonItem();
            this.bbiDcomSetting = new DevExpress.XtraBars.BarButtonItem();
            this.bbiDisableIPv6 = new DevExpress.XtraBars.BarButtonItem();
            this.bbiRegisterTinsoft = new DevExpress.XtraBars.BarButtonItem();
            this.bbiAbout = new DevExpress.XtraBars.BarButtonItem();
            this.bbiBuy = new DevExpress.XtraBars.BarButtonItem();
            this.bbiClose = new DevExpress.XtraBars.BarButtonItem();
            this.bbiBuyVPS = new DevExpress.XtraBars.BarButtonItem();
            this.bsiFanpage = new DevExpress.XtraBars.BarStaticItem();
            this.cbeLang = new DevExpress.XtraBars.BarEditItem();
            this.cbeeLang = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.bsiHelp = new DevExpress.XtraBars.BarSubItem();
            this.bbiRegister = new DevExpress.XtraBars.BarButtonItem();
            this.bbiNotification = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.bbiPrice = new DevExpress.XtraBars.BarButtonItem();
            this.bbiTest = new DevExpress.XtraBars.BarButtonItem();
            this.bbiCreateProfile = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemRadioGroup1 = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tmSaveHistory = new System.Windows.Forms.Timer(this.components);
            this.tmChangeMAC = new System.Windows.Forms.Timer(this.components);
            this.tmAutoStart = new System.Windows.Forms.Timer(this.components);
            this.tmResetDcom = new System.Windows.Forms.Timer(this.components);
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.tmCanhBao = new System.Windows.Forms.Timer(this.components);
            this.btnRun = new DevExpress.XtraEditors.SimpleButton();
            this.lcMain = new DevExpress.XtraLayout.LayoutControl();
            this.mmeHistory = new DevExpress.XtraEditors.MemoEdit();
            this.lueDeviceType = new DevExpress.XtraEditors.GridLookUpEdit();
            this.luevDeviceType = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.luevDeviceType_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ccbBrowserLanguage = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.ceiClearChrome = new DevExpress.XtraEditors.CheckEdit();
            this.ceiCallPhoneZalo = new DevExpress.XtraEditors.CheckEdit();
            this.speClearChromeTime = new DevExpress.XtraEditors.SpinEdit();
            this.ceiViewFilm = new DevExpress.XtraEditors.CheckEdit();
            this.lueDisplayMode = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.luevDisplayMode_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnTinsoftImportExcel = new DevExpress.XtraEditors.SimpleButton();
            this.btnTinsoftExportExcel = new DevExpress.XtraEditors.SimpleButton();
            this.ceiStarupWindow = new DevExpress.XtraEditors.CheckEdit();
            this.speDCOMProxyDelay = new DevExpress.XtraEditors.SpinEdit();
            this.speSpeedKeyboard = new DevExpress.XtraEditors.SpinEdit();
            this.speInternalCount = new DevExpress.XtraEditors.SpinEdit();
            this.btnSyncUserAgent = new DevExpress.XtraEditors.SimpleButton();
            this.speLoadProfilePercent = new DevExpress.XtraEditors.SpinEdit();
            this.ceiViewYoutube = new DevExpress.XtraEditors.CheckEdit();
            this.grdTMProxy = new DevExpress.XtraGrid.GridControl();
            this.grdvTMProxy = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvTMProxy_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueTMProxy_Type = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvTMProxy_Key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvTMProxy_TinhTP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueTMProxy_TinhTP = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.grdvTMProxy_Xoa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvbeiTMProxy_Xoa = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnTMProxyHome = new DevExpress.XtraEditors.SimpleButton();
            this.speDcomDelay = new DevExpress.XtraEditors.SpinEdit();
            this.speResetDcomInterval = new DevExpress.XtraEditors.SpinEdit();
            this.radDcomTypeReset = new DevExpress.XtraEditors.RadioGroup();
            this.btnSetupDCOM = new DevExpress.XtraEditors.SimpleButton();
            this.cbeProxySupplier = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnCreateProfile = new DevExpress.XtraEditors.SimpleButton();
            this.btnFirefoxProfile = new DevExpress.XtraEditors.SimpleButton();
            this.grdTinsoft = new DevExpress.XtraGrid.GridControl();
            this.grdvTinsoft = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvTinsoft_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueTinsoft_Type = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.grdvluevTinsoft_Type = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvTinsoft_Key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvTinsoft_TinhTP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvccbTinsoft_TinhTP = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.grdvTinsoft_Select = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueTinsoft_Select = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grdvTinsoft_Delete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvbeiTinsoft_Delete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grdvcbeTinsoft_Type = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.lbeFakeIpNoticMacAddress = new DevExpress.XtraEditors.LabelControl();
            this.memOBCV3Proxy = new DevExpress.XtraEditors.MemoEdit();
            this.txtOBCV3Host = new DevExpress.XtraEditors.TextEdit();
            this.ceiSaveReport = new DevExpress.XtraEditors.CheckEdit();
            this.grdOBCV2 = new DevExpress.XtraGrid.GridControl();
            this.grdvOBCV2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnOBCV2HomePage = new DevExpress.XtraEditors.SimpleButton();
            this.txtOBCV2Host = new DevExpress.XtraEditors.TextEdit();
            this.btnConnectOBCV2 = new DevExpress.XtraEditors.SimpleButton();
            this.ceiAutoStart = new DevExpress.XtraEditors.CheckEdit();
            this.speSubLinkViewTo = new DevExpress.XtraEditors.SpinEdit();
            this.speOtherSiteViewTimeTo = new DevExpress.XtraEditors.SpinEdit();
            this.speTimeViewSearchTo = new DevExpress.XtraEditors.SpinEdit();
            this.btnGetAgent = new DevExpress.XtraEditors.SimpleButton();
            this.btnRegisterTinsoft = new DevExpress.XtraEditors.SimpleButton();
            this.btnDisableIPv6 = new DevExpress.XtraEditors.SimpleButton();
            this.btnTinSoftHomepage = new DevExpress.XtraEditors.SimpleButton();
            this.btnXProxyHomepage = new DevExpress.XtraEditors.SimpleButton();
            this.btnOBCHomePage = new DevExpress.XtraEditors.SimpleButton();
            this.btnMultiProxyConnect = new DevExpress.XtraEditors.SimpleButton();
            this.grdMultiOBC = new DevExpress.XtraGrid.GridControl();
            this.grdvMultiOBC = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvMultiOBC_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_idKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_proxyAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_proxyStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_sockPort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_socksEnable = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_ServiceUrl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiOBC_IsRun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueMultiOBC_IsRun = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grdMultiXProxy = new DevExpress.XtraGrid.GridControl();
            this.grdvMultiXProxy = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvMultiXProxy_stt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_public_ip = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_system = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_proxy_port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_sock_port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_proxy_full = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_imei = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_ServiceUrl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvMultiXProxy_IsRun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueMultiXProxy_IsRun = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.radMultiProxyType = new DevExpress.XtraEditors.RadioGroup();
            this.grdMultiProxy = new DevExpress.XtraGrid.GridControl();
            this.grdvMultiProxy = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvMultiProxy_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueMultiProxy_Type = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.grdvMultiProxy_ServiceUrl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.btnClearAllKeyword = new DevExpress.XtraEditors.SimpleButton();
            this.btnClearSelectionKeyword = new DevExpress.XtraEditors.SimpleButton();
            this.grdOBC = new DevExpress.XtraGrid.GridControl();
            this.grdvOBC = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvOBC_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_idKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_proxyAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_proxyStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_sockPort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_socksEnable = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_publicIp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvOBC_IsRun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnConnectOBC = new DevExpress.XtraEditors.SimpleButton();
            this.txtOBCHost = new DevExpress.XtraEditors.TextEdit();
            this.btnImportKeyword = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportKeyword = new DevExpress.XtraEditors.SimpleButton();
            this.grdKeyword = new DevExpress.XtraGrid.GridControl();
            this.grdvKeyword = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvKeyword_Key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvKeyword_Domain = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvKeyword_SubLink = new DevExpress.XtraGrid.Columns.GridColumn();
            this.luevSubLink = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.grdvKeyword_Delete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvbeiKeyword_Delete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.grdvKeyword_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvlueKeyword_Type = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.grdvluevKeyword_Type = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvluevKeyword_Type_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnExcelIp = new DevExpress.XtraEditors.SimpleButton();
            this.btnExcelDomain = new DevExpress.XtraEditors.SimpleButton();
            this.memReChuot = new DevExpress.XtraEditors.MemoEdit();
            this.grdXProxyList = new DevExpress.XtraGrid.GridControl();
            this.grdvXProxyList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvXProxyList_stt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvXProxyList_public_ip = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvXProxyList_system = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvXProxyList_proxy_port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvXProxyList_sock_port = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvXProxyList_proxy_full = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvXProxyList_imei = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvXProxyList_IsRun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ceiIsRun = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btnConnectxProxy = new DevExpress.XtraEditors.SimpleButton();
            this.txtXProxyHost = new DevExpress.XtraEditors.TextEdit();
            this.lbeNoticeFreeProxy = new DevExpress.XtraEditors.LabelControl();
            this.btnChangeMAC = new DevExpress.XtraEditors.SimpleButton();
            this.speMACAddressInterval = new DevExpress.XtraEditors.SpinEdit();
            this.ceiChangeMACAddress = new DevExpress.XtraEditors.CheckEdit();
            this.speSubLinkView = new DevExpress.XtraEditors.SpinEdit();
            this.grdIp = new DevExpress.XtraGrid.GridControl();
            this.grdvIp = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvIp_Ngay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvIp_Ip = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvIp_Click = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speSoLuong = new DevExpress.XtraEditors.SpinEdit();
            this.memProfile = new DevExpress.XtraEditors.MemoEdit();
            this.radGMail = new DevExpress.XtraEditors.RadioGroup();
            this.lbeNoticeTinsoft = new DevExpress.XtraEditors.LabelControl();
            this.btnDeleteHistoryXml = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoadHistory = new DevExpress.XtraEditors.SimpleButton();
            this.grdHistory = new DevExpress.XtraGrid.GridControl();
            this.grdvHistory = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdvHistory_Ngay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvHistory_Domain = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdvHistory_Click = new DevExpress.XtraGrid.Columns.GridColumn();
            this.memProxyNote = new DevExpress.XtraEditors.MemoEdit();
            this.memProxy = new DevExpress.XtraEditors.MemoEdit();
            this.lbeUserAgentNotice = new DevExpress.XtraEditors.LabelControl();
            this.raiTypeProxy = new DevExpress.XtraEditors.RadioGroup();
            this.txtDialUp = new DevExpress.XtraEditors.MemoEdit();
            this.btnHomepage = new DevExpress.XtraEditors.SimpleButton();
            this.radTypeIp = new DevExpress.XtraEditors.RadioGroup();
            this.cbeGoogleSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.speTimeout = new DevExpress.XtraEditors.SpinEdit();
            this.speEmailDelay = new DevExpress.XtraEditors.SpinEdit();
            this.btnDeleteHistory = new DevExpress.XtraEditors.SimpleButton();
            this.memEmail = new DevExpress.XtraEditors.MemoEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.ceiNotViewImage = new DevExpress.XtraEditors.CheckEdit();
            this.ceiUseHistory = new DevExpress.XtraEditors.CheckEdit();
            this.speTimeViewTo = new DevExpress.XtraEditors.SpinEdit();
            this.speTimeViewSearch = new DevExpress.XtraEditors.SpinEdit();
            this.txtOtherSiteUrl = new DevExpress.XtraEditors.MemoEdit();
            this.speOtherSiteViewTime = new DevExpress.XtraEditors.SpinEdit();
            this.ceiViewOtherSite = new DevExpress.XtraEditors.CheckEdit();
            this.txtAgent = new DevExpress.XtraEditors.MemoEdit();
            this.speSoTrang = new DevExpress.XtraEditors.SpinEdit();
            this.speSumClick = new DevExpress.XtraEditors.SpinEdit();
            this.speTimeViewFrom = new DevExpress.XtraEditors.SpinEdit();
            this.btnStop = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem19 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem30 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabMain = new DevExpress.XtraLayout.TabbedControlGroup();
            this.lcgMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup8 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lgTuKhoa = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem20 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem24 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem16 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem28 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem14 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcgHistory = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciHistory = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem3 = new DevExpress.XtraLayout.SplitterItem();
            this.lcgTimeSetup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lgTraffic = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciSuDung = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOtherSiteListUrl = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOtherSiteViewTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOtherSiteViewTimeTo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgSettings = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciGoogleSite = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSoLanClick = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSoLuong = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciDuyet = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgOtherConfig = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciUseHistory = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAutoStart = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTimeout = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciLoadProfilePercent = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStarupWindow = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciDisplayMode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciClearChrome = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem40 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBrowserLanguage = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgTimeGoogle = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciSpeedKeyboard = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciThoiGianTK = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTimeViewSearchTo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgViewAds = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciTimeViewFrom = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTimeViewTo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgTimeInternalExternal = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem63 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSubLinkTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSubLinkViewTo = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem39 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCallPhoneZalo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciViewYoutube = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciNotViewImage = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem16 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcgLoginGmail = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciGMail = new DevExpress.XtraLayout.LayoutControlItem();
            this.tagGMail = new DevExpress.XtraLayout.TabbedControlGroup();
            this.lcgMail = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciEmailDelay = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciListEmail = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem8 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcgProfile = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciListProfile = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem11 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem53 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem60 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgUserAgent = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lbeNoticeAgent = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem38 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem62 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciDeviceType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgFakeIP = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgDcom = new DevExpress.XtraLayout.LayoutControlGroup();
            this.tabIP = new DevExpress.XtraLayout.TabbedControlGroup();
            this.lcgProxy = new DevExpress.XtraLayout.LayoutControlGroup();
            this.tabProxyMain = new DevExpress.XtraLayout.TabbedControlGroup();
            this.lcgOBCv2Proxy = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem49 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup9 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem50 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem45 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem54 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem65 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgProxyFree = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgFreeProxySub = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgAddonGetProxy = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem4 = new DevExpress.XtraLayout.SplitterItem();
            this.layoutControlItem25 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgFreeProxyConfig = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgTinSoftProxy = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem37 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem52 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem47 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem55 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgXProxy = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciHostXProxy = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem26 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem7 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem27 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem36 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgOBCProxy = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem18 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem21 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem10 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem22 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem35 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgOBCv2Proxy_Old = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem43 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem44 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem13 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem46 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgMultiProxy = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem29 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem31 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem32 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem33 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem34 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem9 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            this.splitterItem2 = new DevExpress.XtraLayout.SplitterItem();
            this.lcgTMProxy = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem57 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem58 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem15 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciProxySupplier = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgListDcom = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciDialUp = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem56 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciResetDcomInterval = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciDcomDelay = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem48 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciChangeIp = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgChangeMac = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciChangeMac = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciChangeMacTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem23 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem12 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem51 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgReport = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciReportIp = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcgReportDomain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.splitterItem7 = new DevExpress.XtraLayout.SplitterItem();
            this.lciSaveReport = new DevExpress.XtraLayout.LayoutControlItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView6 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView7 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView8 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView9 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView10 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView11 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsieStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeeLang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcMain)).BeginInit();
            this.lcMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mmeHistory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDeviceType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.luevDeviceType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ccbBrowserLanguage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiClearChrome.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiCallPhoneZalo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speClearChromeTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiViewFilm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDisplayMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiStarupWindow.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speDCOMProxyDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSpeedKeyboard.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speInternalCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speLoadProfilePercent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiViewYoutube.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTMProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvTMProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTMProxy_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTMProxy_TinhTP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvbeiTMProxy_Xoa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speDcomDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speResetDcomInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDcomTypeReset.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeProxySupplier.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTinsoft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvTinsoft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTinsoft_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvluevTinsoft_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvccbTinsoft_TinhTP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTinsoft_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvbeiTinsoft_Delete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvcbeTinsoft_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memOBCV3Proxy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOBCV3Host.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiSaveReport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOBCV2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvOBCV2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOBCV2Host.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiAutoStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSubLinkViewTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speOtherSiteViewTimeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewSearchTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMultiOBC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMultiOBC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueMultiOBC_IsRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMultiXProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMultiXProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueMultiXProxy_IsRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiProxyType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMultiProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMultiProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueMultiProxy_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOBC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvOBC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOBCHost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdKeyword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvKeyword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.luevSubLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvbeiKeyword_Delete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueKeyword_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvluevKeyword_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memReChuot.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdXProxyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvXProxyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiIsRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXProxyHost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speMACAddressInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiChangeMACAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSubLinkView.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvIp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSoLuong.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memProfile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGMail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memProxyNote.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memProxy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.raiTypeProxy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDialUp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTypeIp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeGoogleSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeout.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speEmailDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiNotViewImage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiUseHistory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherSiteUrl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speOtherSiteViewTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiViewOtherSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAgent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSoTrang.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSumClick.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem30)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgTuKhoa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTimeSetup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgTraffic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSuDung)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOtherSiteListUrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOtherSiteViewTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOtherSiteViewTimeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGoogleSite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSoLanClick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDuyet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOtherConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciUseHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAutoStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLoadProfilePercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStarupWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDisplayMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClearChrome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem40)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBrowserLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTimeGoogle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSpeedKeyboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciThoiGianTK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeViewSearchTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgViewAds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeViewFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeViewTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTimeInternalExternal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem63)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSubLinkTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSubLinkViewTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem39)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCallPhoneZalo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciViewYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciNotViewImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgLoginGmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagGMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEmailDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciListEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciListProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem53)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem60)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgUserAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbeNoticeAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem38)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem62)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDeviceType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgFakeIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgDcom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabProxyMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOBCv2Proxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem49)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem50)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem45)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem54)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem65)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgProxyFree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgFreeProxySub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgAddonGetProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgFreeProxyConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTinSoftProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem37)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem52)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem47)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem55)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgXProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciHostXProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem36)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOBCProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem35)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOBCv2Proxy_Old)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem43)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem46)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMultiProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem29)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem34)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTMProxy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem57)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem58)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProxySupplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgListDcom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDialUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem56)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResetDcomInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDcomDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem48)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChangeIp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgChangeMac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChangeMac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChangeMacTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem51)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciReportIp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgReportDomain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSaveReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView11)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3,
            this.barMain});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bsiStatus,
            this.bsiHelp,
            this.bbiRegister,
            this.bbiAbout,
            this.bsiAccount,
            this.bbiVideoDemo,
            this.bbiUserAgent,
            this.bbiUpdate,
            this.bbiClose,
            this.bbiNotification,
            this.bbiGUI,
            this.cbeLang,
            this.barStaticItem1,
            this.bsiFanpage,
            this.bbiBuy,
            this.bbiPrice,
            this.bbiFirefoxProfile,
            this.bbiTest,
            this.bbiDcomSetting,
            this.bbiDisableIPv6,
            this.bbiRegisterTinsoft,
            this.bsiUserInfo,
            this.bbiChangePass,
            this.bsiGUI,
            this.bbiLogin,
            this.bbiLogout,
            this.bbiResgisterAccount,
            this.bbiAcctiveAccount,
            this.bbiCreateProfile,
            this.bbiCreateProfileMain,
            this.bbiUpdateVersion,
            this.bbiBuyVPS});
            this.barManager1.MaxItemId = 35;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.bsieStatus,
            this.repositoryItemRadioGroup1,
            this.cbeeLang});
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bsiStatus, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // bsiStatus
            // 
            this.bsiStatus.AutoFillWidth = true;
            this.bsiStatus.Caption = "Sẳn sàng";
            this.bsiStatus.Edit = this.bsieStatus;
            this.bsiStatus.Id = 0;
            this.bsiStatus.Name = "bsiStatus";
            // 
            // bsieStatus
            // 
            this.bsieStatus.EndColor = System.Drawing.Color.SpringGreen;
            this.bsieStatus.Name = "bsieStatus";
            this.bsieStatus.StartColor = System.Drawing.Color.Blue;
            // 
            // barMain
            // 
            this.barMain.BarName = "Custom 3";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bsiAccount, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bbiCreateProfileMain, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bsiGUI, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bbiBuy, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bbiClose, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiBuyVPS, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiFanpage, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.cbeLang, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barMain.OptionsBar.UseWholeRow = true;
            this.barMain.Text = "Custom 3";
            // 
            // bsiAccount
            // 
            this.bsiAccount.Caption = "Tài khoản";
            this.bsiAccount.Id = 5;
            this.bsiAccount.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiUserInfo),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiLogin),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiChangePass),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiResgisterAccount),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiAcctiveAccount),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiLogout)});
            this.bsiAccount.Name = "bsiAccount";
            // 
            // bsiUserInfo
            // 
            this.bsiUserInfo.Caption = "Chưa đăng nhập";
            this.bsiUserInfo.Id = 23;
            this.bsiUserInfo.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Blue;
            this.bsiUserInfo.ItemAppearance.Normal.Options.UseForeColor = true;
            this.bsiUserInfo.Name = "bsiUserInfo";
            // 
            // bbiLogin
            // 
            this.bbiLogin.Caption = "Đăng nhập";
            this.bbiLogin.Id = 26;
            this.bbiLogin.Name = "bbiLogin";
            this.bbiLogin.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiLogin_ItemClick);
            // 
            // bbiChangePass
            // 
            this.bbiChangePass.Caption = "Đổi mật khẩu";
            this.bbiChangePass.Id = 24;
            this.bbiChangePass.Name = "bbiChangePass";
            this.bbiChangePass.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiChangePass_ItemClick);
            // 
            // bbiResgisterAccount
            // 
            this.bbiResgisterAccount.Caption = "Đăng ký tài khoản";
            this.bbiResgisterAccount.Id = 28;
            this.bbiResgisterAccount.Name = "bbiResgisterAccount";
            this.bbiResgisterAccount.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiResgisterAccount_ItemClick);
            // 
            // bbiAcctiveAccount
            // 
            this.bbiAcctiveAccount.Caption = "Kích hoạt tài khoản";
            this.bbiAcctiveAccount.Id = 29;
            this.bbiAcctiveAccount.Name = "bbiAcctiveAccount";
            this.bbiAcctiveAccount.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiAcctiveAccount_ItemClick);
            // 
            // bbiLogout
            // 
            this.bbiLogout.Caption = "Đăng xuất";
            this.bbiLogout.Id = 27;
            this.bbiLogout.Name = "bbiLogout";
            this.bbiLogout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiLogout_ItemClick);
            // 
            // bbiCreateProfileMain
            // 
            this.bbiCreateProfileMain.Caption = "Tạo profile và Login Gmail";
            this.bbiCreateProfileMain.Id = 31;
            this.bbiCreateProfileMain.Name = "bbiCreateProfileMain";
            toolTipTitleItem1.Text = "Tạo profile";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Tạo profile có sẵn. Các profile này sẽ sống mãi và được sử dụng khi thiết lập \'Tỷ" +
    " lệ % mở profile có sẵn\". Khi tool mở những profile có sẵn này thì sẽ có Cookie," +
    " Cache, v..v";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.bbiCreateProfileMain.SuperTip = superToolTip1;
            this.bbiCreateProfileMain.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiCreateProfile_ItemClick);
            // 
            // bsiGUI
            // 
            this.bsiGUI.Caption = "Hướng dẫn";
            this.bsiGUI.Id = 25;
            this.bsiGUI.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiUpdateVersion),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiUserAgent),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiVideoDemo),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiUpdate),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiGUI),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiFirefoxProfile),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiDcomSetting),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiDisableIPv6),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiRegisterTinsoft),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiAbout)});
            this.bsiGUI.Name = "bsiGUI";
            toolTipTitleItem2.Text = "Hướng dẫn sử dụng";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Thông tin chi tiết hướng dẫn sử dụng Phần mềm NATECH SEO";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.bsiGUI.SuperTip = superToolTip2;
            // 
            // bbiUpdateVersion
            // 
            this.bbiUpdateVersion.Caption = "Cập nhật";
            this.bbiUpdateVersion.Id = 33;
            this.bbiUpdateVersion.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.bbiUpdateVersion.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Blue;
            this.bbiUpdateVersion.ItemAppearance.Normal.Options.UseFont = true;
            this.bbiUpdateVersion.ItemAppearance.Normal.Options.UseForeColor = true;
            this.bbiUpdateVersion.Name = "bbiUpdateVersion";
            this.bbiUpdateVersion.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUpdateVersion_ItemClick);
            // 
            // bbiUserAgent
            // 
            this.bbiUserAgent.Caption = "Hướng dẫn cài đặt Addon lấy User Agent";
            this.bbiUserAgent.Id = 7;
            this.bbiUserAgent.Name = "bbiUserAgent";
            this.bbiUserAgent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUserAgent_ItemClick);
            // 
            // bbiVideoDemo
            // 
            this.bbiVideoDemo.Caption = "Video demo";
            this.bbiVideoDemo.Id = 6;
            this.bbiVideoDemo.Name = "bbiVideoDemo";
            this.bbiVideoDemo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiVideoDemo_ItemClick);
            // 
            // bbiUpdate
            // 
            this.bbiUpdate.Caption = "Bản cập nhật mới dùng GoLogin";
            this.bbiUpdate.Id = 8;
            this.bbiUpdate.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Blue;
            this.bbiUpdate.ItemAppearance.Normal.Options.UseForeColor = true;
            this.bbiUpdate.ItemInMenuAppearance.Normal.ForeColor = System.Drawing.Color.Blue;
            this.bbiUpdate.ItemInMenuAppearance.Normal.Options.UseForeColor = true;
            this.bbiUpdate.Name = "bbiUpdate";
            this.bbiUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiUpdate_ItemClick);
            // 
            // bbiGUI
            // 
            this.bbiGUI.Caption = "Tài liệu hướng dẫn sử dụng";
            this.bbiGUI.Id = 11;
            this.bbiGUI.Name = "bbiGUI";
            this.bbiGUI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiGUI_ItemClick_1);
            // 
            // bbiFirefoxProfile
            // 
            this.bbiFirefoxProfile.Caption = "Hướng dẫn tạo và đăng nhập Gmail bằng Firefox Profile";
            this.bbiFirefoxProfile.Id = 18;
            this.bbiFirefoxProfile.Name = "bbiFirefoxProfile";
            this.bbiFirefoxProfile.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.bbiFirefoxProfile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiFirefoxProfile_ItemClick);
            // 
            // bbiDcomSetting
            // 
            this.bbiDcomSetting.Caption = "Hướng dẫn cài đặt và cấu hình DCOM 3G/4G";
            this.bbiDcomSetting.Id = 20;
            this.bbiDcomSetting.Name = "bbiDcomSetting";
            this.bbiDcomSetting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiDcomSetting_ItemClick);
            // 
            // bbiDisableIPv6
            // 
            this.bbiDisableIPv6.Caption = "Hướng dẫn tắt IPv6 để sử dụng Tinsoft Proxy";
            this.bbiDisableIPv6.Id = 21;
            this.bbiDisableIPv6.Name = "bbiDisableIPv6";
            this.bbiDisableIPv6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiDisableIPv6_ItemClick);
            // 
            // bbiRegisterTinsoft
            // 
            this.bbiRegisterTinsoft.Caption = "Hướng dẫn đăng ký Tinsoft Proxy và cách sử dụng";
            this.bbiRegisterTinsoft.Id = 22;
            this.bbiRegisterTinsoft.Name = "bbiRegisterTinsoft";
            this.bbiRegisterTinsoft.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiRegisterTinsoft_ItemClick);
            // 
            // bbiAbout
            // 
            this.bbiAbout.Caption = "Tác giả";
            this.bbiAbout.Id = 3;
            this.bbiAbout.Name = "bbiAbout";
            this.bbiAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiAbout_ItemClick);
            // 
            // bbiBuy
            // 
            this.bbiBuy.Caption = "Mua bản quyền";
            this.bbiBuy.Id = 16;
            this.bbiBuy.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bbiBuy.ItemAppearance.Normal.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.bbiBuy.ItemAppearance.Normal.Options.UseFont = true;
            this.bbiBuy.ItemAppearance.Normal.Options.UseForeColor = true;
            this.bbiBuy.Name = "bbiBuy";
            toolTipTitleItem3.Text = "Đăng ký bản quyền";
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "Bảng giá & và thông tin đăng ký bản quyền phần mềm";
            superToolTip3.Items.Add(toolTipTitleItem3);
            superToolTip3.Items.Add(toolTipItem3);
            this.bbiBuy.SuperTip = superToolTip3;
            this.bbiBuy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiBuy_ItemClick);
            // 
            // bbiClose
            // 
            this.bbiClose.Caption = "Đóng";
            this.bbiClose.Id = 9;
            this.bbiClose.Name = "bbiClose";
            toolTipTitleItem4.Text = "Thoát";
            toolTipItem4.LeftIndent = 6;
            toolTipItem4.Text = "Tắt phần mềm";
            superToolTip4.Items.Add(toolTipTitleItem4);
            superToolTip4.Items.Add(toolTipItem4);
            this.bbiClose.SuperTip = superToolTip4;
            this.bbiClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiClose_ItemClick);
            // 
            // bbiBuyVPS
            // 
            this.bbiBuyVPS.Caption = "Dùng thử VPS";
            this.bbiBuyVPS.Id = 34;
            this.bbiBuyVPS.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.bbiBuyVPS.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Fuchsia;
            this.bbiBuyVPS.ItemAppearance.Normal.Options.UseFont = true;
            this.bbiBuyVPS.ItemAppearance.Normal.Options.UseForeColor = true;
            this.bbiBuyVPS.Name = "bbiBuyVPS";
            this.bbiBuyVPS.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiBuyVPS_ItemClick);
            // 
            // bsiFanpage
            // 
            this.bsiFanpage.Caption = "Bản quyền duy nhất tại na.com.vn";
            this.bsiFanpage.Id = 15;
            this.bsiFanpage.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bsiFanpage.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Red;
            this.bsiFanpage.ItemAppearance.Normal.Options.UseFont = true;
            this.bsiFanpage.ItemAppearance.Normal.Options.UseForeColor = true;
            this.bsiFanpage.Name = "bsiFanpage";
            this.bsiFanpage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bsiFanpage_ItemClick);
            // 
            // cbeLang
            // 
            this.cbeLang.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.cbeLang.Edit = this.cbeeLang;
            this.cbeLang.EditValue = "Tiếng Việt";
            this.cbeLang.EditWidth = 167;
            this.cbeLang.Id = 13;
            this.cbeLang.Name = "cbeLang";
            this.cbeLang.EditValueChanged += new System.EventHandler(this.cbeLang_EditValueChanged);
            // 
            // cbeeLang
            // 
            this.cbeeLang.AutoHeight = false;
            this.cbeeLang.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeeLang.Items.AddRange(new object[] {
            "Tiếng Việt",
            "English"});
            this.cbeeLang.Name = "cbeeLang";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1276, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 652);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1276, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 623);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1276, 29);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 623);
            // 
            // bsiHelp
            // 
            this.bsiHelp.Caption = "Hệ thống";
            this.bsiHelp.Id = 1;
            this.bsiHelp.Name = "bsiHelp";
            this.bsiHelp.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // bbiRegister
            // 
            this.bbiRegister.Caption = "Đăng ký bản quyền";
            this.bbiRegister.Id = 2;
            this.bbiRegister.Name = "bbiRegister";
            this.bbiRegister.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiRegister_ItemClick);
            // 
            // bbiNotification
            // 
            this.bbiNotification.Caption = "Nhận thông báo";
            this.bbiNotification.Id = 10;
            this.bbiNotification.Name = "bbiNotification";
            this.bbiNotification.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.bbiNotification.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiNotification_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 14;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // bbiPrice
            // 
            this.bbiPrice.Caption = "Bảng giá";
            this.bbiPrice.Id = 17;
            this.bbiPrice.Name = "bbiPrice";
            // 
            // bbiTest
            // 
            this.bbiTest.Caption = "Test";
            this.bbiTest.Id = 19;
            this.bbiTest.Name = "bbiTest";
            this.bbiTest.Visibility = DevExpress.XtraBars.BarItemVisibility.OnlyInCustomizing;
            this.bbiTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiTest_ItemClick);
            // 
            // bbiCreateProfile
            // 
            this.bbiCreateProfile.Caption = "Tạo Profile";
            this.bbiCreateProfile.Id = 30;
            this.bbiCreateProfile.Name = "bbiCreateProfile";
            this.bbiCreateProfile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiCreateProfile_ItemClick);
            // 
            // repositoryItemRadioGroup1
            // 
            this.repositoryItemRadioGroup1.Columns = 2;
            this.repositoryItemRadioGroup1.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Tiếng Việt"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Tiếng Anh")});
            this.repositoryItemRadioGroup1.Name = "repositoryItemRadioGroup1";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tmSaveHistory
            // 
            this.tmSaveHistory.Enabled = true;
            this.tmSaveHistory.Interval = 600000;
            this.tmSaveHistory.Tick += new System.EventHandler(this.tmSaveHistory_Tick);
            // 
            // tmChangeMAC
            // 
            this.tmChangeMAC.Interval = 1000;
            this.tmChangeMAC.Tick += new System.EventHandler(this.tmChangeMAC_Tick);
            // 
            // tmAutoStart
            // 
            this.tmAutoStart.Interval = 1000;
            this.tmAutoStart.Tick += new System.EventHandler(this.tmAutoStart_Tick);
            // 
            // tmResetDcom
            // 
            this.tmResetDcom.Interval = 1000;
            this.tmResetDcom.Tick += new System.EventHandler(this.tmResetDcom_Tick);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            // 
            // tmCanhBao
            // 
            this.tmCanhBao.Enabled = true;
            this.tmCanhBao.Interval = 1000;
            this.tmCanhBao.Tick += new System.EventHandler(this.tmCanhBao_Tick);
            // 
            // btnRun
            // 
            this.btnRun.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRun.Appearance.Options.UseFont = true;
            this.btnRun.Location = new System.Drawing.Point(367, 569);
            this.btnRun.MinimumSize = new System.Drawing.Size(0, 30);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(178, 51);
            this.btnRun.StyleController = this.lcMain;
            this.btnRun.TabIndex = 34;
            this.btnRun.Text = "Bắt đầu";
            this.btnRun.ToolTip = "Click vào đây rồi ngồi chơi xơi nước nhé!";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // lcMain
            // 
            this.lcMain.Controls.Add(this.mmeHistory);
            this.lcMain.Controls.Add(this.lueDeviceType);
            this.lcMain.Controls.Add(this.ccbBrowserLanguage);
            this.lcMain.Controls.Add(this.ceiClearChrome);
            this.lcMain.Controls.Add(this.ceiCallPhoneZalo);
            this.lcMain.Controls.Add(this.speClearChromeTime);
            this.lcMain.Controls.Add(this.ceiViewFilm);
            this.lcMain.Controls.Add(this.lueDisplayMode);
            this.lcMain.Controls.Add(this.btnTinsoftImportExcel);
            this.lcMain.Controls.Add(this.btnTinsoftExportExcel);
            this.lcMain.Controls.Add(this.ceiStarupWindow);
            this.lcMain.Controls.Add(this.speDCOMProxyDelay);
            this.lcMain.Controls.Add(this.speSpeedKeyboard);
            this.lcMain.Controls.Add(this.speInternalCount);
            this.lcMain.Controls.Add(this.btnSyncUserAgent);
            this.lcMain.Controls.Add(this.speLoadProfilePercent);
            this.lcMain.Controls.Add(this.ceiViewYoutube);
            this.lcMain.Controls.Add(this.grdTMProxy);
            this.lcMain.Controls.Add(this.btnTMProxyHome);
            this.lcMain.Controls.Add(this.speDcomDelay);
            this.lcMain.Controls.Add(this.speResetDcomInterval);
            this.lcMain.Controls.Add(this.radDcomTypeReset);
            this.lcMain.Controls.Add(this.btnSetupDCOM);
            this.lcMain.Controls.Add(this.cbeProxySupplier);
            this.lcMain.Controls.Add(this.btnCreateProfile);
            this.lcMain.Controls.Add(this.btnFirefoxProfile);
            this.lcMain.Controls.Add(this.grdTinsoft);
            this.lcMain.Controls.Add(this.lbeFakeIpNoticMacAddress);
            this.lcMain.Controls.Add(this.memOBCV3Proxy);
            this.lcMain.Controls.Add(this.txtOBCV3Host);
            this.lcMain.Controls.Add(this.ceiSaveReport);
            this.lcMain.Controls.Add(this.grdOBCV2);
            this.lcMain.Controls.Add(this.btnOBCV2HomePage);
            this.lcMain.Controls.Add(this.txtOBCV2Host);
            this.lcMain.Controls.Add(this.btnConnectOBCV2);
            this.lcMain.Controls.Add(this.ceiAutoStart);
            this.lcMain.Controls.Add(this.speSubLinkViewTo);
            this.lcMain.Controls.Add(this.speOtherSiteViewTimeTo);
            this.lcMain.Controls.Add(this.speTimeViewSearchTo);
            this.lcMain.Controls.Add(this.btnGetAgent);
            this.lcMain.Controls.Add(this.btnRegisterTinsoft);
            this.lcMain.Controls.Add(this.btnDisableIPv6);
            this.lcMain.Controls.Add(this.btnTinSoftHomepage);
            this.lcMain.Controls.Add(this.btnXProxyHomepage);
            this.lcMain.Controls.Add(this.btnOBCHomePage);
            this.lcMain.Controls.Add(this.btnMultiProxyConnect);
            this.lcMain.Controls.Add(this.grdMultiOBC);
            this.lcMain.Controls.Add(this.grdMultiXProxy);
            this.lcMain.Controls.Add(this.radMultiProxyType);
            this.lcMain.Controls.Add(this.grdMultiProxy);
            this.lcMain.Controls.Add(this.btnClearAllKeyword);
            this.lcMain.Controls.Add(this.btnClearSelectionKeyword);
            this.lcMain.Controls.Add(this.grdOBC);
            this.lcMain.Controls.Add(this.btnConnectOBC);
            this.lcMain.Controls.Add(this.txtOBCHost);
            this.lcMain.Controls.Add(this.btnImportKeyword);
            this.lcMain.Controls.Add(this.btnExportKeyword);
            this.lcMain.Controls.Add(this.grdKeyword);
            this.lcMain.Controls.Add(this.btnExcelIp);
            this.lcMain.Controls.Add(this.btnExcelDomain);
            this.lcMain.Controls.Add(this.memReChuot);
            this.lcMain.Controls.Add(this.grdXProxyList);
            this.lcMain.Controls.Add(this.btnConnectxProxy);
            this.lcMain.Controls.Add(this.txtXProxyHost);
            this.lcMain.Controls.Add(this.lbeNoticeFreeProxy);
            this.lcMain.Controls.Add(this.btnChangeMAC);
            this.lcMain.Controls.Add(this.speMACAddressInterval);
            this.lcMain.Controls.Add(this.ceiChangeMACAddress);
            this.lcMain.Controls.Add(this.speSubLinkView);
            this.lcMain.Controls.Add(this.grdIp);
            this.lcMain.Controls.Add(this.speSoLuong);
            this.lcMain.Controls.Add(this.memProfile);
            this.lcMain.Controls.Add(this.radGMail);
            this.lcMain.Controls.Add(this.lbeNoticeTinsoft);
            this.lcMain.Controls.Add(this.btnDeleteHistoryXml);
            this.lcMain.Controls.Add(this.btnLoadHistory);
            this.lcMain.Controls.Add(this.grdHistory);
            this.lcMain.Controls.Add(this.memProxyNote);
            this.lcMain.Controls.Add(this.memProxy);
            this.lcMain.Controls.Add(this.lbeUserAgentNotice);
            this.lcMain.Controls.Add(this.raiTypeProxy);
            this.lcMain.Controls.Add(this.txtDialUp);
            this.lcMain.Controls.Add(this.btnHomepage);
            this.lcMain.Controls.Add(this.radTypeIp);
            this.lcMain.Controls.Add(this.cbeGoogleSite);
            this.lcMain.Controls.Add(this.speTimeout);
            this.lcMain.Controls.Add(this.speEmailDelay);
            this.lcMain.Controls.Add(this.btnDeleteHistory);
            this.lcMain.Controls.Add(this.memEmail);
            this.lcMain.Controls.Add(this.btnSave);
            this.lcMain.Controls.Add(this.ceiNotViewImage);
            this.lcMain.Controls.Add(this.ceiUseHistory);
            this.lcMain.Controls.Add(this.speTimeViewTo);
            this.lcMain.Controls.Add(this.speTimeViewSearch);
            this.lcMain.Controls.Add(this.txtOtherSiteUrl);
            this.lcMain.Controls.Add(this.speOtherSiteViewTime);
            this.lcMain.Controls.Add(this.ceiViewOtherSite);
            this.lcMain.Controls.Add(this.txtAgent);
            this.lcMain.Controls.Add(this.speSoTrang);
            this.lcMain.Controls.Add(this.speSumClick);
            this.lcMain.Controls.Add(this.speTimeViewFrom);
            this.lcMain.Controls.Add(this.btnStop);
            this.lcMain.Controls.Add(this.btnRun);
            this.lcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcMain.Location = new System.Drawing.Point(0, 29);
            this.lcMain.Name = "lcMain";
            this.lcMain.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(470, 341, 250, 350);
            this.lcMain.Root = this.layoutControlGroup1;
            this.lcMain.Size = new System.Drawing.Size(1276, 623);
            this.lcMain.TabIndex = 0;
            this.lcMain.Text = "layoutControl1";
            // 
            // mmeHistory
            // 
            this.mmeHistory.Location = new System.Drawing.Point(8, 268);
            this.mmeHistory.Name = "mmeHistory";
            this.mmeHistory.Properties.ReadOnly = true;
            this.mmeHistory.Size = new System.Drawing.Size(1260, 290);
            this.mmeHistory.StyleController = this.lcMain;
            this.mmeHistory.TabIndex = 54;
            this.mmeHistory.ToolTip = "Ghi lại lịch sử quá trình thao tác";
            // 
            // lueDeviceType
            // 
            this.lueDeviceType.Location = new System.Drawing.Point(1154, 27);
            this.lueDeviceType.Margin = new System.Windows.Forms.Padding(2);
            this.lueDeviceType.MenuManager = this.barManager1;
            this.lueDeviceType.Name = "lueDeviceType";
            this.lueDeviceType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueDeviceType.Properties.DisplayMember = "NAME";
            this.lueDeviceType.Properties.NullText = "";
            this.lueDeviceType.Properties.ValueMember = "ID";
            this.lueDeviceType.Properties.View = this.luevDeviceType;
            this.lueDeviceType.Size = new System.Drawing.Size(117, 20);
            this.lueDeviceType.StyleController = this.lcMain;
            this.lueDeviceType.TabIndex = 172;
            this.lueDeviceType.EditValueChanged += new System.EventHandler(this.lueDeviceType_EditValueChanged);
            // 
            // luevDeviceType
            // 
            this.luevDeviceType.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.luevDeviceType_NAME});
            this.luevDeviceType.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.luevDeviceType.Name = "luevDeviceType";
            this.luevDeviceType.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.luevDeviceType.OptionsView.ShowGroupPanel = false;
            // 
            // luevDeviceType_NAME
            // 
            this.luevDeviceType_NAME.Caption = "Loại thiết bị";
            this.luevDeviceType_NAME.FieldName = "NAME";
            this.luevDeviceType_NAME.Name = "luevDeviceType_NAME";
            this.luevDeviceType_NAME.Visible = true;
            this.luevDeviceType_NAME.VisibleIndex = 0;
            // 
            // ccbBrowserLanguage
            // 
            this.ccbBrowserLanguage.EditValue = "";
            this.ccbBrowserLanguage.Location = new System.Drawing.Point(1130, 160);
            this.ccbBrowserLanguage.Margin = new System.Windows.Forms.Padding(2);
            this.ccbBrowserLanguage.MenuManager = this.barManager1;
            this.ccbBrowserLanguage.Name = "ccbBrowserLanguage";
            this.ccbBrowserLanguage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.ccbBrowserLanguage.Properties.DisplayMember = "NAME";
            this.ccbBrowserLanguage.Properties.ValueMember = "ID";
            this.ccbBrowserLanguage.Size = new System.Drawing.Size(138, 20);
            this.ccbBrowserLanguage.StyleController = this.lcMain;
            this.ccbBrowserLanguage.TabIndex = 171;
            // 
            // ceiClearChrome
            // 
            this.ceiClearChrome.Location = new System.Drawing.Point(899, 184);
            this.ceiClearChrome.Margin = new System.Windows.Forms.Padding(2);
            this.ceiClearChrome.MenuManager = this.barManager1;
            this.ceiClearChrome.Name = "ceiClearChrome";
            this.ceiClearChrome.Properties.Caption = "";
            this.ceiClearChrome.Size = new System.Drawing.Size(119, 19);
            this.ceiClearChrome.StyleController = this.lcMain;
            this.ceiClearChrome.TabIndex = 170;
            // 
            // ceiCallPhoneZalo
            // 
            this.ceiCallPhoneZalo.Location = new System.Drawing.Point(757, 211);
            this.ceiCallPhoneZalo.Margin = new System.Windows.Forms.Padding(2);
            this.ceiCallPhoneZalo.MenuManager = this.barManager1;
            this.ceiCallPhoneZalo.Name = "ceiCallPhoneZalo";
            this.ceiCallPhoneZalo.Properties.Caption = "";
            this.ceiCallPhoneZalo.Size = new System.Drawing.Size(116, 19);
            this.ceiCallPhoneZalo.StyleController = this.lcMain;
            this.ceiCallPhoneZalo.TabIndex = 169;
            // 
            // speClearChromeTime
            // 
            this.speClearChromeTime.EditValue = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.speClearChromeTime.Location = new System.Drawing.Point(1180, 184);
            this.speClearChromeTime.Margin = new System.Windows.Forms.Padding(2);
            this.speClearChromeTime.MenuManager = this.barManager1;
            this.speClearChromeTime.Name = "speClearChromeTime";
            this.speClearChromeTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.speClearChromeTime.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.speClearChromeTime.Properties.MinValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.speClearChromeTime.Size = new System.Drawing.Size(88, 20);
            this.speClearChromeTime.StyleController = this.lcMain;
            this.speClearChromeTime.TabIndex = 168;
            // 
            // ceiViewFilm
            // 
            this.ceiViewFilm.Location = new System.Drawing.Point(397, 211);
            this.ceiViewFilm.Margin = new System.Windows.Forms.Padding(2);
            this.ceiViewFilm.MenuManager = this.barManager1;
            this.ceiViewFilm.Name = "ceiViewFilm";
            this.ceiViewFilm.Properties.Caption = "";
            this.ceiViewFilm.Size = new System.Drawing.Size(239, 19);
            this.ceiViewFilm.StyleController = this.lcMain;
            this.ceiViewFilm.TabIndex = 167;
            // 
            // lueDisplayMode
            // 
            this.lueDisplayMode.EditValue = 1;
            this.lueDisplayMode.Location = new System.Drawing.Point(849, 160);
            this.lueDisplayMode.Margin = new System.Windows.Forms.Padding(2);
            this.lueDisplayMode.MenuManager = this.barManager1;
            this.lueDisplayMode.Name = "lueDisplayMode";
            this.lueDisplayMode.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.lueDisplayMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueDisplayMode.Properties.DisplayMember = "NAME";
            this.lueDisplayMode.Properties.ImmediatePopup = true;
            this.lueDisplayMode.Properties.NullText = "";
            this.lueDisplayMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lueDisplayMode.Properties.ValueMember = "ID";
            this.lueDisplayMode.Properties.View = this.gridLookUpEdit1View;
            this.lueDisplayMode.Size = new System.Drawing.Size(169, 20);
            this.lueDisplayMode.StyleController = this.lcMain;
            this.lueDisplayMode.TabIndex = 166;
            this.lueDisplayMode.EditValueChanged += new System.EventHandler(this.lueDisplayMode_EditValueChanged);
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.luevDisplayMode_NAME});
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // luevDisplayMode_NAME
            // 
            this.luevDisplayMode_NAME.Caption = "Display Mode Name";
            this.luevDisplayMode_NAME.FieldName = "NAME";
            this.luevDisplayMode_NAME.Name = "luevDisplayMode_NAME";
            this.luevDisplayMode_NAME.Visible = true;
            this.luevDisplayMode_NAME.VisibleIndex = 0;
            // 
            // btnTinsoftImportExcel
            // 
            this.btnTinsoftImportExcel.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnTinsoftImportExcel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.btnTinsoftImportExcel.Appearance.Options.UseFont = true;
            this.btnTinsoftImportExcel.Appearance.Options.UseForeColor = true;
            this.btnTinsoftImportExcel.Location = new System.Drawing.Point(853, 339);
            this.btnTinsoftImportExcel.Margin = new System.Windows.Forms.Padding(2);
            this.btnTinsoftImportExcel.Name = "btnTinsoftImportExcel";
            this.btnTinsoftImportExcel.Size = new System.Drawing.Size(204, 22);
            this.btnTinsoftImportExcel.StyleController = this.lcMain;
            this.btnTinsoftImportExcel.TabIndex = 164;
            this.btnTinsoftImportExcel.Text = "Nhập từ Excel";
            this.btnTinsoftImportExcel.Click += new System.EventHandler(this.btnTinsoftImportExcel_Click);
            // 
            // btnTinsoftExportExcel
            // 
            this.btnTinsoftExportExcel.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnTinsoftExportExcel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.btnTinsoftExportExcel.Appearance.Options.UseFont = true;
            this.btnTinsoftExportExcel.Appearance.Options.UseForeColor = true;
            this.btnTinsoftExportExcel.Location = new System.Drawing.Point(645, 339);
            this.btnTinsoftExportExcel.Margin = new System.Windows.Forms.Padding(2);
            this.btnTinsoftExportExcel.Name = "btnTinsoftExportExcel";
            this.btnTinsoftExportExcel.Size = new System.Drawing.Size(204, 22);
            this.btnTinsoftExportExcel.StyleController = this.lcMain;
            this.btnTinsoftExportExcel.TabIndex = 163;
            this.btnTinsoftExportExcel.Text = "Xuất Excel";
            this.btnTinsoftExportExcel.Click += new System.EventHandler(this.btnTinsoftExportExcel_Click);
            // 
            // ceiStarupWindow
            // 
            this.ceiStarupWindow.Location = new System.Drawing.Point(391, 184);
            this.ceiStarupWindow.Margin = new System.Windows.Forms.Padding(2);
            this.ceiStarupWindow.MenuManager = this.barManager1;
            this.ceiStarupWindow.Name = "ceiStarupWindow";
            this.ceiStarupWindow.Properties.Caption = "";
            this.ceiStarupWindow.Size = new System.Drawing.Size(127, 19);
            this.ceiStarupWindow.StyleController = this.lcMain;
            this.ceiStarupWindow.TabIndex = 162;
            // 
            // speDCOMProxyDelay
            // 
            this.speDCOMProxyDelay.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.speDCOMProxyDelay.Location = new System.Drawing.Point(800, 323);
            this.speDCOMProxyDelay.Margin = new System.Windows.Forms.Padding(2);
            this.speDCOMProxyDelay.MenuManager = this.barManager1;
            this.speDCOMProxyDelay.Name = "speDCOMProxyDelay";
            this.speDCOMProxyDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.speDCOMProxyDelay.Size = new System.Drawing.Size(150, 20);
            this.speDCOMProxyDelay.StyleController = this.lcMain;
            this.speDCOMProxyDelay.TabIndex = 160;
            // 
            // speSpeedKeyboard
            // 
            this.speSpeedKeyboard.EditValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.speSpeedKeyboard.Location = new System.Drawing.Point(776, 46);
            this.speSpeedKeyboard.Margin = new System.Windows.Forms.Padding(2);
            this.speSpeedKeyboard.MenuManager = this.barManager1;
            this.speSpeedKeyboard.Name = "speSpeedKeyboard";
            this.speSpeedKeyboard.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.speSpeedKeyboard.Size = new System.Drawing.Size(494, 20);
            this.speSpeedKeyboard.StyleController = this.lcMain;
            toolTipTitleItem5.Text = "Độ trễ gõ phím";
            toolTipItem5.LeftIndent = 6;
            toolTipItem5.Text = "Độ trễ gõ phím trên trang tìm kiếm (Google, Youtube, ...)";
            superToolTip5.Items.Add(toolTipTitleItem5);
            superToolTip5.Items.Add(toolTipItem5);
            this.speSpeedKeyboard.SuperTip = superToolTip5;
            this.speSpeedKeyboard.TabIndex = 159;
            // 
            // speInternalCount
            // 
            this.speInternalCount.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speInternalCount.Location = new System.Drawing.Point(632, 114);
            this.speInternalCount.MenuManager = this.barManager1;
            this.speInternalCount.Name = "speInternalCount";
            this.speInternalCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speInternalCount.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.speInternalCount.Size = new System.Drawing.Size(131, 20);
            this.speInternalCount.StyleController = this.lcMain;
            toolTipTitleItem6.Text = "Số lần click vào Internal Link sau khi duyệt trang chính.";
            toolTipItem6.LeftIndent = 6;
            toolTipItem6.Text = "Sau khi vào trang chính và duyệt xong. Phần mềm sẽ click tiếp vào các Internal li" +
    "nk một cách ngẫu nhiên để tăng tính tương tác tự nhiên.";
            superToolTip6.Items.Add(toolTipTitleItem6);
            superToolTip6.Items.Add(toolTipItem6);
            this.speInternalCount.SuperTip = superToolTip6;
            this.speInternalCount.TabIndex = 158;
            this.speInternalCount.ToolTip = "Là số lần click tiếp các Internal Link sau khi duyệt trang";
            // 
            // btnSyncUserAgent
            // 
            this.btnSyncUserAgent.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnSyncUserAgent.Appearance.Options.UseForeColor = true;
            this.btnSyncUserAgent.Location = new System.Drawing.Point(730, 27);
            this.btnSyncUserAgent.Name = "btnSyncUserAgent";
            this.btnSyncUserAgent.Size = new System.Drawing.Size(178, 22);
            this.btnSyncUserAgent.StyleController = this.lcMain;
            this.btnSyncUserAgent.TabIndex = 157;
            this.btnSyncUserAgent.Text = "Tải User Agent";
            this.btnSyncUserAgent.Click += new System.EventHandler(this.btnSyncUserAgent_Click);
            // 
            // speLoadProfilePercent
            // 
            this.speLoadProfilePercent.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.speLoadProfilePercent.Location = new System.Drawing.Point(657, 184);
            this.speLoadProfilePercent.Name = "speLoadProfilePercent";
            this.speLoadProfilePercent.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.speLoadProfilePercent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speLoadProfilePercent.Properties.IsFloatValue = false;
            this.speLoadProfilePercent.Properties.Mask.EditMask = "N00";
            this.speLoadProfilePercent.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.speLoadProfilePercent.Size = new System.Drawing.Size(111, 20);
            this.speLoadProfilePercent.StyleController = this.lcMain;
            toolTipTitleItem7.Text = "Tỷ lệ % chạy profile có sẵn";
            toolTipItem7.LeftIndent = 6;
            toolTipItem7.Text = resources.GetString("toolTipItem7.Text");
            superToolTip7.Items.Add(toolTipTitleItem7);
            superToolTip7.Items.Add(toolTipItem7);
            this.speLoadProfilePercent.SuperTip = superToolTip7;
            this.speLoadProfilePercent.TabIndex = 156;
            this.speLoadProfilePercent.ToolTip = "Số lần click quảng cáo vào trang web mục tiêu";
            // 
            // ceiViewYoutube
            // 
            this.ceiViewYoutube.EditValue = true;
            this.ceiViewYoutube.Location = new System.Drawing.Point(97, 211);
            this.ceiViewYoutube.MenuManager = this.barManager1;
            this.ceiViewYoutube.Name = "ceiViewYoutube";
            this.ceiViewYoutube.Properties.Caption = "";
            this.ceiViewYoutube.Size = new System.Drawing.Size(222, 19);
            this.ceiViewYoutube.StyleController = this.lcMain;
            toolTipTitleItem8.Text = "Cick Xem Youtube";
            toolTipItem8.LeftIndent = 6;
            toolTipItem8.Text = "Nếu check thuộc tính này thì đối với các trường hợp SEO Youtube, Direct Youtube t" +
    "hì phần mềm tiến hành bấm phím \"Space\" để phát video (Nếu video đã tự động phát " +
    "thì Uncheck)";
            superToolTip8.Items.Add(toolTipTitleItem8);
            superToolTip8.Items.Add(toolTipItem8);
            this.ceiViewYoutube.SuperTip = superToolTip8;
            this.ceiViewYoutube.TabIndex = 155;
            this.ceiViewYoutube.ToolTip = "Có duyệt 1 trang web khác sau khi xem quảng cáo ở trang mục tiêu hay không";
            // 
            // grdTMProxy
            // 
            this.grdTMProxy.Location = new System.Drawing.Point(12, 349);
            this.grdTMProxy.MainView = this.grdvTMProxy;
            this.grdTMProxy.MenuManager = this.barManager1;
            this.grdTMProxy.Name = "grdTMProxy";
            this.grdTMProxy.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grdvlueTMProxy_TinhTP,
            this.grdvlueTMProxy_Type,
            this.grdvbeiTMProxy_Xoa});
            this.grdTMProxy.Size = new System.Drawing.Size(1252, 205);
            this.grdTMProxy.TabIndex = 149;
            this.grdTMProxy.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvTMProxy,
            this.gridView11});
            // 
            // grdvTMProxy
            // 
            this.grdvTMProxy.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvTMProxy_Type,
            this.grdvTMProxy_Key,
            this.grdvTMProxy_TinhTP,
            this.grdvTMProxy_Xoa});
            this.grdvTMProxy.GridControl = this.grdTMProxy;
            this.grdvTMProxy.Name = "grdvTMProxy";
            this.grdvTMProxy.NewItemRowText = "Nhập để thêm ApiKey TMProxy mới";
            this.grdvTMProxy.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.grdvTMProxy.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.grdvTMProxy.OptionsView.ShowGroupPanel = false;
            // 
            // grdvTMProxy_Type
            // 
            this.grdvTMProxy_Type.Caption = "Loại key";
            this.grdvTMProxy_Type.ColumnEdit = this.grdvlueTMProxy_Type;
            this.grdvTMProxy_Type.FieldName = "Type";
            this.grdvTMProxy_Type.Name = "grdvTMProxy_Type";
            this.grdvTMProxy_Type.Visible = true;
            this.grdvTMProxy_Type.VisibleIndex = 0;
            this.grdvTMProxy_Type.Width = 326;
            // 
            // grdvlueTMProxy_Type
            // 
            this.grdvlueTMProxy_Type.AutoHeight = false;
            this.grdvlueTMProxy_Type.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdvlueTMProxy_Type.DisplayMember = "NAME";
            this.grdvlueTMProxy_Type.Name = "grdvlueTMProxy_Type";
            this.grdvlueTMProxy_Type.NullText = "";
            this.grdvlueTMProxy_Type.ValueMember = "ID";
            this.grdvlueTMProxy_Type.View = this.gridView3;
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn11});
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Loại key";
            this.gridColumn11.FieldName = "NAME";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 0;
            // 
            // grdvTMProxy_Key
            // 
            this.grdvTMProxy_Key.Caption = "Key";
            this.grdvTMProxy_Key.FieldName = "Key";
            this.grdvTMProxy_Key.Name = "grdvTMProxy_Key";
            this.grdvTMProxy_Key.Visible = true;
            this.grdvTMProxy_Key.VisibleIndex = 1;
            this.grdvTMProxy_Key.Width = 790;
            // 
            // grdvTMProxy_TinhTP
            // 
            this.grdvTMProxy_TinhTP.Caption = "Tỉnh/TP";
            this.grdvTMProxy_TinhTP.ColumnEdit = this.grdvlueTMProxy_TinhTP;
            this.grdvTMProxy_TinhTP.FieldName = "TinhTP";
            this.grdvTMProxy_TinhTP.Name = "grdvTMProxy_TinhTP";
            this.grdvTMProxy_TinhTP.Visible = true;
            this.grdvTMProxy_TinhTP.VisibleIndex = 2;
            this.grdvTMProxy_TinhTP.Width = 298;
            // 
            // grdvlueTMProxy_TinhTP
            // 
            this.grdvlueTMProxy_TinhTP.AutoHeight = false;
            this.grdvlueTMProxy_TinhTP.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdvlueTMProxy_TinhTP.DisplayMember = "name";
            this.grdvlueTMProxy_TinhTP.Name = "grdvlueTMProxy_TinhTP";
            this.grdvlueTMProxy_TinhTP.ValueMember = "location";
            // 
            // grdvTMProxy_Xoa
            // 
            this.grdvTMProxy_Xoa.Caption = "Xóa";
            this.grdvTMProxy_Xoa.ColumnEdit = this.grdvbeiTMProxy_Xoa;
            this.grdvTMProxy_Xoa.Name = "grdvTMProxy_Xoa";
            this.grdvTMProxy_Xoa.Visible = true;
            this.grdvTMProxy_Xoa.VisibleIndex = 3;
            // 
            // grdvbeiTMProxy_Xoa
            // 
            this.grdvbeiTMProxy_Xoa.AutoHeight = false;
            this.grdvbeiTMProxy_Xoa.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Xóa", -1, true, true, false, editorButtonImageOptions1)});
            this.grdvbeiTMProxy_Xoa.Name = "grdvbeiTMProxy_Xoa";
            this.grdvbeiTMProxy_Xoa.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grdvbeiTMProxy_Xoa_ButtonClick);
            // 
            // btnTMProxyHome
            // 
            this.btnTMProxyHome.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnTMProxyHome.Appearance.Options.UseForeColor = true;
            this.btnTMProxyHome.Location = new System.Drawing.Point(12, 323);
            this.btnTMProxyHome.Name = "btnTMProxyHome";
            this.btnTMProxyHome.Size = new System.Drawing.Size(624, 22);
            this.btnTMProxyHome.StyleController = this.lcMain;
            this.btnTMProxyHome.TabIndex = 154;
            this.btnTMProxyHome.Text = "Trang chủ TM Proxy";
            this.btnTMProxyHome.Click += new System.EventHandler(this.btnTMProxyHome_Click);
            // 
            // speDcomDelay
            // 
            this.speDcomDelay.EditValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.speDcomDelay.Location = new System.Drawing.Point(664, 211);
            this.speDcomDelay.MenuManager = this.barManager1;
            this.speDcomDelay.Name = "speDcomDelay";
            this.speDcomDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speDcomDelay.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.speDcomDelay.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speDcomDelay.Size = new System.Drawing.Size(98, 20);
            this.speDcomDelay.StyleController = this.lcMain;
            this.speDcomDelay.TabIndex = 146;
            // 
            // speResetDcomInterval
            // 
            this.speResetDcomInterval.EditValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.speResetDcomInterval.Location = new System.Drawing.Point(332, 211);
            this.speResetDcomInterval.MenuManager = this.barManager1;
            this.speResetDcomInterval.Name = "speResetDcomInterval";
            this.speResetDcomInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speResetDcomInterval.Properties.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.speResetDcomInterval.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speResetDcomInterval.Size = new System.Drawing.Size(177, 20);
            this.speResetDcomInterval.StyleController = this.lcMain;
            this.speResetDcomInterval.TabIndex = 147;
            // 
            // radDcomTypeReset
            // 
            this.radDcomTypeReset.EditValue = 1;
            this.radDcomTypeReset.Location = new System.Drawing.Point(88, 211);
            this.radDcomTypeReset.MenuManager = this.barManager1;
            this.radDcomTypeReset.Name = "radDcomTypeReset";
            this.radDcomTypeReset.Properties.Columns = 3;
            this.radDcomTypeReset.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Reset sau 1 lượt"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Reset theo chu kỳ")});
            this.radDcomTypeReset.Size = new System.Drawing.Size(169, 25);
            this.radDcomTypeReset.StyleController = this.lcMain;
            toolTipTitleItem9.Text = "Hướng dẫn/";
            toolTipItem9.LeftIndent = 6;
            toolTipItem9.Text = "Chọn hình thức thay đổi IP.";
            superToolTip9.Items.Add(toolTipTitleItem9);
            superToolTip9.Items.Add(toolTipItem9);
            this.radDcomTypeReset.SuperTip = superToolTip9;
            this.radDcomTypeReset.TabIndex = 151;
            this.radDcomTypeReset.SelectedIndexChanged += new System.EventHandler(this.radDcomTypeReset_SelectedIndexChanged);
            // 
            // btnSetupDCOM
            // 
            this.btnSetupDCOM.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnSetupDCOM.Appearance.Options.UseForeColor = true;
            this.btnSetupDCOM.Location = new System.Drawing.Point(766, 211);
            this.btnSetupDCOM.Name = "btnSetupDCOM";
            this.btnSetupDCOM.Size = new System.Drawing.Size(248, 22);
            this.btnSetupDCOM.StyleController = this.lcMain;
            this.btnSetupDCOM.TabIndex = 139;
            this.btnSetupDCOM.Text = "Hướng dẫn thiết lập DCOM";
            this.btnSetupDCOM.Click += new System.EventHandler(this.btnSetupDCOM_Click);
            // 
            // cbeProxySupplier
            // 
            this.cbeProxySupplier.EditValue = "OBC Proxy";
            this.cbeProxySupplier.Location = new System.Drawing.Point(110, 323);
            this.cbeProxySupplier.MenuManager = this.barManager1;
            this.cbeProxySupplier.Name = "cbeProxySupplier";
            this.cbeProxySupplier.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeProxySupplier.Properties.Items.AddRange(new object[] {
            "OBC Proxy",
            "xProxy",
            "Eager Proxy",
            "SProxy",
            "MobiProxy",
            "MobiProxy v2",
            "MobiProxy v3"});
            this.cbeProxySupplier.Size = new System.Drawing.Size(212, 20);
            this.cbeProxySupplier.StyleController = this.lcMain;
            this.cbeProxySupplier.TabIndex = 148;
            this.cbeProxySupplier.SelectedIndexChanged += new System.EventHandler(this.cbeProxySupplier_SelectedIndexChanged);
            this.cbeProxySupplier.EditValueChanged += new System.EventHandler(this.cbeProxySupplier_EditValueChanged);
            // 
            // btnCreateProfile
            // 
            this.btnCreateProfile.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnCreateProfile.Appearance.Options.UseForeColor = true;
            this.btnCreateProfile.Location = new System.Drawing.Point(640, 27);
            this.btnCreateProfile.Name = "btnCreateProfile";
            this.btnCreateProfile.Size = new System.Drawing.Size(314, 22);
            this.btnCreateProfile.StyleController = this.lcMain;
            this.btnCreateProfile.TabIndex = 140;
            this.btnCreateProfile.Text = "Tạo Profile";
            this.btnCreateProfile.Click += new System.EventHandler(this.btnCreateProfile_Click);
            // 
            // btnFirefoxProfile
            // 
            this.btnFirefoxProfile.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnFirefoxProfile.Appearance.Options.UseForeColor = true;
            this.btnFirefoxProfile.Location = new System.Drawing.Point(323, 27);
            this.btnFirefoxProfile.Name = "btnFirefoxProfile";
            this.btnFirefoxProfile.Size = new System.Drawing.Size(313, 22);
            this.btnFirefoxProfile.StyleController = this.lcMain;
            this.btnFirefoxProfile.TabIndex = 140;
            this.btnFirefoxProfile.Text = "Hướng dẫn đăng nhập Gmail bằng Firefox Profile";
            this.btnFirefoxProfile.Click += new System.EventHandler(this.btnFirefoxProfile_Click);
            // 
            // grdTinsoft
            // 
            this.grdTinsoft.Location = new System.Drawing.Point(12, 365);
            this.grdTinsoft.MainView = this.grdvTinsoft;
            this.grdTinsoft.MenuManager = this.barManager1;
            this.grdTinsoft.Name = "grdTinsoft";
            this.grdTinsoft.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grdvccbTinsoft_TinhTP,
            this.grdvcbeTinsoft_Type,
            this.grdvlueTinsoft_Type,
            this.grdvbeiTinsoft_Delete,
            this.grdvlueTinsoft_Select});
            this.grdTinsoft.Size = new System.Drawing.Size(1252, 189);
            this.grdTinsoft.TabIndex = 129;
            this.grdTinsoft.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvTinsoft,
            this.gridView10});
            this.grdTinsoft.Click += new System.EventHandler(this.grdTinsoft_Click);
            this.grdTinsoft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdTinsoft_KeyDown);
            // 
            // grdvTinsoft
            // 
            this.grdvTinsoft.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvTinsoft_Type,
            this.grdvTinsoft_Key,
            this.grdvTinsoft_TinhTP,
            this.grdvTinsoft_Select,
            this.grdvTinsoft_Delete});
            this.grdvTinsoft.GridControl = this.grdTinsoft;
            this.grdvTinsoft.Name = "grdvTinsoft";
            this.grdvTinsoft.NewItemRowText = "Nhập để thêm key Tinsoft mới";
            this.grdvTinsoft.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.grdvTinsoft.OptionsView.ShowGroupPanel = false;
            this.grdvTinsoft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdvTinsoft_KeyDown);
            // 
            // grdvTinsoft_Type
            // 
            this.grdvTinsoft_Type.Caption = "Loại key";
            this.grdvTinsoft_Type.ColumnEdit = this.grdvlueTinsoft_Type;
            this.grdvTinsoft_Type.FieldName = "Type";
            this.grdvTinsoft_Type.Name = "grdvTinsoft_Type";
            this.grdvTinsoft_Type.Visible = true;
            this.grdvTinsoft_Type.VisibleIndex = 0;
            this.grdvTinsoft_Type.Width = 220;
            // 
            // grdvlueTinsoft_Type
            // 
            this.grdvlueTinsoft_Type.AutoHeight = false;
            this.grdvlueTinsoft_Type.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdvlueTinsoft_Type.DisplayMember = "NAME";
            this.grdvlueTinsoft_Type.Name = "grdvlueTinsoft_Type";
            this.grdvlueTinsoft_Type.NullText = "";
            this.grdvlueTinsoft_Type.ValueMember = "ID";
            this.grdvlueTinsoft_Type.View = this.grdvluevTinsoft_Type;
            // 
            // grdvluevTinsoft_Type
            // 
            this.grdvluevTinsoft_Type.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9});
            this.grdvluevTinsoft_Type.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdvluevTinsoft_Type.Name = "grdvluevTinsoft_Type";
            this.grdvluevTinsoft_Type.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdvluevTinsoft_Type.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Loại key";
            this.gridColumn9.FieldName = "NAME";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 0;
            // 
            // grdvTinsoft_Key
            // 
            this.grdvTinsoft_Key.Caption = "Key";
            this.grdvTinsoft_Key.FieldName = "Key";
            this.grdvTinsoft_Key.Name = "grdvTinsoft_Key";
            this.grdvTinsoft_Key.Visible = true;
            this.grdvTinsoft_Key.VisibleIndex = 1;
            this.grdvTinsoft_Key.Width = 480;
            // 
            // grdvTinsoft_TinhTP
            // 
            this.grdvTinsoft_TinhTP.Caption = "Tỉnh/TP";
            this.grdvTinsoft_TinhTP.ColumnEdit = this.grdvccbTinsoft_TinhTP;
            this.grdvTinsoft_TinhTP.FieldName = "TinhTP";
            this.grdvTinsoft_TinhTP.Name = "grdvTinsoft_TinhTP";
            this.grdvTinsoft_TinhTP.Visible = true;
            this.grdvTinsoft_TinhTP.VisibleIndex = 2;
            this.grdvTinsoft_TinhTP.Width = 199;
            // 
            // grdvccbTinsoft_TinhTP
            // 
            this.grdvccbTinsoft_TinhTP.AutoHeight = false;
            this.grdvccbTinsoft_TinhTP.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdvccbTinsoft_TinhTP.DisplayMember = "name";
            this.grdvccbTinsoft_TinhTP.Name = "grdvccbTinsoft_TinhTP";
            this.grdvccbTinsoft_TinhTP.ValueMember = "location";
            // 
            // grdvTinsoft_Select
            // 
            this.grdvTinsoft_Select.Caption = "Chọn";
            this.grdvTinsoft_Select.ColumnEdit = this.grdvlueTinsoft_Select;
            this.grdvTinsoft_Select.FieldName = "Select";
            this.grdvTinsoft_Select.Name = "grdvTinsoft_Select";
            this.grdvTinsoft_Select.Visible = true;
            this.grdvTinsoft_Select.VisibleIndex = 3;
            this.grdvTinsoft_Select.Width = 98;
            // 
            // grdvlueTinsoft_Select
            // 
            this.grdvlueTinsoft_Select.AutoHeight = false;
            this.grdvlueTinsoft_Select.Name = "grdvlueTinsoft_Select";
            // 
            // grdvTinsoft_Delete
            // 
            this.grdvTinsoft_Delete.Caption = "Xóa";
            this.grdvTinsoft_Delete.ColumnEdit = this.grdvbeiTinsoft_Delete;
            this.grdvTinsoft_Delete.Name = "grdvTinsoft_Delete";
            this.grdvTinsoft_Delete.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.grdvTinsoft_Delete.Visible = true;
            this.grdvTinsoft_Delete.VisibleIndex = 4;
            this.grdvTinsoft_Delete.Width = 59;
            // 
            // grdvbeiTinsoft_Delete
            // 
            this.grdvbeiTinsoft_Delete.AutoHeight = false;
            this.grdvbeiTinsoft_Delete.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Xóa", -1, true, true, false, editorButtonImageOptions2)});
            this.grdvbeiTinsoft_Delete.Name = "grdvbeiTinsoft_Delete";
            this.grdvbeiTinsoft_Delete.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grdvbeiTinsoft_Delete_ButtonClick);
            // 
            // grdvcbeTinsoft_Type
            // 
            this.grdvcbeTinsoft_Type.AutoHeight = false;
            this.grdvcbeTinsoft_Type.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdvcbeTinsoft_Type.Items.AddRange(new object[] {
            "Key thường",
            "Key VIP",
            "Key dùng nhanh"});
            this.grdvcbeTinsoft_Type.Name = "grdvcbeTinsoft_Type";
            // 
            // lbeFakeIpNoticMacAddress
            // 
            this.lbeFakeIpNoticMacAddress.Appearance.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Italic);
            this.lbeFakeIpNoticMacAddress.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lbeFakeIpNoticMacAddress.Appearance.Options.UseFont = true;
            this.lbeFakeIpNoticMacAddress.Appearance.Options.UseForeColor = true;
            this.lbeFakeIpNoticMacAddress.Location = new System.Drawing.Point(767, 46);
            this.lbeFakeIpNoticMacAddress.Name = "lbeFakeIpNoticMacAddress";
            this.lbeFakeIpNoticMacAddress.Size = new System.Drawing.Size(432, 12);
            this.lbeFakeIpNoticMacAddress.StyleController = this.lcMain;
            this.lbeFakeIpNoticMacAddress.TabIndex = 107;
            this.lbeFakeIpNoticMacAddress.Text = "Lưu ý: Sử dụng đổi MACAddress phải mở tool dưới quyền Adminstrator (Run as Admini" +
    "strator)";
            // 
            // memOBCV3Proxy
            // 
            this.memOBCV3Proxy.Location = new System.Drawing.Point(15, 370);
            this.memOBCV3Proxy.MenuManager = this.barManager1;
            this.memOBCV3Proxy.Name = "memOBCV3Proxy";
            this.memOBCV3Proxy.Size = new System.Drawing.Size(1246, 181);
            this.memOBCV3Proxy.StyleController = this.lcMain;
            this.memOBCV3Proxy.TabIndex = 99;
            // 
            // txtOBCV3Host
            // 
            this.txtOBCV3Host.Location = new System.Drawing.Point(382, 323);
            this.txtOBCV3Host.MenuManager = this.barManager1;
            this.txtOBCV3Host.Name = "txtOBCV3Host";
            this.txtOBCV3Host.Size = new System.Drawing.Size(254, 20);
            this.txtOBCV3Host.StyleController = this.lcMain;
            toolTipTitleItem10.Text = "Hướng dẫn";
            toolTipItem10.LeftIndent = 6;
            toolTipItem10.Text = "Địa chỉ host XProxy cung cấp";
            superToolTip10.Items.Add(toolTipTitleItem10);
            superToolTip10.Items.Add(toolTipItem10);
            this.txtOBCV3Host.SuperTip = superToolTip10;
            this.txtOBCV3Host.TabIndex = 120;
            // 
            // ceiSaveReport
            // 
            this.ceiSaveReport.Location = new System.Drawing.Point(704, 27);
            this.ceiSaveReport.MenuManager = this.barManager1;
            this.ceiSaveReport.Name = "ceiSaveReport";
            this.ceiSaveReport.Properties.Caption = "";
            this.ceiSaveReport.Size = new System.Drawing.Size(250, 19);
            this.ceiSaveReport.StyleController = this.lcMain;
            this.ceiSaveReport.TabIndex = 146;
            // 
            // grdOBCV2
            // 
            this.grdOBCV2.Location = new System.Drawing.Point(12, 349);
            this.grdOBCV2.MainView = this.grdvOBCV2;
            this.grdOBCV2.MenuManager = this.barManager1;
            this.grdOBCV2.Name = "grdOBCV2";
            this.grdOBCV2.Size = new System.Drawing.Size(1252, 205);
            this.grdOBCV2.TabIndex = 145;
            this.grdOBCV2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvOBCV2,
            this.gridView9});
            // 
            // grdvOBCV2
            // 
            this.grdvOBCV2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.grdvOBCV2.GridControl = this.grdOBCV2;
            this.grdvOBCV2.Name = "grdvOBCV2";
            this.grdvOBCV2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "No";
            this.gridColumn1.FieldName = "stt";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 117;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Public Ip";
            this.gridColumn2.FieldName = "public_ip";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 212;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "WAN IP";
            this.gridColumn3.FieldName = "system";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            this.gridColumn3.Width = 212;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Port";
            this.gridColumn4.FieldName = "proxy_port";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            this.gridColumn4.Width = 212;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Sock port";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            this.gridColumn5.Width = 172;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Proxy Full";
            this.gridColumn6.FieldName = "proxy_full";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 218;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "IMEI";
            this.gridColumn7.FieldName = "imei";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            this.gridColumn7.Width = 152;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Is Run";
            this.gridColumn8.FieldName = "IsRun";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            this.gridColumn8.Width = 119;
            // 
            // btnOBCV2HomePage
            // 
            this.btnOBCV2HomePage.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnOBCV2HomePage.Appearance.Options.UseForeColor = true;
            this.btnOBCV2HomePage.Location = new System.Drawing.Point(954, 323);
            this.btnOBCV2HomePage.Name = "btnOBCV2HomePage";
            this.btnOBCV2HomePage.Size = new System.Drawing.Size(310, 22);
            this.btnOBCV2HomePage.StyleController = this.lcMain;
            this.btnOBCV2HomePage.TabIndex = 139;
            this.btnOBCV2HomePage.Text = "Trang chủ OBC Proxy";
            this.btnOBCV2HomePage.Click += new System.EventHandler(this.btnOBCV2HomePage_Click);
            // 
            // txtOBCV2Host
            // 
            this.txtOBCV2Host.Location = new System.Drawing.Point(68, 323);
            this.txtOBCV2Host.MenuManager = this.barManager1;
            this.txtOBCV2Host.Name = "txtOBCV2Host";
            this.txtOBCV2Host.Size = new System.Drawing.Size(359, 20);
            this.txtOBCV2Host.StyleController = this.lcMain;
            toolTipTitleItem11.Text = "Hướng dẫn";
            toolTipItem11.LeftIndent = 6;
            toolTipItem11.Text = "Địa chỉ host XProxy cung cấp";
            superToolTip11.Items.Add(toolTipTitleItem11);
            superToolTip11.Items.Add(toolTipItem11);
            this.txtOBCV2Host.SuperTip = superToolTip11;
            this.txtOBCV2Host.TabIndex = 119;
            // 
            // btnConnectOBCV2
            // 
            this.btnConnectOBCV2.Location = new System.Drawing.Point(431, 323);
            this.btnConnectOBCV2.Name = "btnConnectOBCV2";
            this.btnConnectOBCV2.Size = new System.Drawing.Size(414, 22);
            this.btnConnectOBCV2.StyleController = this.lcMain;
            toolTipTitleItem12.Text = "Hướng dẫn";
            toolTipItem12.LeftIndent = 6;
            toolTipItem12.Text = "Click để tiến hành kết nối đến Service XProxy";
            superToolTip12.Items.Add(toolTipTitleItem12);
            superToolTip12.Items.Add(toolTipItem12);
            this.btnConnectOBCV2.SuperTip = superToolTip12;
            this.btnConnectOBCV2.TabIndex = 138;
            this.btnConnectOBCV2.Text = "Kết nối thử";
            this.btnConnectOBCV2.Click += new System.EventHandler(this.btnConnectOBCV2_Click);
            // 
            // ceiAutoStart
            // 
            this.ceiAutoStart.Location = new System.Drawing.Point(228, 184);
            this.ceiAutoStart.MenuManager = this.barManager1;
            this.ceiAutoStart.Name = "ceiAutoStart";
            this.ceiAutoStart.Properties.Caption = "";
            this.ceiAutoStart.Size = new System.Drawing.Size(40, 19);
            this.ceiAutoStart.StyleController = this.lcMain;
            this.ceiAutoStart.TabIndex = 144;
            // 
            // speSubLinkViewTo
            // 
            this.speSubLinkViewTo.EditValue = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.speSubLinkViewTo.Location = new System.Drawing.Point(1046, 114);
            this.speSubLinkViewTo.MenuManager = this.barManager1;
            this.speSubLinkViewTo.Name = "speSubLinkViewTo";
            this.speSubLinkViewTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speSubLinkViewTo.Size = new System.Drawing.Size(224, 20);
            this.speSubLinkViewTo.StyleController = this.lcMain;
            toolTipTitleItem13.Text = "Hướng dẫn";
            toolTipItem13.LeftIndent = 6;
            toolTipItem13.Text = "Mốc thời gian tối đa phần mềm sẽ lấy để duyệt vào các Internal Link hoặc External" +
    " Link";
            superToolTip13.Items.Add(toolTipTitleItem13);
            superToolTip13.Items.Add(toolTipItem13);
            this.speSubLinkViewTo.SuperTip = superToolTip13;
            this.speSubLinkViewTo.TabIndex = 143;
            // 
            // speOtherSiteViewTimeTo
            // 
            this.speOtherSiteViewTimeTo.EditValue = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.speOtherSiteViewTimeTo.Location = new System.Drawing.Point(665, 446);
            this.speOtherSiteViewTimeTo.MenuManager = this.barManager1;
            this.speOtherSiteViewTimeTo.Name = "speOtherSiteViewTimeTo";
            this.speOtherSiteViewTimeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speOtherSiteViewTimeTo.Size = new System.Drawing.Size(605, 20);
            this.speOtherSiteViewTimeTo.StyleController = this.lcMain;
            toolTipTitleItem14.Text = "Hướng dẫn";
            toolTipItem14.LeftIndent = 6;
            toolTipItem14.Text = "Mốc thời gian tối đa để phần mềm lấy duyệt trang khác, nếu kéo Traffic thì nên để" +
    " thời gian dài (trên 2p) để tránh tăng tỷ lệ thoát. Còn nếu để trang bất kỳ thì " +
    "có thể để thấp hơn.";
            superToolTip14.Items.Add(toolTipTitleItem14);
            superToolTip14.Items.Add(toolTipItem14);
            this.speOtherSiteViewTimeTo.SuperTip = superToolTip14;
            this.speOtherSiteViewTimeTo.TabIndex = 142;
            // 
            // speTimeViewSearchTo
            // 
            this.speTimeViewSearchTo.EditValue = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.speTimeViewSearchTo.Location = new System.Drawing.Point(982, 70);
            this.speTimeViewSearchTo.MenuManager = this.barManager1;
            this.speTimeViewSearchTo.Name = "speTimeViewSearchTo";
            this.speTimeViewSearchTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speTimeViewSearchTo.Size = new System.Drawing.Size(288, 20);
            this.speTimeViewSearchTo.StyleController = this.lcMain;
            toolTipTitleItem15.Text = "Hướng dẫn";
            toolTipItem15.LeftIndent = 6;
            toolTipItem15.Text = "Là mốc thời gian tối đa phần mềm sẽ lấy để lướt tìm kiếm trên trang Google";
            superToolTip15.Items.Add(toolTipTitleItem15);
            superToolTip15.Items.Add(toolTipItem15);
            this.speTimeViewSearchTo.SuperTip = superToolTip15;
            this.speTimeViewSearchTo.TabIndex = 141;
            // 
            // btnGetAgent
            // 
            this.btnGetAgent.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnGetAgent.Appearance.Options.UseForeColor = true;
            this.btnGetAgent.Location = new System.Drawing.Point(912, 27);
            this.btnGetAgent.Name = "btnGetAgent";
            this.btnGetAgent.Size = new System.Drawing.Size(178, 22);
            this.btnGetAgent.StyleController = this.lcMain;
            this.btnGetAgent.TabIndex = 139;
            this.btnGetAgent.Text = "Hướng dẫn lấy UserAgent";
            this.btnGetAgent.Click += new System.EventHandler(this.btnGetAgent_Click);
            // 
            // btnRegisterTinsoft
            // 
            this.btnRegisterTinsoft.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnRegisterTinsoft.Appearance.Options.UseForeColor = true;
            this.btnRegisterTinsoft.Location = new System.Drawing.Point(437, 339);
            this.btnRegisterTinsoft.Name = "btnRegisterTinsoft";
            this.btnRegisterTinsoft.Size = new System.Drawing.Size(204, 22);
            this.btnRegisterTinsoft.StyleController = this.lcMain;
            this.btnRegisterTinsoft.TabIndex = 138;
            this.btnRegisterTinsoft.Text = "Hướng dẫn sử dụng Tinsoftproxy";
            this.btnRegisterTinsoft.Click += new System.EventHandler(this.btnRegisterTinsoft_Click);
            // 
            // btnDisableIPv6
            // 
            this.btnDisableIPv6.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnDisableIPv6.Appearance.Options.UseForeColor = true;
            this.btnDisableIPv6.Location = new System.Drawing.Point(221, 339);
            this.btnDisableIPv6.Name = "btnDisableIPv6";
            this.btnDisableIPv6.Size = new System.Drawing.Size(212, 22);
            this.btnDisableIPv6.StyleController = this.lcMain;
            this.btnDisableIPv6.TabIndex = 138;
            this.btnDisableIPv6.Text = "Hướng dẫn tắt IPv6 để chạy Tinsoft Proxy";
            this.btnDisableIPv6.Click += new System.EventHandler(this.btnDisableIPv6_Click);
            // 
            // btnTinSoftHomepage
            // 
            this.btnTinSoftHomepage.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnTinSoftHomepage.Appearance.Options.UseForeColor = true;
            this.btnTinSoftHomepage.Location = new System.Drawing.Point(12, 339);
            this.btnTinSoftHomepage.Name = "btnTinSoftHomepage";
            this.btnTinSoftHomepage.Size = new System.Drawing.Size(205, 22);
            this.btnTinSoftHomepage.StyleController = this.lcMain;
            this.btnTinSoftHomepage.TabIndex = 138;
            this.btnTinSoftHomepage.Text = "Trang chủ TinSoft";
            this.btnTinSoftHomepage.Click += new System.EventHandler(this.btnTinSoftHomepage_Click);
            // 
            // btnXProxyHomepage
            // 
            this.btnXProxyHomepage.Location = new System.Drawing.Point(640, 323);
            this.btnXProxyHomepage.Name = "btnXProxyHomepage";
            this.btnXProxyHomepage.Size = new System.Drawing.Size(310, 22);
            this.btnXProxyHomepage.StyleController = this.lcMain;
            this.btnXProxyHomepage.TabIndex = 137;
            this.btnXProxyHomepage.Text = "Trang chủ XProxy";
            this.btnXProxyHomepage.Click += new System.EventHandler(this.btnXProxyHomepage_Click);
            // 
            // btnOBCHomePage
            // 
            this.btnOBCHomePage.Location = new System.Drawing.Point(640, 323);
            this.btnOBCHomePage.Name = "btnOBCHomePage";
            this.btnOBCHomePage.Size = new System.Drawing.Size(310, 22);
            this.btnOBCHomePage.StyleController = this.lcMain;
            this.btnOBCHomePage.TabIndex = 136;
            this.btnOBCHomePage.Text = "Trang chủ OBC";
            this.btnOBCHomePage.Click += new System.EventHandler(this.btnOBCHomePage_Click);
            // 
            // btnMultiProxyConnect
            // 
            this.btnMultiProxyConnect.Location = new System.Drawing.Point(431, 323);
            this.btnMultiProxyConnect.Name = "btnMultiProxyConnect";
            this.btnMultiProxyConnect.Size = new System.Drawing.Size(414, 74);
            this.btnMultiProxyConnect.StyleController = this.lcMain;
            this.btnMultiProxyConnect.TabIndex = 135;
            this.btnMultiProxyConnect.Text = "Kết nối thử";
            this.btnMultiProxyConnect.Click += new System.EventHandler(this.btnMultiProxyConnect_Click);
            // 
            // grdMultiOBC
            // 
            this.grdMultiOBC.Location = new System.Drawing.Point(642, 484);
            this.grdMultiOBC.MainView = this.grdvMultiOBC;
            this.grdMultiOBC.MenuManager = this.barManager1;
            this.grdMultiOBC.Name = "grdMultiOBC";
            this.grdMultiOBC.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grdvlueMultiOBC_IsRun});
            this.grdMultiOBC.Size = new System.Drawing.Size(622, 70);
            this.grdMultiOBC.TabIndex = 134;
            this.grdMultiOBC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvMultiOBC,
            this.gridView8});
            // 
            // grdvMultiOBC
            // 
            this.grdvMultiOBC.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvMultiOBC_name,
            this.grdvMultiOBC_idKey,
            this.grdvMultiOBC_proxyAddress,
            this.grdvMultiOBC_proxyStatus,
            this.grdvMultiOBC_port,
            this.grdvMultiOBC_sockPort,
            this.grdvMultiOBC_socksEnable,
            this.grdvMultiOBC_ServiceUrl,
            this.grdvMultiOBC_IsRun});
            this.grdvMultiOBC.GridControl = this.grdMultiOBC;
            this.grdvMultiOBC.GroupCount = 1;
            this.grdvMultiOBC.Name = "grdvMultiOBC";
            this.grdvMultiOBC.OptionsBehavior.AutoExpandAllGroups = true;
            this.grdvMultiOBC.OptionsView.ShowGroupPanel = false;
            this.grdvMultiOBC.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.grdvMultiOBC_ServiceUrl, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // grdvMultiOBC_name
            // 
            this.grdvMultiOBC_name.Caption = "Name";
            this.grdvMultiOBC_name.FieldName = "name";
            this.grdvMultiOBC_name.Name = "grdvMultiOBC_name";
            this.grdvMultiOBC_name.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_name.Visible = true;
            this.grdvMultiOBC_name.VisibleIndex = 0;
            this.grdvMultiOBC_name.Width = 108;
            // 
            // grdvMultiOBC_idKey
            // 
            this.grdvMultiOBC_idKey.Caption = "idKey";
            this.grdvMultiOBC_idKey.FieldName = "idKey";
            this.grdvMultiOBC_idKey.Name = "grdvMultiOBC_idKey";
            this.grdvMultiOBC_idKey.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_idKey.Visible = true;
            this.grdvMultiOBC_idKey.VisibleIndex = 3;
            this.grdvMultiOBC_idKey.Width = 196;
            // 
            // grdvMultiOBC_proxyAddress
            // 
            this.grdvMultiOBC_proxyAddress.Caption = "proxyAddress";
            this.grdvMultiOBC_proxyAddress.FieldName = "proxyAddress";
            this.grdvMultiOBC_proxyAddress.Name = "grdvMultiOBC_proxyAddress";
            this.grdvMultiOBC_proxyAddress.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_proxyAddress.Visible = true;
            this.grdvMultiOBC_proxyAddress.VisibleIndex = 4;
            this.grdvMultiOBC_proxyAddress.Width = 196;
            // 
            // grdvMultiOBC_proxyStatus
            // 
            this.grdvMultiOBC_proxyStatus.Caption = "proxyStatus";
            this.grdvMultiOBC_proxyStatus.FieldName = "proxyStatus";
            this.grdvMultiOBC_proxyStatus.Name = "grdvMultiOBC_proxyStatus";
            this.grdvMultiOBC_proxyStatus.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_proxyStatus.Visible = true;
            this.grdvMultiOBC_proxyStatus.VisibleIndex = 5;
            this.grdvMultiOBC_proxyStatus.Width = 196;
            // 
            // grdvMultiOBC_port
            // 
            this.grdvMultiOBC_port.Caption = "port";
            this.grdvMultiOBC_port.FieldName = "port";
            this.grdvMultiOBC_port.Name = "grdvMultiOBC_port";
            this.grdvMultiOBC_port.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_port.Visible = true;
            this.grdvMultiOBC_port.VisibleIndex = 6;
            this.grdvMultiOBC_port.Width = 199;
            // 
            // grdvMultiOBC_sockPort
            // 
            this.grdvMultiOBC_sockPort.Caption = "sockPort";
            this.grdvMultiOBC_sockPort.FieldName = "sockPort";
            this.grdvMultiOBC_sockPort.Name = "grdvMultiOBC_sockPort";
            this.grdvMultiOBC_sockPort.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_sockPort.Visible = true;
            this.grdvMultiOBC_sockPort.VisibleIndex = 2;
            this.grdvMultiOBC_sockPort.Width = 201;
            // 
            // grdvMultiOBC_socksEnable
            // 
            this.grdvMultiOBC_socksEnable.Caption = "socksEnable";
            this.grdvMultiOBC_socksEnable.FieldName = "socksEnable";
            this.grdvMultiOBC_socksEnable.Name = "grdvMultiOBC_socksEnable";
            this.grdvMultiOBC_socksEnable.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_socksEnable.Visible = true;
            this.grdvMultiOBC_socksEnable.VisibleIndex = 1;
            this.grdvMultiOBC_socksEnable.Width = 140;
            // 
            // grdvMultiOBC_ServiceUrl
            // 
            this.grdvMultiOBC_ServiceUrl.Caption = "Service Url";
            this.grdvMultiOBC_ServiceUrl.FieldName = "ServiceUrl";
            this.grdvMultiOBC_ServiceUrl.Name = "grdvMultiOBC_ServiceUrl";
            this.grdvMultiOBC_ServiceUrl.OptionsColumn.AllowEdit = false;
            this.grdvMultiOBC_ServiceUrl.Visible = true;
            this.grdvMultiOBC_ServiceUrl.VisibleIndex = 0;
            this.grdvMultiOBC_ServiceUrl.Width = 106;
            // 
            // grdvMultiOBC_IsRun
            // 
            this.grdvMultiOBC_IsRun.Caption = "Is Run";
            this.grdvMultiOBC_IsRun.ColumnEdit = this.grdvlueMultiOBC_IsRun;
            this.grdvMultiOBC_IsRun.FieldName = "IsRun";
            this.grdvMultiOBC_IsRun.Name = "grdvMultiOBC_IsRun";
            this.grdvMultiOBC_IsRun.Visible = true;
            this.grdvMultiOBC_IsRun.VisibleIndex = 7;
            this.grdvMultiOBC_IsRun.Width = 72;
            // 
            // grdvlueMultiOBC_IsRun
            // 
            this.grdvlueMultiOBC_IsRun.AutoHeight = false;
            this.grdvlueMultiOBC_IsRun.Name = "grdvlueMultiOBC_IsRun";
            // 
            // grdMultiXProxy
            // 
            this.grdMultiXProxy.Location = new System.Drawing.Point(642, 401);
            this.grdMultiXProxy.MainView = this.grdvMultiXProxy;
            this.grdMultiXProxy.MenuManager = this.barManager1;
            this.grdMultiXProxy.Name = "grdMultiXProxy";
            this.grdMultiXProxy.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grdvlueMultiXProxy_IsRun});
            this.grdMultiXProxy.Size = new System.Drawing.Size(622, 74);
            this.grdMultiXProxy.TabIndex = 133;
            this.grdMultiXProxy.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvMultiXProxy,
            this.gridView7});
            // 
            // grdvMultiXProxy
            // 
            this.grdvMultiXProxy.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvMultiXProxy_stt,
            this.grdvMultiXProxy_public_ip,
            this.grdvMultiXProxy_system,
            this.grdvMultiXProxy_proxy_port,
            this.grdvMultiXProxy_sock_port,
            this.grdvMultiXProxy_proxy_full,
            this.grdvMultiXProxy_imei,
            this.grdvMultiXProxy_ServiceUrl,
            this.grdvMultiXProxy_IsRun});
            this.grdvMultiXProxy.GridControl = this.grdMultiXProxy;
            this.grdvMultiXProxy.GroupCount = 1;
            this.grdvMultiXProxy.Name = "grdvMultiXProxy";
            this.grdvMultiXProxy.OptionsBehavior.AutoExpandAllGroups = true;
            this.grdvMultiXProxy.OptionsView.ShowGroupPanel = false;
            this.grdvMultiXProxy.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.grdvMultiXProxy_ServiceUrl, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // grdvMultiXProxy_stt
            // 
            this.grdvMultiXProxy_stt.Caption = "No";
            this.grdvMultiXProxy_stt.FieldName = "stt";
            this.grdvMultiXProxy_stt.Name = "grdvMultiXProxy_stt";
            this.grdvMultiXProxy_stt.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_stt.Visible = true;
            this.grdvMultiXProxy_stt.VisibleIndex = 0;
            this.grdvMultiXProxy_stt.Width = 124;
            // 
            // grdvMultiXProxy_public_ip
            // 
            this.grdvMultiXProxy_public_ip.Caption = "Public Ip";
            this.grdvMultiXProxy_public_ip.FieldName = "public_ip";
            this.grdvMultiXProxy_public_ip.Name = "grdvMultiXProxy_public_ip";
            this.grdvMultiXProxy_public_ip.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_public_ip.Visible = true;
            this.grdvMultiXProxy_public_ip.VisibleIndex = 3;
            this.grdvMultiXProxy_public_ip.Width = 224;
            // 
            // grdvMultiXProxy_system
            // 
            this.grdvMultiXProxy_system.Caption = "WAN IP";
            this.grdvMultiXProxy_system.FieldName = "system";
            this.grdvMultiXProxy_system.Name = "grdvMultiXProxy_system";
            this.grdvMultiXProxy_system.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_system.Visible = true;
            this.grdvMultiXProxy_system.VisibleIndex = 4;
            this.grdvMultiXProxy_system.Width = 224;
            // 
            // grdvMultiXProxy_proxy_port
            // 
            this.grdvMultiXProxy_proxy_port.Caption = "Port";
            this.grdvMultiXProxy_proxy_port.FieldName = "proxy_port";
            this.grdvMultiXProxy_proxy_port.Name = "grdvMultiXProxy_proxy_port";
            this.grdvMultiXProxy_proxy_port.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_proxy_port.Visible = true;
            this.grdvMultiXProxy_proxy_port.VisibleIndex = 5;
            this.grdvMultiXProxy_proxy_port.Width = 224;
            // 
            // grdvMultiXProxy_sock_port
            // 
            this.grdvMultiXProxy_sock_port.Caption = "Sock port";
            this.grdvMultiXProxy_sock_port.FieldName = "sock_port";
            this.grdvMultiXProxy_sock_port.Name = "grdvMultiXProxy_sock_port";
            this.grdvMultiXProxy_sock_port.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_sock_port.Visible = true;
            this.grdvMultiXProxy_sock_port.VisibleIndex = 6;
            this.grdvMultiXProxy_sock_port.Width = 227;
            // 
            // grdvMultiXProxy_proxy_full
            // 
            this.grdvMultiXProxy_proxy_full.Caption = "Proxy Full";
            this.grdvMultiXProxy_proxy_full.FieldName = "proxy_full";
            this.grdvMultiXProxy_proxy_full.Name = "grdvMultiXProxy_proxy_full";
            this.grdvMultiXProxy_proxy_full.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_proxy_full.Visible = true;
            this.grdvMultiXProxy_proxy_full.VisibleIndex = 2;
            this.grdvMultiXProxy_proxy_full.Width = 230;
            // 
            // grdvMultiXProxy_imei
            // 
            this.grdvMultiXProxy_imei.Caption = "IMEI";
            this.grdvMultiXProxy_imei.FieldName = "imei";
            this.grdvMultiXProxy_imei.Name = "grdvMultiXProxy_imei";
            this.grdvMultiXProxy_imei.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_imei.Visible = true;
            this.grdvMultiXProxy_imei.VisibleIndex = 1;
            this.grdvMultiXProxy_imei.Width = 161;
            // 
            // grdvMultiXProxy_ServiceUrl
            // 
            this.grdvMultiXProxy_ServiceUrl.Caption = "Service Url";
            this.grdvMultiXProxy_ServiceUrl.FieldName = "ServiceUrl";
            this.grdvMultiXProxy_ServiceUrl.Name = "grdvMultiXProxy_ServiceUrl";
            this.grdvMultiXProxy_ServiceUrl.OptionsColumn.AllowEdit = false;
            this.grdvMultiXProxy_ServiceUrl.Visible = true;
            this.grdvMultiXProxy_ServiceUrl.VisibleIndex = 7;
            // 
            // grdvMultiXProxy_IsRun
            // 
            this.grdvMultiXProxy_IsRun.Caption = "Is Run";
            this.grdvMultiXProxy_IsRun.ColumnEdit = this.grdvlueMultiXProxy_IsRun;
            this.grdvMultiXProxy_IsRun.FieldName = "IsRun";
            this.grdvMultiXProxy_IsRun.Name = "grdvMultiXProxy_IsRun";
            this.grdvMultiXProxy_IsRun.Visible = true;
            this.grdvMultiXProxy_IsRun.VisibleIndex = 7;
            // 
            // grdvlueMultiXProxy_IsRun
            // 
            this.grdvlueMultiXProxy_IsRun.AutoHeight = false;
            this.grdvlueMultiXProxy_IsRun.Name = "grdvlueMultiXProxy_IsRun";
            // 
            // radMultiProxyType
            // 
            this.radMultiProxyType.EditValue = 1;
            this.radMultiProxyType.Enabled = false;
            this.radMultiProxyType.Location = new System.Drawing.Point(50, 323);
            this.radMultiProxyType.MenuManager = this.barManager1;
            this.radMultiProxyType.Name = "radMultiProxyType";
            this.radMultiProxyType.Properties.Columns = 2;
            this.radMultiProxyType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Số luồng theo số cổng DCOM"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Số luồng cố định và lấy ngẫu nhiên")});
            this.radMultiProxyType.Size = new System.Drawing.Size(377, 74);
            this.radMultiProxyType.StyleController = this.lcMain;
            this.radMultiProxyType.TabIndex = 132;
            // 
            // grdMultiProxy
            // 
            this.grdMultiProxy.Location = new System.Drawing.Point(12, 401);
            this.grdMultiProxy.MainView = this.grdvMultiProxy;
            this.grdMultiProxy.MenuManager = this.barManager1;
            this.grdMultiProxy.Name = "grdMultiProxy";
            this.grdMultiProxy.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.grdvlueMultiProxy_Type});
            this.grdMultiProxy.Size = new System.Drawing.Size(621, 153);
            this.grdMultiProxy.TabIndex = 131;
            this.grdMultiProxy.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvMultiProxy,
            this.gridView6});
            // 
            // grdvMultiProxy
            // 
            this.grdvMultiProxy.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvMultiProxy_Type,
            this.grdvMultiProxy_ServiceUrl});
            this.grdvMultiProxy.GridControl = this.grdMultiProxy;
            this.grdvMultiProxy.Name = "grdvMultiProxy";
            this.grdvMultiProxy.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.grdvMultiProxy.OptionsView.ShowGroupPanel = false;
            this.grdvMultiProxy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdvMultiProxy_KeyDown);
            // 
            // grdvMultiProxy_Type
            // 
            this.grdvMultiProxy_Type.Caption = "Type";
            this.grdvMultiProxy_Type.ColumnEdit = this.grdvlueMultiProxy_Type;
            this.grdvMultiProxy_Type.FieldName = "Type";
            this.grdvMultiProxy_Type.Name = "grdvMultiProxy_Type";
            this.grdvMultiProxy_Type.Visible = true;
            this.grdvMultiProxy_Type.VisibleIndex = 0;
            this.grdvMultiProxy_Type.Width = 394;
            // 
            // grdvlueMultiProxy_Type
            // 
            this.grdvlueMultiProxy_Type.AutoHeight = false;
            this.grdvlueMultiProxy_Type.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdvlueMultiProxy_Type.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "Proxy Type")});
            this.grdvlueMultiProxy_Type.DisplayMember = "NAME";
            this.grdvlueMultiProxy_Type.Name = "grdvlueMultiProxy_Type";
            this.grdvlueMultiProxy_Type.NullText = "";
            this.grdvlueMultiProxy_Type.ValueMember = "ID";
            // 
            // grdvMultiProxy_ServiceUrl
            // 
            this.grdvMultiProxy_ServiceUrl.Caption = "Service Url";
            this.grdvMultiProxy_ServiceUrl.FieldName = "ServiceUrl";
            this.grdvMultiProxy_ServiceUrl.Name = "grdvMultiProxy_ServiceUrl";
            this.grdvMultiProxy_ServiceUrl.Visible = true;
            this.grdvMultiProxy_ServiceUrl.VisibleIndex = 1;
            this.grdvMultiProxy_ServiceUrl.Width = 1020;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // btnClearAllKeyword
            // 
            this.btnClearAllKeyword.Location = new System.Drawing.Point(766, 223);
            this.btnClearAllKeyword.Name = "btnClearAllKeyword";
            this.btnClearAllKeyword.Size = new System.Drawing.Size(250, 22);
            this.btnClearAllKeyword.StyleController = this.lcMain;
            toolTipTitleItem16.Text = "Hướng dẫn";
            toolTipItem16.LeftIndent = 6;
            toolTipItem16.Text = "Xóa toàn bộ keyword";
            superToolTip16.Items.Add(toolTipTitleItem16);
            superToolTip16.Items.Add(toolTipItem16);
            this.btnClearAllKeyword.SuperTip = superToolTip16;
            this.btnClearAllKeyword.TabIndex = 130;
            this.btnClearAllKeyword.Text = "Clear All Keyword";
            this.btnClearAllKeyword.Click += new System.EventHandler(this.btnClearAllKeyword_Click);
            // 
            // btnClearSelectionKeyword
            // 
            this.btnClearSelectionKeyword.Location = new System.Drawing.Point(513, 223);
            this.btnClearSelectionKeyword.Name = "btnClearSelectionKeyword";
            this.btnClearSelectionKeyword.Size = new System.Drawing.Size(249, 22);
            this.btnClearSelectionKeyword.StyleController = this.lcMain;
            toolTipTitleItem17.Text = "Hướng dẫn";
            toolTipItem17.LeftIndent = 6;
            toolTipItem17.Text = "Xóa keyword đang chọn ở danh sách bên trái";
            superToolTip17.Items.Add(toolTipTitleItem17);
            superToolTip17.Items.Add(toolTipItem17);
            this.btnClearSelectionKeyword.SuperTip = superToolTip17;
            this.btnClearSelectionKeyword.TabIndex = 129;
            this.btnClearSelectionKeyword.Text = "Delete Keyword";
            this.btnClearSelectionKeyword.Click += new System.EventHandler(this.btnClearSelectionKeyword_Click);
            // 
            // grdOBC
            // 
            this.grdOBC.Location = new System.Drawing.Point(12, 349);
            this.grdOBC.MainView = this.grdvOBC;
            this.grdOBC.MenuManager = this.barManager1;
            this.grdOBC.Name = "grdOBC";
            this.grdOBC.Size = new System.Drawing.Size(1252, 205);
            this.grdOBC.TabIndex = 128;
            this.grdOBC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvOBC,
            this.gridView5});
            // 
            // grdvOBC
            // 
            this.grdvOBC.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvOBC_name,
            this.grdvOBC_idKey,
            this.grdvOBC_proxyAddress,
            this.grdvOBC_proxyStatus,
            this.grdvOBC_port,
            this.grdvOBC_sockPort,
            this.grdvOBC_socksEnable,
            this.grdvOBC_publicIp,
            this.grdvOBC_IsRun});
            this.grdvOBC.GridControl = this.grdOBC;
            this.grdvOBC.Name = "grdvOBC";
            this.grdvOBC.OptionsView.ShowGroupPanel = false;
            // 
            // grdvOBC_name
            // 
            this.grdvOBC_name.Caption = "Name";
            this.grdvOBC_name.FieldName = "name";
            this.grdvOBC_name.Name = "grdvOBC_name";
            this.grdvOBC_name.OptionsColumn.AllowEdit = false;
            this.grdvOBC_name.Visible = true;
            this.grdvOBC_name.VisibleIndex = 0;
            this.grdvOBC_name.Width = 117;
            // 
            // grdvOBC_idKey
            // 
            this.grdvOBC_idKey.Caption = "idKey";
            this.grdvOBC_idKey.FieldName = "idKey";
            this.grdvOBC_idKey.Name = "grdvOBC_idKey";
            this.grdvOBC_idKey.OptionsColumn.AllowEdit = false;
            this.grdvOBC_idKey.Visible = true;
            this.grdvOBC_idKey.VisibleIndex = 1;
            this.grdvOBC_idKey.Width = 158;
            // 
            // grdvOBC_proxyAddress
            // 
            this.grdvOBC_proxyAddress.Caption = "proxyAddress";
            this.grdvOBC_proxyAddress.FieldName = "proxyAddress";
            this.grdvOBC_proxyAddress.Name = "grdvOBC_proxyAddress";
            this.grdvOBC_proxyAddress.OptionsColumn.AllowEdit = false;
            this.grdvOBC_proxyAddress.Visible = true;
            this.grdvOBC_proxyAddress.VisibleIndex = 5;
            this.grdvOBC_proxyAddress.Width = 231;
            // 
            // grdvOBC_proxyStatus
            // 
            this.grdvOBC_proxyStatus.Caption = "proxyStatus";
            this.grdvOBC_proxyStatus.FieldName = "proxyStatus";
            this.grdvOBC_proxyStatus.Name = "grdvOBC_proxyStatus";
            this.grdvOBC_proxyStatus.OptionsColumn.AllowEdit = false;
            this.grdvOBC_proxyStatus.Visible = true;
            this.grdvOBC_proxyStatus.VisibleIndex = 7;
            this.grdvOBC_proxyStatus.Width = 298;
            // 
            // grdvOBC_port
            // 
            this.grdvOBC_port.Caption = "port";
            this.grdvOBC_port.FieldName = "port";
            this.grdvOBC_port.Name = "grdvOBC_port";
            this.grdvOBC_port.OptionsColumn.AllowEdit = false;
            this.grdvOBC_port.Visible = true;
            this.grdvOBC_port.VisibleIndex = 3;
            this.grdvOBC_port.Width = 134;
            // 
            // grdvOBC_sockPort
            // 
            this.grdvOBC_sockPort.Caption = "sockPort";
            this.grdvOBC_sockPort.FieldName = "sockPort";
            this.grdvOBC_sockPort.Name = "grdvOBC_sockPort";
            this.grdvOBC_sockPort.OptionsColumn.AllowEdit = false;
            this.grdvOBC_sockPort.Visible = true;
            this.grdvOBC_sockPort.VisibleIndex = 4;
            this.grdvOBC_sockPort.Width = 156;
            // 
            // grdvOBC_socksEnable
            // 
            this.grdvOBC_socksEnable.Caption = "socksEnable";
            this.grdvOBC_socksEnable.FieldName = "socksEnable";
            this.grdvOBC_socksEnable.Name = "grdvOBC_socksEnable";
            this.grdvOBC_socksEnable.OptionsColumn.AllowEdit = false;
            this.grdvOBC_socksEnable.Visible = true;
            this.grdvOBC_socksEnable.VisibleIndex = 6;
            this.grdvOBC_socksEnable.Width = 102;
            // 
            // grdvOBC_publicIp
            // 
            this.grdvOBC_publicIp.Caption = "publicIp";
            this.grdvOBC_publicIp.FieldName = "publicIp";
            this.grdvOBC_publicIp.Name = "grdvOBC_publicIp";
            this.grdvOBC_publicIp.OptionsColumn.AllowEdit = false;
            this.grdvOBC_publicIp.Visible = true;
            this.grdvOBC_publicIp.VisibleIndex = 2;
            this.grdvOBC_publicIp.Width = 218;
            // 
            // grdvOBC_IsRun
            // 
            this.grdvOBC_IsRun.Caption = "Is Run";
            this.grdvOBC_IsRun.FieldName = "IsRun";
            this.grdvOBC_IsRun.Name = "grdvOBC_IsRun";
            this.grdvOBC_IsRun.Visible = true;
            this.grdvOBC_IsRun.VisibleIndex = 8;
            // 
            // btnConnectOBC
            // 
            this.btnConnectOBC.Location = new System.Drawing.Point(326, 323);
            this.btnConnectOBC.Name = "btnConnectOBC";
            this.btnConnectOBC.Size = new System.Drawing.Size(310, 22);
            this.btnConnectOBC.StyleController = this.lcMain;
            this.btnConnectOBC.TabIndex = 127;
            this.btnConnectOBC.Text = "Kết nối thử";
            this.btnConnectOBC.Click += new System.EventHandler(this.btnConnectOBC_Click);
            // 
            // txtOBCHost
            // 
            this.txtOBCHost.Location = new System.Drawing.Point(68, 323);
            this.txtOBCHost.MenuManager = this.barManager1;
            this.txtOBCHost.Name = "txtOBCHost";
            this.txtOBCHost.Size = new System.Drawing.Size(254, 20);
            this.txtOBCHost.StyleController = this.lcMain;
            toolTipTitleItem18.Text = "Hướng dẫn";
            toolTipItem18.LeftIndent = 6;
            toolTipItem18.Text = "Địa chỉ host XProxy cung cấp";
            superToolTip18.Items.Add(toolTipTitleItem18);
            superToolTip18.Items.Add(toolTipItem18);
            this.txtOBCHost.SuperTip = superToolTip18;
            this.txtOBCHost.TabIndex = 119;
            // 
            // btnImportKeyword
            // 
            this.btnImportKeyword.Location = new System.Drawing.Point(260, 223);
            this.btnImportKeyword.Name = "btnImportKeyword";
            this.btnImportKeyword.Size = new System.Drawing.Size(249, 22);
            this.btnImportKeyword.StyleController = this.lcMain;
            toolTipTitleItem19.Text = "Hướng dẫn";
            toolTipItem19.LeftIndent = 6;
            toolTipItem19.Text = "Nhập danh sách Keyword từ excel.\r\nFile excel mẫu chính là file xuất ra ở nút \"Exp" +
    "ort Keyword\"";
            superToolTip19.Items.Add(toolTipTitleItem19);
            superToolTip19.Items.Add(toolTipItem19);
            this.btnImportKeyword.SuperTip = superToolTip19;
            this.btnImportKeyword.TabIndex = 126;
            this.btnImportKeyword.Text = "Import Keyword";
            this.btnImportKeyword.Click += new System.EventHandler(this.btnImportKeyword_Click);
            // 
            // btnExportKeyword
            // 
            this.btnExportKeyword.Location = new System.Drawing.Point(7, 223);
            this.btnExportKeyword.Name = "btnExportKeyword";
            this.btnExportKeyword.Size = new System.Drawing.Size(249, 22);
            this.btnExportKeyword.StyleController = this.lcMain;
            toolTipTitleItem20.Text = "Hướng dẫn";
            toolTipItem20.LeftIndent = 6;
            toolTipItem20.Text = "Xuất danh sách keyword ra file excel";
            superToolTip20.Items.Add(toolTipTitleItem20);
            superToolTip20.Items.Add(toolTipItem20);
            this.btnExportKeyword.SuperTip = superToolTip20;
            this.btnExportKeyword.TabIndex = 125;
            this.btnExportKeyword.Text = "Export Keyword";
            this.btnExportKeyword.Click += new System.EventHandler(this.btnExportKeyword_Click);
            // 
            // grdKeyword
            // 
            this.grdKeyword.Location = new System.Drawing.Point(5, 45);
            this.grdKeyword.MainView = this.grdvKeyword;
            this.grdKeyword.MenuManager = this.barManager1;
            this.grdKeyword.Name = "grdKeyword";
            this.grdKeyword.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.luevSubLink,
            this.grdvbeiKeyword_Delete,
            this.grdvlueKeyword_Type});
            this.grdKeyword.Size = new System.Drawing.Size(1266, 171);
            this.grdKeyword.TabIndex = 124;
            this.grdKeyword.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvKeyword,
            this.gridView4});
            // 
            // grdvKeyword
            // 
            this.grdvKeyword.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvKeyword_Key,
            this.grdvKeyword_Domain,
            this.grdvKeyword_SubLink,
            this.grdvKeyword_Delete,
            this.grdvKeyword_Type});
            this.grdvKeyword.GridControl = this.grdKeyword;
            this.grdvKeyword.Name = "grdvKeyword";
            this.grdvKeyword.OptionsSelection.MultiSelect = true;
            this.grdvKeyword.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.grdvKeyword.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.grdvKeyword.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.grdvKeyword.OptionsView.ShowGroupPanel = false;
            this.grdvKeyword.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdvKeyword_InvalidRowException);
            this.grdvKeyword.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdvKeyword_ValidateRow);
            this.grdvKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdvKeyword_KeyDown);
            this.grdvKeyword.InvalidValueException += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(this.grdvKeyword_InvalidValueException);
            // 
            // grdvKeyword_Key
            // 
            this.grdvKeyword_Key.Caption = "Keyword";
            this.grdvKeyword_Key.FieldName = "Key";
            this.grdvKeyword_Key.Name = "grdvKeyword_Key";
            this.grdvKeyword_Key.ToolTip = "Danh sách từ khóa muốn SEO Google";
            this.grdvKeyword_Key.Visible = true;
            this.grdvKeyword_Key.VisibleIndex = 0;
            this.grdvKeyword_Key.Width = 351;
            // 
            // grdvKeyword_Domain
            // 
            this.grdvKeyword_Domain.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.grdvKeyword_Domain.AppearanceHeader.Options.UseForeColor = true;
            this.grdvKeyword_Domain.Caption = "Domain/Link/YoutubeVideoID";
            this.grdvKeyword_Domain.FieldName = "Domain";
            this.grdvKeyword_Domain.Name = "grdvKeyword_Domain";
            this.grdvKeyword_Domain.ToolTip = "Danh sách domain/link muốn click khi search từ khóa ở Google";
            this.grdvKeyword_Domain.Visible = true;
            this.grdvKeyword_Domain.VisibleIndex = 1;
            this.grdvKeyword_Domain.Width = 994;
            // 
            // grdvKeyword_SubLink
            // 
            this.grdvKeyword_SubLink.Caption = "External link (1 link/line)";
            this.grdvKeyword_SubLink.ColumnEdit = this.luevSubLink;
            this.grdvKeyword_SubLink.FieldName = "SubLink";
            this.grdvKeyword_SubLink.Name = "grdvKeyword_SubLink";
            this.grdvKeyword_SubLink.ToolTip = "Danh sách link con muốn click tiếp sau khi duyệt trang đích";
            this.grdvKeyword_SubLink.Visible = true;
            this.grdvKeyword_SubLink.VisibleIndex = 2;
            this.grdvKeyword_SubLink.Width = 337;
            // 
            // luevSubLink
            // 
            this.luevSubLink.AutoHeight = false;
            this.luevSubLink.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luevSubLink.Name = "luevSubLink";
            // 
            // grdvKeyword_Delete
            // 
            this.grdvKeyword_Delete.Caption = "Option";
            this.grdvKeyword_Delete.ColumnEdit = this.grdvbeiKeyword_Delete;
            this.grdvKeyword_Delete.Name = "grdvKeyword_Delete";
            this.grdvKeyword_Delete.Visible = true;
            this.grdvKeyword_Delete.VisibleIndex = 4;
            this.grdvKeyword_Delete.Width = 205;
            // 
            // grdvbeiKeyword_Delete
            // 
            this.grdvbeiKeyword_Delete.AutoHeight = false;
            this.grdvbeiKeyword_Delete.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Xóa", -1, true, true, false, editorButtonImageOptions3),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Chi tiết", -1, true, true, false, editorButtonImageOptions4)});
            this.grdvbeiKeyword_Delete.Name = "grdvbeiKeyword_Delete";
            this.grdvbeiKeyword_Delete.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grdvbeiKeyword_Delete_ButtonClick);
            // 
            // grdvKeyword_Type
            // 
            this.grdvKeyword_Type.Caption = "Type";
            this.grdvKeyword_Type.ColumnEdit = this.grdvlueKeyword_Type;
            this.grdvKeyword_Type.FieldName = "Type";
            this.grdvKeyword_Type.Name = "grdvKeyword_Type";
            this.grdvKeyword_Type.Visible = true;
            this.grdvKeyword_Type.VisibleIndex = 3;
            this.grdvKeyword_Type.Width = 244;
            // 
            // grdvlueKeyword_Type
            // 
            this.grdvlueKeyword_Type.AutoHeight = false;
            this.grdvlueKeyword_Type.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdvlueKeyword_Type.DisplayMember = "NAME";
            this.grdvlueKeyword_Type.Name = "grdvlueKeyword_Type";
            this.grdvlueKeyword_Type.NullText = "";
            this.grdvlueKeyword_Type.ValueMember = "ID";
            this.grdvlueKeyword_Type.View = this.grdvluevKeyword_Type;
            // 
            // grdvluevKeyword_Type
            // 
            this.grdvluevKeyword_Type.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvluevKeyword_Type_NAME});
            this.grdvluevKeyword_Type.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdvluevKeyword_Type.Name = "grdvluevKeyword_Type";
            this.grdvluevKeyword_Type.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdvluevKeyword_Type.OptionsView.ShowGroupPanel = false;
            // 
            // grdvluevKeyword_Type_NAME
            // 
            this.grdvluevKeyword_Type_NAME.Caption = "Loại";
            this.grdvluevKeyword_Type_NAME.FieldName = "NAME";
            this.grdvluevKeyword_Type_NAME.Name = "grdvluevKeyword_Type_NAME";
            this.grdvluevKeyword_Type_NAME.Visible = true;
            this.grdvluevKeyword_Type_NAME.VisibleIndex = 0;
            // 
            // btnExcelIp
            // 
            this.btnExcelIp.Location = new System.Drawing.Point(646, 74);
            this.btnExcelIp.Name = "btnExcelIp";
            this.btnExcelIp.Size = new System.Drawing.Size(309, 22);
            this.btnExcelIp.StyleController = this.lcMain;
            this.btnExcelIp.TabIndex = 123;
            this.btnExcelIp.Text = "Xuất excel";
            this.btnExcelIp.Click += new System.EventHandler(this.btnExcelIp_Click);
            // 
            // btnExcelDomain
            // 
            this.btnExcelDomain.Location = new System.Drawing.Point(8, 74);
            this.btnExcelDomain.Name = "btnExcelDomain";
            this.btnExcelDomain.Size = new System.Drawing.Size(310, 22);
            this.btnExcelDomain.StyleController = this.lcMain;
            this.btnExcelDomain.TabIndex = 122;
            this.btnExcelDomain.Text = "Xuất excel";
            this.btnExcelDomain.Click += new System.EventHandler(this.btnExcelDomain_Click);
            // 
            // memReChuot
            // 
            this.memReChuot.EditValue = "Lưu ý: Rê chuột vào từng mục để đọc hướng dẫn sử dụng của chức năng đó.";
            this.memReChuot.Location = new System.Drawing.Point(1095, 569);
            this.memReChuot.MenuManager = this.barManager1;
            this.memReChuot.Name = "memReChuot";
            this.memReChuot.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Italic);
            this.memReChuot.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.memReChuot.Properties.Appearance.Options.UseFont = true;
            this.memReChuot.Properties.Appearance.Options.UseForeColor = true;
            this.memReChuot.Properties.ReadOnly = true;
            this.memReChuot.Size = new System.Drawing.Size(178, 51);
            this.memReChuot.StyleController = this.lcMain;
            this.memReChuot.TabIndex = 121;
            // 
            // grdXProxyList
            // 
            this.grdXProxyList.Location = new System.Drawing.Point(12, 349);
            this.grdXProxyList.MainView = this.grdvXProxyList;
            this.grdXProxyList.MenuManager = this.barManager1;
            this.grdXProxyList.Name = "grdXProxyList";
            this.grdXProxyList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ceiIsRun});
            this.grdXProxyList.Size = new System.Drawing.Size(1252, 205);
            this.grdXProxyList.TabIndex = 120;
            this.grdXProxyList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvXProxyList});
            // 
            // grdvXProxyList
            // 
            this.grdvXProxyList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvXProxyList_stt,
            this.grdvXProxyList_public_ip,
            this.grdvXProxyList_system,
            this.grdvXProxyList_proxy_port,
            this.grdvXProxyList_sock_port,
            this.grdvXProxyList_proxy_full,
            this.grdvXProxyList_imei,
            this.grdvXProxyList_IsRun});
            this.grdvXProxyList.GridControl = this.grdXProxyList;
            this.grdvXProxyList.Name = "grdvXProxyList";
            this.grdvXProxyList.OptionsView.ShowGroupPanel = false;
            // 
            // grdvXProxyList_stt
            // 
            this.grdvXProxyList_stt.Caption = "No";
            this.grdvXProxyList_stt.FieldName = "stt";
            this.grdvXProxyList_stt.Name = "grdvXProxyList_stt";
            this.grdvXProxyList_stt.OptionsColumn.AllowEdit = false;
            this.grdvXProxyList_stt.Visible = true;
            this.grdvXProxyList_stt.VisibleIndex = 0;
            this.grdvXProxyList_stt.Width = 117;
            // 
            // grdvXProxyList_public_ip
            // 
            this.grdvXProxyList_public_ip.Caption = "Public Ip";
            this.grdvXProxyList_public_ip.FieldName = "public_ip";
            this.grdvXProxyList_public_ip.Name = "grdvXProxyList_public_ip";
            this.grdvXProxyList_public_ip.OptionsColumn.AllowEdit = false;
            this.grdvXProxyList_public_ip.Visible = true;
            this.grdvXProxyList_public_ip.VisibleIndex = 3;
            this.grdvXProxyList_public_ip.Width = 212;
            // 
            // grdvXProxyList_system
            // 
            this.grdvXProxyList_system.Caption = "WAN IP";
            this.grdvXProxyList_system.FieldName = "system";
            this.grdvXProxyList_system.Name = "grdvXProxyList_system";
            this.grdvXProxyList_system.OptionsColumn.AllowEdit = false;
            this.grdvXProxyList_system.Visible = true;
            this.grdvXProxyList_system.VisibleIndex = 4;
            this.grdvXProxyList_system.Width = 212;
            // 
            // grdvXProxyList_proxy_port
            // 
            this.grdvXProxyList_proxy_port.Caption = "Port";
            this.grdvXProxyList_proxy_port.FieldName = "proxy_port";
            this.grdvXProxyList_proxy_port.Name = "grdvXProxyList_proxy_port";
            this.grdvXProxyList_proxy_port.OptionsColumn.AllowEdit = false;
            this.grdvXProxyList_proxy_port.Visible = true;
            this.grdvXProxyList_proxy_port.VisibleIndex = 5;
            this.grdvXProxyList_proxy_port.Width = 212;
            // 
            // grdvXProxyList_sock_port
            // 
            this.grdvXProxyList_sock_port.Caption = "Sock port";
            this.grdvXProxyList_sock_port.Name = "grdvXProxyList_sock_port";
            this.grdvXProxyList_sock_port.OptionsColumn.AllowEdit = false;
            this.grdvXProxyList_sock_port.Visible = true;
            this.grdvXProxyList_sock_port.VisibleIndex = 6;
            this.grdvXProxyList_sock_port.Width = 172;
            // 
            // grdvXProxyList_proxy_full
            // 
            this.grdvXProxyList_proxy_full.Caption = "Proxy Full";
            this.grdvXProxyList_proxy_full.FieldName = "proxy_full";
            this.grdvXProxyList_proxy_full.Name = "grdvXProxyList_proxy_full";
            this.grdvXProxyList_proxy_full.OptionsColumn.AllowEdit = false;
            this.grdvXProxyList_proxy_full.Visible = true;
            this.grdvXProxyList_proxy_full.VisibleIndex = 2;
            this.grdvXProxyList_proxy_full.Width = 218;
            // 
            // grdvXProxyList_imei
            // 
            this.grdvXProxyList_imei.Caption = "IMEI";
            this.grdvXProxyList_imei.FieldName = "imei";
            this.grdvXProxyList_imei.Name = "grdvXProxyList_imei";
            this.grdvXProxyList_imei.OptionsColumn.AllowEdit = false;
            this.grdvXProxyList_imei.Visible = true;
            this.grdvXProxyList_imei.VisibleIndex = 1;
            this.grdvXProxyList_imei.Width = 152;
            // 
            // grdvXProxyList_IsRun
            // 
            this.grdvXProxyList_IsRun.Caption = "Is Run";
            this.grdvXProxyList_IsRun.ColumnEdit = this.ceiIsRun;
            this.grdvXProxyList_IsRun.FieldName = "IsRun";
            this.grdvXProxyList_IsRun.Name = "grdvXProxyList_IsRun";
            this.grdvXProxyList_IsRun.Visible = true;
            this.grdvXProxyList_IsRun.VisibleIndex = 7;
            this.grdvXProxyList_IsRun.Width = 119;
            // 
            // ceiIsRun
            // 
            this.ceiIsRun.AutoHeight = false;
            this.ceiIsRun.Name = "ceiIsRun";
            // 
            // btnConnectxProxy
            // 
            this.btnConnectxProxy.Location = new System.Drawing.Point(326, 323);
            this.btnConnectxProxy.Name = "btnConnectxProxy";
            this.btnConnectxProxy.Size = new System.Drawing.Size(310, 22);
            this.btnConnectxProxy.StyleController = this.lcMain;
            toolTipTitleItem21.Text = "Hướng dẫn";
            toolTipItem21.LeftIndent = 6;
            toolTipItem21.Text = "Click để tiến hành kết nối đến Service XProxy";
            superToolTip21.Items.Add(toolTipTitleItem21);
            superToolTip21.Items.Add(toolTipItem21);
            this.btnConnectxProxy.SuperTip = superToolTip21;
            this.btnConnectxProxy.TabIndex = 119;
            this.btnConnectxProxy.Text = "Kết nối thử";
            this.btnConnectxProxy.Click += new System.EventHandler(this.btnConnectxProxy_Click);
            // 
            // txtXProxyHost
            // 
            this.txtXProxyHost.Location = new System.Drawing.Point(68, 323);
            this.txtXProxyHost.MenuManager = this.barManager1;
            this.txtXProxyHost.Name = "txtXProxyHost";
            this.txtXProxyHost.Size = new System.Drawing.Size(254, 20);
            this.txtXProxyHost.StyleController = this.lcMain;
            toolTipTitleItem22.Text = "Hướng dẫn";
            toolTipItem22.LeftIndent = 6;
            toolTipItem22.Text = "Địa chỉ host XProxy cung cấp";
            superToolTip22.Items.Add(toolTipTitleItem22);
            superToolTip22.Items.Add(toolTipItem22);
            this.txtXProxyHost.SuperTip = superToolTip22;
            this.txtXProxyHost.TabIndex = 118;
            // 
            // lbeNoticeFreeProxy
            // 
            this.lbeNoticeFreeProxy.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.lbeNoticeFreeProxy.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lbeNoticeFreeProxy.Appearance.Options.UseFont = true;
            this.lbeNoticeFreeProxy.Appearance.Options.UseForeColor = true;
            this.lbeNoticeFreeProxy.Location = new System.Drawing.Point(15, 326);
            this.lbeNoticeFreeProxy.Name = "lbeNoticeFreeProxy";
            this.lbeNoticeFreeProxy.Size = new System.Drawing.Size(1246, 13);
            this.lbeNoticeFreeProxy.StyleController = this.lcMain;
            this.lbeNoticeFreeProxy.TabIndex = 117;
            this.lbeNoticeFreeProxy.Text = "Danh sách proxy (Lưu ý: Hiện tại proxy free rất chậm và đa số bị Google đánh giá " +
    "spam)";
            // 
            // btnChangeMAC
            // 
            this.btnChangeMAC.Location = new System.Drawing.Point(514, 46);
            this.btnChangeMAC.Name = "btnChangeMAC";
            this.btnChangeMAC.Size = new System.Drawing.Size(249, 22);
            this.btnChangeMAC.StyleController = this.lcMain;
            toolTipTitleItem23.Text = "Hướng dẫn";
            toolTipItem23.LeftIndent = 6;
            toolTipItem23.Text = "Click để tiến hành đổi thử MAC Address";
            superToolTip23.Items.Add(toolTipTitleItem23);
            superToolTip23.Items.Add(toolTipItem23);
            this.btnChangeMAC.SuperTip = superToolTip23;
            this.btnChangeMAC.TabIndex = 115;
            this.btnChangeMAC.Text = "Đổi thử";
            this.btnChangeMAC.Click += new System.EventHandler(this.btnChangeMAC_Click);
            // 
            // speMACAddressInterval
            // 
            this.speMACAddressInterval.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.speMACAddressInterval.Location = new System.Drawing.Point(341, 46);
            this.speMACAddressInterval.MenuManager = this.barManager1;
            this.speMACAddressInterval.Name = "speMACAddressInterval";
            this.speMACAddressInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speMACAddressInterval.Properties.MaxValue = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.speMACAddressInterval.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speMACAddressInterval.Size = new System.Drawing.Size(169, 20);
            this.speMACAddressInterval.StyleController = this.lcMain;
            toolTipTitleItem24.Text = "Hướng dẫn";
            toolTipItem24.LeftIndent = 6;
            toolTipItem24.Text = "Chu kỳ thời gian thay đổi địa chỉ MAC (MAC Address)";
            superToolTip24.Items.Add(toolTipTitleItem24);
            superToolTip24.Items.Add(toolTipItem24);
            this.speMACAddressInterval.SuperTip = superToolTip24;
            this.speMACAddressInterval.TabIndex = 114;
            // 
            // ceiChangeMACAddress
            // 
            this.ceiChangeMACAddress.Location = new System.Drawing.Point(51, 46);
            this.ceiChangeMACAddress.MenuManager = this.barManager1;
            this.ceiChangeMACAddress.Name = "ceiChangeMACAddress";
            this.ceiChangeMACAddress.Properties.Caption = "";
            this.ceiChangeMACAddress.Size = new System.Drawing.Size(205, 19);
            this.ceiChangeMACAddress.StyleController = this.lcMain;
            toolTipTitleItem25.Text = "Hướng dẫn";
            toolTipItem25.LeftIndent = 6;
            toolTipItem25.Text = "Check chọn để phần mềm tiến hành tự động thay đổi địa chỉ MAC (MAC Address) theo " +
    "chu kỳ. Thay đổi địa chỉ MAC nhằm tăng tỷ lệ thành công cao hơn.";
            superToolTip25.Items.Add(toolTipTitleItem25);
            superToolTip25.Items.Add(toolTipItem25);
            this.ceiChangeMACAddress.SuperTip = superToolTip25;
            this.ceiChangeMACAddress.TabIndex = 113;
            // 
            // speSubLinkView
            // 
            this.speSubLinkView.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.speSubLinkView.Location = new System.Drawing.Point(825, 114);
            this.speSubLinkView.MenuManager = this.barManager1;
            this.speSubLinkView.Name = "speSubLinkView";
            this.speSubLinkView.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speSubLinkView.Properties.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.speSubLinkView.Properties.MinValue = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.speSubLinkView.Size = new System.Drawing.Size(192, 20);
            this.speSubLinkView.StyleController = this.lcMain;
            toolTipTitleItem26.Text = "Hướng dẫn";
            toolTipItem26.LeftIndent = 6;
            toolTipItem26.Text = "Mốc thời gian tối thiểu phần mềm sẽ lấy để duyệt vào các Internal Link hoặc Exter" +
    "nal Link";
            superToolTip26.Items.Add(toolTipTitleItem26);
            superToolTip26.Items.Add(toolTipItem26);
            this.speSubLinkView.SuperTip = superToolTip26;
            this.speSubLinkView.TabIndex = 112;
            this.speSubLinkView.EditValueChanged += new System.EventHandler(this.speSubLinkView_EditValueChanged);
            // 
            // grdIp
            // 
            this.grdIp.Location = new System.Drawing.Point(646, 100);
            this.grdIp.MainView = this.grdvIp;
            this.grdIp.MenuManager = this.barManager1;
            this.grdIp.Name = "grdIp";
            this.grdIp.Size = new System.Drawing.Size(622, 458);
            this.grdIp.TabIndex = 110;
            this.grdIp.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvIp,
            this.gridView2});
            // 
            // grdvIp
            // 
            this.grdvIp.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvIp_Ngay,
            this.grdvIp_Ip,
            this.grdvIp_Click});
            this.grdvIp.GridControl = this.grdIp;
            this.grdvIp.GroupCount = 1;
            this.grdvIp.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Click", this.grdvIp_Click, "")});
            this.grdvIp.Name = "grdvIp";
            this.grdvIp.OptionsBehavior.AutoExpandAllGroups = true;
            this.grdvIp.OptionsBehavior.Editable = false;
            this.grdvIp.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.grdvIp_Ngay, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // grdvIp_Ngay
            // 
            this.grdvIp_Ngay.Caption = "Ngày";
            this.grdvIp_Ngay.FieldName = "Ngay";
            this.grdvIp_Ngay.Name = "grdvIp_Ngay";
            this.grdvIp_Ngay.Visible = true;
            this.grdvIp_Ngay.VisibleIndex = 0;
            // 
            // grdvIp_Ip
            // 
            this.grdvIp_Ip.Caption = "Địa chỉ IP";
            this.grdvIp_Ip.FieldName = "IP";
            this.grdvIp_Ip.Name = "grdvIp_Ip";
            this.grdvIp_Ip.Visible = true;
            this.grdvIp_Ip.VisibleIndex = 0;
            // 
            // grdvIp_Click
            // 
            this.grdvIp_Click.Caption = "Số lượt click";
            this.grdvIp_Click.FieldName = "Click";
            this.grdvIp_Click.Name = "grdvIp_Click";
            this.grdvIp_Click.Visible = true;
            this.grdvIp_Click.VisibleIndex = 1;
            // 
            // speSoLuong
            // 
            this.speSoLuong.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speSoLuong.Location = new System.Drawing.Point(93, 70);
            this.speSoLuong.MenuManager = this.barManager1;
            this.speSoLuong.Name = "speSoLuong";
            this.speSoLuong.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speSoLuong.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.speSoLuong.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speSoLuong.Size = new System.Drawing.Size(221, 20);
            this.speSoLuong.StyleController = this.lcMain;
            toolTipTitleItem27.Text = "Số luồng";
            toolTipItem27.LeftIndent = 6;
            toolTipItem27.Text = "Số luồng của một Proxy phần mềm sẽ chạy. Số cửa sổ Trình duyệt = Số luồng x Số ke" +
    "y proxy. Nếu dùng DCOM thì [Số key proxy] tương ứng=1.";
            superToolTip27.Items.Add(toolTipTitleItem27);
            superToolTip27.Items.Add(toolTipItem27);
            this.speSoLuong.SuperTip = superToolTip27;
            this.speSoLuong.TabIndex = 109;
            // 
            // memProfile
            // 
            this.memProfile.Location = new System.Drawing.Point(8, 99);
            this.memProfile.MenuManager = this.barManager1;
            this.memProfile.Name = "memProfile";
            this.memProfile.Size = new System.Drawing.Size(1260, 459);
            this.memProfile.StyleController = this.lcMain;
            toolTipTitleItem28.Text = "Hướng dẫn";
            toolTipItem28.LeftIndent = 6;
            toolTipItem28.Text = "Mỗi dòng là 1 Tên Firefox Profile (vd: Profile1). Đọc HDSD cách tạo và sử dụng pr" +
    "ofile ở menu Hướng dẫn sử dụng.";
            superToolTip28.Items.Add(toolTipTitleItem28);
            superToolTip28.Items.Add(toolTipItem28);
            this.memProfile.SuperTip = superToolTip28;
            this.memProfile.TabIndex = 108;
            // 
            // radGMail
            // 
            this.radGMail.EditValue = 0;
            this.radGMail.Location = new System.Drawing.Point(5, 27);
            this.radGMail.MenuManager = this.barManager1;
            this.radGMail.Name = "radGMail";
            this.radGMail.Properties.Columns = 3;
            this.radGMail.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "No Login"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "GMail (Firefox v75 trở xuống)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Profile (Khuyên dùng)")});
            this.radGMail.Size = new System.Drawing.Size(314, 25);
            this.radGMail.StyleController = this.lcMain;
            toolTipTitleItem29.Text = "Hướng dẫn";
            toolTipItem29.LeftIndent = 6;
            toolTipItem29.Text = "Chọn hình thức đăng nhập GMail";
            superToolTip29.Items.Add(toolTipTitleItem29);
            superToolTip29.Items.Add(toolTipItem29);
            this.radGMail.SuperTip = superToolTip29;
            this.radGMail.TabIndex = 107;
            this.radGMail.SelectedIndexChanged += new System.EventHandler(this.radGMail_SelectedIndexChanged);
            // 
            // lbeNoticeTinsoft
            // 
            this.lbeNoticeTinsoft.Appearance.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Italic);
            this.lbeNoticeTinsoft.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lbeNoticeTinsoft.Appearance.Options.UseFont = true;
            this.lbeNoticeTinsoft.Appearance.Options.UseForeColor = true;
            this.lbeNoticeTinsoft.Location = new System.Drawing.Point(12, 323);
            this.lbeNoticeTinsoft.Name = "lbeNoticeTinsoft";
            this.lbeNoticeTinsoft.Size = new System.Drawing.Size(1252, 12);
            this.lbeNoticeTinsoft.StyleController = this.lcMain;
            this.lbeNoticeTinsoft.TabIndex = 106;
            this.lbeNoticeTinsoft.Text = resources.GetString("lbeNoticeTinsoft.Text");
            // 
            // btnDeleteHistoryXml
            // 
            this.btnDeleteHistoryXml.Location = new System.Drawing.Point(323, 27);
            this.btnDeleteHistoryXml.Name = "btnDeleteHistoryXml";
            this.btnDeleteHistoryXml.Size = new System.Drawing.Size(313, 22);
            this.btnDeleteHistoryXml.StyleController = this.lcMain;
            this.btnDeleteHistoryXml.TabIndex = 105;
            this.btnDeleteHistoryXml.Text = "Xóa lịch sử báo cáo";
            this.btnDeleteHistoryXml.Click += new System.EventHandler(this.btnDeleteHistoryXml_Click);
            // 
            // btnLoadHistory
            // 
            this.btnLoadHistory.Location = new System.Drawing.Point(5, 27);
            this.btnLoadHistory.Name = "btnLoadHistory";
            this.btnLoadHistory.Size = new System.Drawing.Size(314, 22);
            this.btnLoadHistory.StyleController = this.lcMain;
            this.btnLoadHistory.TabIndex = 104;
            this.btnLoadHistory.Text = "Xem báo cáo";
            this.btnLoadHistory.Click += new System.EventHandler(this.btnLoadHistory_Click);
            // 
            // grdHistory
            // 
            this.grdHistory.Location = new System.Drawing.Point(8, 100);
            this.grdHistory.MainView = this.grdvHistory;
            this.grdHistory.MenuManager = this.barManager1;
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(623, 458);
            this.grdHistory.TabIndex = 103;
            this.grdHistory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdvHistory,
            this.gridView1});
            // 
            // grdvHistory
            // 
            this.grdvHistory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdvHistory_Ngay,
            this.grdvHistory_Domain,
            this.grdvHistory_Click});
            this.grdvHistory.GridControl = this.grdHistory;
            this.grdvHistory.GroupCount = 1;
            this.grdvHistory.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Click", this.grdvHistory_Click, "")});
            this.grdvHistory.Name = "grdvHistory";
            this.grdvHistory.OptionsBehavior.AutoExpandAllGroups = true;
            this.grdvHistory.OptionsBehavior.Editable = false;
            this.grdvHistory.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.grdvHistory_Ngay, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // grdvHistory_Ngay
            // 
            this.grdvHistory_Ngay.Caption = "Ngày";
            this.grdvHistory_Ngay.FieldName = "Ngay";
            this.grdvHistory_Ngay.Name = "grdvHistory_Ngay";
            this.grdvHistory_Ngay.Visible = true;
            this.grdvHistory_Ngay.VisibleIndex = 0;
            // 
            // grdvHistory_Domain
            // 
            this.grdvHistory_Domain.Caption = "Tên miền";
            this.grdvHistory_Domain.FieldName = "Domain";
            this.grdvHistory_Domain.Name = "grdvHistory_Domain";
            this.grdvHistory_Domain.Visible = true;
            this.grdvHistory_Domain.VisibleIndex = 0;
            // 
            // grdvHistory_Click
            // 
            this.grdvHistory_Click.Caption = "Số lượt click";
            this.grdvHistory_Click.FieldName = "Click";
            this.grdvHistory_Click.Name = "grdvHistory_Click";
            this.grdvHistory_Click.Visible = true;
            this.grdvHistory_Click.VisibleIndex = 1;
            // 
            // memProxyNote
            // 
            this.memProxyNote.EditValue = resources.GetString("memProxyNote.EditValue");
            this.memProxyNote.Location = new System.Drawing.Point(642, 359);
            this.memProxyNote.MenuManager = this.barManager1;
            this.memProxyNote.Name = "memProxyNote";
            this.memProxyNote.Size = new System.Drawing.Size(619, 192);
            this.memProxyNote.StyleController = this.lcMain;
            this.memProxyNote.TabIndex = 99;
            // 
            // memProxy
            // 
            this.memProxy.Location = new System.Drawing.Point(18, 364);
            this.memProxy.MenuManager = this.barManager1;
            this.memProxy.Name = "memProxy";
            this.memProxy.Properties.NullText = "115.12.32.44:8080:username:password";
            this.memProxy.Properties.NullValuePrompt = "Dữ liệu proxy mẫu";
            this.memProxy.Size = new System.Drawing.Size(612, 184);
            this.memProxy.StyleController = this.lcMain;
            this.memProxy.TabIndex = 98;
            // 
            // lbeUserAgentNotice
            // 
            this.lbeUserAgentNotice.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.lbeUserAgentNotice.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lbeUserAgentNotice.Appearance.Options.UseFont = true;
            this.lbeUserAgentNotice.Appearance.Options.UseForeColor = true;
            this.lbeUserAgentNotice.Location = new System.Drawing.Point(5, 27);
            this.lbeUserAgentNotice.Name = "lbeUserAgentNotice";
            this.lbeUserAgentNotice.Size = new System.Drawing.Size(721, 13);
            this.lbeUserAgentNotice.StyleController = this.lcMain;
            this.lbeUserAgentNotice.TabIndex = 96;
            this.lbeUserAgentNotice.Text = "Mỗi dòng User Agent đại diện cho 1 thiết bị + 1 trình duyệt và 1 hệ điều hành nhấ" +
    "t định. Các dòng có chứa từ \"Android\" hoặc \"iPhone\" là giả lập Mobile";
            // 
            // raiTypeProxy
            // 
            this.raiTypeProxy.EditValue = 1;
            this.raiTypeProxy.Location = new System.Drawing.Point(105, 211);
            this.raiTypeProxy.MenuManager = this.barManager1;
            this.raiTypeProxy.Name = "raiTypeProxy";
            this.raiTypeProxy.Properties.Columns = 6;
            this.raiTypeProxy.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Proxy (IP:Port:User:Pass)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "TinSoft Proxy"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(4, "OBC Proxy v1"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(5, "DCOM Proxy"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(7, "TMProxy")});
            this.raiTypeProxy.Size = new System.Drawing.Size(1162, 83);
            this.raiTypeProxy.StyleController = this.lcMain;
            toolTipTitleItem30.Text = "Hướng dẫn";
            toolTipItem30.LeftIndent = 6;
            toolTipItem30.Text = "Chọn nhà cung cấp Proxy";
            superToolTip30.Items.Add(toolTipTitleItem30);
            superToolTip30.Items.Add(toolTipItem30);
            this.raiTypeProxy.SuperTip = superToolTip30;
            this.raiTypeProxy.TabIndex = 92;
            this.raiTypeProxy.SelectedIndexChanged += new System.EventHandler(this.raiTypeProxy_SelectedIndexChanged);
            // 
            // txtDialUp
            // 
            this.txtDialUp.EditValue = "Viettel";
            this.txtDialUp.Location = new System.Drawing.Point(10, 259);
            this.txtDialUp.MenuManager = this.barManager1;
            this.txtDialUp.Name = "txtDialUp";
            this.txtDialUp.Size = new System.Drawing.Size(1256, 297);
            this.txtDialUp.StyleController = this.lcMain;
            toolTipTitleItem31.Text = "Hướng dẫn";
            toolTipItem31.LeftIndent = 6;
            toolTipItem31.Text = "Danh sách các Dial-Up của dcom. Mỗi dòng 1 DialUp. DialUp chính là tên cấu hình d" +
    "com khi thiết lập Profile. Nó thường được hiển thị trong Network Connection.";
            superToolTip31.Items.Add(toolTipTitleItem31);
            superToolTip31.Items.Add(toolTipItem31);
            this.txtDialUp.SuperTip = superToolTip31;
            this.txtDialUp.TabIndex = 91;
            // 
            // btnHomepage
            // 
            this.btnHomepage.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnHomepage.Appearance.Options.UseFont = true;
            this.btnHomepage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHomepage.ImageOptions.Image")));
            this.btnHomepage.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnHomepage.Location = new System.Drawing.Point(913, 569);
            this.btnHomepage.Name = "btnHomepage";
            this.btnHomepage.Size = new System.Drawing.Size(178, 51);
            this.btnHomepage.StyleController = this.lcMain;
            this.btnHomepage.TabIndex = 78;
            this.btnHomepage.Text = "Liên hệ";
            this.btnHomepage.Click += new System.EventHandler(this.btnHomepage_Click);
            // 
            // radTypeIp
            // 
            this.radTypeIp.EditValue = 0;
            this.radTypeIp.Location = new System.Drawing.Point(6, 92);
            this.radTypeIp.MenuManager = this.barManager1;
            this.radTypeIp.Name = "radTypeIp";
            this.radTypeIp.Properties.Columns = 3;
            this.radTypeIp.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "No change"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "DCOM (Hỗ trợ đa luồng)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Proxy (Hỗ trợ đa luồng)")});
            this.radTypeIp.Size = new System.Drawing.Size(1264, 90);
            this.radTypeIp.StyleController = this.lcMain;
            toolTipTitleItem32.Text = "Hướng dẫn/";
            toolTipItem32.LeftIndent = 6;
            toolTipItem32.Text = "Chọn hình thức thay đổi IP.";
            superToolTip32.Items.Add(toolTipTitleItem32);
            superToolTip32.Items.Add(toolTipItem32);
            this.radTypeIp.SuperTip = superToolTip32;
            this.radTypeIp.TabIndex = 81;
            this.radTypeIp.SelectedIndexChanged += new System.EventHandler(this.radTypeIp_SelectedIndexChanged);
            // 
            // cbeGoogleSite
            // 
            this.cbeGoogleSite.EditValue = "https://www.google.com";
            this.cbeGoogleSite.Location = new System.Drawing.Point(95, 46);
            this.cbeGoogleSite.MenuManager = this.barManager1;
            this.cbeGoogleSite.Name = "cbeGoogleSite";
            this.cbeGoogleSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeGoogleSite.Properties.Items.AddRange(new object[] {
            "https://www.google.com",
            "https://www.google.com.vn"});
            this.cbeGoogleSite.Size = new System.Drawing.Size(219, 20);
            this.cbeGoogleSite.StyleController = this.lcMain;
            toolTipTitleItem33.Text = "Hướng dẫn";
            toolTipItem33.LeftIndent = 6;
            toolTipItem33.Text = "Chọn tên miền Google cần duyệt. Tên miền .vn thì sẽ ưu tiên các kết quả ở Việt Na" +
    "m hơn.";
            superToolTip33.Items.Add(toolTipTitleItem33);
            superToolTip33.Items.Add(toolTipItem33);
            this.cbeGoogleSite.SuperTip = superToolTip33;
            this.cbeGoogleSite.TabIndex = 80;
            // 
            // speTimeout
            // 
            this.speTimeout.EditValue = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.speTimeout.Location = new System.Drawing.Point(475, 160);
            this.speTimeout.MenuManager = this.barManager1;
            this.speTimeout.Name = "speTimeout";
            this.speTimeout.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speTimeout.Properties.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.speTimeout.Properties.MinValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.speTimeout.Size = new System.Drawing.Size(293, 20);
            this.speTimeout.StyleController = this.lcMain;
            toolTipTitleItem34.Text = "Hướng dẫn";
            toolTipItem34.LeftIndent = 6;
            toolTipItem34.Text = "Là khoảng thời gian chờ tải trang. Sau thời gian này nếu trang tải chưa xong thì " +
    "sẽ bỏ qua và chạy bước tiếp theo. Khoảng thời gian này là cần thiết để gặp websi" +
    "te bị lỗi sẽ không bị đứng phần mềm.";
            superToolTip34.Items.Add(toolTipTitleItem34);
            superToolTip34.Items.Add(toolTipItem34);
            this.speTimeout.SuperTip = superToolTip34;
            this.speTimeout.TabIndex = 79;
            this.speTimeout.ToolTip = "Là khoảng thời gian chờ tải trang. Sau thời gian này nếu trang tải chưa xong thì " +
    "sẽ bỏ qua và chạy bước tiếp theo.";
            // 
            // speEmailDelay
            // 
            this.speEmailDelay.EditValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.speEmailDelay.Location = new System.Drawing.Point(40, 81);
            this.speEmailDelay.MenuManager = this.barManager1;
            this.speEmailDelay.Name = "speEmailDelay";
            this.speEmailDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speEmailDelay.Size = new System.Drawing.Size(596, 20);
            this.speEmailDelay.StyleController = this.lcMain;
            toolTipTitleItem35.Text = "Hướng dẫn";
            toolTipItem35.LeftIndent = 6;
            toolTipItem35.Text = "Độ trễ trong quá trình đăng nhập GMail";
            superToolTip35.Items.Add(toolTipTitleItem35);
            superToolTip35.Items.Add(toolTipItem35);
            this.speEmailDelay.SuperTip = superToolTip35;
            this.speEmailDelay.TabIndex = 78;
            // 
            // btnDeleteHistory
            // 
            this.btnDeleteHistory.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnDeleteHistory.Appearance.Options.UseFont = true;
            this.btnDeleteHistory.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnDeleteHistory.Location = new System.Drawing.Point(731, 569);
            this.btnDeleteHistory.Name = "btnDeleteHistory";
            this.btnDeleteHistory.Size = new System.Drawing.Size(178, 51);
            this.btnDeleteHistory.StyleController = this.lcMain;
            this.btnDeleteHistory.TabIndex = 77;
            this.btnDeleteHistory.Text = "Xóa lịch sử";
            this.btnDeleteHistory.Click += new System.EventHandler(this.btnDeleteHistory_Click);
            // 
            // memEmail
            // 
            this.memEmail.EditValue = "gmail1@gmail.com|password1|emailkhoiphuc@gmail.com";
            this.memEmail.Location = new System.Drawing.Point(8, 121);
            this.memEmail.MenuManager = this.barManager1;
            this.memEmail.Name = "memEmail";
            this.memEmail.Properties.NullText = "gmail1@gmail.com|password1|emailkhoiphuc@gmail.com";
            this.memEmail.Size = new System.Drawing.Size(1260, 437);
            this.memEmail.StyleController = this.lcMain;
            toolTipTitleItem36.Text = "Hướng dẫn";
            toolTipItem36.LeftIndent = 6;
            toolTipItem36.Text = "Danh sách tài khoản GMail để phần mềm tiến hành đăng nhập tự động. Hiện tại khuyế" +
    "n khích đăng nhập bằng Profile sẽ tiện và nhanh hơn nhiều.";
            superToolTip36.Items.Add(toolTipTitleItem36);
            superToolTip36.Items.Add(toolTipItem36);
            this.memEmail.SuperTip = superToolTip36;
            this.memEmail.TabIndex = 76;
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(185, 569);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(178, 51);
            this.btnSave.StyleController = this.lcMain;
            this.btnSave.TabIndex = 74;
            this.btnSave.Text = "Lưu cấu hình";
            this.btnSave.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // ceiNotViewImage
            // 
            this.ceiNotViewImage.Location = new System.Drawing.Point(1252, 211);
            this.ceiNotViewImage.MenuManager = this.barManager1;
            this.ceiNotViewImage.Name = "ceiNotViewImage";
            this.ceiNotViewImage.Properties.Caption = "";
            this.ceiNotViewImage.Size = new System.Drawing.Size(19, 19);
            this.ceiNotViewImage.StyleController = this.lcMain;
            toolTipTitleItem37.Text = "Hướng dẫn";
            toolTipItem37.LeftIndent = 6;
            toolTipItem37.Text = "Có hiển thị hình ảnh ở trang mục tiêu hay không? Chức năng mục đích để tiết kiệm " +
    "dung lượng 3G/4G và tăng tốc độ load web.";
            superToolTip37.Items.Add(toolTipTitleItem37);
            superToolTip37.Items.Add(toolTipItem37);
            this.ceiNotViewImage.SuperTip = superToolTip37;
            this.ceiNotViewImage.TabIndex = 73;
            // 
            // ceiUseHistory
            // 
            this.ceiUseHistory.EditValue = true;
            this.ceiUseHistory.Location = new System.Drawing.Point(249, 160);
            this.ceiUseHistory.MenuManager = this.barManager1;
            this.ceiUseHistory.Name = "ceiUseHistory";
            this.ceiUseHistory.Properties.Caption = "";
            this.ceiUseHistory.Size = new System.Drawing.Size(19, 19);
            this.ceiUseHistory.StyleController = this.lcMain;
            this.ceiUseHistory.TabIndex = 71;
            this.ceiUseHistory.ToolTip = "Nếu phần mềm ổn định và cắm máy cả ngày thì không cần ghi lại lịch sử để giảm tài" +
    " nguyên bộ nhớ";
            // 
            // speTimeViewTo
            // 
            this.speTimeViewTo.EditValue = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.speTimeViewTo.Location = new System.Drawing.Point(334, 114);
            this.speTimeViewTo.MenuManager = this.barManager1;
            this.speTimeViewTo.Name = "speTimeViewTo";
            this.speTimeViewTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speTimeViewTo.Properties.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.speTimeViewTo.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speTimeViewTo.Size = new System.Drawing.Size(174, 20);
            this.speTimeViewTo.StyleController = this.lcMain;
            toolTipTitleItem38.Text = "Hướng dẫn";
            toolTipItem38.LeftIndent = 6;
            toolTipItem38.Text = "Khoảng thời gian xem trang web mục tiêu tối đa. Tại mỗi lần click thì phần mềm sẽ" +
    " lấy một con số ngẫu nhiên trong khoảng thời gian thiết lập để tạo ra tính tự nh" +
    "iên.";
            superToolTip38.Items.Add(toolTipTitleItem38);
            superToolTip38.Items.Add(toolTipItem38);
            this.speTimeViewTo.SuperTip = superToolTip38;
            this.speTimeViewTo.TabIndex = 69;
            this.speTimeViewTo.ToolTip = "Khoảng thời gian xem trang web mục tiêu tối đa";
            this.speTimeViewTo.EditValueChanged += new System.EventHandler(this.speTimeViewTo_EditValueChanged);
            // 
            // speTimeViewSearch
            // 
            this.speTimeViewSearch.EditValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.speTimeViewSearch.Location = new System.Drawing.Point(699, 70);
            this.speTimeViewSearch.MenuManager = this.barManager1;
            this.speTimeViewSearch.Name = "speTimeViewSearch";
            this.speTimeViewSearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speTimeViewSearch.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.speTimeViewSearch.Properties.MinValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.speTimeViewSearch.Size = new System.Drawing.Size(254, 20);
            this.speTimeViewSearch.StyleController = this.lcMain;
            toolTipTitleItem39.Text = "Hướng dẫn";
            toolTipItem39.LeftIndent = 6;
            toolTipItem39.Text = "Là khoảng thời gian phần mềm sẽ lướt như người dùng thật trên trang Google sau kh" +
    "i nhập từ khóa tìm kiếm";
            superToolTip39.Items.Add(toolTipTitleItem39);
            superToolTip39.Items.Add(toolTipItem39);
            this.speTimeViewSearch.SuperTip = superToolTip39;
            this.speTimeViewSearch.TabIndex = 68;
            this.speTimeViewSearch.ToolTip = "Khoảng thời gian duyệt trang tìm kiếm google để giống như người thật, tránh để qu" +
    "á nhanh sẽ ảnh hưởng đến hiệu quả.";
            this.speTimeViewSearch.EditValueChanged += new System.EventHandler(this.speTimeViewSearch_EditValueChanged);
            // 
            // txtOtherSiteUrl
            // 
            this.txtOtherSiteUrl.EditValue = "https://na.com.vn\r\nhttps://na.com.vn/phan-mem-seo-traffic\r\nhttps://NATech.vn/phan" +
    "-mem-seo-shopee\r\nhttps://na.com.vn/phan-mem-dau-gia-tiki-tu-dong";
            this.txtOtherSiteUrl.Location = new System.Drawing.Point(6, 488);
            this.txtOtherSiteUrl.MenuManager = this.barManager1;
            this.txtOtherSiteUrl.Name = "txtOtherSiteUrl";
            this.txtOtherSiteUrl.Properties.NullText = "https://g.page/ChaBoDaNang";
            this.txtOtherSiteUrl.Size = new System.Drawing.Size(1264, 72);
            this.txtOtherSiteUrl.StyleController = this.lcMain;
            toolTipTitleItem40.Text = "Hướng dẫn";
            toolTipItem40.LeftIndent = 6;
            superToolTip40.Items.Add(toolTipTitleItem40);
            superToolTip40.Items.Add(toolTipItem40);
            this.txtOtherSiteUrl.SuperTip = superToolTip40;
            this.txtOtherSiteUrl.TabIndex = 67;
            this.txtOtherSiteUrl.ToolTip = "Để càng nhiều site càng tốt, phần mềm sẽ lấy ngẫu nhiên.";
            // 
            // speOtherSiteViewTime
            // 
            this.speOtherSiteViewTime.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.speOtherSiteViewTime.Location = new System.Drawing.Point(64, 446);
            this.speOtherSiteViewTime.MenuManager = this.barManager1;
            this.speOtherSiteViewTime.Name = "speOtherSiteViewTime";
            this.speOtherSiteViewTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speOtherSiteViewTime.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.speOtherSiteViewTime.Properties.MinValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.speOtherSiteViewTime.Size = new System.Drawing.Size(572, 20);
            this.speOtherSiteViewTime.StyleController = this.lcMain;
            toolTipTitleItem41.Text = "Hướng dẫn";
            toolTipItem41.LeftIndent = 6;
            toolTipItem41.Text = "Mốc thời gian tối thiểu để phần mềm lấy duyệt trang khác, nếu kéo Traffic thì nên" +
    " để thời gian dài (trên 2p) để tránh tăng tỷ lệ thoát. Còn nếu để trang bất kỳ t" +
    "hì có thể để thấp hơn.";
            superToolTip41.Items.Add(toolTipTitleItem41);
            superToolTip41.Items.Add(toolTipItem41);
            this.speOtherSiteViewTime.SuperTip = superToolTip41;
            this.speOtherSiteViewTime.TabIndex = 66;
            this.speOtherSiteViewTime.ToolTip = "Khoảng thời gian xem trang web khác, nên để trên 5s";
            this.speOtherSiteViewTime.EditValueChanged += new System.EventHandler(this.speOtherSiteViewTime_EditValueChanged);
            // 
            // ceiViewOtherSite
            // 
            this.ceiViewOtherSite.Location = new System.Drawing.Point(51, 423);
            this.ceiViewOtherSite.MenuManager = this.barManager1;
            this.ceiViewOtherSite.Name = "ceiViewOtherSite";
            this.ceiViewOtherSite.Properties.Caption = "";
            this.ceiViewOtherSite.Size = new System.Drawing.Size(1219, 19);
            this.ceiViewOtherSite.StyleController = this.lcMain;
            toolTipTitleItem42.Text = "Có duyệt thêm trang khác sau khi duyệt trang chính không?";
            toolTipItem42.LeftIndent = 6;
            toolTipItem42.Text = "Sau khi xem Trang chính xong, phần mềm sẽ tiến hành lấy 1 trong những site bên dư" +
    "ới để tiến hành truy cập để tăng tính tự nhiên. Lưu ý: Site bên dưới phải là lin" +
    "k đầy đủ (có cả http...)";
            superToolTip42.Items.Add(toolTipTitleItem42);
            superToolTip42.Items.Add(toolTipItem42);
            this.ceiViewOtherSite.SuperTip = superToolTip42;
            this.ceiViewOtherSite.TabIndex = 64;
            this.ceiViewOtherSite.ToolTip = "Có duyệt 1 trang web khác sau khi xem quảng cáo ở trang mục tiêu hay không";
            this.ceiViewOtherSite.CheckedChanged += new System.EventHandler(this.ceiViewOtherSite_CheckedChanged);
            // 
            // txtAgent
            // 
            this.txtAgent.EditValue = "";
            this.txtAgent.Location = new System.Drawing.Point(5, 53);
            this.txtAgent.MenuManager = this.barManager1;
            this.txtAgent.Name = "txtAgent";
            this.txtAgent.Size = new System.Drawing.Size(1266, 508);
            this.txtAgent.StyleController = this.lcMain;
            toolTipTitleItem43.Text = "Hướng dẫn";
            toolTipItem43.LeftIndent = 6;
            toolTipItem43.Text = "Mỗi dòng là 1 UserAgent. Mỗi UserAgent đại diện cho một Thiết bị + Trình duyệt + " +
    "Hệ điều hành khác nhau. Mục đích để phần mềm giả lập như người dùng ngẫu nhiên ở" +
    " một thiết bị bất kỳ.\r\n";
            superToolTip43.Items.Add(toolTipTitleItem43);
            superToolTip43.Items.Add(toolTipItem43);
            this.txtAgent.SuperTip = superToolTip43;
            this.txtAgent.TabIndex = 63;
            this.txtAgent.EditValueChanged += new System.EventHandler(this.txtAgent_EditValueChanged);
            // 
            // speSoTrang
            // 
            this.speSoTrang.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.speSoTrang.Location = new System.Drawing.Point(585, 70);
            this.speSoTrang.Name = "speSoTrang";
            this.speSoTrang.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speSoTrang.Properties.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.speSoTrang.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speSoTrang.Size = new System.Drawing.Size(50, 20);
            this.speSoTrang.StyleController = this.lcMain;
            toolTipTitleItem44.Text = "Hướng dẫn";
            toolTipItem44.LeftIndent = 6;
            toolTipItem44.Text = "Số trang kết quả phần mềm sẽ quét tìm link trên Google. (vd: Bạn để 2 thì phần mề" +
    "m sẽ kiểm tra trên 2 trang đầu tiên của Google)";
            superToolTip44.Items.Add(toolTipTitleItem44);
            superToolTip44.Items.Add(toolTipItem44);
            this.speSoTrang.SuperTip = superToolTip44;
            this.speSoTrang.TabIndex = 62;
            this.speSoTrang.ToolTip = "Là số trang kết quả tìm kiếm trên google. Nếu tìm ở trang 1 không có thì qua tran" +
    "g 2 tìm kiếm.";
            // 
            // speSumClick
            // 
            this.speSumClick.EditValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.speSumClick.Location = new System.Drawing.Point(508, 46);
            this.speSumClick.Name = "speSumClick";
            this.speSumClick.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speSumClick.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.speSumClick.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speSumClick.Size = new System.Drawing.Size(127, 20);
            this.speSumClick.StyleController = this.lcMain;
            toolTipTitleItem45.Text = "Hướng dẫn";
            toolTipItem45.LeftIndent = 6;
            toolTipItem45.Text = "Số vòng lặp lại. Chạy hết thì phần mềm sẽ dừng lại.";
            superToolTip45.Items.Add(toolTipTitleItem45);
            superToolTip45.Items.Add(toolTipItem45);
            this.speSumClick.SuperTip = superToolTip45;
            this.speSumClick.TabIndex = 53;
            this.speSumClick.ToolTip = "Số lần click quảng cáo vào trang web mục tiêu";
            // 
            // speTimeViewFrom
            // 
            this.speTimeViewFrom.EditValue = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.speTimeViewFrom.Location = new System.Drawing.Point(111, 114);
            this.speTimeViewFrom.Name = "speTimeViewFrom";
            this.speTimeViewFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speTimeViewFrom.Properties.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.speTimeViewFrom.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speTimeViewFrom.Size = new System.Drawing.Size(144, 20);
            this.speTimeViewFrom.StyleController = this.lcMain;
            toolTipTitleItem46.Text = "Hướng dẫn";
            toolTipItem46.LeftIndent = 6;
            toolTipItem46.Text = "Khoảng thời gian xem trang web mục tiêu tối thiểu. Tại mỗi lần click thì phần mềm" +
    " sẽ lấy một con số ngẫu nhiên trong khoảng thời gian thiết lập để tạo ra tính tự" +
    " nhiên.";
            superToolTip46.Items.Add(toolTipTitleItem46);
            superToolTip46.Items.Add(toolTipItem46);
            this.speTimeViewFrom.SuperTip = superToolTip46;
            this.speTimeViewFrom.TabIndex = 49;
            this.speTimeViewFrom.ToolTip = "Khoảng thời gian xem trang web mục tiêu tối thiểu";
            this.speTimeViewFrom.EditValueChanged += new System.EventHandler(this.speTimeViewFrom_EditValueChanged);
            // 
            // btnStop
            // 
            this.btnStop.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnStop.Appearance.Options.UseFont = true;
            this.btnStop.Location = new System.Drawing.Point(549, 569);
            this.btnStop.MinimumSize = new System.Drawing.Size(0, 35);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(178, 51);
            this.btnStop.StyleController = this.lcMain;
            this.btnStop.TabIndex = 35;
            this.btnStop.Text = "Kết thúc";
            this.btnStop.ToolTip = "Kết thúc quá trình tự động click quảng cáo";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.tabMain});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1276, 623);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem4,
            this.layoutControlItem6,
            this.layoutControlItem9,
            this.layoutControlItem14,
            this.layoutControlItem19,
            this.layoutControlItem30,
            this.layoutControlItem1});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 566);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(1276, 57);
            this.layoutControlGroup2.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.CustomizationFormText = "emptySpaceItem4";
            this.emptySpaceItem4.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(182, 55);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnRun;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(364, 0);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(36, 30);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(182, 55);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btnStop;
            this.layoutControlItem9.CustomizationFormText = "layoutControlItem9";
            this.layoutControlItem9.Location = new System.Drawing.Point(546, 0);
            this.layoutControlItem9.MinSize = new System.Drawing.Size(97, 39);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(182, 55);
            this.layoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btnSave;
            this.layoutControlItem14.CustomizationFormText = "layoutControlItem14";
            this.layoutControlItem14.Location = new System.Drawing.Point(182, 0);
            this.layoutControlItem14.MinSize = new System.Drawing.Size(82, 26);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(182, 55);
            this.layoutControlItem14.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // layoutControlItem19
            // 
            this.layoutControlItem19.Control = this.btnDeleteHistory;
            this.layoutControlItem19.CustomizationFormText = "layoutControlItem19";
            this.layoutControlItem19.Location = new System.Drawing.Point(728, 0);
            this.layoutControlItem19.MinSize = new System.Drawing.Size(82, 26);
            this.layoutControlItem19.Name = "layoutControlItem19";
            this.layoutControlItem19.Size = new System.Drawing.Size(182, 55);
            this.layoutControlItem19.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem19.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem19.TextVisible = false;
            // 
            // layoutControlItem30
            // 
            this.layoutControlItem30.Control = this.btnHomepage;
            this.layoutControlItem30.CustomizationFormText = "layoutControlItem30";
            this.layoutControlItem30.Location = new System.Drawing.Point(910, 0);
            this.layoutControlItem30.MinSize = new System.Drawing.Size(71, 26);
            this.layoutControlItem30.Name = "layoutControlItem30";
            this.layoutControlItem30.Size = new System.Drawing.Size(182, 55);
            this.layoutControlItem30.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem30.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem30.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.memReChuot;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(1092, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(182, 55);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // tabMain
            // 
            this.tabMain.CustomizationFormText = "tabbedControlGroup1";
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.tabMain.SelectedTabPage = this.lcgMain;
            this.tabMain.SelectedTabPageIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1276, 566);
            this.tabMain.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgMain,
            this.lcgTimeSetup,
            this.lcgLoginGmail,
            this.lcgUserAgent,
            this.lcgFakeIP,
            this.lcgReport});
            this.tabMain.Text = "Agent";
            // 
            // lcgMain
            // 
            this.lcgMain.CustomizationFormText = "Thiết lập";
            this.lcgMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup8});
            this.lcgMain.Location = new System.Drawing.Point(0, 0);
            this.lcgMain.Name = "lcgMain";
            this.lcgMain.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgMain.Size = new System.Drawing.Size(1270, 538);
            this.lcgMain.Text = "Trang chính";
            // 
            // layoutControlGroup8
            // 
            this.layoutControlGroup8.ContentImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.layoutControlGroup8.CustomizationFormText = "Thông tin chung";
            this.layoutControlGroup8.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lgTuKhoa});
            this.layoutControlGroup8.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup8.Name = "layoutControlGroup8";
            this.layoutControlGroup8.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup8.Size = new System.Drawing.Size(1270, 538);
            this.layoutControlGroup8.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup8.Text = "Thông tin chung";
            this.layoutControlGroup8.TextVisible = false;
            // 
            // lgTuKhoa
            // 
            this.lgTuKhoa.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lgTuKhoa.AppearanceGroup.Options.UseFont = true;
            this.lgTuKhoa.CustomizationFormText = "Từ khóa";
            this.lgTuKhoa.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem20,
            this.layoutControlItem5,
            this.layoutControlItem24,
            this.layoutControlItem16,
            this.layoutControlItem28,
            this.emptySpaceItem14,
            this.lcgHistory,
            this.splitterItem3});
            this.lgTuKhoa.Location = new System.Drawing.Point(0, 0);
            this.lgTuKhoa.Name = "lgTuKhoa";
            this.lgTuKhoa.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lgTuKhoa.Size = new System.Drawing.Size(1268, 536);
            this.lgTuKhoa.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lgTuKhoa.Text = "Từ khóa (Mỗi từ khóa 1 dòng). Nếu có lỗi thì rê chuột vào dấu X đỏ để đọc lỗi";
            // 
            // layoutControlItem20
            // 
            this.layoutControlItem20.Control = this.grdKeyword;
            this.layoutControlItem20.CustomizationFormText = "layoutControlItem20";
            this.layoutControlItem20.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem20.Name = "layoutControlItem20";
            this.layoutControlItem20.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem20.Size = new System.Drawing.Size(1266, 171);
            this.layoutControlItem20.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem20.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnExportKeyword;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 176);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(253, 26);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem24
            // 
            this.layoutControlItem24.Control = this.btnClearSelectionKeyword;
            this.layoutControlItem24.CustomizationFormText = "layoutControlItem24";
            this.layoutControlItem24.Location = new System.Drawing.Point(506, 176);
            this.layoutControlItem24.Name = "layoutControlItem24";
            this.layoutControlItem24.Size = new System.Drawing.Size(253, 26);
            this.layoutControlItem24.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem24.TextVisible = false;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Control = this.btnImportKeyword;
            this.layoutControlItem16.CustomizationFormText = "layoutControlItem16";
            this.layoutControlItem16.Location = new System.Drawing.Point(253, 176);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Size = new System.Drawing.Size(253, 26);
            this.layoutControlItem16.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem16.TextVisible = false;
            // 
            // layoutControlItem28
            // 
            this.layoutControlItem28.Control = this.btnClearAllKeyword;
            this.layoutControlItem28.CustomizationFormText = "layoutControlItem28";
            this.layoutControlItem28.Location = new System.Drawing.Point(759, 176);
            this.layoutControlItem28.Name = "layoutControlItem28";
            this.layoutControlItem28.Size = new System.Drawing.Size(254, 26);
            this.layoutControlItem28.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem28.TextVisible = false;
            // 
            // emptySpaceItem14
            // 
            this.emptySpaceItem14.AllowHotTrack = false;
            this.emptySpaceItem14.CustomizationFormText = "emptySpaceItem14";
            this.emptySpaceItem14.Location = new System.Drawing.Point(1013, 176);
            this.emptySpaceItem14.Name = "emptySpaceItem14";
            this.emptySpaceItem14.Size = new System.Drawing.Size(253, 26);
            this.emptySpaceItem14.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcgHistory
            // 
            this.lcgHistory.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcgHistory.AppearanceGroup.Options.UseFont = true;
            this.lcgHistory.CustomizationFormText = "Quá trình thao tác của phần mềm";
            this.lcgHistory.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciHistory});
            this.lcgHistory.Location = new System.Drawing.Point(0, 202);
            this.lcgHistory.Name = "lcgHistory";
            this.lcgHistory.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgHistory.Size = new System.Drawing.Size(1266, 314);
            this.lcgHistory.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgHistory.Text = "Quá trình thao tác của phần mềm (Lưu ý: Khi cắm máy chạy lâu nên bỏ \"Ghi lịch sử\"" +
    " để tránh tràn RAM)";
            // 
            // lciHistory
            // 
            this.lciHistory.Control = this.mmeHistory;
            this.lciHistory.CustomizationFormText = "Lịch sử";
            this.lciHistory.Location = new System.Drawing.Point(0, 0);
            this.lciHistory.Name = "lciHistory";
            this.lciHistory.Size = new System.Drawing.Size(1264, 294);
            this.lciHistory.Text = "Lịch sử";
            this.lciHistory.TextLocation = DevExpress.Utils.Locations.Top;
            this.lciHistory.TextSize = new System.Drawing.Size(0, 0);
            this.lciHistory.TextVisible = false;
            // 
            // splitterItem3
            // 
            this.splitterItem3.AllowHotTrack = true;
            this.splitterItem3.CustomizationFormText = "splitterItem3";
            this.splitterItem3.Location = new System.Drawing.Point(0, 171);
            this.splitterItem3.Name = "splitterItem3";
            this.splitterItem3.Size = new System.Drawing.Size(1266, 5);
            // 
            // lcgTimeSetup
            // 
            this.lcgTimeSetup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lgTraffic,
            this.lcgSettings,
            this.lcgOtherConfig,
            this.lcgTimeGoogle,
            this.lcgViewAds,
            this.lcgTimeInternalExternal,
            this.layoutControlItem39,
            this.lciCallPhoneZalo,
            this.lciViewYoutube,
            this.lciNotViewImage,
            this.emptySpaceItem16});
            this.lcgTimeSetup.Location = new System.Drawing.Point(0, 0);
            this.lcgTimeSetup.Name = "lcgTimeSetup";
            this.lcgTimeSetup.OptionsItemText.TextToControlDistance = 5;
            this.lcgTimeSetup.Size = new System.Drawing.Size(1270, 538);
            this.lcgTimeSetup.Text = "Thiết lập tham số thời gian";
            // 
            // lgTraffic
            // 
            this.lgTraffic.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lgTraffic.AppearanceGroup.Options.UseFont = true;
            this.lgTraffic.CustomizationFormText = "Duyệt web bất kỳ sau khi xem quảng cáo";
            this.lgTraffic.ExpandButtonVisible = true;
            this.lgTraffic.ExpandOnDoubleClick = true;
            this.lgTraffic.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciSuDung,
            this.lciOtherSiteListUrl,
            this.lciOtherSiteViewTime,
            this.lciOtherSiteViewTimeTo});
            this.lgTraffic.Location = new System.Drawing.Point(0, 377);
            this.lgTraffic.Name = "lgTraffic";
            this.lgTraffic.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lgTraffic.Size = new System.Drawing.Size(1270, 161);
            this.lgTraffic.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lgTraffic.Text = "Duyệt thêm sau khi xem trang";
            this.lgTraffic.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciSuDung
            // 
            this.lciSuDung.Control = this.ceiViewOtherSite;
            this.lciSuDung.CustomizationFormText = "Duyệt web bất kỳ sau khi xem";
            this.lciSuDung.Location = new System.Drawing.Point(0, 0);
            this.lciSuDung.Name = "lciSuDung";
            this.lciSuDung.Size = new System.Drawing.Size(1268, 23);
            this.lciSuDung.Text = "Sử dụng";
            this.lciSuDung.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciSuDung.TextSize = new System.Drawing.Size(40, 13);
            this.lciSuDung.TextToControlDistance = 5;
            // 
            // lciOtherSiteListUrl
            // 
            this.lciOtherSiteListUrl.Control = this.txtOtherSiteUrl;
            this.lciOtherSiteListUrl.CustomizationFormText = "Danh sách website duyệt sau khi xem quảng cáo (Mỗi dòng là 1 site)";
            this.lciOtherSiteListUrl.Location = new System.Drawing.Point(0, 47);
            this.lciOtherSiteListUrl.Name = "lciOtherSiteListUrl";
            this.lciOtherSiteListUrl.Size = new System.Drawing.Size(1268, 94);
            this.lciOtherSiteListUrl.Text = "Danh sách website(Mỗi dòng là 1 site)";
            this.lciOtherSiteListUrl.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciOtherSiteListUrl.TextLocation = DevExpress.Utils.Locations.Top;
            this.lciOtherSiteListUrl.TextSize = new System.Drawing.Size(181, 13);
            this.lciOtherSiteListUrl.TextToControlDistance = 5;
            // 
            // lciOtherSiteViewTime
            // 
            this.lciOtherSiteViewTime.Control = this.speOtherSiteViewTime;
            this.lciOtherSiteViewTime.CustomizationFormText = "Thời gian xem (s)";
            this.lciOtherSiteViewTime.Location = new System.Drawing.Point(0, 23);
            this.lciOtherSiteViewTime.Name = "lciOtherSiteViewTime";
            this.lciOtherSiteViewTime.Size = new System.Drawing.Size(634, 24);
            this.lciOtherSiteViewTime.Text = "Random từ";
            this.lciOtherSiteViewTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciOtherSiteViewTime.TextSize = new System.Drawing.Size(53, 13);
            this.lciOtherSiteViewTime.TextToControlDistance = 5;
            // 
            // lciOtherSiteViewTimeTo
            // 
            this.lciOtherSiteViewTimeTo.Control = this.speOtherSiteViewTimeTo;
            this.lciOtherSiteViewTimeTo.CustomizationFormText = "Đến";
            this.lciOtherSiteViewTimeTo.Location = new System.Drawing.Point(634, 23);
            this.lciOtherSiteViewTimeTo.Name = "lciOtherSiteViewTimeTo";
            this.lciOtherSiteViewTimeTo.Size = new System.Drawing.Size(634, 24);
            this.lciOtherSiteViewTimeTo.Text = "Đến";
            this.lciOtherSiteViewTimeTo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciOtherSiteViewTimeTo.TextSize = new System.Drawing.Size(20, 13);
            this.lciOtherSiteViewTimeTo.TextToControlDistance = 5;
            // 
            // lcgSettings
            // 
            this.lcgSettings.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcgSettings.AppearanceGroup.Options.UseFont = true;
            this.lcgSettings.CustomizationFormText = "Thiết lập tham số tìm kiếm";
            this.lcgSettings.ExpandButtonVisible = true;
            this.lcgSettings.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciGoogleSite,
            this.lciSoLanClick,
            this.lciSoLuong,
            this.lciDuyet});
            this.lcgSettings.Location = new System.Drawing.Point(0, 0);
            this.lcgSettings.Name = "lcgSettings";
            this.lcgSettings.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgSettings.Size = new System.Drawing.Size(635, 68);
            this.lcgSettings.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgSettings.Text = "Thiết lập tham số tìm kiếm trên Google/Youtube";
            // 
            // lciGoogleSite
            // 
            this.lciGoogleSite.Control = this.cbeGoogleSite;
            this.lciGoogleSite.CustomizationFormText = "Google";
            this.lciGoogleSite.Location = new System.Drawing.Point(0, 0);
            this.lciGoogleSite.Name = "lciGoogleSite";
            this.lciGoogleSite.Size = new System.Drawing.Size(312, 24);
            this.lciGoogleSite.Text = "Trang chủ Google";
            this.lciGoogleSite.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciGoogleSite.TextSize = new System.Drawing.Size(84, 13);
            this.lciGoogleSite.TextToControlDistance = 5;
            // 
            // lciSoLanClick
            // 
            this.lciSoLanClick.Control = this.speSumClick;
            this.lciSoLanClick.CustomizationFormText = "Số lần muốn bơm";
            this.lciSoLanClick.Location = new System.Drawing.Point(312, 0);
            this.lciSoLanClick.Name = "lciSoLanClick";
            this.lciSoLanClick.Size = new System.Drawing.Size(321, 24);
            this.lciSoLanClick.Text = "Tổng số lượt Click vào kết quả tìm kiếm";
            this.lciSoLanClick.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciSoLanClick.TextSize = new System.Drawing.Size(185, 13);
            this.lciSoLanClick.TextToControlDistance = 5;
            // 
            // lciSoLuong
            // 
            this.lciSoLuong.Control = this.speSoLuong;
            this.lciSoLuong.CustomizationFormText = "Số luồng";
            this.lciSoLuong.Location = new System.Drawing.Point(0, 24);
            this.lciSoLuong.Name = "lciSoLuong";
            this.lciSoLuong.Size = new System.Drawing.Size(312, 24);
            this.lciSoLuong.Text = "Số luồng/1 Proxy";
            this.lciSoLuong.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciSoLuong.TextSize = new System.Drawing.Size(82, 13);
            this.lciSoLuong.TextToControlDistance = 5;
            // 
            // lciDuyet
            // 
            this.lciDuyet.Control = this.speSoTrang;
            this.lciDuyet.CustomizationFormText = "Số trang tìm kiếm";
            this.lciDuyet.Location = new System.Drawing.Point(312, 24);
            this.lciDuyet.Name = "lciDuyet";
            this.lciDuyet.Size = new System.Drawing.Size(321, 24);
            this.lciDuyet.Text = "Số trang kết quả mà tool tìm kiếm trên Google/Youtube";
            this.lciDuyet.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciDuyet.TextSize = new System.Drawing.Size(262, 13);
            this.lciDuyet.TextToControlDistance = 5;
            // 
            // lcgOtherConfig
            // 
            this.lcgOtherConfig.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lcgOtherConfig.AppearanceGroup.Options.UseFont = true;
            this.lcgOtherConfig.CustomizationFormText = "Tham số khác";
            this.lcgOtherConfig.ExpandButtonVisible = true;
            this.lcgOtherConfig.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciUseHistory,
            this.lciAutoStart,
            this.lciTimeout,
            this.lciLoadProfilePercent,
            this.lciStarupWindow,
            this.lciDisplayMode,
            this.lciClearChrome,
            this.layoutControlItem40,
            this.lciBrowserLanguage});
            this.lcgOtherConfig.Location = new System.Drawing.Point(0, 112);
            this.lcgOtherConfig.Name = "lcgOtherConfig";
            this.lcgOtherConfig.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgOtherConfig.Size = new System.Drawing.Size(1270, 72);
            this.lcgOtherConfig.Text = "Tham số khác";
            // 
            // lciUseHistory
            // 
            this.lciUseHistory.Control = this.ceiUseHistory;
            this.lciUseHistory.CustomizationFormText = "Ghi lịch sử";
            this.lciUseHistory.Location = new System.Drawing.Point(0, 0);
            this.lciUseHistory.Name = "lciUseHistory";
            this.lciUseHistory.Size = new System.Drawing.Size(264, 24);
            this.lciUseHistory.Text = "Hiển thị lịch sử quá trình thao tác của tool không?";
            this.lciUseHistory.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciUseHistory.TextSize = new System.Drawing.Size(236, 13);
            this.lciUseHistory.TextToControlDistance = 5;
            // 
            // lciAutoStart
            // 
            this.lciAutoStart.Control = this.ceiAutoStart;
            this.lciAutoStart.CustomizationFormText = "Tự động [Bắt đầu] sau khi mở phần mềm";
            this.lciAutoStart.Location = new System.Drawing.Point(0, 24);
            this.lciAutoStart.Name = "lciAutoStart";
            this.lciAutoStart.Size = new System.Drawing.Size(264, 24);
            this.lciAutoStart.Text = "Tự động click [Bắt đầu] sau khi mở phần mềm";
            this.lciAutoStart.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciAutoStart.TextSize = new System.Drawing.Size(215, 13);
            this.lciAutoStart.TextToControlDistance = 5;
            // 
            // lciTimeout
            // 
            this.lciTimeout.Control = this.speTimeout;
            this.lciTimeout.CustomizationFormText = "Timeout (s)";
            this.lciTimeout.Location = new System.Drawing.Point(264, 0);
            this.lciTimeout.Name = "lciTimeout";
            this.lciTimeout.Size = new System.Drawing.Size(500, 24);
            this.lciTimeout.Text = "Thời gian tự đóng trình duyệt khi lỗi mạng";
            this.lciTimeout.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciTimeout.TextSize = new System.Drawing.Size(198, 13);
            this.lciTimeout.TextToControlDistance = 5;
            // 
            // lciLoadProfilePercent
            // 
            this.lciLoadProfilePercent.Control = this.speLoadProfilePercent;
            this.lciLoadProfilePercent.CustomizationFormText = "Tỷ lệ load profile có sẵn";
            this.lciLoadProfilePercent.Location = new System.Drawing.Point(514, 24);
            this.lciLoadProfilePercent.Name = "lciLoadProfilePercent";
            this.lciLoadProfilePercent.Size = new System.Drawing.Size(250, 24);
            this.lciLoadProfilePercent.Text = "Tỷ lệ % chạy profile có sẵn";
            this.lciLoadProfilePercent.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciLoadProfilePercent.TextSize = new System.Drawing.Size(130, 13);
            this.lciLoadProfilePercent.TextToControlDistance = 5;
            // 
            // lciStarupWindow
            // 
            this.lciStarupWindow.Control = this.ceiStarupWindow;
            this.lciStarupWindow.Location = new System.Drawing.Point(264, 24);
            this.lciStarupWindow.Name = "lciStarupWindow";
            this.lciStarupWindow.Size = new System.Drawing.Size(250, 24);
            this.lciStarupWindow.Text = "Khởi động cùng Window";
            this.lciStarupWindow.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciStarupWindow.TextSize = new System.Drawing.Size(114, 13);
            this.lciStarupWindow.TextToControlDistance = 5;
            // 
            // lciDisplayMode
            // 
            this.lciDisplayMode.Control = this.lueDisplayMode;
            this.lciDisplayMode.Location = new System.Drawing.Point(764, 0);
            this.lciDisplayMode.Name = "lciDisplayMode";
            this.lciDisplayMode.Size = new System.Drawing.Size(250, 24);
            this.lciDisplayMode.Text = "Chế độ hiển thị";
            this.lciDisplayMode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciDisplayMode.TextSize = new System.Drawing.Size(72, 13);
            this.lciDisplayMode.TextToControlDistance = 5;
            // 
            // lciClearChrome
            // 
            this.lciClearChrome.Control = this.ceiClearChrome;
            this.lciClearChrome.Location = new System.Drawing.Point(764, 24);
            this.lciClearChrome.Name = "lciClearChrome";
            this.lciClearChrome.Size = new System.Drawing.Size(250, 24);
            this.lciClearChrome.Text = "Tự động giải phóng RAM?";
            this.lciClearChrome.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciClearChrome.TextSize = new System.Drawing.Size(122, 13);
            this.lciClearChrome.TextToControlDistance = 5;
            // 
            // layoutControlItem40
            // 
            this.layoutControlItem40.Control = this.speClearChromeTime;
            this.layoutControlItem40.Location = new System.Drawing.Point(1014, 24);
            this.layoutControlItem40.Name = "layoutControlItem40";
            this.layoutControlItem40.Size = new System.Drawing.Size(250, 24);
            this.layoutControlItem40.Text = "Thời gian giải phóng RAM (phút)";
            this.layoutControlItem40.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem40.TextSize = new System.Drawing.Size(153, 13);
            this.layoutControlItem40.TextToControlDistance = 5;
            // 
            // lciBrowserLanguage
            // 
            this.lciBrowserLanguage.Control = this.ccbBrowserLanguage;
            this.lciBrowserLanguage.Location = new System.Drawing.Point(1014, 0);
            this.lciBrowserLanguage.Name = "lciBrowserLanguage";
            this.lciBrowserLanguage.Size = new System.Drawing.Size(250, 24);
            this.lciBrowserLanguage.Text = "Ngôn ngữ trình duyệt";
            this.lciBrowserLanguage.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciBrowserLanguage.TextSize = new System.Drawing.Size(103, 13);
            this.lciBrowserLanguage.TextToControlDistance = 5;
            // 
            // lcgTimeGoogle
            // 
            this.lcgTimeGoogle.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lcgTimeGoogle.AppearanceGroup.Options.UseFont = true;
            this.lcgTimeGoogle.CustomizationFormText = "Thời gian tìm kiếm trên Google";
            this.lcgTimeGoogle.ExpandButtonVisible = true;
            this.lcgTimeGoogle.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciSpeedKeyboard,
            this.lciThoiGianTK,
            this.lciTimeViewSearchTo});
            this.lcgTimeGoogle.Location = new System.Drawing.Point(635, 0);
            this.lcgTimeGoogle.Name = "lcgTimeGoogle";
            this.lcgTimeGoogle.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTimeGoogle.Size = new System.Drawing.Size(635, 68);
            this.lcgTimeGoogle.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTimeGoogle.Text = "Thời gian tìm kiếm từ khóa";
            // 
            // lciSpeedKeyboard
            // 
            this.lciSpeedKeyboard.Control = this.speSpeedKeyboard;
            this.lciSpeedKeyboard.Location = new System.Drawing.Point(0, 0);
            this.lciSpeedKeyboard.Name = "lciSpeedKeyboard";
            this.lciSpeedKeyboard.Size = new System.Drawing.Size(633, 24);
            this.lciSpeedKeyboard.Text = "Độ trễ gõ phím (milisecond)";
            this.lciSpeedKeyboard.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciSpeedKeyboard.TextSize = new System.Drawing.Size(130, 13);
            this.lciSpeedKeyboard.TextToControlDistance = 5;
            // 
            // lciThoiGianTK
            // 
            this.lciThoiGianTK.Control = this.speTimeViewSearch;
            this.lciThoiGianTK.CustomizationFormText = "Thời gian xem trang tìm kiếm (s)";
            this.lciThoiGianTK.Location = new System.Drawing.Point(0, 24);
            this.lciThoiGianTK.Name = "lciThoiGianTK";
            this.lciThoiGianTK.Size = new System.Drawing.Size(316, 24);
            this.lciThoiGianTK.Text = "Random từ";
            this.lciThoiGianTK.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciThoiGianTK.TextSize = new System.Drawing.Size(53, 13);
            this.lciThoiGianTK.TextToControlDistance = 5;
            // 
            // lciTimeViewSearchTo
            // 
            this.lciTimeViewSearchTo.Control = this.speTimeViewSearchTo;
            this.lciTimeViewSearchTo.CustomizationFormText = "Đến";
            this.lciTimeViewSearchTo.Location = new System.Drawing.Point(316, 24);
            this.lciTimeViewSearchTo.Name = "lciTimeViewSearchTo";
            this.lciTimeViewSearchTo.Size = new System.Drawing.Size(317, 24);
            this.lciTimeViewSearchTo.Text = "Đến";
            this.lciTimeViewSearchTo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciTimeViewSearchTo.TextSize = new System.Drawing.Size(20, 13);
            this.lciTimeViewSearchTo.TextToControlDistance = 5;
            // 
            // lcgViewAds
            // 
            this.lcgViewAds.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcgViewAds.AppearanceGroup.Options.UseFont = true;
            this.lcgViewAds.CustomizationFormText = "Thời gian xem quảng cáo trang mục tiêu";
            this.lcgViewAds.ExpandButtonVisible = true;
            this.lcgViewAds.ExpandOnDoubleClick = true;
            this.lcgViewAds.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciTimeViewFrom,
            this.lciTimeViewTo});
            this.lcgViewAds.Location = new System.Drawing.Point(0, 68);
            this.lcgViewAds.Name = "lcgViewAds";
            this.lcgViewAds.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgViewAds.Size = new System.Drawing.Size(508, 44);
            this.lcgViewAds.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgViewAds.Text = "Thời gian duyệt trang mục tiêu/Thời gian xem Video (SEO Youtube)";
            // 
            // lciTimeViewFrom
            // 
            this.lciTimeViewFrom.Control = this.speTimeViewFrom;
            this.lciTimeViewFrom.CustomizationFormText = "Sender/Proxy";
            this.lciTimeViewFrom.Location = new System.Drawing.Point(0, 0);
            this.lciTimeViewFrom.Name = "lciTimeViewFrom";
            this.lciTimeViewFrom.Size = new System.Drawing.Size(253, 24);
            this.lciTimeViewFrom.Text = "Random từ";
            this.lciTimeViewFrom.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTimeViewFrom.TextSize = new System.Drawing.Size(100, 13);
            this.lciTimeViewFrom.TextToControlDistance = 5;
            // 
            // lciTimeViewTo
            // 
            this.lciTimeViewTo.Control = this.speTimeViewTo;
            this.lciTimeViewTo.CustomizationFormText = "Đến";
            this.lciTimeViewTo.Location = new System.Drawing.Point(253, 0);
            this.lciTimeViewTo.Name = "lciTimeViewTo";
            this.lciTimeViewTo.Size = new System.Drawing.Size(253, 24);
            this.lciTimeViewTo.Text = "Đến";
            this.lciTimeViewTo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTimeViewTo.TextSize = new System.Drawing.Size(70, 13);
            this.lciTimeViewTo.TextToControlDistance = 5;
            // 
            // lcgTimeInternalExternal
            // 
            this.lcgTimeInternalExternal.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcgTimeInternalExternal.AppearanceGroup.Options.UseFont = true;
            this.lcgTimeInternalExternal.CustomizationFormText = "Thời gian duyệt Sub Link";
            this.lcgTimeInternalExternal.ExpandButtonVisible = true;
            this.lcgTimeInternalExternal.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem63,
            this.lciSubLinkTime,
            this.lciSubLinkViewTo});
            this.lcgTimeInternalExternal.Location = new System.Drawing.Point(508, 68);
            this.lcgTimeInternalExternal.Name = "lcgTimeInternalExternal";
            this.lcgTimeInternalExternal.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTimeInternalExternal.Size = new System.Drawing.Size(762, 44);
            this.lcgTimeInternalExternal.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTimeInternalExternal.Text = "Thời gian duyệt Internal + External Link";
            // 
            // layoutControlItem63
            // 
            this.layoutControlItem63.Control = this.speInternalCount;
            this.layoutControlItem63.CustomizationFormText = "Số lần click Internal Link";
            this.layoutControlItem63.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem63.Name = "layoutControlItem63";
            this.layoutControlItem63.Size = new System.Drawing.Size(253, 24);
            this.layoutControlItem63.Text = "Số lần click Internal Link";
            this.layoutControlItem63.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem63.TextSize = new System.Drawing.Size(113, 13);
            this.layoutControlItem63.TextToControlDistance = 5;
            // 
            // lciSubLinkTime
            // 
            this.lciSubLinkTime.Control = this.speSubLinkView;
            this.lciSubLinkTime.CustomizationFormText = "Thời gian lướt tại mỗi link";
            this.lciSubLinkTime.Location = new System.Drawing.Point(253, 0);
            this.lciSubLinkTime.Name = "lciSubLinkTime";
            this.lciSubLinkTime.Size = new System.Drawing.Size(254, 24);
            this.lciSubLinkTime.Text = "Random từ";
            this.lciSubLinkTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciSubLinkTime.TextSize = new System.Drawing.Size(53, 13);
            this.lciSubLinkTime.TextToControlDistance = 5;
            // 
            // lciSubLinkViewTo
            // 
            this.lciSubLinkViewTo.Control = this.speSubLinkViewTo;
            this.lciSubLinkViewTo.CustomizationFormText = "Đến";
            this.lciSubLinkViewTo.Location = new System.Drawing.Point(507, 0);
            this.lciSubLinkViewTo.Name = "lciSubLinkViewTo";
            this.lciSubLinkViewTo.Size = new System.Drawing.Size(253, 24);
            this.lciSubLinkViewTo.Text = "Đến";
            this.lciSubLinkViewTo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciSubLinkViewTo.TextSize = new System.Drawing.Size(20, 13);
            this.lciSubLinkViewTo.TextToControlDistance = 5;
            // 
            // layoutControlItem39
            // 
            this.layoutControlItem39.Control = this.ceiViewFilm;
            this.layoutControlItem39.Location = new System.Drawing.Point(318, 184);
            this.layoutControlItem39.Name = "layoutControlItem39";
            this.layoutControlItem39.Size = new System.Drawing.Size(317, 23);
            this.layoutControlItem39.Text = "Click xem phim";
            this.layoutControlItem39.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem39.TextSize = new System.Drawing.Size(69, 13);
            this.layoutControlItem39.TextToControlDistance = 5;
            // 
            // lciCallPhoneZalo
            // 
            this.lciCallPhoneZalo.Control = this.ceiCallPhoneZalo;
            this.lciCallPhoneZalo.Location = new System.Drawing.Point(635, 184);
            this.lciCallPhoneZalo.Name = "lciCallPhoneZalo";
            this.lciCallPhoneZalo.Size = new System.Drawing.Size(237, 23);
            this.lciCallPhoneZalo.Text = "Click cuộc gọi hoặc Zalo";
            this.lciCallPhoneZalo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciCallPhoneZalo.TextSize = new System.Drawing.Size(112, 13);
            this.lciCallPhoneZalo.TextToControlDistance = 5;
            // 
            // lciViewYoutube
            // 
            this.lciViewYoutube.Control = this.ceiViewYoutube;
            this.lciViewYoutube.CustomizationFormText = "Click Xem Youtube";
            this.lciViewYoutube.Location = new System.Drawing.Point(0, 184);
            this.lciViewYoutube.Name = "lciViewYoutube";
            this.lciViewYoutube.Size = new System.Drawing.Size(318, 23);
            this.lciViewYoutube.Text = "Click Xem Youtube";
            this.lciViewYoutube.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciViewYoutube.TextSize = new System.Drawing.Size(87, 13);
            this.lciViewYoutube.TextToControlDistance = 5;
            // 
            // lciNotViewImage
            // 
            this.lciNotViewImage.Control = this.ceiNotViewImage;
            this.lciNotViewImage.CustomizationFormText = "Không hiển thị hình ảnh trang đích";
            this.lciNotViewImage.Location = new System.Drawing.Point(872, 184);
            this.lciNotViewImage.Name = "lciNotViewImage";
            this.lciNotViewImage.Size = new System.Drawing.Size(398, 23);
            this.lciNotViewImage.Text = "Không hiển thị hình ảnh khi duyệt web (tăng tốc độ và đỡ tốn dung lượng 4G)";
            this.lciNotViewImage.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciNotViewImage.TextSize = new System.Drawing.Size(370, 13);
            this.lciNotViewImage.TextToControlDistance = 5;
            // 
            // emptySpaceItem16
            // 
            this.emptySpaceItem16.AllowHotTrack = false;
            this.emptySpaceItem16.Location = new System.Drawing.Point(0, 207);
            this.emptySpaceItem16.Name = "emptySpaceItem16";
            this.emptySpaceItem16.Size = new System.Drawing.Size(1270, 170);
            this.emptySpaceItem16.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcgLoginGmail
            // 
            this.lcgLoginGmail.CustomizationFormText = "Đăng nhập Gmail";
            this.lcgLoginGmail.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciGMail,
            this.tagGMail,
            this.emptySpaceItem11,
            this.layoutControlItem53,
            this.layoutControlItem60});
            this.lcgLoginGmail.Location = new System.Drawing.Point(0, 0);
            this.lcgLoginGmail.Name = "lcgLoginGmail";
            this.lcgLoginGmail.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgLoginGmail.Size = new System.Drawing.Size(1270, 538);
            this.lcgLoginGmail.Text = "Đăng nhập Gmail";
            this.lcgLoginGmail.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciGMail
            // 
            this.lciGMail.Control = this.radGMail;
            this.lciGMail.CustomizationFormText = "Loại";
            this.lciGMail.Location = new System.Drawing.Point(0, 0);
            this.lciGMail.Name = "lciGMail";
            this.lciGMail.Size = new System.Drawing.Size(318, 29);
            this.lciGMail.Text = "Loại";
            this.lciGMail.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciGMail.TextSize = new System.Drawing.Size(0, 0);
            this.lciGMail.TextToControlDistance = 0;
            this.lciGMail.TextVisible = false;
            // 
            // tagGMail
            // 
            this.tagGMail.CustomizationFormText = "Profile";
            this.tagGMail.Location = new System.Drawing.Point(0, 29);
            this.tagGMail.Name = "tagGMail";
            this.tagGMail.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.tagGMail.SelectedTabPage = this.lcgMail;
            this.tagGMail.SelectedTabPageIndex = 0;
            this.tagGMail.Size = new System.Drawing.Size(1270, 509);
            this.tagGMail.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgMail,
            this.lcgProfile});
            this.tagGMail.Text = "Profile";
            // 
            // lcgMail
            // 
            this.lcgMail.CustomizationFormText = "GMail";
            this.lcgMail.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciEmailDelay,
            this.lciListEmail,
            this.emptySpaceItem8});
            this.lcgMail.Location = new System.Drawing.Point(0, 0);
            this.lcgMail.Name = "lcgMail";
            this.lcgMail.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgMail.Size = new System.Drawing.Size(1264, 481);
            this.lcgMail.Text = "GMail";
            // 
            // lciEmailDelay
            // 
            this.lciEmailDelay.Control = this.speEmailDelay;
            this.lciEmailDelay.CustomizationFormText = "Delay";
            this.lciEmailDelay.Location = new System.Drawing.Point(0, 0);
            this.lciEmailDelay.Name = "lciEmailDelay";
            this.lciEmailDelay.Size = new System.Drawing.Size(632, 24);
            this.lciEmailDelay.Text = "Delay";
            this.lciEmailDelay.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciEmailDelay.TextSize = new System.Drawing.Size(27, 13);
            this.lciEmailDelay.TextToControlDistance = 5;
            // 
            // lciListEmail
            // 
            this.lciListEmail.Control = this.memEmail;
            this.lciListEmail.CustomizationFormText = "Danh sách Gmail (email|password|emailkhoiphuc)";
            this.lciListEmail.Location = new System.Drawing.Point(0, 24);
            this.lciListEmail.Name = "lciListEmail";
            this.lciListEmail.Size = new System.Drawing.Size(1264, 457);
            this.lciListEmail.Text = "Gmail (email|password|emailkhoiphuc) 1 dòng 1 tài khoản";
            this.lciListEmail.TextLocation = DevExpress.Utils.Locations.Top;
            this.lciListEmail.TextSize = new System.Drawing.Size(272, 13);
            // 
            // emptySpaceItem8
            // 
            this.emptySpaceItem8.AllowHotTrack = false;
            this.emptySpaceItem8.CustomizationFormText = "emptySpaceItem8";
            this.emptySpaceItem8.Location = new System.Drawing.Point(632, 0);
            this.emptySpaceItem8.Name = "emptySpaceItem8";
            this.emptySpaceItem8.Size = new System.Drawing.Size(632, 24);
            this.emptySpaceItem8.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcgProfile
            // 
            this.lcgProfile.CustomizationFormText = "Profile";
            this.lcgProfile.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciListProfile});
            this.lcgProfile.Location = new System.Drawing.Point(0, 0);
            this.lcgProfile.Name = "lcgProfile";
            this.lcgProfile.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgProfile.Size = new System.Drawing.Size(1264, 481);
            this.lcgProfile.Text = "Profile";
            // 
            // lciListProfile
            // 
            this.lciListProfile.Control = this.memProfile;
            this.lciListProfile.CustomizationFormText = "Mỗi dòng là 1 profile";
            this.lciListProfile.Location = new System.Drawing.Point(0, 0);
            this.lciListProfile.Name = "lciListProfile";
            this.lciListProfile.Size = new System.Drawing.Size(1264, 481);
            this.lciListProfile.Text = "Mỗi dòng là 1 profile";
            this.lciListProfile.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciListProfile.TextLocation = DevExpress.Utils.Locations.Top;
            this.lciListProfile.TextSize = new System.Drawing.Size(96, 13);
            this.lciListProfile.TextToControlDistance = 5;
            // 
            // emptySpaceItem11
            // 
            this.emptySpaceItem11.AllowHotTrack = false;
            this.emptySpaceItem11.CustomizationFormText = "emptySpaceItem11";
            this.emptySpaceItem11.Location = new System.Drawing.Point(953, 0);
            this.emptySpaceItem11.Name = "emptySpaceItem11";
            this.emptySpaceItem11.Size = new System.Drawing.Size(317, 29);
            this.emptySpaceItem11.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem53
            // 
            this.layoutControlItem53.Control = this.btnFirefoxProfile;
            this.layoutControlItem53.CustomizationFormText = "layoutControlItem53";
            this.layoutControlItem53.Location = new System.Drawing.Point(318, 0);
            this.layoutControlItem53.Name = "layoutControlItem53";
            this.layoutControlItem53.Size = new System.Drawing.Size(317, 29);
            this.layoutControlItem53.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem53.TextVisible = false;
            // 
            // layoutControlItem60
            // 
            this.layoutControlItem60.Control = this.btnCreateProfile;
            this.layoutControlItem60.CustomizationFormText = "layoutControlItem60";
            this.layoutControlItem60.Location = new System.Drawing.Point(635, 0);
            this.layoutControlItem60.Name = "layoutControlItem60";
            this.layoutControlItem60.Size = new System.Drawing.Size(318, 29);
            this.layoutControlItem60.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem60.TextVisible = false;
            // 
            // lcgUserAgent
            // 
            this.lcgUserAgent.CustomizationFormText = "layoutControlGroup4";
            this.lcgUserAgent.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem13,
            this.lbeNoticeAgent,
            this.layoutControlItem38,
            this.layoutControlItem62,
            this.lciDeviceType});
            this.lcgUserAgent.Location = new System.Drawing.Point(0, 0);
            this.lcgUserAgent.Name = "lcgUserAgent";
            this.lcgUserAgent.Size = new System.Drawing.Size(1270, 538);
            this.lcgUserAgent.Text = "Thiết lập giả lập thiết bị";
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.txtAgent;
            this.layoutControlItem13.CustomizationFormText = "layoutControlItem13";
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(1270, 512);
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            // 
            // lbeNoticeAgent
            // 
            this.lbeNoticeAgent.Control = this.lbeUserAgentNotice;
            this.lbeNoticeAgent.CustomizationFormText = "lbeNoticeAgent";
            this.lbeNoticeAgent.Location = new System.Drawing.Point(0, 0);
            this.lbeNoticeAgent.Name = "lbeNoticeAgent";
            this.lbeNoticeAgent.Size = new System.Drawing.Size(725, 26);
            this.lbeNoticeAgent.TextSize = new System.Drawing.Size(0, 0);
            this.lbeNoticeAgent.TextVisible = false;
            // 
            // layoutControlItem38
            // 
            this.layoutControlItem38.Control = this.btnGetAgent;
            this.layoutControlItem38.CustomizationFormText = "layoutControlItem38";
            this.layoutControlItem38.Location = new System.Drawing.Point(907, 0);
            this.layoutControlItem38.Name = "layoutControlItem38";
            this.layoutControlItem38.Size = new System.Drawing.Size(182, 26);
            this.layoutControlItem38.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem38.TextVisible = false;
            // 
            // layoutControlItem62
            // 
            this.layoutControlItem62.Control = this.btnSyncUserAgent;
            this.layoutControlItem62.CustomizationFormText = "Lấy User Agent từ NATech";
            this.layoutControlItem62.Location = new System.Drawing.Point(725, 0);
            this.layoutControlItem62.Name = "layoutControlItem62";
            this.layoutControlItem62.Size = new System.Drawing.Size(182, 26);
            this.layoutControlItem62.Text = "Lấy User Agent từ NATech";
            this.layoutControlItem62.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem62.TextVisible = false;
            // 
            // lciDeviceType
            // 
            this.lciDeviceType.Control = this.lueDeviceType;
            this.lciDeviceType.Location = new System.Drawing.Point(1089, 0);
            this.lciDeviceType.Name = "lciDeviceType";
            this.lciDeviceType.Size = new System.Drawing.Size(181, 26);
            this.lciDeviceType.Text = "Loại thiết bị";
            this.lciDeviceType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciDeviceType.TextSize = new System.Drawing.Size(55, 13);
            this.lciDeviceType.TextToControlDistance = 5;
            // 
            // lcgFakeIP
            // 
            this.lcgFakeIP.CustomizationFormText = "Proxy ";
            this.lcgFakeIP.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgDcom,
            this.lcgChangeMac});
            this.lcgFakeIP.Location = new System.Drawing.Point(0, 0);
            this.lcgFakeIP.Name = "lcgFakeIP";
            this.lcgFakeIP.Size = new System.Drawing.Size(1270, 538);
            this.lcgFakeIP.Text = "Thiết lập Fake IP";
            // 
            // lcgDcom
            // 
            this.lcgDcom.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcgDcom.AppearanceGroup.Options.UseFont = true;
            this.lcgDcom.CustomizationFormText = "DCOM";
            this.lcgDcom.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabIP,
            this.lciChangeIp});
            this.lcgDcom.Location = new System.Drawing.Point(0, 46);
            this.lcgDcom.Name = "lcgDcom";
            this.lcgDcom.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgDcom.Size = new System.Drawing.Size(1270, 492);
            this.lcgDcom.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgDcom.Text = "Fake IP";
            // 
            // tabIP
            // 
            this.tabIP.CustomizationFormText = "tabIP";
            this.tabIP.Location = new System.Drawing.Point(0, 94);
            this.tabIP.Name = "tabIP";
            this.tabIP.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.tabIP.SelectedTabPage = this.lcgProxy;
            this.tabIP.SelectedTabPageIndex = 1;
            this.tabIP.Size = new System.Drawing.Size(1268, 378);
            this.tabIP.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgListDcom,
            this.lcgProxy});
            // 
            // lcgProxy
            // 
            this.lcgProxy.CustomizationFormText = "Proxy";
            this.lcgProxy.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabProxyMain,
            this.lciProxySupplier});
            this.lcgProxy.Location = new System.Drawing.Point(0, 0);
            this.lcgProxy.Name = "lcgProxy";
            this.lcgProxy.Size = new System.Drawing.Size(1262, 350);
            this.lcgProxy.Text = "Proxy";
            // 
            // tabProxyMain
            // 
            this.tabProxyMain.CustomizationFormText = "tabbedControlGroup1";
            this.tabProxyMain.Location = new System.Drawing.Point(0, 87);
            this.tabProxyMain.Name = "tabProxyMain";
            this.tabProxyMain.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.tabProxyMain.SelectedTabPage = this.lcgOBCv2Proxy;
            this.tabProxyMain.SelectedTabPageIndex = 5;
            this.tabProxyMain.Size = new System.Drawing.Size(1262, 263);
            this.tabProxyMain.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgProxyFree,
            this.lcgTinSoftProxy,
            this.lcgXProxy,
            this.lcgOBCProxy,
            this.lcgOBCv2Proxy_Old,
            this.lcgOBCv2Proxy,
            this.lcgMultiProxy,
            this.lcgTMProxy});
            // 
            // lcgOBCv2Proxy
            // 
            this.lcgOBCv2Proxy.CustomizationFormText = "tcgOBCProxyV3";
            this.lcgOBCv2Proxy.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem49,
            this.layoutControlGroup9,
            this.layoutControlItem45,
            this.layoutControlItem54,
            this.layoutControlItem65});
            this.lcgOBCv2Proxy.Location = new System.Drawing.Point(0, 0);
            this.lcgOBCv2Proxy.Name = "lcgOBCv2Proxy";
            this.lcgOBCv2Proxy.Size = new System.Drawing.Size(1256, 235);
            this.lcgOBCv2Proxy.Text = "DCOM Proxy";
            // 
            // layoutControlItem49
            // 
            this.layoutControlItem49.Control = this.txtOBCV3Host;
            this.layoutControlItem49.CustomizationFormText = "Service Url";
            this.layoutControlItem49.Location = new System.Drawing.Point(314, 0);
            this.layoutControlItem49.Name = "layoutControlItem49";
            this.layoutControlItem49.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem49.Text = "Service Url";
            this.layoutControlItem49.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem49.TextSize = new System.Drawing.Size(51, 13);
            this.layoutControlItem49.TextToControlDistance = 5;
            // 
            // layoutControlGroup9
            // 
            this.layoutControlGroup9.CustomizationFormText = "Danh sách Proxy. Cấu trúc IP:PORT  (Ví dụ: 192.168.1.100:4001). Mỗi dòng 1 Proxy";
            this.layoutControlGroup9.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem50});
            this.layoutControlGroup9.Location = new System.Drawing.Point(0, 26);
            this.layoutControlGroup9.Name = "layoutControlGroup9";
            this.layoutControlGroup9.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup9.Size = new System.Drawing.Size(1256, 209);
            this.layoutControlGroup9.Text = "Danh sách Proxy. Cấu trúc IP:PORT or ID:PORT:USER:PASS  (Ví dụ: 192.168.1.100:400" +
    "1). Mỗi dòng 1 Proxy, mỗi proxy chạy 1 luồng";
            // 
            // layoutControlItem50
            // 
            this.layoutControlItem50.Control = this.memOBCV3Proxy;
            this.layoutControlItem50.CustomizationFormText = "layoutControlItem50";
            this.layoutControlItem50.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem50.Name = "layoutControlItem50";
            this.layoutControlItem50.Size = new System.Drawing.Size(1250, 185);
            this.layoutControlItem50.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem50.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem50.TextVisible = false;
            // 
            // layoutControlItem45
            // 
            this.layoutControlItem45.Control = this.btnOBCV2HomePage;
            this.layoutControlItem45.CustomizationFormText = "layoutControlItem45";
            this.layoutControlItem45.Location = new System.Drawing.Point(942, 0);
            this.layoutControlItem45.Name = "layoutControlItem45";
            this.layoutControlItem45.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem45.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem45.TextVisible = false;
            // 
            // layoutControlItem54
            // 
            this.layoutControlItem54.Control = this.cbeProxySupplier;
            this.layoutControlItem54.CustomizationFormText = "Nhà cung cấp mạch";
            this.layoutControlItem54.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem54.Name = "layoutControlItem54";
            this.layoutControlItem54.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem54.Text = "Nhà cung cấp mạch";
            this.layoutControlItem54.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem54.TextSize = new System.Drawing.Size(93, 13);
            this.layoutControlItem54.TextToControlDistance = 5;
            // 
            // layoutControlItem65
            // 
            this.layoutControlItem65.Control = this.speDCOMProxyDelay;
            this.layoutControlItem65.Location = new System.Drawing.Point(628, 0);
            this.layoutControlItem65.Name = "layoutControlItem65";
            this.layoutControlItem65.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem65.Text = "Thời gian trễ sau khi reset (giây)";
            this.layoutControlItem65.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem65.TextSize = new System.Drawing.Size(155, 13);
            this.layoutControlItem65.TextToControlDistance = 5;
            // 
            // lcgProxyFree
            // 
            this.lcgProxyFree.CustomizationFormText = "Danh sách proxy";
            this.lcgProxyFree.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgFreeProxySub});
            this.lcgProxyFree.Location = new System.Drawing.Point(0, 0);
            this.lcgProxyFree.Name = "lcgProxyFree";
            this.lcgProxyFree.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgProxyFree.Size = new System.Drawing.Size(1256, 235);
            this.lcgProxyFree.Text = "Proxy tỉnh";
            // 
            // lcgFreeProxySub
            // 
            this.lcgFreeProxySub.CustomizationFormText = "lcgFreeProxySub";
            this.lcgFreeProxySub.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgAddonGetProxy,
            this.splitterItem4,
            this.layoutControlItem25,
            this.lcgFreeProxyConfig});
            this.lcgFreeProxySub.Location = new System.Drawing.Point(0, 0);
            this.lcgFreeProxySub.Name = "lcgFreeProxySub";
            this.lcgFreeProxySub.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgFreeProxySub.Size = new System.Drawing.Size(1256, 235);
            this.lcgFreeProxySub.TextVisible = false;
            // 
            // lcgAddonGetProxy
            // 
            this.lcgAddonGetProxy.Control = this.memProxyNote;
            this.lcgAddonGetProxy.CustomizationFormText = "Danh sách các website và addon lấy proxy";
            this.lcgAddonGetProxy.Location = new System.Drawing.Point(627, 17);
            this.lcgAddonGetProxy.Name = "lcgAddonGetProxy";
            this.lcgAddonGetProxy.Size = new System.Drawing.Size(623, 212);
            this.lcgAddonGetProxy.Text = "Danh sách các website và addon lấy proxy";
            this.lcgAddonGetProxy.TextLocation = DevExpress.Utils.Locations.Top;
            this.lcgAddonGetProxy.TextSize = new System.Drawing.Size(272, 13);
            // 
            // splitterItem4
            // 
            this.splitterItem4.AllowHotTrack = true;
            this.splitterItem4.CustomizationFormText = "splitterItem4";
            this.splitterItem4.Location = new System.Drawing.Point(622, 17);
            this.splitterItem4.Name = "splitterItem4";
            this.splitterItem4.Size = new System.Drawing.Size(5, 212);
            // 
            // layoutControlItem25
            // 
            this.layoutControlItem25.Control = this.lbeNoticeFreeProxy;
            this.layoutControlItem25.CustomizationFormText = "layoutControlItem25";
            this.layoutControlItem25.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem25.Name = "layoutControlItem25";
            this.layoutControlItem25.Size = new System.Drawing.Size(1250, 17);
            this.layoutControlItem25.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem25.TextVisible = false;
            // 
            // lcgFreeProxyConfig
            // 
            this.lcgFreeProxyConfig.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.lcgFreeProxyConfig.Location = new System.Drawing.Point(0, 17);
            this.lcgFreeProxyConfig.Name = "lcgFreeProxyConfig";
            this.lcgFreeProxyConfig.OptionsItemText.TextToControlDistance = 5;
            this.lcgFreeProxyConfig.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgFreeProxyConfig.Size = new System.Drawing.Size(622, 212);
            this.lcgFreeProxyConfig.Text = "Cú pháp IP:Port hoặc IP:Port:User:Pass ";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.memProxy;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(616, 188);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // lcgTinSoftProxy
            // 
            this.lcgTinSoftProxy.CustomizationFormText = "TinSoft Proxy (tinsoftproxy.com)";
            this.lcgTinSoftProxy.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem3,
            this.layoutControlItem11,
            this.layoutControlItem37,
            this.layoutControlItem52,
            this.layoutControlItem47,
            this.layoutControlItem55,
            this.layoutControlItem3,
            this.layoutControlItem7});
            this.lcgTinSoftProxy.Location = new System.Drawing.Point(0, 0);
            this.lcgTinSoftProxy.Name = "lcgTinSoftProxy";
            this.lcgTinSoftProxy.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTinSoftProxy.Size = new System.Drawing.Size(1256, 235);
            this.lcgTinSoftProxy.Text = "TinSoft Proxy";
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.CustomizationFormText = "emptySpaceItem3";
            this.emptySpaceItem3.Location = new System.Drawing.Point(1049, 16);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(207, 26);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.lbeNoticeTinsoft;
            this.layoutControlItem11.CustomizationFormText = "layoutControlItem11";
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(1256, 16);
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // layoutControlItem37
            // 
            this.layoutControlItem37.Control = this.btnTinSoftHomepage;
            this.layoutControlItem37.CustomizationFormText = "layoutControlItem37";
            this.layoutControlItem37.Location = new System.Drawing.Point(0, 16);
            this.layoutControlItem37.Name = "layoutControlItem37";
            this.layoutControlItem37.Size = new System.Drawing.Size(209, 26);
            this.layoutControlItem37.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem37.TextVisible = false;
            // 
            // layoutControlItem52
            // 
            this.layoutControlItem52.Control = this.grdTinsoft;
            this.layoutControlItem52.CustomizationFormText = "layoutControlItem52";
            this.layoutControlItem52.Location = new System.Drawing.Point(0, 42);
            this.layoutControlItem52.Name = "layoutControlItem52";
            this.layoutControlItem52.Size = new System.Drawing.Size(1256, 193);
            this.layoutControlItem52.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem52.TextVisible = false;
            // 
            // layoutControlItem47
            // 
            this.layoutControlItem47.Control = this.btnDisableIPv6;
            this.layoutControlItem47.CustomizationFormText = "layoutControlItem47";
            this.layoutControlItem47.Location = new System.Drawing.Point(209, 16);
            this.layoutControlItem47.Name = "layoutControlItem47";
            this.layoutControlItem47.Size = new System.Drawing.Size(216, 26);
            this.layoutControlItem47.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem47.TextVisible = false;
            // 
            // layoutControlItem55
            // 
            this.layoutControlItem55.Control = this.btnRegisterTinsoft;
            this.layoutControlItem55.CustomizationFormText = "layoutControlItem55";
            this.layoutControlItem55.Location = new System.Drawing.Point(425, 16);
            this.layoutControlItem55.Name = "layoutControlItem55";
            this.layoutControlItem55.Size = new System.Drawing.Size(208, 26);
            this.layoutControlItem55.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem55.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnTinsoftExportExcel;
            this.layoutControlItem3.Location = new System.Drawing.Point(633, 16);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(208, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnTinsoftImportExcel;
            this.layoutControlItem7.Location = new System.Drawing.Point(841, 16);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(208, 26);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // lcgXProxy
            // 
            this.lcgXProxy.CustomizationFormText = "xProxy";
            this.lcgXProxy.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciHostXProxy,
            this.layoutControlItem26,
            this.emptySpaceItem7,
            this.layoutControlItem27,
            this.layoutControlItem36});
            this.lcgXProxy.Location = new System.Drawing.Point(0, 0);
            this.lcgXProxy.Name = "lcgXProxy";
            this.lcgXProxy.Size = new System.Drawing.Size(1256, 235);
            this.lcgXProxy.Text = "xProxy";
            this.lcgXProxy.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInCustomization;
            // 
            // lciHostXProxy
            // 
            this.lciHostXProxy.Control = this.txtXProxyHost;
            this.lciHostXProxy.CustomizationFormText = "Host";
            this.lciHostXProxy.Location = new System.Drawing.Point(0, 0);
            this.lciHostXProxy.Name = "lciHostXProxy";
            this.lciHostXProxy.Size = new System.Drawing.Size(314, 26);
            this.lciHostXProxy.Text = "Service Url";
            this.lciHostXProxy.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciHostXProxy.TextSize = new System.Drawing.Size(51, 13);
            this.lciHostXProxy.TextToControlDistance = 5;
            // 
            // layoutControlItem26
            // 
            this.layoutControlItem26.Control = this.btnConnectxProxy;
            this.layoutControlItem26.CustomizationFormText = "layoutControlItem26";
            this.layoutControlItem26.Location = new System.Drawing.Point(314, 0);
            this.layoutControlItem26.Name = "layoutControlItem26";
            this.layoutControlItem26.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem26.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem26.TextVisible = false;
            // 
            // emptySpaceItem7
            // 
            this.emptySpaceItem7.AllowHotTrack = false;
            this.emptySpaceItem7.CustomizationFormText = "emptySpaceItem7";
            this.emptySpaceItem7.Location = new System.Drawing.Point(942, 0);
            this.emptySpaceItem7.Name = "emptySpaceItem7";
            this.emptySpaceItem7.Size = new System.Drawing.Size(314, 26);
            this.emptySpaceItem7.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem27
            // 
            this.layoutControlItem27.Control = this.grdXProxyList;
            this.layoutControlItem27.CustomizationFormText = "layoutControlItem27";
            this.layoutControlItem27.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem27.Name = "layoutControlItem27";
            this.layoutControlItem27.Size = new System.Drawing.Size(1256, 209);
            this.layoutControlItem27.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem27.TextVisible = false;
            // 
            // layoutControlItem36
            // 
            this.layoutControlItem36.Control = this.btnXProxyHomepage;
            this.layoutControlItem36.CustomizationFormText = "layoutControlItem36";
            this.layoutControlItem36.Location = new System.Drawing.Point(628, 0);
            this.layoutControlItem36.Name = "layoutControlItem36";
            this.layoutControlItem36.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem36.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem36.TextVisible = false;
            // 
            // lcgOBCProxy
            // 
            this.lcgOBCProxy.CustomizationFormText = "OBC Proxy";
            this.lcgOBCProxy.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem18,
            this.layoutControlItem21,
            this.emptySpaceItem10,
            this.layoutControlItem22,
            this.layoutControlItem35});
            this.lcgOBCProxy.Location = new System.Drawing.Point(0, 0);
            this.lcgOBCProxy.Name = "lcgOBCProxy";
            this.lcgOBCProxy.Size = new System.Drawing.Size(1256, 235);
            this.lcgOBCProxy.Text = "OBC Proxy";
            // 
            // layoutControlItem18
            // 
            this.layoutControlItem18.Control = this.txtOBCHost;
            this.layoutControlItem18.CustomizationFormText = "Host OBC";
            this.layoutControlItem18.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem18.Name = "layoutControlItem18";
            this.layoutControlItem18.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem18.Text = "Service Url";
            this.layoutControlItem18.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem18.TextSize = new System.Drawing.Size(51, 13);
            this.layoutControlItem18.TextToControlDistance = 5;
            // 
            // layoutControlItem21
            // 
            this.layoutControlItem21.Control = this.btnConnectOBC;
            this.layoutControlItem21.CustomizationFormText = "layoutControlItem21";
            this.layoutControlItem21.Location = new System.Drawing.Point(314, 0);
            this.layoutControlItem21.Name = "layoutControlItem21";
            this.layoutControlItem21.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem21.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem21.TextVisible = false;
            // 
            // emptySpaceItem10
            // 
            this.emptySpaceItem10.AllowHotTrack = false;
            this.emptySpaceItem10.CustomizationFormText = "emptySpaceItem10";
            this.emptySpaceItem10.Location = new System.Drawing.Point(942, 0);
            this.emptySpaceItem10.Name = "emptySpaceItem10";
            this.emptySpaceItem10.Size = new System.Drawing.Size(314, 26);
            this.emptySpaceItem10.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem22
            // 
            this.layoutControlItem22.Control = this.grdOBC;
            this.layoutControlItem22.CustomizationFormText = "layoutControlItem22";
            this.layoutControlItem22.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem22.Name = "layoutControlItem22";
            this.layoutControlItem22.Size = new System.Drawing.Size(1256, 209);
            this.layoutControlItem22.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem22.TextVisible = false;
            // 
            // layoutControlItem35
            // 
            this.layoutControlItem35.Control = this.btnOBCHomePage;
            this.layoutControlItem35.CustomizationFormText = "layoutControlItem35";
            this.layoutControlItem35.Location = new System.Drawing.Point(628, 0);
            this.layoutControlItem35.Name = "layoutControlItem35";
            this.layoutControlItem35.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem35.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem35.TextVisible = false;
            // 
            // lcgOBCv2Proxy_Old
            // 
            this.lcgOBCv2Proxy_Old.CustomizationFormText = "OBC Proxy V2";
            this.lcgOBCv2Proxy_Old.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem43,
            this.layoutControlItem44,
            this.emptySpaceItem13,
            this.layoutControlItem46});
            this.lcgOBCv2Proxy_Old.Location = new System.Drawing.Point(0, 0);
            this.lcgOBCv2Proxy_Old.Name = "lcgOBCv2Proxy_Old";
            this.lcgOBCv2Proxy_Old.Size = new System.Drawing.Size(1256, 235);
            this.lcgOBCv2Proxy_Old.Text = "OBC Proxy V2";
            this.lcgOBCv2Proxy_Old.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInCustomization;
            // 
            // layoutControlItem43
            // 
            this.layoutControlItem43.Control = this.txtOBCV2Host;
            this.layoutControlItem43.CustomizationFormText = "Service Url";
            this.layoutControlItem43.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem43.Name = "layoutControlItem43";
            this.layoutControlItem43.Size = new System.Drawing.Size(419, 26);
            this.layoutControlItem43.Text = "Service Url";
            this.layoutControlItem43.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem43.TextSize = new System.Drawing.Size(51, 13);
            this.layoutControlItem43.TextToControlDistance = 5;
            // 
            // layoutControlItem44
            // 
            this.layoutControlItem44.Control = this.btnConnectOBCV2;
            this.layoutControlItem44.CustomizationFormText = "layoutControlItem44";
            this.layoutControlItem44.Location = new System.Drawing.Point(419, 0);
            this.layoutControlItem44.Name = "layoutControlItem44";
            this.layoutControlItem44.Size = new System.Drawing.Size(418, 26);
            this.layoutControlItem44.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem44.TextVisible = false;
            // 
            // emptySpaceItem13
            // 
            this.emptySpaceItem13.AllowHotTrack = false;
            this.emptySpaceItem13.CustomizationFormText = "emptySpaceItem13";
            this.emptySpaceItem13.Location = new System.Drawing.Point(837, 0);
            this.emptySpaceItem13.Name = "emptySpaceItem13";
            this.emptySpaceItem13.Size = new System.Drawing.Size(419, 26);
            this.emptySpaceItem13.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem46
            // 
            this.layoutControlItem46.Control = this.grdOBCV2;
            this.layoutControlItem46.CustomizationFormText = "layoutControlItem46";
            this.layoutControlItem46.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem46.Name = "layoutControlItem46";
            this.layoutControlItem46.Size = new System.Drawing.Size(1256, 209);
            this.layoutControlItem46.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem46.TextVisible = false;
            // 
            // lcgMultiProxy
            // 
            this.lcgMultiProxy.CustomizationFormText = "Multi Proxy";
            this.lcgMultiProxy.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem29,
            this.layoutControlItem31,
            this.layoutControlItem32,
            this.layoutControlItem33,
            this.layoutControlItem34,
            this.emptySpaceItem9,
            this.splitterItem1,
            this.splitterItem2});
            this.lcgMultiProxy.Location = new System.Drawing.Point(0, 0);
            this.lcgMultiProxy.Name = "lcgMultiProxy";
            this.lcgMultiProxy.Size = new System.Drawing.Size(1256, 235);
            this.lcgMultiProxy.Text = "Multi Proxy";
            this.lcgMultiProxy.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInCustomization;
            // 
            // layoutControlItem29
            // 
            this.layoutControlItem29.Control = this.grdMultiProxy;
            this.layoutControlItem29.CustomizationFormText = "layoutControlItem29";
            this.layoutControlItem29.Location = new System.Drawing.Point(0, 78);
            this.layoutControlItem29.Name = "layoutControlItem29";
            this.layoutControlItem29.Size = new System.Drawing.Size(625, 157);
            this.layoutControlItem29.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem29.TextVisible = false;
            // 
            // layoutControlItem31
            // 
            this.layoutControlItem31.Control = this.radMultiProxyType;
            this.layoutControlItem31.CustomizationFormText = "Cơ chế";
            this.layoutControlItem31.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem31.Name = "layoutControlItem31";
            this.layoutControlItem31.Size = new System.Drawing.Size(419, 78);
            this.layoutControlItem31.Text = "Cơ chế";
            this.layoutControlItem31.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem31.TextSize = new System.Drawing.Size(33, 13);
            this.layoutControlItem31.TextToControlDistance = 5;
            // 
            // layoutControlItem32
            // 
            this.layoutControlItem32.Control = this.grdMultiXProxy;
            this.layoutControlItem32.CustomizationFormText = "layoutControlItem32";
            this.layoutControlItem32.Location = new System.Drawing.Point(630, 78);
            this.layoutControlItem32.Name = "layoutControlItem32";
            this.layoutControlItem32.Size = new System.Drawing.Size(626, 78);
            this.layoutControlItem32.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem32.TextVisible = false;
            // 
            // layoutControlItem33
            // 
            this.layoutControlItem33.Control = this.grdMultiOBC;
            this.layoutControlItem33.CustomizationFormText = "layoutControlItem33";
            this.layoutControlItem33.Location = new System.Drawing.Point(630, 161);
            this.layoutControlItem33.Name = "layoutControlItem33";
            this.layoutControlItem33.Size = new System.Drawing.Size(626, 74);
            this.layoutControlItem33.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem33.TextVisible = false;
            // 
            // layoutControlItem34
            // 
            this.layoutControlItem34.Control = this.btnMultiProxyConnect;
            this.layoutControlItem34.CustomizationFormText = "layoutControlItem34";
            this.layoutControlItem34.Location = new System.Drawing.Point(419, 0);
            this.layoutControlItem34.MinSize = new System.Drawing.Size(82, 26);
            this.layoutControlItem34.Name = "layoutControlItem34";
            this.layoutControlItem34.Size = new System.Drawing.Size(418, 78);
            this.layoutControlItem34.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem34.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem34.TextVisible = false;
            // 
            // emptySpaceItem9
            // 
            this.emptySpaceItem9.AllowHotTrack = false;
            this.emptySpaceItem9.CustomizationFormText = "emptySpaceItem9";
            this.emptySpaceItem9.Location = new System.Drawing.Point(837, 0);
            this.emptySpaceItem9.Name = "emptySpaceItem9";
            this.emptySpaceItem9.Size = new System.Drawing.Size(419, 78);
            this.emptySpaceItem9.TextSize = new System.Drawing.Size(0, 0);
            // 
            // splitterItem1
            // 
            this.splitterItem1.AllowHotTrack = true;
            this.splitterItem1.CustomizationFormText = "splitterItem1";
            this.splitterItem1.Location = new System.Drawing.Point(630, 156);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(626, 5);
            // 
            // splitterItem2
            // 
            this.splitterItem2.AllowHotTrack = true;
            this.splitterItem2.CustomizationFormText = "splitterItem2";
            this.splitterItem2.Location = new System.Drawing.Point(625, 78);
            this.splitterItem2.Name = "splitterItem2";
            this.splitterItem2.Size = new System.Drawing.Size(5, 157);
            // 
            // lcgTMProxy
            // 
            this.lcgTMProxy.CustomizationFormText = "TMProxy";
            this.lcgTMProxy.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem57,
            this.layoutControlItem58,
            this.emptySpaceItem15});
            this.lcgTMProxy.Location = new System.Drawing.Point(0, 0);
            this.lcgTMProxy.Name = "lcgTMProxy";
            this.lcgTMProxy.Size = new System.Drawing.Size(1256, 235);
            this.lcgTMProxy.Text = "TMProxy";
            // 
            // layoutControlItem57
            // 
            this.layoutControlItem57.Control = this.btnTMProxyHome;
            this.layoutControlItem57.CustomizationFormText = "layoutControlItem57";
            this.layoutControlItem57.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem57.Name = "layoutControlItem57";
            this.layoutControlItem57.Size = new System.Drawing.Size(628, 26);
            this.layoutControlItem57.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem57.TextVisible = false;
            // 
            // layoutControlItem58
            // 
            this.layoutControlItem58.Control = this.grdTMProxy;
            this.layoutControlItem58.CustomizationFormText = "layoutControlItem58";
            this.layoutControlItem58.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem58.Name = "layoutControlItem58";
            this.layoutControlItem58.Size = new System.Drawing.Size(1256, 209);
            this.layoutControlItem58.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem58.TextVisible = false;
            // 
            // emptySpaceItem15
            // 
            this.emptySpaceItem15.AllowHotTrack = false;
            this.emptySpaceItem15.CustomizationFormText = "emptySpaceItem15";
            this.emptySpaceItem15.Location = new System.Drawing.Point(628, 0);
            this.emptySpaceItem15.Name = "emptySpaceItem15";
            this.emptySpaceItem15.Size = new System.Drawing.Size(628, 26);
            this.emptySpaceItem15.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciProxySupplier
            // 
            this.lciProxySupplier.Control = this.raiTypeProxy;
            this.lciProxySupplier.CustomizationFormText = "Sử dụng proxy của";
            this.lciProxySupplier.Location = new System.Drawing.Point(0, 0);
            this.lciProxySupplier.Name = "lciProxySupplier";
            this.lciProxySupplier.Size = new System.Drawing.Size(1262, 87);
            this.lciProxySupplier.Text = "Sử dụng proxy của";
            this.lciProxySupplier.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciProxySupplier.TextSize = new System.Drawing.Size(91, 13);
            this.lciProxySupplier.TextToControlDistance = 5;
            // 
            // lcgListDcom
            // 
            this.lcgListDcom.CustomizationFormText = "Dial-Up/Tên profile (Mỗi dcom là 1 dòng)";
            this.lcgListDcom.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup4,
            this.emptySpaceItem6,
            this.layoutControlItem56,
            this.lciResetDcomInterval,
            this.lciDcomDelay,
            this.layoutControlItem48});
            this.lcgListDcom.Location = new System.Drawing.Point(0, 0);
            this.lcgListDcom.Name = "lcgListDcom";
            this.lcgListDcom.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgListDcom.Size = new System.Drawing.Size(1262, 350);
            this.lcgListDcom.Text = "DCOM 3G/4G";
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroup4.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroup4.CustomizationFormText = "Dial-Up/Tên dcom (Mỗi dcom là 1 dòng)";
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciDialUp});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 29);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup4.Size = new System.Drawing.Size(1262, 321);
            this.layoutControlGroup4.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup4.Text = "Dial-Up/Tên dcom (Mỗi dcom là 1 dòng)";
            // 
            // lciDialUp
            // 
            this.lciDialUp.Control = this.txtDialUp;
            this.lciDialUp.CustomizationFormText = "lciDialUp";
            this.lciDialUp.Location = new System.Drawing.Point(0, 0);
            this.lciDialUp.Name = "lciDialUp";
            this.lciDialUp.Size = new System.Drawing.Size(1260, 301);
            this.lciDialUp.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciDialUp.TextLocation = DevExpress.Utils.Locations.Top;
            this.lciDialUp.TextSize = new System.Drawing.Size(0, 0);
            this.lciDialUp.TextToControlDistance = 0;
            this.lciDialUp.TextVisible = false;
            // 
            // emptySpaceItem6
            // 
            this.emptySpaceItem6.AllowHotTrack = false;
            this.emptySpaceItem6.CustomizationFormText = "emptySpaceItem6";
            this.emptySpaceItem6.Location = new System.Drawing.Point(1009, 0);
            this.emptySpaceItem6.Name = "emptySpaceItem6";
            this.emptySpaceItem6.Size = new System.Drawing.Size(253, 29);
            this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem56
            // 
            this.layoutControlItem56.Control = this.radDcomTypeReset;
            this.layoutControlItem56.CustomizationFormText = "Hình thức reset";
            this.layoutControlItem56.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem56.Name = "layoutControlItem56";
            this.layoutControlItem56.Size = new System.Drawing.Size(252, 29);
            this.layoutControlItem56.Text = "Hình thức reset";
            this.layoutControlItem56.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem56.TextSize = new System.Drawing.Size(74, 13);
            this.layoutControlItem56.TextToControlDistance = 5;
            // 
            // lciResetDcomInterval
            // 
            this.lciResetDcomInterval.Control = this.speResetDcomInterval;
            this.lciResetDcomInterval.CustomizationFormText = "Chu kỳ (phút)";
            this.lciResetDcomInterval.Location = new System.Drawing.Point(252, 0);
            this.lciResetDcomInterval.Name = "lciResetDcomInterval";
            this.lciResetDcomInterval.Size = new System.Drawing.Size(252, 29);
            this.lciResetDcomInterval.Text = "Chu kỳ (phút)";
            this.lciResetDcomInterval.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciResetDcomInterval.TextSize = new System.Drawing.Size(66, 13);
            this.lciResetDcomInterval.TextToControlDistance = 5;
            // 
            // lciDcomDelay
            // 
            this.lciDcomDelay.Control = this.speDcomDelay;
            this.lciDcomDelay.CustomizationFormText = "Thời gian chờ bật DCOM (giây)";
            this.lciDcomDelay.Location = new System.Drawing.Point(504, 0);
            this.lciDcomDelay.Name = "lciDcomDelay";
            this.lciDcomDelay.Size = new System.Drawing.Size(253, 29);
            this.lciDcomDelay.Text = "Thời gian chờ bật DCOM (giây)";
            this.lciDcomDelay.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciDcomDelay.TextSize = new System.Drawing.Size(146, 13);
            this.lciDcomDelay.TextToControlDistance = 5;
            // 
            // layoutControlItem48
            // 
            this.layoutControlItem48.Control = this.btnSetupDCOM;
            this.layoutControlItem48.CustomizationFormText = "layoutControlItem48";
            this.layoutControlItem48.Location = new System.Drawing.Point(757, 0);
            this.layoutControlItem48.Name = "layoutControlItem48";
            this.layoutControlItem48.Size = new System.Drawing.Size(252, 29);
            this.layoutControlItem48.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem48.TextVisible = false;
            // 
            // lciChangeIp
            // 
            this.lciChangeIp.Control = this.radTypeIp;
            this.lciChangeIp.CustomizationFormText = "Đổi IP";
            this.lciChangeIp.Location = new System.Drawing.Point(0, 0);
            this.lciChangeIp.Name = "lciChangeIp";
            this.lciChangeIp.Size = new System.Drawing.Size(1268, 94);
            this.lciChangeIp.Text = "Đổi IP";
            this.lciChangeIp.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciChangeIp.TextSize = new System.Drawing.Size(0, 0);
            this.lciChangeIp.TextToControlDistance = 0;
            this.lciChangeIp.TextVisible = false;
            // 
            // lcgChangeMac
            // 
            this.lcgChangeMac.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcgChangeMac.AppearanceGroup.Options.UseFont = true;
            this.lcgChangeMac.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciChangeMac,
            this.lciChangeMacTime,
            this.layoutControlItem23,
            this.emptySpaceItem12,
            this.layoutControlItem51});
            this.lcgChangeMac.Location = new System.Drawing.Point(0, 0);
            this.lcgChangeMac.Name = "lcgChangeMac";
            this.lcgChangeMac.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgChangeMac.Size = new System.Drawing.Size(1270, 46);
            this.lcgChangeMac.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgChangeMac.Text = "Đổi MAC Address";
            // 
            // lciChangeMac
            // 
            this.lciChangeMac.Control = this.ceiChangeMACAddress;
            this.lciChangeMac.CustomizationFormText = "Đổi MAC Address";
            this.lciChangeMac.Location = new System.Drawing.Point(0, 0);
            this.lciChangeMac.Name = "lciChangeMac";
            this.lciChangeMac.Size = new System.Drawing.Size(254, 26);
            this.lciChangeMac.Text = "Sử dụng";
            this.lciChangeMac.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciChangeMac.TextSize = new System.Drawing.Size(40, 13);
            this.lciChangeMac.TextToControlDistance = 5;
            // 
            // lciChangeMacTime
            // 
            this.lciChangeMacTime.Control = this.speMACAddressInterval;
            this.lciChangeMacTime.CustomizationFormText = "Thời gian đổi MAC Adress (Phút)";
            this.lciChangeMacTime.Location = new System.Drawing.Point(254, 0);
            this.lciChangeMacTime.Name = "lciChangeMacTime";
            this.lciChangeMacTime.Size = new System.Drawing.Size(254, 26);
            this.lciChangeMacTime.Text = "Thời gian (Phút)";
            this.lciChangeMacTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciChangeMacTime.TextSize = new System.Drawing.Size(76, 13);
            this.lciChangeMacTime.TextToControlDistance = 5;
            // 
            // layoutControlItem23
            // 
            this.layoutControlItem23.Control = this.btnChangeMAC;
            this.layoutControlItem23.CustomizationFormText = "layoutControlItem23";
            this.layoutControlItem23.Location = new System.Drawing.Point(508, 0);
            this.layoutControlItem23.Name = "layoutControlItem23";
            this.layoutControlItem23.Size = new System.Drawing.Size(253, 26);
            this.layoutControlItem23.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem23.TextVisible = false;
            // 
            // emptySpaceItem12
            // 
            this.emptySpaceItem12.AllowHotTrack = false;
            this.emptySpaceItem12.CustomizationFormText = "emptySpaceItem12";
            this.emptySpaceItem12.Location = new System.Drawing.Point(1197, 0);
            this.emptySpaceItem12.Name = "emptySpaceItem12";
            this.emptySpaceItem12.Size = new System.Drawing.Size(71, 26);
            this.emptySpaceItem12.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem51
            // 
            this.layoutControlItem51.Control = this.lbeFakeIpNoticMacAddress;
            this.layoutControlItem51.CustomizationFormText = "layoutControlItem51";
            this.layoutControlItem51.Location = new System.Drawing.Point(761, 0);
            this.layoutControlItem51.Name = "layoutControlItem51";
            this.layoutControlItem51.Size = new System.Drawing.Size(436, 26);
            this.layoutControlItem51.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem51.TextVisible = false;
            // 
            // lcgReport
            // 
            this.lcgReport.CustomizationFormText = "Báo cáo";
            this.lcgReport.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem8,
            this.emptySpaceItem2,
            this.layoutControlItem10,
            this.lciReportIp,
            this.lcgReportDomain,
            this.splitterItem7,
            this.lciSaveReport});
            this.lcgReport.Location = new System.Drawing.Point(0, 0);
            this.lcgReport.Name = "lcgReport";
            this.lcgReport.Size = new System.Drawing.Size(1270, 538);
            this.lcgReport.Text = "Báo cáo";
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnLoadHistory;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem8";
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(318, 26);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(953, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(317, 26);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btnDeleteHistoryXml;
            this.layoutControlItem10.CustomizationFormText = "layoutControlItem10";
            this.layoutControlItem10.Location = new System.Drawing.Point(318, 0);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(317, 26);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // lciReportIp
            // 
            this.lciReportIp.CustomizationFormText = "Báo cáo số lượt click theo IP";
            this.lciReportIp.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem17,
            this.layoutControlItem15,
            this.emptySpaceItem5});
            this.lciReportIp.Location = new System.Drawing.Point(638, 26);
            this.lciReportIp.Name = "lciReportIp";
            this.lciReportIp.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciReportIp.Size = new System.Drawing.Size(632, 512);
            this.lciReportIp.Text = "Báo cáo số lượt click theo IP";
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Control = this.grdIp;
            this.layoutControlItem17.CustomizationFormText = "layoutControlItem17";
            this.layoutControlItem17.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Size = new System.Drawing.Size(626, 462);
            this.layoutControlItem17.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem17.TextVisible = false;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.btnExcelIp;
            this.layoutControlItem15.CustomizationFormText = "layoutControlItem15";
            this.layoutControlItem15.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(313, 26);
            this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem15.TextVisible = false;
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.CustomizationFormText = "emptySpaceItem5";
            this.emptySpaceItem5.Location = new System.Drawing.Point(313, 0);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(313, 26);
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcgReportDomain
            // 
            this.lcgReportDomain.CustomizationFormText = "Báo cáo số lượt click theo tên miền";
            this.lcgReportDomain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem12,
            this.emptySpaceItem1});
            this.lcgReportDomain.Location = new System.Drawing.Point(0, 26);
            this.lcgReportDomain.Name = "lcgReportDomain";
            this.lcgReportDomain.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgReportDomain.Size = new System.Drawing.Size(633, 512);
            this.lcgReportDomain.Text = "Báo cáo số lượt click theo tên miền";
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.grdHistory;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(627, 462);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.btnExcelDomain;
            this.layoutControlItem12.CustomizationFormText = "layoutControlItem12";
            this.layoutControlItem12.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(314, 26);
            this.layoutControlItem12.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem12.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(314, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(313, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // splitterItem7
            // 
            this.splitterItem7.AllowHotTrack = true;
            this.splitterItem7.CustomizationFormText = "splitterItem7";
            this.splitterItem7.Location = new System.Drawing.Point(633, 26);
            this.splitterItem7.Name = "splitterItem7";
            this.splitterItem7.Size = new System.Drawing.Size(5, 512);
            // 
            // lciSaveReport
            // 
            this.lciSaveReport.Control = this.ceiSaveReport;
            this.lciSaveReport.CustomizationFormText = "Lưu báo cáo";
            this.lciSaveReport.Location = new System.Drawing.Point(635, 0);
            this.lciSaveReport.Name = "lciSaveReport";
            this.lciSaveReport.Size = new System.Drawing.Size(318, 26);
            this.lciSaveReport.Text = "Lưu báo cáo";
            this.lciSaveReport.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciSaveReport.TextSize = new System.Drawing.Size(59, 13);
            this.lciSaveReport.TextToControlDistance = 5;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.grdHistory;
            this.gridView1.Name = "gridView1";
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.grdIp;
            this.gridView2.Name = "gridView2";
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.grdKeyword;
            this.gridView4.Name = "gridView4";
            // 
            // gridView5
            // 
            this.gridView5.GridControl = this.grdOBC;
            this.gridView5.Name = "gridView5";
            // 
            // gridView6
            // 
            this.gridView6.GridControl = this.grdMultiProxy;
            this.gridView6.Name = "gridView6";
            // 
            // gridView7
            // 
            this.gridView7.GridControl = this.grdMultiXProxy;
            this.gridView7.Name = "gridView7";
            // 
            // gridView8
            // 
            this.gridView8.GridControl = this.grdMultiOBC;
            this.gridView8.Name = "gridView8";
            // 
            // gridView9
            // 
            this.gridView9.GridControl = this.grdOBCV2;
            this.gridView9.Name = "gridView9";
            // 
            // gridView10
            // 
            this.gridView10.GridControl = this.grdTinsoft;
            this.gridView10.Name = "gridView10";
            // 
            // gridView11
            // 
            this.gridView11.GridControl = this.grdTMProxy;
            this.gridView11.Name = "gridView11";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1276, 677);
            this.Controls.Add(this.lcMain);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NATECH SEO - Design by NATECH (fb.com/na.com.vn)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSendMail_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsieStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeeLang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcMain)).EndInit();
            this.lcMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mmeHistory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDeviceType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.luevDeviceType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ccbBrowserLanguage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiClearChrome.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiCallPhoneZalo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speClearChromeTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiViewFilm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDisplayMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiStarupWindow.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speDCOMProxyDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSpeedKeyboard.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speInternalCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speLoadProfilePercent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiViewYoutube.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTMProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvTMProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTMProxy_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTMProxy_TinhTP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvbeiTMProxy_Xoa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speDcomDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speResetDcomInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDcomTypeReset.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeProxySupplier.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTinsoft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvTinsoft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTinsoft_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvluevTinsoft_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvccbTinsoft_TinhTP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueTinsoft_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvbeiTinsoft_Delete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvcbeTinsoft_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memOBCV3Proxy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOBCV3Host.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiSaveReport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOBCV2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvOBCV2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOBCV2Host.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiAutoStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSubLinkViewTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speOtherSiteViewTimeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewSearchTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMultiOBC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMultiOBC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueMultiOBC_IsRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMultiXProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMultiXProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueMultiXProxy_IsRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiProxyType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMultiProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvMultiProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueMultiProxy_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOBC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvOBC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOBCHost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdKeyword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvKeyword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.luevSubLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvbeiKeyword_Delete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvlueKeyword_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvluevKeyword_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memReChuot.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdXProxyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvXProxyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiIsRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXProxyHost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speMACAddressInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiChangeMACAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSubLinkView.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvIp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSoLuong.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memProfile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGMail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdvHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memProxyNote.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memProxy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.raiTypeProxy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDialUp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTypeIp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeGoogleSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeout.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speEmailDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiNotViewImage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiUseHistory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherSiteUrl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speOtherSiteViewTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceiViewOtherSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAgent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSoTrang.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSumClick.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTimeViewFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem30)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgTuKhoa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTimeSetup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgTraffic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSuDung)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOtherSiteListUrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOtherSiteViewTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOtherSiteViewTimeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGoogleSite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSoLanClick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDuyet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOtherConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciUseHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAutoStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLoadProfilePercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStarupWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDisplayMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClearChrome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem40)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBrowserLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTimeGoogle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSpeedKeyboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciThoiGianTK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeViewSearchTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgViewAds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeViewFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTimeViewTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTimeInternalExternal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem63)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSubLinkTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSubLinkViewTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem39)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCallPhoneZalo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciViewYoutube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciNotViewImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgLoginGmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagGMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEmailDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciListEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciListProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem53)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem60)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgUserAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbeNoticeAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem38)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem62)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDeviceType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgFakeIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgDcom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabProxyMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOBCv2Proxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem49)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem50)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem45)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem54)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem65)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgProxyFree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgFreeProxySub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgAddonGetProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgFreeProxyConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTinSoftProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem37)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem52)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem47)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem55)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgXProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciHostXProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem36)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOBCProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem35)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOBCv2Proxy_Old)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem43)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem46)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMultiProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem29)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem34)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTMProxy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem57)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem58)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProxySupplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgListDcom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDialUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem56)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResetDcomInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDcomDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem48)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChangeIp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgChangeMac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChangeMac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChangeMacTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem51)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciReportIp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgReportDomain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSaveReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView11)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}
}
