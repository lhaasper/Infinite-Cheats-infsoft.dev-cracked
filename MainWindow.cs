using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Launcher.Common.Components;
using Launcher.Components;
using Launcher.CustomControls.AdornedControl;
using Launcher.CustomControls.CircularProgressBar;
using Launcher.CustomControls.Dialog;
using Launcher.Dependencies;
using Launcher.Dependencies.API;
using Launcher.Internal.Data;
using Launcher.Language;
using Launcher.Language.Resources;
using Launcher.Presentation;
using Launcher.Presentation.Interfaces;
using Launcher.UserControls;
using Microsoft.Win32;
using SciChart.Wpf.UI.Transitionz;

namespace Launcher
{
	// Token: 0x0200001A RID: 26
	public class MainWindow : Window, IDisplayMainWindowPages, IComponentConnector
	{
		// Token: 0x0600006C RID: 108 RVA: 0x000031BE File Offset: 0x000031BE
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			MainWindow.ExitApp();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000031CC File Offset: 0x000031CC
		private void GridHeader_MouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			if (e.ChangedButton == MouseButton.Left)
			{
				this.clicked = true;
				this.m_toolTipClosingTimer.Change(350, -1);
				base.DragMove();
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000031FC File Offset: 0x000031FC
		private void GridHeader_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000031FE File Offset: 0x000031FE
		private void GridHeader_MouseMove(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003200 File Offset: 0x00003200
		private void GridHeader_MouseLeave(object sender, MouseEventArgs e)
		{
			if (this.clicked)
			{
				this.m_toolTipClosingTimer.Change(-1, -1);
				this.clicked = false;
				this.leaved = true;
			}
			if (this.leaved && base.Opacity < 1.0)
			{
				this.leaved = false;
				base.BeginStoryboard(this.HeaderStoryBoardUp);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000325D File Offset: 0x0000325D
		private void XmainWindow_MouseLeave(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000325F File Offset: 0x0000325F
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.ExitApp();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003266 File Offset: 0x00003266
		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000326F File Offset: 0x0000326F
		private void ToolTipClosingCallBack(object p_useless)
		{
			base.Dispatcher.InvokeAsync(delegate()
			{
				if (this.clicked)
				{
					base.BeginStoryboard(this.HeaderStoryBoardDown);
				}
			});
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000328C File Offset: 0x0000328C
		public MainWindow(IMainWindowRegionManager regionManager)
		{
			this.InitializeComponent();
			this.m_toolTipClosingTimer = new Timer(new TimerCallback(this.ToolTipClosingCallBack), null, -1, -1);
			this.HeaderStoryBoardDown = (Storyboard)base.FindResource("HeaderStoryBoardDown");
			this.HeaderStoryBoardUp = (Storyboard)base.FindResource("HeaderStoryBoardUp");
			this.AddRegionContent(regionManager);
			this.PagesHost.ClosePage();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003300 File Offset: 0x00003300
		private void AddRegionContent(IMainWindowRegionManager regionManager)
		{
			this.HelpControl.RegionContent = regionManager.GetHelpControl();
			this.AboutControl.RegionContent = regionManager.GetAboutControl();
			this.ProductControl.RegionContent = regionManager.GetProductControl();
			this.MainNewsControl.RegionContent = regionManager.GetNewsBarControl();
			this.MenuBarControl.RegionContent = regionManager.GetMenuBarControl();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003362 File Offset: 0x00003362
		public static MainWindow getInstance()
		{
			return MainWindow.mainWindow;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003369 File Offset: 0x00003369
		public void InitializeApp(App app)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00C924A0 File Offset: 0x00C924A0
		public static void InitializeAll()
		{
			new 0A0E460F().6BED6B99(null, 66506);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000354C File Offset: 0x0000354C
		private void Completed(object sender, AsyncCompletedEventArgs e)
		{
			try
			{
				DateTime randomDate = util.GetRandomDate(vars.AppModifyStartTime, DateTime.Now);
				using (FileStream fileStream = new FileStream(vars.appNewFileName, FileMode.Open))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
					{
						binaryWriter.Seek(136, SeekOrigin.Begin);
						binaryWriter.Write(randomDate.ToFileTimeUtc());
						binaryWriter.Close();
					}
					fileStream.Close();
				}
				util.SetFltCustomInfo(vars.appNewFileName, true, false);
				util.CreateProcess(vars.appNewFileName, string.Format("{0} {1} {2}", crypt.EncodeTo64(vars.appFileName), "FF", "FFF"), 16777216U);
				MainWindow.ExitApp();
			}
			catch (Exception ex)
			{
				util.CreateLog(string.Format("[HRESULT: 0x{0:X8}] {1}", ex.HResult, ex.Message));
				MessageBox.Show(this, ex.Message, CLanguage.GetTranslateText("err_header_msg"));
				MainWindow.ExitApp();
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003664 File Offset: 0x00003664
		private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (this.cpb.Value != (double)e.ProgressPercentage)
			{
				this.cpb.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000368C File Offset: 0x0000368C
		private void downloadFile()
		{
			try
			{
				DispatchService.Dispatch(new Action(this.<downloadFile>b__23_0));
			}
			catch (WebException ex)
			{
				DispatchService.Dispatch(delegate
				{
					this.gridDownload.Visibility = Visibility.Collapsed;
				});
				util.CreateLog(string.Format("[HRESULT: 0x{0:X8}] {1}", ex.HResult, ex.Message));
				DispatchService.Dispatch(delegate
				{
					CustomMessageBox.Show(CLanguage.GetTranslateText("srvconnect"), CLanguage.GetTranslateText("error_srv_out"), MessageBoxButton.OK, true);
				});
				MainWindow.ExitApp();
			}
			catch (Exception ex2)
			{
				DispatchService.Dispatch(delegate
				{
					this.gridDownload.Visibility = Visibility.Collapsed;
				});
				util.CreateLog(string.Format("[HRESULT: 0x{0:X8}] {1}", ex2.HResult, ex2.Message));
				DispatchService.Dispatch(delegate
				{
					CustomMessageBox.Show(CLanguage.GetTranslateText("srvconnect"), CLanguage.GetTranslateText("error_srv_out"), MessageBoxButton.OK, true);
				});
				MainWindow.ExitApp();
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00C924C0 File Offset: 0x00C924C0
		private string selector(string val, bool IsOptionally = false)
		{
			uint num = 141196919U;
			object obj;
			if (num <= 1204357138U)
			{
				object locker = vars.locker;
				num = 1161263444U * num;
				obj = locker;
			}
			num %= 1330137197U;
			bool flag = (num ^ 134247839U) != 0U;
			num = 1787505127U * num;
			bool flag2 = flag;
			string result2;
			try
			{
				if (num * 1271548271U == 0U)
				{
					goto IL_61;
				}
				IL_43:
				Monitor.Enter(obj, ref flag2);
				num >>= 31;
				IL_53:
				if (IsOptionally)
				{
					num = (1044646843U ^ num);
				}
				else
				{
					num = 266147855U >> (int)num;
					if (839586521U < num)
					{
						goto IL_43;
					}
					bool responceMain = vars.ResponceMain != null;
					num >>= 0;
					if (responceMain)
					{
						if (num > 859181631U)
						{
							goto IL_61;
						}
						Dictionary<string, string> responceMain2 = vars.ResponceMain;
						num = 514459531U - num;
						bool flag3 = responceMain2.ContainsKey(val);
						num = 2028566868U - num;
						if (flag3)
						{
							goto IL_214;
						}
						num ^= 1707307479U;
					}
					num -= 1029636828U;
					vars.SetInit(num + 896562902U != 0U);
					if (num == 1880513779U)
					{
						goto IL_43;
					}
					string str = "STATUS_NOT_FOUND[";
					num |= 868641370U;
					num = 563414914U % num;
					string str2 = str + new string(val.Reverse<char>().ToArray<char>()) + "]";
					num = 1319643108U % num;
					util.CreateLog(str2);
					num = 404450856U + num;
					Action action;
					bool flag4 = (action = MainWindow.<>c.<>9__24_0) != null;
					num = (2029211749U ^ num);
					if (!flag4)
					{
						object <> = MainWindow.<>c.<>9;
						num = (1776646697U ^ num);
						IntPtr method = ldftn(<selector>b__24_0);
						num = 763778692U + num;
						Action action2 = new Action(<>, method);
						num = 288648858U >> (int)num;
						action = action2;
						MainWindow.<>c.<>9__24_0 = action2;
						num ^= 1534845699U;
					}
					num %= 781267030U;
					DispatchService.Dispatch(action);
					MainWindow.ExitApp();
					num ^= 1324636871U;
					IL_214:
					num /= 1896745154U;
					if (num <= 214237517U)
					{
						string result = vars.ResponceMain[val];
						num = 241896902U >> (int)num;
						return result;
					}
					goto IL_43;
				}
				IL_61:
				string text;
				if (vars.ResponceMain != null)
				{
					num = 255153475U / num;
					if (num >= 650207054U)
					{
						goto IL_53;
					}
					Dictionary<string, string> responceMain3 = vars.ResponceMain;
					num <<= 23;
					bool flag5 = responceMain3.ContainsKey(val);
					num /= 481712925U;
					if (!flag5)
					{
						num += 1044646842U;
					}
					else
					{
						if ((1521372485U ^ num) != 0U)
						{
							text = vars.ResponceMain[val];
							num ^= 1044646842U;
							goto IL_CF;
						}
						goto IL_43;
					}
				}
				text = "0";
				IL_CF:
				num = (2062812110U ^ num);
				result2 = text;
				if (num * 1375997934U == 0U)
				{
					goto IL_53;
				}
			}
			finally
			{
				num = 1057171347U;
				if ((num ^ 375459643U) != 0U)
				{
					if (!flag2)
					{
						goto IL_27F;
					}
					num += 1568080502U;
					if (num <= 1647997179U)
					{
						goto IL_27F;
					}
				}
				Monitor.Exit(obj);
				num ^= 2742619546U;
				IL_27F:;
			}
			return result2;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00C9276C File Offset: 0x00C9276C
		private bool getInternalData(out string key)
		{
			try
			{
				MainWindow.<>c__DisplayClass25_0 CS$<>8__locals1;
				uint num;
				for (;;)
				{
					IL_00:
					CS$<>8__locals1 = new MainWindow.<>c__DisplayClass25_0();
					for (;;)
					{
						IL_06:
						num = 2139766417U;
						long num2 = (long)(num ^ 2139766417U);
						num = 986909328U * num;
						long num3 = num2;
						if (num / 252607756U != 0U)
						{
							IntPtr intPtr5;
							uint? num11;
							for (;;)
							{
								IL_2E:
								for (;;)
								{
									num -= 1288703772U;
									if (524640355U == num)
									{
										goto IL_00;
									}
									long num4 = num3;
									num = 1662080742U - num;
									byte[] buffer = szData.buffer;
									num &= 1951952644U;
									long num5 = (long)buffer.Length;
									num %= 1690204167U;
									if (num4 >= num5)
									{
										num -= 915692355U;
										if (673717484U - num != 0U)
										{
											break;
										}
									}
									num = 432865921U;
									if (690099049U - num != 0U)
									{
										byte[] buffer2 = szData.buffer;
										IntPtr intPtr = checked((IntPtr)num3);
										byte b = (byte)((uint)buffer2[(int)intPtr] ^ (num ^ 432865977U));
										num = 1214060088U >> (int)num;
										buffer2[(int)intPtr] = b;
										num /= 1476752532U;
										if (2097707251U < num)
										{
											goto IL_00;
										}
										long num6 = num3;
										long num7 = (long)(num - uint.MaxValue);
										num = 1318399860U - num;
										long num8 = num7;
										num = 802913525U / num;
										long num9 = num6 + num8;
										num += 1623942333U;
										num3 = num9;
										num += 787248851U;
									}
								}
								do
								{
									GCHandle gchandle = GCHandle.Alloc(szData.buffer, (GCHandleType)(num ^ 3916672702U));
									num *= 550332677U;
									GCHandle gchandle2 = gchandle;
									if (1817850069U - num == 0U)
									{
										goto IL_00;
									}
									num = (567042717U | num);
									IntPtr image = gchandle2.AddrOfPinnedObject();
									IntPtr zero = IntPtr.Zero;
									num *= 1814330628U;
									IntPtr intPtr2 = zero;
									string expname = "Ldrp";
									num |= 1792547821U;
									IntPtr intPtr3 = util.ExMapGetProcAddress(image, expname);
									num ^= 1733574532U;
									if (intPtr3 == IntPtr.Zero)
									{
										goto Block_6;
									}
									uint dwSize = num - 497950595U;
									uint page_EXECUTE_READWRITE = WinAPI.Win32Constants.PAGE_EXECUTE_READWRITE;
									num = 1412648422U - num;
									uint num10;
									WinAPI.VirtualProtect(intPtr3, dwSize, page_EXECUTE_READWRITE, out num10);
									num = 793260042U + num;
									util_data.Loader unmanagedFunction = util.GetUnmanagedFunction<util_data.Loader>(intPtr3);
									num *= 1476033791U;
									bool flag = unmanagedFunction(ref intPtr2) != 0UL;
									num = 502224346U + num;
									if (!flag)
									{
										goto IL_230;
									}
									num = 1643923122U >> (int)num;
									IntPtr value = intPtr2;
									num = 762462738U / num;
									if (value == IntPtr.Zero)
									{
										goto Block_8;
									}
									IntPtr ptr = intPtr2;
									num = 662785974U << (int)num;
									Type typeFromHandle = typeof(util_data.Instances);
									num *= 12009245U;
									util_data.pInstances = new util_data.Instances?((util_data.Instances)Marshal.PtrToStructure(ptr, typeFromHandle));
									num = 680551635U / num;
									if (17243036U - num == 0U)
									{
										goto IL_06;
									}
									gchandle2.Free();
									num = 1488219358U >> (int)num;
									if ((815334652U & num) == 0U)
									{
										goto IL_00;
									}
									MainWindow.<>c__DisplayClass25_0 CS$<>8__locals2 = CS$<>8__locals1;
									util_data.EREQUIRED pRequired = default(util_data.EREQUIRED);
									num /= 1256785020U;
									num = 196036358U % num;
									string appVersion = vars.AppVersion;
									string oldValue = ".";
									string newValue = "";
									num ^= 1362457576U;
									pRequired.p_lver = appVersion.Replace(oldValue, newValue);
									CS$<>8__locals2.pRequired = pRequired;
									num = 252911295U % num;
									if (num < 6956137U)
									{
										goto IL_2E;
									}
									IntPtr zero2 = IntPtr.Zero;
									bool bManualReset = (num ^ 252911295U) != 0U;
									num = (730232264U ^ num);
									IntPtr intPtr4 = WinAPI.CreateEvent(zero2, bManualReset, (num ^ 613772151U) != 0U, string.Empty);
									num /= 237128158U;
									intPtr5 = intPtr4;
									num *= 734878220U;
									num = 406143111U - num;
									bool flag2 = util_data.pInstances != null;
									num = (1004626040U | num);
									if (flag2)
									{
										goto IL_3B3;
									}
									num = 1151035770U * num;
								}
								while (num <= 871975836U);
								num11 = null;
								if (1644695520U >= num)
								{
									goto Block_14;
								}
							}
							IL_3FC:
							num ^= 723666422U;
							uint? num12;
							num11 = num12;
							uint num13 = num ^ 2110885488U;
							if ((num ^ 170797678U) == 0U)
							{
								continue;
							}
							uint valueOrDefault = num11.GetValueOrDefault();
							num += 1281189773U;
							bool flag3 = valueOrDefault == num13;
							num *= 2002918059U;
							bool flag4 = num11 != null;
							num = 749821068U << (int)num;
							if (flag3 && flag4)
							{
								break;
							}
							num <<= 17;
							IntPtr hObject = intPtr5;
							num = 180041912U + num;
							int dwTimeout = (int)(num ^ 180040448U);
							num = 611926499U - num;
							bool flag5 = WinAPI.WaitForSingleObject(hObject, dwTimeout) != 0U;
							WinAPI.CloseHandle(intPtr5);
							num >>= 12;
							if (!flag5)
							{
								goto IL_546;
							}
							bool is_restricted = num - 105439U != 0U;
							num -= 1038244975U;
							vars.is_restricted = is_restricted;
							num = 171928590U + num;
							if (1766536703U != num)
							{
								goto Block_20;
							}
							continue;
							IL_3B3:
							num <<= 0;
							util_data.ExInstance exInstance = util_data.pInstances.GetValueOrDefault().ExInstance;
							num = 2017608781U << (int)num;
							IntPtr unk = intPtr5;
							MainWindow.<>c__DisplayClass25_0 CS$<>8__locals3 = CS$<>8__locals1;
							num += 1237613725U;
							num12 = new uint?(exInstance(unk, ref CS$<>8__locals3.pRequired));
							num += 2368687849U;
							goto IL_3FC;
							Block_14:
							num12 = num11;
							goto IL_3FC;
						}
						goto IL_00;
					}
					num = 1467028903U - num;
					if ((num & 460142068U) != 0U)
					{
						goto Block_17;
					}
					continue;
					IL_546:
					num |= 218029U;
					WinVer winType = versionHelper.GetWinType();
					WinVer winVer = (WinVer)(num ^ 252908U);
					num <<= 18;
					bool isWin = winType == winVer;
					num = 1376977743U + num;
					vars.IsWin7 = isWin;
					WinVer winVer2 = (WinVer)(num ^ 3251045194U);
					num = 783352188U - num;
					bool isWin10_20H = winType == winVer2;
					num = 73739764U / num;
					vars.IsWin10_20H = isWin10_20H;
					num /= 1373722522U;
					bool isWin2 = vars.IsWin7;
					num ^= 222253724U;
					if (isWin2)
					{
						break;
					}
					vars.vm_state = new Dictionary<eVm, byte>();
					Dictionary<eVm, byte> vm_state = vars.vm_state;
					num = 260665026U - num;
					eVm key2 = (eVm)(num ^ 38411302U);
					MainWindow.<>c__DisplayClass25_0 CS$<>8__locals4 = CS$<>8__locals1;
					num = 355756138U / num;
					byte slat = CS$<>8__locals4.pRequired.slat;
					num >>= 2;
					vm_state.Add(key2, slat);
					Dictionary<eVm, byte> vm_state2 = vars.vm_state;
					num = (1462307491U ^ num);
					eVm key3 = (eVm)(num ^ 1462307488U);
					MainWindow.<>c__DisplayClass25_0 CS$<>8__locals5 = CS$<>8__locals1;
					num &= 1744249833U;
					vm_state2.Add(key3, CS$<>8__locals5.pRequired.vtx);
					Dictionary<eVm, byte> vm_state3 = vars.vm_state;
					num = 191463410U * num;
					eVm key4 = (eVm)(num - 2626378546U);
					num *= 635970400U;
					bool g_isHardVtSupport;
					if (vm_state3.ContainsKey(key4))
					{
						Dictionary<eVm, byte> vm_state4 = vars.vm_state;
						eVm key5 = (eVm)(num ^ 2701789376U);
						num = 1977044159U * num;
						g_isHardVtSupport = ((uint)vm_state4[key5] == (num ^ 978683713U));
					}
					else
					{
						g_isHardVtSupport = (num - 2701789376U != 0U);
						num += 2571861632U;
					}
					vars.g_isHardVtSupport = g_isHardVtSupport;
					num ^= 2138323891U;
					if (1821538333U / num != 0U)
					{
						goto Block_23;
					}
				}
				do
				{
					IL_885:
					MainWindow.<>c__DisplayClass25_0 CS$<>8__locals6 = CS$<>8__locals1;
					num = 1989899368U;
					key = Marshal.PtrToStringAnsi(CS$<>8__locals6.pRequired.Userkey);
				}
				while (1214456844U >> (int)num == 0U);
				byte[] buffer3 = null;
				num >>= 3;
				szData.buffer = buffer3;
				GC.Collect();
				string text = key;
				object obj = null;
				num = (159341558U | num);
				return text > obj;
				Block_6:
				num %= 1153451040U;
				string message = "Image fault";
				num *= 1931637101U;
				throw new Exception(message);
				Block_8:
				num += 3423016800U;
				IL_230:
				string message2 = "ExInstance fault [2]";
				num = 937317107U << (int)num;
				throw new Exception(message2);
				Block_17:
				if (WinAPI.GetLastError() == (num ^ 1467028897U))
				{
					num /= 2050456806U;
					throw new Exception(CLanguage.GetTranslateText("srvconnectfault"));
				}
				num >>= 0;
				throw new Exception(string.Format("Initial initialization failed\r\nLastWin32Error : 0x{0:X8}", WinAPI.GetLastError()));
				Block_20:
				string message3 = "ExInstance fault : 777";
				num = 55798919U * num;
				throw new Exception(message3);
				Block_23:
				bool flag6 = vars.vm_state.ContainsKey((int)num + (eVm)(-1159838962));
				num -= 477178873U;
				bool g_isHardVtEnable;
				if (flag6)
				{
					Dictionary<eVm, byte> vm_state5 = vars.vm_state;
					num = 1937008031U * num;
					byte b2 = vm_state5[(eVm)(num ^ 3995381063U)];
					num = 2124954070U << (int)num;
					uint num14 = num - 2853074303U;
					num >>= 6;
					g_isHardVtEnable = (b2 == num14);
				}
				else
				{
					num = (825835115U ^ num);
					g_isHardVtEnable = ((num ^ 428453521U) != 0U);
					num ^= 455186247U;
				}
				vars.g_isHardVtEnable = g_isHardVtEnable;
				num |= 1534475166U;
				object @object = CS$<>8__locals1;
				num = 1828349144U >> (int)num;
				Action action = delegate()
				{
					vars.IsAmdProperty = (@object.pRequired.isAmd == 1);
				};
				num = 1975520166U / num;
				DispatchService.Dispatch(action);
				try
				{
					num += 419130063U;
					ObjectQuery query = new SelectQuery("Win32_processor");
					num = (1396258427U | num);
					ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher(query).Get().GetEnumerator();
					try
					{
						for (;;)
						{
							while (num != 1230061901U)
							{
								bool flag7 = enumerator.MoveNext();
								num |= 1232928802U;
								if (!flag7)
								{
									goto Block_30;
								}
								num = 534937592U;
								ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = enumerator;
								num = 1917258004U - num;
								object obj2 = ((ManagementObject)managementObjectEnumerator.Current)["VirtualizationFirmwareEnabled"];
								num = 2063996748U / num;
								obj2.ToString();
								num ^= 3753604734U;
							}
						}
						Block_30:;
					}
					finally
					{
						num = 705705397U;
						if (770140865U > num && enumerator != null)
						{
							((IDisposable)enumerator).Dispose();
							num ^= 0U;
						}
					}
				}
				catch (Exception ex)
				{
					num = 1214127945U;
					string format = "WMI_CPU : [LastWin32Error: 0x{0:X8}] {1}";
					num = 714759713U << (int)num;
					object arg = WinAPI.GetLastError();
					object message4 = ex.Message;
					num = (843404226U ^ num);
					util.CreateLog(string.Format(format, arg, message4));
				}
				goto IL_885;
			}
			catch (Exception)
			{
				uint num = 544431348U;
				if (num % 229054227U != 0U)
				{
					do
					{
						num = 1728972799U + num;
						string empty = string.Empty;
						num ^= 1145861785U;
						key = empty;
						num = (584145589U | num);
					}
					while (num < 1717834105U);
					byte[] buffer4 = null;
					num = 706504124U / num;
					szData.buffer = buffer4;
					if (num > 1240206891U)
					{
						goto IL_943;
					}
				}
				GC.Collect();
				IL_943:
				throw new Exception(CLanguage.GetTranslateText("retrieve_error"));
			}
			bool result;
			return result;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00C93124 File Offset: 0x00C93124
		private void getClientData(string SrvParam, string[] EnumDate, ref Dictionary<nf_server, Tuple<eKeyStates, string, string>> clientData)
		{
			uint num = 1435842723U;
			while (vars.IsActiveKey())
			{
				num = 599392299U * num;
				bool flag = EnumDate.Count<string>() != 0;
				num >>= 26;
				if (flag)
				{
					num |= 426594972U;
					clientData.Clear();
					num |= 1937197514U;
					IL_66:
					Dictionary<nf_server, string> serversCollection = vars.ServersCollection;
					num &= 1132814064U;
					Dictionary<nf_server, string>.Enumerator enumerator = serversCollection.GetEnumerator();
					try
					{
						for (;;)
						{
							IL_7C:
							for (;;)
							{
								num ^= 1863329850U;
								if (!enumerator.MoveNext())
								{
									goto Block_34;
								}
								KeyValuePair<nf_server, string> keyValuePair2;
								bool flag2;
								for (;;)
								{
									IL_81:
									KeyValuePair<nf_server, string> keyValuePair = enumerator.Current;
									num = 295178763U;
									keyValuePair2 = keyValuePair;
									for (;;)
									{
										IL_91:
										flag2 = ((num ^ 295178763U) != 0U);
										for (;;)
										{
											num |= 59913891U;
											num /= 1943093257U;
											int num2 = (int)(num - 0U);
											if (num > 719273754U)
											{
												goto IL_7C;
											}
											for (;;)
											{
												int num3 = num2;
												num <<= 15;
												int num4 = EnumDate.Length;
												num <<= 28;
												if (num3 >= num4)
												{
													break;
												}
												num = 1676955751U;
												int num5 = num2;
												num = 186936668U - num;
												string text = EnumDate[num5];
												num >>= 4;
												string text2 = text;
												num = 2129734926U >> (int)num;
												num = 693578248U * num;
												if (!SrvParam.Contains(keyValuePair2.Value))
												{
													goto IL_68A;
												}
												if (num <= 1905201148U)
												{
													goto IL_7C;
												}
												string text3 = text2;
												num %= 1840988281U;
												bool flag3 = text3.Contains(keyValuePair2.Value);
												num += 1840988281U;
												if (!flag3)
												{
													goto IL_68A;
												}
												num = 1777539463U << (int)num;
												Dictionary<nf_server, Tuple<eKeyStates, string, string>> dictionary = clientData;
												num = (1351706397U & num);
												num = 2120973344U % num;
												bool flag4 = dictionary.ContainsKey(keyValuePair2.Key);
												num ^= 2845409072U;
												if (flag4)
												{
													goto IL_68A;
												}
												num = (1698377613U & num);
												if (num << 0 == 0U)
												{
													goto IL_81;
												}
												string text4 = text2;
												char[] array = new char[num ^ 557384449U];
												num += 1595175992U;
												int num6 = (int)(num + 2142406856U);
												num /= 782448228U;
												char c = (char)(num ^ 35U);
												num = 1704545322U + num;
												array[num6] = c;
												string[] array2 = text4.Split(array);
												num &= 880759722U;
												string text5 = array2[(int)(num ^ 605634601U)];
												num /= 1769296302U;
												bool flag5 = text5.Equals("hold");
												num = 1372217484U + num;
												if (!flag5)
												{
													num += 1532064569U;
													string empty = string.Empty;
													num = 2012299840U - num;
													string arg = empty;
													num = (1248489437U | num);
													if (1635936835U % num == 0U)
													{
														goto IL_7C;
													}
													string text6 = text2;
													num %= 1805150309U;
													char[] array3 = new char[num ^ 1600587675U];
													num /= 351743427U;
													array3[(int)(num - 4U)] = (char)(num ^ 37U);
													string[] array4 = text6.Split(array3);
													num &= 1711154011U;
													string text7 = array4[(int)(num ^ 1U)];
													if ((num ^ 800402414U) == 0U)
													{
														goto IL_7C;
													}
													string text8 = text2;
													num = 1153780084U + num;
													char[] array5 = new char[num + 3141187213U];
													num -= 630075173U;
													int num7 = (int)(num ^ 523704911U);
													num |= 276369674U;
													array5[num7] = (char)(num - 528423726U);
													num /= 1949660909U;
													string[] array6 = text8.Split(array5);
													num = 781089156U >> (int)num;
													int num8 = (int)(num - 781089154U);
													num >>= 1;
													string text9 = array6[num8];
													num /= 274812489U;
													string text10 = text9;
													if (!(text7 == "D"))
													{
														num ^= 1581672456U;
														if (!(text7 == "H"))
														{
															num -= 203293799U;
															if (num / 848197504U == 0U)
															{
																goto IL_7C;
															}
															string a = text7;
															num &= 1520383398U;
															if (!(a == "I"))
															{
																if (num <= 948128937U)
																{
																	goto IL_7C;
																}
															}
															else
															{
																if (932216622U > num)
																{
																	goto IL_7C;
																}
																string value = text10;
																num = 782592377U / num;
																int n = Convert.ToInt32(value);
																num %= 878537272U;
																string name = "mn1";
																num = 145372622U >> (int)num;
																string translateText = CLanguage.GetTranslateText(name);
																num = (2014672667U & num);
																arg = util.plural(n, translateText, CLanguage.GetTranslateText("mn2"), CLanguage.GetTranslateText("mn3"));
																num += 1242033304U;
															}
														}
														else
														{
															num = (94849040U | num);
															if (2100173970U == num)
															{
																goto IL_7C;
															}
															string value2 = text10;
															num %= 1360668562U;
															int n2 = Convert.ToInt32(value2);
															string name2 = "hr1";
															num = 2103857136U << (int)num;
															string translateText2 = CLanguage.GetTranslateText(name2);
															num += 1486316364U;
															string translateText3 = CLanguage.GetTranslateText("hr2");
															string translateText4 = CLanguage.GetTranslateText("hr3");
															num ^= 529427870U;
															arg = util.plural(n2, translateText2, translateText3, translateText4);
															num ^= 1178146672U;
														}
													}
													else
													{
														num += 971732350U;
														if ((num & 1138771409U) == 0U)
														{
															goto IL_91;
														}
														string value3 = text10;
														num *= 458186351U;
														int n3 = Convert.ToInt32(value3);
														num = 1008486493U - num;
														string translateText5 = CLanguage.GetTranslateText("day1");
														num = (1882003895U | num);
														string name3 = "day2";
														num ^= 1506768907U;
														string translateText6 = CLanguage.GetTranslateText(name3);
														string name4 = "day3";
														num = 68376134U + num;
														string translateText7 = CLanguage.GetTranslateText(name4);
														num = (1875722660U ^ num);
														arg = util.plural(n3, translateText5, translateText6, translateText7);
														if (816393266U / num != 0U)
														{
															goto IL_7C;
														}
														num ^= 313621564U;
													}
													if (num <= 1264333458U)
													{
														goto IL_7C;
													}
													Dictionary<nf_server, Tuple<eKeyStates, string, string>> dictionary2 = clientData;
													num = 2014738581U / num;
													nf_server key = keyValuePair2.Key;
													eKeyStates item = (eKeyStates)(num - 1U);
													string item2;
													if (text10.Equals("0"))
													{
														num <<= 7;
														string name5 = "LightTm";
														num = 1125843974U / num;
														item2 = CLanguage.GetTranslateText(name5);
													}
													else
													{
														num |= 1158614800U;
														string format = "{0} {1}";
														object arg2 = text10;
														num = 1475837806U << (int)num;
														item2 = string.Format(format, arg2, arg);
														num += 27932168U;
													}
													num &= 995889826U;
													string empty2 = string.Empty;
													num = 915426850U >> (int)num;
													dictionary2.Add(key, new Tuple<eKeyStates, string, string>(item, item2, empty2));
													if (num >= 1882219147U)
													{
														goto IL_81;
													}
												}
												else
												{
													num &= 1333534526U;
													if (974201483U > num)
													{
														goto IL_7C;
													}
													num = (2025460041U ^ num);
													Dictionary<nf_server, Tuple<eKeyStates, string, string>> dictionary3 = clientData;
													num ^= 1252610265U;
													num >>= 12;
													nf_server key2 = keyValuePair2.Key;
													eKeyStates item3 = (eKeyStates)(num - 472499U);
													num = 1084832801U >> (int)num;
													string name6 = "cclicense";
													num ^= 2038576023U;
													string translateText8 = CLanguage.GetTranslateText(name6);
													string translateText9 = CLanguage.GetTranslateText("cclicense_selected");
													num = 659491724U - num;
													Tuple<eKeyStates, string, string> value4 = new Tuple<eKeyStates, string, string>(item3, translateText8, translateText9);
													num &= 1127421948U;
													dictionary3.Add(key2, value4);
													if (1156728467U % num == 0U)
													{
														goto IL_7C;
													}
													num ^= 932208078U;
												}
												IL_6B1:
												num >>= 26;
												int num9 = num2;
												num = 962350049U % num;
												int num10 = (int)(num ^ 10U);
												num -= 852577015U;
												num2 = num9 + num10;
												num += 852577004U;
												continue;
												IL_68A:
												num = (721098427U | num);
												bool flag6 = num - 2885621690U != 0U;
												num -= 1711631095U;
												flag2 = flag6;
												num += 4036403550U;
												goto IL_6B1;
											}
											num *= 1491696262U;
											if (num != 349510415U)
											{
												goto Block_30;
											}
										}
									}
								}
								Block_30:
								bool flag7 = flag2;
								num <<= 5;
								num += 1124424432U;
								if (flag7)
								{
									num = (1469320813U | num);
									Dictionary<nf_server, Tuple<eKeyStates, string, string>> dictionary4 = clientData;
									num = 1621387501U - num;
									nf_server key3 = keyValuePair2.Key;
									num = 878192285U - num;
									bool flag8 = dictionary4.ContainsKey(key3);
									num = 21039521U << (int)num;
									num ^= 1647409904U;
									if (!flag8)
									{
										if (num - 1019616265U != 0U)
										{
											Dictionary<nf_server, Tuple<eKeyStates, string, string>> dictionary5 = clientData;
											num &= 927163246U;
											nf_server key4 = keyValuePair2.Key;
											num = (1415327732U ^ num);
											eKeyStates item4 = (eKeyStates)(num ^ 1465740694U);
											num -= 651824907U;
											string translateText10 = CLanguage.GetTranslateText("nokeyactive");
											num /= 2105871539U;
											dictionary5.Add(key4, new Tuple<eKeyStates, string, string>(item4, translateText10, CLanguage.GetTranslateText("nokeyactive_selected")));
											num ^= 1124424432U;
										}
									}
								}
							}
						}
						Block_34:;
					}
					finally
					{
						num = 1086347747U;
						if (102902555U != num)
						{
							num = (469962572U | num);
							((IDisposable)enumerator).Dispose();
						}
					}
					return;
				}
				if (num + 1303928963U != 0U)
				{
					return;
				}
			}
			if (344332873U != num)
			{
				return;
			}
			goto IL_66;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00C93980 File Offset: 0x00C93980
		private void setClientDataInactiveState(ref Dictionary<nf_server, Tuple<eKeyStates, string, string>> clientData)
		{
			bool flag = vars.IsActiveKey();
			uint num = 49425595U;
			if (flag && 1405441862U > num)
			{
				return;
			}
			num = (1837903978U | num);
			clientData.Clear();
			num = 160177553U << (int)num;
			Dictionary<nf_server, string> serversCollection = vars.ServersCollection;
			num = 603291438U / num;
			Dictionary<nf_server, string>.Enumerator enumerator = serversCollection.GetEnumerator();
			num = 244196975U + num;
			Dictionary<nf_server, string>.Enumerator enumerator2 = enumerator;
			try
			{
				do
				{
					for (;;)
					{
						num = 1453921838U + num;
						if (!enumerator2.MoveNext())
						{
							break;
						}
						KeyValuePair<nf_server, string> keyValuePair = enumerator2.Current;
						num = 1373514092U;
						KeyValuePair<nf_server, string> keyValuePair2 = keyValuePair;
						num >>= 31;
						if (num != 842080948U)
						{
							Dictionary<nf_server, Tuple<eKeyStates, string, string>> dictionary = clientData;
							num = 360985653U * num;
							bool flag2 = dictionary.ContainsKey(keyValuePair2.Key);
							num ^= 244196975U;
							if (!flag2)
							{
								if ((1525949769U ^ num) != 0U)
								{
									Dictionary<nf_server, Tuple<eKeyStates, string, string>> dictionary2 = clientData;
									num -= 1325151559U;
									nf_server key = keyValuePair2.Key;
									eKeyStates item = (int)num + (eKeyStates)1080954586;
									num /= 151002008U;
									string translateText = CLanguage.GetTranslateText("nokeyactive");
									num *= 1722908048U;
									string name = "nokeyactive_selected";
									num = 446463612U * num;
									string translateText2 = CLanguage.GetTranslateText(name);
									num &= 565603072U;
									dictionary2.Add(key, new Tuple<eKeyStates, string, string>(item, translateText, translateText2));
									num ^= 255338095U;
								}
							}
						}
					}
				}
				while (1818967730U >> (int)num == 0U);
			}
			finally
			{
				do
				{
					num = 1350764046U;
					((IDisposable)enumerator2).Dispose();
				}
				while (num - 739646971U == 0U);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00C93AFC File Offset: 0x00C93AFC
		private bool getHashState(eHashesType type)
		{
			uint num = 1039401247U;
			if (2010215723U != num)
			{
				num = 417546533U >> (int)num;
				switch (type)
				{
				case eHashesType.hash_WFRU:
					break;
				case eHashesType.hash_WFEU:
				{
					eHashes type2 = (int)num + eHashes.hash_eu_game;
					num |= 962491078U;
					string val = "ghash_eu";
					num = 821901889U + num;
					string fromContent = this.selector(val, num + 2510574329U != 0U);
					num <<= 13;
					bool flag = util.IsHashValid(type2, fromContent);
					num ^= 1973477376U;
					if (!flag)
					{
						goto IL_2AA;
					}
					num %= 693243938U;
					bool flag2 = util.IsHashValid((int)num + eHashes.hash_eu_mrac, this.selector("mrac_hash_eu", num + 0U != 0U));
					num ^= 0U;
					if (!flag2)
					{
						goto IL_2AA;
					}
					eHashes type3 = (eHashes)(num ^ 5U);
					num += 2079149178U;
					string val2 = "mraci64_hash_eu";
					num = 869091538U / num;
					bool isOptionally = (num ^ 0U) != 0U;
					num %= 1175260411U;
					string fromContent2 = this.selector(val2, isOptionally);
					num = (955265713U & num);
					bool flag3 = util.IsHashValid(type3, fromContent2);
					num += 0U;
					if (flag3)
					{
						return (num ^ 1U) != 0U;
					}
					goto IL_2AA;
				}
				case eHashesType.hash_MW19:
				{
					eHashes type4 = (eHashes)(num ^ 6U);
					num /= 732435518U;
					num = (643121965U | num);
					string val3 = "ghash_mw19";
					num *= 1647067746U;
					bool isOptionally2 = num - 1840337210U != 0U;
					num = (458972116U | num);
					string fromContent3 = this.selector(val3, isOptionally2);
					num /= 590621198U;
					bool flag4 = util.IsHashValid(type4, fromContent3);
					num ^= 3U;
					if (flag4)
					{
						num = 450321230U << (int)num;
						return (num ^ 450321231U) != 0U;
					}
					goto IL_2AA;
				}
				case eHashesType.hash_APEX:
				{
					num &= 1540051709U;
					eHashes type5 = (eHashes)(num ^ 7U);
					num = 2017211132U >> (int)num;
					string val4 = "ghash_apex";
					num %= 1545960828U;
					string fromContent4 = this.selector(val4, (num ^ 471250304U) != 0U);
					num += 851983656U;
					bool flag5 = util.IsHashValid(type5, fromContent4);
					num += 2971733336U;
					if (flag5)
					{
						return num - uint.MaxValue != 0U;
					}
					goto IL_2AA;
				}
				default:
					if (1560031101U + num != 0U)
					{
						goto IL_41;
					}
					break;
				}
				eHashes type6 = (eHashes)(num ^ 0U);
				num ^= 358893055U;
				string fromContent5 = this.selector("ghash_ru", (num ^ 358893055U) != 0U);
				num *= 1137926902U;
				bool flag6 = util.IsHashValid(type6, fromContent5);
				num ^= 1599332618U;
				if (flag6)
				{
					eHashes type7 = (eHashes)(num - uint.MaxValue);
					num &= 1397247326U;
					string val5 = "mrac_hash_ru";
					num = (924473461U ^ num);
					bool flag7 = util.IsHashValid(type7, this.selector(val5, num - 924473461U != 0U));
					num ^= 924473461U;
					if (flag7)
					{
						num = 2131239695U + num;
						eHashes type8 = (eHashes)(num ^ 2131239693U);
						num = (117201233U ^ num);
						string val6 = "mraci64_hash_ru";
						num = (1178563166U | num);
						bool isOptionally3 = num + 2147517858U != 0U;
						num += 1469654229U;
						bool flag8 = util.IsHashValid(type8, this.selector(val6, isOptionally3));
						num &= 385684757U;
						num ^= 379065617U;
						if (flag8)
						{
							num &= 121250660U;
							return num - uint.MaxValue != 0U;
						}
					}
				}
			}
			IL_41:
			IL_2AA:
			return num + 0U != 0U;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00C93DBC File Offset: 0x00C93DBC
		private KeyValuePair<bool, string> getStatusService_neutral(nf_server pFeatures)
		{
			eStatusServiceType eStatusServiceType;
			uint num;
			for (;;)
			{
				IL_00:
				eStatusServiceType = eStatusServiceType.status_update;
				for (;;)
				{
					num = 375741264U;
					switch (pFeatures)
					{
					case nf_server.server_RU:
						break;
					case nf_server.server_RUi:
					{
						num = 1288928684U - num;
						string text = this.selector("status_rui", num - 913187420U != 0U);
						num = 1986820105U >> (int)num;
						string value = "1";
						num = 1321367670U * num;
						if (!text.Equals(value))
						{
							num >>= 7;
							eStatusServiceType eStatusServiceType2 = (eStatusServiceType)(num ^ 5153428U);
							num = 1786260907U - num;
							eStatusServiceType = eStatusServiceType2;
							num ^= 2085408837U;
							goto IL_6CF;
						}
						bool flag = num - 659639098U != 0U;
						num = 1616739507U / num;
						bool flag2 = flag;
						num = (809178891U | num);
						Dictionary<eHashesType, bool> hashCompareStates = vars.hashCompareStates;
						num = 923170484U / num;
						eHashesType key = (eHashesType)(num ^ 1U);
						num >>= 12;
						num = (1663658645U | num);
						hashCompareStates.TryGetValue(key, out flag2);
						num = (1687883933U | num);
						if (!flag2)
						{
							eStatusServiceType = (eStatusServiceType)(num ^ 1740339868U);
							num ^= 1910383053U;
							goto IL_6CF;
						}
						eStatusServiceType = (eStatusServiceType)(num ^ 1740339869U);
						if (10710823U != num)
						{
							num ^= 1910383053U;
							goto IL_6CF;
						}
						goto IL_00;
					}
					case nf_server.server_EU:
					{
						string val = "status_eu";
						bool isOptionally = num + 3919226032U != 0U;
						num += 1017644214U;
						string text2 = this.selector(val, isOptionally);
						string value2 = "1";
						num >>= 14;
						bool flag3 = text2.Equals(value2);
						num -= 2123651847U;
						if (!flag3)
						{
							eStatusServiceType eStatusServiceType3 = (eStatusServiceType)(num ^ 2171400492U);
							num = 2066577541U % num;
							eStatusServiceType = eStatusServiceType3;
							num ^= 1833444309U;
							goto IL_6CF;
						}
						bool flag4 = (num ^ 2171400494U) != 0U;
						if (num <= 213740019U)
						{
							goto IL_00;
						}
						Dictionary<eHashesType, bool> hashCompareStates2 = vars.hashCompareStates;
						num = 1964902638U % num;
						hashCompareStates2.TryGetValue((eHashesType)(num ^ 1964902639U), out flag4);
						num |= 604400416U;
						bool flag5 = flag4;
						num <<= 25;
						if (!flag5)
						{
							eStatusServiceType eStatusServiceType4 = (int)num + (eStatusServiceType)603979777;
							num <<= 4;
							eStatusServiceType = eStatusServiceType4;
							if ((num & 1898912609U) != 0U)
							{
								num ^= 3596966736U;
								goto IL_6CF;
							}
							goto IL_00;
						}
						else
						{
							num += 1307720329U;
							eStatusServiceType = (eStatusServiceType)(num - 703740553U);
							if (num < 1631460593U)
							{
								num ^= 1066885593U;
								goto IL_6CF;
							}
							continue;
						}
						break;
					}
					case nf_server.server_APEX:
					{
						if (2006324067U == num)
						{
							goto IL_00;
						}
						string val2 = "status_apex";
						bool isOptionally2 = (num ^ 375741264U) != 0U;
						num %= 1048776444U;
						bool flag6 = this.selector(val2, isOptionally2).Equals("1");
						num ^= 710161807U;
						if (!flag6)
						{
							num = 1670925794U >> (int)num;
							eStatusServiceType eStatusServiceType5 = (eStatusServiceType)(num ^ 2U);
							num ^= 1806634395U;
							eStatusServiceType = eStatusServiceType5;
							num += 2864074165U;
							goto IL_6CF;
						}
						bool flag7 = (num ^ 1009872607U) != 0U;
						if (2092572952U * num == 0U)
						{
							goto IL_00;
						}
						Dictionary<eHashesType, bool> hashCompareStates3 = vars.hashCompareStates;
						eHashesType key2 = (eHashesType)(num - 1009872604U);
						num = 182930414U * num;
						hashCompareStates3.TryGetValue(key2, out flag7);
						num = 1042173615U >> (int)num;
						bool flag8 = flag7;
						num |= 2068141116U;
						if (!flag8)
						{
							eStatusServiceType eStatusServiceType6 = (eStatusServiceType)(num ^ 2068144062U);
							num = 724381554U * num;
							eStatusServiceType = eStatusServiceType6;
							num += 2337908546U;
							goto IL_6CF;
						}
						eStatusServiceType = (int)num + (eStatusServiceType)(-2068144063);
						num ^= 1830814959U;
						goto IL_6CF;
					}
					case nf_server.server_AB:
					{
						if (705768019U >> (int)num == 0U)
						{
							goto IL_00;
						}
						num %= 2007769654U;
						string val3 = "status_ab";
						bool isOptionally3 = num - 375741264U != 0U;
						num = 259024031U << (int)num;
						string text3 = this.selector(val3, isOptionally3);
						num = 286991376U << (int)num;
						string value3 = "1";
						num = 1228290631U >> (int)num;
						if (!text3.Equals(value3))
						{
							eStatusServiceType = (eStatusServiceType)(num ^ 18740U);
							num += 375722522U;
							goto IL_6CF;
						}
						eStatusServiceType eStatusServiceType7 = (int)num + (eStatusServiceType)(-18742);
						num += 1984526167U;
						eStatusServiceType = eStatusServiceType7;
						if (num - 193208426U != 0U)
						{
							num += 2686163651U;
							goto IL_6CF;
						}
						goto IL_00;
					}
					case nf_server.server_CODMW19:
					{
						num ^= 1819104902U;
						string val4 = "status_mw19";
						bool isOptionally4 = (num ^ 2047348182U) != 0U;
						num ^= 894374843U;
						string text4 = this.selector(val4, isOptionally4);
						string value4 = "1";
						num <<= 7;
						if (text4.Equals(value4))
						{
							num = 1101100077U - num;
							bool flag9 = (num ^ 2651995565U) != 0U;
							num = 1414297925U - num;
							bool flag10 = flag9;
							num <<= 8;
							Dictionary<eHashesType, bool> hashCompareStates4 = vars.hashCompareStates;
							eHashesType key3 = (eHashesType)(num ^ 976984066U);
							num = 1702890877U % num;
							hashCompareStates4.TryGetValue(key3, out flag10);
							num = 824211671U + num;
							num = (242427795U & num);
							if (flag10)
							{
								num <<= 7;
								eStatusServiceType eStatusServiceType8 = (eStatusServiceType)(num - 806553600U);
								num = 1121479759U >> (int)num;
								eStatusServiceType = eStatusServiceType8;
								num += 3549228801U;
								goto IL_6CF;
							}
							num %= 1718119439U;
							eStatusServiceType = (int)num + (eStatusServiceType)(-207627791);
							if (num != 95971345U)
							{
								num += 168113472U;
								goto IL_6CF;
							}
							goto IL_A0;
						}
						else
						{
							num = (1582241386U & num);
							eStatusServiceType eStatusServiceType9 = (eStatusServiceType)(num - 34543102U);
							num += 141585003U;
							eStatusServiceType = eStatusServiceType9;
							if (2050254345U != num)
							{
								num += 199613157U;
								goto IL_6CF;
							}
							goto IL_3B;
						}
						break;
					}
					case nf_server.server_ABMW19:
					{
						if (num > 1064125305U)
						{
							goto IL_00;
						}
						string val5 = "status_abmw19";
						bool isOptionally5 = num - 375741264U != 0U;
						num = 114242833U / num;
						if (!this.selector(val5, isOptionally5).Equals("1"))
						{
							eStatusServiceType eStatusServiceType10 = (eStatusServiceType)(num ^ 2U);
							num *= 1971472585U;
							eStatusServiceType = eStatusServiceType10;
							num ^= 375741264U;
							goto IL_6CF;
						}
						eStatusServiceType = (eStatusServiceType)(num ^ 0U);
						if (num + 1132213228U != 0U)
						{
							num += 375741264U;
							goto IL_6CF;
						}
						break;
					}
					case nf_server.server_ABAPEX:
					{
						num |= 895353301U;
						if (num / 1594059645U != 0U)
						{
							goto IL_00;
						}
						num |= 2082221547U;
						string text5 = this.selector("status_abapex", num - 2139062271U != 0U);
						string value5 = "1";
						num -= 1494107822U;
						bool flag11 = text5.Equals(value5);
						num %= 610015971U;
						if (flag11)
						{
							eStatusServiceType eStatusServiceType11 = (eStatusServiceType)(num ^ 34938478U);
							num = 1169758476U >> (int)num;
							eStatusServiceType = eStatusServiceType11;
							if ((1576878239U ^ num) != 0U)
							{
								num ^= 375672244U;
								goto IL_6CF;
							}
							goto IL_00;
						}
						else
						{
							eStatusServiceType = (eStatusServiceType)(num ^ 34938476U);
							if (1585931534U + num != 0U)
							{
								num += 340802786U;
								goto IL_6CF;
							}
							continue;
						}
						break;
					}
					default:
						if (1268984944U / num != 0U)
						{
							goto IL_3B;
						}
						continue;
					}
					if (num == 908526887U)
					{
						continue;
					}
					num *= 1214189162U;
					string val6 = "status_ru";
					num = 270158997U + num;
					string text6 = this.selector(val6, (num ^ 1136688053U) != 0U);
					num <<= 31;
					string value6 = "1";
					num *= 822173586U;
					bool flag12;
					if (text6.Equals(value6))
					{
						flag12 = (num + 0U != 0U);
						if (num - 1696098068U != 0U)
						{
							goto IL_A0;
						}
						break;
					}
					else
					{
						num <<= 25;
						eStatusServiceType eStatusServiceType12 = (eStatusServiceType)(num ^ 2U);
						num = (1684880142U ^ num);
						eStatusServiceType = eStatusServiceType12;
						if (684734149U >= num)
						{
							break;
						}
						num ^= 1913151582U;
					}
					IL_6CF:
					if (1549534043U <= num)
					{
						break;
					}
					if (eStatusServiceType == eStatusServiceType.status_work)
					{
						goto Block_33;
					}
					num = 676736422U + num;
					uint num2 = (uint)eStatusServiceType;
					num = 1284641914U >> (int)num;
					if (num2 == num - 305U)
					{
						goto Block_34;
					}
					num <<= 14;
					if (num < 2057243697U)
					{
						goto Block_35;
					}
					continue;
					IL_A0:
					Dictionary<eHashesType, bool> hashCompareStates5 = vars.hashCompareStates;
					num += 1415726451U;
					hashCompareStates5.TryGetValue((eHashesType)(num ^ 1415726451U), out flag12);
					num = 47002699U / num;
					if (!flag12)
					{
						eStatusServiceType eStatusServiceType13 = (eStatusServiceType)(num ^ 1U);
						num *= 1364201057U;
						eStatusServiceType = eStatusServiceType13;
						if (num >= 767704395U)
						{
							break;
						}
						num += 375741264U;
					}
					else
					{
						num |= 1497523496U;
						eStatusServiceType = (eStatusServiceType)(num ^ 1497523496U);
						num ^= 1327971960U;
					}
					IL_3B:
					goto IL_6CF;
				}
			}
			Block_33:
			num &= 1686319055U;
			return new KeyValuePair<bool, string>(num + 4227790017U != 0U, "product_work_status");
			Block_34:
			bool key4 = (num ^ 306U) != 0U;
			num = 703666792U + num;
			return new KeyValuePair<bool, string>(key4, "product_hash_update_status");
			Block_35:
			uint num3 = (uint)eStatusServiceType;
			num = 860576061U >> (int)num;
			if (num3 == (num ^ 860576063U))
			{
				bool key5 = num - 860576061U != 0U;
				string value7 = "product_update_status";
				num = 388983699U + num;
				return new KeyValuePair<bool, string>(key5, value7);
			}
			num >>= 26;
			bool key6 = (num ^ 12U) != 0U;
			num = 338047137U - num;
			return new KeyValuePair<bool, string>(key6, "product_unk_status");
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00C9456C File Offset: 0x00C9456C
		private KeyValuePair<bool, string> checkServerPatchState(nf_server server)
		{
			object lock_states = vars.lock_states;
			uint num = 102376252U;
			object obj = lock_states;
			bool flag = (num ^ 102376252U) != 0U;
			num = (1066271319U ^ num);
			bool flag2 = flag;
			try
			{
				object obj2 = obj;
				num -= 1482689615U;
				num = 915625079U + num;
				Monitor.Enter(obj2, ref flag2);
				bool serversPatchState = vars.ServersPatchState != null;
				num = (1017203287U ^ num);
				if (serversPatchState)
				{
					do
					{
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState2 = vars.ServersPatchState;
						num |= 941114193U;
						KeyValuePair<bool, string> keyValuePair;
						serversPatchState2.TryGetValue(server, out keyValuePair);
						num = (1392728251U | num);
						KeyValuePair<bool, string> result = keyValuePair;
					}
					while (num < 534079921U);
					goto IL_CF;
				}
			}
			finally
			{
				bool flag3 = flag2;
				num = 1520330956U;
				if (flag3)
				{
					num = (442465840U | num);
					object obj3 = obj;
					num >>= 17;
					Monitor.Exit(obj3);
					num ^= 1520321955U;
				}
			}
			IL_BC:
			return new KeyValuePair<bool, string>(false, string.Empty);
			IL_CF:
			num = 1555760541U;
			if (num > 453129784U)
			{
				KeyValuePair<bool, string> result;
				return result;
			}
			goto IL_BC;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00C94678 File Offset: 0x00C94678
		private void reloadClientData(bool bFirst)
		{
			if (util_data.pInstances == null)
			{
				return;
			}
			uint num;
			object obj;
			try
			{
				string[] array;
				for (;;)
				{
					string text = "infsoft.dev/infapi/isource/Launcher/data.php";
					num = 278536865U;
					string text2 = text;
					num += 1998993164U;
					if (1586126936U <= num)
					{
						for (;;)
						{
							array = null;
							if ((2123172333U ^ num) == 0U)
							{
								break;
							}
							num /= 5191102U;
							bool flag = util_data.pInstances != null;
							num *= 256581041U;
							ulong? num2;
							ulong? num3;
							if (!flag)
							{
								num = 1235223482U << (int)num;
								num |= 773064204U;
								num = 233790285U << (int)num;
								num2 = null;
								num *= 58552088U;
								num3 = num2;
							}
							else
							{
								util_data.FnInitSrvData fnInitSrvData = util_data.pInstances.GetValueOrDefault().fnInitSrvData;
								num = 1023108213U % num;
								num %= 756549248U;
								string url = text2;
								num = 1784940868U + num;
								ulong value = fnInitSrvData(bFirst, url, out array);
								num = 1150381529U << (int)num;
								num3 = new ulong?(value);
								num += 3667398968U;
							}
							num %= 69080568U;
							ulong? num4 = num3;
							num -= 2116885824U;
							num2 = num4;
							long num5 = (long)(num + 2050283864U);
							num |= 1717517664U;
							ulong num6 = (ulong)num5;
							num = (1114310532U & num);
							ulong num7 = num6;
							num = 1088046789U >> (int)num;
							num += 1879907967U;
							ulong valueOrDefault = num2.GetValueOrDefault();
							num = 1745495328U / num;
							ulong num8 = num7;
							num = (1425622188U & num);
							bool flag2 = valueOrDefault == num8;
							num ^= 296381242U;
							bool flag3 = num2 != null;
							num += 2072276686U;
							if (flag2 && flag3)
							{
								num %= 1563715269U;
								bool flag4 = array != null;
								num |= 1715291211U;
								if (flag4)
								{
									goto IL_27B;
								}
								num ^= 3805392195U;
							}
							num >>= 28;
							bool init = (num ^ 9U) != 0U;
							num = 346443855U + num;
							vars.SetInit(init);
							num -= 363664530U;
							if (86383123U / num != 0U)
							{
								break;
							}
							string str = "Key : ";
							num = 2015380927U / num;
							util.CreateLog(str + vars.UKeyProperty);
							string format = "Failed to load data. LastWin32Error: {0}";
							num &= 886914758U;
							util.CreateLog(string.Format(format, num4));
							Action <>9__31_ = MainWindow.<>c.<>9__31_0;
							num = 19626109U - num;
							Action action = <>9__31_;
							num = 964783623U * num;
							if (<>9__31_ == null)
							{
								num = (2069185786U | num);
								if ((num ^ 1576747249U) == 0U)
								{
									break;
								}
								object <> = MainWindow.<>c.<>9;
								IntPtr method = ldftn(<reloadClientData>b__31_0);
								num |= 106255007U;
								Action <>9__31_2 = action = new Action(<>, method);
								num ^= 522654755U;
								MainWindow.<>c.<>9__31_0 = <>9__31_2;
								num += 3777150351U;
							}
							num /= 158228666U;
							DispatchService.Dispatch(action);
							num = 319973397U >> (int)num;
							if (1561199753U != num)
							{
								goto Block_13;
							}
						}
					}
				}
				Block_13:
				MainWindow.ExitApp();
				num += 1874013611U;
				IL_27B:
				obj = vars.locker;
				num = (594354303U | num);
				bool flag5 = (num ^ 1879013247U) != 0U;
				try
				{
					object obj2 = obj;
					num = 561473635U >> (int)num;
					Monitor.Enter(obj2, ref flag5);
					IEnumerable<string> source = array;
					Func<string, string[]> selector;
					if ((selector = MainWindow.<>c.<>9__31_2) == null)
					{
						num <<= 19;
						num = 1172194394U * num;
						object <>2 = MainWindow.<>c.<>9;
						num >>= 30;
						Func<string, string[]> func = (string v) => crypt.Decode64(v).Split(new char[]
						{
							'='
						});
						num /= 853505967U;
						selector = func;
						num = (1231558135U ^ num);
						MainWindow.<>c.<>9__31_2 = func;
						num += 3063409161U;
					}
					IEnumerable<string[]> source2 = source.Select(selector);
					num <<= 4;
					Func<string[], string> keySelector;
					if ((keySelector = MainWindow.<>c.<>9__31_3) == null)
					{
						object <>3 = MainWindow.<>c.<>9;
						num = 1373054427U * num;
						Func<string[], string> func2 = (string[] v) => v.First<string>();
						num = 1073898529U + num;
						keySelector = func2;
						MainWindow.<>c.<>9__31_3 = func2;
						num ^= 1073898529U;
					}
					Func<string[], string> elementSelector;
					bool flag6 = (elementSelector = MainWindow.<>c.<>9__31_4) != null;
					num = 200092509U * num;
					if (!flag6)
					{
						num = (642653998U & num);
						object <>4 = MainWindow.<>c.<>9;
						num /= 2100308371U;
						Func<string[], string> func3 = (string[] v) => v.Last<string>();
						num = 617491599U >> (int)num;
						elementSelector = func3;
						MainWindow.<>c.<>9__31_4 = func3;
						num ^= 617491599U;
					}
					num = (530863074U & num);
					Dictionary<string, string> responceMain = source2.ToDictionary(keySelector, elementSelector);
					num -= 1920425951U;
					vars.ResponceMain = responceMain;
				}
				finally
				{
					do
					{
						num = 733302360U;
						bool flag7 = flag5;
						num *= 1906204055U;
						if (flag7)
						{
							if (num <= 693993020U)
							{
								continue;
							}
							object obj3 = obj;
							num = 340416555U * num;
							Monitor.Exit(obj3);
							num += 1058715632U;
						}
					}
					while (944516493 << (int)num == 0);
				}
				for (;;)
				{
					num = 1554473732U;
					if (1553551447U * num == 0U)
					{
						goto IL_446;
					}
					bool responceMain2 = vars.ResponceMain != null;
					num <<= 29;
					if (responceMain2)
					{
						goto IL_41F;
					}
					goto IL_446;
					IL_513:
					if (num >> 7 != 0U)
					{
						break;
					}
					continue;
					IL_41F:
					IEnumerable<KeyValuePair<string, string>> responceMain3 = vars.ResponceMain;
					num &= 1561019592U;
					bool flag8 = responceMain3.Count<KeyValuePair<string, string>>() != 0;
					num = 932270317U + num;
					if (flag8)
					{
						goto IL_513;
					}
					num += 1215213331U;
					IL_446:
					num *= 1065703089U;
					vars.SetRestricted(num + 2147483649U != 0U);
					num = (840390916U ^ num);
					vars.SetInit(num + 1307092733U != 0U);
					string str2 = "Key : ";
					string ukeyProperty = vars.UKeyProperty;
					num ^= 652828291U;
					util.CreateLog(str2 + ukeyProperty);
					num <<= 16;
					util.CreateLog("Loaded data is corrupted. [#1]");
					num &= 2006200244U;
					Action <>9__31_3 = MainWindow.<>c.<>9__31_1;
					num = 517279014U * num;
					Action action2 = <>9__31_3;
					if (<>9__31_3 == null)
					{
						if (835415688U >= num)
						{
							goto IL_41F;
						}
						object <>5 = MainWindow.<>c.<>9;
						num %= 1617714965U;
						Action <>9__31_4 = action2 = delegate()
						{
							CustomMessageBox.Show(CLanguage.GetTranslateText("srvconnect"), CLanguage.GetTranslateText("error_srv_out"), MessageBoxButton.OK, true);
						};
						num >>= 24;
						MainWindow.<>c.<>9__31_1 = <>9__31_4;
						num += 3516399600U;
					}
					num |= 997740597U;
					DispatchService.Dispatch(action2);
					num <<= 8;
					MainWindow.ExitApp();
					num ^= 3485558253U;
					goto IL_513;
				}
			}
			catch (Exception ex)
			{
				for (;;)
				{
					num = 196877419U;
					bool init2 = (num ^ 196877418U) != 0U;
					num = 717953479U >> (int)num;
					vars.SetInit(init2);
					num |= 883976398U;
					if (788610019U < num)
					{
						string str3 = "Key : ";
						num <<= 14;
						string str4 = str3 + vars.UKeyProperty;
						num *= 1907510548U;
						util.CreateLog(str4);
						num = 992480802U >> (int)num;
						string format2 = "[HRESULT: 0x{0:X8}] {1}";
						num /= 723592619U;
						Exception ex2 = ex;
						num &= 1429569385U;
						int hresult = ex2.HResult;
						num >>= 10;
						object arg = hresult;
						num = 1602046990U + num;
						Exception ex3 = ex;
						num = 399856522U >> (int)num;
						string str5 = string.Format(format2, arg, ex3.Message);
						num = (1573199011U & num);
						util.CreateLog(str5);
						if (num != 9129023U)
						{
							util.CreateLog("Failed to load data [exception block]");
							Action action3;
							bool flag9 = (action3 = MainWindow.<>c.<>9__31_5) != null;
							num /= 520229195U;
							if (!flag9)
							{
								object <>6 = MainWindow.<>c.<>9;
								IntPtr method2 = ldftn(<reloadClientData>b__31_5);
								num = 1334130952U >> (int)num;
								action3 = (MainWindow.<>c.<>9__31_5 = new Action(<>6, method2));
								num ^= 1334130952U;
							}
							DispatchService.Dispatch(action3);
							num = 1533960862U * num;
							if ((num ^ 1022979113U) != 0U)
							{
								break;
							}
						}
					}
				}
				MainWindow.ExitApp();
			}
			do
			{
				num = 1852645756U;
				num = 678191426U >> (int)num;
				string text3 = this.selector("update", (num ^ 3U) != 0U);
				string value2 = "1";
				num ^= 1168723129U;
				bool flag10 = text3.Equals(value2);
				num = 1697514686U + num;
				if (!flag10)
				{
					goto IL_71E;
				}
				num = 1291076951U % num;
				vars.IsShowMainNotesProperty = (num + 3003890345U != 0U);
				num &= 780547337U;
				vars.is_update = ((num ^ 209978624U) != 0U);
				num += 1364662155U;
				bool init3 = (num ^ 1574640781U) != 0U;
				num &= 1718694323U;
				vars.SetInit(init3);
				num |= 1563255083U;
				bool flag11 = num - 1568502186U != 0U;
				num *= 1627199516U;
				MainWindow.bMainThEnd = flag11;
			}
			while ((164770767U ^ num) == 0U);
			do
			{
				this.downloadFile();
			}
			while (1128426444U == num);
			return;
			IL_71E:
			try
			{
				num = 1829394977U % num;
				if (num <= 1990490484U)
				{
					goto IL_732;
				}
				for (;;)
				{
					IL_757:
					bool hasInit = vars.HasInit;
					num |= 357908201U;
					if (!hasInit)
					{
						num = 81215886U * num;
						if (num > 1449865089U)
						{
							goto IL_77C;
						}
						goto IL_808;
					}
					IL_914:
					num /= 1197947135U;
					string val = "nwork";
					num = 255293641U >> (int)num;
					string text4 = this.selector(val, num + 4039673656U != 0U);
					string value3 = "1";
					num |= 103508191U;
					if (text4.Equals(value3))
					{
						vars.SetInit((num ^ 255817950U) != 0U);
						string str6 = "Key : " + vars.UKeyProperty;
						num |= 914772480U;
						util.CreateLog(str6);
						Action action4;
						if ((action4 = MainWindow.<>c.<>9__31_6) == null)
						{
							num = (646147437U & num);
							Action <>9__31_5 = action4 = delegate()
							{
								CustomMessageBox.Show(CLanguage.GetTranslateText("clauncher"), "Preventive works", MessageBoxButton.OK, true);
							};
							num <<= 15;
							MainWindow.<>c.<>9__31_6 = <>9__31_5;
							num ^= 2342124255U;
						}
						num = 1662734642U << (int)num;
						DispatchService.Dispatch(action4);
						MainWindow.ExitApp();
						num ^= 255817951U;
					}
					num ^= 19287431U;
					if (num < 79841566U)
					{
						goto IL_740;
					}
					num = (245506441U | num);
					string val2 = "banned";
					num = 1503811262U * num;
					bool isOptionally = (num ^ 1465572623U) != 0U;
					num = 725100307U << (int)num;
					if (this.selector(val2, isOptionally).Equals("1"))
					{
						bool init4 = (num ^ 163889153U) != 0U;
						num = (1256666285U & num);
						vars.SetInit(init4);
						num ^= 1769085171U;
						vars.SetRestricted(num + 2655643406U != 0U);
						num /= 314311871U;
						bool flag12 = (num ^ 4U) != 0U;
						num /= 1761020222U;
						MainWindow.bMainThEnd = flag12;
						num = 1593471899U >> (int)num;
						string str7 = "Key : ";
						num *= 2033126163U;
						string ukeyProperty2 = vars.UKeyProperty;
						num <<= 9;
						string str8 = str7 + ukeyProperty2;
						num <<= 3;
						util.CreateLog(str8);
						num = 73151245U / num;
						if (num >= 681320501U)
						{
							goto IL_732;
						}
						string str9 = "__restricted [banned]";
						num = 1817003104U >> (int)num;
						util.CreateLog(str9);
						num = 1133985688U % num;
						if (num - 795501005U == 0U)
						{
							goto IL_732;
						}
						Action action5;
						if ((action5 = MainWindow.<>c.<>9__31_7) == null)
						{
							num <<= 3;
							if (num << 26 != 0U)
							{
								goto IL_732;
							}
							object <>7 = MainWindow.<>c.<>9;
							num %= 326137764U;
							IntPtr method3 = ldftn(<reloadClientData>b__31_7);
							num %= 1513303934U;
							action5 = (MainWindow.<>c.<>9__31_7 = new Action(<>7, method3));
							num ^= 1256110724U;
						}
						DispatchService.Dispatch(action5);
						num = (714545718U | num);
						if (num - 327508900U == 0U)
						{
							goto IL_740;
						}
						MainWindow.ExitApp();
						num ^= 1649672126U;
					}
					num = 1708222031U + num;
					string text5 = this.selector("vbanned", (num ^ 1872111182U) != 0U);
					num = 1744004020U << (int)num;
					if (text5.Equals("1"))
					{
						vars.SetInit(num + 1311113217U != 0U);
						vars.SetRestricted(num - 2983854079U != 0U);
						num &= 917771626U;
						bool flag13 = num - 814743551U != 0U;
						num >>= 1;
						MainWindow.bMainThEnd = flag13;
						string str10 = "Key : ";
						num ^= 1305102967U;
						string ukeyProperty3 = vars.UKeyProperty;
						num -= 1614037647U;
						string str11 = str10 + ukeyProperty3 + Environment.NewLine;
						num = 2054241090U << (int)num;
						string str12 = str11 + Environment.NewLine + "__violation [spoofer detected]";
						num /= 166205761U;
						string newLine = Environment.NewLine;
						num *= 235875870U;
						util.CreateLog(str12 + newLine + "Disable the spoofer and try again");
						if (73484068U >= num)
						{
							goto IL_732;
						}
						Action action6;
						bool flag14 = (action6 = MainWindow.<>c.<>9__31_9) != null;
						num = 657145508U / num;
						if (!flag14)
						{
							num ^= 634810711U;
							if (1183782697U < num)
							{
								goto IL_732;
							}
							action6 = (MainWindow.<>c.<>9__31_9 = delegate()
							{
								CustomMessageBox.Show(CLanguage.GetTranslateText("err_spoofer"), CLanguage.GetTranslateText("err_header_msg"), MessageBoxButton.OK, true);
							});
							num += 3660156585U;
						}
						DispatchService.Dispatch(action6);
						num <<= 19;
						if (num * 1039991238U != 0U)
						{
							continue;
						}
						MainWindow.ExitApp();
						num += 2983854080U;
					}
					num = (798230530U ^ num);
					IEnumerable<KeyValuePair<string, string>> responceMain4 = vars.ResponceMain;
					num <<= 27;
					int num9 = responceMain4.Count<KeyValuePair<string, string>>();
					uint num10 = num ^ 268435457U;
					num = 1662583136U + num;
					if (num9 != num10)
					{
						goto IL_E5A;
					}
					if (1105464511U > num)
					{
						goto IL_7DD;
					}
					num += 1968923328U;
					bool flag15 = util_data.pInstances != null;
					num = 1727464238U - num;
					if (!flag15)
					{
						num >>= 14;
					}
					else
					{
						util_data.ExpLookUpSight expLookUpSight = util_data.pInstances.GetValueOrDefault().ExpLookUpSight;
						num %= 1788939001U;
						string lpParam = "Responce_count=1";
						num = 1670932459U * num;
						string lpNTSTATUS = null;
						num = 1532193669U * num;
						expLookUpSight(lpParam, lpNTSTATUS);
						num ^= 2389131753U;
					}
					num = (1839609724U | num);
					bool restricted = num - 1839726461U != 0U;
					num = 693044317U + num;
					vars.SetRestricted(restricted);
					num = 1112028675U << (int)num;
					vars.SetInit(num - 402653183U != 0U);
					bool flag16 = num + 3892314113U != 0U;
					num <<= 27;
					MainWindow.bMainThEnd = flag16;
					num = 189665448U * num;
					if (num + 106961630U != 0U)
					{
						string str13 = "Key : ";
						num += 1553793811U;
						util.CreateLog(str13 + vars.UKeyProperty);
						num ^= 1146302275U;
						if (num < 517018487U)
						{
							break;
						}
						goto IL_7DD;
					}
					IL_77C:
					num = 1260400208U % num;
					string text6 = this.selector("URLVK", (num ^ 1260400209U) != 0U);
					num -= 1737104383U;
					string text7 = text6;
					num *= 169435122U;
					string text8 = text7;
					num = (612921629U ^ num);
					string value4 = "0";
					num = (873009249U ^ num);
					bool flag17 = text8.Equals(value4);
					num ^= 3354312723U;
					if (flag17)
					{
						goto IL_914;
					}
					if ((639307706U ^ num) == 0U)
					{
						goto IL_740;
					}
					IL_7DD:
					int length = text7.Length;
					num -= 202059511U;
					uint num11 = num + 3960287228U;
					num >>= 8;
					num += 535432237U;
					if (length <= num11)
					{
						goto IL_914;
					}
					IL_808:
					string text9 = text7;
					int num12 = (int)(num + 3758227716U);
					num >>= 3;
					char[] array2 = new char[num12];
					num ^= 1210062009U;
					int num13 = (int)(num - 1272949606U);
					num %= 2059277639U;
					array2[num13] = (char)(num + 3022017814U);
					num = 253906856U * num;
					string[] array3 = text9.Split(array2, (StringSplitOptions)(num ^ 2386097393U));
					num = (1306609034U & num);
					string[] array4 = array3;
					num -= 1718245955U;
					int num14 = (int)(num ^ 2780162621U);
					num = 1269376352U << (int)num;
					int num15 = num14;
					while ((num ^ 1996645569U) != 0U)
					{
						if (num15 >= array4.Length)
						{
							num ^= 536739581U;
							goto IL_914;
						}
						string[] array5 = array4;
						num = 913526830U;
						int num16 = num15;
						num = 516838535U >> (int)num;
						string fileName = array5[num16];
						num = (1584802274U | num);
						ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
						num *= 405675391U;
						Process.Start(startInfo);
						num /= 1546918696U;
						num /= 219703208U;
						if (138487632U + num == 0U)
						{
							goto IL_732;
						}
						int num17 = num15;
						int num18 = (int)(num + 1U);
						num = 1953853388U << (int)num;
						int num19 = num17 + num18;
						num >>= 30;
						num15 = num19;
						num ^= 1U;
					}
					goto IL_740;
				}
				util.CreateLog("Loaded data is corrupted. [#2]");
				if ((420488729U & num) == 0U)
				{
					goto IL_732;
				}
				Action <>9__31_6 = MainWindow.<>c.<>9__31_8;
				num %= 1504196006U;
				Action action7 = <>9__31_6;
				num <<= 27;
				if (<>9__31_6 == null)
				{
					num %= 1524646689U;
					num += 1177441973U;
					object <>8 = MainWindow.<>c.<>9;
					IntPtr method4 = ldftn(<reloadClientData>b__31_8);
					num -= 1130065544U;
					Action action8 = new Action(<>8, method4);
					num %= 1800419556U;
					action7 = action8;
					MainWindow.<>c.<>9__31_8 = action8;
					num += 1477270260U;
				}
				num %= 922451041U;
				DispatchService.Dispatch(action7);
				num /= 1558001361U;
				MainWindow.ExitApp();
				num ^= 1931018592U;
				IL_E5A:
				MainWindow.<>c__DisplayClass31_0 CS$<>8__locals1;
				if (!vars.IsRestricted())
				{
					num = 1440697522U + num;
					string text10 = this.selector("IsActive", (num ^ 3371716114U) != 0U);
					num *= 157828649U;
					string value5 = "1";
					num -= 666512715U;
					bool flag5;
					if (text10.Equals(value5))
					{
						num ^= 822295287U;
						bool is_active = (num ^ 2008024417U) != 0U;
						num = 2111189281U << (int)num;
						vars.is_active = is_active;
						try
						{
							num = 1010828327U / num;
							string text12;
							if ((1701073703U ^ num) != 0U)
							{
								string variable = "USERPROFILE";
								num = 1170551313U + num;
								string environmentVariable = Environment.GetEnvironmentVariable(variable);
								num /= 1988577944U;
								string path = "saved games";
								num /= 2026049076U;
								string text11 = Path.Combine(environmentVariable, path, "My Games\\Warface");
								num = 1914638298U * num;
								text12 = text11;
								num = 731862015U - num;
								if (num % 757077919U == 0U)
								{
									goto IL_F68;
								}
							}
							bool flag18 = Directory.Exists(text12);
							num = 754283621U * num;
							if (!flag18)
							{
								goto IL_111B;
							}
							num ^= 199494036U;
							IL_F68:
							string path2 = text12;
							num %= 1873805537U;
							IEnumerable<string> enumerable = Directory.EnumerateDirectories(path2);
							num = 531564743U >> (int)num;
							IEnumerator<string> enumerator = enumerable.GetEnumerator();
							try
							{
								if (num < 1049318812U)
								{
									goto IL_FCD;
								}
								IL_F9C:
								IEnumerator<string> enumerator2 = enumerator;
								num = 1959926085U;
								string path3 = enumerator2.Current;
								num <<= 9;
								bool recursive = num + 1540191745U != 0U;
								num = (192873265U | num);
								Directory.Delete(path3, recursive);
								num ^= 2944398733U;
								IL_FCD:
								num ^= 365592228U;
								if (enumerator.MoveNext())
								{
									goto IL_F9C;
								}
							}
							finally
							{
								num = 290613538U;
								if (559559996U >= num)
								{
									bool flag19 = enumerator != null;
									num &= 1310065980U;
									if (flag19)
									{
										enumerator.Dispose();
										num += 0U;
									}
								}
							}
							num = 1827541122U;
							string path4 = text12;
							num /= 1890809329U;
							IEnumerable<string> source3 = Directory.EnumerateFiles(path4);
							Func<string, bool> <>9__31_7 = MainWindow.<>c.<>9__31_10;
							num = 2043240214U << (int)num;
							Func<string, bool> predicate = <>9__31_7;
							num = (615476865U & num);
							if (<>9__31_7 == null)
							{
								num |= 39538264U;
								Func<string, bool> func4 = (string x) => !x.Contains("game.cfg");
								num *= 1020033803U;
								predicate = func4;
								MainWindow.<>c.<>9__31_10 = func4;
								num += 878225464U;
							}
							num = 1980901546U - num;
							List<string>.Enumerator enumerator3 = source3.Where(predicate).ToList<string>().GetEnumerator();
							num = 1518552149U / num;
							List<string>.Enumerator enumerator4 = enumerator3;
							try
							{
								if (1081230817U != num)
								{
									goto IL_10CF;
								}
								IL_10AC:
								num = 1084061956U;
								string lpstr = enumerator4.Current;
								num <<= 12;
								util.ForceDeleteFile(lpstr);
								num ^= 3616555009U;
								IL_10CF:
								if (num - 512170928U == 0U)
								{
									goto IL_10AC;
								}
								num %= 813398291U;
								if (enumerator4.MoveNext())
								{
									goto IL_10AC;
								}
							}
							finally
							{
								do
								{
									num = 1764129184U;
									num = 1778856709U * num;
									((IDisposable)enumerator4).Dispose();
								}
								while (111703684U == num);
							}
							IL_111B:;
						}
						catch (Exception ex4)
						{
						}
						num = 337904949U;
						if (853347152 << (int)num != 0)
						{
							obj = vars.lock_data;
						}
						num *= 507924180U;
						bool flag20 = (num ^ 1239971300U) != 0U;
						num = (573457203U ^ num);
						flag5 = flag20;
						try
						{
							num = 806626443U >> (int)num;
							do
							{
								object obj4 = obj;
								num = (89810745U ^ num);
								Monitor.Enter(obj4, ref flag5);
								num += 648704956U;
								num = (178586650U & num);
								string val3 = "params";
								num |= 1682900544U;
								bool isOptionally2 = (num ^ 1817118288U) != 0U;
								num %= 1175272322U;
								string srvParam = this.selector(val3, isOptionally2);
								string val4 = "endtime";
								num %= 1214933775U;
								bool isOptionally3 = num - 641845966U != 0U;
								num = 1957448222U + num;
								string text13 = this.selector(val4, isOptionally3);
								num ^= 130950208U;
								int num20 = (int)(num + 1658834773U);
								num %= 1094655912U;
								char[] array6 = new char[num20];
								int num21 = (int)(num ^ 446820700U);
								num += 1313226223U;
								char c = (char)(num + 2534920497U);
								num >>= 15;
								array6[num21] = c;
								StringSplitOptions options = (StringSplitOptions)(num ^ 53713U);
								num -= 1589516947U;
								string[] enumDate = text13.Split(array6, options);
								num += 354553764U;
								this.getClientData(srvParam, enumDate, ref vars.clientMembershipData);
							}
							while (2144754404U >= num);
							goto IL_131B;
						}
						finally
						{
							bool flag21 = flag5;
							num = 1393436943U;
							if (flag21)
							{
								num *= 1799685176U;
								if (num >> 18 != 0U)
								{
									Monitor.Exit(obj);
									num += 2571497927U;
								}
							}
						}
						goto IL_1278;
					}
					goto IL_1278;
					string input;
					JavaScriptSerializer javaScriptSerializer2;
					for (;;)
					{
						IL_131B:
						num = 2002197998U;
						string empty = string.Empty;
						num = 1111521778U * num;
						input = empty;
						if (num > 693922232U)
						{
							if (vars.g_currentLanguage == eLanguage.eRus)
							{
								num = 743312496U * num;
								if (1586105591U == num)
								{
									continue;
								}
								string val5 = "main_Notes_ru";
								bool isOptionally4 = num - 3122380352U != 0U;
								num *= 486017529U;
								string text14 = this.selector(val5, isOptionally4);
								num -= 1579893510U;
								input = text14;
								if (400169869U > num)
								{
									continue;
								}
							}
							else
							{
								num -= 1989615957U;
								string text15 = this.selector("main_Notes_en", (num ^ 1583916967U) != 0U);
								num /= 1059487698U;
								input = text15;
								num += 4001433913U;
							}
						}
						IL_13B8:
						num = (454914247U | num);
						if (1573482361U != num)
						{
							JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
							num = 1956671990U - num;
							javaScriptSerializer2 = javaScriptSerializer;
							num /= 1418920901U;
							if ((num & 1816360594U) == 0U)
							{
								break;
							}
							continue;
						}
						goto IL_13B8;
					}
					CS$<>8__locals1.notes = javaScriptSerializer2.Deserialize<Notes>(input);
					try
					{
						if (881606046U == num)
						{
							goto IL_140F;
						}
						IL_1408:
						MainWindow.<>c__DisplayClass31_1 CS$<>8__locals2 = new MainWindow.<>c__DisplayClass31_1();
						IL_140F:
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						num %= 918054085U;
						do
						{
							CS$<>8__locals2.newsControl = null;
							num %= 195656710U;
						}
						while (num == 1753370399U);
						MainWindow.<>c__DisplayClass31_1 CS$<>8__locals3 = CS$<>8__locals2;
						num %= 243082305U;
						CS$<>8__locals3.aboutTextStyle = null;
						if (num > 1087376927U)
						{
							goto IL_1408;
						}
						MainWindow.<>c__DisplayClass31_1 CS$<>8__locals4 = CS$<>8__locals2;
						num = 1178165780U + num;
						Style noteContentStyle = null;
						num ^= 944910950U;
						CS$<>8__locals4.noteContentStyle = noteContentStyle;
						num = 1296511493U << (int)num;
						if (num == 1428431699U)
						{
							goto IL_1408;
						}
						MainWindow.<>c__DisplayClass31_1 CS$<>8__locals5 = CS$<>8__locals2;
						Style noteDateStyle = null;
						num = (1004754650U | num);
						CS$<>8__locals5.noteDateStyle = noteDateStyle;
						object @object = CS$<>8__locals2;
						num /= 291443736U;
						Action action9 = delegate()
						{
							@object.newsControl = (@object.CS$<>8__locals1.<>4__this.MainNewsControl.RegionContent as NewsBarControl);
							@object.newsControl.pinNotifyContent.Text = @object.CS$<>8__locals1.notes.mainInfo;
							@object.newsControl.pinNotifyContent.Visibility = Visibility.Visible;
							foreach (Grid element in ((IEnumerable<Grid>)(from x in @object.newsControl.newsMainCollection.Children.OfType<Grid>()
							where x.Name != "pinNotify"
							select x).ToList<Grid>()))
							{
								@object.newsControl.newsMainCollection.Children.Remove(element);
							}
							@object.aboutTextStyle = (@object.CS$<>8__locals1.<>4__this.FindResource("aboutText") as Style);
							@object.noteContentStyle = (@object.CS$<>8__locals1.<>4__this.FindResource("noteContentStyle") as Style);
							@object.noteDateStyle = (@object.CS$<>8__locals1.<>4__this.FindResource("noteDateStyle") as Style);
						};
						num = (2020620352U ^ num);
						DispatchService.Dispatch(action9);
						num -= 1153392652U;
						MainWindow.<>c__DisplayClass31_1 CS$<>8__locals6 = CS$<>8__locals2;
						num = 1684017172U << (int)num;
						MainWindow.<>c__DisplayClass31_0 CS$<>8__locals7 = CS$<>8__locals6.CS$<>8__locals1;
						num += 872949437U;
						List<CommonNote> commonNotes = CS$<>8__locals7.notes.commonNotes;
						num = (1205162128U | num);
						List<CommonNote>.Enumerator enumerator5 = commonNotes.GetEnumerator();
						num *= 790249956U;
						using (List<CommonNote>.Enumerator enumerator6 = enumerator5)
						{
							if (num != 1310946334U)
							{
								for (;;)
								{
									while ((num & 1782262767U) != 0U)
									{
										num = (1888886230U ^ num);
										if (!enumerator6.MoveNext())
										{
											goto IL_15A2;
										}
										num = 235688095U;
										if (num % 2308647U != 0U)
										{
											MainWindow.<>c__DisplayClass31_2 CS$<>8__locals8 = new MainWindow.<>c__DisplayClass31_2();
											num = 887046069U + num;
											MainWindow.<>c__DisplayClass31_1 CS$<>8__locals9 = CS$<>8__locals2;
											num &= 139156720U;
											CS$<>8__locals8.CS$<>8__locals2 = CS$<>8__locals9;
											num &= 1929522029U;
											num -= 513878229U;
											CommonNote item = enumerator6.Current;
											num += 1797411002U;
											CS$<>8__locals8.item = item;
											IntPtr method5 = ldftn(<reloadClientData>b__13);
											num *= 448283987U;
											DispatchService.Dispatch(new Action(CS$<>8__locals8, method5));
											num += 4188389461U;
										}
									}
								}
							}
							IL_15A2:;
						}
					}
					catch (Exception ex5)
					{
						do
						{
							util.CreateLog("Key : " + vars.UKeyProperty);
							num = 1947561168U;
							string format3 = "[HRESULT: 0x{0:X8}] {1}";
							object arg2 = ex5.HResult;
							num /= 631733391U;
							string str14 = string.Format(format3, arg2, ex5.Message);
							num %= 2079922257U;
							util.CreateLog(str14);
							if (num * 1912305239U == 0U)
							{
								goto IL_168A;
							}
							Action action10;
							bool flag22 = (action10 = MainWindow.<>c.<>9__31_14) != null;
							num = 571161069U >> (int)num;
							if (!flag22)
							{
								num = 372988610U >> (int)num;
								Action action11 = delegate()
								{
									CustomMessageBox.Show(CLanguage.GetTranslateText("srvconnect"), CLanguage.GetTranslateText("err_header_msg"), MessageBoxButton.OK, true);
								};
								num >>= 21;
								action10 = action11;
								MainWindow.<>c.<>9__31_14 = action11;
								num += 71395133U;
							}
							num <<= 21;
							DispatchService.Dispatch(action10);
						}
						while (334440436U == num);
						do
						{
							MainWindow.ExitApp();
						}
						while (665257684U >= num);
						IL_168A:;
					}
					num = 554107309U;
					obj = vars.lock_notes;
					bool flag23 = num - 554107309U != 0U;
					num <<= 14;
					flag5 = flag23;
					try
					{
						num += 1732205445U;
						if (num != 209410027U)
						{
							goto IL_16BF;
						}
						goto IL_178B;
						IL_16FD:
						while (vars.g_currentLanguage != eLanguage.eRus)
						{
							num = (1679180911U & num);
							Dictionary<nf_server, Notes> serverNotes = vars.serverNotes;
							nf_server key = (int)num + (nf_server)(-394253);
							JavaScriptSerializer javaScriptSerializer3 = javaScriptSerializer2;
							num = 1430611155U >> (int)num;
							num = 1103574598U >> (int)num;
							string val6 = "RU_Notes_en";
							num ^= 1372127503U;
							Notes value6 = javaScriptSerializer3.Deserialize<Notes>(this.selector(val6, (num ^ 1371617769U) != 0U));
							num >>= 31;
							serverNotes.Add(key, value6);
							Dictionary<nf_server, Notes> serverNotes2 = vars.serverNotes;
							nf_server key2 = (nf_server)(num ^ 1U);
							num /= 404060340U;
							JavaScriptSerializer javaScriptSerializer4 = javaScriptSerializer2;
							num += 1591940418U;
							string val7 = "RUIN_Notes_en";
							num = (1112355095U | num);
							serverNotes2.Add(key2, javaScriptSerializer4.Deserialize<Notes>(this.selector(val7, (num ^ 1592735063U) != 0U)));
							num = (1694516129U & num);
							Dictionary<nf_server, Notes> serverNotes3 = vars.serverNotes;
							num = 1471756088U % num;
							nf_server key3 = (nf_server)(num ^ 330905141U);
							JavaScriptSerializer javaScriptSerializer5 = javaScriptSerializer2;
							num /= 281285450U;
							string val8 = "EU_Notes_en";
							bool isOptionally5 = num + uint.MaxValue != 0U;
							num -= 434992870U;
							string input2 = this.selector(val8, isOptionally5);
							num |= 1722316545U;
							Notes value7 = javaScriptSerializer5.Deserialize<Notes>(input2);
							num |= 1514754543U;
							serverNotes3.Add(key3, value7);
							num = 383989748U - num;
							Dictionary<nf_server, Notes> serverNotes4 = vars.serverNotes;
							num &= 963606696U;
							nf_server key4 = (int)num + (nf_server)(-291975325);
							num <<= 26;
							JavaScriptSerializer javaScriptSerializer6 = javaScriptSerializer2;
							num -= 162424874U;
							num = 1313042254U >> (int)num;
							string val9 = "APEX_Notes_en";
							bool isOptionally6 = (num ^ 313U) != 0U;
							num = (237514439U & num);
							Notes value8 = javaScriptSerializer6.Deserialize<Notes>(this.selector(val9, isOptionally6));
							num |= 60390285U;
							serverNotes4.Add(key4, value8);
							if (num >= 822168509U)
							{
								goto IL_16BF;
							}
							Dictionary<nf_server, Notes> serverNotes5 = vars.serverNotes;
							num >>= 26;
							nf_server key5 = (nf_server)(num ^ 4U);
							num = (1562273725U & num);
							JavaScriptSerializer javaScriptSerializer7 = javaScriptSerializer2;
							num %= 2116435612U;
							string val10 = "AB_Notes_en";
							num = (1129465666U ^ num);
							string input3 = this.selector(val10, num - 1129465666U != 0U);
							num -= 1594370694U;
							serverNotes5.Add(key5, javaScriptSerializer7.Deserialize<Notes>(input3));
							if (num == 638547410U)
							{
								goto IL_1749;
							}
							Dictionary<nf_server, Notes> serverNotes6 = vars.serverNotes;
							nf_server key6 = (nf_server)(num ^ 3830062265U);
							num <<= 0;
							JavaScriptSerializer javaScriptSerializer8 = javaScriptSerializer2;
							num = 1229744386U - num;
							string val11 = "MW19_Notes_en";
							num ^= 225778071U;
							bool isOptionally7 = (num ^ 1752651217U) != 0U;
							num ^= 829833781U;
							string input4 = this.selector(val11, isOptionally7);
							num = (516118279U ^ num);
							Notes value9 = javaScriptSerializer8.Deserialize<Notes>(input4);
							num %= 524384985U;
							serverNotes6.Add(key6, value9);
							num = 2043226526U * num;
							if (num >= 1588294379U)
							{
								goto IL_16BF;
							}
							Dictionary<nf_server, Notes> serverNotes7 = vars.serverNotes;
							num = 1605139923U / num;
							nf_server key7 = (nf_server)(num ^ 15U);
							JavaScriptSerializer javaScriptSerializer9 = javaScriptSerializer2;
							num ^= 46359189U;
							serverNotes7.Add(key7, javaScriptSerializer9.Deserialize<Notes>(this.selector("ABMW_Notes_en", (num ^ 46359196U) != 0U)));
							if (num != 53113678U)
							{
								Dictionary<nf_server, Notes> serverNotes8 = vars.serverNotes;
								nf_server key8 = (nf_server)(num - 46359189U);
								num *= 289933962U;
								JavaScriptSerializer javaScriptSerializer10 = javaScriptSerializer2;
								num *= 1408983635U;
								string val12 = "ABAPEX_Notes_en";
								num %= 2140231196U;
								Notes value10 = javaScriptSerializer10.Deserialize<Notes>(this.selector(val12, (num ^ 197894572U) != 0U));
								num = 2119528135U - num;
								serverNotes8.Add(key8, value10);
								num ^= 1921633563U;
								goto IL_1C41;
							}
						}
						goto IL_1707;
						IL_16BF:
						object obj5 = obj;
						num >>= 14;
						Monitor.Enter(obj5, ref flag5);
						num = 508589318U >> (int)num;
						if (926956743U > num)
						{
							for (;;)
							{
								vars.serverNotes.Clear();
								if (num < 705641931U)
								{
									goto IL_16FD;
								}
							}
						}
						IL_1707:
						Dictionary<nf_server, Notes> serverNotes9 = vars.serverNotes;
						nf_server key9 = (int)num + (nf_server)(-496669);
						num += 1454463684U;
						JavaScriptSerializer javaScriptSerializer11 = javaScriptSerializer2;
						string val13 = "RU_Notes_ru";
						bool isOptionally8 = (num ^ 1454960353U) != 0U;
						num += 1754792538U;
						string input5 = this.selector(val13, isOptionally8);
						num %= 106066944U;
						serverNotes9.Add(key9, javaScriptSerializer11.Deserialize<Notes>(input5));
						IL_1749:
						Dictionary<nf_server, Notes> serverNotes10 = vars.serverNotes;
						num /= 368978237U;
						nf_server key10 = (int)num + nf_server.server_RUi;
						JavaScriptSerializer javaScriptSerializer12 = javaScriptSerializer2;
						num <<= 20;
						string val14 = "RUIN_Notes_ru";
						num += 745226635U;
						serverNotes10.Add(key10, javaScriptSerializer12.Deserialize<Notes>(this.selector(val14, num + 3549740661U != 0U)));
						IL_178B:
						Dictionary<nf_server, Notes> serverNotes11 = vars.serverNotes;
						num = 164831165U + num;
						nf_server key11 = (nf_server)(num - 910057798U);
						JavaScriptSerializer javaScriptSerializer13 = javaScriptSerializer2;
						num &= 384061735U;
						string val15 = "EU_Notes_ru";
						num = 2012434025U % num;
						bool isOptionally9 = (num ^ 155060585U) != 0U;
						num = 318140228U + num;
						Notes value11 = javaScriptSerializer13.Deserialize<Notes>(this.selector(val15, isOptionally9));
						num = 1215892059U % num;
						serverNotes11.Add(key11, value11);
						num = (58665520U ^ num);
						if (344205213U <= num)
						{
							goto IL_16FD;
						}
						Dictionary<nf_server, Notes> serverNotes12 = vars.serverNotes;
						nf_server key12 = (nf_server)(num ^ 326054706U);
						JavaScriptSerializer javaScriptSerializer14 = javaScriptSerializer2;
						num = (64831175U | num);
						serverNotes12.Add(key12, javaScriptSerializer14.Deserialize<Notes>(this.selector("APEX_Notes_ru", num + 3959472137U != 0U)));
						Dictionary<nf_server, Notes> serverNotes13 = vars.serverNotes;
						num ^= 691340278U;
						nf_server key13 = (int)num + (nf_server)(-986332157);
						JavaScriptSerializer javaScriptSerializer15 = javaScriptSerializer2;
						num ^= 1271608736U;
						string val16 = "AB_Notes_ru";
						bool isOptionally10 = num + 2399073887U != 0U;
						num ^= 1864439368U;
						serverNotes13.Add(key13, javaScriptSerializer15.Deserialize<Notes>(this.selector(val16, isOptionally10)));
						num += 961630115U;
						Dictionary<nf_server, Notes> serverNotes14 = vars.serverNotes;
						num = (1547921539U | num);
						nf_server key14 = (nf_server)(num - 1601400714U);
						num = 568465507U / num;
						JavaScriptSerializer javaScriptSerializer16 = javaScriptSerializer2;
						num += 114110437U;
						string val17 = "MW19_Notes_ru";
						num = 1120951499U - num;
						bool isOptionally11 = num + 3288126234U != 0U;
						num = 1654609843U / num;
						serverNotes14.Add(key14, javaScriptSerializer16.Deserialize<Notes>(this.selector(val17, isOptionally11)));
						Dictionary<nf_server, Notes> serverNotes15 = vars.serverNotes;
						nf_server key15 = (nf_server)(num ^ 7U);
						num = 410728953U + num;
						JavaScriptSerializer javaScriptSerializer17 = javaScriptSerializer2;
						num <<= 11;
						string val18 = "ABMW_Notes_ru";
						num = 19535866U + num;
						bool isOptionally12 = (num ^ 3673810938U) != 0U;
						num |= 1607171198U;
						string input6 = this.selector(val18, isOptionally12);
						num = 1454458964U * num;
						serverNotes15.Add(key15, javaScriptSerializer17.Deserialize<Notes>(input6));
						Dictionary<nf_server, Notes> serverNotes16 = vars.serverNotes;
						num = (1521645570U ^ num);
						nf_server key16 = (nf_server)(num ^ 3959363421U);
						num <<= 27;
						JavaScriptSerializer javaScriptSerializer18 = javaScriptSerializer2;
						num *= 2100633952U;
						string val19 = "ABAPEX_Notes_ru";
						bool isOptionally13 = (num ^ 0U) != 0U;
						num /= 520060793U;
						Notes value12 = javaScriptSerializer18.Deserialize<Notes>(this.selector(val19, isOptionally13));
						num = (655719706U & num);
						serverNotes16.Add(key16, value12);
						if (659952137U - num == 0U)
						{
							goto IL_16BF;
						}
						IL_1C41:;
					}
					finally
					{
						num = 361956007U;
						if (flag5)
						{
							object obj6 = obj;
							num = 1946313198U * num;
							Monitor.Exit(obj6);
							num += 2711045221U;
						}
					}
					object lock_states = vars.lock_states;
					num = 258149730U;
					obj = lock_states;
					bool flag24 = num - 258149730U != 0U;
					num = 1892815209U + num;
					flag5 = flag24;
					try
					{
						num *= 545486678U;
						if ((num ^ 102305302U) == 0U)
						{
							goto IL_1CEC;
						}
						IL_1C9B:
						Monitor.Enter(obj, ref flag5);
						IL_1CA3:
						bool serversPatchState = vars.ServersPatchState != null;
						num = 1651380577U % num;
						if (serversPatchState)
						{
							goto IL_1CCF;
						}
						num *= 1594166952U;
						IL_1CBD:
						vars.ServersPatchState = new Dictionary<nf_server, KeyValuePair<bool, string>>();
						num ^= 2440930505U;
						IL_1CCF:
						if (vars.hashCompareStates != null)
						{
							goto IL_1D06;
						}
						num = 885682267U % num;
						if (num >= 2094685961U)
						{
							goto IL_1C9B;
						}
						IL_1CEC:
						Dictionary<eHashesType, bool> hashCompareStates = new Dictionary<eHashesType, bool>();
						num /= 1654803499U;
						vars.hashCompareStates = hashCompareStates;
						num += 1651380577U;
						IL_1D06:
						Dictionary<eHashesType, bool> hashCompareStates2 = vars.hashCompareStates;
						num = 693185159U >> (int)num;
						hashCompareStates2.Clear();
						Dictionary<eHashesType, bool> hashCompareStates3 = vars.hashCompareStates;
						eHashesType key17 = (eHashesType)(num ^ 346592579U);
						num += 2064980025U;
						bool hashState = this.getHashState((eHashesType)(num ^ 2411572604U));
						num %= 79111992U;
						hashCompareStates3.Add(key17, hashState);
						num = 912934100U >> (int)num;
						Dictionary<eHashesType, bool> hashCompareStates4 = vars.hashCompareStates;
						num = 2108250703U >> (int)num;
						eHashesType key18 = (eHashesType)(num ^ 131765669U);
						num = 1219515254U + num;
						num = (27211883U & num);
						bool hashState2 = this.getHashState((eHashesType)(num ^ 9054219U));
						num = (777131824U | num);
						hashCompareStates4.Add(key18, hashState2);
						if (2002274776U == num)
						{
							goto IL_1C9B;
						}
						Dictionary<eHashesType, bool> hashCompareStates5 = vars.hashCompareStates;
						num += 1923624984U;
						eHashesType key19 = (int)num + (eHashesType)1585287345;
						num = 1722813573U / num;
						hashCompareStates5.Add(key19, this.getHashState((int)num + eHashesType.hash_APEX));
						Dictionary<eHashesType, bool> hashCompareStates6 = vars.hashCompareStates;
						num = 199369813U >> (int)num;
						eHashesType key20 = (eHashesType)(num - 199369811U);
						eHashesType type = (int)num + (eHashesType)(-199369811);
						num = 432540954U + num;
						bool hashState3 = this.getHashState(type);
						num *= 44780924U;
						hashCompareStates6.Add(key20, hashState3);
						num = (1231045196U ^ num);
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState2 = vars.ServersPatchState;
						num = 14820634U * num;
						serversPatchState2.Clear();
						if (1529506002U == num)
						{
							goto IL_1CA3;
						}
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState3 = vars.ServersPatchState;
						nf_server key21 = (int)num + (nf_server)(-1715597776);
						num = 452681250U - num;
						nf_server pFeatures = (int)num + (nf_server)1262916526;
						num = 441132055U % num;
						KeyValuePair<bool, string> statusService_neutral = this.getStatusService_neutral(pFeatures);
						num >>= 7;
						serversPatchState3.Add(key21, statusService_neutral);
						num += 1174815193U;
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState4 = vars.ServersPatchState;
						num = 1205554793U % num;
						nf_server key22 = (int)num + (nf_server)(-27293255);
						num &= 635591369U;
						num = 158952795U >> (int)num;
						nf_server pFeatures2 = (nf_server)(num ^ 620908U);
						num = 1489200696U - num;
						serversPatchState4.Add(key22, this.getStatusService_neutral(pFeatures2));
						num = 1003226149U % num;
						if (250707667U * num == 0U)
						{
							goto IL_1C9B;
						}
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState5 = vars.ServersPatchState;
						nf_server key23 = (nf_server)(num - 1003226147U);
						num = (1750990955U ^ num);
						serversPatchState5.Add(key23, this.getStatusService_neutral((nf_server)(num ^ 1402078284U)));
						if (num >= 1500057868U)
						{
							goto IL_1CA3;
						}
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState6 = vars.ServersPatchState;
						nf_server key24 = (int)num + (nf_server)(-1402078283);
						KeyValuePair<bool, string> statusService_neutral2 = this.getStatusService_neutral((nf_server)(num - 1402078283U));
						num *= 1772504047U;
						serversPatchState6.Add(key24, statusService_neutral2);
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState7 = vars.ServersPatchState;
						nf_server key25 = (nf_server)(num - 84307661U);
						num -= 552219570U;
						nf_server pFeatures3 = (nf_server)(num ^ 3827055397U);
						num = (22616528U | num);
						KeyValuePair<bool, string> statusService_neutral3 = this.getStatusService_neutral(pFeatures3);
						num = (2112692770U & num);
						serversPatchState7.Add(key25, statusService_neutral3);
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState8 = vars.ServersPatchState;
						nf_server key26 = (nf_server)(num ^ 1699553828U);
						num >>= 20;
						nf_server pFeatures4 = (int)num + (nf_server)(-1616);
						num = 382817836U + num;
						serversPatchState8.Add(key26, this.getStatusService_neutral(pFeatures4));
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState9 = vars.ServersPatchState;
						num <<= 26;
						nf_server key27 = (nf_server)(num ^ 6U);
						num = (462637167U ^ num);
						num &= 884370522U;
						nf_server pFeatures5 = (nf_server)(num ^ 278022220U);
						num = (1767975455U ^ num);
						serversPatchState9.Add(key27, this.getStatusService_neutral(pFeatures5));
						Dictionary<nf_server, KeyValuePair<bool, string>> serversPatchState10 = vars.ServersPatchState;
						num = 1652435648U >> (int)num;
						nf_server key28 = (nf_server)(num - 780U);
						num = 1581804476U >> (int)num;
						KeyValuePair<bool, string> statusService_neutral4 = this.getStatusService_neutral((nf_server)(num - 3010U));
						num &= 658712215U;
						serversPatchState10.Add(key28, statusService_neutral4);
						if (2079136419U == num)
						{
							goto IL_1CBD;
						}
					}
					finally
					{
						if (flag5)
						{
							num = 560143154U;
							object obj7 = obj;
							num <<= 18;
							Monitor.Exit(obj7);
						}
					}
					for (;;)
					{
						IL_2035:
						num = 410803482U;
						if (num / 1004889458U == 0U)
						{
							if (vars.IsActiveKey())
							{
								goto IL_2051;
							}
							goto IL_2779;
						}
						for (;;)
						{
							IL_208C:
							List<Tuple<nf_server, bool, string>> list_nfEvnts = vars.list_nfEvnts;
							nf_server item2 = (nf_server)(num ^ 27U);
							bool item3 = (num ^ 26U) != 0U;
							num = 1725722850U / num;
							bool flag25 = util_data.pInstances != null;
							num /= 380127471U;
							string item4;
							if (!flag25)
							{
								num = 1693676366U + num;
								item4 = null;
							}
							else
							{
								util_data.fnunk fnunk = util_data.pInstances.GetValueOrDefault().fnunk;
								string ukeyProperty4 = vars.UKeyProperty;
								num -= 2059037651U;
								int startIndex = (int)(num ^ 2235929642U);
								num = 1766662671U / num;
								item4 = fnunk(ukeyProperty4.Substring(startIndex) + "RU!", (nf_server)(num ^ 0U));
								num ^= 1693676366U;
							}
							Tuple<nf_server, bool, string> item5 = new Tuple<nf_server, bool, string>(item2, item3, item4);
							num = (279341593U & num);
							list_nfEvnts.Add(item5);
							if ((1837713618U ^ num) == 0U)
							{
								break;
							}
							List<Tuple<nf_server, bool, string>> list_nfEvnts2 = vars.list_nfEvnts;
							nf_server item6 = (nf_server)(num ^ 10641928U);
							bool item7 = num - 10641927U != 0U;
							num = 710030159U * num;
							num = 1150054911U - num;
							string item8;
							if (util_data.pInstances == null)
							{
								item8 = null;
							}
							else
							{
								ref util_data.Instances valueOrDefault2 = util_data.pInstances.GetValueOrDefault();
								num = 1755335430U - num;
								util_data.fnunk fnunk2 = valueOrDefault2.fnunk;
								string ukeyProperty5 = vars.UKeyProperty;
								num %= 461162U;
								string s = ukeyProperty5.Substring((int)(num ^ 451881U));
								num = 1221484662U % num;
								item8 = fnunk2(s, (int)num + (nf_server)(-34101));
								num += 39326802U;
							}
							num = 1885414009U + num;
							list_nfEvnts2.Add(new Tuple<nf_server, bool, string>(item6, item7, item8));
							num = 1437672123U + num;
							if ((num & 1059201751U) == 0U)
							{
								goto IL_2035;
							}
							List<Tuple<nf_server, bool, string>> list_nfEvnts3 = vars.list_nfEvnts;
							num = 1694189039U << (int)num;
							nf_server item9 = (nf_server)(num - 2013265919U);
							bool item10 = (num ^ 2013265921U) != 0U;
							num = (350370592U & num);
							bool flag26 = util_data.pInstances != null;
							num = 1157238894U + num;
							string item11;
							if (!flag26)
							{
								num = (473697891U ^ num);
								item11 = null;
							}
							else
							{
								num = 508180897U + num;
								ref util_data.Instances valueOrDefault3 = util_data.pInstances.GetValueOrDefault();
								num <<= 13;
								util_data.fnunk fnunk3 = valueOrDefault3.fnunk;
								num <<= 19;
								string ukeyProperty6 = vars.UKeyProperty;
								num = 364199227U >> (int)num;
								int startIndex2 = (int)(num ^ 364199228U);
								num = 1567825905U >> (int)num;
								string s2 = ukeyProperty6.Substring(startIndex2) + "RUi!";
								nf_server srv = (int)num + (nf_server)(-10);
								num ^= 1122442201U;
								item11 = fnunk3(s2, srv);
								num += 98501179U;
							}
							list_nfEvnts3.Add(new Tuple<nf_server, bool, string>(item9, item10, item11));
							num += 1730630626U;
							if (1666522180U > num)
							{
								break;
							}
							List<Tuple<nf_server, bool, string>> list_nfEvnts4 = vars.list_nfEvnts;
							nf_server item12 = (nf_server)(num ^ 2951573997U);
							num = 2073701075U << (int)num;
							bool item13 = (num ^ 359235585U) != 0U;
							num = 1691759370U >> (int)num;
							num = (1874998535U & num);
							string item14;
							if (util_data.pInstances == null)
							{
								num *= 1494893173U;
								num *= 1366499829U;
								item14 = null;
							}
							else
							{
								num = (1017343722U & num);
								ref util_data.Instances valueOrDefault4 = util_data.pInstances.GetValueOrDefault();
								num += 912080410U;
								util_data.fnunk fnunk4 = valueOrDefault4.fnunk;
								num >>= 7;
								string ukeyProperty7 = vars.UKeyProperty;
								num = (233913462U | num);
								int startIndex3 = (int)(num - 234209015U);
								num = 1651601585U / num;
								string s3 = ukeyProperty7.Substring(startIndex3) + "EU!";
								num = (1010913943U ^ num);
								item14 = fnunk4(s3, (nf_server)(num ^ 1010913938U));
								num ^= 1139954786U;
							}
							Tuple<nf_server, bool, string> item15 = new Tuple<nf_server, bool, string>(item12, item13, item14);
							num *= 825982462U;
							list_nfEvnts4.Add(item15);
							if (num < 143729343U)
							{
								goto IL_2035;
							}
							List<Tuple<nf_server, bool, string>> list_nfEvnts5 = vars.list_nfEvnts;
							num -= 792158820U;
							nf_server item16 = (nf_server)(num ^ 3667110842U);
							bool item17 = (num ^ 3667110841U) != 0U;
							num *= 1816030795U;
							string item18;
							if (util_data.pInstances == null)
							{
								num = (1584156834U | num);
								item18 = null;
							}
							else
							{
								num |= 1840267316U;
								ref util_data.Instances valueOrDefault5 = util_data.pInstances.GetValueOrDefault();
								num = 142502376U / num;
								util_data.fnunk fnunk5 = valueOrDefault5.fnunk;
								string ukeyProperty8 = vars.UKeyProperty;
								num /= 1863789668U;
								item18 = fnunk5(ukeyProperty8.Substring((int)(num - 4294967290U)), (nf_server)(num ^ 2U));
								num ^= 4294729450U;
							}
							num ^= 717627760U;
							list_nfEvnts5.Add(new Tuple<nf_server, bool, string>(item16, item17, item18));
							List<Tuple<nf_server, bool, string>> list_nfEvnts6 = vars.list_nfEvnts;
							nf_server item19 = (nf_server)(num - 3577380759U);
							bool item20 = num + 717586535U != 0U;
							num = 1100822543U * num;
							bool flag27 = util_data.pInstances != null;
							num = 939083472U >> (int)num;
							string item21;
							if (!flag27)
							{
								num = 787899759U >> (int)num;
								num = 1174086985U + num;
								item21 = null;
							}
							else
							{
								util_data.fnunk fnunk6 = util_data.pInstances.GetValueOrDefault().fnunk;
								string str15 = vars.UKeyProperty.Substring((int)(num + 4280294124U));
								string str16 = "APEX!";
								num *= 1214914873U;
								item21 = fnunk6(str15 + str16, (nf_server)(num ^ 2207284736U));
								num ^= 3328924493U;
							}
							Tuple<nf_server, bool, string> item22 = new Tuple<nf_server, bool, string>(item19, item20, item21);
							num |= 1061642246U;
							list_nfEvnts6.Add(item22);
							if (num < 1794391632U)
							{
								goto IL_2035;
							}
							List<Tuple<nf_server, bool, string>> list_nfEvnts7 = vars.list_nfEvnts;
							nf_server item23 = (nf_server)(num ^ 2147444043U);
							bool item24 = (num ^ 2147444047U) != 0U;
							num *= 820012817U;
							bool flag28 = util_data.pInstances != null;
							num ^= 173766657U;
							string item25;
							if (!flag28)
							{
								num = 1160392457U - num;
								num ^= 313939969U;
								item25 = null;
							}
							else
							{
								ref util_data.Instances valueOrDefault6 = util_data.pInstances.GetValueOrDefault();
								num = 924482080U / num;
								util_data.fnunk fnunk7 = valueOrDefault6.fnunk;
								num = (707409349U & num);
								string ukeyProperty9 = vars.UKeyProperty;
								num -= 1042552832U;
								int startIndex4 = (int)(num + 1042552839U);
								num ^= 558898807U;
								string s4 = ukeyProperty9.Substring(startIndex4);
								num = (237793847U & num);
								item25 = fnunk7(s4, (nf_server)(num ^ 553522U));
								num += 2804424868U;
							}
							list_nfEvnts7.Add(new Tuple<nf_server, bool, string>(item23, item24, item25));
							if (num == 1410222258U)
							{
								break;
							}
							List<Tuple<nf_server, bool, string>> list_nfEvnts8 = vars.list_nfEvnts;
							num = 1001218235U / num;
							nf_server item26 = (nf_server)(num - 4294967292U);
							num %= 674693145U;
							bool item27 = num - 0U != 0U;
							num -= 276758878U;
							string item28;
							if (util_data.pInstances == null)
							{
								num <<= 21;
								num += 1582503671U;
								item28 = null;
							}
							else
							{
								num = 1349205025U - num;
								ref util_data.Instances valueOrDefault7 = util_data.pInstances.GetValueOrDefault();
								num *= 1572418068U;
								util_data.fnunk fnunk8 = valueOrDefault7.fnunk;
								string ukeyProperty10 = vars.UKeyProperty;
								num = 1487285064U + num;
								int startIndex5 = (int)(num ^ 1052559155U);
								num ^= 772019947U;
								string str17 = ukeyProperty10.Substring(startIndex5);
								num += 1872329182U;
								string str18 = "AB!";
								num ^= 1680755782U;
								string s5 = str17 + str18;
								nf_server srv2 = (nf_server)(num - 3833333751U);
								num = 538057791U / num;
								item28 = fnunk8(s5, srv2);
								num ^= 848500471U;
							}
							num *= 767383261U;
							Tuple<nf_server, bool, string> item29 = new Tuple<nf_server, bool, string>(item26, item27, item28);
							num = 1384583081U * num;
							list_nfEvnts8.Add(item29);
							num = 227697769U << (int)num;
							if ((num ^ 1689872003U) != 0U)
							{
								goto Block_89;
							}
						}
						IL_2051:
						IEnumerable<Tuple<nf_server, bool, string>> list_nfEvnts9 = vars.list_nfEvnts;
						num &= 1135744652U;
						bool flag29 = list_nfEvnts9.Count<Tuple<nf_server, bool, string>>() != 0;
						num = 1523323164U >> (int)num;
						num += 404853001U;
						if (!flag29)
						{
							num = 1846045883U >> (int)num;
							goto IL_208C;
						}
						goto IL_2779;
					}
					Block_89:
					List<Tuple<nf_server, bool, string>> list_nfEvnts10 = vars.list_nfEvnts;
					num = 1159219122U >> (int)num;
					nf_server item30 = (nf_server)(num - 1159219115U);
					num &= 1272852573U;
					bool item31 = (num ^ 1092091920U) != 0U;
					num /= 1853429310U;
					num = 121597647U * num;
					num /= 1562130896U;
					string item32;
					if (util_data.pInstances == null)
					{
						num *= 971514847U;
						item32 = null;
					}
					else
					{
						num = (966197598U & num);
						util_data.fnunk fnunk9 = util_data.pInstances.GetValueOrDefault().fnunk;
						num = (1960856826U ^ num);
						string str19 = vars.UKeyProperty.Substring((int)(num + 2334110477U));
						num = (1143304846U & num);
						string s6 = str19 + "ABAPX!";
						num |= 1724532380U;
						nf_server srv3 = (nf_server)(num ^ 1726629529U);
						num = 763636273U / num;
						item32 = fnunk9(s6, srv3);
						num ^= 0U;
					}
					Tuple<nf_server, bool, string> item33 = new Tuple<nf_server, bool, string>(item30, item31, item32);
					num = (68372104U ^ num);
					list_nfEvnts10.Add(item33);
					num += 342431378U;
					IL_2779:
					return;
					IL_1278:
					vars.is_active = (num + 3108814953U != 0U);
					object lock_data = vars.lock_data;
					num &= 650342259U;
					obj = lock_data;
					flag5 = ((num ^ 109260563U) != 0U);
					try
					{
						do
						{
							Monitor.Enter(obj, ref flag5);
							if (934352712U < num)
							{
								break;
							}
							num <<= 21;
							this.setClientDataInactiveState(ref vars.clientMembershipData);
						}
						while (829235905U * num == 0U);
					}
					finally
					{
						do
						{
							num = 343045814U;
							bool flag30 = flag5;
							num = 1659794660U << (int)num;
							if (flag30)
							{
								if (num == 601185198U)
								{
									continue;
								}
								object obj8 = obj;
								num /= 612200406U;
								Monitor.Exit(obj8);
								num += 956301311U;
							}
						}
						while (1031098752U * num == 0U);
					}
					goto IL_131B;
				}
				num <<= 30;
				MainWindow.bMainThEnd = ((num ^ 1U) != 0U);
				if (num / 1613657466U == 0U)
				{
					MainWindow.ExitApp();
					return;
				}
				goto IL_740;
				IL_732:
				CS$<>8__locals1 = new MainWindow.<>c__DisplayClass31_0();
				num = 2047363923U - num;
				IL_740:
				MainWindow.<>c__DisplayClass31_0 CS$<>8__locals10 = CS$<>8__locals1;
				num = 1866869345U % num;
				num = 144916388U + num;
				CS$<>8__locals10.<>4__this = this;
				goto IL_757;
			}
			catch (Exception ex6)
			{
				num = 85198958U;
				Exception ex7 = ex6;
				for (;;)
				{
					num = (1668172741U ^ num);
					vars.SetInit((num ^ 1719288746U) != 0U);
					num = (685391301U & num);
					for (;;)
					{
						string str20 = "Key : ";
						num |= 335359574U;
						util.CreateLog(str20 + vars.UKeyProperty);
						num &= 869945031U;
						if ((num & 378275359U) == 0U)
						{
							break;
						}
						string format4 = "[HRESULT: 0x{0:X8}] {1}";
						Exception ex8 = ex7;
						num <<= 24;
						int hresult2 = ex8.HResult;
						num = 468650866U >> (int)num;
						object arg3 = hresult2;
						num >>= 7;
						object message = ex7.Message;
						num = (786970654U ^ num);
						util.CreateLog(string.Format(format4, arg3, message));
						num = (1136884995U | num);
						if (532106184U + num != 0U)
						{
							goto Block_150;
						}
					}
				}
				Block_150:
				Action <>9__31_8 = MainWindow.<>c.<>9__31_15;
				num = 1557205393U + num;
				Action action12 = <>9__31_8;
				if (<>9__31_8 == null)
				{
					num %= 17787845U;
					object <>9 = MainWindow.<>c.<>9;
					num = 2040288944U % num;
					Action <>9__31_9 = action12 = delegate()
					{
						CustomMessageBox.Show(CLanguage.GetTranslateText("srvconnect"), CLanguage.GetTranslateText("err_header_msg"), MessageBoxButton.OK, true);
					};
					num >>= 6;
					MainWindow.<>c.<>9__31_15 = <>9__31_9;
					num += 3434151559U;
				}
				DispatchService.Dispatch(action12);
				MainWindow.ExitApp();
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00C97030 File Offset: 0x00C97030
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			uint num = 1638008916U;
			if (323517330 << (int)num == 0)
			{
				goto IL_B8;
			}
			MainWindow.<>c__DisplayClass33_0 CS$<>8__locals1;
			for (;;)
			{
				IL_18:
				CS$<>8__locals1 = new MainWindow.<>c__DisplayClass33_0();
				if (64177143U != num)
				{
					MainWindow.<>c__DisplayClass33_0 CS$<>8__locals2 = CS$<>8__locals1;
					num %= 298339342U;
					CS$<>8__locals2.<>4__this = this;
					num /= 2072577335U;
					if (1273046163U >> (int)num != 0U)
					{
						num *= 1727813158U;
						WindowInteropHelper windowInteropHelper = new WindowInteropHelper(this);
						num -= 1666789327U;
						HwndSource hwndSource = HwndSource.FromHwnd(windowInteropHelper.Handle);
						IntPtr method = ldftn(ShowWindowRequestProc);
						num = 1252680444U / num;
						HwndSourceHook hook = new HwndSourceHook(this, method);
						num &= 1506372074U;
						hwndSource.AddHook(hook);
						if (1304107239U != num)
						{
							break;
						}
					}
				}
			}
			CS$<>8__locals1.doneEvent = new ManualResetEvent(num + 0U != 0U);
			num = (1853125511U | num);
			IL_B8:
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			object @object = CS$<>8__locals1;
			num &= 1473072773U;
			backgroundWorker.DoWork += delegate(object o, DoWorkEventArgs ea)
			{
				ThreadStart start2;
				Exception ex;
				if ((start2 = @object.<>9__3) == null)
				{
					start2 = (@object.<>9__3 = delegate()
					{
						string empty = string.Empty;
						try
						{
							@object.<>4__this.getInternalData(out empty);
						}
						catch (Exception ex2)
						{
							Exception ex3 = ex2;
							Exception ex = ex3;
							vars.SetInit(true);
							DispatchService.Dispatch(delegate
							{
								CustomMessageBox.Show(ex.Message, CLanguage.GetTranslateText("err_header_msg"), MessageBoxButton.OK, true);
							});
							MainWindow.ExitApp();
						}
						if (string.IsNullOrEmpty(empty) || empty == crypt.CHTS("756e6b6e6f776e"))
						{
							vars.SetInit(true);
							MainWindow.ExitApp();
						}
						vars.UKeyProperty = empty;
					});
				}
				Thread thread3 = new Thread(start2);
				thread3.Start();
				try
				{
					vars.GamesAndDllsPath = new Dictionary<eHashes, string>();
					string path = string.Empty;
					object value2 = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Warface", "InstallLocation", "");
					if (value2 != null)
					{
						path = Path.Combine((string)value2, "LogBackups");
						if (Directory.Exists(path))
						{
							Directory.Delete(path, true);
						}
						util.ForceDeleteFile(Path.Combine((string)value2, "Game.log"));
						vars.GamesAndDllsPath.Add(eHashes.hash_ru_game, (string)value2 + "Bin64Release\\Game.exe");
						vars.GamesAndDllsPath.Add(eHashes.hash_ru_mrac, (string)value2 + "Bin64Release\\mrac64.dll");
						vars.GamesAndDllsPath.Add(eHashes.hash_ru_mracinstall, (string)value2 + "Bin64Release\\mracinstall64.exe");
					}
					object value3 = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Warface My.Com", "InstallLocation", "");
					if (value3 != null)
					{
						path = Path.Combine((string)value3, "LogBackups");
						if (Directory.Exists(path))
						{
							Directory.Delete(path, true);
						}
						util.ForceDeleteFile(Path.Combine((string)value3, "Game.log"));
						vars.GamesAndDllsPath.Add(eHashes.hash_eu_game, (string)value3 + "Bin64Release\\Game.exe");
						vars.GamesAndDllsPath.Add(eHashes.hash_eu_mrac, (string)value3 + "Bin64Release\\mrac64.dll");
						vars.GamesAndDllsPath.Add(eHashes.hash_eu_mracinstall, (string)value3 + "Bin64Release\\mracinstall64.exe");
					}
					object value4 = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Respawn\\Apex", "Install Dir", "");
					if (value4 != null)
					{
						vars.GamesAndDllsPath.Add(eHashes.hash_apex_game, (string)value4 + "r5apex.exe");
					}
					object value5 = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Call of Duty Modern Warfare", "InstallLocation", "");
					if (value5 != null)
					{
						vars.GamesAndDllsPath.Add(eHashes.hash_mw19_game, (string)value5 + "\\ModernWarfare.exe");
					}
				}
				catch (Exception ex)
				{
					vars.SetInit(true);
					util.CreateLog(string.Format("#HASH_PATH [HRESULT: 0x{0:X8} - {1}] {2}", ex.HResult, ex.GetType().Name, ex.Message));
					DispatchService.Dispatch(delegate
					{
						CustomMessageBox.Show("Error : [reg path not found]", CLanguage.GetTranslateText("err_header_msg"), MessageBoxButton.OK, true);
					});
					MainWindow.ExitApp();
				}
				if (thread3 != null)
				{
					thread3.Join();
					thread3 = null;
				}
				@object.<>4__this.reloadClientData(true);
			};
			num %= 282466469U;
			object object2 = CS$<>8__locals1;
			num = (1414095251U & num);
			IntPtr method2 = ldftn(<Window_Loaded>b__1);
			num <<= 31;
			RunWorkerCompletedEventHandler value = new RunWorkerCompletedEventHandler(object2, method2);
			num = 1106597496U - num;
			backgroundWorker.RunWorkerCompleted += value;
			this.gridFooter.IsEnabled = ((num ^ 3254081144U) != 0U);
			num = 798192019U << (int)num;
			AdornedControl loadingAdorner = this.LoadingAdorner;
			num %= 1966416832U;
			loadingAdorner.IsAdornerVisible = ((num ^ 499833921U) != 0U);
			backgroundWorker.RunWorkerAsync();
			num %= 874382336U;
			object object3 = CS$<>8__locals1;
			num = 1604929904U * num;
			IntPtr method3 = ldftn(<Window_Loaded>b__2);
			num = 576003551U - num;
			Thread thread = new Thread(new ThreadStart(object3, method3));
			num = 2120897562U % num;
			thread.Start();
			num = 214638764U * num;
			ThreadStart start;
			if ((start = MainWindow.<>c.<>9__33_6) == null)
			{
				if (num <= 609944553U)
				{
					goto IL_18;
				}
				object <> = MainWindow.<>c.<>9;
				num = (582643043U | num);
				IntPtr method4 = ldftn(<Window_Loaded>b__33_6);
				num = (1762593306U ^ num);
				start = (MainWindow.<>c.<>9__33_6 = new ThreadStart(<>, method4));
				num ^= 1260736345U;
			}
			num = (1492220367U ^ num);
			Thread thread2 = new Thread(start);
			num += 1700414879U;
			thread2.Priority = (int)num + (ThreadPriority)444954235;
			thread2.Start();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000051EF File Offset: 0x000051EF
		public void ShowHelp()
		{
			this.PagesHost.ShowPage(this.HelpControl.Name, null);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005208 File Offset: 0x00005208
		public void ShowAbout()
		{
			this.PagesHost.ShowPage(this.AboutControl.Name, null);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00C97230 File Offset: 0x00C97230
		private void getProductInfo(nf_server selected_server, TextBlock textblock, CancellationToken cancelToken)
		{
			uint num = 1642486111U;
			MainWindow.<>c__DisplayClass37_0 CS$<>8__locals2;
			bool flag2;
			bool flag3;
			for (;;)
			{
				IL_07:
				MainWindow.<>c__DisplayClass37_0 CS$<>8__locals1 = new MainWindow.<>c__DisplayClass37_0();
				num /= 795040674U;
				CS$<>8__locals2 = CS$<>8__locals1;
				num = 1419651318U * num;
				if (2138574813U % num != 0U)
				{
					for (;;)
					{
						MainWindow.<>c__DisplayClass37_0 CS$<>8__locals3 = CS$<>8__locals2;
						num = 1955669223U % num;
						CS$<>8__locals3.<>4__this = this;
						num += 1246699157U;
						if (num >= 1977359284U)
						{
							goto IL_57;
						}
						IL_DB:
						num = (1824751304U ^ num);
						bool isHardVtEnableErrorProperty = num + 2733901054U != 0U;
						num = (1717985777U | num);
						vars.IsHardVtEnableErrorProperty = isHardVtEnableErrorProperty;
						num = 185356819U >> (int)num;
						vars.IsHardVtSupportErrorProperty = ((num ^ 353U) != 0U);
						num = (2059426458U ^ num);
						vars.IsSystemSupportErrorProperty = ((num ^ 2059426811U) != 0U);
						num >>= 10;
						vars.IsLaunchIndicatorVisible = ((num ^ 2011158U) != 0U);
						num = 1840731645U % num;
						if (329997374U >> (int)num != 0U)
						{
							Thread.CurrentThread.Priority = (ThreadPriority)(num ^ 522079U);
							num ^= 167598200U;
							bool flag = num - 167423779U != 0U;
							num = 9326814U + num;
							flag2 = flag;
							flag3 = (num - 176750593U != 0U);
							switch (selected_server)
							{
							case nf_server.server_RU:
								goto IL_1C8;
							case nf_server.server_RUi:
							{
								if (num >> 17 == 0U)
								{
									goto IL_07;
								}
								string ruin_product_url = Texts.ruin_product_url;
								num %= 284956241U;
								vars.ProductUriProperty = ruin_product_url;
								if (1795625852U == num)
								{
									continue;
								}
								if (!vars.IsWin7)
								{
									num *= 1249970996U;
									bool flag4 = num - 1364527923U != 0U;
									num = (119210188U & num);
									flag2 = flag4;
									num += 158859261U;
								}
								flag3 = (num + 4118216704U != 0U);
								if (num + 634259528U != 0U)
								{
									goto Block_7;
								}
								goto IL_75;
							}
							case nf_server.server_EU:
							{
								num = 349452545U / num;
								vars.ProductUriProperty = Texts.eu_product_url;
								num /= 1764898808U;
								bool flag5 = (num ^ 1U) != 0U;
								num = 348858415U >> (int)num;
								flag2 = flag5;
								num = 996098519U % num;
								bool flag6 = (num ^ 298381688U) != 0U;
								num /= 1639725317U;
								flag3 = flag6;
								if ((1791101155U ^ num) != 0U)
								{
									goto Block_8;
								}
								continue;
							}
							case nf_server.server_APEX:
								if (1323714131U <= num)
								{
									goto IL_07;
								}
								vars.ProductUriProperty = Texts.apex_product_url;
								num = 21258797U % num;
								if (num + 1573211091U != 0U)
								{
									goto Block_10;
								}
								goto IL_75;
							case nf_server.server_AB:
							{
								if ((208735849U & num) == 0U)
								{
									goto IL_07;
								}
								vars.ProductUriProperty = Texts.ab_product_url;
								num |= 1309153797U;
								bool isWin = vars.IsWin7;
								num += 3154109948U;
								if (!isWin)
								{
									if (num == 484059418U)
									{
										goto IL_57;
									}
									bool isWin10_20H = vars.IsWin10_20H;
									num %= 1398609607U;
									num ^= 0U;
									if (!isWin10_20H)
									{
										num = 1697266441U % num;
										bool flag7 = num - 106511103U != 0U;
										num = 1443958577U << (int)num;
										flag2 = flag7;
										if (92037470U % num != 0U)
										{
											goto Block_17;
										}
										continue;
									}
								}
								break;
							}
							case nf_server.server_CODMW19:
								goto IL_433;
							case nf_server.server_ABMW19:
								goto IL_4AE;
							case nf_server.server_ABAPEX:
							{
								num ^= 1751388649U;
								string abapex_product_url = Texts.abapex_product_url;
								num = 1930121748U + num;
								vars.ProductUriProperty = abapex_product_url;
								num = 2102737756U >> (int)num;
								if (num != 866665785U)
								{
									goto Block_24;
								}
								goto IL_AB;
							}
							}
							goto Block_2;
						}
						goto IL_57;
						IL_AB:
						string translateText = CLanguage.GetTranslateText("product_data_loading");
						num = 1489709502U + num;
						vars.ProductStatusProperty = translateText;
						bool isLaunchAccessProperty = num - 1506199118U != 0U;
						num = 1674658067U * num;
						vars.IsLaunchAccessProperty = isLaunchAccessProperty;
						goto IL_DB;
						IL_75:
						string name = "product_data_loading";
						num = 1226270187U << (int)num;
						string translateText2 = CLanguage.GetTranslateText(name);
						num %= 786051512U;
						vars.ProductDateProperty = translateText2;
						if (249442085U * num != 0U)
						{
							goto IL_AB;
						}
						goto IL_07;
						IL_57:
						CS$<>8__locals2.textblock = textblock;
						num *= 1166363307U;
						if (1909150968U < num)
						{
							goto IL_75;
						}
						goto IL_07;
					}
					IL_1C8:
					num = (1364483615U & num);
					string ru_product_url = Texts.ru_product_url;
					num >>= 25;
					vars.ProductUriProperty = ru_product_url;
					flag2 = (num - uint.MaxValue != 0U);
					flag3 = ((num ^ 1U) != 0U);
					if (414270369U != num)
					{
						goto Block_3;
					}
					continue;
					IL_433:
					vars.ProductUriProperty = Texts.mw19_product_url;
					num = (1008951238U | num);
					if (1170421650 << (int)num != 0)
					{
						goto Block_18;
					}
					continue;
					IL_4AE:
					string mwab_product_url = Texts.mwab_product_url;
					num = 273963978U % num;
					vars.ProductUriProperty = mwab_product_url;
					if (vars.IsWin7)
					{
						goto IL_567;
					}
					num = 1426589327U - num;
					if (!vars.IsWin10_20H)
					{
						goto Block_21;
					}
					num /= 990841673U;
					if (343936100U >= num)
					{
						goto Block_22;
					}
				}
			}
			Block_2:
			goto IL_623;
			Block_3:
			num ^= 176750593U;
			goto IL_623;
			Block_7:
			num ^= 0U;
			goto IL_623;
			Block_8:
			num ^= 176750593U;
			goto IL_623;
			Block_10:
			if (!vars.IsWin7)
			{
				flag2 = (num + 4273708500U != 0U);
				num ^= 0U;
			}
			bool isAmdProperty = vars.IsAmdProperty;
			num = 2111774803U * num;
			num += 2531560298U;
			if (!isAmdProperty)
			{
				bool flag8 = (num ^ 176750592U) != 0U;
				num &= 1873767496U;
				flag3 = flag8;
				num ^= 1U;
				goto IL_623;
			}
			goto IL_623;
			Block_17:
			num ^= 1553469232U;
			goto IL_623;
			Block_18:
			bool isWin2 = vars.IsWin7;
			num |= 1396263015U;
			if (!isWin2)
			{
				flag2 = ((num ^ 2142986214U) != 0U);
				num ^= 0U;
			}
			num %= 276904422U;
			bool flag9 = (num ^ 204655260U) != 0U;
			num = (42629076U & num);
			flag3 = flag9;
			num += 176602477U;
			goto IL_623;
			Block_21:
			num = (1333748688U ^ num);
			bool flag10 = (num ^ 4442391U) != 0U;
			goto IL_53A;
			Block_22:
			bool isAmdProperty2 = vars.IsAmdProperty;
			num -= 531379525U;
			uint num2 = num ^ 3763587772U;
			num ^= 321223614U;
			flag10 = (isAmdProperty2 == num2);
			num ^= 4080367636U;
			IL_53A:
			num += 92770995U;
			if (flag10)
			{
				num /= 1651452113U;
				flag2 = ((num ^ 1U) != 0U);
				num += 97213385U;
			}
			IL_567:
			num = 139943899U << (int)num;
			flag3 = ((num ^ 2931799553U) != 0U);
			num += 1539918337U;
			goto IL_623;
			Block_24:
			bool isWin3 = vars.IsWin7;
			num |= 351482707U;
			num ^= 511325014U;
			if (!isWin3)
			{
				bool isWin10_20H2 = vars.IsWin10_20H;
				num = (857035012U ^ num);
				num ^= 857035012U;
				if (!isWin10_20H2)
				{
					num += 1121326012U;
					flag2 = (num - 1298076604U != 0U);
					num += 3173641284U;
				}
			}
			IL_623:
			bool isWin4 = vars.IsWin7;
			num = 1419074626U % num;
			if (isWin4)
			{
				num = 1513954385U << (int)num;
				bool flag11 = num - 1140850688U != 0U;
				num *= 1059087892U;
				flag3 = flag11;
				num += 2957859898U;
			}
			try
			{
				num |= 285373636U;
				if (num == 1913547381U)
				{
					goto IL_698;
				}
				IL_67B:
				MainWindow.<>c__DisplayClass37_1 CS$<>8__locals4 = new MainWindow.<>c__DisplayClass37_1();
				num >>= 2;
				if ((num & 1116369889U) == 0U)
				{
					goto IL_6CA;
				}
				IL_698:
				MainWindow.<>c__DisplayClass37_1 CS$<>8__locals5 = CS$<>8__locals4;
				MainWindow.<>c__DisplayClass37_0 CS$<>8__locals6 = CS$<>8__locals2;
				num = 818547409U * num;
				CS$<>8__locals5.CS$<>8__locals1 = CS$<>8__locals6;
				bool flag12 = (num ^ 3337074798U) != 0U;
				num %= 547713787U;
				bool flag13 = flag12;
				if (num >= 786776397U)
				{
					goto IL_67B;
				}
				IL_6CA:
				CS$<>8__locals4.notes = null;
				if (85943070U * num == 0U)
				{
					goto IL_698;
				}
				object obj = vars.lock_notes;
				num *= 1052271144U;
				bool flag14 = num - 83034120U != 0U;
				try
				{
					num /= 1174479984U;
					if ((1221818464U & num) == 0U)
					{
						object obj2 = obj;
						num <<= 19;
						num %= 1431186626U;
						Monitor.Enter(obj2, ref flag14);
						num -= 825967053U;
						if (num >= 1948532660U)
						{
							Dictionary<nf_server, Notes> serverNotes = vars.serverNotes;
							MainWindow.<>c__DisplayClass37_1 CS$<>8__locals7 = CS$<>8__locals4;
							num += 1899504511U;
							flag13 = serverNotes.TryGetValue(selected_server, out CS$<>8__locals7.notes);
						}
					}
				}
				finally
				{
					do
					{
						num = 1878023985U;
						if (flag14)
						{
							num *= 612043788U;
							object obj3 = obj;
							num = (1749631199U | num);
							Monitor.Exit(obj3);
							num += 60988498U;
						}
					}
					while (25042696U > num);
				}
				Tuple<eKeyStates, string, string> tuple;
				do
				{
					IL_7A2:
					num = 1344347676U;
					if (1084848842U >= num || flag13)
					{
						for (;;)
						{
							MainWindow.<>c__DisplayClass37_1 CS$<>8__locals8 = CS$<>8__locals4;
							num += 1939489876U;
							if (CS$<>8__locals8.notes == null)
							{
								break;
							}
							while (1637434218U != num)
							{
								tuple = null;
								num ^= 1803513836U;
								MainWindow.<>c__DisplayClass37_1 CS$<>8__locals9 = CS$<>8__locals4;
								num += 1160711135U;
								CS$<>8__locals9.productControl = null;
								num = (1481004936U ^ num);
								if (num / 446979794U == 0U)
								{
									goto IL_7DB;
								}
								CS$<>8__locals4.aboutTextStyle = null;
								if (num << 27 != 0U)
								{
									MainWindow.<>c__DisplayClass37_1 CS$<>8__locals10 = CS$<>8__locals4;
									Style noteContentStyle = null;
									num &= 245451178U;
									CS$<>8__locals10.noteContentStyle = noteContentStyle;
									if (1506946967U * num == 0U)
									{
										goto IL_7A2;
									}
									MainWindow.<>c__DisplayClass37_1 CS$<>8__locals11 = CS$<>8__locals4;
									Style noteDateStyle = null;
									num |= 950341731U;
									CS$<>8__locals11.noteDateStyle = noteDateStyle;
									num ^= 1240621813U;
									if (num + 586749276U == 0U)
									{
										goto IL_7A2;
									}
									Action action = delegate()
									{
										CS$<>8__locals4.productControl = (CS$<>8__locals4.CS$<>8__locals1.<>4__this.ProductControl.RegionContent as ProductUserControl);
										CS$<>8__locals4.productControl.pinNotifyContent.Text = CS$<>8__locals4.notes.mainInfo;
										CS$<>8__locals4.productControl.pinNotifyContent.Visibility = Visibility.Visible;
										foreach (Grid element in ((IEnumerable<Grid>)(from x in CS$<>8__locals4.productControl.newsCollection.Children.OfType<Grid>()
										where x.Name != "pinNotify"
										select x).ToList<Grid>()))
										{
											CS$<>8__locals4.productControl.newsCollection.Children.Remove(element);
										}
										CS$<>8__locals4.aboutTextStyle = (CS$<>8__locals4.CS$<>8__locals1.<>4__this.FindResource("aboutText") as Style);
										CS$<>8__locals4.noteContentStyle = (CS$<>8__locals4.CS$<>8__locals1.<>4__this.FindResource("noteContentStyle") as Style);
										CS$<>8__locals4.noteDateStyle = (CS$<>8__locals4.CS$<>8__locals1.<>4__this.FindResource("noteDateStyle") as Style);
									};
									num = (1187728608U & num);
									DispatchService.Dispatch(action);
									num |= 1075802971U;
									if (1503159125U != num)
									{
										goto Block_40;
									}
									break;
								}
							}
						}
						num += 2355477420U;
					}
					IL_7DB:
					num = 1112808971U - num;
				}
				while (num % 451948840U == 0U);
				throw new Exception("notes is null");
				Block_40:
				Notes notes = CS$<>8__locals4.notes;
				num = (1878665624U & num);
				List<CommonNote>.Enumerator enumerator = notes.commonNotes.GetEnumerator();
				num = 865287142U >> (int)num;
				List<CommonNote>.Enumerator enumerator2 = enumerator;
				try
				{
					do
					{
						for (;;)
						{
							num |= 590635176U;
							if (!enumerator2.MoveNext())
							{
								break;
							}
							num = 1134917755U;
							if (579492871U / num == 0U)
							{
								MainWindow.<>c__DisplayClass37_2 CS$<>8__locals12 = new MainWindow.<>c__DisplayClass37_2();
								num %= 1282506377U;
								CS$<>8__locals12.CS$<>8__locals2 = CS$<>8__locals4;
								num = (1617830182U ^ num);
								CS$<>8__locals12.item = enumerator2.Current;
								IntPtr method = ldftn(<getProductInfo>b__3);
								num *= 1212882152U;
								Action action2 = new Action(CS$<>8__locals12, method);
								num = 443315039U << (int)num;
								DispatchService.Dispatch(action2);
								num ^= 1819500339U;
							}
						}
					}
					while ((num & 1328692264U) == 0U);
				}
				finally
				{
					do
					{
						num = 1971214232U;
						if (num == 1075581360U)
						{
							break;
						}
						((IDisposable)enumerator2).Dispose();
					}
					while (num << 6 == 0U);
				}
				obj = vars.lock_data;
				bool flag15 = false;
				num = 707334855U;
				flag14 = flag15;
				try
				{
					num &= 1014892265U;
					if (1090146666U / num != 0U)
					{
						do
						{
							object obj4 = obj;
							num %= 1742430864U;
							Monitor.Enter(obj4, ref flag14);
							num = 192619990U / num;
						}
						while (470224314U == num);
					}
					flag13 = vars.clientMembershipData.TryGetValue(selected_server, out tuple);
				}
				finally
				{
					do
					{
						bool flag16 = flag14;
						num = 1938774086U;
						if (flag16)
						{
							num = 1874539738U + num;
							object obj5 = obj;
							num -= 549008814U;
							Monitor.Exit(obj5);
							num += 2969436372U;
						}
					}
					while (439699325U >= num);
				}
				for (;;)
				{
					num = 1817779266U;
					bool isCancellationRequested = cancelToken.IsCancellationRequested;
					num += 1288183581U;
					if (isCancellationRequested)
					{
						break;
					}
					bool flag17 = tuple != null;
					num /= 1805995914U;
					if (!flag17)
					{
						goto Block_43;
					}
					if (num < 2126790894U)
					{
						vars.ProductDateProperty = tuple.Item2;
						num = (1271286812U | num);
						bool flag18 = num - 1271286813U != 0U;
						num = 1947480020U << (int)num;
						KeyValuePair<bool, string> keyValuePair = this.checkServerPatchState(selected_server);
						num |= 826438704U;
						if ((num ^ 293995309U) == 0U)
						{
							break;
						}
						num -= 2120622321U;
						if (!string.IsNullOrEmpty(keyValuePair.Value))
						{
							if (num >= 28051728U)
							{
								string value = keyValuePair.Value;
								num >>= 27;
								vars.ProductStatusProperty = CLanguage.GetTranslateText(value);
								num <<= 15;
								flag18 = keyValuePair.Key;
								num %= 1106846779U;
								bool flag19 = flag3;
								num = 33690754U * num;
								if (flag19)
								{
									num += 1443110718U;
									if (691029347U >= num)
									{
										continue;
									}
									uint g_isHardVtSupport = vars.g_isHardVtSupport ? 1U : 0U;
									num = 1938033423U >> (int)num;
									vars.IsHardVtSupportErrorProperty = (g_isHardVtSupport == (num ^ 1U));
									if ((num ^ 1365987830U) == 0U)
									{
										continue;
									}
									bool isHardVtSupportErrorProperty = vars.IsHardVtSupportErrorProperty;
									num = 1743548779U * num;
									num ^= 1516925291U;
									if (!isHardVtSupportErrorProperty)
									{
										num = (1225926247U | num);
										bool g_isHardVtEnable = vars.g_isHardVtEnable;
										uint num3 = num ^ 2106992231U;
										num = 685996981U * num;
										vars.IsHardVtEnableErrorProperty = (g_isHardVtEnable == num3);
										num += 680811565U;
									}
								}
								bool isHardVtSupportErrorProperty2 = vars.IsHardVtSupportErrorProperty;
								num /= 365971664U;
								if (!isHardVtSupportErrorProperty2)
								{
									num <<= 22;
									if (1235898235U == num)
									{
										continue;
									}
									bool isHardVtEnableErrorProperty2 = vars.IsHardVtEnableErrorProperty;
									num -= 1378030641U;
									num += 1369642035U;
									if (!isHardVtEnableErrorProperty2)
									{
										bool isSystemSupportErrorProperty = (flag2 ? 1U : 0U) == (num ^ 2U);
										num = (2000634181U & num);
										vars.IsSystemSupportErrorProperty = isSystemSupportErrorProperty;
										num ^= 2U;
									}
								}
								num <<= 16;
								if (num != 49027303U)
								{
									bool isHardVtSupportErrorProperty3 = vars.IsHardVtSupportErrorProperty;
									num = 1311259602U >> (int)num;
									if (!isHardVtSupportErrorProperty3)
									{
										num = (651828598U & num);
										if (num > 1518286583U)
										{
											continue;
										}
										bool isHardVtEnableErrorProperty3 = vars.IsHardVtEnableErrorProperty;
										num = 1922316966U >> (int)num;
										num ^= 1311254391U;
										if (!isHardVtEnableErrorProperty3)
										{
											bool isSystemSupportErrorProperty2 = vars.IsSystemSupportErrorProperty;
											num += 0U;
											if (!isSystemSupportErrorProperty2)
											{
												bool flag20 = flag18;
												num += 0U;
												if (flag20)
												{
													if (1965299520U <= num)
													{
														break;
													}
													Tuple<eKeyStates, string, string> tuple2 = tuple;
													num = 1495551958U % num;
													bool item = tuple2.Item1 != eKeyStates.activeState;
													num %= 2023968716U;
													num += 1126967246U;
													if (!item)
													{
														bool isLaunchAccessProperty2 = (num ^ 1311259603U) != 0U;
														num = 74477015U << (int)num;
														vars.IsLaunchAccessProperty = isLaunchAccessProperty2;
														num = (1317478238U ^ num);
														if (num <= 1248006593U)
														{
															break;
														}
														num |= 1482913872U;
														vars.SelectedServerProperty = selected_server;
														num -= 1716806156U;
														bool isLaunchIndicatorVisible = (num ^ 2477202771U) != 0U;
														num *= 1505578930U;
														vars.IsLaunchIndicatorVisible = isLaunchIndicatorVisible;
														num ^= 769690326U;
													}
												}
											}
										}
									}
									if (num > 244656751U)
									{
										goto Block_65;
									}
								}
							}
						}
						else
						{
							num = (115876265U | num);
							if (1945204417U * num != 0U)
							{
								goto Block_48;
							}
						}
					}
				}
				return;
				Block_43:
				string message = "clientData is null";
				num = 837115702U - num;
				throw new Exception(message);
				Block_48:
				string message2 = "server state [name] is null";
				num <<= 10;
				throw new Exception(message2);
				Block_65:;
			}
			catch (Exception ex)
			{
				num = 1746479816U;
				Exception ex2 = ex;
				num = (111085524U & num);
				string format = "[HRESULT: 0x{0:X8} - {1}] {2} [Line : {3}]";
				num = 1739006507U % num;
				object[] array = new object[num + 4293411033U];
				num %= 1837595925U;
				int num4 = (int)(num - 1556267U);
				num = 302916745U >> (int)num;
				int hresult = ex2.HResult;
				num ^= 2126714745U;
				object obj6 = hresult;
				num ^= 806551774U;
				array[num4] = obj6;
				num -= 614154306U;
				int num5 = (int)(num ^ 708253216U);
				num = 341736776U << (int)num;
				MemberInfo type = ex2.GetType();
				num *= 1240538777U;
				object name2 = type.Name;
				num = 888352715U * num;
				array[num5] = name2;
				num = 1136936900U << (int)num;
				int num6 = (int)(num + 3090939906U);
				object message3 = ex2.Message;
				num *= 1835737368U;
				array[num6] = message3;
				num += 1980129193U;
				int num7 = (int)(num - 1952866214U);
				num -= 1028071661U;
				int num8 = ex2.LineNumber();
				num += 1336096075U;
				object obj7 = num8;
				num ^= 152961981U;
				array[num7] = obj7;
				num = 1518149062U % num;
				string str = string.Format(format, array);
				num += 117073979U;
				util.CreateLog(str);
				if (1484742385U != num)
				{
					do
					{
						object @object = CS$<>8__locals2;
						num = (442384369U | num);
						DispatchService.Dispatch(delegate
						{
							@object.<>4__this.PagesHostProduct.ClosePage();
							CustomMessageBox.Show(CLanguage.GetTranslateText("error_when_load_notes"), CLanguage.GetTranslateText("err_header_msg"), MessageBoxButton.OK, true);
						});
					}
					while (num >> 22 == 0U);
				}
				return;
			}
			object object2 = CS$<>8__locals2;
			num = 486020371U;
			IntPtr method2 = ldftn(<getProductInfo>b__0);
			num = 1806109382U % num;
			DispatchService.Dispatch(new Action(object2, method2));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00C9822C File Offset: 0x00C9822C
		public void ShowProduct(object param)
		{
			uint num = 1350576747U;
			num /= 179140555U;
			num *= 31883188U;
			MainWindow.<ShowProduct>d__38 <ShowProduct>d__;
			<ShowProduct>d__.<>4__this = this;
			for (;;)
			{
				num = (1180521682U | num);
				num -= 436168533U;
				<ShowProduct>d__.param = param;
				if (num != 1043535781U)
				{
					AsyncVoidMethodBuilder <>t__builder = AsyncVoidMethodBuilder.Create();
					num %= 1011773935U;
					<ShowProduct>d__.<>t__builder = <>t__builder;
					num = (723543237U ^ num);
					for (;;)
					{
						num = 1714357655U >> (int)num;
						<ShowProduct>d__.<>1__state = (int)(num + 4294548751U);
						num = 1884424611U + num;
						AsyncVoidMethodBuilder <>t__builder2 = <ShowProduct>d__.<>t__builder;
						num *= 1778263750U;
						AsyncVoidMethodBuilder asyncVoidMethodBuilder = <>t__builder2;
						if (num + 389293271U == 0U)
						{
							break;
						}
						num ^= 1663372992U;
						num = 1251289386U >> (int)num;
						asyncVoidMethodBuilder.Start<MainWindow.<ShowProduct>d__38>(ref <ShowProduct>d__);
						if (num >> 10 != 0U)
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000056A8 File Offset: 0x000056A8
		public void EnterProduct(object param)
		{
			Button button = param as Button;
			if (button.Content == null || button.Tag == null)
			{
				return;
			}
			TextBlock textBlock = (TextBlock)button.Content;
			nf_server key = (nf_server)button.Tag;
			bool flag = true;
			string str = string.Empty;
			string text = string.Empty;
			if (textBlock.Text.Contains("APEX", StringComparison.OrdinalIgnoreCase))
			{
				str = "APEX";
			}
			else if (textBlock.Text.Contains("2019"))
			{
				str = "MW";
			}
			else
			{
				str = (textBlock.Text.Split(new char[]
				{
					' '
				})[2] ?? "");
			}
			object lock_data = vars.lock_data;
			Tuple<eKeyStates, string, string> tuple;
			lock (lock_data)
			{
				flag = vars.clientMembershipData.TryGetValue(key, out tuple);
			}
			if (flag)
			{
				if (tuple.Item1 == eKeyStates.activeState)
				{
					text = str + " - " + tuple.Item2;
				}
				else
				{
					text = str + " - " + tuple.Item3;
				}
				eKeyStates item = tuple.Item1;
				if (item > eKeyStates.frozenState)
				{
					if (item == eKeyStates.inactiveState)
					{
						ButtonExtensions.SetDisableColor(button, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF504E5B")));
					}
				}
				else
				{
					ButtonExtensions.SetDisableColor(button, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF423D5F")));
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				textBlock.Text = text;
			}
			TranslateParams translateParams = new TranslateParams
			{
				Duration = 250.0,
				From = new Point(30.0, 0.0),
				To = new Point(0.0, 0.0),
				TransitionOn = 1,
				FillBehavior = FillBehavior.HoldEnd
			};
			Transitionz.SetTranslate(textBlock, translateParams);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000588C File Offset: 0x0000588C
		public void LeaveProduct(object param)
		{
			Button button = param as Button;
			if (button.Content == null)
			{
				return;
			}
			TextBlock textBlock = (TextBlock)button.Content;
			TranslateParams translateParams = new TranslateParams
			{
				Duration = 250.0,
				From = new Point(-30.0, 0.0),
				To = new Point(0.0, 0.0),
				TransitionOn = 1,
				FillBehavior = FillBehavior.HoldEnd
			};
			Transitionz.SetTranslate(textBlock, translateParams);
			((TextBlock)button.Content).Text = ButtonExtensions.GetOrigContent(textBlock);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005930 File Offset: 0x00005930
		public void DisplayWindow()
		{
			try
			{
				if (base.WindowState == WindowState.Minimized)
				{
					base.WindowState = WindowState.Normal;
				}
				base.Topmost = true;
				base.Show();
				base.Activate();
			}
			catch (InvalidOperationException)
			{
			}
			finally
			{
				try
				{
					base.Topmost = false;
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000599C File Offset: 0x0000599C
		private IntPtr ShowWindowRequestProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WinAPI.WmShowme)
			{
				this.DisplayWindow();
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000059B1 File Offset: 0x000059B1
		private static void ExitApp()
		{
			App.AppExit();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000059B8 File Offset: 0x000059B8
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/INFINITE;component/views/mainwindow/mainwindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000059E8 File Offset: 0x000059E8
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000059F4 File Offset: 0x000059F4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.XmainWindow = (MainWindow)target;
				this.XmainWindow.Loaded += this.Window_Loaded;
				this.XmainWindow.MouseLeave += this.XmainWindow_MouseLeave;
				return;
			case 2:
				this.mainGrid = (Grid)target;
				return;
			case 3:
				this.gridHeader = (Grid)target;
				this.gridHeader.MouseLeftButtonDown += this.GridHeader_MouseDown;
				this.gridHeader.PreviewMouseUp += this.GridHeader_PreviewMouseUp;
				this.gridHeader.MouseMove += this.GridHeader_MouseMove;
				this.gridHeader.MouseLeave += this.GridHeader_MouseLeave;
				return;
			case 4:
				this.exitButton = (Button)target;
				this.exitButton.Click += this.ExitButton_Click;
				return;
			case 5:
				this.minimizeButton = (Button)target;
				this.minimizeButton.Click += this.MinimizeButton_Click;
				return;
			case 6:
				this.viewGrid = (Grid)target;
				return;
			case 7:
				this.gridDownload = (Grid)target;
				return;
			case 8:
				this.cpb = (CircularProgressBar)target;
				return;
			case 9:
				this.LoadingAdorner = (AdornedControl)target;
				return;
			case 10:
				this.MainNewsControl = (RegionControl)target;
				return;
			case 11:
				this.PagesHostProduct = (PagesHost)target;
				return;
			case 12:
				this.ProductControl = (RegionControl)target;
				return;
			case 13:
				this.gridFooter = (Grid)target;
				return;
			case 14:
				this.MenuBarControl = (RegionControl)target;
				return;
			case 15:
				this.PagesHost = (PagesHost)target;
				return;
			case 16:
				this.HelpControl = (RegionControl)target;
				return;
			case 17:
				this.AboutControl = (RegionControl)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005C08 File Offset: 0x00005C08
		private void <downloadFile>b__23_0()
		{
			this.gridDownload.Visibility = Visibility.Visible;
			ServicePointManager.SecurityProtocol = (SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
			using (WebClient webClient = new WebClient())
			{
				webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
				webClient.DownloadFileCompleted += this.Completed;
				webClient.DownloadProgressChanged += this.ProgressChanged;
				webClient.DownloadFileAsync(new Uri(vars.hURIDownload), vars.appNewFileName);
			}
		}

		// Token: 0x0400006C RID: 108
		private Timer m_toolTipClosingTimer;

		// Token: 0x0400006D RID: 109
		private bool clicked;

		// Token: 0x0400006E RID: 110
		private bool leaved;

		// Token: 0x0400006F RID: 111
		private Storyboard HeaderStoryBoardDown;

		// Token: 0x04000070 RID: 112
		private Storyboard HeaderStoryBoardUp;

		// Token: 0x04000071 RID: 113
		public static App app;

		// Token: 0x04000072 RID: 114
		public static MainWindow mainWindow;

		// Token: 0x04000073 RID: 115
		private static bool bMainThEnd;

		// Token: 0x04000074 RID: 116
		private CancellationTokenSource cancelTokenSource;

		// Token: 0x04000075 RID: 117
		internal MainWindow XmainWindow;

		// Token: 0x04000076 RID: 118
		internal Grid mainGrid;

		// Token: 0x04000077 RID: 119
		internal Grid gridHeader;

		// Token: 0x04000078 RID: 120
		internal Button exitButton;

		// Token: 0x04000079 RID: 121
		internal Button minimizeButton;

		// Token: 0x0400007A RID: 122
		internal Grid viewGrid;

		// Token: 0x0400007B RID: 123
		internal Grid gridDownload;

		// Token: 0x0400007C RID: 124
		internal CircularProgressBar cpb;

		// Token: 0x0400007D RID: 125
		internal AdornedControl LoadingAdorner;

		// Token: 0x0400007E RID: 126
		internal RegionControl MainNewsControl;

		// Token: 0x0400007F RID: 127
		internal PagesHost PagesHostProduct;

		// Token: 0x04000080 RID: 128
		internal RegionControl ProductControl;

		// Token: 0x04000081 RID: 129
		internal Grid gridFooter;

		// Token: 0x04000082 RID: 130
		internal RegionControl MenuBarControl;

		// Token: 0x04000083 RID: 131
		internal PagesHost PagesHost;

		// Token: 0x04000084 RID: 132
		internal RegionControl HelpControl;

		// Token: 0x04000085 RID: 133
		internal RegionControl AboutControl;

		// Token: 0x04000086 RID: 134
		private bool _contentLoaded;
	}
}
