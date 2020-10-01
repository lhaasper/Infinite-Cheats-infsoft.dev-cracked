using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Launcher
{
	// Token: 0x0200000B RID: 11
	[MarkupExtensionReturnType(typeof(BindingExpression))]
	public class ApplyBindingDefinition : MarkupExtension
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000024B5 File Offset: 0x000024B5
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000024BD File Offset: 0x000024BD
		public BindingDefinition Definition { get; set; }

		// Token: 0x06000025 RID: 37 RVA: 0x000024C8 File Offset: 0x000024C8
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new Binding
			{
				Source = this.Definition.Source,
				Mode = this.Definition.Mode,
				Converter = this.Definition.Converter,
				Path = this.Definition.Path
			}.ProvideValue(serviceProvider);
		}
	}
}
