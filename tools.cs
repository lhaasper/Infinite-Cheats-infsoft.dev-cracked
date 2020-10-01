using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Launcher
{
	// Token: 0x02000003 RID: 3
	public static class tools
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002074 File Offset: 0x00002074
		public static Task WaitOneAsync(WaitHandle waitHandle)
		{
			if (waitHandle == null)
			{
				throw new ArgumentNullException("waitHandle");
			}
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			RegisteredWaitHandle rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle, delegate(object <p0>, bool <p1>)
			{
				tcs.TrySetResult(true);
			}, null, -1, true);
			Task<bool> task = tcs.Task;
			task.ContinueWith<bool>((Task<bool> antecedent) => rwh.Unregister(null));
			return task;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020DC File Offset: 0x000020DC
		public static void Swap<T>(ref T lhs, ref T rhs)
		{
			T t = lhs;
			lhs = rhs;
			rhs = t;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002103 File Offset: 0x00002103
		public static T Swap<T>(this T x, ref T y)
		{
			T result = y;
			y = x;
			return result;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002112 File Offset: 0x00002112
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				int num;
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i = num + 1)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if (child != null && child is T)
					{
						yield return (T)((object)child);
					}
					foreach (T t in tools.FindVisualChildren<T>(child))
					{
						yield return t;
					}
					IEnumerator<T> enumerator = null;
					child = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x02000072 RID: 114
		internal static class ResourceAccessor
		{
			// Token: 0x0600041D RID: 1053 RVA: 0x0000E648 File Offset: 0x0000E648
			public static Uri Get(string resourcePath)
			{
				return new Uri(string.Format("pack://application:,,,/{0};component/{1}", Assembly.GetExecutingAssembly().GetName().Name, resourcePath));
			}
		}
	}
}
