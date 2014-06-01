// <copyright file="XmlHelper.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace LlConsole
{
        using System;
        using System.Xml;

        /// <summary>
        /// Xml helper class, containing easier input routines.
        /// </summary>
        public static class XmlHelper
        {
                /// <summary>
                /// Read a value from XML if the value exists.
                /// </summary>
                /// <param name="node">The XML node.</param>
                /// <param name="attribute">The Attribute of interest.</param>
                /// <param name="target">Default value.</param>
                /// <typeparam name="T">The type of value and target's type.</typeparam>
                /// <returns>The found value or default</returns>
                public static T FromXmlIfExists<T>(
                        XmlNode node,
                        string attribute,
                        T target)
                {
                        T value;
                        XmlAttribute attr;

                        if (node == null)
                        {
                                return target;
                        }

                        attr = node.Attributes[attribute];
                        if (attr == null)
                        {
                                return target;
                        }

                        try
                        {
                                string xmlValue = attr.InnerText.Trim();
                                value = (T)Convert.ChangeType(
                                        xmlValue,
                                        Convert.GetTypeCode(target),
                                        System.Globalization.CultureInfo.CurrentCulture);
                        }
                        catch (XmlException)
                        {
                                return target;
                        }

                        return value;
                }
        }
}
