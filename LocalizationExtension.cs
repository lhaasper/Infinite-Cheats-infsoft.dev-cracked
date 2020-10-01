using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Launcher
{
	// Token: 0x02000004 RID: 4
	public class LocalizationExtension : MarkupExtension
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002122 File Offset: 0x00002122
		public string StringName { get; }

		// Token: 0x06000008 RID: 8 RVA: 0x0000212A File Offset: 0x0000212A
		public LocalizationExtension(string name)
		{
			this.StringName = name;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000213C File Offset: 0x0000213C
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new Binding
			{
				Mode = BindingMode.OneWay,
				Path = new PropertyPath("[" + this.StringName + "]", Array.Empty<object>()),
				Source = TranslationSource.Instance,
				FallbackValue = TranslationSource.Instance.get_Item(this.StringName)
			}.ProvideValue(serviceProvider);
		}
	}
}
