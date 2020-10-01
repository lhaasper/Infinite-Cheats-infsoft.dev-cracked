using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace Launcher
{
	// Token: 0x0200000A RID: 10
	public class BindingDefinition
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002469 File Offset: 0x00002469
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002471 File Offset: 0x00002471
		public PropertyPath Path
		{
			[CompilerGenerated]
			get
			{
				return this.<Path>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Path>k__BackingField = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000247A File Offset: 0x0000247A
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002482 File Offset: 0x00002482
		public object Source
		{
			[CompilerGenerated]
			get
			{
				return this.<Source>k__BackingField;
			}
			set
			{
				this.<Source>k__BackingField = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000248B File Offset: 0x0000248B
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002493 File Offset: 0x00002493
		public IValueConverter Converter
		{
			[CompilerGenerated]
			get
			{
				return this.<Converter>k__BackingField;
			}
			set
			{
				this.<Converter>k__BackingField = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000249C File Offset: 0x0000249C
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000024A4 File Offset: 0x000024A4
		public BindingMode Mode
		{
			[CompilerGenerated]
			get
			{
				return this.<Mode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Mode>k__BackingField = value;
			}
		}

		// Token: 0x04000006 RID: 6
		private PropertyPath <Path>k__BackingField;

		// Token: 0x04000009 RID: 9
		private BindingMode <Mode>k__BackingField;
	}
}
