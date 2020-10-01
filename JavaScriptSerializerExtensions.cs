using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Web.Script.Serialization;

namespace Launcher
{
	// Token: 0x02000006 RID: 6
	internal static class JavaScriptSerializerExtensions
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000022C8 File Offset: 0x000022C8
		[return: Dynamic]
		public static dynamic DeserializeDynamic(this JavaScriptSerializer serializer, string value)
		{
			return JavaScriptSerializerExtensions.GetExpando(serializer.Deserialize<IDictionary<string, object>>(value));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022D8 File Offset: 0x000022D8
		private static ExpandoObject GetExpando(IDictionary<string, object> dictionary)
		{
			IDictionary<string, object> dictionary2 = new ExpandoObject();
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				IDictionary<string, object> dictionary3 = keyValuePair.Value as IDictionary<string, object>;
				if (dictionary3 != null)
				{
					dictionary2.Add(keyValuePair.Key, JavaScriptSerializerExtensions.GetExpando(dictionary3));
				}
				else
				{
					dictionary2.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return (ExpandoObject)dictionary2;
		}
	}
}
