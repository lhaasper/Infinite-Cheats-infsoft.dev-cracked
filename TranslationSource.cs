using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using Launcher.Language.Resources;

namespace Launcher
{
	// Token: 0x02000005 RID: 5
	public class TranslationSource : INotifyPropertyChanged
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021A1 File Offset: 0x000021A1
		public static TranslationSource Instance { get; } = new TranslationSource();

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021A8 File Offset: 0x000021A8
		public string Item
		{
			get
			{
				return this.resManager.GetString(key, this.currentCulture);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000021BC File Offset: 0x000021BC
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000021C4 File Offset: 0x000021C4
		public ResourceManager ResManager
		{
			get
			{
				return this.resManager;
			}
			set
			{
				if (this.resManager != value)
				{
					this.resManager = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs(string.Empty));
					}
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021FC File Offset: 0x000021FC
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002204 File Offset: 0x00002204
		public CultureInfo CurrentCulture
		{
			get
			{
				return this.currentCulture;
			}
			set
			{
				if (this.currentCulture != value)
				{
					this.currentCulture = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs(string.Empty));
					}
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000010 RID: 16 RVA: 0x0000223C File Offset: 0x0000223C
		// (remove) Token: 0x06000011 RID: 17 RVA: 0x00002274 File Offset: 0x00002274
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04000003 RID: 3
		private ResourceManager resManager = ru_RU.ResourceManager;

		// Token: 0x04000004 RID: 4
		private CultureInfo currentCulture;
	}
}
