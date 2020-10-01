using System;
using System.Collections.Generic;

namespace Launcher
{
	// Token: 0x0200000D RID: 13
	public static class DictionaryExtensions
	{
		// Token: 0x0600002C RID: 44 RVA: 0x0000269B File Offset: 0x0000269B
		public static void SafeAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			dict[key] = value;
		}
	}
}
