// <copyright file="XmlHelper.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LL_Console
{
    using System;
    using System.Xml;

    public class XmlHelper
    {
        private XmlHelper ()
        {
        }

        public static void BoolFromXmlIfExists(
            XmlNode node,
            string attribute,
            ref bool target)
        {
            bool value;
            XmlAttribute attr = node.Attributes[attribute];
            if (attr == null)
            {
                return;
            }

            try
            {
                value = attr.InnerText.ToLower().Trim() == "true";
            }
            catch
            {
                return;
            }

            target = value;
        }

        public static void IntFromXmlIfExists(
            XmlNode node,
            string attribute,
            ref int target)
        {
            int value;
            XmlAttribute attr = node.Attributes[attribute];
            if (attr == null)
            {
                return;
            }

            try
            {
                value = int.Parse(attr.InnerText);
            }
            catch
            {
                return;
            }

            target = value;
        }

        public static void StringFromXmlIfExists(
            XmlNode node,
            string attribute,
            ref string target)
        {
            string value;
            XmlAttribute attr = node.Attributes[attribute];
            if (attr == null)
            {
                return;
            }

            try
            {
                value = attr.InnerText;
            }
            catch
            {
                return;
            }

            target = value;
        }
    }
}
