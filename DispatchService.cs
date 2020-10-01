using System;
using System.Windows;
using System.Windows.Threading;

namespace Launcher
{
	// Token: 0x02000007 RID: 7
	public static class DispatchService
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002360 File Offset: 0x00002360
		public static void Dispatch(Action action)
		{
			Dispatcher dispatcher = Application.Current.Dispatcher;
			if (dispatcher == null || dispatcher.CheckAccess())
			{
				action();
				return;
			}
			dispatcher.Invoke(action);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002394 File Offset: 0x00002394
		public static void DispatchAsync(Action action)
		{
			Dispatcher dispatcher = Application.Current.Dispatcher;
			if (dispatcher == null || dispatcher.CheckAccess())
			{
				action();
				return;
			}
			dispatcher.InvokeAsync(action);
		}
	}
}
