using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Launcher
{
	// Token: 0x02000008 RID: 8
	public static class FrameworkElementExt
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000023C8 File Offset: 0x000023C8
		public static void BringToFront(this FrameworkElement element)
		{
			if (element == null)
			{
				return;
			}
			Panel panel = element.Parent as Panel;
			if (panel == null)
			{
				return;
			}
			int num = (from x in panel.Children.OfType<UIElement>()
			where x != element
			select Panel.GetZIndex(x)).Max();
			Panel.SetZIndex(element, num + 1);
		}
	}
}
