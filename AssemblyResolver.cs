using System;
using System.Reflection;
using Launcher.Properties;

namespace Launcher
{
	// Token: 0x02000002 RID: 2
	internal static class AssemblyResolver
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002048 File Offset: 0x00002048
		public static Assembly Resolve(object sender, ResolveEventArgs args)
		{
			if (new AssemblyName(args.Name).Name.StartsWith("SciChart"))
			{
				return Assembly.Load(Resources.SciChart);
			}
			return null;
		}
	}
}
