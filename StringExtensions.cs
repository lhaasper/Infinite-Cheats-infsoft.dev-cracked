using System;

namespace Launcher
{
	// Token: 0x02000009 RID: 9
	public static class StringExtensions
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002454 File Offset: 0x00002454
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source != null && source.IndexOf(toCheck, comp) >= 0;
		}
	}
}
