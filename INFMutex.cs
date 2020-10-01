using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Launcher.Dependencies;
using Launcher.Dependencies.API;

namespace Launcher
{
	// Token: 0x02000018 RID: 24
	public class INFMutex : appMutex
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00002BAC File Offset: 0x00002BAC
		static INFMutex()
		{
			GuidAttribute guidAttribute = typeof(App).Assembly.GetCustomAttributes(typeof(GuidAttribute), true).OfType<GuidAttribute>().FirstOrDefault<GuidAttribute>();
			object obj;
			if (guidAttribute != null)
			{
				obj = guidAttribute.Value;
			}
			else
			{
				obj = "inf_inf";
			}
			INFMutex.mutexId = (string)obj;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002C00 File Offset: 0x00002C00
		internal INFMutex(Func<bool> initializeProgram) : base(INFMutex.mutexId)
		{
			this.initializeProgram = initializeProgram;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002C14 File Offset: 0x00002C14
		protected override bool InitializeProgram()
		{
			return this.initializeProgram();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002C21 File Offset: 0x00002C21
		protected override void OnMutexFound()
		{
			WinAPI.ShowApp();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002C28 File Offset: 0x00002C28
		protected override void Shutdown()
		{
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x04000066 RID: 102
		private static readonly string mutexId;

		// Token: 0x04000067 RID: 103
		private readonly Func<bool> initializeProgram;
	}
}
