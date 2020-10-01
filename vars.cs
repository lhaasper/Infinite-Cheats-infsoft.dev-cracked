using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Launcher.Dependencies;
using Launcher.Framework;
using Launcher.Language.Resources;
using Launcher.Presentation;
using Launcher.Presentation.Interfaces;

namespace Launcher
{
	// Token: 0x02000017 RID: 23
	public static class vars
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000276C File Offset: 0x0000276C
		static vars()
		{
			vars.ApproximateAppSize = 19849344;
			vars.hURIDownload = Texts.inf_down_url;
			vars.g_gen = new Random((int)DateTime.Now.Ticks);
			vars.AppModifyStartTime = new DateTime(2020, 9, 1, vars.g_gen.Next(0, 24), vars.g_gen.Next(0, 60), vars.g_gen.Next(0, 60));
			vars.UKeyProperty = "unknown";
			vars.g_currentLanguage = eLanguage.eEng;
			vars.appFileName = Process.GetCurrentProcess().MainModule.FileName;
			vars.appNewFileName = util.GetUniqueKey(new Random().Next(5, 8), false) + ".exe";
			vars.errorlogName = "error_log.log";
			vars.LogFilePath = Directory.GetCurrentDirectory() + "\\" + vars.errorlogName;
			vars.Logger = FileLogger.CreateLogger("");
			vars.IsAmdProperty = false;
			vars.IsLaunchAccessProperty = false;
			vars.IsHardVtEnableErrorProperty = false;
			vars.IsHardVtSupportErrorProperty = false;
			vars.IsSystemSupportErrorProperty = false;
			vars.IsShowProductSuccessProperty = true;
			vars.IsShowMainNotesProperty = false;
			vars.openUriCommand = new SimpleCommand<object>(delegate(object o)
			{
				try
				{
					if (o is string)
					{
						Process.Start(new ProcessStartInfo(o as string));
					}
				}
				catch (Exception)
				{
				}
			});
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000292F File Offset: 0x0000292F
		public static SimpleCommand<object> openUriCommand { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002936 File Offset: 0x00002936
		internal static string LogDirectory
		{
			get
			{
				return vars.<LogDirectory>k__BackingField;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000293D File Offset: 0x0000293D
		public static bool IsActiveKey()
		{
			return vars.is_active;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002944 File Offset: 0x00002944
		public static bool IsRestricted()
		{
			return vars.is_restricted;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000294B File Offset: 0x0000294B
		public static bool HasInit
		{
			get
			{
				return vars.is_init;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002952 File Offset: 0x00002952
		public static void SetInit(bool value)
		{
			vars.is_init = value;
			if (vars.timeoutWorker != null)
			{
				vars.timeoutWorker.CancelAsync();
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000296B File Offset: 0x0000296B
		public static void SetRestricted(bool value)
		{
			vars.is_restricted = value;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002973 File Offset: 0x00002973
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0000297A File Offset: 0x0000297A
		public static string AppVersion { get; set; } = "1.8.3.0";

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000039 RID: 57 RVA: 0x00002984 File Offset: 0x00002984
		// (remove) Token: 0x0600003A RID: 58 RVA: 0x000029B8 File Offset: 0x000029B8
		public static event PropertyChangedEventHandler StaticPropertyChanged;

		// Token: 0x0600003B RID: 59 RVA: 0x000029EB File Offset: 0x000029EB
		private static void OnStaticPropertyChanged(string propertyName = "")
		{
			PropertyChangedEventHandler staticPropertyChanged = vars.StaticPropertyChanged;
			if (staticPropertyChanged == null)
			{
				return;
			}
			staticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002A03 File Offset: 0x00002A03
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002A0A File Offset: 0x00002A0A
		public static string UKeyProperty
		{
			get
			{
				return vars.keyProperty;
			}
			set
			{
				vars.keyProperty = value;
				vars.OnStaticPropertyChanged("UKeyProperty");
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002A1C File Offset: 0x00002A1C
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002A23 File Offset: 0x00002A23
		public static string LogFilePath
		{
			get
			{
				return vars.logFileName;
			}
			set
			{
				vars.logFileName = value;
				vars.OnStaticPropertyChanged("LogFilePath");
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002A35 File Offset: 0x00002A35
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002A3C File Offset: 0x00002A3C
		public static bool IsAmdProperty
		{
			get
			{
				return vars.isAmd;
			}
			set
			{
				if (vars.isAmd != value)
				{
					vars.isAmd = value;
					vars.OnStaticPropertyChanged("IsAmdProperty");
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002A56 File Offset: 0x00002A56
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002A5D File Offset: 0x00002A5D
		public static string ProductUriProperty
		{
			get
			{
				return vars.hCurrentProductUri;
			}
			set
			{
				vars.hCurrentProductUri = value;
				vars.OnStaticPropertyChanged("ProductUriProperty");
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002A6F File Offset: 0x00002A6F
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002A76 File Offset: 0x00002A76
		public static string ProductDateProperty
		{
			get
			{
				return vars.productDateProperty;
			}
			set
			{
				vars.productDateProperty = value;
				vars.OnStaticPropertyChanged("ProductDateProperty");
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002A88 File Offset: 0x00002A88
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002A8F File Offset: 0x00002A8F
		public static string ProductStatusProperty
		{
			get
			{
				return vars.productStatusProperty;
			}
			set
			{
				vars.productStatusProperty = value;
				vars.OnStaticPropertyChanged("ProductStatusProperty");
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002AA1 File Offset: 0x00002AA1
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002AA8 File Offset: 0x00002AA8
		public static bool IsLaunchAccessProperty
		{
			get
			{
				return vars.isLaunchAccess;
			}
			set
			{
				if (vars.isLaunchAccess != value)
				{
					vars.isLaunchAccess = value;
					vars.OnStaticPropertyChanged("IsLaunchAccessProperty");
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002AC2 File Offset: 0x00002AC2
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002AC9 File Offset: 0x00002AC9
		public static bool IsHardVtEnableErrorProperty
		{
			get
			{
				return vars.isHardVtEnableError;
			}
			set
			{
				if (vars.isHardVtEnableError != value)
				{
					vars.isHardVtEnableError = value;
					vars.OnStaticPropertyChanged("IsHardVtEnableErrorProperty");
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002AE3 File Offset: 0x00002AE3
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002AEA File Offset: 0x00002AEA
		public static bool IsHardVtSupportErrorProperty
		{
			get
			{
				return vars.isHardVtSupportError;
			}
			set
			{
				if (vars.isHardVtSupportError != value)
				{
					vars.isHardVtSupportError = value;
					vars.OnStaticPropertyChanged("IsHardVtSupportErrorProperty");
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002B04 File Offset: 0x00002B04
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002B0B File Offset: 0x00002B0B
		public static bool IsSystemSupportErrorProperty
		{
			get
			{
				return vars.isSystemSupportError;
			}
			set
			{
				if (vars.isSystemSupportError != value)
				{
					vars.isSystemSupportError = value;
					vars.OnStaticPropertyChanged("IsSystemSupportErrorProperty");
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002B25 File Offset: 0x00002B25
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002B2C File Offset: 0x00002B2C
		public static bool IsShowProductSuccessProperty
		{
			get
			{
				return vars.isShowProductSuccess;
			}
			set
			{
				if (vars.isShowProductSuccess != value)
				{
					vars.isShowProductSuccess = value;
					vars.OnStaticPropertyChanged("IsShowProductSuccessProperty");
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002B46 File Offset: 0x00002B46
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002B4D File Offset: 0x00002B4D
		public static bool IsShowMainNotesProperty
		{
			get
			{
				return vars._IsShowMainNotesProperty;
			}
			set
			{
				if (vars._IsShowMainNotesProperty != value)
				{
					vars._IsShowMainNotesProperty = value;
					vars.OnStaticPropertyChanged("IsShowMainNotesProperty");
				}
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002B67 File Offset: 0x00002B67
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002B6E File Offset: 0x00002B6E
		public static nf_server SelectedServerProperty
		{
			get
			{
				return vars._SelectedServerProperty;
			}
			set
			{
				if (vars._SelectedServerProperty != value)
				{
					vars._SelectedServerProperty = value;
					vars.OnStaticPropertyChanged("SelectedServerProperty");
				}
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002B88 File Offset: 0x00002B88
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002B8F File Offset: 0x00002B8F
		public static bool IsLaunchIndicatorVisible
		{
			get
			{
				return vars._IsLaunchIndicatorVisible;
			}
			set
			{
				if (vars._IsLaunchIndicatorVisible != value)
				{
					vars._IsLaunchIndicatorVisible = value;
					vars.OnStaticPropertyChanged("IsLaunchIndicatorVisible");
				}
			}
		}

		// Token: 0x04000035 RID: 53
		public static int ApproximateAppSize;

		// Token: 0x04000036 RID: 54
		public static Random g_gen;

		// Token: 0x04000037 RID: 55
		public static DateTime AppModifyStartTime;

		// Token: 0x04000038 RID: 56
		public const string STATUS_SUCCESS = "1";

		// Token: 0x04000039 RID: 57
		public static BackgroundWorker timeoutWorker = null;

		// Token: 0x0400003A RID: 58
		public static Dictionary<string, string> ResponceMain = null;

		// Token: 0x0400003B RID: 59
		public static object locker = new object();

		// Token: 0x0400003C RID: 60
		public static bool is_init = false;

		// Token: 0x0400003D RID: 61
		public static bool is_update = false;

		// Token: 0x0400003E RID: 62
		public static bool is_active = false;

		// Token: 0x0400003F RID: 63
		public static bool is_restricted = false;

		// Token: 0x04000040 RID: 64
		public static string appFileName;

		// Token: 0x04000041 RID: 65
		public static string appNewFileName;

		// Token: 0x04000043 RID: 67
		public static string hURIDownload;

		// Token: 0x04000044 RID: 68
		public static string errorlogName;

		// Token: 0x04000045 RID: 69
		public static string logFileName;

		// Token: 0x04000046 RID: 70
		public static string hCurrentProductUri;

		// Token: 0x04000047 RID: 71
		public static ILogger Logger;

		// Token: 0x04000048 RID: 72
		public static eLanguage g_currentLanguage;

		// Token: 0x04000049 RID: 73
		public static bool IsWin7 = false;

		// Token: 0x0400004A RID: 74
		public static bool IsWin10_20H = false;

		// Token: 0x0400004B RID: 75
		public static string keyProperty;

		// Token: 0x0400004C RID: 76
		public static string productDateProperty;

		// Token: 0x0400004D RID: 77
		public static string productStatusProperty;

		// Token: 0x0400004E RID: 78
		public static bool isSystemSupportError;

		// Token: 0x0400004F RID: 79
		public static bool g_isHardVtSupport;

		// Token: 0x04000050 RID: 80
		public static bool isHardVtSupportError;

		// Token: 0x04000051 RID: 81
		public static bool g_isHardVtEnable;

		// Token: 0x04000052 RID: 82
		public static bool isHardVtEnableError;

		// Token: 0x04000053 RID: 83
		public static bool isAmd;

		// Token: 0x04000054 RID: 84
		public static bool isLaunchAccess;

		// Token: 0x04000055 RID: 85
		public static bool isShowProductSuccess;

		// Token: 0x04000056 RID: 86
		public static Dictionary<eVm, byte> vm_state = null;

		// Token: 0x04000057 RID: 87
		public static Dictionary<nf_server, Tuple<eKeyStates, string, string>> clientMembershipData = null;

		// Token: 0x04000058 RID: 88
		public static object lock_data = new object();

		// Token: 0x04000059 RID: 89
		public static Dictionary<nf_server, Notes> serverNotes = null;

		// Token: 0x0400005A RID: 90
		public static object lock_notes = new object();

		// Token: 0x0400005B RID: 91
		public static Dictionary<nf_server, string> ServersCollection = null;

		// Token: 0x0400005C RID: 92
		public static Dictionary<eHashesType, bool> hashCompareStates = null;

		// Token: 0x0400005D RID: 93
		public static Dictionary<eHashes, string> GamesAndDllsPath = null;

		// Token: 0x0400005E RID: 94
		public static Dictionary<nf_server, KeyValuePair<bool, string>> ServersPatchState = null;

		// Token: 0x0400005F RID: 95
		public static object lock_states = new object();

		// Token: 0x04000060 RID: 96
		public static List<Tuple<nf_server, bool, string>> list_nfEvnts = new List<Tuple<nf_server, bool, string>>();

		// Token: 0x04000063 RID: 99
		private static bool _IsShowMainNotesProperty;

		// Token: 0x04000064 RID: 100
		private static nf_server _SelectedServerProperty;

		// Token: 0x04000065 RID: 101
		private static bool _IsLaunchIndicatorVisible;
	}
}
