using System;
using System.Xml;

namespace LL_Console
{
	public class XmlHelper
	{
		private XmlHelper ()
		{
		}

		public static void IntFromXmlIfExists (XmlNode node, string attribute,
		                                       ref int target) {
			int value;
			XmlAttribute attr = node.Attributes [attribute];
			if (attr == null) {
				return;
			}
			try {
				value = int.Parse (attr.InnerText);
			} catch {
				return;
			}
			target = value;
		}

		public static void StringFromXmlIfExists (XmlNode node, string attribute,
		                                          ref string target) {
			string value;
			XmlAttribute attr = node.Attributes [attribute];
			if (attr == null) {
				return;
			}
			try {
				value = attr.InnerText;
			} catch {
				return;
			}
			target = value;
		}
	}
}
