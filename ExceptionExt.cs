using System;
using System.Collections;

namespace Launcher
{
	// Token: 0x0200000E RID: 14
	public static class ExceptionExt
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000026A8 File Offset: 0x000026A8
		public static IEnumerable GetDataRecursive(this Exception exception)
		{
			Stack stack = new Stack(exception.Data.Values);
			for (Exception innerException = exception.InnerException; innerException != null; innerException = innerException.InnerException)
			{
				if (innerException.Data != null)
				{
					foreach (object obj in innerException.Data.Values)
					{
						stack.Push(obj);
					}
				}
			}
			return stack;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002734 File Offset: 0x00002734
		public static bool IsSourceOfType<T>(this Exception exception)
		{
			Exception ex = exception;
			for (;;)
			{
				bool flag = ex != null && ex.InnerException != null;
				if (!flag)
				{
					break;
				}
				ex = ex.InnerException;
			}
			return ex is T;
		}
	}
}
