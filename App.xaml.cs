using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Launcher.CustomControls.Dialog;
using Launcher.Dependencies;
using Launcher.Dependencies.API;
using Launcher.Framework;
using Launcher.Language;

namespace Launcher
{
	// Token: 0x02000019 RID: 25
	public partial class App : Application
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002C34 File Offset: 0x00002C34
		private bool HasCriticalException
		{
			get
			{
				return this.criticalException != null;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002C3F File Offset: 0x00002C3F
		static App()
		{
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.Resolve;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002C58 File Offset: 0x00002C58
		public App()
		{
			try
			{
				util.Initialize(typeof(App).Module);
				base.DispatcherUnhandledException += this.OnApplicationDispatcherUnhandledException;
				AppDomain.CurrentDomain.UnhandledException += this.OnCurrentDomainUnhandledException;
				CLanguage.SetLanguage(null);
				Launcher.MainWindow.app = this;
			}
			catch (Exception ex)
			{
				this.criticalException = ex;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002CD0 File Offset: 0x00002CD0
		public static void AppExit()
		{
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002CDC File Offset: 0x00002CDC
		private bool InitializeProgram()
		{
			try
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				if (commandLineArgs.Length == 3)
				{
					string text = crypt.Decode64(commandLineArgs[0]);
					string processName = Path.GetFileName(text).Replace(".exe", "");
					while (Process.GetProcessesByName(processName).Length != 0)
					{
						Process[] processesByName = Process.GetProcessesByName(processName);
						for (int i = 1; i < processesByName.Length; i++)
						{
							processesByName[i].Kill();
						}
						Thread.Sleep(5);
					}
					util.ForceDeleteFile(text);
				}
				else if (commandLineArgs.Length == 2)
				{
					string text2 = crypt.Decode64(commandLineArgs[0]);
					while (!string.IsNullOrEmpty(text2))
					{
						if (!File.Exists(text2))
						{
							break;
						}
						util.ForceDeleteFile(text2);
						Thread.Sleep(5);
					}
				}
				else if (commandLineArgs.Length == 1)
				{
					string appFileName = vars.appFileName;
					string text3 = Directory.GetCurrentDirectory() + "\\" + (util.GetUniqueKey(new Random().Next(5, 8), false) + ".exe");
					File.Copy(appFileName, text3);
					DateTime randomDate = util.GetRandomDate(vars.AppModifyStartTime, DateTime.Now);
					using (FileStream fileStream = new FileStream(text3, FileMode.Open))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							binaryWriter.Seek(136, SeekOrigin.Begin);
							binaryWriter.Write(randomDate.ToFileTimeUtc());
							binaryWriter.Close();
						}
						fileStream.Close();
					}
					util.SetFltCustomInfo(text3, true, false);
					util.SetFltCustomInfo(appFileName, false, false);
					if (File.Exists(text3))
					{
						this.Cleanup();
						util.CreateProcess(text3, string.Format("{0} {1}", crypt.EncodeTo64(appFileName), new Random().Next(54).ToString()), 16777216U);
					}
					return false;
				}
				Launcher.MainWindow.InitializeAll();
				Launcher.MainWindow.getInstance().Show();
			}
			catch (Exception exception)
			{
				this.HandleCriticalError(exception);
			}
			return true;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002F04 File Offset: 0x00002F04
		protected override void OnStartup(StartupEventArgs e)
		{
			if (this.HasCriticalException)
			{
				this.HandleCriticalError(this.criticalException);
				return;
			}
			base.OnStartup(e);
			try
			{
				bool flag = true;
				Process[] processesByName = Process.GetProcessesByName("r5Apex");
				IntPtr value = WinAPI.FindWindow("CryENGINE", "Warface");
				Process[] processesByName2 = Process.GetProcessesByName("Game");
				Process[] processesByName3 = Process.GetProcessesByName("GameCenter");
				if (processesByName2.Length != 0 || value != IntPtr.Zero || processesByName.Length != 0)
				{
					flag = false;
				}
				if (processesByName3.Length != 0)
				{
					Protect.KillProcesses(new string[]
					{
						"GameCenter"
					});
				}
				if (flag)
				{
					if (!util.IsConnectedToInternet())
					{
						base.Dispatcher.Invoke(delegate()
						{
							CustomMessageBox.Show(CLanguage.GetTranslateText("errorconnect"), CLanguage.GetTranslateText("err_header_msg"), MessageBoxButton.OK, false);
						});
						flag = false;
					}
					if (!util.IsRunAsAdmin())
					{
						Process.Start(new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName)
						{
							Verb = "runas"
						});
						flag = false;
					}
				}
				if (flag)
				{
					this.appMutex = new INFMutex(new Func<bool>(this.InitializeProgram));
					flag = this.appMutex.InitializeProgramOrSendMessage();
				}
				if (!flag)
				{
					this.Cleanup();
					App.AppExit();
				}
			}
			catch (Exception)
			{
				this.Cleanup();
				App.AppExit();
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003044 File Offset: 0x00003044
		private void HandleCriticalError(Exception exception)
		{
			if (exception != null)
			{
				this.HandleUnhandledException(exception);
				App.AppExit();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003058 File Offset: 0x00003058
		private void HandleUnhandledException(Exception exception)
		{
			if (!this.hasUnhandledException)
			{
				this.hasUnhandledException = true;
				util.CreateLog(string.Format("[HRESULT: 0x{0:X8} - {1}] {2} [Line : {3}]", new object[]
				{
					exception.HResult,
					exception.GetType().Name,
					exception.Message,
					exception.LineNumber()
				}));
				base.Dispatcher.Invoke(delegate()
				{
					MessageBox.Show("The app must close due to an error. The error log file is created.", "Launcher error");
				});
				this.Cleanup();
				App.AppExit();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000030F4 File Offset: 0x000030F4
		private void OnApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleUnhandledException(e.Exception);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000310C File Offset: 0x0000310C
		private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
			{
				Exception ex = e.ExceptionObject as Exception;
				if (ex != null)
				{
					this.HandleUnhandledException(ex);
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003137 File Offset: 0x00003137
		private void Cleanup()
		{
			base.Dispatcher.Invoke(delegate()
			{
				appMutex appMutex = this.appMutex;
				if (appMutex == null)
				{
					return;
				}
				appMutex.Dispose();
			});
		}

		// Token: 0x04000068 RID: 104
		private appMutex appMutex;

		// Token: 0x04000069 RID: 105
		private Exception criticalException;

		// Token: 0x0400006A RID: 106
		private bool hasUnhandledException;
	}
}
