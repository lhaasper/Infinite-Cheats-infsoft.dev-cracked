using System;
using System.Windows;

namespace Launcher
{
	// Token: 0x0200000C RID: 12
	public class StyleTriggersInheritanceHelper
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0000252C File Offset: 0x0000252C
		private static void OnAddInheritedTriggers(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			bool? flag = e.NewValue as bool?;
			if (flag != null && flag.Value)
			{
				FrameworkElement frameworkElement = sender as FrameworkElement;
				if (frameworkElement != null)
				{
					Style style = frameworkElement.Style;
					if (style != null)
					{
						Style basedOn = frameworkElement.Style.BasedOn;
						Style style2 = new Style
						{
							BasedOn = style
						};
						if (basedOn != null)
						{
							foreach (TriggerBase item in style.Triggers)
							{
								style2.Triggers.Add(item);
							}
							foreach (TriggerBase item2 in basedOn.Triggers)
							{
								style2.Triggers.Add(item2);
							}
						}
						frameworkElement.Style = style2;
					}
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002638 File Offset: 0x00002638
		public static bool GetAddInheritedTriggers(DependencyObject obj)
		{
			return (bool)obj.GetValue(StyleTriggersInheritanceHelper.AddInheritedTriggersProperty);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000264A File Offset: 0x0000264A
		public static void SetAddInheritedTriggers(DependencyObject obj, bool value)
		{
			obj.SetValue(StyleTriggersInheritanceHelper.AddInheritedTriggersProperty, value);
		}

		// Token: 0x0400000B RID: 11
		public static readonly DependencyProperty AddInheritedTriggersProperty = DependencyProperty.RegisterAttached("AddInheritedTriggers", typeof(bool), typeof(FrameworkElement), new FrameworkPropertyMetadata(new PropertyChangedCallback(StyleTriggersInheritanceHelper.OnAddInheritedTriggers)));
	}
}
